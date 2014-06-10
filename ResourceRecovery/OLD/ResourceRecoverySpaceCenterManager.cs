using KSP.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Philotical
{
    class ResourceRecoverySpaceCenterManager : MonoBehaviour, Savable
    {
        private ResourceRecoveryGlobalSettings globalSettings;
        private ResourceRecoveryGameSettings gameSettings;
        private IButton button;
        private ResourceRecoverySavedGameConfigWindow configWindow;

        public void SpaceCenterManager()
        {
            Debug.Log("Constructor");
            globalSettings = ResourceRecovery.Instance.globalSettings;
            gameSettings = ResourceRecovery.Instance.gameSettings;
            
            button = ToolbarManager.Instance.add("ResourceRecovery", "SpaceCenterIcon");
            button.TexturePath = "Philotical/ResourceRecovery/textures/ResourceRecoverySymbol";
            button.ToolTip = "Resource Recovery";
            button.OnClick += (e) => OnIconClicked();
            
            configWindow = new ResourceRecoverySavedGameConfigWindow(globalSettings, gameSettings);
        }

        void Start()
        {
            Debug.Log("Start, new game = " + gameSettings.IsNewSave);
            button.Visible = true;

            if (gameSettings.IsNewSave)
            {
                Debug.Log("New save detected!");
                configWindow.SetVisible(true);
                gameSettings.IsNewSave = false;
            }

        }

        public void Load(ConfigNode globalNode)
        {
            //button.Load(globalNode);
            configWindow.Load(globalNode);
        }

        public void Save(ConfigNode globalNode)
        {
            //button.Save(globalNode);
            configWindow.Save(globalNode);
        }

        void OnDestroy()
        {
            Debug.Log("SpaceCenterManager OnDestroy");
            button.Destroy();
        }

        private void OnIconClicked()
        {
            configWindow.ToggleVisible();
        }
    }
}