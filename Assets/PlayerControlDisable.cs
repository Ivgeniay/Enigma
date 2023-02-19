using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerControlDisable : MonoBehaviour
{
    private Player player;
    private void Awake() {
        player = FindObjectOfType<Player>();
    }

    private void PlayerControll(bool value) {
        player.Controll(value);
    }

    private void OnTriggerEnter(Collider other) {
        PlayerControll(false);
    }
}
