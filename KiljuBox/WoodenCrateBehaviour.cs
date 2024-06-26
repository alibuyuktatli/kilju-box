using HutongGames.PlayMaker;
using MSCLoader;
using MSCLoader.Helper;
using System;
using System.Collections;
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

        // bottle save states
        public string[] bottleIDs = new string[6];
        public bool loading = true;

        int GetEmptySlot()
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

        bool IsUpsideDown()
        {
            return this.transform.rotation.eulerAngles.x < 90f && this.transform.rotation.eulerAngles.x > 70f;
        }

        public void SetKiljuFreeze(GameObject obj, bool set)
        {
            if (set)
            {
                obj.AddComponent<KiljuFreeze>();
            }
            else
            {
                KiljuFreeze freezeComponent = obj.GetComponent<KiljuFreeze>();
                if (freezeComponent != null)
                {
                    Destroy(freezeComponent);
                }
            }
        }

        public void RemoveKilju(int index)
        {
            GameObject kilju = slots[index];
            kilju.tag = "ITEM";
            kilju.layer = 19;
            SetKiljuFreeze(kilju, false);
            kilju.transform.parent = null;
            slots.RemoveAt(index);
        }

        public void SetLid(bool action)
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

        public void Init()
        {
            lid = this.transform.Find("lid").gameObject;
            SetLid(isSecured);
            StartCoroutine(LoadBottles());
        }

        IEnumerator LoadBottles()
        {
            if (bottleIDs.Length > 0)
            {
                //yield return new WaitForSeconds(1f);
                foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("ITEM"))
                {
                    try
                    {
                        if (CanProcessItem(gameObject))
                        {
                            if (gameObject.GetFsmByName("Use") != null && bottleIDs.Contains(gameObject.GetFsmID().Value))
                            {
                                AddKilju(gameObject);
                            }
                        }
                    }
                    catch (Exception e) { }
                    yield return null;
                }
            }
            this.loading = false;
            bottleIDs = new string[6];
            yield break;
        }

        void Update()
        {
            if (loading) return;
            if (isSecured) return;
            if (slots.Count <= 0) return;
            if (IsUpsideDown())
            {
                for (int i = 0; i < slots.Count; i++)
                {
                    RemoveKilju(i);
                }
            }
        }

        public static bool CanProcessItem(GameObject gameObject)
        {
            return gameObject.name.Contains("kilju") || gameObject.name.Contains("plastic can") || gameObject.name.Contains("juice");
        }

        private int AddKilju(GameObject gameObject)
        {
            int emptySlot = GetEmptySlot();
            if (emptySlot >= 0)
            {
                gameObject.transform.position = this.transform.position + this.transform.rotation * (SLOT_POSITIONS[emptySlot]);
                gameObject.transform.rotation = this.transform.rotation;
                gameObject.tag = "Untagged";
                gameObject.layer = 2;
                gameObject.transform.parent = this.transform;
                SetKiljuFreeze(gameObject, true);
                slots.Add(gameObject);
            }
            return emptySlot;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (loading) return;
            if (isSecured) return;
            if (slots.Count >= 6) return;
            if (IsUpsideDown()) return;

            if (CanProcessItem(other.gameObject))
            {
                AddKilju(other.gameObject);
            }
        }
    }
}