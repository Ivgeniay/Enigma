using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeTrigger : MonoBehaviour
{
    [SerializeField] private Transform rope;
    private void OnTriggerEnter(Collider collider) {
        var controller = collider.gameObject.GetComponent<PlayerController>();
        if (controller) {
            controller.SetRope(rope);
            controller.IsRope(true);
        }
    }

    private void OnTriggerExit(Collider collider) {
        var controller = collider.gameObject.GetComponent<PlayerController>();
        if (controller)
            controller.IsRope(false);
    }
}
