using HarmonyLib;
using System.Collections.Generic;

namespace MarlthonShips
{
    [HarmonyPatch]
    internal class AnimatorPatch
    {
        internal static bool IsCustomShip(Ship ship)
        {
            foreach(string item in yourShipsNames)
            {
                if(ship.name.StartsWith(item))
                {
                    return true;
                }
            }
            return false;
        }

        internal static List<string> yourShipsNames = new()
        {
            "Shippy"
        };

        [HarmonyPatch(typeof(Ship), nameof(Ship.Awake)), HarmonyPrefix]
        public static void ShipAwake(Ship __instance)
        {
            if(IsCustomShip(__instance))
            {
                if(!__instance.gameObject.GetComponent<ShipOars>())
                {
                    ShipOars shipOars = __instance.gameObject.AddComponent<ShipOars>();
                    shipOars.ship = __instance;
                }
            }
        }
    }
}
