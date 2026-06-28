using HarmonyLib;
using Il2CppFormulaBase;
using CustomSceneBg.Managers;

namespace CustomSceneBg.Patches
{
    [HarmonyPatch(typeof(StageBattleComponent), "Resume")]
    internal class ResumePatch
    {
        static void Postfix()
        {
            BackgroundManager.isPaused = false;
        }
    }
}