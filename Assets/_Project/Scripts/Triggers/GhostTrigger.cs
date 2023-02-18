
using UnityEngine;

public class GhostTrigger : MonoBehaviour
{
    [SerializeField] private ChandelierFallingTrigger chandelierFalling;
    [SerializeField] private Ghost GhostPrefab;

    [SerializeField] private float minPosX;
    [SerializeField] private float maxPosX;
    [SerializeField] private float minPosY;
    [SerializeField] private float maxPosY;

    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] GhostDeadTrigger deadTrigger;

    private bool isActive = true;

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player && isActive) {
            if (spawnPosition == Vector3.zero)
                spawnPosition = transform.position;

            isActive = false;
            chandelierFalling.isActive = true;
            var ghost = Instantiate(GhostPrefab, spawnPosition, Quaternion.identity, null);
            ghost.SetBorder(minPosX, maxPosX, minPosY, maxPosY);

            if (deadTrigger != null)
                deadTrigger.SetGhost(ghost);
        }
    }
}

