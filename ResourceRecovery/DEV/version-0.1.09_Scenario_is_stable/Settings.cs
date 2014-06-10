using System;
using System.Collections.Generic;
using UnityEngine;

namespace Philotical
{
    
	internal class RRSettingsController 
    {
        internal static ConfigNode root;
        internal static ConfigNode RRSettings = null;
        internal static ConfigNode FilePaths;


        internal static string get_KSCPlanet()
        {   // ConfigDefaults
            RRSettings = RRSettingsController.getRRSettings();
            FilePaths = RRSettingsController.getFilePaths();
            string p = FilePaths.GetValue("KSCPlanet");
            // ConfigDefaults
            return p;
        }

        internal static string get_KSCStorage_Lon_Lat()
        {   // ConfigDefaults
            string p = "KSC";
            // ConfigDefaults
            return p;
        }

        internal static string get_KSCStorage_capacity()
        {   // ConfigDefaults
            string p = get_resource_supply_mode_definition(0);
            // ConfigDefaults
            return p;
        }

        internal static string get_RR_ModuleName()
        {   // ConfigDefaults
            string p = "ResourceRecoveryPartModule";
            // ConfigDefaults
            return p;
        }
        //settings global
        internal static string get_path_ResourceRecovery_cfg()
        {   // ConfigDefaults
            string p = "GameData/Philotical/ResourceRecovery/ResourceRecovery.cfg";
            // ConfigDefaults
            return p;
        }
        //settings per save game
        internal static string get_ResourceRecoverySave_cfg()
        {   // ConfigDefaults
            string p = "ResourceRecoverySave.cfg";
            // ConfigDefaults
            return p;
        }
        internal static string get_path_Resources_factory_settings_cfg()
        {   // ConfigDefaults
            string p = "Philotical/ResourceRecovery/FactorySettings/Resource_FactorySettings.txt";
            // ConfigDefaults
            return p;
        }
        internal static string get_resource_supply_mode_definition(int supply_mode_int)
        {   // ConfigDefaults
            List<string> supply_mode_list = new List<string>();
	        supply_mode_list.Add("unlimited");
	        supply_mode_list.Add("normal");
	        supply_mode_list.Add("recycle");
            return supply_mode_list[supply_mode_int];
        }
        internal static GUIStyle get_label_default()
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.normal.textColor = Color.white;
            return style;
        }
        internal static GUIStyle get_label_10px()
        {
            GUIStyle label_10px = new GUIStyle(get_label_default());
            label_10px.fixedWidth = 10;
            return label_10px;
        }
        internal static GUIStyle get_label_20px()
        {
            GUIStyle label_20px = new GUIStyle(get_label_default());
            label_20px.fixedWidth = 20;
            return label_20px;
        }
        internal static GUIStyle get_label_40px()
        {
            GUIStyle label_40px = new GUIStyle(get_label_default());
            label_40px.fixedWidth = 40;
            return label_40px;
        }
        internal static GUIStyle get_label_60px()
        {
            GUIStyle label_60px = new GUIStyle(get_label_default());
            label_60px.fixedWidth = 60;
            return label_60px;
        }
        internal static GUIStyle get_label_80px()
        {
            GUIStyle label_80px = new GUIStyle(get_label_default());
            label_80px.fixedWidth = 80;
            return label_80px;
        }
        internal static GUIStyle get_label_100px()
        {
            GUIStyle label_100px = new GUIStyle(get_label_default());
            label_100px.fixedWidth = 100;
            return label_100px;
        }
        internal static GUIStyle get_label_120px()
        {
            GUIStyle label_120px = new GUIStyle(get_label_default());
            label_120px.fixedWidth = 120;
            return label_120px;
        }
        internal static GUIStyle get_label_stretchWidth()
        {
            GUIStyle label_stretchWidth = new GUIStyle(get_label_default());
            label_stretchWidth.stretchWidth = true;
            return label_stretchWidth;
        }
        internal static GUIStyle get_button_small()
        {
            GUIStyle button = new GUIStyle(GUI.skin.button);
            button.normal.textColor = Color.white;
            return button;
        }
        internal static GUIStyle get_button_small_active()
        {
            GUIStyle button_active = new GUIStyle(get_button_small());
            button_active.normal.textColor = Color.green;
            return button_active;
        }
        internal static GUIStyle get_button_80px()
        {
            GUIStyle button = new GUIStyle(GUI.skin.button);
            button.normal.textColor = Color.white;
            button.fixedWidth = 80;
            return button;
        }
        internal static GUIStyle get_button_80px_active()
        {
            GUIStyle button_active = new GUIStyle(get_button_80px());
            button_active.normal.textColor = Color.green;
            return button_active;
        }
        internal static GUIStyle get_button_100px()
        {
            GUIStyle button = new GUIStyle(GUI.skin.button);
            button.normal.textColor = Color.white;
            button.fixedWidth = 100;
            return button;
        }
        internal static GUIStyle get_button_100px_active()
        {
            GUIStyle button_active = new GUIStyle(get_button_100px());
            button_active.normal.textColor = Color.green;
            return button_active;
        }
        internal static GUIStyle get_button_stretchWidth()
        {
            GUIStyle get_button_stretchWidth = new GUIStyle(GUI.skin.button);
            get_button_stretchWidth.normal.textColor = Color.white;
            get_button_stretchWidth.stretchWidth = true;
            return get_button_stretchWidth;
        }
        internal static GUIStyle get_button_stretchWidth_active()
        {
            GUIStyle button_active = new GUIStyle(get_button_stretchWidth());
            button_active.normal.textColor = Color.green;
            return button_active;
        }
        internal static GUIStyle get_splitter()
        {
            GUIStyle splitter = new GUIStyle();
            splitter.normal.background = Utilities.MakeTex(5, 5, Color.white);
            splitter.stretchWidth = true;
            splitter.margin = new RectOffset(0, 0, 0, 7);
            splitter.fixedHeight = 1;
            return splitter;
        }

