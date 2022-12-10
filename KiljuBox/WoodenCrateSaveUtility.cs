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
                woodenCrateSaveData.Add(new WoodenCrateSaveData()
                {
                    position = crates[i].transform.position,
                    rotation = crates[i].transform.eulerAngles
                });
            }
            SaveUtility.Save(woodenCrateSaveData);
        }
    }
}
