using Modding;
using System;
using System.Collections.Generic;
using UnityEngine;
using Modding.Common;
using Modding.Blocks;

namespace MordenFirearmKitMod
{

    // If you need documentation about any of these values or the mod loader
    // in general, take a look at http://wiki.spiderlinggames.co.uk/besiege/modding-documentation/articles/introduction.html.

    public enum BlockList
    {
        火箭弹模块 = 650,
        火箭巢模块 = 651,
        机枪模块 = 652,
        指向模块 = 653
    }

    public class MordenFirearmKitBlockMod : ModEntryPoint
    {
      

        public override void OnLoad()
        {
            // Your initialization code here

            //添加MOD更新推送功能
            //new GameObject("Mod更新组件").AddComponent<Updater>().SetUrl(Author, Name);

            //注册控制台指令
            //Commands.RegisterCommand("mfk", ConfigurationCommand, "MordernFirearmKit Mod configuration command.\n");

            //LoadBlock(RocketBlock);
            //LoadBlock(RocketPodBlock);
            //LoadBlock(MachineGun);
            //LoadBlock(direction);

            if (ModernFirearmKitMod.RocketPodBlockScript.RocketTemp == null)
            {
                ModernFirearmKitMod.RocketPodBlockScript.CreateRocketBlockTemp();
            }
            ModResource.CreateAssetBundleResource("fo", "Resources" + @"/" + "flashasset");
            new GameObject("test").AddComponent<test>();

         

            //增加灯关渲染数量
            QualitySettings.pixelLightCount += 10;

        }
    

        /// <summary>
        /// MOD控制台指令
        /// </summary>
        /// <param name="args">输入的字符串</param>
        /// <param name="namedArgs"></param>
        /// <returns></returns>
        //public static string ConfigurationCommand(string[] args, IDictionary<string, string> namedArgs)
        //{
        //    if (args.Length <= 0)
        //        return "可用命令:\n" +
        //               "  mfk 子弹类型  \t 列出所有子弹类型\n" +
        //               "  mfk 弹链      \t 如何使用弹链滑条\n" +
        //               "  mfk 音量      \t 设置加特林转动音量\n";
        //    switch (args[0].ToLower())
        //    {
        //        case "子弹类型":
        //            string str = "\n";
        //            for (int i = 0; i < BulletScript.GetBulletKindNumber(); i++)
        //            {
        //                str += (i + 1).ToString() + "." + BulletScript.GetBulletKind(i + 1).ToString() + "\n";
        //            }
        //            return str;

        //        case "弹链":
        //            return "\n输入子弹类型相应数值即可调整弹链\n比如输入: 112\n弹链即设置为两发破坏弹一发曳光弹不断循环发射\n如果你不知道有哪些子弹类型输入:mfk 子弹类型 来查看\n";

        //        case "音量":
        //            if (args.Length > 1)
        //            {
        //                try
        //                {
        //                    float volume = Mathf.Clamp01(Convert.ToSingle(args[1]));
        //                    MGLauncher.Volume = volume;
        //                    return "\n音量调整已生效\n";
        //                }
        //                catch
        //                {
        //                    return "\n输入的什么鬼! 输入:mfk 音量 0-1 之间的数来调整加特林转动音量...\n";
        //                }
        //            }
        //            return "\n输入:mfk 音量 0-1 之间的数来调整加特林转动音量\n";
        //        default:
        //            return "\n输入mfk查看可用指令";

        //    }
        //}

    }

    public class test : MonoBehaviour
    {
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.I))
            {
                Debug.Log("input");
            }
        }
    }

}
