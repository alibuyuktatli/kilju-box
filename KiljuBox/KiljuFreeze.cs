using UnityEngine;

namespace KiljuBox
{
    class KiljuFreeze : MonoBehaviour
    {
        private readonly Rigidbody rb;

        public KiljuFreeze()
        {
            rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            rb.detectCollisions = false;
            rb.isKinematic = true;
        }
        void OnDestroy()
        {
            rb.detectCollisions = true;
            rb.isKinematic = false;
        }
    }
}