using Modding;
using System;
using System.Collections.Generic;
using UnityEngine;

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
        public static Configuration Configuration;

        public override void OnLoad()
        {
            // Your initialization code here

            loadConfiguration();

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

        private void loadConfiguration()
        {
            Configuration = Configuration.FormatXDataToConfig();
        }
    }

    public class Configuration
    {
        public Vector3 RocketTrailEffectScale = Vector3.one;
        public Vector3 ExplosionEffectScale = Vector3.one;
        public Vector3 GatlingFireEffectScale = Vector3.one;
        public Vector3 MachineGunFireEffectScale = Vector3.one;
        public Vector3 ImpactWoodEffectScale = Vector3.one;
        public Vector3 ImpactStoneEffectScale = Vector3.one;
        public Vector3 ImpactMetalEffectScale = Vector3.one;

        public static Configuration FormatXDataToConfig(Configuration config = null)
        {
            XDataHolder xDataHolder = Modding.Configuration.GetData();
            bool reWrite = true;

            if (config == null)
            {
                reWrite = false;
                config = new Configuration();
            }

            string[] keys = new string[] { "RocketTrail","Explosion", "GaltingFire", "MachineGunFire", "ImpactWood", "ImpactStone", "ImpactMetal" };

            config.RocketTrailEffectScale = getValue(keys[0],config.RocketTrailEffectScale);
            config.ExplosionEffectScale = getValue(keys[1], config.ExplosionEffectScale) ;
            config.GatlingFireEffectScale = getValue(keys[2], config.GatlingFireEffectScale);
            config.MachineGunFireEffectScale = getValue(keys[3], config.MachineGunFireEffectScale);
            config.ImpactWoodEffectScale = getValue(keys[4], config.ImpactWoodEffectScale);
            config.ImpactStoneEffectScale = getValue(keys[5], config.ImpactStoneEffectScale);
            config.ImpactMetalEffectScale = getValue(keys[6], config.ImpactMetalEffectScale);

            Modding.Configuration.Save();
            return config;

            T getValue<T>(string key, T defaultValue)
            {
                foreach (var type in typeSpecialAction.Keys)
                {
                    if (defaultValue.GetType() == type)
                    {
                        if (xDataHolder.HasKey(key) && !reWrite)
                        {
                            defaultValue = (T)Convert.ChangeType(typeSpecialAction[type](xDataHolder, key), typeof(T));
                        }
                        else
                        {
                            xDataHolder.Write(key, defaultValue);
                        }
                        break;
                    }
                }
                return defaultValue;
            }
        }
        private static Dictionary<Type, Func<XDataHolder, string, object>> typeSpecialAction = new Dictionary<Type, Func<XDataHolder, string, object>>
        {
            { typeof(int), (xDataHolder,key)=>xDataHolder.ReadInt(key)},
            { typeof(bool), (xDataHolder,key)=>xDataHolder.ReadBool(key)},
            { typeof(float), (xDataHolder,key)=>xDataHolder.ReadFloat(key)},
            { typeof(string), (xDataHolder,key)=>xDataHolder.ReadString(key)},
            { typeof(Vector3), (xDataHolder,key)=>xDataHolder.ReadVector3(key)},
        };
        //public Configuration()
        //{
        //    ExplosionEffectScale = Vector3.one;
        //    GatlingFireEffectScale = Vector3.one;
        //    MachineGunFireEffectScale = Vector3.one;
        //    ImpactWoodEffectScale = Vector3.one;
        //    ImpactStoneEffectScale = Vector3.one;
        //    ImpactMetalEffectScale = Vector3.one;
        //}
    }
}
