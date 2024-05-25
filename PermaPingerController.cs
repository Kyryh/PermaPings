using RoR2;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace PermaPings {
    internal static class PermaPingerController {
        private static List<PingerController.PingInfo> permapings = new();
        public static List<PingIndicator> permapingsIndicators = new();
        public static void AttemptPing(Ray aimRay, GameObject bodyObject, GameObject owner) {
            if (PingerController.GeneratePingInfo(aimRay, bodyObject, out var result) && result.targetNetworkIdentity != null) {
                int i = permapings.FindIndex(permaping => permaping.targetNetworkIdentity == result.targetNetworkIdentity);
                if (i == -1) {
                    permapings.Add(result);
                    BuildPing(result, owner);
                }
                else {
                    RemovePing(i);
                }
            }
        }


        private static void BuildPing(PingerController.PingInfo pingInfo, GameObject owner) {
            GameObject gameObject = GameObject.Instantiate(LegacyResourcesAPI.Load<GameObject>("Prefabs/PingIndicator"));
            PingIndicator pingIndicator = gameObject.GetComponent<PingIndicator>();
            pingIndicator.pingOwner = owner;
            pingIndicator.pingOrigin = pingInfo.origin;
            pingIndicator.pingNormal = pingInfo.normal;
            pingIndicator.pingTarget = pingInfo.targetGameObject;
            pingIndicator.RebuildPing();

            pingIndicator.interactablePingGameObjects[0].transform.localScale *= PermaPingsConfig.permaPingSize.Value/100;
            SpriteRenderer pingIcon = pingIndicator.interactablePingGameObjects[0].GetComponent<SpriteRenderer>();

            Color pingColor = GetItemColor(pingInfo.targetGameObject) ?? pingIcon.color;
            
            pingColor.a *= PermaPingsConfig.permaPingAlpha.Value/100;

            pingIcon.color = pingColor;

            pingIndicator.pingText.fontSize *= PermaPingsConfig.permaPingSize.Value/100;
            pingIndicator.pingText.alpha *= PermaPingsConfig.permaPingAlpha.Value/100;

            permapingsIndicators.Add(pingIndicator);
        }

        private static Color? GetItemColor(GameObject interactable) {
            if (!PermaPingsConfig.permaPingItemTierColor.Value)
                return null;
            GenericPickupController gpc;
            if ((gpc = interactable.GetComponent<GenericPickupController>()) != null) {
                return gpc.pickupIndex.GetPickupColor();
            }
            PickupPickerController ppc;
            if (interactable.GetComponent<ScrapperController>() == null && (ppc = interactable.GetComponent<PickupPickerController>()) != null && ppc.options.Length > 0) {
                return ppc.options[0].pickupIndex.GetPickupColor();
            }
            ShopTerminalBehavior stb;
            if ((stb = interactable.GetComponent<ShopTerminalBehavior>()) != null) {
                return stb.CurrentPickupIndex().GetPickupColor();
            }
            return null;
        }

        internal static void RemovePing(int index) {
            permapings.RemoveAt(index);
            if (permapingsIndicators[index] != null && permapingsIndicators[index].gameObject != null) {
                GameObject.Destroy(permapingsIndicators[index]?.gameObject);
            }
            permapingsIndicators.RemoveAt(index);
        }
        internal static void ResetPings(Stage stage) {
            permapings = new();
            permapingsIndicators = new();
        }
    }
}
