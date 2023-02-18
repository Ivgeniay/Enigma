using UnityEngine;


public class CameraSetNormalTrigger : MonoBehaviour
{
    [SerializeField] private CameraFollower cameraFollower;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float minPosX;
    [SerializeField] private float maxPosX;
    [SerializeField] private float minPosY;
    [SerializeField] private float maxPosY;

    private void Awake()
    {
        if (!cameraFollower)
            cameraFollower = FindObjectOfType<CameraFollower>();
    }

    private void OnTriggerEnter(Collider other)
    {
        cameraFollower.SetOffset(offset);
        if(maxPosX != 0) cameraFollower.SetMaxPosX(maxPosX);
        if(minPosX != 0) cameraFollower.SetMinPosX(minPosX);
        if(maxPosY != 0) cameraFollower.SetMaxPosY(maxPosY);
        if(minPosY != 0) cameraFollower.SetMinPosY(minPosY);
    }
}

