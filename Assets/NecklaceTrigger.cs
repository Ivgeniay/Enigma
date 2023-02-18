using UnityEngine;

public class NecklaceTrigger : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private int fade = Animator.StringToHash("Fade");

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player) {
            player.PickNecllace();
            animator.SetTrigger(fade);
        }
    }
    
}
