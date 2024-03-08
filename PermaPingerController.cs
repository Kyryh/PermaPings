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

            pingIndicator.interactablePingGameObjects[0].transform.localScale *= 0.75f;
            SpriteRenderer pingIcon = pingIndicator.interactablePingGameObjects[0].GetComponent<SpriteRenderer>();

            Color pingColor = GetItemColor(pingInfo.targetGameObject) ?? pingIcon.color;
            pingColor.a /= 2;

            pingIcon.color = pingColor;

            pingIndicator.pingText.fontSize *= 0.75f;
            pingIndicator.pingText.alpha /= 2;

            permapingsIndicators.Add(pingIndicator);
        }

        private static Color? GetItemColor(GameObject interactable) {
            GenericPickupController gpc;
            if ((gpc = interactable.GetComponent<GenericPickupController>()) != null) {
                return gpc.pickupIndex.GetPickupColor();
            }
            PickupPickerController ppc;
            if ((ppc = interactable.GetComponent<PickupPickerController>()) != null) {
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
