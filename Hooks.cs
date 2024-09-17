using RoR2;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using MonoMod.RuntimeDetour.HookGen;
using R2API.Utils;

namespace PermaPings {
    internal static class Hooks {
        public static void Init() {
            On.RoR2.PlayerCharacterMasterController.Update += PlayerCharacterMasterController_Update;
            Stage.onServerStageComplete += PermaPingerController.ResetPings;
            On.RoR2.UI.PingIndicator.Update += PingIndicator_Update;

            var isPingableProperty = typeof(NetworkIdentity).GetPropertySetter("isPingable");

            HookEndpointManager.Add(isPingableProperty, NetworkIdentity_set_isPingable);
                
        }
        
        private static void NetworkIdentity_set_isPingable(Action<NetworkIdentity, bool> orig, NetworkIdentity self, bool value) {
            orig(self, value);
            if (!value) {
                PermaPingerController.AttemptRemovePing(self);
            }
        }
        private static void PingIndicator_Update(On.RoR2.UI.PingIndicator.orig_Update orig, PingIndicator self) {
            if (!PermaPingerController.permapingsIndicators.Contains(self)) {
                orig(self);
            }
        }

        private static void PlayerCharacterMasterController_Update(On.RoR2.PlayerCharacterMasterController.orig_Update orig, PlayerCharacterMasterController self) {
            orig(self);
            if (self.hasEffectiveAuthority && self.bodyInputs && self.body && PermaPingsConfig.permaPingKey.Value.IsDown())
                PermaPingerController.AttemptPing(new Ray(self.bodyInputs.aimOrigin, self.bodyInputs.aimDirection), self.body.gameObject, self.gameObject);
        }
    }
}
