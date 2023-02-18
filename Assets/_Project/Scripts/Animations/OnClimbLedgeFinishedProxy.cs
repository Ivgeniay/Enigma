using UnityEngine;

public class OnClimbLedgeFinishedProxy : MonoBehaviour
{
    private PlayerController playerController;
    private void Awake() {
        playerController = GetComponentInParent<PlayerController>();
    }

    private void OnClimbLedgeFinished() =>
        playerController.FinishLedgeClimb();
}