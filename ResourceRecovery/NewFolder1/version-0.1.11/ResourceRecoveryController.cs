using KSPPluginFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Philotical
{
    /*
    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    [KSPAddon(KSPAddon.Startup.Instantly, false)]
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
    [KSPAddon(KSPAddon.Startup.TrackingStation, false)]
    [KSPAddon(KSPAddon.Startup.Editor, false)]
    [KSPAddon(KSPAddon.Startup.SPH, false)]
    */

    //[KSPAddon(KSPAddon.Startup.TrackingStation, false)]
    //[KSPAddon(KSPAddon.Startup.Flight, false)]
    //[KSPAddon(KSPAddon.Startup.EditorAny, false)]

    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    [WindowInitials(Caption = "ResourceRecovery", Visible = false, DragEnabled = true, ClampToScreen = true, TooltipsEnabled = true)]
    public class ResourceRecoveryController_SpaceCentre : MonoBehaviour
    {
        private static WindowsController windows;
        private int frame_update_counter;
        public static ConfigNode RRSettings;
        internal void Awake()
        {
            //Debug.Log("ResourceRecovery Awake running");
            RRSettings = RRSettingsController.getRRSettings();
            RRSettings = RRSettingsController.getFilePaths();
            ResourceRecoveryData currentGame = ResourceRecoveryData.controller;

            //Debug.Log("ResourceRecovery Awake currentGame=" + currentGame);
            windows = new WindowsController();
            windows.Awake();
            windows.Visible = false;
            Debug.Log("ResourceRecovery Awake Done");
        }
        internal void Start()
        {
            //Debug.Log("ResourceRecovery Start running ");
            Debug.Log("ResourceRecovery Start done ");
        }
        internal void OnDestroy()
        {
            //Debug.Log("ResourceRecovery OnDestroy running");
            Debug.Log("ResourceRecovery OnDestroy done");
        }
        internal void Update()
        {
            frame_update_counter++;
            windows.Update();
        }
        public static void ToggleVisible()
        {
            windows.Visible = !windows.Visible;
        }
    }

    [KSPAddon(KSPAddon.Startup.TrackingStation, false)]
    [WindowInitials(Caption = "ResourceRecovery", Visible = false, DragEnabled = true, ClampToScreen = true, TooltipsEnabled = true)]
    public class ResourceRecoveryController_TrackingStation : MonoBehaviour
    {
        private static WindowsController windows;
        private int frame_update_counter;
        public static ConfigNode RRSettings;
        internal void Awake()
        {
            Debug.Log("ResourceRecovery Awake running");
            RRSettings = RRSettingsController.getRRSettings();
            ResourceRecoveryData currentGame = ResourceRecoveryData.controller;
            Debug.Log("ResourceRecovery Awake currentGame=" + currentGame);
            windows = new WindowsController();
            windows.Awake();
            Debug.Log("ResourceRecovery Awake Done");
        }
        internal void Start()
        {
            Debug.Log("ResourceRecovery Start running ");
            Debug.Log("ResourceRecovery Start done ");
        }
        internal void OnDestroy()
        {
            Debug.Log("ResourceRecovery OnDestroy running");
            Debug.Log("ResourceRecovery OnDestroy done");
        }
        internal void Update()
        {
            frame_update_counter++;
            windows.Update();
        }
    }

    [KSPAddon(KSPAddon.Startup.Flight, false)]
    [WindowInitials(Caption = "ResourceRecovery", Visible = false, DragEnabled = true, ClampToScreen = true, TooltipsEnabled = true)]
    public class ResourceRecoveryController_Flight : MonoBehaviour
    {
        private static WindowsController windows;
        private int frame_update_counter;
        public static ConfigNode RRSettings;
        internal void Awake()
        {
            Debug.Log("ResourceRecovery Awake running");
            RRSettings = RRSettingsController.getRRSettings();
            ResourceRecoveryData currentGame = ResourceRecoveryData.controller;
            Debug.Log("ResourceRecovery Awake currentGame=" + currentGame);
            windows = new WindowsController();
            windows.Awake();
            Debug.Log("ResourceRecovery Awake Done");
        }
        internal void Start()
        {
            Debug.Log("ResourceRecovery Start running ");
            Debug.Log("ResourceRecovery Start done ");
        }
        internal void OnDestroy()
        {
            Debug.Log("ResourceRecovery OnDestroy running");
            Debug.Log("ResourceRecovery OnDestroy done");
        }
        internal void Update()
        {
            frame_update_counter++;
            windows.Update();
        }
    }
}
