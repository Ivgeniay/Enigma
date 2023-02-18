
using UnityEngine;

public class ChandelierFallingTrigger : MonoBehaviour
{
    [SerializeField] private Rigidbody chandelierRigitbody;
    public bool isActive;

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player && isActive) {
            chandelierRigitbody.useGravity = true;
        }
    }
}

