using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    private PlayerController playerController;
    private new Rigidbody rigidbody;

    private int isGroundedHash = Animator.StringToHash("isGrounded");
    private int isMovingHash = Animator.StringToHash("isMoving");
    private int movingBlendHash = Animator.StringToHash("MovingBlend");
    private int jumpHash = Animator.StringToHash("Jump");
    private int isSquatDownHash = Animator.StringToHash("isSquatDown");
    private int isInteractiveObject = Animator.StringToHash("IsInteractiveObject");
    private int isSlide = Animator.StringToHash("IsSlide");
    private int isRope = Animator.StringToHash("IsRope");
    private int isPull = Animator.StringToHash("IsPull");
    private int isDead = Animator.StringToHash("IsDead");
    
    private int isClimb = Animator.StringToHash("IsClimb");

    private void Awake()
    {
        rigidbody = GetComponentInParent<Rigidbody>();
        playerController = GetComponentInParent<PlayerController>();

        animator = GetComponent<Animator>();
    }

    private void Start() =>
        Subscribe();

    private void Update() {
        animator.SetFloat(movingBlendHash, GetPersent(rigidbody.velocity.x, playerController.Speed));
    }


    private void Subscribe()
    {
        playerController.IsGrounded.OnValueChangeEvent += IsGroundedOnValueChangeHandler;
        playerController.IsMoving.OnValueChangeEvent += IsMovingOnValueChangeHandler;
        playerController.IsSquitDown.OnValueChangeEvent += IsSquitDownOnValueChangeHandler;
        playerController.IsInteractiveObject.OnValueChangeEvent += IsInteractiveObjectOnValueChangeHandler;
        playerController.IsSlide.OnValueChangeEvent += IsSlideOnValueChangeHandler;
        playerController.IsRopeTrigger.OnValueChangeEvent += IsRopeTriggerOnValueChangeHandler;
        playerController.IsClimb.OnValueChangeEvent += IsClimbOnValueChangeHandler;
        playerController.IsPull.OnValueChangeEvent += IsPullOnValueChangeHandler;
        playerController.IsDead.OnValueChangeEvent += IsDeadOnValueChangeHandler;

        playerController.OnJumpEvent += OnJumpHandler;
    }


    private float GetPersent(float num, float from) => Mathf.Abs(num/from);


    #region Event
    private void IsPullOnValueChangeHandler(bool newValue) =>
        animator.SetBool(isPull, newValue);
    private void OnJumpHandler() =>
        animator.SetTrigger(jumpHash);
    private void IsDeadOnValueChangeHandler(bool newValue) =>
        animator.SetBool(isDead, newValue);
    private void IsSlideOnValueChangeHandler(bool newValue) =>
        animator.SetBool(isSlide, newValue);
    private void IsClimbOnValueChangeHandler(bool newValue) =>
        animator.SetBool(isClimb, newValue);
    private void IsMovingOnValueChangeHandler(bool newValue) =>
        animator.SetBool(isMovingHash, newValue);
    private void IsGroundedOnValueChangeHandler(bool newValue) =>    
        animator.SetBool(isGroundedHash, newValue);
    private void IsSquitDownOnValueChangeHandler(bool newValue) =>
        animator.SetBool(isSquatDownHash, newValue);
    private void IsRopeTriggerOnValueChangeHandler(bool newValue) =>
        animator.SetBool(isRope, newValue);
    private void IsInteractiveObjectOnValueChangeHandler(bool newValue) =>
        animator.SetBool(isInteractiveObject, newValue);
    private void CurrentAnimationStateOnValueChangeHander(AnimationState newValue) =>
        Debug.Log(newValue);

    #endregion




}

public class ObservableVariable<T>
{
    public event Action<T> OnValueChangeEvent;

    private T _value;
    public T Value
    {
        get { return _value; }
        set
        {
            var oldValue = _value;
            if (!_value.Equals(value))
                OnValueChangeEvent?.Invoke(value);
            _value = value;
        }
    }
}