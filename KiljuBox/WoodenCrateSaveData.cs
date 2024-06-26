using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace KiljuBox
{
    public class WoodenCrateSaveData
    {
        public Vector3 position = new Vector3();
        public Vector3 rotation = new Vector3();
        public String[] bottleIDs = new String[6];
        public bool isSecured = false;
    }
}
