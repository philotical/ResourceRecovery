using System;
using System.Collections.Generic;
using UnityEngine;

namespace Philotical
{

    public class ScenarioNodeDatabase
    {

        public string KSCPlanet;
        public string path_ResourceRecoverySave_cfg;
        public string path_Resources_factory_settings_cfg;
        internal ConfigNode RRSettings;
        internal ConfigNode FilePaths;

        internal List<string> global_resource_name_array = new List<string>();
        internal List<string> global_resource_storagecapacity_array = new List<string>();
        internal List<string> global_resource_storedamount_array = new List<string>();
        internal ConfigNode RootNode;
        internal ConfigNode GameNode = null;
        internal ConfigNode[] scenarios;
        internal ConfigNode RRDataNode = null;
        internal ConfigNode PlanetNode = null;
        internal ConfigNode StorageNode = null;
        internal ConfigNode LocationsNode = null;
        internal ConfigNode ResourcesNode = null;
        internal ConfigNode ResourceNode = null;
        internal int total_number_recource_tanks = 0;
        internal int total_capacity = 0;
        internal decimal total_capacity_used = 0;


        public bool save()
        {
            string PluginSaveFilePath = KSPUtil.ApplicationRootPath.Replace("KSP_Data/../", "") + "saves/" + HighLogic.SaveFolder + "/" + path_ResourceRecoverySave_cfg;
            RootNode.SetValue("lastupdate", DateTime.Now.ToString());
            if (!RootNode.Save(PluginSaveFilePath))
            {
                return false;
            }
            return true;
        }
        public ConfigNode open()
        {
            string PluginSaveFilePath = KSPUtil.ApplicationRootPath.Replace("KSP_Data/../", "") + "saves/" + HighLogic.SaveFolder + "/" + path_ResourceRecoverySave_cfg;
            //Debug.Log("ResourceRecovery getPluginSaveFile PluginSaveFilePath = " + PluginSaveFilePath);
            ConfigNode RootNode = ConfigNode.Load(PluginSaveFilePath);
            if (RootNode == null)
            {
                //Debug.Log("ResourceRecovery getPluginSaveFile config == null = " + PluginSaveFilePath);
                RootNode = new ConfigNode();
                RootNode.AddValue("writtenat", DateTime.Now.ToString());
                RootNode.AddValue("lastupdate", DateTime.Now.ToString());
                ConfigNode ResourceRecoveryData = new ConfigNode("SCENARIO");
                ResourceRecoveryData.AddValue("name", "ResourceRecoveryData");
                ResourceRecoveryData.AddValue("SaveGameKredits", "0.00");
                ConfigNode RRDataNode_tmp = RootNode.AddNode(ResourceRecoveryData);
                RootNode.Save(PluginSaveFilePath);
                //Debug.Log("ResourceRecovery getPluginSaveFile config.Save(PluginSaveFilePath) done  " + PluginSaveFilePath);
            }
            return RootNode;
        }

        public void init(ConfigNode RRSettings_ref, ConfigNode FilePaths_ref)
        {
            RRSettings = RRSettings_ref;
            FilePaths = FilePaths_ref;

            KSCPlanet = FilePaths.GetValue("KSCPlanet");
            path_ResourceRecoverySave_cfg = FilePaths.GetValue("ResourceRecoverySave_cfg");
            path_Resources_factory_settings_cfg = FilePaths.GetValue("path_Resources_factory_settings_cfg");
        }

