using System;
using UnityEngine;

public interface IConrollable
{
    public event Action OnJumpEvent;
    public float Speed { get; }
    public ObservableVariable<bool> IsGrounded { get; }
    public ObservableVariable<bool> IsMoving { get; }
    public bool IsWallContact { get; }
    public void Move(Vector2 vectorMove);
    public void Jump();
}