using UnityEngine;

public class GhostDeadTrigger : MonoBehaviour
{
    private Ghost ghost;
    private bool isActive = true;

    public void SetGhost(Ghost ghost) => 
        this.ghost = ghost;

    private void OnTriggerEnter(Collider other) {
        var player = other.GetComponent<Player>();
        if (player && isActive) {
            if (ghost != null) {
                isActive = false;
                ghost.Died();
            }
        }
        
    }

}
