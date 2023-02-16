using Box;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Player : MonoBehaviour
{
    private PlayerController plyaerController;
    
    private void Awake()
    {
        plyaerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        Vector2 moveVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        plyaerController.Move(moveVector);

        if (Input.GetKeyDown(KeyCode.Space)) plyaerController.Jump();

        var squatDown = Input.GetKey(KeyCode.LeftControl);
        if (squatDown is false) squatDown= Input.GetKey(KeyCode.S);
        plyaerController.SquatDown(squatDown);


        plyaerController.IsInteractiveButton(Input.GetKey(KeyCode.E));
        plyaerController.IsUpButtonDown(Input.GetKey(KeyCode.W));
    }

    private void OnCollisionEnter(Collision collision) {
        var interactiveObj = collision.gameObject.GetComponent<IInteractive>();
        if (interactiveObj is not null)
        {
            Vector2 normal = collision.contacts[0].normal;
            float dot = Vector2.Dot(normal, new Vector2(-transform.forward.z, 0));
            if (dot > 0.7) {
                plyaerController.CollisionInteractiveObject(true, interactiveObj);
            }
        }
    }
    private void OnCollisionExit(Collision collision) {
        var interactiveObj = collision.gameObject.GetComponent<IInteractive>();
        if (interactiveObj is not null) {
            plyaerController.CollisionInteractiveObject(false, interactiveObj);
        }
    }
    
}