        public bool ValidateDatabaseForPlanetaryBody()
        {
            // determine if install is allowed
            bool perform_install = true;
            string currentPlanet = Utilities.get_current_planet();
            int capacity_found = 0;
            if (currentPlanet != RRSettingsController.get_KSCPlanet() && capacity_found < 1)
            {
                perform_install = false;
            }
            if (perform_install)
            {
                if (RRDataNode == null)
                {
                    RRDataNode = get_RRScenarioNode();
                    //Debug.Log("ResourceRecovery ValidateDatabaseForPlanetaryBody perform_install RRDataNode=" + RRDataNode);
                }
                if (PlanetNode == null)
                {
                    PlanetNode = get_PlanetNode(currentPlanet);
                    //Debug.Log("ResourceRecovery ValidateDatabaseForPlanetaryBody perform_install added PlanetNode=" + RRDataNode);
                }
                if (StorageNode == null)
                {
                    StorageNode = get_StorageNode();
                    //Debug.Log("ResourceRecovery ValidateDatabaseForPlanetaryBody perform_install added StorageNode=" + RRDataNode);
                }
                if (LocationsNode == null)
                {
                    LocationsNode = get_LocationsNode();
                    //Debug.Log("ResourceRecovery ValidateDatabaseForPlanetaryBody perform_install added LocationsNode=" + RRDataNode);
                }

                if (ResourcesNode == null)
                {
                    ResourcesNode = Validate_ResourcesNode();
                    //Debug.Log("ResourceRecovery ValidateDatabaseForPlanetaryBody perform_install added ResourcesNode=" + ResourcesNode);
                }


            }
            //if (Utilities.SavePluginSaveFile(RootNode))
            if (save())
            {
                //Debug.Log("ResourceRecovery ValidateDatabaseForPlanetaryBody Utilities.SavePluginSaveFile(RootNode) worked");
                return true;
            }
            else
            {
                //Debug.Log("ResourceRecovery EXCEPTION: ValidateDatabaseForPlanetaryBody SavePluginSaveFile failed");
                return false;
            }
        }



        public ConfigNode get_RootNode()
        {
            if (RootNode != null) { return RootNode; }
            RootNode = open();
            //if (RootNode == null) { return null; }
            return RootNode;
        }

        /*
        public ConfigNode get_GameNode()
        {
            if (RootNode == null) { RootNode = get_RootNode(); }
            //GameNode = RootNode.GetNode("GAME");
            if (GameNode == null) { return null; }
            return GameNode;
        }
        */

        public ConfigNode get_N_Node(ConfigNode parent, string nodeName)
        {
            if (parent == null) { return null; }
            ConfigNode Node = parent.GetNode(nodeName);
            if (Node == null) { return null; }
            return Node;
        }
        public ConfigNode[] get_N_Nodes(ConfigNode parent, string nodesName)
        {
            if (parent == null) { return null; }
            ConfigNode[] Nodes = parent.GetNodes(nodesName);
            if (Nodes == null) { return null; }
            return Nodes;
        }

        // Parse the SFS File to get the RRData Node
        public ConfigNode get_RRScenarioNode()
        {
            //RootNode = get_RootNode();
            //Debug.Log("ResourceRecovery ScenarioNodeDatabase get_RRScenarioNode 1 ");
            RootNode = get_RootNode();
            scenarios = RootNode.GetNodes("SCENARIO");
            //Debug.Log("ResourceRecovery ScenarioNodeDatabase get_RRScenarioNode config=" + RootNode);
            foreach (ConfigNode scenario in scenarios)
            {
                if (scenario.GetValue("name") == "ResourceRecoveryData")
                {
                    RRDataNode = scenario;
                }
            }
            /*
            if (RRDataNode == null)
            {
                //Debug.Log("ResourceRecovery ScenarioNodeDatabase RRData==null");
                ConfigNode ResourceRecoveryData = new ConfigNode("SCENARIO");
                ResourceRecoveryData.AddValue("name", "ResourceRecoveryData");
                ConfigNode RRDataNode_tmp = ResourceRecoverySave.AddNode(ResourceRecoveryData);
                Debug.Log("ResourceRecovery ScenarioNodeDatabase get_RRScenarioNode ResourceRecoverySave=" + ResourceRecoverySave);
                ResourceRecoverySave.Save(path_ResourceRecoverySave_cfg);
                return get_RRScenarioNode();
            }
            */
            return RRDataNode;
        }

