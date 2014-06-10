using KSPPluginFramework;
using UnityEngine;

namespace Philotical
{
    class TS_Window : MonoBehaviourWindow
    {
        public static string currentGameScene;
        internal string main_nav_selection;
        internal static ScenarioNodeDatabase Scenario = null;
        internal static ConfigNode RootNode = null;

        internal void TS_Window_main(int id, ScenarioNodeDatabase Sc)
        {
            Scenario = Sc;
            currentGameScene = "TRACKSTATION";

            // WindowSettings
            DragEnabled = true;
            ClampToScreen = true;
            TooltipsEnabled = true;
            TooltipMaxWidth = 250;
            //Debug.Log("ResourceRecovery: DrawWindows: SpaceCenter Scene main_nav_selection=" + main_nav_selection);

            //GUILayout.Label(new GUIContent("Window Contents", "Here is a reallly long tooltip to demonstrate the war and peace model of writing too much text in a tooltip\r\n\r\nIt even includes a couple of carriage returns to make stuff fun"));
            //GUILayout.Label(String.Format("Drag Enabled:{0}", DragEnabled.ToString()));

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
                mark.MarketplaceMain(id, Scenario);
            }
            else if (main_nav_selection == "settings")
            {
                settingsWindowController set = new settingsWindowController();
                set.settingsMain(id);
            }
            else
            {
                ManifestController man = new ManifestController();
                man.ManifestMain(id, Scenario);
            }
            GUILayout.EndVertical();


            GUILayout.BeginHorizontal();
            //Debug.Log("ResourceRecovery: DrawWindows: pre CelestialBody");
            //GUILayout.Label(String.Format("current_planet:{0}", Utilities.get_current_planet()));
            //GUILayout.Label(String.Format("Tooltips:{0}", TooltipsEnabled.ToString()));
            GUILayout.Label("Alt+F10 - shows/hides window");
            GUILayout.EndHorizontal();
        }
        internal override void DrawWindow(int id)
        {
        }
    }
}
