using HarmonyLib;
using UnityEngine;

namespace MarlthonShips
{
    [HarmonyPatch]
    internal class ShipContainerPatch
    {
        [HarmonyPatch(typeof(Container), nameof(Container.Awake)), HarmonyPrefix]
        public static bool ContainerAwake(Container __instance)
        {
            if(__instance is ShipContainer shipContainer)
            {
                shipContainer.ShipContainerAwake();

                return false;
            }

            return true;
        }
        [HarmonyPatch(typeof(Container), nameof(Container.UpdateUseVisual)), HarmonyPrefix]
        public static bool ContainerUpdateUseVisual(Container __instance)
        {
            if(__instance is ShipContainer shipContainer)
            {
                shipContainer.ShipContainerUpdateUseVisual();

                return false;
            }

            return true;
        }
        [HarmonyPatch(typeof(Container), nameof(Container.Interact)), HarmonyPrefix]
        public static bool ContainerInteract(Humanoid character, bool hold, bool alt, Container __instance)
        {
            if(__instance is ShipContainer shipContainer)
            {
                shipContainer.ShipContainerInteract(character, hold, alt);

                return false;
            }

            return true;
        }
        [HarmonyPatch(typeof(Container), nameof(Container.RPC_RequestOpen)), HarmonyPrefix]
        public static bool ContainerRPC_RequestOpen(long uid, long playerID, Container __instance)
        {
            if(__instance is ShipContainer shipContainer)
            {
                shipContainer.ShipContainerRPC_RequestOpen(uid, playerID);

                return false;
            }

            return true;
        }
        [HarmonyPatch(typeof(Container), nameof(Container.TakeAll)), HarmonyPrefix]
        public static bool ContainerTakeAll(Humanoid character, Container __instance)
        {
            if(__instance is ShipContainer shipContainer)
            {
                shipContainer.ShipContainerTakeAll(character);

                return false;
            }

            return true;
        }
        [HarmonyPatch(typeof(Container), nameof(Container.RPC_RequestTakeAll)), HarmonyPrefix]
        public static bool ContainerRPC_RequestTakeAll(long uid, long playerID, Container __instance)
        {
            if(__instance is ShipContainer shipContainer)
            {
                shipContainer.ShipContainerRPC_RequestTakeAll(uid, playerID);

                return false;
            }

            return true;
        }
        [HarmonyPatch(typeof(Container), nameof(Container.Save)), HarmonyPrefix]
        public static bool ContainerSave(Container __instance)
        {
            if(__instance is ShipContainer shipContainer)
            {
                shipContainer.ShipContainerSave();

                return false;
            }

            return true;
        }
        [HarmonyPatch(typeof(Container), nameof(Container.Load)), HarmonyPrefix]
        public static bool ContainerLoad(Container __instance)
        {
            if(__instance is ShipContainer shipContainer)
            {
                shipContainer.ShipContainerLoad();

                return false;
            }

            return true;
        }
    }
}
