using Modding;
using System;
using System.Collections.Generic;
using UnityEngine;
using Modding.Common;
using Modding.Blocks;
using System.Collections;
using UnityEngine.SceneManagement;
using Modding;

namespace ModernFirearmKitMod
{

    // If you need documentation about any of these values or the mod loader
    // in general, take a look at http://wiki.spiderlinggames.co.uk/besiege/modding-documentation/articles/introduction.html.

    public enum BlockList
    {
        RocketBlock = 650,
        RocketPodBlock = 651,
        GatlingGunBlock = 655,
        MachineGunBlock = 656,
        GunBarrelBlock = 657,
        DirectionBlock = 660,
    }

    public class MordenFirearmKitBlockMod : ModEntryPoint
    {

        public static GameObject Mod;
        public static GameObject RocketPool_Idle;
        public static GameObject MachineGunBulletPool_Idle;



        public override void OnLoad()
        {
            // Your initialization code here

            Mod = new GameObject("Morden Firearm Kit Mod");
            UnityEngine.Object.DontDestroyOnLoad(Mod);
            RocketPool_Idle = new GameObject("Rocket Pool Idle");
            RocketPool_Idle.transform.SetParent(Mod.transform);
            MachineGunBulletPool_Idle = new GameObject("MachineGunBullet Pool Idle");
            MachineGunBulletPool_Idle.transform.SetParent(Mod.transform);

            AssetManager.Instance.transform.SetParent(Mod.transform);
            LanguageManager.Instance.transform.SetParent(Mod.transform);
            NetworkMessageManager.Instance.transform.SetParent(Mod.transform);
            //SceneManager.sceneLoaded += (s, a) => { Debug.Log("loaded"); };

            //增加灯光渲染数量
            //QualitySettings.pixelLightCount += 10;

        }
    } 
}
