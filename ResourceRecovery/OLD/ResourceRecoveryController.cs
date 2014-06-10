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
    class ResourceRecoveryController : MonoBehaviour, Savable
    {
        private ResourceRecoveryGlobalSettings globalSettings;
        private ResourceRecoveryGameSettings gameSettings;
        private string configFilename;
        private bool loadingNewScene = false;
        private IButton button;

        void Awake()
        {
            Debug.Log("Awake");
            globalSettings = ResourceRecovery.Instance.globalSettings;
            gameSettings = ResourceRecovery.Instance.gameSettings;

            button = ToolbarManager.Instance.add("ResourceRecovery", "FlightIcon");
            button.TexturePath = "Philotical/ResourceRecovery/textures/ResourceRecoveryGasStationSymbol_small";
            button.ToolTip = "Resource Recovery";
            button.OnClick += (e) => OnIconClicked();


            configFilename = IOUtils.GetFilePathFor(this.GetType(), "ResourceRecovery.cfg");
        }

        void Start()
        {
            Debug.Log("Start");
            if (gameSettings.Enabled)
            {
                button.Visible = true;

                CrewRoster crewRoster = HighLogic.CurrentGame.CrewRoster;

                GameEvents.onGameSceneLoadRequested.Add(OnGameSceneLoadRequested);
            }
            else
            {
                button.Visible = false;
                Destroy(this);
            }
        }

        void OnDestroy()
        {
            Debug.Log("OnDestroy");
            button.Destroy();

            GameEvents.onGameSceneLoadRequested.Remove(OnGameSceneLoadRequested);
        }

        void FixedUpdate()
        {
            if (Time.timeSinceLevelLoad < 1.0f || !FlightGlobals.ready || loadingNewScene)
            {
                return;
            }

            double currentTime = Planetarium.GetUniversalTime();

        }


 
        public void Load(ConfigNode globalNode)
        {
           //button.Load(globalNode);
        }

        public void Save(ConfigNode globalNode)
        {
           // button.Save(globalNode);
        }

        private void OnIconClicked()
        {
        }

        private void OnGameSceneLoadRequested(GameScenes gameScene)
        {
            Debug.Log("Game scene load requested: " + gameScene);

            // Disable this instance becuase a new instance will be created after the new scene is loaded
            loadingNewScene = true;
        }

        private bool IsLaunched(Vessel vessel)
        {
            return vessel.missionTime > 0.01 || (Time.timeSinceLevelLoad > 5.0f && vessel.srf_velocity.magnitude > 2.0);
        }

    }
}
