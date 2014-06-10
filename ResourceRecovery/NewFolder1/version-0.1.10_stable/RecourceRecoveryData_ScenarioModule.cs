using System;
using System.Collections.Generic;
using UnityEngine;


//SCAN version

namespace Philotical
{
    // used to store/access the SubRootNode through all classes
    public static class SubRootNode
    {
        public static ConfigNode sub_root_node;
        static SubRootNode()
        {
        }
        public static ConfigNode get_sub_root()
        {
            //Debug.Log("ResourceRecovery Foo get_root sub_root_node=" + sub_root_node);
            return sub_root_node;
        }
        public static void set_sub_root(ConfigNode foo)
        {
            sub_root_node = foo;
        }
    }
    // used to store/access the ScenarioAttributes through all classes
    public static class ScenarioAttributes
    {
        // get the rootnode from ProtoScenarioModule, split the subrootnode and work with the attributes
        public static Dictionary<string, string> ScenarioAttr;
        static ScenarioAttributes()
        {
        }
        public static void InitScenarioAttributes(ConfigNode attributes)
        {
            ScenarioAttr = new Dictionary<string, string>();
            //Debug.Log("ResourceRecovery InitScenarioAttributes beginn attributes=" + attributes);
            //Debug.Log("ResourceRecovery InitScenarioAttributes 2 attributes.GetValue(version)=" + attributes.GetValue("version"));
            //Debug.Log("ResourceRecovery InitScenarioAttributes 3 attributes.values=" + attributes.values);
            //Debug.Log("ResourceRecovery InitScenarioAttributes 3 attributes.values.Count=" + attributes.values.Count);
            ConfigNode.ValueList SC = attributes.values;
            int i = 0;
            foreach (ConfigNode.Value value in SC)
            {
                string v = value.name;
                //Debug.Log("ResourceRecovery InitScenarioAttributes 4 foreach v=" + v);
                //Debug.Log("ResourceRecovery InitScenarioAttributes 4 foreach attributes.GetValue(v)=" + attributes.GetValue(v));
                ScenarioAttr.Add(value.name, value.value);
                i++;
            }

            ScenarioAttr["dll_version"] = RRSettingsController.getSetting("PluginVersion");


            //Debug.Log("ResourceRecovery InitScenarioAttributes 5 ");

            ConfigNode sub_root = attributes.GetNode("PLANETS");
            //Debug.Log("ResourceRecovery InitScenarioAttributes 6 sub_root=" + sub_root);
            //Send subrootnode further
            RRDATAcontroller.init(sub_root);
            //Debug.Log("ResourceRecovery InitScenarioAttributes 7 ");
        }
        public static string get_ScenarioAttribute(string attributName)
        {
            //Debug.Log("ResourceRecovery get_ScenarioAttribute  "+attributName+"=" + ScenarioAttr[attributName]);
            return ScenarioAttr[attributName];
        }
        public static void set_ScenarioAttribute(string attributName, string newValue)
        {
            //Debug.Log("ResourceRecovery set_ScenarioAttribute  " + attributName + "=" + newValue);
            /*
            foreach (KeyValuePair<string, string> kvp in ScenarioAttr)
            { 
                Debug.Log("ResourceRecovery set_ScenarioAttribute pre kvp.Key=" + kvp.Key + "  kvp.Value=" + kvp.Value);
            }
            */
            ScenarioAttr[attributName] = newValue;
            /*
            foreach (KeyValuePair<string, string> kvp in ScenarioAttr)
            {
                Debug.Log("ResourceRecovery set_ScenarioAttribute post kvp.Key=" + kvp.Key + "  kvp.Value=" + kvp.Value);
            }
            */
        }

    }


