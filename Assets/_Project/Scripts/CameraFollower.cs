using System;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float Speed;

    [SerializeField] private Vector2 offset;

    [SerializeField] private float minPosX;
    [SerializeField] private float maxPosX;
    [SerializeField] private float minPosY;
    [SerializeField] private float maxPosY;

    public void SetOffset(Vector3 offset) =>    
        this.offset = offset;
    

    private void Awake() {
        if (player == null)
            player = FindObjectOfType<Player>().GetComponent<Transform>();
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.position + new Vector3(offset.x, offset.y, 0), Time.deltaTime * Speed);
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minPosX, maxPosX),
            Mathf.Clamp(transform.position.y, minPosY, maxPosY),
            transform.position.z
            );
    }
    
}
