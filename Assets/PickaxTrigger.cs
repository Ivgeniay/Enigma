using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxTrigger : MonoBehaviour
{
    private Animator animator;

    private int isPeak = Animator.StringToHash("IsPick");

    private void Awake() {
        animator = GetComponentInParent<Animator>(true);
    }


    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player) {
            player.PickPickax();
            animator.SetBool(isPeak, true);
        }
    }
}
