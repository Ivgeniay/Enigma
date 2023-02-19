using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public event Action OnPlayerDeadEvent;
    [field: SerializeField] private float deathImpulse { get; set; } = 6f;

    private PlayerController playerController;
    private PlayerSound playerSound;

    private bool isDead;
    public static bool isPickax { get; private set; }
    public static bool isNecllace { get; private set; }

    private void Awake() {
        Reloaded.Instance.Instantiate(this);
        playerController = GetComponent<PlayerController>();
        playerSound = GetComponent<PlayerSound>();
    }


    void Update()
    {
        if (isDead) return;
        if (!playerController) return;

        Vector2 moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        playerController.Move(moveVector);
        playerSound.Move(moveVector);

        if (Input.GetKeyDown(KeyCode.Space)) playerController.Jump();

        var squatDown = Input.GetKey(KeyCode.LeftControl);
        if (squatDown is false) squatDown= Input.GetKey(KeyCode.S);
        playerController.SquatDown(squatDown);


        playerController.IsInteractiveButton(Input.GetKey(KeyCode.E));
        playerController.IsUpButtonDown(Input.GetKey(KeyCode.W));
    }

    private void OnCollisionEnter(Collision collision) {
        if (playerController is not null)
        {
            var interactiveObj = collision.gameObject.GetComponent<IInteractive>();
            if (interactiveObj is not null)
            {
                Vector2 normal = collision.contacts[0].normal;
                float dot = Vector2.Dot(normal, new Vector2(-transform.forward.z, 0));
                if (dot > 0.7) {
                    playerController.CollisionInteractiveObject(true, interactiveObj);
                }
            }

            if (collision.impulse.y > 1) {
                var level = Mathf.Clamp(collision.impulse.y, 0f, 10);
                level = Mathf.InverseLerp(0, 10, level);
                playerSound.Landing(level);
            }

            if (collision.impulse.y > deathImpulse) {
                Dead();
            }
        }
    }

    private void OnCollisionStay(Collision collision) {
        if (playerController is not null) {
            var _normal = collision.contacts[0].normal;
            if (_normal.y <= 0.1) _normal = new Vector3(_normal.x, 0.2f, _normal.z);
            if (_normal.y >= 1) _normal = new Vector3(_normal.x, 1f, _normal.z);
            playerController.SetNormal(_normal);
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (playerController is not null) {
            playerController.CollisionInteractiveObject(false, null);
        }
    }

    public void PickPickax() =>
        isPickax= true;

    public void PickNecllace() =>
        isNecllace = true;

    public void Dead()
    {
        isDead = true;
        playerController.Dead();
        playerController = null;
        OnPlayerDeadEvent?.Invoke();
    }

    internal void Controll(bool value) {
        if (value == false) {
            playerController = null;
        }
    }
}
