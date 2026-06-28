using MelonLoader;
using MelonLoader.Utils;
using ModTemplate.Utils;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace CustomSceneBg.Managers
{
    internal static class BackgroundManager
    {
        public static GameObject quad1;
        public static GameObject quad2;
        public static Camera bgCamera;

        public static bool isPaused = false;

        private static object scrollCoroutine = null;

        // 缓存图片字节，避免频繁读硬盘
        private static Dictionary<string, byte[]> imageCache =
            new Dictionary<string, byte[]>();

        private static float width;
        private static int bgLayer = 30;
        private static bool isLoaded = false;

        //private static Dictionary<string, Texture2D> cache =
        //    new Dictionary<string, Texture2D>();

        public static void InitImmediate()
        {
            if (isLoaded)
                return;

            bool loaded = InitImageBackground();

            if (!loaded)
                return;

            isLoaded = true;

            SceneHelper.HideScene();

            if (quad1 != null && quad2 != null)
            {
                // 保证同时只有一个滚动协程
                if (scrollCoroutine != null)
                {
                    MelonCoroutines.Stop(scrollCoroutine);
                    scrollCoroutine = null;
                }

                scrollCoroutine = MelonCoroutines.Start(Scroll());
            }
        }

        private static bool InitImageBackground()
        {
            MelonLogger.Msg("========== 开始加载背景 ==========");

            if (Camera.allCameras == null ||
                Camera.allCameras.Length == 0)
            {
                MelonLogger.Warning("没有找到任何相机");
                return false;
            }

            string sceneName = SceneHelper.GetCurrentSceneName();

            MelonLogger.Msg($"当前场景: {sceneName}");

            string path = Path.Combine(
                MelonEnvironment.UserDataDirectory,
                "CustomSceneBg",
                sceneName,
                "bg.png"
            );

            MelonLogger.Msg($"图片路径: {path}");

            if (!File.Exists(path))
            {
                MelonLogger.Warning("图片不存在");
                return false;
            }

            MelonLogger.Msg("图片存在");

            var camera = Camera.allCameras
                .FirstOrDefault(c => c.name == "Camera_3D");

            if (camera == null)
            {
                MelonLogger.Warning("没有找到 Camera_3D");

                foreach (var cam in Camera.allCameras)
                    MelonLogger.Msg($"当前相机: {cam.name}");

                return false;
            }

            MelonLogger.Msg("找到 Camera_3D");

            // 后面的代码保持不变

            bgCamera = UnityEngine.Object.Instantiate(camera);

            MelonLogger.Msg("创建背景相机成功");

            bgCamera.name = "Camera_ImageBG";
            bgCamera.depth = -100;
            bgCamera.clearFlags = CameraClearFlags.SolidColor;
            bgCamera.backgroundColor = Color.black;
            bgCamera.cullingMask = 1 << bgLayer;

            bgCamera.allowHDR = false;
            bgCamera.allowMSAA = false;
            bgCamera.orthographic = true;

            if (Camera.main != null)
            {
                bgCamera.orthographicSize =
                    Camera.main.orthographicSize;

                Camera.main.cullingMask &= ~(1 << bgLayer);
                Camera.main.clearFlags = CameraClearFlags.Depth;
            }

            byte[] data;

            if (imageCache.ContainsKey(path))
            {
                data = imageCache[path];
            }
            else
            {
                data = File.ReadAllBytes(path);
                imageCache[path] = data;
            }

            Texture2D tex = new Texture2D(
                2,
                2,
                TextureFormat.RGBA32,
                false
            );

            tex.LoadImage(data);

            tex.Apply(false, false);

            tex.filterMode = FilterMode.Bilinear;
            tex.wrapMode = TextureWrapMode.Clamp;

            Material mat = new Material(
                Shader.Find("UI/Default")
            );

            mat.mainTexture = tex;

            float imgAspect =
                (float)tex.width / tex.height;

            float height =
                bgCamera.orthographicSize * 2f;

            width = height * imgAspect;

            float startX = -width / 2f;

            quad1 = CreateQuad(
                bgCamera,
                mat,
                new Vector3(startX, 0, 10f),
                width,
                height
            );

            MelonLogger.Msg("创建 quad1 成功");

            quad2 = CreateQuad(
                bgCamera,
                mat,
                new Vector3(startX + width, 0, 10f),
                width,
                height
            );

            MelonLogger.Msg("创建 quad2 成功");

            MelonLogger.Msg("背景初始化完成");

            return true;
        }

        private static GameObject CreateQuad(
            Camera cam,
            Material mat,
            Vector3 pos,
            float w,
            float h)
        {
            GameObject quad =
                GameObject.CreatePrimitive(
                    PrimitiveType.Quad);

            quad.transform.SetParent(cam.transform);
            quad.transform.localPosition = pos;
            quad.transform.localScale =
                new Vector3(w, h, 1f);

            quad.layer = bgLayer;

            quad.GetComponent<Renderer>().material = mat;

            return quad;
        }

        private static IEnumerator Scroll()
        {
            float speed = 3.75f;

            while (true)
            {
                if (quad1 == null ||
                    quad2 == null ||
                    bgCamera == null)
                    yield break;

                if (isPaused)
                {
                    yield return null;
                    continue;
                }

                if (Camera.main != null)
                    bgCamera.orthographicSize =
                        Camera.main.orthographicSize;

                float move =
                    speed * Time.deltaTime;

                quad1.transform.localPosition +=
                    new Vector3(-move, 0, 0);

                quad2.transform.localPosition +=
                    new Vector3(-move, 0, 0);

                if (quad1.transform.localPosition.x <= -width)
                {
                    quad1.transform.localPosition =
                        quad2.transform.localPosition +
                        new Vector3(width, 0, 0);
                }

                if (quad2.transform.localPosition.x <= -width)
                {
                    quad2.transform.localPosition =
                        quad1.transform.localPosition +
                        new Vector3(width, 0, 0);
                }

                yield return null;
            }
        }


        public static void Clear()
        {
            // 停止滚动协程
            if (scrollCoroutine != null)
            {
                MelonCoroutines.Stop(scrollCoroutine);
                scrollCoroutine = null;
            }

            // 重置暂停状态
            isPaused = false;

            isLoaded = false;

            if (quad1 != null)
                UnityEngine.Object.Destroy(quad1);

            if (quad2 != null)
                UnityEngine.Object.Destroy(quad2);

            if (bgCamera != null)
                UnityEngine.Object.Destroy(bgCamera.gameObject);

            quad1 = null;
            quad2 = null;
            bgCamera = null;
        }
    }
}

    
