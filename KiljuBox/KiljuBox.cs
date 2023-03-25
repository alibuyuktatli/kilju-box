using MSCLoader;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KiljuBox
{
    public class KiljuBox : Mod
    {
        //Mod config
        public override string ID => "KiljuBox";
        public override string Name => "KiljuBox";
        public override string Author => "alibuyuktatli";
        public override string Version => "1.43";
        public override string Description => "This mod adds wooden crates to the entrance of the house that can be used to carry kilju. You can change crate count from settings";

        public List<GameObject> crates = new List<GameObject>();
        public GameObject cratePrefab;
        public static Vector3 SPAWN_POSITION = new Vector3(-13.16654f, -0.574089766f, 12.9128084f);
        public static Vector3 SPAWN_ROTATION = new Vector3(270f, 89.34943f, 0f);

        //Settings
        SettingsSliderInt totalCrateCountSlider;

        public override void ModSettings()
        {
            Settings.AddHeader(this, "Total crate count");
            totalCrateCountSlider = Settings.AddSlider(this, "totalCrateCount", "Total crate count", 1, 10, 5);
            Settings.AddHeader(this, "Reset crate locations");
            Settings.AddButton(this, "resetCrates", "Reset", resetCrateData);
        }

        public void resetCrateData()
        {
            SaveUtility.Remove();
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

        /*
        public override void update()
        {
            raycasthit hitınfo;
            bool hashit = (physics.raycast(
                camera.main.screenpointtoray(ınput.mouseposition), // where the camera is facing.
                out hitınfo, // the hit info. 
                5)); // checking if raycast hit said gameobject

            if (hashit)
            {
                modconsole.print("has detected: " + hitınfo.transform.gameobject.name); // prints the game object's name to the console when the player is looking at it. expecting 'truck engine(xxxxx)'.
                modconsole.print("layer: " + hitınfo.transform.gameobject.layer);
                modconsole.print("tag: " + hitınfo.transform.gameobject.tag);
            }
        }
        */

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
                for(int i = 0; i < (int)totalCrateCountSlider.GetValue(); i++)
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
