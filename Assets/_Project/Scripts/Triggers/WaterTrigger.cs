using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    private ParticleSystem.MinMaxGradient _defaultPSColor;
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player)
        {
            var sp = player.GetComponentInChildren<SpriteRenderer>();
            sp.sortingOrder = 1;
            
            var ps = player.GetComponentInChildren<ParticleSystem>();
            var main = ps.main;
            _defaultPSColor = main.startColor;
            main.startColor = new Color(0, 100, 102);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player)
        {
            var sp = player.GetComponentInChildren<SpriteRenderer>();
            sp.sortingOrder = 3;
            
            var ps = player.GetComponentInChildren<ParticleSystem>();
            var main = ps.main;
            main.startColor = _defaultPSColor;
        }
    }
}
