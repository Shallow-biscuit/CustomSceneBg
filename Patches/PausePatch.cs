using HarmonyLib;
using Il2CppFormulaBase;
using CustomSceneBg.Managers;

namespace CustomSceneBg.Patches
{
    [HarmonyPatch(typeof(StageBattleComponent), "Pause")]
    internal class PausePatch
    {
        static void Postfix()
        {
            BackgroundManager.isPaused = true;
        }
    }
}