        // Parse the SFS File to get the PlanetNode 
        public ConfigNode get_PlanetNode(string PlanetNodeName)
        {
            //Debug.Log("ResourceRecovery get_PlanetNode RootNode Pre = : " + RootNode);
            if (RRDataNode == null) { RRDataNode = get_RRScenarioNode(); }
            PlanetNode = RRDataNode.GetNode(PlanetNodeName);
            //Debug.Log("ResourceRecovery get_PlanetNode RRDataNode Pre = : " + RRDataNode);
            if (PlanetNode == null)
            {
                //Debug.Log("ResourceRecovery get_PlanetNode PlanetNode == null : " + PlanetNodeName);
                ConfigNode AddPlanetNode = new ConfigNode(PlanetNodeName);
                AddPlanetNode.AddValue("name", PlanetNodeName);
                AddPlanetNode.AddValue("total_locations", "ToDo");
                AddPlanetNode.AddValue("total_number_recource_tanks", "ToDo");
                AddPlanetNode.AddValue("total_capacity", "ToDo");
                AddPlanetNode.AddValue("total_capacity_used", "ToDo");
                PlanetNode = RRDataNode.AddNode(AddPlanetNode);
                //Debug.Log("ResourceRecovery get_PlanetNode RRDataNode Post = : " + RRDataNode);
                PlanetNode = RRDataNode.GetNode(PlanetNodeName);
                //return get_PlanetNode(PlanetNodeName);
            }
            RootNode.Save(path_ResourceRecoverySave_cfg);
            //Debug.Log("ResourceRecovery get_PlanetNode RootNode Post = : " + RootNode);
            return PlanetNode;
        }

        // Parse the SFS File to get the StorageNode 
        public ConfigNode get_StorageNode()
        {
            StorageNode = PlanetNode.GetNode("STORAGE");
            if (StorageNode == null)
            {
                //Debug.Log("ResourceRecovery get_StorageNode StorageNode==null : StorageNode=" + StorageNode);
                ConfigNode AddStorageNode = new ConfigNode("STORAGE");
                if (PlanetNode.GetValue("name") == RRSettingsController.get_KSCPlanet())
                {
                    AddStorageNode.AddValue("capacity", RRSettingsController.get_KSCStorage_capacity());
                }
                else
                {
                    AddStorageNode.AddValue("capacity", "ToDo");
                }
                AddStorageNode.AddValue("modulecount", "ToDo");
                StorageNode = PlanetNode.AddNode(AddStorageNode);
                StorageNode = PlanetNode.GetNode("STORAGE");
                //return get_StorageNode();
            }
            else
            {
                StorageNode = PlanetNode.GetNode("STORAGE");
                Debug.Log("ResourceRecovery get_StorageNode running PlanetNode.GetValue(name)=" + PlanetNode.GetValue("name"));
                Debug.Log("ResourceRecovery get_StorageNode running RRSettingsController.get_KSCPlanet()=" + RRSettingsController.get_KSCPlanet());
                Debug.Log("ResourceRecovery get_StorageNode running PlanetNode.GetValue(name)=" + PlanetNode.GetValue("name"));
                //Debug.Log("ResourceRecovery get_StorageNode running RRSettingsController.get_KSCPlanet()=" + RRSettingsController.get_KSCPlanet());
                if (PlanetNode.GetValue("name") == RRSettingsController.get_KSCPlanet())
                {
                    StorageNode.SetValue("capacity", RRSettingsController.get_KSCStorage_capacity());
                }
                else
                {
                    StorageNode.SetValue("capacity", "ToDo");
                }
                StorageNode.SetValue("modulecount", "ToDo");

            }
            //RootNode.Save(path_ResourceRecoverySave_cfg);
            return StorageNode;
        }
        public bool remove_StorageNode()
        {
            if (PlanetNode == null) { return false; }
            PlanetNode.RemoveNode("STORAGE");
            return true;
        }
        public bool update_StorageTotals()
        {
            total_capacity_used = 0;
            total_number_recource_tanks = 0;
            foreach (ConfigNode n in ResourcesNode.GetNodes("RESOURCE"))
            {
                string st_am = n.GetValue("storedamount");
                try
                {
                    decimal myParsedInt = Convert.ToDecimal(st_am);
                    total_capacity_used += myParsedInt;
                }
                catch
                {
                    total_capacity_used += 0;
                }
                total_number_recource_tanks += 1;
                //Debug.Log("ResourceRecovery ValidateDatabaseForPlanetaryBody running 17");
                PlanetNode.SetValue("total_capacity_used", total_capacity_used.ToString());
                PlanetNode.SetValue("total_number_recource_tanks", total_number_recource_tanks.ToString());
            }

            return true;
        }

