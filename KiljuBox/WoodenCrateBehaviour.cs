using MSCLoader;
using MSCLoader.Helper;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KiljuBox
{
    public class WoodenCrateBehaviour : MonoBehaviour
    {
        readonly List<Vector3> SLOT_POSITIONS = new List<Vector3>
        {
            new Vector3(-0.12f, -0.045f, slotHeight),
            new Vector3(-0.12f, 0.075f, slotHeight),
            new Vector3(0f, -0.045f, slotHeight),
            new Vector3(0f, 0.073f, slotHeight),
            new Vector3(0.12f, -0.045f, slotHeight),
            new Vector3(0.12f, 0.075f, slotHeight)
        };

        const float slotHeight = 0.13f;
        public List<GameObject> slots = new List<GameObject>();

        int getEmptySlot()
        {
            for (int i = 0; i < 6; i++)
            {

                GameObject emptySlot = slots.ElementAtOrDefault(i);
                if (!emptySlot)
                {
                    return i;
                }
            }
            return -1;
        }

        bool isUpsideDown()
        {
            return this.transform.rotation.eulerAngles.x < 90f && this.transform.rotation.eulerAngles.x > 70f;
        }

        public void removeKilju(int index)
        {
            slots[index].tag = "ITEM";
            slots[index].layer = 19;
            slots[index].GetComponent<Rigidbody>().detectCollisions = true;
            Destroy(slots[index].GetComponent<FixedJoint>());
            slots.RemoveAt(index);
        }

        void Update()
        {
            if (slots.Count <= 0) return;
            if (isUpsideDown())
            {
                for (int i = 0; i < slots.Count; i++)
                {
                    removeKilju(i);
                }
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (slots.Count >= 6) return;
            if (isUpsideDown()) return;

            if (other.gameObject.name.Contains("kilju") || other.gameObject.name.Contains("plastic can") || other.gameObject.name.Contains("juice"))
            {
                int emptySlot = getEmptySlot();
                if (emptySlot >= 0)
                {
                    other.transform.position = this.transform.position + this.transform.rotation * (SLOT_POSITIONS[emptySlot]);
                    other.transform.rotation = this.transform.rotation;
                    other.gameObject.tag = "Untagged";
                    other.gameObject.layer = 0;
                    other.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
                    if (other.gameObject.GetComponent<FixedJoint>() == null)
                    {
                        other.gameObject.AddComponent<FixedJoint>().connectedBody = this.gameObject.GetComponent<Rigidbody>();
                    }

                    FixedJoint joint = other.gameObject.GetComponent<FixedJoint>();
                    joint.autoConfigureConnectedAnchor = false;
                    joint.connectedBody = this.gameObject.GetComponent<Rigidbody>();
                    slots.Add(other.gameObject);
                }
            }
        }
    }
}