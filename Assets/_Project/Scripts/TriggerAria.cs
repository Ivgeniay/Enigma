using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAria : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody; 
    [SerializeField] private int count = default;


    private void OnTriggerExit(Collider collision)
    {
        count++;
        if (count == 2)  {
            rigidbody.useGravity = true;
        }
    }
}
