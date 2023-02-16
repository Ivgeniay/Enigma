using Box;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action OnJumpEvent;
    [field: SerializeField] public float Speed { get; private set; } = 1f;
    [field: SerializeField] public float ImpulsRopeUp { get; set; } = 3f;
    [field: SerializeField] private float ImpulsRopeUpSlowdown { get; set; } = 0.03f;
    [field: SerializeField] public float JumpForce { get; set; } = 1f;
    [field: SerializeField] private float pullForce { get; set; } = 1f;
    [field: SerializeField] private float deathImpulse { get; set; } = 6f;

    [SerializeField] private LayerMask groudLayerMask;

    [SerializeField] private Transform rope;

    private float capsuleHeight;
    private float capsuleRadius;
    private float capsuleYOffset;
    private float capsuleYOffsetConstant = 0.81f;

    private List<Transform> ropeKnots;
    private Transform currentRopeKnot;

    private CapsuleCollider collider;
    private Rigidbody rigidbody;
    private float speed;

    private IInteractive currentInteractiveObj;

    private bool isRope;
    private bool isClimb;

    public ObservableVariable<bool> IsWall { get; private set; }
    public ObservableVariable<bool> IsDead { get; private set; }
    public ObservableVariable<bool> IsPull { get; private set; }
    public ObservableVariable<bool> IsClimb { get; private set; }
    public ObservableVariable<bool> IsSlide { get; private set; }
    public ObservableVariable<bool> IsMoving { get; private set; }
    public ObservableVariable<bool> IsGrounded { get; private set; }
    public ObservableVariable<bool> IsSquitDown { get; private set; }
    public ObservableVariable<bool> IsRopeTrigger { get; private set; }
    public ObservableVariable<bool> IsInteractiveObject { get; private set; }


    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();

        IsGrounded = new ObservableVariable<bool>();
        IsMoving = new ObservableVariable<bool>();
        IsSquitDown = new ObservableVariable<bool>();
        IsInteractiveObject = new ObservableVariable<bool>();
        IsSlide = new ObservableVariable<bool>();
        IsRopeTrigger = new ObservableVariable<bool>();
        IsWall = new ObservableVariable<bool>();
        IsPull = new ObservableVariable<bool>();
        IsDead = new ObservableVariable<bool>();

        IsClimb = new ObservableVariable<bool>();

        capsuleHeight = collider.height;
        capsuleRadius = collider.radius;
        capsuleYOffset = collider.center.y;

        IsRopeTrigger.OnValueChangeEvent += IsRopeTriggerOnValueChangeHandler;
    }
    private void Update() {
        if (IsDead.Value) return;

        IsGrounded.Value = CheckGround();
        IsWall.Value = CheckWall();

        if (IsRopeTrigger.Value) {
            var distance = Vector3.Distance(transform.position, currentRopeKnot.position);
            if (distance > 0.2) {

                //transform.position = Vector3.MoveTowards(
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

        //if (isClimb && IsMoving.Value && !IsGrounded.Value) IsClimb.Value = true;
        //else IsClimb.Value = false;

        if (rigidbody.useGravity != true) return;

        if (rigidbody.velocity.y < -0.5 && IsGrounded.Value && !IsMoving.Value) IsSlide.Value = true;
        else IsSlide.Value = false;
    }



    public void IsUpButtonDown(bool button) {
        if (button && isRope) IsRopeTrigger.Value = true;
        else IsRopeTrigger.Value = false;
    }
    public void IsInteractiveButton(bool button) {
        if (button && IsInteractiveObject.Value) IsPull.Value = true;
        else IsPull.Value = false;
    }



    #region Move
    public void Move(Vector2 vectorMove) {
        if (IsDead.Value) return;

        if (vectorMove.x != 0) IsMoving.Value = true;
        else IsMoving.Value = false;

        PlayerRotation(vectorMove);

        if (currentInteractiveObj is ICapableMoving capable) {
            if (IsPull.Value) {
                Pull(capable, vectorMove);
                return;
            }
        }
        float horizontal =  vectorMove.x * speed;
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

        if (vector.x > 0)
            transform.rotation = Quaternion.identity;
        else if (vector.x < 0)
            transform.rotation = Quaternion.Euler(new Vector2(0, -180));
    }
    #endregion

    #region SquatDown
    public void SquatDown(bool isButtonPressed) {
        if (isButtonPressed) {
            collider.height = capsuleHeight /2;
            collider.radius = capsuleRadius /2;
            collider.center = new Vector3(collider.center.x, capsuleYOffset - capsuleYOffsetConstant, collider.center.z); 

            speed = Speed / 3;
        }
        else {
            collider.height = capsuleHeight;
            collider.radius = capsuleRadius;
            collider.center = new Vector3(collider.center.x, capsuleYOffset, collider.center.z);

            speed = Speed;
        }
        IsSquitDown.Value = isButtonPressed;
    }
    #endregion

    #region Pull/Push
    public void CollisionInteractiveObject(bool isCollision, IInteractive interactiveGo) {
        rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0);
        IsInteractiveObject.Value = isCollision;
        currentInteractiveObj = interactiveGo;
    }

    private void Pull(ICapableMoving capableMoving, Vector2 vectorMove) {
        capableMoving.Move(vectorMove * pullForce);
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
            rigidbody.useGravity = false;
            rigidbody.velocity = Vector3.zero;
            currentRopeKnot = GetNearlestKnotTransform(ropeKnots);
        }
        if (value == false) {
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
    private bool CheckWall()
    {
        Ray ray = new Ray(
            new Vector3(transform.position.x, transform.position.y + (collider.height - collider.radius) * transform.localScale.y, transform.position.z),
            new Vector3(transform.forward.z, 0, 0)
            );

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            if (Mathf.Abs(hit.normal.x) == 1)
                return true; 
        }

        Debug.DrawRay(
            new Vector3(transform.position.x, transform.position.y + (collider.height - collider.radius) * transform.localScale.y, transform.position.z),
            new Vector3(transform.forward.z, 0, 0), 
            Color.green, 10);

        return false;
    }
    #endregion


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.impulse);
        if (collision.impulse.y > deathImpulse)
            IsDead.Value= true;
    }

}
