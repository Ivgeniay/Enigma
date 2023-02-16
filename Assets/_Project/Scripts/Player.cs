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

        var squatDown = Input.GetKey(KeyCode.LeftControl);
        if (squatDown is false) squatDown= Input.GetKey(KeyCode.S);
        plyaerController.SquatDown(squatDown);

        if (Input.GetKeyDown(KeyCode.Space)) 
            plyaerController.Jump();
        plyaerController.IsUpButtonDown(Input.GetKey(KeyCode.W));
    }

    private void OnCollisionEnter(Collision collision) {
        var interactiveObj = collision.gameObject.GetComponent<ICapableMoving>();
        if (interactiveObj is not null)
        {
            Vector2 normal = collision.contacts[0].normal;
            float dot = Vector2.Dot(normal, new Vector2(-transform.forward.z, 0));
            if (dot > 0.7)
                plyaerController.CollisionInteractiveObject(true);
        }
    }
    private void OnCollisionExit(Collision collision) {
        var interactiveObj = collision.gameObject.GetComponent<ICapableMoving>();
        if (interactiveObj is not null)
        {
            plyaerController.CollisionInteractiveObject(false);
        }
    }
    
}
