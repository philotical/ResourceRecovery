using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Philotical
{


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
	


    class RRDATAcontroller
    {

        public static string path_Resources_factory_settings_cfg;
        public static string KSCPlanet;

        internal static void init(ConfigNode sub_root)
        {
            //Debug.Log("ResourceRecovery RRDATAcontroller init begin");

            path_Resources_factory_settings_cfg = RRSettingsController.FilePaths.GetValue("path_Resources_factory_settings_cfg");
            //Debug.Log("ResourceRecovery RRDATAcontroller init 1");


            string p = Utilities.get_current_planet();
            //Debug.Log("ResourceRecovery RRDATAcontroller init 2");
            KSCPlanet = RRSettingsController.get_KSCPlanet();
            //Debug.Log("ResourceRecovery RRDATAcontroller init 3");

            get_PlanetNode(p);
            //Debug.Log("ResourceRecovery RRDATAcontroller init 4");
            if (sub_root.HasNode("PLANET"))
            {
                //Debug.Log("ResourceRecovery RRDATAcontroller init sub_root.HasNode(PLANET)");
                // do stuff

                get_StorageNode(p);
                get_ResourcesNode(p);
                ConfigNode Factory_Resources = Utilities.getResourcesConfigFile(path_Resources_factory_settings_cfg);
                //Debug.Log("ResourceRecovery get_factory_settings: path_Resources_factory_settings_cfg=" + path_Resources_factory_settings_cfg);
                if (Factory_Resources != null)
                {
                    //Debug.Log("ResourceRecovery init get_factory_settings: factory_settings found");
                    ConfigNode RESOURCE_FACTORY_SETTINGS = Factory_Resources.GetNode("RESOURCE_FACTORY_SETTINGS");
                    ConfigNode[] RESOURCEs = RESOURCE_FACTORY_SETTINGS.GetNodes("RESOURCE");
                    //Debug.Log("ResourceRecovery init get_factory_settings:  RESOURCEs=" + RESOURCEs);
                    string resourceName;
                    int i = 0;
                    foreach (ConfigNode node in RESOURCEs)
                    {
                        string s = node.GetValue("name");
                        //Debug.Log("ResourceRecovery init get_factory_settings:  s=" + s);
                        resourceName = Utilities.Sanitize(s, false);
                        get_ResourceNode(p, resourceName,node);
                        i++;
                    }
                    ValueSet(get_PlanetNode(p), "total_number_recource_tanks", i.ToString());
                    ValueSet(get_PlanetNode(p), "total_capacity", "ToDo");
                }
            
                get_LocationsNode(p);
            
            }
            else 
            {
                Debug.Log("ResourceRecovery RRDATAcontroller init EXCEPTION !sub_root.HasNode(PLANET)");
            }


            //Debug.Log("ResourceRecovery RRDATAcontroller init sub_root=" + sub_root);



            //Debug.Log("ResourceRecovery init end sub_root=" + sub_root);
            // store modified sub_root for later use
            SubRootNode.set_sub_root(sub_root);
        }

        internal static ConfigNode get_all_planets()
        {
            return SubRootNode.get_sub_root();
        }

        internal static ConfigNode get_planet(string pN)
        {
            return NodeGet(get_all_planets(), "PLANET", pN);
            /*
            foreach (ConfigNode node in SubRootNode.get_root().GetNodes("PLANET"))
            {
                if(node.GetValue("name")==pN)
                {
                    return node;
                }
            }
           */
        }


        /* region contains:
         ConfigNode NodeGet(ConfigNode parent, string nodeName, string Value_nameAttribut = null)
         string ValueGet(ConfigNode parent, string valueName)
         bool NodeValueSet(ConfigNode parent, string[] node, string[] Value)
         bool ValueSet(ConfigNode parent, string valueName, string newValue)
         */
        #region tools
        public static ConfigNode NodeGet(ConfigNode parent, string nodeName, string Value_nameAttribut = null)
        {
            //Debug.Log("ResourceRecovery NodeGet nodeName=" + nodeName + " Value_nameAttribut=" + Value_nameAttribut + " parent=" + parent);
            ConfigNode[] result1;
            ConfigNode result2;
            result1 = null;
            result2 = null;
            if (Value_nameAttribut != null)
            {
                result1 = parent.GetNodes(nodeName);
                foreach (ConfigNode check2 in result1)
                {
                    if (check2.GetValue("name") == Value_nameAttribut)
                    {
                        result2 = check2;
                        //Debug.Log("ResourceRecovery NodeGet running 1 result2=" + result2);
                    }
                }
            }
            else
            {
                result2 = parent.GetNode(nodeName);
                //Debug.Log("ResourceRecovery NodeGet running 2 result2=" + result2);
            }
            
            if (result2 == null)
            {
                result2 = new ConfigNode(nodeName);
                if (Value_nameAttribut != null)
                {
                    result2.AddValue("name", Value_nameAttribut);
                }
                parent.AddNode(result2);

                //Debug.Log("ResourceRecovery NodeGet done  Make_new and restarting result2=" + result2);
                return NodeGet(parent,  nodeName,  Value_nameAttribut);
            }
            else 
            {
                if (result2.GetValue("name") == null && Value_nameAttribut != null)
                {
                    result2.AddValue("name", Value_nameAttribut);
                }
                //Debug.Log("ResourceRecovery NodeGet done Value_nameAttribut=" + Value_nameAttribut + " found result2=" + result2);
                return result2;
            }

        }
        public static string ValueGet(ConfigNode parent, string valueName)
        {
            //Debug.Log("ResourceRecovery ValueGet valueName=" + valueName + " parent.GetValue(valueName)=" + parent.GetValue(valueName) + " parent=" + parent); 
            return parent.GetValue(valueName);
        }


        public static bool NodeValueSet(ConfigNode parent, string[] node, string[] Value)
        {
            //Debug.Log("ResourceRecovery NodeValueSet start parent=" + parent);
            /*
              usage:
                 ConfigNode parent = new ConfigNode;
                 string[] node = new string[] {"nodeName","Value_nameAttribut"};
                 string[] value = new string[] {"valueName","newValue"};
                 NodeValueSet(parent, node, value);
              */
            string nodeName = (node[0] != "" && node[0] != null) ? node[0] : null;
            string nodeNameAttribut = null;
            if (node.Length > 1)
            {
                nodeNameAttribut = (node[1] != "" && node[1] != null) ? node[1] : null;
            }
            if (nodeName != null)
            {
                string valueName = (Value[0] != "" && Value[0] != null) ? Value[0] : null;
                string newValue = (Value[1] != "" && Value[1] != null) ? Value[1] : null;
                //Debug.Log("ResourceRecovery NodeValueSet nodeName=" + nodeName + " nodeNameAttribut=" + nodeNameAttribut + " valueName=" + valueName + " newValue=" + newValue + " parent=" + parent);
                if (valueName != null)
                {
                    ConfigNode oldNode = NodeGet(parent, nodeName, nodeNameAttribut);
                    if (oldNode != null)
                    {
                        return ValueSet(oldNode, valueName, newValue);
                    }
                }


            }
            return false;
        }

        public static bool ValueSet(ConfigNode parent, string valueName, string newValue)
        {
            //Debug.Log("ResourceRecovery ValueSet start ValueName=" + valueName + " newValue=" + newValue + " parent=" + parent);
            if (parent.HasValue(valueName))
            {
                parent.SetValue(valueName, newValue);
                //Debug.Log("ResourceRecovery ValueSet 1 new parent=" + parent);
                return true;
            }
            else
            {
                parent.AddValue(valueName, newValue);
                parent.SetValue(valueName, newValue);
                //Debug.Log("ResourceRecovery ValueSet 2 new parent=" + parent);
                return true;
            }
        }

        public static bool ValueCreate(ConfigNode parent, string valueName, string newValue)
        {
            //Debug.Log("ResourceRecovery ValueCreate start ValueName=" + valueName + " newValue=" + newValue + " parent=" + parent);
            if (!parent.HasValue(valueName))
            {
                parent.AddValue(valueName, newValue);
                parent.SetValue(valueName, newValue);
                //Debug.Log("ResourceRecovery ValueCreate 2 new parent=" + parent);
                return true;
            }
            return true;
        }
        public static bool NodeCreate(ConfigNode parent, string nodeName)
        {
            //Debug.Log("ResourceRecovery NodeCreate start nodeName=" + nodeName + " parent=" + parent);
            if (!parent.HasNode(nodeName))
            {
                parent.AddNode(nodeName);
                //Debug.Log("ResourceRecovery NodeCreate 2 new parent=" + parent);
                return true;
            }
            return true;
        }
        #endregion

        
        
        // get the PlanetNode (create if needed)
        public static ConfigNode get_PlanetNode(string PlanetNodeName)
        {
            //Debug.Log("ResourceRecovery get_PlanetNode PlanetNodeName : " + PlanetNodeName);
            ConfigNode PlanetNode = NodeGet(get_all_planets(), "PLANET", PlanetNodeName);
            ValueCreate(PlanetNode, "total_locations", "ToDo");
            ValueCreate(PlanetNode, "total_number_recource_tanks", "ToDo");
            ValueCreate(PlanetNode, "total_capacity", "ToDo");
            ValueCreate(PlanetNode, "total_capacity_used", "ToDo");
            //Debug.Log("ResourceRecovery get_PlanetNode RootNode Post = : " + RootNode);
            return PlanetNode;
        }
        // get the StorageNode (create if needed)
        public static ConfigNode get_StorageNode(string PlanetNodeName)
        {
            //Debug.Log("ResourceRecovery get_StorageNode ");
            ConfigNode StorageNode = NodeGet(get_PlanetNode(PlanetNodeName), "STORAGE");
            ValueCreate(StorageNode, "storagecapacity", "ToDo");
            ValueCreate(StorageNode, "modulecount", "ToDo");
            return StorageNode;
         }
        // get the ResourcesNode (create if needed)
        public static ConfigNode get_ResourcesNode(string PlanetNodeName)
        {
            //Debug.Log("ResourceRecovery get_ResourcesNode ");
            ConfigNode ResourcesNode = NodeGet(get_StorageNode(PlanetNodeName), "RESOURCES");
            return ResourcesNode;
        }

        // get the ResourceNode (create if needed)
        public static ConfigNode get_ResourceNode(string resourceName)
        {
            ConfigNode ResourceNode = NodeGet(get_ResourcesNode(Utilities.get_current_planet()), "RESOURCE", resourceName);
            return ResourceNode;
        }

        // get the ResourceNode (create if needed)
        public static ConfigNode get_ResourceNode(string PlanetNodeName, string resourceName, ConfigNode FactorySettings)
        {
            //Debug.Log("ResourceRecovery get_ResourceNode beginn resourceName=" + resourceName);
            ConfigNode ResourceNode = NodeGet(get_ResourcesNode(PlanetNodeName), "RESOURCE", resourceName);
            //ResourceNode = NodeGet(get_ResourcesNode(PlanetNodeName), "RESOURCE", resourceName);
            ValueCreate(ResourceNode, "resourceID", FactorySettings.GetValue("display_order"));
            ValueCreate(ResourceNode, "storagecapacity", "ToDo");
            ValueCreate(ResourceNode, "storedamount", "0.00");
            ValueCreate(ResourceNode, "density", FactorySettings.GetValue("density"));
            ValueCreate(ResourceNode, "supply_mode", FactorySettings.GetValue("supply_mode"));
            ValueCreate(ResourceNode, "value_factor", FactorySettings.GetValue("value_factor"));
            ValueCreate(ResourceNode, "total_sold", "0");
            ValueCreate(ResourceNode, "total_bought", "0");
            ValueCreate(ResourceNode, "display_order", FactorySettings.GetValue("display_order"));
            ValueCreate(ResourceNode, "hide", FactorySettings.GetValue("hide"));
            ValueCreate(ResourceNode, "group", FactorySettings.GetValue("group"));
            //Debug.Log("ResourceRecovery get_ResourceNode end ResourceNode=" + ResourceNode);
            return ResourceNode;
        }
        // get the LocationsNode (create if needed)
        public static ConfigNode get_LocationsNode(string PlanetNodeName)
        {
            ConfigNode stats = new ConfigNode();
            ConfigNode LV = new ConfigNode();
            string KSCPlanet = RRSettingsController.get_KSCPlanet();

            //Debug.Log("ResourceRecovery get_LocationsNode beginn");
            //ConfigNode LocationsNode = NodeGet(get_StorageNode(PlanetNodeName), "LOCATIONS");

            ConfigNode StorageNode = get_StorageNode(PlanetNodeName);

            StorageNode.RemoveNodes("LOCATIONS");
            ConfigNode LocationsNode = StorageNode.GetNode("LOCATIONS");
            if (LocationsNode == null)
            {
                //Debug.Log("ResourceRecovery get_LocationsNode LocationsNode == null");
                LV = Utilities.GetLandedVesselsByPlanetName(PlanetNodeName);
                //Debug.Log("ResourceRecovery get_LOCATIONSNode() running LV=" + LV + " PartModuleName=" + RRSettingsController.get_RR_ModuleName());
                stats = Utilities.CountStorageCapacityInGivenVesselNodes(LV, RRSettingsController.get_RR_ModuleName());
                //Debug.Log("ResourceRecovery get_LOCATIONSNode() running stats=" + stats);

                ConfigNode LNode = stats.GetNode("LOCATIONS");
                ConfigNode[] LNodes = LNode.GetNodes("LOCATION");
                List<string> LList = new List<string>(new string[] { });
                List<string> LCList = new List<string>(new string[] { });
                //Debug.Log("ResourceRecovery get_LocationsNode running 14 stats=" + stats);
                //Debug.Log("ResourceRecovery get_LocationsNode running 14 LNode=" + LNode);
                //Debug.Log("ResourceRecovery get_LocationsNode running 14 LNodes=" + LNodes);

                int location_count = 0;
                ConfigNode AddLocationsNode = new ConfigNode("LOCATIONS");
                if (PlanetNodeName == KSCPlanet)
                {
                    //Debug.Log("ResourceRecovery get_LocationsNode running 15");
                    ConfigNode AddLocationNode = new ConfigNode("LOCATION");
                    AddLocationNode.AddValue("lon_lat", RRSettingsController.get_KSCStorage_Lon_Lat());
                    AddLocationNode.AddValue("capacity", RRSettingsController.get_KSCStorage_capacity());
                    AddLocationsNode.AddNode(AddLocationNode);
                    LList.Add(RRSettingsController.get_KSCStorage_Lon_Lat());
                    LCList.Add(RRSettingsController.get_KSCStorage_capacity());
                    location_count++;
                }
                foreach (ConfigNode n in LNodes)
                {
                    //Debug.Log("ResourceRecovery get_LocationsNode running 17 n.GetValue(capacity)=" + n.GetValue("capacity") + " n.GetValue(lon_lat)=" + n.GetValue("lon_lat"));
                    ConfigNode AddLocationNode = new ConfigNode("LOCATION");
                    AddLocationNode.AddValue("lon_lat", n.GetValue("lon_lat"));
                    AddLocationNode.AddValue("capacity", n.GetValue("capacity"));
                    AddLocationsNode.AddNode(AddLocationNode);
                    LList.Add(n.GetValue("lon_lat"));
                    LCList.Add(n.GetValue("capacity"));
                    location_count++;
                }
                //Debug.Log("ResourceRecovery get_LOCATIONSNode() running LCList=" + LCList);

                int module_count = 0;
                int capacity_found = 0;
                foreach (string s in LList)
                {
                    //Debug.Log("ResourceRecovery get_LocationsNode running 18 s=" + s);
                    //Debug.Log("ResourceRecovery get_LocationsNode running 19 LCList[module_count]=" + LCList[module_count]);
                    //ConfigNode AddLocationNode = new ConfigNode("LOCATION");
                    //AddLocationNode.AddValue("lon_lat", s);
                    //AddLocationNode.AddValue("capacity", LCList[module_count]);
                    //AddLocationsNode.AddNode(AddLocationNode);
                    int string_number = 0;
                    if (Int32.TryParse(LCList[module_count], out string_number))
                    {
                        //Debug.Log("ResourceRecovery get_LocationsNode running 19 LCList[module_count] s=" + LCList[module_count]);
                        //Console.WriteLine("Converted '{0}' to {1}.", LCList[module_count], string_number);
                        capacity_found += string_number;
                    }
                    module_count++;
                }

                //Update the StorageNode node with the totals:
                //Update the PlanetNode node with the totals:
                ConfigNode PlanetNode = get_PlanetNode(PlanetNodeName);
                if (PlanetNodeName == KSCPlanet)
                {
                    StorageNode.SetValue("storagecapacity", RRSettingsController.get_KSCStorage_capacity() + " (" + capacity_found + ")");
                    //location_count++;
                    PlanetNode.SetValue("total_locations", location_count.ToString());
                }
                else
                {
                    StorageNode.SetValue("storagecapacity", capacity_found.ToString());
                    PlanetNode.SetValue("total_locations", location_count.ToString());
                }
                StorageNode.SetValue("modulecount", module_count.ToString());
                PlanetNode.SetValue("total_capacity", capacity_found.ToString());


                StorageNode = StorageNode.AddNode(AddLocationsNode);
                LocationsNode = PlanetNode.GetNode("LOCATIONS");
                //return get_LocationsNode();
            }
            return LocationsNode;
        }


        public static bool update_StorageTotals()
        {
            decimal total_capacity_used = 0;
            int total_number_recource_tanks = 0;
            string PlanetNodeName = Utilities.get_current_planet();
            ConfigNode ResourcesNode = get_ResourcesNode(PlanetNodeName);
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
                ValueSet(get_PlanetNode(PlanetNodeName), "total_number_recource_tanks", total_number_recource_tanks.ToString());
                ValueSet(get_PlanetNode(PlanetNodeName), "total_capacity_used", total_capacity_used.ToString());
            }

            return true;
        }
            
            
        public static bool modify_ResourceNode_by_ResourceName(string resourceName, string valueName, string newvalue)
        {
            //ToDo
            //Debug.Log("ResourceRecovery ScenarioNodeDatabase modify_ResourceNode_by_ResourceName running");
            
            ConfigNode ResourceNode = get_ResourceNode(resourceName);
            
            if (ResourceNode.SetValue(valueName, newvalue))
            {
                    return true;
            }
            return false;
        }
        public static bool remove_ResourceNode(string SanitizedResourceNodeName)
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

    
    
    
    }
}
