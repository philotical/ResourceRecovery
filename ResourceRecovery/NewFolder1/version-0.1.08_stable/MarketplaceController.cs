using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Philotical
{
    class MarketplaceController
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
        public static GUIStyle buttonStyle_small;
        public static GUIStyle buttonStyle;
        public static GUIStyle buttonStyle_active;
        public static GUIStyle splitter;
        public static ConfigNode RootNode;
        public static ScenarioNodeDatabase Scenario = null;


        internal void MarketplaceMain(int id, ScenarioNodeDatabase Sc)
        {
            Scenario = Sc;

            style = RRSettingsController.get_label_default();
            style_10px_row = RRSettingsController.get_label_10px();
            style_20px_row = RRSettingsController.get_label_20px();
            style_40px_row = RRSettingsController.get_label_40px();
            style_60px_row = RRSettingsController.get_label_60px();
            style_80px_row = RRSettingsController.get_label_80px();
            style_100px_row = RRSettingsController.get_label_100px();
            style_120px_row = RRSettingsController.get_label_120px();
            style_stretchWidth_row = RRSettingsController.get_label_stretchWidth();
            buttonStyle_small = RRSettingsController.get_button_small();
            buttonStyle = RRSettingsController.get_button_100px();
            buttonStyle_active = RRSettingsController.get_button_100px_active();
            splitter = RRSettingsController.get_splitter();

            RootNode = Scenario.get_RootNode();
            Scenario.update_StorageTotals();


            GUILayout.Label("Marketplace: ToDo", style);

            string currentPlanet = Utilities.get_current_planet();
            ConfigNode PlanetNode = Scenario.get_PlanetNode(currentPlanet);
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
            ConfigNode SCENARIO = RootNode.GetNode("SCENARIO");
            string kred_string = SCENARIO.GetValue("SaveGameKredits");
            GUILayout.Label(String.Format("Kredits: {0}", kred_string), style);

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

            ConfigNode RNode_tmp = Scenario.get_ResourcesNode();
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
            var sortedRNodes = RNodesList.OrderBy(x => x.display_order).ToList();
            foreach (ResourceNodeType node in sortedRNodes)
            {
                    _expanded.Add(false);
                    //GUILayout.Label(String.Format("{0}:{1}", node.GetValue("name"), node.GetValue("storedamount")));
                    ResourceRow(node);
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
            GUILayout.Label("Name", style_stretchWidth_row);
            GUILayout.Label("Amount", style_100px_row);
            GUILayout.Label("Buy", style, GUILayout.Width(180));
            GUILayout.Label("Mode", style_60px_row);
            GUILayout.Label("Price", style_60px_row);
            GUILayout.Label("Sell", style, GUILayout.Width(200));
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
        internal void ResourceRow(ResourceNodeType node)
        {
            string res_nam = node.name;

            //Debug.Log("ResourceRecovery: ResourceRow: res_nam=" + res_nam + " this_id=" + this_id + " last_id=" + last_id + " next_id=" + next_id);

            GUILayout.BeginHorizontal();
            GUILayout.Label(String.Format("{0}", res_nam + " (" + node.display_order + ")"), style_stretchWidth_row);
            if (node.supply_mode != "0")
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


            double calculated_kredit_value=0;
            Double.TryParse(node.value_factor, out calculated_kredit_value);
            calculated_kredit_value = (node.supply_mode != "2") ? calculated_kredit_value : (calculated_kredit_value*(-1));
            if (node.supply_mode != "2")
            {
                if (GUILayout.Button("1", buttonStyle_small, GUILayout.Width(20)))
                {
                    Debug.Log("Clicked Button = Buy 1" + res_nam);
                    BuyResource(res_nam, calculated_kredit_value, 1);
                }
                if (GUILayout.Button("10", buttonStyle_small, GUILayout.Width(30)))
                {
                    Debug.Log("Clicked Button = Buy 10" + res_nam);
                    BuyResource(res_nam, calculated_kredit_value, 10);
                }
                if (GUILayout.Button("100", buttonStyle_small, GUILayout.Width(40)))
                {
                    Debug.Log("Clicked Button = Buy 100" + res_nam);
                    BuyResource(res_nam, calculated_kredit_value, 100);
                }
                if (GUILayout.Button("1000", buttonStyle_small, GUILayout.Width(40)))
                {
                    Debug.Log("Clicked Button = Buy 1000" + res_nam);
                    BuyResource(res_nam, calculated_kredit_value, 1000);
                }
                if (GUILayout.Button("10000", buttonStyle_small, GUILayout.Width(50)))
                {
                    Debug.Log("Clicked Button = Buy 10000" + res_nam);
                    BuyResource(res_nam, calculated_kredit_value, 10000);
                }
            }
            else 
            {
                if (GUILayout.Button("-", buttonStyle_small, GUILayout.Width(20)))
                {
                }
                if (GUILayout.Button("--", buttonStyle_small, GUILayout.Width(30)))
                {
                }
                if (GUILayout.Button("---", buttonStyle_small, GUILayout.Width(40)))
                {
                }
                if (GUILayout.Button("----", buttonStyle_small, GUILayout.Width(40)))
                {
                }
                if (GUILayout.Button("-----", buttonStyle_small, GUILayout.Width(50)))
                {
                }
            }

            int string_number = 0;
            if (Int32.TryParse(node.supply_mode, out string_number))
            {
                GUILayout.Label(String.Format("{0}", RRSettingsController.get_resource_supply_mode_definition(string_number)), style_60px_row);
            }
            else
            {
                GUILayout.Label(String.Format("{0}", node.supply_mode), style_60px_row);
            }

            GUIStyle centeredStyle = new GUIStyle(GUI.skin.label);
            centeredStyle.normal.textColor = Color.white;
            centeredStyle.alignment = TextAnchor.UpperCenter;
            GUILayout.Label(String.Format("{0}", calculated_kredit_value), centeredStyle, GUILayout.Width(60));
            
            if (node.supply_mode!="0")
            {
                if (GUILayout.Button("10000", buttonStyle_small, GUILayout.Width(50)))
                {
                    Debug.Log("Clicked Button = Sell 10000" + res_nam);
                    SellResource(res_nam, calculated_kredit_value, 10000);
                }
                if (GUILayout.Button("1000", buttonStyle_small, GUILayout.Width(40)))
                {
                    Debug.Log("Clicked Button = Sell 1000" + res_nam);
                    SellResource(res_nam, calculated_kredit_value, 1000);
                }
                if (GUILayout.Button("100", buttonStyle_small, GUILayout.Width(40)))
                {
                    Debug.Log("Clicked Button = Sell 100" + res_nam);
                    SellResource(res_nam, calculated_kredit_value, 100);
                }
                if (GUILayout.Button("10", buttonStyle_small, GUILayout.Width(30)))
                {
                    Debug.Log("Clicked Button = Sell 10" + res_nam);
                    SellResource(res_nam, calculated_kredit_value, 10);
                }
                if (GUILayout.Button("1", buttonStyle_small, GUILayout.Width(20)))
                {
                    Debug.Log("Clicked Button = Sell 1" + res_nam);
                    SellResource(res_nam, calculated_kredit_value, 1);
                }
            }
            else
            {
                if (GUILayout.Button("-----", buttonStyle_small, GUILayout.Width(50)))
                {
                }
                if (GUILayout.Button("----", buttonStyle_small, GUILayout.Width(40)))
                {
                }
                if (GUILayout.Button("---", buttonStyle_small, GUILayout.Width(40)))
                {
                }
                if (GUILayout.Button("--", buttonStyle_small, GUILayout.Width(30)))
                {
                }
                if (GUILayout.Button("-", buttonStyle_small, GUILayout.Width(20)))
                {
                }
            }
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal(splitter); GUILayout.EndHorizontal();

        }

        private void BuyResource(string res_nam, double price, double buyAmount)
        {
            
            // get Kredit data
            ConfigNode SCENARIO = RootNode.GetNode("SCENARIO");
            string kred_string = SCENARIO.GetValue("SaveGameKredits");
            double kred = 0;
            Double.TryParse(kred_string, out kred);


            // get scenario resource data
            ConfigNode res = Scenario.get_ResourceNode(res_nam);
            double storage_amount = 0;
            string storage_amount_string = res.GetValue("storedamount");
            storage_amount_string = (res.GetValue("supply_mode") != "0") ? storage_amount_string : RRSettingsController.get_KSCStorage_capacity();
            double string_number1 = 0;
            if (Double.TryParse(storage_amount_string, out string_number1))
            { storage_amount = string_number1; }
            else { storage_amount = 0.00; }

            double storage_maxamount = 0;
            string storage_maxamount_string = res.GetValue("storagecapacity");
            double string_number2 = 0;
            if (Double.TryParse(storage_maxamount_string, out string_number2))
            { storage_maxamount = string_number2; }
            else { storage_maxamount = 0.00; }

            double percentFull = storage_amount / storage_maxamount * 100.0;
            double storage_percentFull = percentFull;
            //Debug.Log("ResourceRecovery: SellResource: start res_nam=" + res_nam + " kred=" + kred + " storage_amount=" + storage_amount);

            // get requested action data
            double payment = price * buyAmount;
            //Debug.Log("ResourceRecovery: BuyResource2: res_nam=" + res_nam + " price=" + price + " buyAmount=" + buyAmount + " payment=" + payment);
            if (kred < payment)
            {
                buyAmount = kred / price;
                payment = kred;
            }
            //Debug.Log("ResourceRecovery: BuyResource3: res_nam=" + res_nam + " price=" + price + " buyAmount=" + buyAmount + " payment=" + payment);
            if (buyAmount > storage_maxamount && Utilities.get_current_planet() != RRSettingsController.get_KSCPlanet())
            {
                buyAmount = storage_maxamount;
                payment = buyAmount * price;
            }
            //Debug.Log("ResourceRecovery: BuyResource4: res_nam=" + res_nam + " price=" + price + " buyAmount=" + buyAmount + " payment=" + payment);
            double new_storage_amount = storage_amount + buyAmount;
            double new_Kredit = kred - payment;

            //Debug.Log("ResourceRecovery: BuyResource5: running res_nam=" + res_nam + " new_Kredit=" + new_Kredit + " new_storage_amount=" + new_storage_amount);
            double earned = price * buyAmount;
            //Debug.Log("ResourceRecovery: BuyResource6: done res_nam=" + res_nam + " price=" + price + " buyAmount=" + buyAmount + " earned=" + earned);
            if (Scenario.modify_ResourceNode_by_ResourceName(res_nam, "storedamount", new_storage_amount.ToString()))
            {
                SCENARIO.SetValue("SaveGameKredits", new_Kredit.ToString());
                if (Scenario.save())
                {
                    Debug.Log("ResourceRecovery BuyResource Utilities.SavePluginSaveFile(RootNode) worked");
                }
                else
                {
                    Debug.Log("ResourceRecovery EXCEPTION: BuyResource SavePluginSaveFile failed");
                }
            }

        }

        private void SellResource(string res_nam, double price, double sellAmount)
        {
            // get Kredit data
            ConfigNode SCENARIO = RootNode.GetNode("SCENARIO");
            string kred_string = SCENARIO.GetValue("SaveGameKredits");
            double kred = 0;
            Double.TryParse(kred_string, out kred);

            // get scenario resource data
            ConfigNode res = Scenario.get_ResourceNode(res_nam);
            double storage_amount = 0;
            string storage_amount_string = res.GetValue("storedamount");
            storage_amount_string = (res.GetValue("supply_mode") != "0") ? storage_amount_string : RRSettingsController.get_KSCStorage_capacity();
            double string_number1 = 0;
            if (Double.TryParse(storage_amount_string, out string_number1))
            { storage_amount = string_number1; }
            else { storage_amount = 0.00; }

            double storage_maxamount = 0;
            string storage_maxamount_string = res.GetValue("storagecapacity");
            double string_number2 = 0;
            if (Double.TryParse(storage_maxamount_string, out string_number2))
            { storage_maxamount = string_number2; }
            else { storage_maxamount = 0.00; }

            double percentFull = storage_amount / storage_maxamount * 100.0;
            double storage_percentFull = percentFull;

            //Debug.Log("ResourceRecovery: SellResource: start res_nam=" + res_nam + " kred=" + kred + " storage_amount=" + storage_amount);
            // get requested action data
            double payment = price * sellAmount;
            //Debug.Log("ResourceRecovery: SellResource: running res_nam=" + res_nam + " price=" + price + " sellAmount=" + sellAmount + " payment=" + payment);
            if (sellAmount > storage_amount)
            {
                sellAmount = storage_amount;
                payment = sellAmount * price;
            }
            //Debug.Log("ResourceRecovery: SellResource: running res_nam=" + res_nam + " price=" + price + " sellAmount=" + sellAmount + " payment=" + payment);
            double new_storage_amount = storage_amount - sellAmount;
            double new_Kredit = kred + payment;
            //Debug.Log("ResourceRecovery: SellResource: running res_nam=" + res_nam + " new_Kredit=" + new_Kredit + " new_storage_amount=" + new_storage_amount);
            double earned = price * sellAmount;
            //Debug.Log("ResourceRecovery: SellResource: done res_nam=" + res_nam + " price=" + price + " sellAmount=" + sellAmount + " earned=" + earned);

            if (Scenario.modify_ResourceNode_by_ResourceName(res_nam, "storedamount", new_storage_amount.ToString()))
            {
                SCENARIO.SetValue("SaveGameKredits", new_Kredit.ToString());
                if (Scenario.save())
                {
                    Debug.Log("ResourceRecovery perform_resource_transfer Utilities.SavePluginSaveFile(RootNode) worked");
                }
                else
                {
                    Debug.Log("ResourceRecovery EXCEPTION: perform_resource_transfer SavePluginSaveFile failed");
                }
            }

        }


    }
}
