using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAria : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody2D; 
    [SerializeField] private int count = default;


    private void OnTriggerExit2D(Collider2D collision)
    {
        count++;
        if (count == 2)  {
            rigidbody2D.simulated = true;
        }
    }
}
