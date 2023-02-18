using UnityEngine;

public class CheckpointZoneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Player>())
            Reloaded.Instance.SetRebirthPlace(transform.position, this);
    }


}
