using UnityEngine;

public class CheckpointZoneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        Reloaded.Instance.SetRebirthPlace(transform.position);
    }


}
