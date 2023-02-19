using UnityEngine;

public class GhostDeathTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player) {
            player.Dead();
        }
    }
}

