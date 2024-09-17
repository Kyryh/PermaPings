using BepInEx;
using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace PermaPings {
    internal static class PermaPingsConfig {

        public static ConfigEntry<KeyboardShortcut> permaPingKey;

        public static ConfigEntry<float> permaPingAlpha;
        public static ConfigEntry<float> permaPingSize;
        public static ConfigEntry<bool> permaPingItemTierColor;

        //public static ConfigEntry<bool> allowScannerPermaPings;
        //public static ConfigEntry<PermaPingCategory> scannerPermaPingCategories;

        public static void Init(ConfigFile config) {


            permaPingKey = config.Bind(
                "Keybinds",
                "PermaPingButton",
                new KeyboardShortcut(KeyCode.Mouse3),
                "Which key to use to place a permanent ping"
            );

            permaPingAlpha = config.Bind(
                "Visuals",
                "PermaPingAlpha",
                50f,
                "Alpha value of the perma-ping icon, i.e. how opaque it is\n" + 
                "100 means it's completely visible, 0 means it's completely transparent"
            );

            permaPingSize = config.Bind(
                "Visuals",
                "PermaPingSize",
                75f,
                "Size of the perma-ping icon and text"
            );

            permaPingItemTierColor = config.Bind(
                "Visuals",
                "PermaPingItemTierColor",
                true,
                "Whether to use the pinged item's tier as the ping's color"
            );

            //allowScannerPermaPings = config.Bind(
            //    "RadioScanner",
            //    "AllowScannerPermaPings",
            //    false,
            //    "Whether the radio scanner should automatically perma-ping some interactables"
            //);

            //scannerPermaPingCategories = config.Bind(
            //    "RadioScanner",
            //    "ScannerPermaPingCategories",
            //    PermaPingCategory.Chest | PermaPingCategory.Shrines | PermaPingCategory.Loot | PermaPingCategory.Teleporter | PermaPingCategory.Mystery,
            //    "What interactable categories the scanner should automatically perma-ping"
            //);


            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions")) {
                AddConfigOptions();
            }
        }

        public static bool GetPermaPingKeyDown() {
            return UnityInput.Current.GetKeyDown(permaPingKey.Value.MainKey);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void AddConfigOptions() {
            ModSettingsManager.AddOption(new KeyBindOption(permaPingKey));

            ModSettingsManager.AddOption(new SliderOption(permaPingAlpha, new SliderConfig() { min = 0, max = 100}));
            ModSettingsManager.AddOption(new SliderOption(permaPingSize, new SliderConfig() { min = 0, max = 100}));
            ModSettingsManager.AddOption(new CheckBoxOption(permaPingItemTierColor));

            //ModSettingsManager.AddOption(new CheckBoxOption(allowScannerPermaPings));
            //ModSettingsManager.AddOption(new ChoiceOption(scannerPermaPingCategories));

        }


        //public enum PermaPingCategory {
        //    Chest = 1,
        //    Barrel = 2,
        //    Shrines = 4,
        //    Drones = 8,
        //    Loot = 16,
        //    Teleporter = 32,
        //    Mystery = 64,
        //    All = 127
        //}
    }
}
