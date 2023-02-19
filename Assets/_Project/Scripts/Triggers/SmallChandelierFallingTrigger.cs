using System.Collections;
using UnityEngine;

public class SmallChandelierFallingTrigger : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float delay;

    private void Awake()
    {
        rigidbody= GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        var player = collision.gameObject.GetComponent<Player>();
        if (player)
            StartCoroutine(Fall(delay));
    }

    private IEnumerator Fall(float delay) {
        yield return new WaitForSeconds(delay);
        rigidbody.useGravity= true;
    }
}

