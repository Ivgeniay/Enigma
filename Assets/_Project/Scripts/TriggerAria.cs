using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriggerAria : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody; 
    [SerializeField] private int count = default;
    [SerializeField] private GameObject ropeTrigger;
    private Transform rope;


    private void OnTriggerExit(Collider collision)
    {
        count++;
        if (count == 2)  {
            rigidbody.useGravity = true;
            //ropeTrigger.SetActive(true);

        }
    }
}
