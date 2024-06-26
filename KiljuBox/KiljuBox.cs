using MSCLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KiljuBox
{
    public class KiljuBox : Mod
    {
        //Mod config
        public override string ID => "KiljuBox";
        public override string Name => "KiljuBox";
        public override string Author => "alibuyuktatli";
        public override string Version => "1.590";
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
            Settings.AddButton(this, "resetCrates", "Reset", ResetCrateData);
        }

        public void ResetCrateData()
        {
            SaveUtility.Remove();
        }

        public void CreateCrate(Vector3 position, Vector3 rotation, string[] bottleIDs, bool isSecured)
        {
            GameObject crate = UnityEngine.Object.Instantiate<GameObject>(cratePrefab);

            WoodenCrateBehaviour behaviour = crate.AddComponent<WoodenCrateBehaviour>();
            behaviour.bottleIDs = bottleIDs;
            behaviour.isSecured = isSecured;
            
            crate.name = "Wooden Crate(itemx)";
            crate.layer = 19;
            crate.tag = "PART";
            crate.transform.transform.localPosition = position;
            crate.transform.localEulerAngles = rotation;
            crates.Add(crate);

            behaviour.Init();
        }
        public override void Update()
        {
            RaycastHit hitInfo;
            bool hit = (Physics.Raycast(
                Camera.main.ScreenPointToRay(Input.mousePosition),
                out hitInfo,
                1,
                1 << 19)
                && hitInfo.transform.gameObject.GetComponent<WoodenCrateBehaviour>() != null);

            if (hit)
            {
                WoodenCrateBehaviour crateBehaviour = hitInfo.transform.gameObject.GetComponent<WoodenCrateBehaviour>();
                PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = true;
                PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = crateBehaviour.isSecured ? "Open lid" : "Close lid";

                if (cInput.GetButtonDown("Use"))
                {
                    crateBehaviour.SetLid(!crateBehaviour.isSecured);
                }
            }
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
                    CreateCrate(data.position, data.rotation, data.bottleIDs, data.isSecured);
                }
            } else {
                for(int i = 0; i < (int)totalCrateCountSlider.GetValue(); i++)
                {
                    CreateCrate(SPAWN_POSITION, SPAWN_ROTATION, new string[6], false);
                }
            }
        }

        public override void OnSave()
        {
            WoodenCrateSaveUtility.Save(crates);
        }
    }
}
