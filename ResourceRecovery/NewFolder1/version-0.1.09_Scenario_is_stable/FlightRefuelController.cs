using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Philotical
{



    class FlightRefuelController : MonoBehaviour
    {
        public static Vector2 scrollPosition;
        private GUIStyle style;
        private GUIStyle style_10px_row;
        private GUIStyle style_20px_row;
        private GUIStyle style_40px_row;
        private GUIStyle style_60px_row;
        private GUIStyle style_80px_row;
        private GUIStyle style_100px_row;
        private GUIStyle style_120px_row;
        private GUIStyle style_stretchWidth_row;
        public static GUIStyle get_button_80px;
        public static GUIStyle buttonStyle;
        public static GUIStyle buttonStyle_active;
        public static GUIStyle splitter;
        public static string resourceSelected=null;
        public static string groupFilter;
        public static Vessel vessel;
        internal static ScenarioNodeDatabase Scenario = null;

        public Dictionary<string, ResourceInfo> resources;
        internal static Vessel currentVessel;
        internal static int numberOfParts;
        internal static Vessel.Situations vesselSituation;



        internal void RefuelMain(int id, ScenarioNodeDatabase Sc)
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
            get_button_80px = RRSettingsController.get_button_80px();
            buttonStyle = RRSettingsController.get_button_100px();
            buttonStyle_active = RRSettingsController.get_button_100px_active();
            splitter = RRSettingsController.get_splitter();

            vessel = FlightGlobals.fetch.activeVessel;
            Scenario = new ScenarioNodeDatabase();
            Scenario = Sc;
            //RootNode = Scenario.get_RootNode();
            Scenario.update_StorageTotals();

            GUILayout.Label("FlightRefuelController: ToDo", style);
            //string currentPlanet = Utilities.get_current_planet();
            //ConfigNode PlanetNode = Scenario.get_PlanetNode(currentPlanet);
            groupFilter = (groupFilter == null) ? "all" : groupFilter;
            groupFilter = ButtonController.get_groupFilter("all");



            //if (vessel.checkLanded())
            if (IsStationary())
            {
                if (resourceSelected != null)
                {
                    ConfigNode SelectedResource = Scenario.get_ResourceNode(resourceSelected);
                    string res_nam = resourceSelected;
                    //Debug.Log("ResourceRecovery: RefuelMain: beginn resourceSelected");
                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Back", get_button_80px))
                    {
                        Debug.Log("Resource Recovery: resourceSelected: Clicked BackButton");
                        resourceSelected = null;
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();

                    GUILayout.BeginVertical();
                    GUILayout.Label("ResourceData: " + res_nam, style, GUILayout.Width(200));
                    if (RRSettingsController.get_KSCPlanet()!=Utilities.get_current_planet())
                    {
                        double string_number3 = 0;
                        double.TryParse(SelectedResource.GetValue("storagecapacity"), out string_number3);
                        GUILayout.Label(String.Format("Storage Capacity: {0}", string_number3), style_120px_row);
                    }
                    else
                    {
                        GUILayout.Label(String.Format("Storage Capacity: {0}", SelectedResource.GetValue("storagecapacity")), style_120px_row);
                    }
                    if(SelectedResource.GetValue("supply_mode") != "0")
                    {
                        double string_number2 = 0;
                        double.TryParse(SelectedResource.GetValue("storedamount"), out string_number2);
                        GUILayout.Label(String.Format("Available: {0}", Math.Round(string_number2,2)), style_120px_row);
                    }
                    else
                    {
                        GUILayout.Label(String.Format("Available: {0}", RRSettingsController.get_resource_supply_mode_definition(0)), style_120px_row);
                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical();
                    GUILayout.Label("ShipData:", style_100px_row);
                    GUILayout.Label(String.Format("{0}", vessel.GetName()), style_120px_row);

                    resources = new Dictionary<string, ResourceInfo>();
                    RebuildPartsLists(vessel, resources);
                    double all_parts_max_amount = 0;
                    double all_parts_amount = 0;
                    double all_parts_required = 0;
                    foreach (KeyValuePair<string, ResourceInfo> pair in GetResourceInfo())
                    {
                        ResourceInfo resourceInfo = pair.Value;
                        resourceInfo.pair_key = pair.Key;
                        resourceInfo.isShowing = (pair.Key != resourceSelected) ? false : true;
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
                    all_parts_required =  all_parts_max_amount - all_parts_amount;

                    GUILayout.Label(String.Format("maxAmount: {0}", Math.Round(all_parts_max_amount,2)), style_120px_row);
                    GUILayout.Label(String.Format("Amount: {0}", Math.Round(all_parts_amount,2)), style_120px_row);
                    GUILayout.Label(String.Format("Required: {0}", Math.Round(all_parts_required,2)), style_120px_row);
                    GUILayout.EndVertical();

                    GUILayout.EndHorizontal();


                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();



                    GUILayout.BeginHorizontal(splitter); GUILayout.EndHorizontal();
                    GUILayout.Label("Parts List:", style_100px_row);
                    ResourceRowHeaderPartsList();
                    GUILayout.BeginHorizontal(splitter); GUILayout.EndHorizontal();
                    scrollPosition = GUILayout.BeginScrollView(scrollPosition);
                    GUILayout.BeginVertical();

                    foreach (KeyValuePair<string, ResourceInfo> pair in GetResourceInfo())
                    {
                        //Debug.Log("ResourceRecovery: RefuelMain: foreach KeyValuePair 1");
                        ResourceInfo resourceInfo = pair.Value;
                        resourceInfo.pair_key = pair.Key;
                        //Debug.Log("ResourceRecovery: RefuelMain: foreach KeyValuePair 2");
                        resourceInfo.isShowing = (pair.Key != resourceSelected) ? false : true;
                        //Debug.Log("ResourceRecovery: RefuelMain: foreach KeyValuePair pair.Key=" + pair.Key);
                        //Debug.Log("ResourceRecovery: RefuelMain: foreach KeyValuePair resourceInfo.isShowing=" + resourceInfo.isShowing);
                        if (resourceInfo.isShowing)
                        {

                            foreach (ResourcePartMap partInfo in resourceInfo.parts)
                            {
                                //Debug.Log("ResourceRecovery: RefuelMain: foreach KeyValuePair 4");
                                PartResource resource = partInfo.resource;
                                //Debug.Log("ResourceRecovery: RefuelMain: foreach KeyValuePair resource=" + resource);
                                Part part = partInfo.part;
                                //Debug.Log("ResourceRecovery: RefuelMain: foreach KeyValuePair part=" + part);
                                double percentFull = resource.amount / resource.maxAmount * 100.0;
                                //Debug.Log("ResourceRecovery: RefuelMain: foreach KeyValuePair 7");
                                        
            

                                GUILayout.BeginVertical();
                                GUILayout.BeginHorizontal();

                                //Debug.Log("ResourceRecovery: RefuelMain: foreach KeyValuePair 8");
                                string partTitle = part.partInfo.title;
                                //Debug.Log("ResourceRecovery: RefuelMain: foreach KeyValuePair partTitle=" + partTitle);
                                GUILayout.Label(partTitle.Substring(0, Math.Min(30, partTitle.Length)), style);
                                //Debug.Log("ResourceRecovery: RefuelMain: foreach KeyValuePair 10");
                                GUILayout.FlexibleSpace();
                                GUILayout.Label(resource.maxAmount.ToString("F2"), style_60px_row);
                                //Debug.Log("ResourceRecovery: RefuelMain: foreach KeyValuePair 11");
                                GUILayout.Label(resource.amount.ToString("F2"), style_60px_row);
                                //Debug.Log("ResourceRecovery: RefuelMain: foreach KeyValuePair 12");
                                GUILayout.Label(percentFull.ToString("F1") + "%", style_60px_row);
                                GUILayout.EndHorizontal();

                                GUILayout.BeginHorizontal();
                                GUILayout.FlexibleSpace();
                                //Debug.Log("ResourceRecovery: RefuelMain: foreach KeyValuePair 13");
                                List<string> RefuelOptions = RRSettingsController.get_VesselRefuelOptionsList();
                                List<string> ro = new List<string>();
                                int count = 0;
                                foreach (string f in RefuelOptions)
                                {
                                    ro.Add(f);
                                    count++;
                                }
                                ButtonController.GetResourceOptionsButtons(ro, part, resource, resourceSelected, Scenario);
                                GUILayout.EndHorizontal();

                                GUILayout.EndVertical();
                                if (Event.current.type == EventType.Repaint && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                                {
                                    SetPartHighlight(partInfo.part, Color.green);
                                }
                                else
                                {
                                    ClearHighlight(partInfo.part);
                                }
                                GUILayout.BeginHorizontal(splitter); GUILayout.EndHorizontal();
                            }
                            //Debug.Log("ResourceRecovery: RefuelMain: foreach KeyValuePair 14");
                        }
                        //Debug.Log("ResourceRecovery: RefuelMain: foreach KeyValuePair 15");
                    }
                    //Debug.Log("ResourceRecovery: RefuelMain: done foreach KeyValuePair");


                    GUILayout.EndVertical();
                    GUILayout.EndScrollView();

                    //Debug.Log("ResourceRecovery: RefuelMain: done resourceSelected");
                }
                else
                {
                    //Debug.Log("ResourceRecovery: RefuelMain: beginn foreach FilterOptions");
                    GUILayout.BeginHorizontal();
                    List<string> FilterOptions = RRSettingsController.get_ResourceGroupList();
                    List<string> fo = new List<string>();
                    int count = 0;
                    foreach (string f in FilterOptions)
                    {
                        string foo = f;
                        if (foo == "none")
                        { foo = "all"; }
                        fo.Add(foo);
                        count++;
                    }
                    ButtonController.GetResourceFilterButtons(fo, groupFilter);
                    GUILayout.EndHorizontal();


                    //Debug.Log("ResourceRecovery: RefuelMain: beginn ResourceRowHeader");
                    GUILayout.BeginHorizontal(splitter); GUILayout.EndHorizontal();
                    ResourceRowHeader();
                    GUILayout.BeginHorizontal(splitter); GUILayout.EndHorizontal();
                    scrollPosition = GUILayout.BeginScrollView(scrollPosition);
                    GUILayout.BeginVertical();

                    //Debug.Log("ResourceRecovery: RefuelMain: beginn Scenario.get_ResourcesNode 1");
                    //Debug.Log("ResourceRecovery: RefuelMain: beginn Scenario.get_ResourcesNode 2  Scenario=" + Scenario + "  Scenario.ToString()=" + Scenario.ToString() + "  Scenario.total_number_recource_tanks=" + Scenario.total_number_recource_tanks);
                    ConfigNode RNode_tmp;
                    //Debug.Log("ResourceRecovery: RefuelMain: beginn Scenario.get_ResourcesNode 3");
                    RNode_tmp = Scenario.get_ResourcesNode();
                    ConfigNode[] RNodes = RNode_tmp.GetNodes("RESOURCE");
                    int counter = 0;
                    //Debug.Log("ResourceRecovery: RefuelMain: beginn RNode_tmp.CountNodes");
                    List<bool> _expanded = new List<bool>();
                    int TotalNumbr = RNode_tmp.CountNodes;

                    //Debug.Log("ResourceRecovery: RefuelMain: beginn foreach RNodes");
                    List<ResourceNodeType> RNodesList = new List<ResourceNodeType>();
                    foreach (ConfigNode node in RNodes)
                    {
                        int string_number = 0;
                        Int32.TryParse(node.GetValue("resourceID"), out string_number);
                        if (node.GetValue("hide") == "0" /*&& node.GetValue("storedamount") != "0.00"*/)
                        {
                            if (groupFilter == "all" || node.GetValue("group") == groupFilter)
                            {
                                RNodesList.Add(new ResourceNodeType()
                                {
                                    name = node.GetValue("name"),
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
                        }
                    }
                    //Debug.Log("ResourceRecovery: RefuelMain: beginn foreach sortedRNodes");
                    var sortedRNodes = RNodesList.OrderBy(x => x.display_order).ToList();
                    foreach (ResourceNodeType node in sortedRNodes)
                    {
                        _expanded.Add(false);
                        //GUILayout.Label(String.Format("{0}:{1}", node.GetValue("name"), node.GetValue("storedamount")));
                        ResourceRow(node, _expanded, counter, TotalNumbr);
                        counter++;
                    }
                    //Debug.Log("ResourceRecovery: RefuelMain: end foreach");
                    GUILayout.EndVertical();
                    GUILayout.EndScrollView();
                }
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Please land the Ship!", style_stretchWidth_row);
                GUILayout.EndHorizontal();

            }

        }

        internal void ResourceRowHeaderPartsList()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Name", style);
            GUILayout.FlexibleSpace();
            GUILayout.Label("Max", style_60px_row);
            GUILayout.Label("Amount", style_60px_row);
            GUILayout.Label("% Full", style_60px_row);
            GUILayout.EndHorizontal();
        }
        internal void ResourceRowHeader()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Name", style);
            GUILayout.FlexibleSpace();
            GUILayout.Label("Amount", style_100px_row);
            GUILayout.Label("Mode", style_80px_row);
            GUILayout.Label("Select", style_100px_row);
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
            int last_id = counter - 1;
            int next_id = counter + 1;
            string res_nam = node.name;

            //Debug.Log("ResourceRecovery: ResourceRow: res_nam=" + res_nam + " this_id=" + this_id + " last_id=" + last_id + " next_id=" + next_id);

            GUILayout.BeginHorizontal();
            GUILayout.Label(String.Format("{0}", res_nam + " (" + node.display_order + ")"), style_stretchWidth_row);
            double string_number0 = 0;
            if (double.TryParse(node.storedamount, out string_number0))
            {
                GUILayout.Label(String.Format("{0}", Math.Round(string_number0, 2)), style_100px_row);
            }
            else 
            {
                GUILayout.Label(String.Format("{0}", node.storedamount), style_100px_row);
            }
            int string_number = 0;
            if (Int32.TryParse(node.supply_mode, out string_number))
            {
                GUILayout.Label(String.Format("{0}", RRSettingsController.get_resource_supply_mode_definition(string_number)), style_80px_row);
            }
            else
            {
                GUILayout.Label(String.Format("{0}", node.supply_mode), style_80px_row);
            }


            if (GUILayout.Button("Select", buttonStyle))
            {
                Debug.Log("Clicked Button = Select" + res_nam);
                resourceSelected = res_nam;
                //DumpResource(res_nam);
            }
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal(splitter); GUILayout.EndHorizontal();

        }

        public static void ClearHighlight(Part part)
        {
            try
            {

                if (part != null)
                {
                    part.SetHighlightDefault();
                    part.SetHighlight(false);
                }
            }
            catch (Exception ex)
            {
                Debug.Log(" in ClearHighlight. Error: {0} \r\n\r\n{1}"+ ex.Message+ ex.StackTrace);
            }
        }

        public static void SetPartHighlight(Part part, Color color)
        {
            try
            {

                if (part != null)
                {
                    part.SetHighlightColor(color);
                    part.SetHighlight(true);
                }
            }
            catch (Exception ex)
            {
                Debug.Log(" in SetPartHighlight. Error: {0} \r\n\r\n{1}"+ ex.Message+ ex.StackTrace);
            }
        }

        private static bool IsControllable()
        {
            return vessel.IsControllable;
        }
        private static bool IsStationary()
        {
            return vessel.situation == Vessel.Situations.PRELAUNCH || vessel.situation == Vessel.Situations.LANDED || vessel.situation == Vessel.Situations.SPLASHED;
        }

        /*
        * (C) Copyright 2013, Taranis Elsu
        * CC BY-NC-SA 3.0
        */

        public void RebuildPartsLists(Vessel vessel, Dictionary<string, ResourceInfo> resources)
        {
            //Debug.Log("Rebuilding resource lists.");

            List<string> toDelete = new List<string>();
            //Debug.Log("Rebuilding before toDelete.");
            foreach (KeyValuePair<string, ResourceInfo> resourceEntry in resources)
            {
                resourceEntry.Value.parts.RemoveAll(partInfo => !vessel.parts.Contains(partInfo.part));

                if (resourceEntry.Value.parts.Count == 0)
                {
                    toDelete.Add(resourceEntry.Key);
                }
            }

            //Debug.Log("Rebuilding before Remove.");
            foreach (string resource in toDelete)
            {
                resources.Remove(resource);
            }

            //Debug.Log("Rebuilding before  vessel.parts.");
            foreach (Part part in vessel.parts)
            {
                foreach (PartResource resource in part.Resources)
                {
                    //Debug.Log("Rebuilding  resource.resourceName="+resource.resourceName);
                    if (resources.ContainsKey(resource.resourceName))
                    {
                        List<ResourcePartMap> resourceParts = resources[resource.resourceName].parts;
                        ResourcePartMap partInfo = resourceParts.Find(info => info.part.Equals(part));

                        if (partInfo == null)
                        {
                            resourceParts.Add(new ResourcePartMap(resource, part));
                        }
                        else
                        {
                            // Make sure we are still pointing at the right resource instance. This is a fix for compatibility with StretchyTanks.
                            partInfo.resource = resource;
                        }
                    }
                    else
                    {
                        ResourceInfo resourceInfo = new ResourceInfo();
                        resourceInfo.parts.Add(new ResourcePartMap(resource, part));

                        resources[resource.resourceName] = resourceInfo;
                    }
                }
            }

            //Debug.Log("Rebuilding before  vessel.parts.Count.");
            numberOfParts = vessel.parts.Count;
            currentVessel = vessel;
            vesselSituation = vessel.situation;
            //Debug.Log("Rebuilding  rnumberOfParts=" + numberOfParts);
            //Debug.Log("Rebuilding  currentVessel=" + currentVessel);
            //Debug.Log("Rebuilding  vesselSituation=" + vesselSituation);
            //Debug.Log("Rebuilding done");
        }
        public Dictionary<string, ResourceInfo> GetResourceInfo()
        {
            //Debug.Log("ResourceRecovery: Utilities  GetResourceInfo 1");
            //Debug.Log("ResourceRecovery: Utilities  GetResourceInfo 2");
            return resources;
        }
    }

}
