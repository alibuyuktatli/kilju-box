using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace KiljuBox
{
    public class WoodenCrateSaveUtility
    {
        public static List<WoodenCrateSaveData> Load()
        {
            return SaveUtility.Load<List<WoodenCrateSaveData>>();
        }
        
        public static void Save(List<GameObject> crates)
        {
            List<WoodenCrateSaveData> woodenCrateSaveData = new List<WoodenCrateSaveData>();

            for (int i = 0; i < crates.Count; i++)
            {
                List<string> bottles = new List<string>();
                WoodenCrateBehaviour woodenCrateBehaviour = crates[i].GetComponent<WoodenCrateBehaviour>();
                for (int ii = 0; ii < woodenCrateBehaviour.slots.Count; ii++)
                {
                    bottles.Add(woodenCrateBehaviour.slots[ii].GetFsmID().Value);
                }
                woodenCrateSaveData.Add(new WoodenCrateSaveData()
                {
                    position = crates[i].transform.position,
                    rotation = crates[i].transform.eulerAngles,
                    bottleIDs = bottles.ToArray(),
                    isSecured = crates[i].GetComponent<WoodenCrateBehaviour>().isSecured
                });
            }
            SaveUtility.Save(woodenCrateSaveData);
        }
    }
}