        internal static List<string> get_ResourceGroupList()
        {   // ConfigDefaults
            List<string> ResourceGroupList = new List<string>();
            ResourceGroupList.Add("none");
            ResourceGroupList.Add("fuel");
            ResourceGroupList.Add("electric");
            ResourceGroupList.Add("gas");
            ResourceGroupList.Add("waste");
            ResourceGroupList.Add("other");
            return ResourceGroupList;
        }

        internal static List<string> get_VesselRefuelOptionsList()
        {   // ConfigDefaults
            List<string> VesselRefuelOptionsList = new List<string>();
            VesselRefuelOptionsList.Add("Empty");
            VesselRefuelOptionsList.Add("1/8");
            VesselRefuelOptionsList.Add("1/4");
            VesselRefuelOptionsList.Add("3/8");
            VesselRefuelOptionsList.Add("Half");
            VesselRefuelOptionsList.Add("5/8");
            VesselRefuelOptionsList.Add("3/4");
            VesselRefuelOptionsList.Add("7/8");
            VesselRefuelOptionsList.Add("Full");
            return VesselRefuelOptionsList;
        }






        internal static bool load()
		{
            string path_ResourceRecovery_cfg = get_path_ResourceRecovery_cfg(); ;
            root = ConfigNode.Load(Utilities.getPluginSettingsFilePath(path_ResourceRecovery_cfg));
            if (root == null)
            {
                Debug.Log("ResourceRecovery getPluginSettingsFile config == null creating new file ");
                RRSettings_create();
                return false;
            }
            else
            {
                Debug.Log("ResourceRecovery getPluginSettingsFile done ");
                return true;
            }
        }
        internal static bool safe(string p, ConfigNode config)
        {
            if (!config.Save(p))
            {
                return false;
            }
            return true;
        }
       private static void RRSettings_create()
        {
            string path_Resources_factory_settings_cfg = get_path_Resources_factory_settings_cfg();
            string ResourceRecoverySave_cfg = get_ResourceRecoverySave_cfg();

            Debug.Log("ResourceRecovery RRSettings_create set ConfigDefaults");
            ConfigNode root = new ConfigNode("root");
            RRSettings = new ConfigNode("RRSettings");
            ConfigNode RRInfo = new ConfigNode("RRInfo");
            RRInfo.AddValue("PluginName", "ResourceRecovery");
            RRInfo.AddValue("PluginVersion", "0.1.8.0");
            RRInfo.AddValue("PluginDate", "16.05.2014");
            RRInfo.AddValue("ConfigFileWrittenAt", DateTime.Now.ToString());
            ConfigNode FPaths = new ConfigNode("FilePaths");
            FPaths.AddValue("KSCPlanet", "Kerbin");
            FPaths.AddValue("path_Resources_factory_settings_cfg", path_Resources_factory_settings_cfg);
            FPaths.AddValue("ResourceRecoverySave_cfg", ResourceRecoverySave_cfg);
            RRSettings.AddNode(RRInfo);
            RRSettings.AddNode(FPaths);
            root.AddNode(RRSettings);
            Utilities.SavePluginSettingsFile(get_path_ResourceRecovery_cfg(), root);
            RRSettings = null;
            RRSettings = Utilities.getPluginSettingsFile(get_path_ResourceRecovery_cfg());
            Debug.Log("ResourceRecovery RRSettings_create set ConfigDefaults Done");
            RRSettingsController.load();
        }

        internal static ConfigNode getRRSettings()
		{
            if (RRSettings==null)
            {
                if (RRSettingsController.load())
                {
                    RRSettings = root.GetNode("RRSettings");
                    return RRSettings;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return RRSettings;
            }
		}
        internal static ConfigNode getFilePaths()
		{
            if (FilePaths==null)
            {
                if (RRSettings == null)
				{
                    if (RRSettingsController.load())
					{
                        RRSettings = root.GetNode("RRSettings");
						FilePaths = RRSettings.GetNode("FilePaths");
						return FilePaths;
					}
					else
					{
						return null;
					}
				}
                else
                {
                    FilePaths = RRSettings.GetNode("FilePaths");
                    return FilePaths;
                }
            }
            else
            {
                return FilePaths;
            }
        }




    }
}
