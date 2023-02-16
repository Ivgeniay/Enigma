using UnityEngine;

public class OnRopeAnimationProxy : MonoBehaviour
{
    //private Rigidbody playerRigidbody;
    private PlayerController playerController;
    private float impuls;

    private void Awake() {
        //playerRigidbody = GetComponentInParent<Rigidbody>();
        playerController = GetComponentInParent<PlayerController>();
        impuls = playerController.ImpulsRopeUp;
    }

    private void SetImpuls() {
        playerController.ImpulsRopeUp = impuls;
    }

}
