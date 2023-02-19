using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action OnJumpEvent;
    public IInteractive CurrentInteractiveObj { get; private set; }
    [field: SerializeField] public float Speed { get; private set; } = 1f;
    [field: SerializeField] public float ImpulsRopeUp { get; set; } = 3f;
    [field: SerializeField] private float ImpulsRopeUpSlowdown { get; set; } = 0.03f;
    [field: SerializeField] public float JumpForce { get; set; } = 1f;
    [field: SerializeField] private float pullForce { get; set; } = 1f;
    [field: SerializeField] private Vector3 normal { get; set; }
    [SerializeField] private LayerMask groudLayerMask;
    
    [SerializeField] private float wallCheckDistance = 0.9f;
    
    //[SerializeField] private ParticleSystem dustParticle;

    #region Ledge Settings
    [Header("--------Ledge settings--------")]
    [SerializeField] private Transform ledgeClimbCheckOrigin;
    [SerializeField] private Transform ledgeClimbPosition;
    [SerializeField] private Vector2 ledgePosition1Offset;
    [SerializeField] private Vector2 ledgePosition2Offset;
    
    private bool isTouchingLedge;
    private bool isLedgeDetected;
    public ObservableVariable<bool> canClimbLedge { get; private set; }
    
    private Vector3 ledgePositionBottom;
    private Vector3 ledgePosition1;
    private Vector3 ledgePosition2;
    #endregion


    private Player player;
    private bool isFacingRight;

    private float impulsRopeUp;

    private float capsuleHeight;
    private float capsuleRadius;
    private float capsuleYOffset;
    private float capsuleYOffsetConstant = 0.81f;

    private Transform rope;
    private List<Transform> ropeKnots;
    private Transform currentRopeKnot;
    private bool isOnRope;

    private PhysicMaterial physicMaterial;
    private float dynamicFriction;
    private float staticFriction;
    [SerializeField] private float maxFriction, frictionIncrement;

    [SerializeField] private Vector3 offsetCenter;
    [SerializeField] private Vector3 offsetBounth;

    private CapsuleCollider collider;
    private Rigidbody rigidbody;
    private float speed;


    private bool canInteractiveObject;
    private bool isRope;
    private bool isClimb;

    public ObservableVariable<bool> IsWall { get; private set; }
    public ObservableVariable<bool> IsDead { get; private set; }
    public ObservableVariable<bool> IsPull { get; private set; }
    public ObservableVariable<bool> IsPush { get; private set; }
    public ObservableVariable<bool> IsClimb { get; private set; }
    public ObservableVariable<bool> IsSlide { get; private set; }
    public ObservableVariable<bool> IsMoving { get; private set; }
    public ObservableVariable<bool> IsGrounded { get; private set; }
    public ObservableVariable<bool> IsSquatDown { get; private set; }
    public ObservableVariable<bool> IsRopeTrigger { get; private set; }
    public ObservableVariable<bool> IsBrakePickax { get; private set; }
    public ObservableVariable<bool> IsInteractiveObject { get; private set; }

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();

        IsGrounded = new ObservableVariable<bool>();
        IsMoving = new ObservableVariable<bool>();
        IsSquatDown = new ObservableVariable<bool>();
        IsInteractiveObject = new ObservableVariable<bool>();
        IsSlide = new ObservableVariable<bool>();
        IsRopeTrigger = new ObservableVariable<bool>();
        IsWall = new ObservableVariable<bool>();
        IsPull = new ObservableVariable<bool>();
        IsPush = new ObservableVariable<bool>();
        IsDead = new ObservableVariable<bool>();
        IsBrakePickax = new ObservableVariable<bool>();
        canClimbLedge = new ObservableVariable<bool>();

        IsClimb = new ObservableVariable<bool>();

        physicMaterial = collider.material;
        dynamicFriction = physicMaterial.dynamicFriction;

        impulsRopeUp = ImpulsRopeUp;

        capsuleHeight = collider.height;
        capsuleRadius = collider.radius;
        capsuleYOffset = collider.center.y;

        player = GetComponent<Player>();

        Subscribe();
    }

    private void Update() {
        if (!isOnRope)
        {
            if (canClimbLedge.Value)
            {
                rigidbody.useGravity = false;
                rigidbody.velocity = Vector3.zero;
                return;
            }
            else
            {
                rigidbody.useGravity = true;
            } 
        }
        
        IsGrounded.Value = CheckGround();
        IsWall.Value = CheckWall();

        if (!IsGrounded.Value)
        {
            CheckLedgeClimb();
            
        }


        if (IsRopeTrigger.Value) {
            var distance = Vector3.Distance(transform.position, currentRopeKnot.position);
            if (distance > 0.2) {
                transform.position = Vector3.Lerp(
                    transform.position,
                    new Vector3(currentRopeKnot.position.x, currentRopeKnot.position.y, 0),
                    Time.deltaTime * ImpulsRopeUp);

                ImpulsRopeUp -= ImpulsRopeUpSlowdown;
            }
            else {
                currentRopeKnot = GetNextKnot(ropeKnots, currentRopeKnot);
            }
        }
        else { ImpulsRopeUp = impulsRopeUp; }

        if (IsBrakePickax.Value) {
            RaiseFriction(physicMaterial, maxFriction, frictionIncrement);
            rigidbody.AddForce(transform.forward.z * speed, 0, 0);
        }
        
        //if(IsMoving.Value && IsGrounded.Value) {
        //    if (dustParticle.isStopped) dustParticle.Play();
        //}
        //else {
        //    if (dustParticle.isPlaying) dustParticle.Stop();
        //}

        if (rigidbody.useGravity != true) return;

        if (rigidbody.velocity.y < -0.5 && IsGrounded.Value && !IsMoving.Value) IsSlide.Value = true;
        else IsSlide.Value = false;
    }

    private void Subscribe()
    {
        IsBrakePickax.OnValueChangeEvent += IsBrakePickaxOnValueChangeHandler;
        IsRopeTrigger.OnValueChangeEvent += IsRopeTriggerOnValueChangeHandler;
    }
    private void Unsubscribe()
    {
        IsBrakePickax.OnValueChangeEvent -= IsBrakePickaxOnValueChangeHandler;
        IsRopeTrigger.OnValueChangeEvent -= IsRopeTriggerOnValueChangeHandler;
    }

    public void IsUpButtonDown(bool button) {
        if (button && isRope) IsRopeTrigger.Value = true;
        else IsRopeTrigger.Value = false;
    }
    public void IsInteractiveButton(bool button) {
        if (button && canInteractiveObject) return;

        if (button && IsInteractiveObject.Value) canInteractiveObject = true;
        else canInteractiveObject = false;
    }



    #region Move
    public void Move(Vector2 vectorMove) {
        if (vectorMove.x != 0)
        {
            IsMoving.Value = true;
        }
        else
        {
            IsMoving.Value = false;
        }

        CheckBreakByPickax(vectorMove);
        
        if (CurrentInteractiveObj is ICapableMoving capable && vectorMove != Vector2.zero && canInteractiveObject) {
            MoveObject(capable, vectorMove);
            return;
        }
        else {
            IsPull.Value = false;
            IsPush.Value = false;
        }
        
        PlayerRotation(vectorMove);
        float horizontal = vectorMove.x * speed * normal.y;
        rigidbody.velocity = new Vector3(horizontal, rigidbody.velocity.y, 0);
    }
    public void Jump() {
        if (IsGrounded.Value) {
            OnJumpEvent.Invoke();
        }
    }
    private void PlayerRotation(Vector2 vector) {
        if (IsSlide.Value) {
            transform.rotation = Quaternion.identity;
            return;
        }

        if (IsPull.Value) return;

        if (vector.x > 0){
            isFacingRight = true;
            transform.rotation = Quaternion.identity;
        } 
        else if (vector.x < 0)
        {
            isFacingRight = false;
            transform.rotation = Quaternion.Euler(new Vector2(0, -180));
        }
    }

    #endregion

    #region SquatDown
    public void SquatDown(bool isButtonPressed) {
        if (isButtonPressed && !IsSquatDown.Value) Squat();
        else if (!isButtonPressed && IsSquatDown.Value && !CheckCeiling()) {
            Stand();
        }
    }
    private void Stand()
    {
        collider.height = capsuleHeight;
        collider.radius = capsuleRadius;
        collider.center = new Vector3(collider.center.x, capsuleYOffset, collider.center.z);

        speed = Speed;

        IsSquatDown.Value = false;
    }
    private void Squat()
    {
        collider.height = capsuleHeight / 2;
        collider.radius = capsuleRadius / 2;
        collider.center = new Vector3(collider.center.x, capsuleYOffset - capsuleYOffsetConstant, collider.center.z);
        speed = Speed / 3;

        IsSquatDown.Value = true;
    }

    #endregion

    #region Pull/Push
    public void CollisionInteractiveObject(bool isCollision, IInteractive interactiveGo) {
        rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0);

        if (canInteractiveObject) return;
        IsInteractiveObject.Value = isCollision;
        CurrentInteractiveObj = interactiveGo;
    }

    private void MoveObject(ICapableMoving capableMoving, Vector2 vectorMove) {
        var normalDirectionMove = vectorMove.normalized;
        var normalTransformDirection = new Vector2(transform.forward.z, 0).normalized;

        if (normalDirectionMove == normalTransformDirection) {
            IsPush.Value= true;
        }
        else {
            IsPull.Value = true;
        }
    }

    #endregion

    #region Climb
    public void IsClimbTrigger(bool value) =>
        isClimb = value;

    #endregion

    #region Rope
    public void SetRope(Transform rope) {
        this.rope = rope;
        ropeKnots = rope.GetComponentsInChildren<Transform>().ToList();
    }
    public void IsRope(bool rope) =>
        isRope = rope;
    private void IsRopeTriggerOnValueChangeHandler(bool value) {
        if (value == true) {
            isOnRope = true;
            rigidbody.useGravity = false;
            rigidbody.velocity = Vector3.zero;
            currentRopeKnot = GetNearlestKnotTransform(ropeKnots);
        }
        if (value == false) {
            isOnRope = false;
            currentRopeKnot = null;
            rigidbody.useGravity = true;
        }
    }
    private Transform GetNearlestKnotTransform(List<Transform> transformKnotList) {
        List<float> distances = new();
        foreach (Transform t in transformKnotList)
            distances.Add(Vector3.Distance(t.position, transform.position));
        float kk = distances.Min();
        int result = distances.IndexOf(kk);
        return transformKnotList[result];
    }
    private Transform GetNextKnot(List<Transform> transformKnotList, Transform currentKnotTransform)
    {
        int index = transformKnotList.IndexOf(currentKnotTransform);
        if (index == 0) return currentKnotTransform;
        return transformKnotList[index - 1];
    }
    #endregion

    #region Cheking
    private bool CheckGround() {
        var bottomCenterPoint = new Vector3(collider.bounds.center.x, collider.bounds.min.y, collider.bounds.center.z);
        return Physics.CheckCapsule(collider.bounds.center, bottomCenterPoint, collider.bounds.size.x / 2 * 0.9f, groudLayerMask);
    }

    private bool CheckCeiling() {
        var center = new Vector3(transform.position.x, transform.position.y + offsetCenter.y, transform.position.z);
        var half = new Vector3(offsetBounth.x, offsetBounth.y, offsetBounth.z);


        return Physics.CheckBox(
            center,
            half,
            Quaternion.identity,
            groudLayerMask
            );
    }

    private bool CheckWall(Vector3 origin)
    {
        Ray ray = new Ray(
            origin,
            new Vector3(transform.forward.z, 0, 0)
        );

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, wallCheckDistance, groudLayerMask)) {
            if (hit.distance < wallCheckDistance)
                if (Mathf.Abs(hit.normal.x) == 1)
                    return true; 
        }

        return false;
    }

    private bool CheckWall()
    {
        return CheckWall(new Vector3(transform.position.x,
            transform.position.y + (collider.height - collider.radius) * transform.localScale.y, transform.position.z));
    }

    private void CheckLedgeClimb()
    {
        isTouchingLedge = CheckWall(ledgeClimbCheckOrigin.position);
        if (IsWall.Value && !isTouchingLedge && !isLedgeDetected)
        {
            isLedgeDetected = true;
            ledgePositionBottom = ledgeClimbPosition.position;
        }
        
        if (isLedgeDetected && !canClimbLedge.Value)
        {
            canClimbLedge.Value = true;

            if (isFacingRight)
            {
                ledgePosition1 =
                    new Vector3(ledgePositionBottom.x + wallCheckDistance - ledgePosition1Offset.x,
                        ledgePositionBottom.y + ledgePosition1Offset.y, ledgePositionBottom.z);
                ledgePosition2 =
                    new Vector3(ledgePositionBottom.x + wallCheckDistance + ledgePosition2Offset.x,
                        ledgePositionBottom.y + ledgePosition2Offset.y, ledgePositionBottom.z);
            }
            else
            {
                ledgePosition1 =
                    new Vector3(ledgePositionBottom.x - wallCheckDistance + ledgePosition1Offset.x,
                        ledgePositionBottom.y + ledgePosition1Offset.y, ledgePositionBottom.z);
                ledgePosition2 =
                    new Vector3(ledgePositionBottom.x - wallCheckDistance - ledgePosition2Offset.x,
                        ledgePositionBottom.y + ledgePosition2Offset.y, ledgePositionBottom.z);
            }
        }

        if (canClimbLedge.Value)
        {
            transform.position = ledgePosition1;
        }
    }
    #endregion

    #region BrakeByPickax
    private void CheckBreakByPickax(Vector2 move)
    {
        if (Player.isPickax == false) return;

        var transformDirection = transform.forward.z;
        var moveNornalize = move.normalized.x;

        if (IsWall.Value && (transformDirection == moveNornalize) && !IsGrounded.Value)
            IsBrakePickax.Value = true;
        else
            IsBrakePickax.Value = false;
    }

    private void RaiseFriction(PhysicMaterial physicMaterial, float maxFriction, float increment) {
        physicMaterial.dynamicFriction = Mathf.Lerp(physicMaterial.dynamicFriction, maxFriction, increment * Time.deltaTime);
        Debug.Log(physicMaterial.dynamicFriction);
    }
    private void IsBrakePickaxOnValueChangeHandler(bool newValue) {
        if (newValue == false) ResetMaterial();
    }
    private void ResetMaterial() {
        physicMaterial.dynamicFriction = dynamicFriction;
    }
    #endregion

    #region Dead
    public void Dead() {
        IsDead.Value = true;
        Unsubscribe();
        rigidbody.velocity = Vector3.zero;
    }
    public void UncheckDeadFlag() {
        IsDead.Value = false;
    }

    #endregion

    public void SetNormal(Vector3 normal) {
        this.normal = normal;
    }   

    // Animation triggers
    public void FinishLedgeClimb() {
        canClimbLedge.Value = false;
        isLedgeDetected = false;
        isTouchingLedge = false;
        transform.position = ledgePosition2;
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawRay(ledgeClimbCheckOrigin.position, new Vector3(transform.forward.z, 0, 0) * wallCheckDistance);
    //     Gizmos.DrawRay(new Vector3(transform.position.x,
    //         transform.position.y + (collider.height - collider.radius) * transform.localScale.y, transform.position.z), new Vector3(transform.forward.z, 0, 0) * wallCheckDistance);
    //     if(ledgePosition1 != Vector3.zero)
    //         Gizmos.color = Color.green;
    //         Gizmos.DrawSphere(ledgePosition1, 0.1f);
    //     
    //     if(ledgePosition2 != Vector3.zero)
    //         Gizmos.color = Color.blue;
    //         Gizmos.DrawSphere(ledgePosition2, 0.1f);
    //     
    // }
}