    public class ResourceRecoveryData : ScenarioModule
    {
        public static ResourceRecoveryData controller
        {
            get
            {
                /*
                 * begin install
                */
                string persistentfile = KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/persistent.sfs";
                ConfigNode check0 = ConfigNode.Load(persistentfile);
                ConfigNode check_game = check0.GetNode("GAME");
                ConfigNode[] check1 = check_game.GetNodes("SCENARIO");
                bool install = true;
                foreach (ConfigNode check2 in check1)
                {
                    if (check2.GetValue("name") == "ResourceRecoveryData")
                    {
                        install = false;
                    }
                }
                if (install)
                {
                    Debug.Log("ResourceRecovery ResourceRecoveryData install running");
                    ConfigNode SCENARIO = new ConfigNode("SCENARIO");
                    string new_version = RRSettingsController.getSetting("PluginVersion");
                    string new_created = DateTime.Now.ToString();
                    string new_updated = "fresh install";
                    SCENARIO.AddValue("name", "ResourceRecoveryData");
                    SCENARIO.AddValue("scene", "7, 8, 5");
                    SCENARIO.AddValue("dll_version", new_version);
                    SCENARIO.AddValue("filesystem_version", new_version);
                    SCENARIO.AddValue("created", new_created);
                    SCENARIO.AddValue("updated", new_updated);
                    SCENARIO.AddValue("SaveGameKredits", "0");
                    SCENARIO.AddValue("loadcount", "0");
                    SCENARIO.AddNode("PLANETS");
                    check_game.AddNode(SCENARIO);
                    check0.Save(persistentfile);
                    Debug.Log("ResourceRecovery ResourceRecoveryData install done SCENARIO=" + SCENARIO);
                }
                /*
                 * ende install
                */

                Game g = HighLogic.CurrentGame;
                if (g == null) return null;
                foreach (ProtoScenarioModule mod in g.scenarios)
                {
                    if (mod.moduleName == typeof(ResourceRecoveryData).Name)
                    {
                        Debug.Log("ResourceRecoveryData controller found Scenario mod.moduleName=" + mod.moduleName);
                        return (ResourceRecoveryData)mod.moduleRef;
                    }
                }
                Debug.Log("ResourceRecoveryData controller did not find Scenario make new one" );
                return (ResourceRecoveryData)g.AddProtoScenarioModule(typeof(ResourceRecoveryData), GameScenes.SPACECENTER, GameScenes.FLIGHT, GameScenes.TRACKSTATION).moduleRef;
            }
            private set { }
        }

        [KSPField(isPersistant = true)]
        private string dll_version = "ToDo";
        [KSPField(isPersistant = true)]
        private string filesystem_version = "ToDo";

        [KSPField(isPersistant = true)]
        private string created = "ToDo";

        [KSPField(isPersistant = true)]
        private string updated = "ToDo";

        [KSPField(isPersistant = true)]
        private string SaveGameKredits = "ToDo";

        [KSPField(isPersistant = true)]
        private string loadcount = "1";


        public override void OnLoad(ConfigNode RRDATA_tmp)
        {
                //Debug.Log("ResourceRecovery ResourceRecoveryData OnLoad beginn RRDATA_tmp=" + RRDATA_tmp);
                ConfigNode sub_root = RRDATA_tmp.GetNode("PLANETS");
                SubRootNode.set_sub_root(sub_root);
                ScenarioAttributes.InitScenarioAttributes(RRDATA_tmp);
                ScenarioAttributes.set_ScenarioAttribute("version", "0.1.09");

        }
        public override void OnSave(ConfigNode config)
        {
            //Debug.Log("ResourceRecovery ResourceRecoveryData OnSave beginn1 config=" + config);
            //Debug.Log("ResourceRecovery ResourceRecoveryData OnSave beginn2 Foo.get_root()=" + SubRootNode.get_root());
            
            string lc = config.GetValue("loadcount");
            int lc2 = 0;
            Int32.TryParse(lc, out lc2);
            lc2 = lc2+1;
            config.SetValue("loadcount", lc2.ToString());

            config.SetValue("updated", DateTime.Now.ToString());

            Debug.Log("ResourceRecovery ResourceRecoveryData OnSave ScenarioAttributes.get_ScenarioAttribute(version)=" + ScenarioAttributes.get_ScenarioAttribute("version"));
            config.SetValue("dll_version", ScenarioAttributes.get_ScenarioAttribute("dll_version"));
            config.SetValue("filesystem_version", ScenarioAttributes.get_ScenarioAttribute("filesystem_version"));

            config.SetValue("SaveGameKredits", ScenarioAttributes.get_ScenarioAttribute("SaveGameKredits"));
            
            config.AddNode(SubRootNode.get_sub_root());

            Debug.Log("ResourceRecovery ResourceRecoveryData OnSave done config=" + config);
        }


    }

}
