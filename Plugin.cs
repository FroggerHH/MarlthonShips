using BepInEx;
using HarmonyLib;
using ParticleReplacerManager;
using UnityEngine;
using PieceManager;

namespace MarlthonShips
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class Plugin : BaseUnityPlugin
    {
        internal const string ModName = "MarlthonShips", ModVersion = "0.0.1", ModGUID = "com.Frogger." + ModName;
        private static Harmony harmony = new(ModGUID);
        internal static Plugin _self;

        private void Awake()
        {
            _self = this;

            BuildPiece ship = new("shippy", "Shippy");
            ship.Name
                .English("shippy")
                .Russian("кораблик");
            ship.Crafting.Set(CraftingTable.Workbench);
            ship.Category.Add(BuildPieceCategory.Misc);

            MaterialReplacer.RegisterGameObjectForShaderSwap(ship.Prefab, MaterialReplacer.ShaderType.UseUnityShader);
            ParticleReplacer.FixShip(ship.Prefab);


            harmony.PatchAll();





            UnityEngine.Debug.LogWarning("vgfdargb");
        }

        internal void Debug(string msg)
        {
            Logger.LogInfo(msg);
        }
    }
}