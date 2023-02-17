using UnityEngine;

public class PlayerSoundAudioclipAnimation : MonoBehaviour
{
    private PlayerSound playerSound;

    private void Awake()
    {
        playerSound = GetComponentInParent<PlayerSound>();
    }

    private void OnStepSound() =>
        playerSound.Step();
    private void OnJumpSound() =>
        playerSound.Jump();
}

