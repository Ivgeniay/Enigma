
using UnityEngine;

public class GhostTrigger : MonoBehaviour
{
    [SerializeField] private ChandelierFallingTrigger chandelierFalling;

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player)
            chandelierFalling.isActive = true;
    }
}

