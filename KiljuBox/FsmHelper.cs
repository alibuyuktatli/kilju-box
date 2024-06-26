using HutongGames.PlayMaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KiljuBox
{
    public static class FsmHelper
    {
        public static FsmString GetFsmID(this GameObject gameObject)
        {
            return gameObject.GetFsmByName("Use").FsmVariables.FindVariable("ID") as FsmString;
        }
        public static PlayMakerFSM GetFsmByName(this GameObject gameObject, string fsmName)
        {
            foreach (var fsm in gameObject.GetComponents<PlayMakerFSM>())
            {
                if (fsm.FsmName == fsmName)
                {
                    return fsm;
                }
            }
            return null;
        }
    }
}
