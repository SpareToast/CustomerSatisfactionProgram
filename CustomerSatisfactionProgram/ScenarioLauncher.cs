using KSP.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CustomerSatisfactionProgram
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class ScenarioLauncher : MonoBehaviour
    {
        public ScenarioLauncher()
        {
        }

        void Awake()
        {
            Debug.Log("Scenario Launcher is Awake");
        }

        void Start()
        {
            var game = HighLogic.CurrentGame;

            var psm = game.scenarios.Find(s => s.moduleName == typeof(CustomerSave).Name);
            if (psm == null)
            {
                game.AddProtoScenarioModule(typeof(CustomerSave), GameScenes.SPACECENTER,
                    GameScenes.FLIGHT, GameScenes.EDITOR, GameScenes.TRACKSTATION);
            }
            else
            {
                if (psm.targetScenes.All(s => s != GameScenes.SPACECENTER))
                {
                    psm.targetScenes.Add(GameScenes.SPACECENTER);
                }
                if (psm.targetScenes.All(s => s != GameScenes.FLIGHT))
                {
                    psm.targetScenes.Add(GameScenes.FLIGHT);
                }
                if (psm.targetScenes.All(s => s != GameScenes.EDITOR))
                {
                    psm.targetScenes.Add(GameScenes.EDITOR);
                }
                if (psm.targetScenes.All(s => s != GameScenes.TRACKSTATION))
                {
                    psm.targetScenes.Add(GameScenes.TRACKSTATION);
                }
            }
        }
    }
}