        //*
        // Parse the SFS File to get the LocationsNode 
        public ConfigNode get_LocationsNode()
        {
            ConfigNode stats = new ConfigNode();
            ConfigNode LV = new ConfigNode();
            string currentPlanet = Utilities.get_current_planet();
            string KSCPlanet = RRSettingsController.get_KSCPlanet();

            StorageNode = get_StorageNode();

            StorageNode.RemoveNodes("LOCATIONS");
            LocationsNode = StorageNode.GetNode("LOCATIONS");
            if (LocationsNode == null)
            {
                //Debug.Log("ResourceRecovery get_LocationsNode LocationsNode == null");
                LV = Utilities.GetLandedVesselsByPlanetName(currentPlanet);
                //Debug.Log("ResourceRecovery get_LOCATIONSNode() running LV=" + LV);
                stats = Utilities.CountStorageCapacityInGivenVesselNodes(LV, RRSettingsController.get_RR_ModuleName());

                ConfigNode LNode = stats.GetNode("LOCATIONS");
                ConfigNode[] LNodes = stats.GetNodes("LOCATION");
                List<string> LList = new List<string>(new string[] { });
                List<string> LCList = new List<string>(new string[] { });

                int location_count = 0;
                if (currentPlanet == KSCPlanet)
                {
                    //Debug.Log("ResourceRecovery ValidateDatabaseForPlanetaryBody running 15");
                    LList.Add(RRSettingsController.get_KSCStorage_Lon_Lat());
                    LCList.Add(RRSettingsController.get_KSCStorage_capacity());
                    location_count++;
                }
                //Debug.Log("ResourceRecovery ValidateDatabaseForPlanetaryBody running 16");
                foreach (ConfigNode n in LNodes)
                {
                    //Debug.Log("ResourceRecovery ValidateDatabaseForPlanetaryBody running 17");
                    LList.Add(n.GetValue("lon_lat"));
                    LCList.Add(n.GetValue("capacity"));
                    location_count++;
                }

                ConfigNode AddLocationsNode = new ConfigNode("LOCATIONS");
                int module_count = 0;
                int capacity_found = 0;
                foreach (string s in LList)
                {
                    ConfigNode AddLocationNode = new ConfigNode("LOCATION");
                    AddLocationNode.AddValue("lon_lat", s);
                    AddLocationNode.AddValue("capacity", LCList[module_count]);
                    AddLocationsNode.AddNode(AddLocationNode);
                    int string_number = 0;
                    if (Int32.TryParse(LCList[module_count], out string_number))
                    {
                        Console.WriteLine("Converted '{0}' to {1}.", LCList[module_count], string_number);
                        capacity_found += string_number;
                    }
                    module_count++;
                }

                //Update the StorageNode node with the totals:
                //Update the PlanetNode node with the totals:
                if (currentPlanet == KSCPlanet)
                {
                    StorageNode.SetValue("capacity", RRSettingsController.get_KSCStorage_capacity() + " (" + capacity_found + ")");
                    //location_count++;
                    PlanetNode.SetValue("total_locations", location_count.ToString());
                }
                else
                {
                    StorageNode.SetValue("capacity", capacity_found.ToString());
                    PlanetNode.SetValue("total_locations", location_count.ToString());
                }
                StorageNode.SetValue("modulecount", module_count.ToString());


                StorageNode = StorageNode.AddNode(AddLocationsNode);
                LocationsNode = PlanetNode.GetNode("LOCATIONS");
                //return get_LocationsNode();
            }
            return LocationsNode;
        }
        //*/

