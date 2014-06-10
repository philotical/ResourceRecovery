/**
 * Thunder Aerospace Corporation's Life Support for Kerbal Space Program.
 * Written by Taranis Elsu.
 * 
 * (C) Copyright 2013, Taranis Elsu
 * 
 * Kerbal Space Program is Copyright (C) 2013 Squad. See http://kerbalspaceprogram.com/. This
 * project is in no way associated with nor endorsed by Squad.
 * 
 * This code is licensed under the Attribution-NonCommercial-ShareAlike 3.0 (CC BY-NC-SA 3.0)
 * creative commons license. See <http://creativecommons.org/licenses/by-nc-sa/3.0/legalcode>
 * for full details.
 * 
 * Attribution — You are free to modify this code, so long as you mention that the resulting
 * work is based upon or adapted from this code.
 * 
 * Non-commercial - You may not use this work for commercial purposes.
 * 
 * Share Alike — If you alter, transform, or build upon this work, you may distribute the
 * resulting work only under the same or similar license to the CC BY-NC-SA 3.0 license.
 * 
 * Note that Thunder Aerospace Corporation is a ficticious entity created for entertainment
 * purposes. It is in no way meant to represent a real entity. Any similarity to a real entity
 * is purely coincidental.
 */

using KSP.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Philotical
{
    class ResourceRecoverySavedGameConfigWindow : Window<ResourceRecoverySavedGameConfigWindow>
    {
        private const int SECONDS_PER_DAY = 24 * 60 * 60;
        private ResourceRecoveryGlobalSettings globalSettings;
        private ResourceRecoveryGameSettings gameSettings;
        private GUIStyle labelStyle;
        private GUIStyle editStyle;
        private GUIStyle headerStyle;
        private GUIStyle headerStyle2;
        private GUIStyle warningStyle;
        private GUIStyle buttonStyle;
        private IButton button;

        private bool showConsumptionRates = false;
        private bool showMaxTimeWithout = false;
        private bool showDefaultResourceAmounts = false;

        private readonly string version;

        public ResourceRecoverySavedGameConfigWindow(ResourceRecoveryGlobalSettings globalSettings, ResourceRecoveryGameSettings gameSettings)
            : base("ResourceRecoveryGlobalSettings")
        {
            this.globalSettings = globalSettings;
            this.gameSettings = gameSettings;

            //version = Utilities.GetDllVersion(this);
        }

        protected override void ConfigureStyles()
        {
            base.ConfigureStyles();

            if (labelStyle == null)
            {
                labelStyle = new GUIStyle(GUI.skin.label);
                labelStyle.alignment = TextAnchor.MiddleLeft;
                labelStyle.fontStyle = FontStyle.Normal;
                labelStyle.normal.textColor = Color.white;
                labelStyle.wordWrap = false;

                editStyle = new GUIStyle(GUI.skin.textField);
                editStyle.alignment = TextAnchor.MiddleRight;

                headerStyle = new GUIStyle(labelStyle);
                headerStyle.fontStyle = FontStyle.Bold;

                headerStyle2 = new GUIStyle(headerStyle);
                headerStyle2.wordWrap = true;

                buttonStyle = new GUIStyle(GUI.skin.button);

                warningStyle = new GUIStyle(headerStyle2);
                warningStyle.normal.textColor = new Color(0.88f, 0.20f, 0.20f, 1.0f);
            }
        }

        protected override void DrawWindowContents(int windowId)
        {
            GUILayout.Label("Version: " + version, labelStyle);
            GUILayout.Label("Configure TAC Life Support for use with this saved game.", headerStyle);
            gameSettings.Enabled = GUILayout.Toggle(gameSettings.Enabled, "Enabled");

            if (gameSettings.Enabled)
            {
                GUILayout.Space(10);

                //string[] killOptions = { "Die", "Hibernate" };
                //int oldValue = (gameSettings.HibernateInsteadOfKill) ? 1 : 0;

                //GUILayout.BeginHorizontal();
                //GUILayout.Label("When resources run out, Kerbals ", labelStyle);
                //int newValue = GUILayout.SelectionGrid(oldValue, killOptions, 2);
                //GUILayout.EndHorizontal();
                //gameSettings.HibernateInsteadOfKill = (newValue == 1);


            }

            if (GUI.changed)
            {
                SetSize(10, 10);
            }
        }

   }
}
