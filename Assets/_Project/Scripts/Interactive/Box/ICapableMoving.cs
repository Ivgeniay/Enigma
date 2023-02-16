using UnityEngine;

namespace Box
{
    public interface ICapableMoving: IInteractive
    {
        public void Move(Vector3 vector);
    }
}