        // Parse the SFS File to get tha ResourcesNode 
        public ConfigNode Validate_ResourcesNode()
        {
            //Debug.Log("ResourceRecovery: ScenarioNodeDatabase: beginn Validate_ResourcesNode 1 ");
            LocationsNode = get_LocationsNode();
            //Debug.Log("ResourceRecovery: ScenarioNodeDatabase:  Validate_ResourcesNode 2");
            ResourcesNode = get_ResourcesNode();
            //Debug.Log("ResourceRecovery: ScenarioNodeDatabase:  Validate_ResourcesNode 3");
            get_factory_settings();
            //Debug.Log("ResourceRecovery: ScenarioNodeDatabase:  Validate_ResourcesNode 4");

            /*
            get_KSPIResources();
            get_KethaneResources();
            get_OSRResources();
            get_KSPResources();
            */

            PlanetNode.SetValue("total_number_recource_tanks", total_number_recource_tanks.ToString());
            //Debug.Log("ResourceRecovery: ScenarioNodeDatabase:  Validate_ResourcesNode 5");
            PlanetNode.SetValue("total_capacity_used", total_capacity_used.ToString());
            //Debug.Log("ResourceRecovery: ScenarioNodeDatabase:  Validate_ResourcesNode 6");
            if (PlanetNode.GetValue("name") == RRSettingsController.get_KSCPlanet())
            {
                PlanetNode.SetValue("total_capacity", RRSettingsController.get_KSCStorage_capacity());
            }
            else
            {
                PlanetNode.SetValue("total_capacity", total_capacity.ToString());
            }
            //Debug.Log("ResourceRecovery: ScenarioNodeDatabase:  Validate_ResourcesNode 7");

            return StorageNode.GetNode("RESOURCES");
        }

        // Parse the SFS File to get tha ResourcesNode 
        public ConfigNode get_ResourcesNode()
        {
            StorageNode = PlanetNode.GetNode("STORAGE");
            //Debug.Log("ResourceRecovery: ScenarioNodeDatabase: beginn get_ResourcesNode  StorageNode=" + StorageNode);
            ResourcesNode = StorageNode.GetNode("RESOURCES");
            //Debug.Log("ResourceRecovery: ScenarioNodeDatabase: after LocationsNode.GetNode  ResourcesNode=" + ResourcesNode);
            if (ResourcesNode == null)
            {
                ConfigNode AddResourcesNode = new ConfigNode("RESOURCES");
                AddResourcesNode = StorageNode.AddNode(AddResourcesNode);
                ResourcesNode = StorageNode.GetNode("RESOURCES");
                return get_ResourcesNode();
            }
            return ResourcesNode;
        }





        // Parse the SFS File to get the ResourceNode 
        public ConfigNode get_ResourceNode(string ResourceNodeName)
        {
            ResourcesNode = get_ResourcesNode();
            if (ResourcesNode == null) { return null; }
            ConfigNode[] RESOURCE = ResourcesNode.GetNodes("RESOURCE");
            foreach (ConfigNode res in RESOURCE)
            {
                if (res == null)
                {
                    continue;
                }
                if (res.GetValue("name") == ResourceNodeName)
                {
                    return res;
                }
            }
            return null;
        }


