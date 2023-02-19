using UnityEngine;

public class NecklaceSpawnTrigger : MonoBehaviour
{
    [SerializeField] private NecklaceDestroy necklaceDestroy;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Camera camera;

    private void OnNecklaceInit()  {
        Instantiate(necklaceDestroy, transform.position + offset, Quaternion.identity, null);
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player != null) {
            OnNecklaceInit();
            if (camera)
            {
                camera.orthographicSize = 6.25f;
            }
        }
    }
}
