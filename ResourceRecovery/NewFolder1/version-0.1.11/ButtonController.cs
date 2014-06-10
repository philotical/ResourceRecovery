using System.Collections.Generic;
using UnityEngine;

namespace Philotical
{
    public class ButtonController
    {
        private static string main_nav_selection;
        private static string groupFilter;

        public ButtonController(string main_nav_selection1)
        {
            main_nav_selection = main_nav_selection1;
        }

        /*
         MainNavButtons
         */
        #region "MainNavButtons"
        public static void GetMainNavButtons(string[] buttonIDs)
        {
            GUIStyle buttonStyle = RRSettingsController.get_button_stretchWidth();
            GUIStyle buttonStyle_active = RRSettingsController.get_button_stretchWidth_active();
            foreach (string bid in buttonIDs)
            {
                GUIStyle thisStyle = (main_nav_selection == bid) ? buttonStyle_active : buttonStyle;
                if (bid == "manifest")
                {
                    if (GUILayout.Button("Manifest", thisStyle))
                    {
                        setMain_nav_selection("manifest");
                        Debug.Log("Clicked Button = " + bid);
                    }
                }
                else if (bid == "refuel")
                {
                    if (GUILayout.Button("Refuel/Recover", thisStyle))
                    {
                        setMain_nav_selection("refuel");
                        Debug.Log("Clicked Button = " + bid);
                    }
                }
                else if (bid == "settings")
                {
                    if (GUILayout.Button("Settings", thisStyle))
                    {
                        setMain_nav_selection("settings");
                        Debug.Log("Clicked Button = " + bid);
                    }
                }
                else if (bid == "marketplace")
                {
                    if (GUILayout.Button("Marketplace", thisStyle))
                    {
                        setMain_nav_selection("marketplace");
                        Debug.Log("Clicked Button = " + bid);
                    }
                }
                else if (bid == "help")
                {
                    if (GUILayout.Button("Help", thisStyle))
                    {
                        setMain_nav_selection("help");
                        Debug.Log("Clicked Button = " + bid);
                    }
                }
            }
        }
        internal static string setMain_nav_selection(string p)
        {
            main_nav_selection = p;
            return main_nav_selection;
        }
        internal static string getMain_nav_selection()
        {
            return main_nav_selection;
        }
        #endregion "MainNavButtons"

        /*
         ResourceOptionsButtons
         */
        #region "ResourceOptionsButtons"
        public static void GetResourceOptionsButtons(List<string> buttonIDs, Part part, PartResource resource, string resourceSelected)
        {
            GUIStyle buttonStyle = RRSettingsController.get_button_stretchWidth();
            foreach (string bid in buttonIDs)
            {
                if (GUILayout.Button(bid, buttonStyle))
                {
                    Debug.Log("ResourceOptionsButtons Clicked bid=" + bid + " part.partInfo.title=" + part.partInfo.title + " resource.resourceName=" + resource.resourceName + " resourceSelected=" + resourceSelected);
                    Utilities.perform_resource_transfer(bid, part, resource, resourceSelected);
                }
            }
        }
        #endregion "ResourceOptionsButtons"

        /*
         ResourceFilterButtons
         */
        #region "ResourceFilterButtons"
        public static void GetResourceFilterButtons(List<string> buttonIDs, string groupFilter)
        {
            GUIStyle buttonStyle = RRSettingsController.get_button_stretchWidth();
            GUIStyle buttonStyle_active = RRSettingsController.get_button_stretchWidth_active();
            foreach (string bid in buttonIDs)
            {
                GUIStyle thisStyle = (groupFilter == bid) ? buttonStyle_active : buttonStyle;
                if (GUILayout.Button(bid, thisStyle))
                {
                    Debug.Log("Clicked Button = " + bid);
                    set_groupFilter(bid);
                }
            }
        }
        internal static string set_groupFilter(string p)
        {
            groupFilter = p;
            return groupFilter;
        }
        internal static string get_groupFilter(string defaultFilter = null)
        {
            groupFilter = (groupFilter == null) ? defaultFilter : groupFilter;
            return groupFilter;
        }
        internal static string unset_groupFilter()
        {
            groupFilter = null;
            return groupFilter;
        }
        #endregion "ResourceFilterButtons"


    }
}
