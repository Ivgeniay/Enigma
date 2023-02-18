
using UnityEngine;

public class GhostTrigger : MonoBehaviour
{
    [SerializeField] private ChandelierFallingTrigger chandelierFalling;
    [SerializeField] private Ghost GhostPrefab;

    [SerializeField] private float minPosX;
    [SerializeField] private float maxPosX;
    [SerializeField] private float minPosY;
    [SerializeField] private float maxPosY;

    private bool isActive = true;

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player && isActive) {
            isActive = false;
            chandelierFalling.isActive = true;
            var ghost = Instantiate(GhostPrefab, transform.position, Quaternion.identity, null);
            ghost.SetBorder(minPosX, maxPosX, minPosY, maxPosY);
        }
    }
}

