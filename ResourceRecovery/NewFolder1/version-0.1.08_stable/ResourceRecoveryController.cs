using KSPPluginFramework;
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
        public static ConfigNode RootNode = null;
        public static ConfigNode RRSettings;
        public static ConfigNode FilePaths;
        private static WindowsController windows;
        public static ScenarioNodeDatabase Scenario = null;


        internal void Awake()
        {
            Debug.Log("ResourceRecovery Awake running");
            RRSettings = RRSettingsController.getRRSettings();
            FilePaths = RRSettingsController.getFilePaths();
            //RootNode = Utilities.openPersistenFile();
            //Scenario = (Scenario == null) ? new ScenarioNodeDatabase() : Scenario;
            Scenario = new ScenarioNodeDatabase() ;
            Scenario.init(RRSettings, FilePaths);
            Scenario.ValidateDatabaseForPlanetaryBody();
            /*
            */
            windows = new WindowsController();
            windows.Awake();
            windows.pass_Scenario(Scenario);
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
            //RootNode = Scenario.get_RootNode();
            //Utilities.SavePersistenFile(RootNode);
            Debug.Log("ResourceRecovery OnDestroy done");
        }
        internal void Update()
        {
                windows.Update();

        }
    }

    
    [KSPAddon(KSPAddon.Startup.TrackingStation, false)]
    [WindowInitials(Caption = "ResourceRecovery", Visible = false, DragEnabled = true, ClampToScreen = true, TooltipsEnabled = true)]
    public class ResourceRecoveryController_TrackingStation : MonoBehaviour
    {

        public static ConfigNode RootNode=null;
        public static ConfigNode RRSettings;
        public static ConfigNode FilePaths;
        private static WindowsController windows;
        public static ScenarioNodeDatabase Scenario = null;


        internal void Awake()
        {
            Debug.Log("ResourceRecovery Awake running");

            RRSettings = RRSettingsController.getRRSettings();
            FilePaths = RRSettingsController.getFilePaths();
            //RootNode = Utilities.openPersistenFile();
            //Scenario = (Scenario == null) ? new ScenarioNodeDatabase() : Scenario;
            Scenario = new ScenarioNodeDatabase();
            Scenario.init(RRSettings, FilePaths);
            Scenario.ValidateDatabaseForPlanetaryBody();
            windows = new WindowsController();
            windows.Awake();
            windows.pass_Scenario(Scenario);
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
            //RootNode = Scenario.get_RootNode();
            //Utilities.SavePersistenFile(RootNode);
            Debug.Log("ResourceRecovery OnDestroy done");
        }

        internal void Update()
        {
            windows.Update();

        }
    }

    [KSPAddon(KSPAddon.Startup.Flight, false)]
    [WindowInitials(Caption = "ResourceRecovery", Visible = false, DragEnabled = true, ClampToScreen = true, TooltipsEnabled = true)]
    public class ResourceRecoveryController_Flight : MonoBehaviour
    {
        internal static ConfigNode RootNode = null;
        internal static ConfigNode RRSettings;
        internal static ConfigNode FilePaths;
        internal static WindowsController windows;
        internal static Vessel vessel;
        public static ScenarioNodeDatabase Scenario = null;


        internal void Awake()
        {
            Debug.Log("ResourceRecovery Awake running");

            vessel = FlightGlobals.ActiveVessel;
            RRSettings = RRSettingsController.getRRSettings();
            FilePaths = RRSettingsController.getFilePaths();
            //Scenario = (Scenario == null) ? new ScenarioNodeDatabase() : Scenario;
            Scenario = new ScenarioNodeDatabase();
            Scenario.init(RRSettings, FilePaths);
            Scenario.open();
            //RootNode = Scenario.get_RootNode();
            Scenario.ValidateDatabaseForPlanetaryBody();

            windows = new WindowsController();
            windows.Awake();
            windows.pass_Scenario(Scenario);
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
            //RootNode = Scenario.get_RootNode();
            //Utilities.SavePersistenFile(RootNode);
            Debug.Log("ResourceRecovery OnDestroy done");
        }
        internal void Update()
        {
            windows.Update();
        }
        public static ScenarioNodeDatabase get_Scenario()
        {
            return Scenario;
        }
    }
}
