using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Philotical
{


    /*
    public class RRdataNode 
    {
        public string Name { get; set; }
        public string scene { get; set; }
        public string version { get; set; }
        public string created { get; set; }
        public string updated { get; set; }
        public List<PlanetNode> Planets { get; set; }

    }
    public class PlanetsNode : IEnumerable
    {
        public Planet[] Planets { get; set; }
        public PlanetsNode(Planet[] pArray)
        {
            Planets = new Planet[pArray.Length];

            for (int i = 0; i < pArray.Length; i++)
            {
                Planets[i] = pArray[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
           return (IEnumerator) GetEnumerator();
        }

        public PlanetsNodeEnum GetEnumerator()
        {
            return new PlanetsNodeEnum(Planets);
        }
    }
    public class PlanetsNodeEnum : IEnumerator
    {
        public Planet[] Planets;

        // Enumerators are positioned before the first element
        // until the first MoveNext() call.
        int position = -1;

        public PlanetsNodeEnum(Planet[] list)
        {
            Planets = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < Planets.Length);
        }

        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public Planet Current
        {
            get
            {
                try
                {
                    return Planets[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
    public class PlanetNode 
    {
        public string planetID { get; set; }
        public string planetName { get; set; }
        public string total_locations { get; set; }
        public string total_number_recource_tanks { get; set; }
        public string total_capacity { get; set; }
        public string total_capacity_used { get; set; }
        public StorageNode Storage { get; set; }
    }
    public class StorageNode 
    {
        public string capacity { get; set; }
        public string modulecount { get; set; }
        public LocationsNode Locations { get; set; }
        public ResourcesNode Resources { get; set; }
    }
    public class LocationsNode 
    {
        public LocationNode[] Locations { get; set; }
    }
    public class LocationNode 
    {
        public string lon_lat { get; set; }
        public string capacity { get; set; }
    }
    public class ResourcesNode 
    {
        public ResourceNode[] Resource { get; set; }
    }
    public class ResourceNode 
    {
        public string resourceID { get; set; }
        public string name { get; set; }
        public string storagecapacity { get; set; }
        public string storedamount { get; set; }
        public string supply_mode { get; set; }
        public string value_factor { get; set; }
        public string total_sold { get; set; }
        public string total_bought { get; set; }
        public string display_order { get; set; }
        public string hide { get; set; }
        public string group { get; set; }
    }
    */
   
    /*
    static class RRdata
    {
        internal static ConfigNode get_root()
        {
            return rrdata;
        }
        private static ConfigNode rrdata;
        internal static ConfigNode _RRdata
        {
            get { return rrdata; }  // Getter
            set { rrdata = value; } // Setter
        }
        private static string name;
        internal static string _Name
        {
            get { return name; }  // Getter
            set {name=value; } // Setter
        }
        private static string scene;
        internal static string _Scene
        {
            get { return scene; }  // Getter
            set { scene = "7, 8, 5"; } // Setter
        }
        private static string version;
        internal static string _Version
        {
            get { return version; }  // Getter
            set { } // Setter
        }
        private static string created;
        internal static string _Created
        {
            get { return created; }  // Getter
            set { } // Setter
        }
        private static string updated;
        internal static string _Updated
        {
            get { return updated; }  // Getter
            set { updated = value; } // Setter
        }
        private static string savegamekredits;
        internal static string _SaveGameKredits
        {
            get { return savegamekredits; }  // Getter
            set { savegamekredits = value; } // Setter
        }
        private static ConfigNode[] rrplanet;
        internal static ConfigNode[] _RRplanet
        {
            get { return rrplanet; }  // Getter
            set { rrplanet = value; } // Setter
        }
    }
        */
    /*
    class RRplanet
    {
        internal ConfigNode rrplanet;
        internal ConfigNode _RRplanet()
        {
            return rrplanet;
        }
        private static string name;
        internal static string _Name
        {
            get { return name; }  // Getter
            set { name = value; } // Setter
        }
        private static string total_locations;
        internal static string _Total_Locations
        {
            get { return total_locations; }  // Getter
            set { total_locations = value; } // Setter
        }
        private static string total_number_recource_tanks;
        internal static string _Total_Number_Recource_Tanks
        {
            get { return total_number_recource_tanks; }  // Getter
            set { total_number_recource_tanks = value; } // Setter
        }
        private static string total_capacity;
        internal static string _Total_Capacity
        {
            get { return total_capacity; }  // Getter
            set { total_capacity = value; } // Setter
        }
        private static string total_capacity_used;
        internal static string _Total_Capacity_Used
        {
            get { return total_capacity_used; }  // Getter
            set { total_capacity_used = value; } // Setter
        }
        private static ConfigNode rrlocations;
        internal ConfigNode _RRlocations
        {
            get { return rrlocations; }  // Getter
            set { rrlocations = value; } // Setter
        }
        private static ConfigNode rrresources;
        internal ConfigNode _RRresources
        {
            get { return rrresources; }  // Getter
            set { rrresources = value; } // Setter
        }
    }
    class RRlocations
    {
        internal ConfigNode rrlocations;
        internal ConfigNode _RRlocations()
        {
            return rrlocations;
        }
        private static ConfigNode location;
        internal static ConfigNode _Location
        {
            get { return location; }  // Getter
            set { location = value; } // Setter
        }
    }
    class RRlocation
    {
        internal ConfigNode rrlocation;
        internal ConfigNode _RRlocation()
        {
            return rrlocation;
        }
        private static string lon_lat;
        internal static string _Lon_Lat
        {
            get { return lon_lat; }  // Getter
            set { lon_lat = value; } // Setter
        }
        private static string capacity;
        internal static string _Capacity
        {
            get { return capacity; }  // Getter
            set { capacity = value; } // Setter
        }
    }
    class RRresources
    {
        internal ConfigNode rrresources;
        internal ConfigNode _RRresources()
        {
            return rrresources;
        }
        private static ConfigNode rrresource;
        internal static ConfigNode _RRresource
        {
            get { return rrresource; }  // Getter
            set { rrresource = value; } // Setter
        }
    }
    class RRresource
    {
        internal ConfigNode rrresource;
        internal ConfigNode _RRresource()
        {
            return rrresource;
        }
        private static string resourceID;
        internal static string _ResourceID
        {
            get { return resourceID; }  // Getter
            set { resourceID = value; } // Setter
        }
        private static string name;
        internal static string _Name
        {
            get { return name; }  // Getter
            set { name = value; } // Setter
        }
        private static string storagecapacity;
        internal static string _StorageCapacity
        {
            get { return storagecapacity; }  // Getter
            set { storagecapacity = value; } // Setter
        }

        private static string storedamount;
        internal static string _StoredAmount
        {
            get { return storedamount; }  // Getter
            set { storedamount = value; } // Setter
        }
        private static string supply_mode;
        internal static string _Supply_Mode
        {
            get { return supply_mode; }  // Getter
            set { supply_mode = value; } // Setter
        }
        private static string value_factor;
        internal static string _Value_Factor
        {
            get { return value_factor; }  // Getter
            set { value_factor = value; } // Setter
        }
        private static string total_sold;
        internal static string _Total_Sold
        {
            get { return total_sold; }  // Getter
            set { total_sold = value; } // Setter
        }
        private static string total_bought;
        internal static string _Total_Bought
        {
            get { return total_bought; }  // Getter
            set { total_bought = value; } // Setter
        }
        private static string display_order;
        internal static string _Display_Order
        {
            get { return display_order; }  // Getter
            set { display_order = value; } // Setter
        }
        private static string hide;
        internal static string _Hide
        {
            get { return hide; }  // Getter
            set { hide = value; } // Setter
        }
        private static string group;
        internal static string _Group
        {
            get { return group; }  // Getter
            set { group = value; } // Setter
        }
}
 	*/	

    class RRDATAcontroller
    {

        internal static void init(ConfigNode sub_root)
        {
                string p = "Kerbin";
                get_PlanetNode(p);
            if (sub_root.HasNode("PLANET"))
            {
                Debug.Log("ResourceRecovery RRDATAcontroller init sub_root.HasNode(PLANET)");
                // do stuff

                get_StorageNode(p);

            }
            else 
            {
                Debug.Log("ResourceRecovery RRDATAcontroller init !sub_root.HasNode(PLANET)");
                /*
                ConfigNode Kerbin = NodeGet(sub_root, "PLANET", "Kerbin");
                ValueSet(Kerbin, "total_locations", "1");
                ValueSet(Kerbin, "total_number_recource_tanks", "0");
                ValueSet(Kerbin, "total_capacity", "0");
                ValueSet(Kerbin, "total_capacity_used", "0");
                */
            }


            Debug.Log("ResourceRecovery RRDATAcontroller init sub_root=" + sub_root);



            Debug.Log("ResourceRecovery init end sub_root=" + sub_root);
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
            Debug.Log("ResourceRecovery NodeGet nodeName=" + nodeName + " Value_nameAttribut=" + Value_nameAttribut + " parent=" + parent);
            ConfigNode[] result1 = parent.GetNodes(nodeName);
            ConfigNode result2 = null;
            if (result1.Length > 1)
            {
                foreach (ConfigNode check2 in result1)
                {
                    if (check2.GetValue("name") == Value_nameAttribut)
                    {
                        result2 = check2;
                    }
                }
                if (result2 == null)
                {
                    result2 = new ConfigNode();
                    if (Value_nameAttribut!=null)
                    {
                        result2.AddValue("name", Value_nameAttribut);
                    }
                    parent.AddNode(result2);
                    result2 = parent.GetNode(nodeName);
                    Debug.Log("ResourceRecovery NodeGet 1 result2=" + result2);
                    return result2;
                }
                else 
                {
                    Debug.Log("ResourceRecovery NodeGet 2 result2=" + result2);
                    return result2;
                }
            }
            else if (result1.Length == 0)
            {
                result2 = new ConfigNode(nodeName);
                if (Value_nameAttribut != null)
                {
                    result2.AddValue("name", Value_nameAttribut);
                }
                parent.AddNode(result2);
                result2 = parent.GetNode(nodeName);

                Debug.Log("ResourceRecovery NodeGet 3 result2=" + result2);
                return result2;
            }
            else
            {
                if (Value_nameAttribut != null && !result1[0].HasValue("name"))
                {
                    result1[0].AddValue("name", Value_nameAttribut);
                }
                Debug.Log("ResourceRecovery NodeGet result1[0]=" + result1[0]);
                return result1[0];
            }
        }
        public static string ValueGet(ConfigNode parent, string valueName)
        {
            Debug.Log("ResourceRecovery ValueGet valueName=" + valueName + " parent.GetValue(valueName)=" + parent.GetValue(valueName) + " parent=" + parent); 
            return parent.GetValue(valueName);
        }


        public static bool NodeValueSet(ConfigNode parent, string[] node, string[] Value)
        {
            Debug.Log("ResourceRecovery NodeValueSet start parent=" + parent);
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
                Debug.Log("ResourceRecovery NodeValueSet nodeName=" + nodeName + " nodeNameAttribut=" + nodeNameAttribut + " valueName=" + valueName + " newValue=" + newValue + " parent=" + parent);
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
            Debug.Log("ResourceRecovery ValueSet start ValueName=" + valueName + " newValue=" + newValue + " parent=" + parent);
            if (parent.HasValue(valueName))
            {
                parent.SetValue(valueName, newValue);
                Debug.Log("ResourceRecovery ValueSet 1 new parent=" + parent);
                return true;
            }
            else
            {
                parent.AddValue(valueName, newValue);
                parent.SetValue(valueName, newValue);
                Debug.Log("ResourceRecovery ValueSet 2 new parent=" + parent);
                return true;
            }
        }

        public static bool ValueCreate(ConfigNode parent, string valueName, string newValue)
        {
            Debug.Log("ResourceRecovery ValueCreate start ValueName=" + valueName + " newValue=" + newValue + " parent=" + parent);
            if (!parent.HasValue(valueName))
            {
                parent.AddValue(valueName, newValue);
                parent.SetValue(valueName, newValue);
                Debug.Log("ResourceRecovery ValueCreate 2 new parent=" + parent);
                return true;
            }
            return true;
        }
        public static bool NodeCreate(ConfigNode parent, string nodeName)
        {
            Debug.Log("ResourceRecovery NodeCreate start nodeName=" + nodeName + " parent=" + parent);
            if (!parent.HasNode(nodeName))
            {
                parent.AddNode(nodeName);
                Debug.Log("ResourceRecovery NodeCreate 2 new parent=" + parent);
                return true;
            }
            return true;
        }
        #endregion

        
        
        // get the PlanetNode (create if needed)
        public static ConfigNode get_PlanetNode(string PlanetNodeName)
        {
            //Debug.Log("ResourceRecovery get_PlanetNode RootNode Pre = : " + RootNode);
            ConfigNode PlanetNode = NodeGet(get_all_planets(), "PLANET", PlanetNodeName);
            ValueCreate(PlanetNode, "total_locations", "ToDo");
            ValueCreate(PlanetNode, "total_number_recource_tanks", "ToDo");
            ValueCreate(PlanetNode, "total_capacity", "ToDo");
            ValueCreate(PlanetNode, "total_capacity_used", "ToDo");
            //Debug.Log("ResourceRecovery get_PlanetNode RootNode Post = : " + RootNode);
            return PlanetNode;
        }
        // get the PlanetNode (create if needed)
        public static ConfigNode get_StorageNode(string PlanetNodeName)
        {
            Debug.Log("ResourceRecovery get_StorageNode ");
            ConfigNode StorageNode = NodeGet(get_PlanetNode(PlanetNodeName), "STORAGE");
            ValueCreate(StorageNode, "capacity", "ToDo");
            ValueCreate(StorageNode, "modulecount", "ToDo");
            return StorageNode;
         }
        /*
        // get the PlanetNode (create if needed)
        public static ConfigNode get_LocationsNode(string PlanetNodeName)
        {
            ConfigNode StorageNode = NodeGet(get_PlanetNode(PlanetNodeName), "STORAGE");
        }
        */
    }
}
