using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KSP;
//using System.Diagnostics.FileVersionInfo;
using System.Diagnostics;
using System.Reflection;

//kethane_version
namespace Philotical
{
    public class RecourceRecoveryData : ScenarioModule
    {
        /*
        [KSPField]
        [KSPField(isPersistant = true)];
        [KSPField(isPersistant = true)];
        */
        public ConfigNode planetNode;
        public ConfigNode LocationsNode;

        public static RecourceRecoveryData Current
        {
            get
            {
                UnityEngine.Debug.Log("RecourceRecoveryData Current running 1 ");
                var game = HighLogic.CurrentGame;
                UnityEngine.Debug.Log("RecourceRecoveryData Current running 2 ");
                if (game == null) { return null; }
                UnityEngine.Debug.Log("RecourceRecoveryData Current running 3 ");

                if (!game.scenarios.Any(p => p.moduleName == typeof(RecourceRecoveryData).Name))
                {
                    UnityEngine.Debug.Log("RecourceRecoveryData Current running 4 ");
                    //var proto = game.AddProtoScenarioModule(typeof(RecourceRecoveryData), GameScenes.SPACECENTER, GameScenes.FLIGHT, GameScenes.TRACKSTATION);
                    var proto = game.AddProtoScenarioModule(typeof(RecourceRecoveryData), GameScenes.SPACECENTER, GameScenes.FLIGHT, GameScenes.TRACKSTATION);
                    UnityEngine.Debug.Log("RecourceRecoveryData Current running 5 ");
                    if (proto.targetScenes.Contains(HighLogic.LoadedScene))
                    {
                        UnityEngine.Debug.Log("RecourceRecoveryData Current running 6 ");
                        proto.Load(ScenarioRunner.fetch);
                        UnityEngine.Debug.Log("RecourceRecoveryData Current running 7 ");
                    }
                }
                UnityEngine.Debug.Log("RecourceRecoveryData Current running 8 ");

                return game.scenarios.Select(s => s.moduleRef).OfType<RecourceRecoveryData>().SingleOrDefault();
            }
        }

        public override void OnLoad(ConfigNode config)
        {
            var timer = System.Diagnostics.Stopwatch.StartNew();
            string persistentfile = KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/persistent.sfs";
            ConfigNode oldConfig = ConfigNode.Load(persistentfile);
            ConfigNode gameconf = config.GetNode("GAME");
            ConfigNode Scenario = gameconf.GetNode("SCENARIO");

            UnityEngine.Debug.Log("RecourceRecoveryData OnLoad running 1 config=" + config);
            /*
            UnityEngine.Debug.Log("RecourceRecoveryData OnLoad running 2" );
            string currentPlanet = "Kerbin";
            UnityEngine.Debug.Log("RecourceRecoveryData OnLoad running 3" );
            if (!config.HasValue("Version") && (config.CountNodes < 1))
            {
                UnityEngine.Debug.Log("RecourceRecoveryData OnLoad running 4");
                //config.AddValue("Version", System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.ResourceAssembly.Location).ProductVersion);

                //config.AddValue("Version", GetInformationalVersion(Windows.Application.ResourceAssembly));

                //config = new ConfigNode();
                
                planetNode = config.AddNode(currentPlanet);
                UnityEngine.Debug.Log("RecourceRecoveryData OnLoad running 5");
                planetNode = config.GetNode(currentPlanet);
                UnityEngine.Debug.Log("RecourceRecoveryData OnLoad running 6");
                planetNode.AddValue("name", currentPlanet);
                planetNode.AddValue("total_locations", "ToDo");
                planetNode.AddValue("total_number_recource_tanks", "ToDo");
                planetNode.AddValue("total_capacity", "ToDo");
                planetNode.AddValue("total_capacity_used","ToDo");
                UnityEngine.Debug.Log("RecourceRecoveryData OnLoad running 7");

                LocationsNode = planetNode.AddNode("LOCATIONS");
                UnityEngine.Debug.Log("RecourceRecoveryData OnLoad running 8");
                LocationsNode = config.GetNode("LOCATIONS");
                UnityEngine.Debug.Log("RecourceRecoveryData OnLoad running 9");

                ConfigNode storageNode = planetNode.AddNode("STORAGE");
                UnityEngine.Debug.Log("RecourceRecoveryData OnLoad running 10");
                storageNode = planetNode.GetNode("STORAGE");
                UnityEngine.Debug.Log("RecourceRecoveryData OnLoad running 11");
                storageNode.AddValue("capacity", "ToDo");
                UnityEngine.Debug.Log("RecourceRecoveryData OnLoad running 12");
                storageNode.AddValue("modulecount", "ToDo");
                UnityEngine.Debug.Log("RecourceRecoveryData OnLoad running 13");



            }

            //planetNode = config.GetNode(currentPlanet);

            UnityEngine.Debug.Log("RecourceRecoveryData OnLoad running 14");
            */
            timer.Stop();
            UnityEngine.Debug.Log("RecourceRecoveryData OnLoad running 15");
            UnityEngine.Debug.Log(String.Format("RecourceRecoveryData loaded ({0}ms)", timer.ElapsedMilliseconds));
            UnityEngine.Debug.Log("RecourceRecoveryData OnLoad running 16 config=" + config);
        }
        public override void OnSave(ConfigNode configNode)
        {
            var timer = System.Diagnostics.Stopwatch.StartNew();

 
            timer.Stop();
            UnityEngine.Debug.Log(String.Format("RecourceRecoveryData saved ({0}ms)", timer.ElapsedMilliseconds));
        }
    }
}
