using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action OnJumpEvent;
    [field: SerializeField] public float Speed { get; private set; } = 1f;
    [field: SerializeField] public float JumpForce { get; private set; } = 1f;
    [SerializeField] private float raycastMaxDistance = 1f;
    [SerializeField] private LayerMask groudLayerMask;

    private CapsuleCollider2D collider2D;
    private Vector2 CapsuleSize = new Vector2(0.35f, 1f);

    private new Rigidbody2D rigidbody;
    private float speed;


    private bool _isWallContact;
    public bool IsWallContact { get => _isWallContact; private set => _isWallContact = value; }

    public ObservableVariable<bool> IsGrounded { get; private set; }
    public ObservableVariable<bool> IsMoving { get; private set; }
    public ObservableVariable<bool> IsSquitDown { get; private set; }
    public ObservableVariable<bool> IsInteractiveObject { get; private set; }
    public ObservableVariable<bool> IsSlide { get; private set; }


    private void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<CapsuleCollider2D>();

        CapsuleSize = collider2D.size;

        IsGrounded = new ObservableVariable<bool>();
        IsMoving = new ObservableVariable<bool>();
        IsSquitDown = new ObservableVariable<bool>();
        IsInteractiveObject = new ObservableVariable<bool>();
        IsSlide = new ObservableVariable<bool>();
    }

    public void Move(Vector2 vectorMove) {
        if (vectorMove.x != 0)
            IsMoving.Value = true;
        else IsMoving.Value = false;

        PlayerRotation(vectorMove);
        
        float horizontal =  vectorMove.x * speed;
        rigidbody.velocity = new Vector2(horizontal, rigidbody.velocity.y);
    }
    public void Jump() {
        if (IsGrounded.Value) {
            OnJumpEvent.Invoke();
        }
    }

    public void SquatDown(bool isButtonPressed) {
        if (isButtonPressed) {
            collider2D.size = CapsuleSize / 2;
            speed = Speed / 3;
        }
        else {
            collider2D.size = CapsuleSize;
            speed = Speed;
        }
        IsSquitDown.Value = isButtonPressed;
    }
    public void CollisionInteractiveObject(bool isCollision) =>
        IsInteractiveObject.Value = isCollision;

    private void Update() {
        IsGrounded.Value = GroundMask();

        if (rigidbody.velocity.y < - 0.1 && IsGrounded.Value)
            IsSlide.Value = true;
        else 
            IsSlide.Value = false;
    }

    private bool GroundMask() {
        if(Physics2D.CapsuleCast(transform.position, CapsuleSize, CapsuleDirection2D.Vertical, 0, Vector2.down, raycastMaxDistance, groudLayerMask)) return true;
        else return false;
    }

    private void PlayerRotation(Vector2 vector)
    {
        if (IsSlide.Value) {
            transform.rotation = Quaternion.identity;
            return;
        }

        if (vector.x > 0)
            transform.rotation = Quaternion.identity;
        else if (vector.x < 0)
            transform.rotation = Quaternion.Euler(new Vector2(0, -180));
    }




}