        // Parse the SFS File tot install and ge the ResourceNode 
        public ConfigNode get_ResourceNode(string ResourceNodeName, ConfigNode FactorySettings)
        {
            ResourcesNode = get_ResourcesNode();
            if (ResourcesNode == null) { return null; }
            //Debug.Log("ResourceRecovery ScenarioNodeDatabase get_ResourceNode() PRE ResourceNodeName=" + ResourceNodeName);
            //Debug.Log("ResourceRecovery ScenarioNodeDatabase get_ResourceNode() PRE ResourcesNode=" + ResourcesNode);
            ConfigNode[] RESOURCE = ResourcesNode.GetNodes("RESOURCE");
            ResourceNode = null;
            foreach (ConfigNode res in RESOURCE)
            {
                if (res == null)
                {
                    continue;
                }
                if (res.GetValue("name") == ResourceNodeName)
                {
                    ResourceNode = res;
                    //Debug.Log("ResourceRecovery ScenarioNodeDatabase get_ResourceNode() res.GetValue(name) ==  ResourceNodeName=" + ResourceNodeName);
                    continue;
                }
            }
            //ResourceNode = ResourcesNode.GetNode(ResourceNodeName);
            if (ResourceNode == null)
            {

                //Debug.Log("ResourceRecovery ScenarioNodeDatabase get_ResourceNode() ResourceNode == null - ResourceNodeName=" + ResourceNodeName);
                ConfigNode AddResourceNode = new ConfigNode("RESOURCE");
                AddResourceNode.AddValue("resourceID", total_number_recource_tanks.ToString());
                AddResourceNode.AddValue("name", ResourceNodeName);
                // 6000000 = one orange Tank full of LiquidFuel
                double cap = 6000000;
                string currentPlanet = Utilities.get_current_planet();
                string KSCPlanet = RRSettingsController.get_KSCPlanet();
                if (currentPlanet == KSCPlanet)
                {
                    AddResourceNode.AddValue("storagecapacity", RRSettingsController.get_KSCStorage_capacity());
                }
                else
                {
                    double string_number = 0;
                    Double.TryParse(FactorySettings.GetValue("density"), out string_number);
                    cap = cap / string_number;
                    AddResourceNode.AddValue("storagecapacity", cap);
                }
                AddResourceNode.AddValue("storedamount", "0.00");
                if (currentPlanet == KSCPlanet)
                {
                    AddResourceNode.AddValue("supply_mode", FactorySettings.GetValue("supply_mode"));
                }
                else
                {
                    AddResourceNode.AddValue("supply_mode", "1");
                }
                AddResourceNode.AddValue("value_factor", FactorySettings.GetValue("value_factor"));
                AddResourceNode.AddValue("total_sold", "0");
                AddResourceNode.AddValue("total_bought", "0");
                //string foo = (total_number_recource_tanks < 10) ? "0" + total_number_recource_tanks.ToString() : total_number_recource_tanks.ToString();
                AddResourceNode.AddValue("display_order", FactorySettings.GetValue("display_order"));
                AddResourceNode.AddValue("hide", FactorySettings.GetValue("hide"));
                AddResourceNode.AddValue("group", FactorySettings.GetValue("group"));
                ConfigNode AddResourceNode_tmp = ResourcesNode.AddNode(AddResourceNode);
                return get_ResourceNode(ResourceNodeName, FactorySettings);
            }
            //Debug.Log("ResourceRecovery ScenarioNodeDatabase get_ResourceNode() POST ResourcesNode=" + ResourcesNode);
            return ResourceNode;
        }
        public bool modify_ResourceNode_by_ResourceName(string SanitizedResourceNodeName, string valueName, string newvalue)
        {
            //ToDo
            //Debug.Log("ResourceRecovery ScenarioNodeDatabase modify_ResourceNode_by_ResourceName running");
            ResourceNode = null;
            ConfigNode[] RESOURCE = ResourcesNode.GetNodes("RESOURCE");
            foreach (ConfigNode res in RESOURCE)
            {
                if (res == null)
                {
                    continue;
                }
                if (res.GetValue("name") == SanitizedResourceNodeName)
                {
                    ResourceNode = res;
                    //Debug.Log("ResourceRecovery ScenarioNodeDatabase modify_ResourceNode_by_ResourceName()  res.GetValue(name)=" + res.GetValue("name") + " + valueName=" + valueName + " + newvalue=" + newvalue);
                    continue;
                }
            }
            if (ResourceNode.SetValue(valueName, newvalue))
            {
                if (save())
                {
                    //Debug.Log("ResourceRecovery modify_ResourceNode_by_ResourceName Utilities.SavePluginSaveFile(RootNode) worked");
                    return true;
                }
                else
                {
                    //Debug.Log("ResourceRecovery EXCEPTION: modify_ResourceNode_by_ResourceName SavePluginSaveFile failed");
                    return false;
                }
            }
            return false;
        }
        public bool modify_ResourceNode_by_attribut_name_and_value(string valueName, string oldvalue, string newvalue)
        {
            //ToDo
            //Debug.Log("ResourceRecovery ScenarioNodeDatabase modify_ResourceNode_by_attribut_name_and_value  TODO");
            ResourceNode = null;
            ConfigNode[] RESOURCE = ResourcesNode.GetNodes("RESOURCE");
            foreach (ConfigNode res in RESOURCE)
            {
                if (res == null)
                {
                    continue;
                }
                if (res.GetValue(valueName) == oldvalue)
                {
                    ResourceNode = res;
                    //Debug.Log("ResourceRecovery ScenarioNodeDatabase modify_ResourceNode_by_attribut_name_and_value()  res.GetValue(name)=" + res.GetValue("name") + " + valueName=" + valueName + " + oldvalue=" + oldvalue + " + newvalue=" + newvalue);
                    continue;
                }
            }
            if (ResourceNode.SetValue(valueName, newvalue))
            {
                if (save())
                {
                    //Debug.Log("ResourceRecovery modify_ResourceNode_by_attribut_name_and_value Utilities.SavePluginSaveFile(RootNode) worked");
                    return true;
                }
                else
                {
                    //Debug.Log("ResourceRecovery EXCEPTION: modify_ResourceNode_by_attribut_name_and_value SavePluginSaveFile failed");
                    return false;
                }
            }
            return false;
        }
        public bool remove_ResourceNode(string SanitizedResourceNodeName)
        {
            //ToDo
            Debug.Log("ResourceRecovery ScenarioNodeDatabase remove_ResourceNode  TODO :" + SanitizedResourceNodeName);
            return false;
            /*
            if (ResourcesNode == null) { return false; }
            ResourcesNode.RemoveNode(ResourceNodeName);
            return true;
            */
        }




