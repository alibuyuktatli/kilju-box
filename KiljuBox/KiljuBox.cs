using MSCLoader;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KiljuBox
{
    public class KiljuBox : Mod
    {
        public override string ID => "KiljuBox";
        public override string Name => "KiljuBox";
        public override string Author => "alibuyuktatli";
        public override string Version => "1.0";
        public override string Description => "This mod adds 3 wooden crates to the entrance of the house that can be used to carry kilju.";

        public List<GameObject> crates = new List<GameObject>();
        public GameObject cratePrefab;
        public static int totalCrateCount = 3;
        public static Vector3 SPAWN_POSITION = new Vector3(-13.16654f, -0.574089766f, 12.9128084f);
        public static Vector3 SPAWN_ROTATION = new Vector3(270f, 89.34943f, 0f);

        public override void ModSettings()
        {

        }

        public void createCrate(Vector3 position, Vector3 rotation)
        {
            GameObject crate = UnityEngine.Object.Instantiate<GameObject>(cratePrefab);
            crate.AddComponent<WoodenCrateBehaviour>();
            crate.name = "Wooden Crate(itemx)";
            crate.layer = 19;
            crate.tag = "PART";
            crate.transform.transform.localPosition = position;
            crate.transform.localEulerAngles = rotation;
            crates.Add(crate);
        }

        public override void OnLoad()
        {
            AssetBundle woodCrateBundle = LoadAssets.LoadBundle(this, "kiljubox.unity3d");
            cratePrefab = woodCrateBundle.LoadAsset("woodencrate.prefab") as GameObject;
            woodCrateBundle.Unload(false);

            List<WoodenCrateSaveData> crateSaveData = WoodenCrateSaveUtility.Load();
            if (crateSaveData.Count > 0)
            {
                for(int i = 0; i < crateSaveData.Count; i++)
                {
                    WoodenCrateSaveData data = crateSaveData[i];
                    createCrate(data.position, data.rotation);
                }
            } else {
                for(int i = 0; i < totalCrateCount; i++)
                {
                    createCrate(SPAWN_POSITION, SPAWN_ROTATION);
                }
            }
        }
        public override void OnSave()
        {
            WoodenCrateSaveUtility.Save(crates);
        }
    }
}
