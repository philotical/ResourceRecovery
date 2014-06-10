using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Philotical
{
    public class Utilities
    {
        internal static Vessel vessel;
        //public static ScenarioNodeDatabase Scenario;

        static internal string Sanitize(string s, bool skip, string subchar_string = "-")
        {
            string admitted = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-";
            StringBuilder output = new StringBuilder(s.Length);
            bool found = false;
            char subchar = subchar_string[0];
            foreach (char c in s)
            {
                found = false;
                foreach (char adm in admitted)
                {
                    if (c == adm)
                    {
                        found = true;
                        output.Append(c);
                    }
                }

                if (found == false)
                {
                    if (!skip)
                        output.Append(subchar);
                }
            }

            return output.ToString();
        }




        internal static string getCustomPluginSettingsFilePath(string p)
        {
            return KSPUtil.ApplicationRootPath + "GameData/" + p;
        }

        internal static string getPluginSettingsFilePath(string path_ResourceRecovery_cfg)
        {
            return KSPUtil.ApplicationRootPath + path_ResourceRecovery_cfg;
        }



        internal static ConfigNode getPluginSettingsFile(string path_ResourceRecovery_cfg)
        {
            //Debug.Log("ResourceRecovery getPluginSettingsFile path = " + path_ResourceRecovery_cfg);
            ConfigNode config = null;
            ConfigNode RRSettings;
            config = ConfigNode.Load(Utilities.getPluginSettingsFilePath(path_ResourceRecovery_cfg));
            //Debug.Log("ResourceRecovery getPluginSettingsFile config = " + config);
            if (config == null)
            {
                //Debug.Log("ResourceRecovery getPluginSettingsFile config == null ");
                return null;
            }
            else
            { 
                RRSettings = config.GetNode("RRSettings");
                //Debug.Log("ResourceRecovery getPluginSettingsFile GetNode RRSettings ");
                return RRSettings;
            }
        }

        internal static bool SavePluginSettingsFile(string p, ConfigNode config)
        {
            if(!config.Save(p))
            {
                return false;
            }
            return true;
        }

        internal static ConfigNode getCustomPluginSettingsFile(string p)
        {
            ConfigNode config = ConfigNode.Load(p);
            if (config == null)
            {
                config = new ConfigNode();
            }
            return config;
        }

        internal static ConfigNode getResourcesConfigFile(string path_cfg)
        {
            string p = Utilities.getCustomPluginSettingsFilePath(path_cfg);
            ConfigNode config = null;
            config = ConfigNode.Load(p);
            //ConfigNode[] Resources = config.GetNodes("RESOURCE_DEFINITION");
            return config;
        }
        

        internal static bool perform_resource_transfer(string buttonID, Part part, PartResource resource, string resourceSelected)
        {
            Vessel vessel = FlightGlobals.fetch.activeVessel;
            //Scenario = new ScenarioNodeDatabase();
            //Scenario = Sc;

            // get part resource data
            double part_amount = resource.amount;
            double part_maxamount = resource.maxAmount;
            double part_empty_space = part_maxamount - part_amount;
            double part_percentFull = part_amount / part_maxamount * 100.0;
            double part_1_step_value = part_maxamount / 8;
            //Debug.Log("ResourceRecovery perform_resource_transfer: part_1_step_value = " + part_1_step_value);
            //Debug.Log("ResourceRecovery perform_resource_transfer: part_amount = " + part_amount);
            //Debug.Log("ResourceRecovery perform_resource_transfer: part_maxamount = " + part_maxamount);

            // get scenario resource data
            ConfigNode res = RRDATAcontroller.get_ResourceNode(resourceSelected);
            double storage_amount = 0;
            string storage_amount_string = res.GetValue("storedamount");
            storage_amount_string = (res.GetValue("supply_mode")!="0") ? storage_amount_string : RRSettingsController.get_KSCStorage_capacity();
            double string_number1 = 0;
            if (Double.TryParse(storage_amount_string, out string_number1))
            { storage_amount = string_number1; } else { storage_amount = 0.00; }

            double storage_maxamount = 0;
            string storage_maxamount_string = res.GetValue("storagecapacity");
            double string_number2 = 0;
            if (Double.TryParse(storage_maxamount_string, out string_number2))
            { storage_maxamount = string_number2; } else { storage_maxamount = 0.00; }

            double percentFull = storage_amount / storage_maxamount * 100.0;
            double storage_percentFull = percentFull;
            //Debug.Log("ResourceRecovery perform_resource_transfer: storage_amount = " + storage_amount);

            // get requested action data
            double change_new_value=0;
            double change_new_percentFull=0;
            double change_difference;
            switch (buttonID)
            {
                case "Empty":
                     change_new_value = part_1_step_value * 0;
                    break;
                case "1/8":
                     change_new_value = part_1_step_value * 1;
                    break;
                case "1/4":
                     change_new_value = part_1_step_value * 2;
                    break;
                case "3/8":
                     change_new_value = part_1_step_value * 3;
                    break;
                case "Half":
                     change_new_value = part_1_step_value * 4;
                    break;
                case "5/8":
                     change_new_value = part_1_step_value * 5;
                    break;
                case "3/4":
                     change_new_value = part_1_step_value * 6;
                    break;
                case "7/8":
                     change_new_value = part_1_step_value * 7;
                    break;
                case "Full":
                     change_new_value = part_1_step_value * 8;
                    break;
            }
            change_new_percentFull = change_new_value / part_maxamount * 100.0;
            change_difference = change_new_value - part_amount;
            //Debug.Log("ResourceRecovery perform_resource_transfer: change_new_value - part_amount = change_difference : change_difference=" + change_difference);

            // validate actions
            storage_amount = (res.GetValue("supply_mode") != "0") ? storage_amount : change_difference;
            if (storage_amount - change_difference < 0)
            { change_difference = storage_amount; }
            if (storage_amount - change_difference < 0)
            { change_difference = storage_amount; }
            //Debug.Log("ResourceRecovery perform_resource_transfer: change_difference = " + change_difference);
            if (part_empty_space - change_difference < 0)
            { change_difference = part_empty_space; }
            //Debug.Log("ResourceRecovery perform_resource_transfer: change_difference = " + change_difference);
            

            // perform actions
            //RootNode = Scenario.get_RootNode();
            //Debug.Log("ResourceRecovery perform_resource_transfer: resource.amount = " + resource.amount);
            double new_part_amount = resource.amount + change_difference;
            new_part_amount = (part_maxamount > new_part_amount) ? new_part_amount : part_maxamount;
            new_part_amount = (new_part_amount > 0) ? new_part_amount : 0;
            resource.amount = new_part_amount;
            //Debug.Log("ResourceRecovery perform_resource_transfer: resource.amount = " + resource.amount);
            //Debug.Log("ResourceRecovery perform_resource_transfer: storage_amount = " + storage_amount);
            double new_storage_amaount = storage_amount - change_difference;
            new_storage_amaount = (new_storage_amaount > 0) ? new_storage_amaount : 0;
            //Debug.Log("ResourceRecovery perform_resource_transfer: new_storage_amaount = " + new_storage_amaount);

            if (RRDATAcontroller.modify_ResourceNode_by_ResourceName(resourceSelected, "storedamount", new_storage_amaount.ToString()))
            {
                //GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
                    return true;
            }
            //Debug.Log("perform_resource_transfer Clicked buttonID=" + buttonID + " part.partInfo.title=" + part.partInfo.title + " resource.resourceName=" + resource.resourceName + " resourceSelected=" + resourceSelected);
            return false;
            //return true;
        }
        /*
        */

        public static ConfigNode get_N_Node(ConfigNode parent, string nodeName)
        {
            if (parent == null) { return null; }
            ConfigNode Node = parent.GetNode(nodeName);
            if (Node == null) { return null; }
            return Node;
        }
        public static ConfigNode[] get_N_Nodes(ConfigNode parent, string nodesName)
        {
            if (parent == null) { return null; }
            ConfigNode[] Nodes = parent.GetNodes(nodesName);
            if (Nodes == null) { return null; }
            return Nodes;
        }

        internal static ConfigNode GetLandedVesselsByPlanetName(string planetName)
        {
            int planetID = Utilities.translate_planetName_to_planetNumber(planetName);
            //Debug.Log("ResourceRecovery get_LocationsNode running 16");
            //List<Vessel> LandedVessels = new List<Vessel>();
            ConfigNode LandedVessels = new ConfigNode("LandedVessels");
            ConfigNode persistentfile = ConfigNode.Load(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/persistent.sfs");
            ConfigNode gameconf = persistentfile.GetNode("GAME");
            ConfigNode FLIGHTSTATENode = gameconf.GetNode("FLIGHTSTATE");
            ConfigNode[] VESSELNodes = FLIGHTSTATENode.GetNodes("VESSEL");
            foreach (ConfigNode v in VESSELNodes)
            {
                string sit;
                sit = v.GetValue("sit");
                if (sit == "LANDED")
                {
                    int refValue;
                    ConfigNode ORBITNode = Utilities.get_N_Node(v, "ORBIT");
                    refValue = Convert.ToInt32(ORBITNode.GetValue("ref"));
                    if (refValue == planetID)
                    {
                        LandedVessels.AddNode(v);
                    }
                }
            }

            return LandedVessels;
        }
        /*
        */


        internal static ConfigNode CountStorageCapacityInGivenVesselNodes(ConfigNode Vessels, string moduleName)
        {
            int modules_found = 0;
            float capacity_found = 0;
            ConfigNode StorageCapacity = null;
            ConfigNode[] VESSELNodes = null;
            ConfigNode[] PARTNodes = null;
            ConfigNode[] MODULENodes = null;
            ConfigNode LOCATION = null;
            List<string> locations_array = new List<string>(new string[] { });
            List<string> locations_capacity_array = new List<string>(new string[] { });

            //ConfigNode PartConfig = PlanetCorePartConfigNode.get_PlanetCorePartConfig();
            //Debug.Log("ResourceRecovery CountStorageCapacityInGivenVesselNodes running PartConfig=" + PartConfig);


            StorageCapacity = new ConfigNode("StorageCapacity"); ;
            //Debug.Log("ResourceRecovery CountStorageCapacityInGivenVesselNodes running 1");
            //Debug.Log("ResourceRecovery CountStorageCapacityInGivenVesselNodes running 2");
            VESSELNodes = Vessels.GetNodes("VESSEL");
            //Debug.Log("ResourceRecovery CountStorageCapacityInGivenVesselNodes running 3");
            foreach (ConfigNode v in VESSELNodes)
            {
                string long_lat = v.GetValue("lon").ToString() + "/" + v.GetValue("lat").ToString();
                PARTNodes = v.GetNodes("PART");
                //Debug.Log("ResourceRecovery CountStorageCapacityInGivenVesselNodes running VESSELNodes");
                foreach (ConfigNode p in PARTNodes)
                {
                    //Debug.Log("ResourceRecovery CountStorageCapacityInGivenVesselNodes running PARTNodes");
                    MODULENodes = p.GetNodes("MODULE");
                    foreach (ConfigNode m in MODULENodes)
                    {
                        string thisModuleName = m.GetValue("name");
                        //Debug.Log("ResourceRecovery CountStorageCapacityInGivenVesselNodes running MODULENodes moduleName=" + moduleName + " thisModuleName=" + thisModuleName);
                        //Debug.Log("ResourceRecovery CountStorageCapacityInGivenVesselNodes running MODULENodes moduleName=" + moduleName + " thisModuleName=" + thisModuleName);
                        if (thisModuleName == moduleName)
                        {
                            //Debug.Log("ResourceRecovery CountStorageCapacityInGivenVesselNodes running MODULENodes moduleName found p.GetValue(modulecapacity)=" + m.GetValue("modulecapacity"));
                            locations_array.Add(long_lat);
                            locations_capacity_array.Add(m.GetValue("modulecapacity"));
                            capacity_found += Convert.ToSingle(m.GetValue("modulecapacity"));
                            modules_found++;
                        }

                    }
                }
            }
            //Debug.Log("ResourceRecovery CountStorageCapacityInGivenVesselNodes running 4 locations_array=" + locations_array);
            StorageCapacity.AddValue("modules_found", modules_found);
            //Debug.Log("ResourceRecovery CountStorageCapacityInGivenVesselNodes running 5 modules_found=" + modules_found);
            StorageCapacity.AddValue("capacity_found", capacity_found);
            //Debug.Log("ResourceRecovery CountStorageCapacityInGivenVesselNodes running 6 capacity_found=" + capacity_found);
            ConfigNode LOCATIONS = new ConfigNode("LOCATIONS");
            //Debug.Log("ResourceRecovery CountStorageCapacityInGivenVesselNodes running 7");
            int i = 0;
            foreach (string s in locations_array)
            {
                //Debug.Log("ResourceRecovery CountStorageCapacityInGivenVesselNodes running locations_array locations_capacity_array[i]=" + locations_capacity_array[i]);
                LOCATION = new ConfigNode("LOCATION");
                LOCATION.AddValue("lon_lat", s);
                LOCATION.AddValue("capacity", locations_capacity_array[i]);
                LOCATIONS.AddNode(LOCATION);
                i++;
            }
            //Debug.Log("ResourceRecovery CountStorageCapacityInGivenVesselNodes running 8");
            StorageCapacity.AddNode(LOCATIONS);
            //Debug.Log("ResourceRecovery CountStorageCapacityInGivenVesselNodes running 9");

            //Debug.Log("ResourceRecovery CountStorageCapacityInGivenVesselNodes StorageCapacity = "+StorageCapacity);

            return StorageCapacity;
        }

        internal static int get_capacity_by_planet_name(string PlanetName)
        {
            ConfigNode LV = Utilities.GetLandedVesselsByPlanetName(PlanetName);
            ConfigNode stats = Utilities.CountStorageCapacityInGivenVesselNodes(LV, RRSettingsController.get_RR_ModuleName());
            return Convert.ToInt32(stats.GetValue("capacity_found"));
        }
        /*
        */

        /*
            ResourceRecovery PResource.Resource b 0 =Sun (CelestialBody)
            ResourceRecovery PResource.Resource b 1 =Kerbin (CelestialBody)
            ResourceRecovery PResource.Resource b 2 =Mun (CelestialBody)
            ResourceRecovery PResource.Resource b 3 =Minmus (CelestialBody)
            ResourceRecovery PResource.Resource b 4 =Moho (CelestialBody)
            ResourceRecovery PResource.Resource b 5 =Eve (CelestialBody)
            ResourceRecovery PResource.Resource b 6 =Duna (CelestialBody)
            ResourceRecovery PResource.Resource b 7 =Ike (CelestialBody)
            ResourceRecovery PResource.Resource b 8 =Jool (CelestialBody)
            ResourceRecovery PResource.Resource b 9 =Laythe (CelestialBody)
            ResourceRecovery PResource.Resource b 10 =Vall (CelestialBody)
            ResourceRecovery PResource.Resource b 11 =Bop (CelestialBody)
            ResourceRecovery PResource.Resource b 12 =Tylo (CelestialBody)
            ResourceRecovery PResource.Resource b 13 =Gilly (CelestialBody)
            ResourceRecovery PResource.Resource b 14 =Pol (CelestialBody)
            ResourceRecovery PResource.Resource b 15 =Dres (CelestialBody)
            ResourceRecovery PResource.Resource b 16 =Eeloo (CelestialBody)
        */

        internal static List<string> get_cBody_List()
        {
            List<CelestialBody> cBody_List_tmp = FlightGlobals.Bodies;
            List<string> cBody_List = new List<string>(){};
            int i = 0;
            foreach (string b in cBody_List)
            {
                cBody_List.Add(b);
                //Debug.Log("ResourceRecovery PResource.Resource b " + i + " =" + b);
                i++;
            }
            return cBody_List;
        }


        internal static int translate_planetName_to_planetNumber(string planetName)
        {
            //List<string> planetNames = new List<string>(new string[] { "Kerbol", "Kerbin", "Mun", "Minmus", "Moho", "Eve", "Duna", "Ike", "Jool", "Laythe", "Vall", "Bop", "Tylo", "Gilly", "Pol", "Dres", "Eeloo" });
            List<string> planetNames = get_cBody_List();
            int i = 0;
            foreach (string p in planetNames)
            {
                if (p == planetName)
                {
                    return i;
                }
                i++;
            }
            return i;
        }

        internal static string translate_planetNumber_to_planetName(int planetNumber)
        {
            //List<string> planetNames = new List<string>(new string[] { "Kerbol", "Kerbin", "Mun", "Minmus", "Moho", "Eve", "Duna", "Ike", "Jool", "Laythe", "Vall", "Bop", "Tylo", "Gilly", "Pol", "Dres", "Eeloo" });
            List<string> planetNames = get_cBody_List();
            return planetNames[planetNumber];
        }



        internal static string get_current_planet()
        {
            string planet;
            planet = null;
            if (HighLogic.LoadedScene == GameScenes.SPACECENTER)
            {
                planet = RRSettingsController.get_KSCPlanet();
            }
            else if (HighLogic.LoadedScene == GameScenes.TRACKSTATION)
            {
                planet = RRSettingsController.get_KSCPlanet();
            }
            else if (HighLogic.LoadedScene == GameScenes.FLIGHT)
            {
                try
                {
                    //vessel = FlightGlobals.ActiveVessel;
                    //planet = vessel.mainBody.GetName();
                }
                catch
                {
                    //get_current_planet();
                }
                planet = RRSettingsController.get_KSCPlanet();
            }
            else
            {
                planet = RRSettingsController.get_KSCPlanet();
            }
            return planet;
        }

        internal static string bool_int_to_string(int boolInt, bool stringFormat = false)
        {   // ConfigDefaults
            List<string> foo = new List<string>();
            foo.Add("false");
            foo.Add("true");
            List<string> bar = new List<string>();
            bar.Add("False");
            bar.Add("True");
            if (stringFormat)
            {
                return bar[boolInt];
            }
            else
            {
                return foo[boolInt];
            }
        }

        /*
        internal static GUIStyle splitter;
        internal static GUIStyle HorizontalLine()
        {
            GUISkin skin = GUI.skin;
            splitter = new GUIStyle();
            splitter.normal.background = MakeTex(5, 5, Color.white);
            splitter.stretchWidth = true;
            splitter.margin = new RectOffset(0, 0, 0, 0);
            return splitter;
        }
        */
        
        internal static Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }

        private static List<PartResource> GetResources()
        {
            vessel = FlightGlobals.ActiveVessel;
            return new List<PartResource>(vessel.Parts.SelectMany(p => p.Resources.list).GroupBy(r => r.resourceName).Select(g => g.First()));
            /*
            List<PartResource> resources = new List<PartResource>();
            foreach (Part part in this.vessel.Parts)
            {
                foreach (PartResource resource in part.Resources)
                {
                    if (!resources.Contains(resource)) { resources.Add(resource); }
                }
            }
            return resources;
            */


        }


        internal static double get_ResourceMaxAmount(string rName)
        {
            double foo = 0.00;
            foreach (PartResource resource in GetResources())
            {
                if (resource.resourceName == rName)
                {
                    foo += resource.maxAmount;
                }
            }
            return foo;
        }
        /*
        internal static List<double> get_VesselResourceAvailableSpace(string rName, Dictionary<string, ResourceInfo> RInfo)
        {
            Dictionary<string, ResourceInfo> ResourceInfo = new Dictionary<string, ResourceInfo>();
            ResourceInfo = RInfo;
            double all_parts_max_amount = 0;
            double all_parts_amount = 0;
            double all_parts_required = 0;
            foreach (KeyValuePair<string, ResourceInfo> pair in ResourceInfo)
            {
                ResourceInfo resourceInfo = pair.Value;
                resourceInfo.pair_key = pair.Key;
                resourceInfo.isShowing = (pair.Key != rName) ? false : true;
                if (resourceInfo.isShowing)
                {
                    foreach (ResourcePartMap partInfo in resourceInfo.parts)
                    {
                        PartResource resource = partInfo.resource;
                        all_parts_max_amount += resource.maxAmount;
                        all_parts_amount += resource.amount;
                    }
                }
            }
            all_parts_required = all_parts_max_amount - all_parts_amount;
            List<double> foo = new List<double>();
            foo.Add(all_parts_max_amount);
            foo.Add(all_parts_amount);
            foo.Add(all_parts_required);
            return foo;
        }
        */
        internal static double get_ResourceAmount(string rName)
        {
            double foo = 0.00;
            foreach (PartResource resource in GetResources())
            {
                if (resource.resourceName == rName)
                {
                    foo += resource.amount;
                }
            }
            return foo;
        }
        internal static void get_ResourceList()
        {
            GUILayout.BeginVertical();
            foreach (PartResource resource in GetResources())
            {
                /*
                resource.maxAmount;
                resource.amount;
                resource.part;
                resource.isTweakable;
                resource.info;
                resource.flowMode;
                resource.flowState;
                resource.hideFlow;
                */
                GUILayout.BeginHorizontal();
                GUILayout.Label(resource.resourceName);
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }



 
    }


    /*
    * (C) Copyright 2013, Taranis Elsu
    * CC BY-NC-SA 3.0
    */

    public enum TransferDirection
    {
        NONE,
        IN,
        OUT,
        BALANCE,
        DUMP,
        LOCKED
    }
    public class ResourcePartMap
    {
        public PartResource resource;
        public Part part;
        public TransferDirection direction = TransferDirection.NONE;
        public bool isSelected = false;

        public ResourcePartMap(PartResource resource, Part part)
        {
            this.resource = resource;
            this.part = part;
        }
    }

    public class ResourceInfo
    {
        public List<ResourcePartMap> parts = new List<ResourcePartMap>();
        public bool balance = false;
        public bool isShowing = false;
        public string pair_key = null;
    }

    public class PartPercentFull
    {
        public ResourcePartMap partInfo;
        public double percentFull;

        public PartPercentFull(ResourcePartMap partInfo, double percentFull)
        {
            this.partInfo = partInfo;
            this.percentFull = percentFull;
        }
    }
}