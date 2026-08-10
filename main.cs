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
则游戏将自动使用原版场景背景。
以下是每个代号所代表的场景：
scene_01：太空
scene_02：都市
scene_03：城堡
scene_04：雨夜街道
scene_05：糖果森林
scene_06：和风
scene_07：节奏过山车
scene_08：车万
scene_09：DJMAX
scene_10：初音未来
scene_11：24愚人节（不可用，ppg这边占了一个场景位置
scene_12：国风
"
                );
            }

            MelonLogger.Msg("CustomSceneBg initialized.");
        }
    }
}