        public List<string> get_factory_settings()
        {
            /*
            RESOURCE_FACTORY_SETTINGS
            {
		            // Squad Resources
	            RESOURCE
	            {
	              name = LiquidFuel
	              density = 0.005
			            supply_mode = 0
			            value_factor = 1
			            display_order = 01
			            hide = 0
			            group = fuel
	            }
			*/
            ResourcesNode = get_ResourcesNode();
            if (ResourcesNode == null) { return null; }
            ConfigNode[] RESOURCE = ResourcesNode.GetNodes("RESOURCE");
            //Debug.Log("ResourceRecovery: get_ResourcesNode ResourcesNode=" + ResourcesNode);
            //Debug.Log("ResourceRecovery: get_ResourcesNode RESOURCE=" + RESOURCE);
            List<ResourceNodeType> RNodesList = new List<ResourceNodeType>();
            List<string> RNodesNamesList = new List<string>();
            foreach (ConfigNode node in RESOURCE)
            {
                RNodesNamesList.Add(node.GetValue("name"));
                //Debug.Log("ResourceRecovery get_factory_settings: add to RNodesNamesList = " + node.GetValue("name"));
            }

            //Debug.Log("ResourceRecovery get_factory_settings: check for factory_settings");
            ConfigNode Factory_Resources = Utilities.getResourcesConfigFile(path_Resources_factory_settings_cfg);
            //Debug.Log("ResourceRecovery get_factory_settings: path_Resources_factory_settings_cfg=" + path_Resources_factory_settings_cfg);
            if (Factory_Resources != null)
            {
                //Debug.Log("ResourceRecovery get_factory_settings: get_factory_settings found");
                ConfigNode RESOURCE_FACTORY_SETTINGS = Factory_Resources.GetNode("RESOURCE_FACTORY_SETTINGS");
                ConfigNode[] RESOURCEs = RESOURCE_FACTORY_SETTINGS.GetNodes("RESOURCE");
                ConfigNode resource;
                string resourceName;
                int i = 0;
                foreach (ConfigNode node in RESOURCEs)
                {
                    string s = node.GetValue("name");
                    resourceName = Utilities.Sanitize(s, false);
                    if (!RNodesNamesList.Contains(resourceName))
                    {
                        //Debug.Log("ResourceRecovery get_factory_settings: !RNodesNamesList.Contains(s) s=" + s);
                        resource = null;
                        resource = get_ResourceNode(resourceName, node);//ResourcesNode.GetNode(resourceName);
                        global_resource_name_array.Add(resourceName);
                        global_resource_storagecapacity_array.Add(RRSettingsController.get_KSCStorage_capacity());
                        global_resource_storedamount_array.Add("0.00");
                        string st_am = resource.GetValue("storedamount");
                        try
                        {
                            decimal myParsedInt = Convert.ToDecimal(st_am);
                            total_capacity_used += myParsedInt;
                        }
                        catch
                        {
                            total_capacity_used += 0;
                        }
                        total_number_recource_tanks += 1;
                        total_capacity += 0;//ToDo
                        i++;
                    }

                }
                //Debug.Log("ResourceRecovery SetUpAndMaintainConfigNodeDatabase: KSPI_Resources installed");
            }
            return global_resource_name_array;
        }


 
    }

