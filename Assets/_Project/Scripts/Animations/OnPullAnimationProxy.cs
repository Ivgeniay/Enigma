using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPullAnimationProxy : MonoBehaviour
{
    [SerializeField] private float Force;
    private Rigidbody rigidbody;
    private Transform _transform;
    private PlayerController playerController;

    private void Awake() {
        rigidbody= GetComponentInParent<Rigidbody>();
        _transform = rigidbody.GetComponent<Transform>();
        playerController = _transform.GetComponent<PlayerController>();
    }
    
    private void OnPull() {
        var monoScr = playerController.CurrentInteractiveObj as ICapableMoving;
        if (monoScr is not null) {
            monoScr.Move(new Vector3(-_transform.forward.z, 0, 0) * Force);
        }
    }
}
