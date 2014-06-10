using KSP.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Philotical
{
    /*
* This gets created when the game loads the Space Center scene. It then checks to make sure
* the scenarios have been added to the game (so they will be automatically created in the
* appropriate scenes).
*/
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class AddScenarioModules : MonoBehaviour
    {
        void Start()
        {
            var game = HighLogic.CurrentGame;

            ProtoScenarioModule psm = game.scenarios.Find(s => s.moduleName == typeof(ResourceRecovery).Name);
            if (psm == null)
            {
                Debug.Log("Adding the scenario module.");
                psm = game.AddProtoScenarioModule(typeof(ResourceRecovery), GameScenes.SPACECENTER,
                    GameScenes.FLIGHT, GameScenes.EDITOR, GameScenes.SPH);
            }
            else
            {
                if (!psm.targetScenes.Any(s => s == GameScenes.SPACECENTER))
                {
                    psm.targetScenes.Add(GameScenes.SPACECENTER);
                }
                if (!psm.targetScenes.Any(s => s == GameScenes.FLIGHT))
                {
                    psm.targetScenes.Add(GameScenes.FLIGHT);
                }
            }
        }
    }

    public class ResourceRecovery : ScenarioModule
    {
        public static ResourceRecovery Instance { get; private set; }

        public ResourceRecoveryGameSettings gameSettings { get; private set; }
        public ResourceRecoveryGlobalSettings globalSettings { get; private set; }

        private readonly string globalConfigFilename;
        private ConfigNode globalNode = new ConfigNode();

        private readonly List<Component> children = new List<Component>();

        public ResourceRecovery()
        {
            Debug.Log("Constructor");
            Instance = this;
            gameSettings = new ResourceRecoveryGameSettings();
            globalSettings = new ResourceRecoveryGlobalSettings();

            globalConfigFilename = IOUtils.GetFilePathFor(this.GetType(), "LifeSupport.cfg");
        }

        public override void OnAwake()
        {
            Debug.Log("OnAwake in " + HighLogic.LoadedScene);
            base.OnAwake();

            if (HighLogic.LoadedScene == GameScenes.SPACECENTER)
            {
                Debug.Log("Adding ResourceRecoverySpaceCenterManager");
                var c = gameObject.AddComponent<ResourceRecoverySpaceCenterManager>();
                children.Add(c);
            }
            else if (HighLogic.LoadedScene == GameScenes.FLIGHT)
            {
                Debug.Log("Adding ResourceRecoveryController");
                var c = gameObject.AddComponent<ResourceRecoveryController>();
                children.Add(c);
            }
        }

        public override void OnLoad(ConfigNode gameNode)
        {
            base.OnLoad(gameNode);
            gameSettings.Load(gameNode);

            // Load the global settings
            if (File.Exists<ResourceRecovery>(globalConfigFilename))
            {
                globalNode = ConfigNode.Load(globalConfigFilename);
                globalSettings.Load(globalNode);
                foreach (Savable s in children.Where(c => c is Savable))
                {
                    s.Load(globalNode);
                }
            }

            Debug.Log("OnLoad: " + gameNode + "\n" + globalNode);
        }

        public override void OnSave(ConfigNode gameNode)
        {
            base.OnSave(gameNode);
            gameSettings.Save(gameNode);

            // Save the global settings
            globalSettings.Save(globalNode);
            foreach (Savable s in children.Where(c => c is Savable))
            {
                s.Save(globalNode);
            }
            globalNode.Save(globalConfigFilename);

            Debug.Log("OnSave: " + gameNode + "\n" + globalNode);
        }

        void OnDestroy()
        {
            Debug.Log("Main OnDestroy");
            foreach (Component c in children)
            {
                Destroy(c);
            }
            children.Clear();
        }
    }

    interface Savable
    {
        void Load(ConfigNode globalNode);
        void Save(ConfigNode globalNode);
    }
}