    public class ResourceNodeType : IEquatable<ResourceNodeType>
    {
        /*
        RESOURCE
        {
            name = Antimatter
            storagecapacity = unlimited 
            storedamount = 10.00
            supply_mode = 1
            value_factor = 1
            display_order = 3
            hide = 0
            group = none
        }
        */
        public string name { get; set; }
        public int resourceID { get; set; }
        public string storagecapacity { get; set; }
        public string storedamount { get; set; }
        public string supply_mode { get; set; }
        public string value_factor { get; set; }
        public string display_order { get; set; }
        public string hide { get; set; }
        public string group { get; set; }
        /*
        public int GetValue(string v)
        {
            int newV = 0;
            switch (v)
            {
                case "resourceID":
                    newV = this.resourceID;
                    break;
                case "display_order":
                    newV = this.display_order;
                    break;
            }
            return newV;
        }
        public string GetValue(string v)
        {
            string newV = null;
            switch (v)
            {
                case "name":
                    newV = this.name;
                    break;
                case "storedamount":
                    newV = this.storedamount;
                    break;
                case "supply_mode":
                    newV = this.supply_mode;
                    break;
                case "value_factor":
                    newV = this.value_factor;
                    break;
                case "hide":
                    newV = this.hide;
                    break;
                case "group":
                    newV = this.group;
                    break;
            }
            return newV;

        }
        */
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            ResourceNodeType objAsResourceNodeType = obj as ResourceNodeType;
            if (objAsResourceNodeType == null) return false;
            else return Equals(objAsResourceNodeType);
        }
        public override int GetHashCode()
        {
            return resourceID;
        }
        public bool Equals(ResourceNodeType other)
        {
            if (other == null) return false;
            return (this.resourceID.Equals(other.resourceID));
        }
    }
}
















































































