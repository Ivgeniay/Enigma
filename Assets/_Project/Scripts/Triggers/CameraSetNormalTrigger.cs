using UnityEngine;


public class CameraSetNormalTrigger : MonoBehaviour
{
    [SerializeField] private CameraFollower cameraFollower;

    private void Awake()
    {
        if (!cameraFollower)
            cameraFollower = FindObjectOfType<CameraFollower>();
    }

    private void OnTriggerEnter(Collider other)
    {
        cameraFollower.SetOffset(Vector3.zero);
    }
}

