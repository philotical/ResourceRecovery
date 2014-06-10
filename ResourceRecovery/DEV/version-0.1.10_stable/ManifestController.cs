using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Philotical
{
    class ManifestController 
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
      
         
        internal void ManifestMain(int id)
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

            //RootNode = Scenario.get_RootNode();
            RRDATAcontroller.update_StorageTotals();


                GUILayout.Label("Manifest: ToDo", style);

                string currentPlanet = Utilities.get_current_planet();
                ConfigNode PlanetNode = RRDATAcontroller.get_PlanetNode(currentPlanet);
                //Debug.Log("ResourceRecovery: ManifestMain: got planet ");
                /*
                name = Kerbin
                total_locations = 1
                total_number_recource_tanks = 35
                total_capacity = 0 (ToDo)
                total_capacity_used = 55.55 (May need a fix)            
                */

                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();

                GUILayout.Label(String.Format("Planet Name: {0}", PlanetNode.GetValue("name")), style);
                GUILayout.Label(String.Format("Storage Locations: {0}", PlanetNode.GetValue("total_locations")), style);

                GUILayout.EndVertical();
                GUILayout.BeginVertical();

                GUILayout.Label(String.Format("Resource Tanks: {0}", PlanetNode.GetValue("total_number_recource_tanks")), style);

                string total_capacity_string = (RRSettingsController.get_KSCPlanet() != Utilities.get_current_planet()) ? PlanetNode.GetValue("total_capacity") : RRSettingsController.get_KSCStorage_capacity();
                GUILayout.Label(String.Format("Stored Total: {0}", PlanetNode.GetValue("total_capacity_used") + "/" + total_capacity_string), style);

                GUILayout.EndVertical();
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(splitter); GUILayout.EndHorizontal();
                ResourceRowHeader();
                GUILayout.BeginHorizontal(splitter); GUILayout.EndHorizontal();

                scrollPosition = GUILayout.BeginScrollView(scrollPosition);
                GUILayout.BeginVertical();

                ConfigNode RNode_tmp = RRDATAcontroller.get_ResourcesNode(currentPlanet);
                ConfigNode[] RNodes = RNode_tmp.GetNodes("RESOURCE");
                int counter = 0;
                List<bool> _expanded = new List<bool>();
                int TotalNumbr = RNode_tmp.CountNodes;
            
                //Debug.Log("ResourceRecovery: ManifestMain: start foreach");
                List<ResourceNodeType> RNodesList = new List<ResourceNodeType>();
                foreach (ConfigNode node in RNodes)
                {
                    int string_number = 0;
                    Int32.TryParse(node.GetValue("resourceID"), out string_number);

                    RNodesList.Add(new ResourceNodeType() { name = node.GetValue("name"),
                                                            resourceID = string_number,
                                                            storagecapacity = node.GetValue("storagecapacity"),
                                                            storedamount = node.GetValue("storedamount"),
                                                            supply_mode = node.GetValue("supply_mode"),
                                                            value_factor = node.GetValue("value_factor"),
                                                            display_order = node.GetValue("display_order"),
                                                            hide = node.GetValue("hide"),
                                                            group = node.GetValue("group")
                    });
                }
                var sortedRNodes = RNodesList.OrderBy(x => x.display_order).ToList();
                foreach (ResourceNodeType node in sortedRNodes)
                {
                    _expanded.Add(false);
                    //GUILayout.Label(String.Format("{0}:{1}", node.GetValue("name"), node.GetValue("storedamount")));
                    ResourceRow(node,_expanded,counter,TotalNumbr);
                    counter++;
                }
                //Debug.Log("ResourceRecovery: ManifestMain: end foreach");
                GUILayout.EndVertical();
                GUILayout.EndScrollView();
                //Debug.Log("ResourceRecovery: ManifestMain: done");
        }



        internal void ResourceRowHeader()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("↑↓", style_20px_row);
            GUILayout.Label("Name", style_stretchWidth_row);
            GUILayout.Label("Amount", style_100px_row);
            //GUILayout.Label("Mode", style_80px_row);
            //GUILayout.Label("Value Factor", style_80px_row);
            GUILayout.Label("Group", style_80px_row);
            GUILayout.Label("Hide in Flight", style_100px_row);
            GUILayout.Label("hidden now?", style_80px_row);
            GUILayout.Label("Dump Resource", style_100px_row);
            GUILayout.Label("FooBar", style_120px_row);
            GUILayout.EndHorizontal();
        }

        /*
        RESOURCE
        {
            name = Antimatter
            storagecapacity = unlimited (ToDo)
            storedamount = 10.00
            supply_mode = 1
            value_factor = 1
            display_order = 3
            hide = 0
            group = none
        }
        */
        internal void ResourceRow(ResourceNodeType node, List<bool> _expanded, int counter, int TotalNumbr)
        {
            int this_id = counter;
            int last_id = counter-1;
            int next_id = counter+1;
            string res_nam = node.name;

            //Debug.Log("ResourceRecovery: ResourceRow: res_nam=" + res_nam + " this_id=" + this_id + " last_id=" + last_id + " next_id=" + next_id);

            GUILayout.BeginHorizontal();
            if (last_id >= 0)
            {
                if (GUILayout.Button("↑", style_10px_row))
                {
                    Debug.Log("ResourceRecorvery: ResourceRow() Clicked Button R:" + res_nam + " : up");
                    change_order(res_nam, this_id, last_id);
                }
            }
            else
            {
                GUILayout.Label("", style_10px_row);
            }
            if (last_id < TotalNumbr)
            {
                if (GUILayout.Button("↓", style_10px_row))
                {
                    Debug.Log("ResourceRecorvery: ResourceRow() Clicked Button R:" + res_nam + " : down");
                    change_order(res_nam,this_id, next_id);
                }
            }
            else
            {
                GUILayout.Label("", style_10px_row);
            }
            GUILayout.Label(String.Format("{0}", res_nam + " (" + node.display_order + ")"), style_stretchWidth_row);
            if (node.supply_mode!="0")
            {
                double string_number0 = 0;
                if (double.TryParse(node.storedamount, out string_number0))
                {
                    GUILayout.Label(String.Format("{0}", Math.Round(string_number0, 2)), style_100px_row);
                }
                else
                {
                    GUILayout.Label(String.Format("{0}", node.storedamount), style_100px_row);
                }
            }
            else
            {
                GUILayout.Label(String.Format("{0}", RRSettingsController.get_resource_supply_mode_definition(0)), style_100px_row);
            }
            /*
            int string_number = 0;
            if (Int32.TryParse(node.supply_mode, out string_number))
            {
                GUILayout.Label(String.Format("{0}", RRSettingsController.get_resource_supply_mode_definition(string_number)), style_80px_row);
            }
            else
            {
                GUILayout.Label(String.Format("{0}", node.supply_mode), style_80px_row);
            }

            GUILayout.Label(String.Format("{0}", node.value_factor), style_80px_row);
            */
            if (GUILayout.Button(node.group, RRSettingsController.get_button_80px()))
            {
                Debug.Log("Clicked Button = change Group" + res_nam);
                changeGroup(res_nam, node.group);
            }
            //GUILayout.Label(String.Format("{0}", node.group), style_80px_row);

            if (GUILayout.Button("Hide/Unhide", buttonStyle))
            {
                Debug.Log("Clicked Button = Hide/Unhide" + res_nam);
                change_hidden_status(res_nam, node.hide);
            }
            int string_number2 = 0;
            if (Int32.TryParse(node.hide, out string_number2))
            {
                GUILayout.Label(String.Format("{0}", Utilities.bool_int_to_string(string_number2, true)), style_80px_row);
            }
            else
            {
                GUILayout.Label(String.Format("{0}", node.hide), style_80px_row);
            }

            if (GUILayout.Button("Dump All", buttonStyle))
            {
                Debug.Log("Clicked Button = Dump All" + res_nam);
                DumpResource(res_nam);
            }
            if (GUILayout.Button("FooBar", buttonStyle))
            {
                Debug.Log("Clicked Button = FooBar" + res_nam);
                DEBUG_FillResource(res_nam, node.storedamount);
            }
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal(splitter);GUILayout.EndHorizontal();

        }

        internal bool DEBUG_FillResource(string res_nam,string current)
        {
            double string_number2 = 0;
            if (Double.TryParse(current, out string_number2))
            {
                string_number2 += 101.01;
                if (RRDATAcontroller.modify_ResourceNode_by_ResourceName(res_nam, "storedamount", string_number2.ToString()))
                {
                    //RootNode = Scenario.get_RootNode();
                    return true;
                }
                return false;
            }
            return false;
        }


        internal bool changeGroup(string res_nam, string current)
        {
            List<string> ResourceGroupList = RRSettingsController.get_ResourceGroupList();
            int i = 0;
            foreach (string G in ResourceGroupList)
            {
                if (current == G)
                {
                    int x = (i < ResourceGroupList.Count - 1) ? i + 1 : 0;
                    //Debug.Log("ResourceRecovery changeGroup  current=" + current + " G=" + G + " ResourceGroupList.Count=" + ResourceGroupList.Count + " i=" + i + " x=" + x);
                    current = ResourceGroupList[x];
                    continue;
                }
                i++;
            }
            /*
            if(current == ResourceGroupList[0])
            {
                current = ResourceGroupList[1];
            }
            else if(current == ResourceGroupList[1])
            {
                current = ResourceGroupList[2];
            }
            else if(current == ResourceGroupList[2])
            {
                current = ResourceGroupList[3];
            }
            else if (current == ResourceGroupList[3])
            {
                current = ResourceGroupList[0];
            }
            else { current = ResourceGroupList[0]; }
            */
            if (RRDATAcontroller.modify_ResourceNode_by_ResourceName(res_nam, "group", current))
            {
                //Scenario.update_StorageTotals();
                //RootNode = Scenario.get_RootNode();
                    return true;
            }
            return false;
        }
        internal bool DumpResource(string res_nam)
        {
            if (RRDATAcontroller.modify_ResourceNode_by_ResourceName(res_nam, "storedamount", "0.00"))
            {
                //Scenario.update_StorageTotals();
                //RootNode = Scenario.get_RootNode();
                    return true;
            }
            return false;
        }

        internal bool change_hidden_status(string res_nam, string current)
        {
            current = (current == "0") ? "1" : "0";
            if (RRDATAcontroller.modify_ResourceNode_by_ResourceName(res_nam, "hide", current))
            {
                    return true;
            }
            return false;
        }

        internal bool change_order(string res_nam, int old_id, int new_id)
        {
            string new_id_string = (new_id < 10) ? "0" + new_id.ToString() : new_id.ToString();
            string old_id_string = (old_id < 10) ? "0" + old_id.ToString() : old_id.ToString();
            //Debug.Log("ResourceRecovery: change_order: " + res_nam + " old_id=" + old_id + " new_id=" + new_id);
            /*
            */
            //RootNode = Scenario.get_RootNode();
            //ResourcesNode = Scenario.get_ResourcesNode();
            ConfigNode ResourceNode1=null;
            ConfigNode ResourceNode2=null;
            ResourcesNode = RRDATAcontroller.get_ResourcesNode(Utilities.get_current_planet());
            ConfigNode[] RESOURCE = ResourcesNode.GetNodes("RESOURCE");
            foreach (ConfigNode res in RESOURCE)
            {
                if (res == null)
                {
                    continue;
                }
                if (res.GetValue("display_order") == old_id_string)
                {
                    ResourceNode1 = res;
                    //Debug.Log("ResourceRecovery ScenarioNodeDatabase change_order()  res.GetValue(name)=" + res.GetValue("name") + "== old_id  old_id=" + old_id_string + " + new_id=" + new_id_string);
                }
                if (res.GetValue("display_order") == new_id_string)
                {
                    ResourceNode2 = res;

                    //Debug.Log("ResourceRecovery ScenarioNodeDatabase change_order()  res.GetValue(name)=" + res.GetValue("name") + "== new_id   old_id=" + old_id_string + " + new_id=" + new_id_string);
                }
            }
            if (ResourceNode1.SetValue("display_order", new_id_string) && ResourceNode2.SetValue("display_order", old_id_string))
            {
                //Scenario.update_StorageTotals();
                //if (Utilities.SavePluginSaveFile(RootNode))
                    return true;
            }
            return false;

        }

    }
}

