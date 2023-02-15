using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float Speed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate() =>    
        transform.position = Vector2.Lerp(transform.position, player.position, Time.deltaTime * Speed);
    
}
