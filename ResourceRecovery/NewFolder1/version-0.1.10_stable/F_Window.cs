using KSPPluginFramework;
using UnityEngine;

namespace Philotical
{
    internal class F_Window : MonoBehaviourWindow
    {
        public static string currentGameScene;
        internal string main_nav_selection;
        internal static ConfigNode RootNode = null;

        internal void F_Window_main(int id)
        {
            currentGameScene = "FLIGHT";

            // nav buttons MainMenu
            GUILayout.BeginHorizontal();
            main_nav_selection = ButtonController.getMain_nav_selection();
            ButtonController buttons = new ButtonController(main_nav_selection);
            string[] bid = new string[] { "refuel", "settings", "help" };
            ButtonController.GetMainNavButtons(bid);
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            if (main_nav_selection == "help")
            {
                HelpWindowController help = new HelpWindowController();
                help.HelpMain(id);
            }
            else if (main_nav_selection == "settings")
            {
                settingsWindowController set = new settingsWindowController();
                set.settingsMain(id);
            }
            else
            {
                FlightRefuelController Fref = new FlightRefuelController();
                Fref.RefuelMain(id);
            }
            GUILayout.EndVertical();


            GUILayout.BeginHorizontal();
            //Debug.Log("ResourceRecovery: DrawWindows: pre CelestialBody");
            //GUILayout.Label(String.Format("current_planet:{0}", Utilities.get_current_planet()));
            GUILayout.Label("Alt+F10 - shows/hides window");
            GUILayout.EndHorizontal();
        }
        internal override void DrawWindow(int id)
        {
            //SC_Window_main(id);
        }
    }
}
