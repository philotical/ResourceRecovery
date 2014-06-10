﻿/**
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Philotical
{
    public class ResourceRecoveryGameSettings
    {
        private const string configNodeName = "SavedGameSettings";

        public bool IsNewSave { get; set; }
        public bool Enabled { get; set; }
        public bool HibernateInsteadOfKill { get; set; }
        public double RespawnDelay { get; set; }

        public bool AllowCrewRespawn
        {
            get
            {
                return HighLogic.CurrentGame.Parameters.Difficulty.MissingCrewsRespawn;
            }
            set
            {
                HighLogic.CurrentGame.Parameters.Difficulty.MissingCrewsRespawn = value;
            }
        }


        public ResourceRecoveryGameSettings()
        {
            IsNewSave = true;
            Enabled = true;
            HibernateInsteadOfKill = false;
            RespawnDelay = 9203545.0; // 1 Kerbin year (the game's default is too short at only 36 minutes)

        }

        public void Load(ConfigNode node)
        {
            if (node.HasNode(configNodeName))
            {
                ConfigNode settingsNode = node.GetNode(configNodeName);

                IsNewSave = Utilities.GetValue(settingsNode, "IsNewSave", IsNewSave);
                Enabled = Utilities.GetValue(settingsNode, "Enabled", Enabled);
                HibernateInsteadOfKill = Utilities.GetValue(settingsNode, "HibernateInsteadOfKill", HibernateInsteadOfKill);
                RespawnDelay = Utilities.GetValue(settingsNode, "RespawnDelay", RespawnDelay);

            }
        }

        public void Save(ConfigNode node)
        {
            ConfigNode settingsNode;
            if (node.HasNode(configNodeName))
            {
                settingsNode = node.GetNode(configNodeName);
            }
            else
            {
                settingsNode = node.AddNode(configNodeName);
            }

            settingsNode.AddValue("IsNewSave", IsNewSave);
            settingsNode.AddValue("Enabled", Enabled);
            settingsNode.AddValue("HibernateInsteadOfKill", HibernateInsteadOfKill);
            settingsNode.AddValue("RespawnDelay", RespawnDelay);


        }
    }
}
