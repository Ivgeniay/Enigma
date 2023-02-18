using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    [SerializeField] private Transform upperPoint;
    [SerializeField] private Rigidbody body;
    [SerializeField] private float force;


    private void FixedUpdate()
    {
        var f = (upperPoint.position - body.position) * force * Time.deltaTime;
        f = new Vector3(0, f.y, 0);

        if (body.position != upperPoint.position)
            body.AddRelativeForce(f);
    }
}

