using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnJumpProxy : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private PlayerController conrollable;
    private float jumpForce;

    private void Awake()
    {
        rigidbody = GetComponentInParent<Rigidbody>();
        conrollable = GetComponentInParent<PlayerController>();
        jumpForce = conrollable.JumpForce;
    }

    public void OnJump() {
        rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.Force);
    }
}
