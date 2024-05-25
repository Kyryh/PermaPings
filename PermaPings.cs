using BepInEx;
using R2API;
using RoR2;
using System.IO;
using System.Reflection;
using System;
using UnityEngine;
using RiskOfOptions;

namespace PermaPings {

    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]
    public class PermaPings : BaseUnityPlugin {

        public const string PluginGUID = "kyryh.permapings";
        public const string PluginName = "PermaPings";
        public const string PluginVersion = "1.0.0";
        public static BepInEx.Logging.ManualLogSource Log { get; private set; }


        private void Awake() {
            Log = Logger;

            PermaPingsConfig.Init(Config);

            Hooks.Init();

            Logger.LogInfo($"Plugin {PluginGUID} is loaded!");
        }


        public static void LogDebug(object data) {
            Log.LogDebug(data);
        }

        public static void LogError(object data) {
            Log.LogError(data);
        }
    }
}
