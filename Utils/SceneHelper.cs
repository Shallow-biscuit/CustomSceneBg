using UnityEngine;
using Il2Cpp;

namespace ModTemplate.Utils
{
    internal static class SceneHelper
    {
        public static string GetCurrentSceneName()
        {
            return SceneChangeController.curScene > 9
                ? $"scene_{SceneChangeController.curScene}"
                : $"scene_0{SceneChangeController.curScene}";
        }

        public static void HideScene()
        {
            GameObject controller =
                GameObject.Find("SceneObjectController");

            if (controller == null)
                return;

            Transform scene =
                controller.transform.Find(GetCurrentSceneName());

            if (scene == null)
                return;

            for (int i = 0; i < scene.childCount; i++)
                scene.GetChild(i).gameObject.SetActive(false);

            scene.gameObject.SetActive(false);
        }
    }
}