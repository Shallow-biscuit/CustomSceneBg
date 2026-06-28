using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using System.Collections;
using CustomSceneBg.Managers;

namespace CustomSceneBg.Patches
{
    [HarmonyPatch(typeof(SceneChangeController), "OnControllerStart")]
    internal class SceneChangePatch
    {
        static void Postfix()
        {
            MelonCoroutines.Start(Reload());
        }

        static IEnumerator Reload()
        {
            yield return null;
            yield return null;
            yield return null;

            BackgroundManager.Clear();
            BackgroundManager.InitImmediate();
        }
    }
}