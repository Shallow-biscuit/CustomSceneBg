using HarmonyLib;
using Il2CppFormulaBase;
using MelonLoader;
using CustomSceneBg.Managers;

namespace CustomSceneBg.Patches
{
    [HarmonyPatch(typeof(StageBattleComponent), "Load")]
    internal class LoadPatch
    {
        static void Postfix()
        {
            MelonLogger.Msg("StageBattleComponent.Load");

            BackgroundManager.Clear();
            BackgroundManager.InitImmediate();
        }
    }
}