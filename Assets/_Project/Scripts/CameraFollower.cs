using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float Speed;
    [Range(0, 10)]
    [SerializeField] private float maxRange;

    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.position, Time.deltaTime * Speed);

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, transform.position.x - maxRange, player.position.x + maxRange),
            Mathf.Clamp(transform.position.y, transform.position.y - maxRange, player.position.y + maxRange),
            Mathf.Clamp(transform.position.y, transform.position.z - maxRange, player.position.z + maxRange)
            );
    }
    
}
