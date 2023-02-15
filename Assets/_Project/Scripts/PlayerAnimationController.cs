using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private ObservableVariable<AnimationState> currentAnimationState;

    private PlayerController playerController;
    private new Rigidbody2D rigidbody2D;

    private int isGroundedHash = Animator.StringToHash("isGrounded");
    private int isMovingHash = Animator.StringToHash("isMoving");
    private int movingBlendHash = Animator.StringToHash("MovingBlend");
    private int jumpHash = Animator.StringToHash("Jump");
    private int isSquatDownHash = Animator.StringToHash("isSquatDown");
    private int isInteractiveObject = Animator.StringToHash("IsInteractiveObject");
    private int isSlide = Animator.StringToHash("IsSlide");

    private void Awake()
    {
        rigidbody2D = GetComponentInParent<Rigidbody2D>();
        playerController = GetComponentInParent<PlayerController>();

        animator = GetComponent<Animator>();
        currentAnimationState = new ObservableVariable<AnimationState>();

        playerController.IsGrounded.OnValueChangeEvent += IsGroundedOnValueChangeHandler;
        playerController.IsMoving.OnValueChangeEvent += IsMovingOnValueChangeHandler;
        playerController.IsSquitDown.OnValueChangeEvent += IsSquitDownOnValueChangeHandler;
        playerController.IsInteractiveObject.OnValueChangeEvent += IsInteractiveObjectOnValueChangeHandler;
        playerController.IsSlide.OnValueChangeEvent += IsSlideOnValueChangeHandler;
        playerController.OnJumpEvent += OnJumpHandler;

        currentAnimationState.OnValueChangeEvent += CurrentAnimationStateOnValueChangeHander;
    }


    public void SetAnimation(AnimationState animationState) =>    
        currentAnimationState.Value = animationState;

    private void Update()
    {
        animator.SetFloat(movingBlendHash, GetPersent(rigidbody2D.velocity.x, playerController.Speed));
    }
    
    private float GetPersent(float num, float from) => Mathf.Abs(num/from);


    #region Event
    private void OnJumpHandler() =>
        animator.SetTrigger(jumpHash);
    private void IsSlideOnValueChangeHandler(bool newValue) =>
        animator.SetBool(isSlide, newValue);
    private void IsMovingOnValueChangeHandler(bool newValue) =>
        animator.SetBool(isMovingHash, newValue);
    private void IsGroundedOnValueChangeHandler(bool newValue) =>    
        animator.SetBool(isGroundedHash, newValue);
    private void IsSquitDownOnValueChangeHandler(bool newValue) =>
        animator.SetBool(isSquatDownHash, newValue);
    private void IsInteractiveObjectOnValueChangeHandler(bool newValue) =>
    animator.SetBool(isInteractiveObject, newValue);
    private void CurrentAnimationStateOnValueChangeHander(AnimationState newValue) =>
        Debug.Log(newValue);
    
    #endregion



    
}
public enum AnimationState
{
    Idle,
    JumpUp,
    SideJump,
    ClingToHands,
    PullUp,
    SquatDown,
    SquatDownWalk,
    Dead,
    Pull,
    Push,
    SlideDown,
    HookShot,
    HookShot2,
    OnRope,
    IceAxeUp
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
            _value = value;
            OnValueChangeEvent?.Invoke(value);
        }
    }
}