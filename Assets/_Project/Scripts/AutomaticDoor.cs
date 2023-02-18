using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    [SerializeField] private Transform upperPoint;
    [SerializeField] private Rigidbody body;
    [SerializeField] private float force;
    [SerializeField] private float constantForce;


    private void FixedUpdate()
    {
        var f = (upperPoint.position - body.position) * force * Time.deltaTime;
        f = new Vector3(0, f.y, 0);
        Debug.Log(f);



        if (body.position != upperPoint.position)
            body.AddRelativeForce(f);
        //body.MovePosition(upperPoint.position);

        body.AddRelativeForce(Vector3.up * constantForce);
    }
}

