using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Philotical
{
    class settingsWindowController
    {
        internal void settingsMain(int id)
        {
            GUILayout.Label("Settings:");
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();

            GUILayout.Label("Currently no settings available..");
            GUILayout.Label("If you think you need a certain setting, report it in the forum thread!");



            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
    }
}
