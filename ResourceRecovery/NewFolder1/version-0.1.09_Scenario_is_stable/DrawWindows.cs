
using KSPPluginFramework;
using UnityEngine;


namespace Philotical
{
    class WindowsController : MonoBehaviourWindow
    {

        public static string currentGameScene;
        public Vector2 scrollPosition { get; set; }
        internal static ConfigNode RootNode = null;

        /*
        internal static bool firstRun = false;
        internal static int RunCount = 0;
        internal static float lastUpdate = 0.00F;
        private float logInterval = 0.50f;
        */

        internal override void Awake()
        {
            //WindowCaption = "Resource Recovery (Flight)";
            WindowRect = new Rect(0, 0, 550, 650);
            Visible = false;
            scrollPosition = Vector2.zero;
            DragEnabled = true;
            TooltipsEnabled = true;
        }
        internal void pass_Scenario()
        {
        }

        internal override void Update()
        {
            /*
            //toggle whether its visible or not
            if (HighLogic.LoadedScene == GameScenes.SPACECENTER)
            {
                //Debug.LogError("KspCelestialOverlay Start not in trackstation wtf?!");
                //toggle whether its visible or not
                if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.F10))
                {
                    Visible = !Visible;
                }
                WindowCaption = "Resource Recovery (SpaceCentre)";
                currentGameScene = "SPACECENTER";
                //WindowRect.width = Screen.width - 100;
                //WindowRect.height = Screen.height - 100;
                WindowRect.Set(50, 50, Screen.width - 100, Screen.height - 100);
            }
            if (HighLogic.LoadedScene == GameScenes.FLIGHT)
            {
                //Debug.LogError("KspCelestialOverlay Start not in trackstation wtf?!");
                //toggle whether its visible or not
                if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.F10))
                {
                    Visible = !Visible;
                }
                WindowCaption = "Resource Recovery (Flight)";
                currentGameScene = "FLIGHT";
            }

            if (HighLogic.LoadedScene == GameScenes.TRACKSTATION)
            {
                //Debug.LogError("KspCelestialOverlay Start not in trackstation wtf?!");
                if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.F10))
                {
                    Visible = !Visible;
                }
                WindowCaption = "Resource Recovery (TrackingStation)";
                currentGameScene = "TRACKSTATION";
                //WindowRect.width = Screen.width - 100;
                //WindowRect.height = Screen.height - 100;
                WindowRect.Set(50, 50, Screen.width - 100, Screen.height - 100);
            }
            //WindowsController.DrawWindow(id);
            */
        }
        // Is called for every Window Content in the Plugin
        internal override void DrawWindow(int id)
        {
            /*
            //Debug.Log("ResourceRecovery: DrawWindows: currentGameScene=" + currentGameScene);
            if (currentGameScene == "SPACECENTER")
            {
                    SC_Window sc = new SC_Window();
                    GUILayout.BeginHorizontal();
                    if (GUI.Button(new Rect(WindowRect.width - 25, 2f, 23f, 23f), "X"))
                    {
                        Visible = !Visible;
                    }
                    GUILayout.EndHorizontal();
                    sc.SC_Window_main(id, Scenario);
                    currentGameScene = "SPACECENTER";
            }
            else if (currentGameScene == "TRACKSTATION")
            {
                TS_Window ts = new TS_Window();
                GUILayout.BeginHorizontal();
                if (GUI.Button(new Rect(WindowRect.width - 25, 2f, 23f, 23f), "X"))
                {
                    Visible = !Visible;
                }
                GUILayout.EndHorizontal();
                ts.TS_Window_main(id, Scenario);
                currentGameScene = "TRACKSTATION";
            }
            else if (currentGameScene == "FLIGHT")
            {
                F_Window fw = new F_Window();
                GUILayout.BeginHorizontal();
                if (GUI.Button(new Rect(WindowRect.width - 25, 2f, 23f, 23f), "X"))
                {
                    Visible = !Visible;
                }
                GUILayout.EndHorizontal();
                fw.F_Window_main(id, Scenario);
                currentGameScene = "FLIGHT";
            }
        */
        }
        
        // displays the Window Content for SpaceCenter Scene
 
        // displays the Window Content for FLIGHT Scene
        internal void F_Window(int id)
        {
        }

        // displays the Window Content for TRACKSTATION Scene

    }
}
