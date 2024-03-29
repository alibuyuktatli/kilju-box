﻿using MSCLoader;
using MSCLoader.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KiljuBox
{
    public class WoodenCrateBehaviour : MonoBehaviour
    {
        public static readonly Vector3 NO_SECURE_COLLIDER_CENTER = new Vector3(-0.001619838f, 0.006906725f, 0.08727966f);
        public static readonly Vector3 NO_SECURE_COLLIDER_SIZE = new Vector3(0.4230931f, 0.2580905f, 0.1740216f);

        public static readonly Vector3 SECURE_COLLIDER_CENTER = new Vector3(-0.001620293f, 0.006906748f, 0.1298048f);
        public static readonly Vector3 SECURE_COLLIDER_SIZE = new Vector3(0.4230931f, 0.2580905f, 0.2590722f);

        public static readonly List<Vector3> SLOT_POSITIONS = new List<Vector3>
        {
            new Vector3(-0.12f, -0.045f, slotHeight),
            new Vector3(-0.12f, 0.075f, slotHeight),
            new Vector3(0f, -0.045f, slotHeight),
            new Vector3(0f, 0.073f, slotHeight),
            new Vector3(0.12f, -0.045f, slotHeight),
            new Vector3(0.12f, 0.075f, slotHeight)
        };

        public bool isSecured = false;
        const float slotHeight = 0.13f;
        public List<GameObject> slots = new List<GameObject>();
        public GameObject lid;

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

        public void setKiljuFreeze(GameObject obj, bool set)
        {
            if (set)
            {
                obj.AddComponent<KiljuFreeze>();
            } else {
                KiljuFreeze freezeComponent = obj.GetComponent<KiljuFreeze>();
                if (freezeComponent != null)
                {
                    Destroy(freezeComponent);
                }
            }
        }

        public void removeKilju(int index)
        {
            GameObject kilju = slots[index];
            kilju.tag = "ITEM";
            kilju.layer = 19;
            setKiljuFreeze(kilju, false);
            kilju.transform.parent = null;
            slots.RemoveAt(index);
        }

        public void setLid(bool action)
        {
            lid.GetComponent<MeshRenderer>().enabled = action;
            this.isSecured = action;
            if (this.isSecured == true)
            {
                this.gameObject.GetComponent<BoxCollider>().center = SECURE_COLLIDER_CENTER;
                this.gameObject.GetComponent<BoxCollider>().size = SECURE_COLLIDER_SIZE;
            }
            else
            {
                this.gameObject.GetComponent<BoxCollider>().center = NO_SECURE_COLLIDER_CENTER;
                this.gameObject.GetComponent<BoxCollider>().size = NO_SECURE_COLLIDER_SIZE;
            }
        }

        void Start()
        {
            lid = this.transform.Find("lid").gameObject;
        }

        void Update()
        {
            if (isSecured) return;
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
            if (isSecured) return;
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
                    other.gameObject.layer = 2;
                    other.transform.parent = this.transform;
                    setKiljuFreeze(other.gameObject, true);
                    slots.Add(other.gameObject);
                }
            }
        }
    }
}