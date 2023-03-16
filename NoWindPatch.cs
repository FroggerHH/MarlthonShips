using HarmonyLib;
using UnityEngine;

namespace MarlthonShips
{
    [HarmonyPatch]
    internal class NoWindPatch
    {
        [HarmonyPatch(typeof(Ship), nameof(Ship.GetSailForce)), HarmonyPrefix]
        public static bool ShipAwake(Ship __instance, ref Vector3 __result)
        {
            if(AnimatorPatch.IsCustomShip(__instance))
            {
                __instance.m_sailForce = Vector3.zero;
                __result = Vector3.zero;
                return false;
            }
            return true;
        }
    }
}