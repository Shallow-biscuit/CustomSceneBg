using MelonLoader;
using MelonLoader.Utils;
using System.IO;
using System;
using System.Text;

namespace CustomSceneBg
{
    public class Main : MelonMod
    {
        public override void OnInitializeMelon()
        {
            base.OnInitializeMelon();

            string root = Path.Combine(
                MelonEnvironment.UserDataDirectory,
                "CustomSceneBg"
            );

            // 创建主目录
            Directory.CreateDirectory(root);

            // 自动创建 scene_01 ~ scene_12
            for (int i = 1; i <= 12; i++)
            {
                string sceneFolder = Path.Combine(
                    root,
                    $"scene_{i:D2}"
                );

                Directory.CreateDirectory(sceneFolder);
            }

            // 创建说明文件
            string readmePath = Path.Combine(root, "README.txt");

            if (!File.Exists(readmePath))
            {
                File.WriteAllText(
                    readmePath,
    @"CustomSceneBg 使用说明

将背景图片放入对应场景文件夹中。

示例：

scene_01/bg.png
scene_02/bg.png
scene_03/bg.png
scene_04/bg.png
scene_05/bg.png
scene_06/bg.png
scene_07/bg.png
scene_08/bg.png
scene_09/bg.png
scene_10/bg.png
scene_11/bg.png
scene_12/bg.png

支持格式：
PNG

文件名必须为：
bg.png

例如：

UserData/CustomSceneBg/scene_02/bg.png

如果对应场景文件夹中不存在 bg.png，
则游戏将自动使用原版场景背景。"
                );
            }

            MelonLogger.Msg("CustomSceneBg initialized.");
        }
    }
}