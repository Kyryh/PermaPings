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
                    permapings.RemoveAt(i);
                    if (permapingsIndicators[i] != null && permapingsIndicators[i].gameObject != null) {
                        GameObject.Destroy(permapingsIndicators[i]?.gameObject);
                    }
                    permapingsIndicators.RemoveAt(i);
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
            //pingIndicator.fixedTimer = float.PositiveInfinity;
            permapingsIndicators.Add(pingIndicator);

        }
        internal static void ResetPings(Stage stage) {
            permapings = new();
            permapingsIndicators = new();
        }
    }
}
