using UnityEngine;

public class GhostDeadTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        var ghost = other.GetComponent<Ghost>();
        if (ghost) {
            ghost.Died();
        }
    }

}
