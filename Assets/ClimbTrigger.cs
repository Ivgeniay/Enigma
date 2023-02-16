using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClimbTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        var controller = collider.gameObject.GetComponent<PlayerController>();
        if (controller)
            controller.IsClimbTrigger(true);
    }

    private void OnTriggerExit(Collider collider)
    {
        var controller = collider.gameObject.GetComponent<PlayerController>();
        if (controller)
            controller.IsClimbTrigger(false);
    }

}
