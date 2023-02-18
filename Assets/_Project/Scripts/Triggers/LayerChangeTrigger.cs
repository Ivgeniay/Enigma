using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerChangeTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player)
        {
            var sp = player.GetComponentInChildren<SpriteRenderer>();
            sp.sortingOrder = 1;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player)
        {
            var sp = player.GetComponentInChildren<SpriteRenderer>();
            sp.sortingOrder = 3;
        }
    }
}
