using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PermaPings {
    internal static class Hooks {
        public static void Init() {
            On.RoR2.PlayerCharacterMasterController.Update += PlayerCharacterMasterController_Update;
            Stage.onServerStageComplete += PermaPingerController.ResetPings;
            On.RoR2.UI.PingIndicator.Update += PingIndicator_Update;
        }
        
        private static void PingIndicator_Update(On.RoR2.UI.PingIndicator.orig_Update orig, RoR2.UI.PingIndicator self) {
            if (!PermaPingerController.permapingsIndicators.Contains(self)) {
                orig(self);
            }
        }

        private static void PlayerCharacterMasterController_Update(On.RoR2.PlayerCharacterMasterController.orig_Update orig, RoR2.PlayerCharacterMasterController self) {
            orig(self);
            if (self.hasEffectiveAuthority && self.bodyInputs && self.body && Input.GetKeyDown(KeyCode.Mouse3))
                PermaPingerController.AttemptPing(new Ray(self.bodyInputs.aimOrigin, self.bodyInputs.aimDirection), self.body.gameObject, self.gameObject);
        }
    }
}
