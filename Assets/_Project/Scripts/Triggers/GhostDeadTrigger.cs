using System.Collections.Generic;
using UnityEngine;

public class GhostDeadTrigger : MonoBehaviour
{
    private List<Ghost> ghosts = new();
    //private bool isActive = true;

    public void SetGhost(Ghost ghost) => 
        this.ghosts.Add(ghost);

    private void OnTriggerEnter(Collider other) {
        var player = other.GetComponent<Player>();

        if (player) {
            if (ghosts != null) {
                ghosts.ForEach(el => el.Died());
            }
        }
        
    }

}
