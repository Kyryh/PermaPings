using BepInEx;
using R2API;
using RoR2;
using System.IO;
using System.Reflection;
using System;
using UnityEngine;

namespace ExamplePlugin
{

    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class ExamplePlugin : BaseUnityPlugin
    {

        public const string PluginGUID = "kyryh.permapings";
        public const string PluginName = "PermaPings";
        public const string PluginVersion = "1.0.0";
        public static BepInEx.Logging.ManualLogSource Log { get; private set; }
        public static BepInEx.Configuration.ConfigFile ConfigFile { get; private set; }


        private void Awake() {
            Log = Logger;

            ConfigFile = Config;

            Assembly assembly = Assembly.GetExecutingAssembly();

            

            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions")) {
                AddConfigOptions();
            }


            Logger.LogInfo($"Plugin {PluginGUID} is loaded!");
        }

        private void AddConfigOptions() {
            /*
            ModSettingsManager.AddOption(new IntSliderOption(ArtifactOfCognation.cloneLifespan, new IntSliderConfig() { min = 5, max = 60 }));

            ModSettingsManager.AddOption(new IntSliderOption(ArtifactOfDistortion.cycleDuration, new IntSliderConfig() { min = 20, max = 300 }));
            ModSettingsManager.AddOption(new SliderOption(ArtifactOfDistortion.cooldownReduction, new SliderConfig() { min = 0, max = 100 }));
            ModSettingsManager.AddOption(new IntSliderOption(ArtifactOfDistortion.skillsToLock, new IntSliderConfig() { min = 1, max = 3 }));

            ModSettingsManager.AddOption(new IntSliderOption(ArtifactOfPrestige.numMountainShrinesToSpawn, new IntSliderConfig() { min = 0, max = 5 }));
            ModSettingsManager.AddOption(new ColorOption(ArtifactOfPrestige.shrineSymbolColor));

            ModSettingsManager.AddOption(new IntSliderOption(ArtifactOfTempus.itemLifespan, new IntSliderConfig() { min = 30, max = 600 }));
            ModSettingsManager.AddOption(new IntSliderOption(ArtifactOfTempus.baseNumStacks, new IntSliderConfig() { min = 1, max = 10 }));
            ModSettingsManager.AddOption(new IntSliderOption(ArtifactOfTempus.bonusNumStacks, new IntSliderConfig() { min = 0, max = 5 }));
            ModSettingsManager.AddOption(new IntSliderOption(ArtifactOfTempus.stagesPerBonusStacks, new IntSliderConfig() { min = 1, max = 5 }));
            */
        }



        public static void LogDebug(object data) {
            Log.LogDebug(data);
        }

        public static void LogError(object data) {
            Log.LogError(data);
        }
    }
}
