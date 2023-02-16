using UnityEngine;

namespace Box
{
    public class Box : MonoBehaviour, ICapableMoving
    {
        private Rigidbody rigidbody;

        private void Awake() {
            rigidbody= GetComponent<Rigidbody>();
        }

        public void Move(Vector3 vector) {
            rigidbody.AddForce(vector, ForceMode.Impulse);
        }
    }
}
