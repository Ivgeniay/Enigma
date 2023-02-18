using UnityEngine;

public class OnGravityTouch : MonoBehaviour
{
    private Rigidbody rigidbody;

    private void Awake() {
        rigidbody= GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        var player = collision.gameObject.GetComponent<Player>();

        if (player) {
            rigidbody.useGravity = true;
        }
    }
}

