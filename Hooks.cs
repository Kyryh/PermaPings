using RoR2;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using MonoMod.RuntimeDetour.HookGen;
using R2API.Utils;
using MonoMod.Cil;
using UnityEngine.UIElements;
using Mono.Cecil.Cil;

namespace PermaPings {
    internal static class Hooks {
        public static void Init() {
            On.RoR2.PlayerCharacterMasterController.Update += PlayerCharacterMasterController_Update;
            Stage.onServerStageComplete += PermaPingerController.ResetPings;
            IL.RoR2.UI.PingIndicator.Update += IL_PingIndicator_Update;
            On.RoR2.UI.PingIndicator.Update += PingIndicator_Update;

            //On.RoR2.UI.PingIndicator.Update += PingIndicator_Update;

            //var isPingableProperty = typeof(NetworkIdentity).GetPropertySetter("isPingable");

            //HookEndpointManager.Add(isPingableProperty, NetworkIdentity_set_isPingable);

        }

        private static void PingIndicator_Update(On.RoR2.UI.PingIndicator.orig_Update orig, PingIndicator self) {
            orig(self);
            if (self.pingTarget && self.pingTarget.TryGetComponent<BarrelInteraction>(out var barrel) && barrel.opened) {
                self.DestroyPing();
            }
        }

        private static void IL_PingIndicator_Update(ILContext il) {
            var c = new ILCursor(il);

            ILLabel label = null;
            c.GotoNext(
                x => x.MatchBrtrue(out label),
                x => x.MatchLdarg(0),
                x => x.MatchCall<PingIndicator>("DestroyPing")
            );
            c.Index++;
            c.Emit(OpCodes.Ldarg_0);
            c.EmitDelegate<Func<PingIndicator, bool>>(ping => {
                if (ping.pingTarget.TryGetComponent<PurchaseInteraction>(out var printer)) {
                    var p = printer.costType switch {
                        CostTypeIndex.WhiteItem
                        or CostTypeIndex.GreenItem
                        or CostTypeIndex.RedItem
                        or CostTypeIndex.BossItem => true,
                        _ => false
                    };
                    return p;
                }
                return false;
            });
            c.Emit(OpCodes.Brtrue_S, label);
        }

        //private static void NetworkIdentity_set_isPingable(Action<NetworkIdentity, bool> orig, NetworkIdentity self, bool value) {
        //    orig(self, value);
        //    if (!value) {
        //        PermaPingerController.AttemptRemovePing(self);
        //    }
        //}
        //private static void PingIndicator_Update(On.RoR2.UI.PingIndicator.orig_Update orig, PingIndicator self) {
        //    if (!PermaPingerController.permapingsIndicators.Contains(self)) {
        //        orig(self);
        //    }
        //}

        private static void PlayerCharacterMasterController_Update(On.RoR2.PlayerCharacterMasterController.orig_Update orig, PlayerCharacterMasterController self) {
            orig(self);
            if (self.hasEffectiveAuthority && self.bodyInputs && self.body && PermaPingsConfig.GetPermaPingKeyDown())
                PermaPingerController.AttemptPing(new Ray(self.bodyInputs.aimOrigin, self.bodyInputs.aimDirection), self.body.gameObject, self.gameObject);
        }
    }
}
