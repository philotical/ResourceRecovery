using KSPPluginFramework;
using UnityEngine;

namespace Philotical
{
    internal class SC_Window : MonoBehaviourWindow
    {
        public static string currentGameScene;
        internal string main_nav_selection;
        internal static ConfigNode RootNode = null;

        internal void SC_Window_main(int id)
        {
            currentGameScene = "SPACECENTER";

            //Debug.Log("ResourceRecovery: SC_Window: 1");

            // nav buttons MainMenu
            GUILayout.BeginHorizontal();
            main_nav_selection = ButtonController.getMain_nav_selection();
            ButtonController buttons = new ButtonController(main_nav_selection);
            string[] bid = new string[] { "manifest", "marketplace", "settings", "help" };
            ButtonController.GetMainNavButtons(bid);
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            if (main_nav_selection == "help")
            {
                HelpWindowController help = new HelpWindowController();
                help.HelpMain(id);
            }
            else if (main_nav_selection == "marketplace")
            {
                MarketplaceController mark = new MarketplaceController();
                mark.MarketplaceMain(id);
            }
            else if (main_nav_selection == "settings")
            {
                settingsWindowController set = new settingsWindowController();
                set.settingsMain(id);
            }
            else
            {
                //Debug.Log("ResourceRecovery: SC_Window: 2");
                ManifestController man = new ManifestController();
                man.ManifestMain(id);
                //Debug.Log("ResourceRecovery: SC_Window: 3");
            }
        /*
        */
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
