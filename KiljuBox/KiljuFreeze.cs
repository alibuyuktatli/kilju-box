using UnityEngine;

namespace MOP.Items
{
    class KiljuFreeze : MonoBehaviour
    {
        private readonly Rigidbody rb;

        public KiljuFreeze()
        {
            rb = GetComponent<Rigidbody>();
        }

        void Update()
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