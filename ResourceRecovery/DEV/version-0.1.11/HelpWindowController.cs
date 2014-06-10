using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Philotical
{
    class HelpWindowController
    {
        public static Vector2 scrollPosition;
        private GUIStyle style;
        private GUIStyle style_10px_row ;
        private GUIStyle style_20px_row;
        private GUIStyle style_40px_row;
        private GUIStyle style_60px_row;
        private GUIStyle style_80px_row;
        private GUIStyle style_100px_row;
        private GUIStyle style_120px_row;
        private GUIStyle style_stretchWidth_row;
        public static GUIStyle buttonStyle;
        public static GUIStyle buttonStyle_active;
        public static GUIStyle splitter;
        public static ConfigNode ResourcesNode;


        internal void HelpMain(int id)
        {
             style = RRSettingsController.get_label_default();
            style_10px_row = RRSettingsController.get_label_10px();
            style_20px_row = RRSettingsController.get_label_20px();
            style_40px_row = RRSettingsController.get_label_40px();
            style_60px_row = RRSettingsController.get_label_60px();
            style_80px_row = RRSettingsController.get_label_80px();
            style_100px_row = RRSettingsController.get_label_100px();
            style_120px_row = RRSettingsController.get_label_120px();
            style_stretchWidth_row = RRSettingsController.get_label_stretchWidth();
            buttonStyle = RRSettingsController.get_button_100px();
            buttonStyle_active = RRSettingsController.get_button_100px_active();
            splitter = RRSettingsController.get_splitter();

            if (HighLogic.LoadedScene == GameScenes.SPACECENTER || HighLogic.LoadedScene == GameScenes.TRACKSTATION)
            {
                GUILayout.Label("Help: Manifest and Marketplace");
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();

                GUILayout.Label("Usage:");
                GUILayout.Label("All buttons do what it says on them! Should be self explanatory");
                GUILayout.Label("");

                GUILayout.Label("Known issues:");
                GUILayout.Label("Changes you made here are currently only saved when the plugin is terminated correctly.");
                GUILayout.Label("This is due to how KSP handles Scenario Modules and afaik I can't change it.");
                GUILayout.Label("To make sure you don't lose the changes, do not close the game window right now!");
                GUILayout.Label("Rather use the 'Exit to Main Menu' button or change scenes at least once,");
                GUILayout.Label("this will save all changes to the persistent file of this save.");
                GUILayout.Label("");



                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                /*
                */
            }

            if (HighLogic.LoadedScene == GameScenes.FLIGHT)
            {
                GUILayout.Label("Help: Refuel and Recover");
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();

                GUILayout.Label("Usage:");
                GUILayout.Label("All buttons do what it says on them! Should be self explanatory");
                GUILayout.Label("");

                GUILayout.Label("Known issues:");
                GUILayout.Label("Reverting a flight does currently NOT revert the storage list unless you save the game before you start.");
                GUILayout.Label("Hit F5 after you added/removed the ships resources");
                GUILayout.Label("After that, 'Revert Flight to Launch' function will revert to this savepoint.");
                GUILayout.Label("");



                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                /*
                */

            }

        }
    }
}
