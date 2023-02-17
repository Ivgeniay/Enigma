using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDieAnimationProxy : MonoBehaviour
{
    private PlayerController playerController;
    private void Awake() {
        playerController = GetComponentInParent<PlayerController>();
    }

    private void OnDie() =>
        playerController.UncheckDeadFlag();
}
