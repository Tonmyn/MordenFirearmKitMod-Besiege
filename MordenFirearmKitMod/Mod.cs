using Modding;
using System;
using System.Collections;
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
        QuickFireGunBlock = 658,
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
        //public /*static*/ Vector3 RocketTrailEffectScale = Vector3.one;
        //public /*static*/ Vector3 BigExplosionEffectScale = Vector3.one;
        //public /*static*/ Vector3 SmokeExplosionEffectScale = Vector3.one;
        //public /*static*/ Vector3 GatlingFireEffectScale = Vector3.one;
        //public /*static*/ Vector3 MachineGunFireEffectScale = Vector3.one;
        //public /*static*/ Vector3 ImpactWoodEffectScale = Vector3.one;
        //public /*static*/ Vector3 ImpactStoneEffectScale = Vector3.one;
        //public /*static*/ Vector3 ImpactMetalEffectScale = Vector3.one;

        internal static ArrayList Propertises { get; private set; } = new ArrayList()
        {
            new Propertise<Vector3>{  Key = "RocketTrail", Value =Vector3.one },
            new Propertise<Vector3>{  Key = "Explosion", Value =Vector3.one },
            new Propertise<Vector3>{  Key = "SmokeExplosion", Value =Vector3.one },
            new Propertise<Vector3>{  Key = "GaltingFire", Value =Vector3.one },
            new Propertise<Vector3>{  Key = "MachineGunFire", Value =Vector3.one },
            new Propertise<Vector3>{  Key = "ImpactWood", Value =Vector3.one },
            new Propertise<Vector3>{  Key = "ImpactStone", Value =Vector3.one },
            new Propertise<Vector3>{  Key = "ImpactMetal", Value =Vector3.one },
        };

        public class Propertise<T>
        {
            public string Key;
            public T Value;
        }

        public T GetValue<T>(string key)
        {
            T value = default;

            foreach (Propertise<T> pro in Propertises)
            {
                if (pro.Key == key)
                {
                    value = pro.Value;
                }
            }
            return value;
        }

        public static Configuration FormatXDataToConfig(Configuration config = null)
        {
            XDataHolder xDataHolder = Modding.Configuration.GetData();
            bool reWrite = true;

            if (config == null)
            {
                reWrite = false;
                config = new Configuration();
            }

            //string[] keys = new string[] { "RocketTrail", "Explosion","SmokeExplosion", "GaltingFire", "MachineGunFire", "ImpactWood", "ImpactStone", "ImpactMetal" };

            //config.RocketTrailEffectScale = getValue(keys[0], config.RocketTrailEffectScale);
            //config.BigExplosionEffectScale = getValue(keys[1], config.BigExplosionEffectScale);
            //config.SmokeExplosionEffectScale = getValue(keys[2], config.SmokeExplosionEffectScale);
            //config.GatlingFireEffectScale = getValue(keys[3], config.GatlingFireEffectScale);
            //config.MachineGunFireEffectScale = getValue(keys[4], config.MachineGunFireEffectScale);
            //config.ImpactWoodEffectScale = getValue(keys[5], config.ImpactWoodEffectScale);
            //config.ImpactStoneEffectScale = getValue(keys[6], config.ImpactStoneEffectScale);
            //config.ImpactMetalEffectScale = getValue(keys[7], config.ImpactMetalEffectScale);

            for (int i = 0; i < Propertises.Count; i++)
            {
                var value = Propertises[i];

                if (value is Propertise<int>)
                {
                    value = getValue(value as Propertise<int>);
                }
                else if (value is Propertise<bool>)
                {
                    value = getValue(value as Propertise<bool>);
                }
                else if (value is Propertise<float>)
                {
                    value = getValue(value as Propertise<float>);
                }
                else if (value is Propertise<string>)
                {
                    value = getValue(value as Propertise<string>);
                }
                else if (value is Propertise<Vector3>)
                {
                    value = getValue(value as Propertise<Vector3>);
                }
                Propertises[i] = value;
            }

            Modding.Configuration.Save();
            return config;

            //T getValue<T>(string key, T defaultValue)
            //{
            //    if (xDataHolder.HasKey(key) && !reWrite)
            //    {
            //        defaultValue = (T)Convert.ChangeType(typeSpecialAction[typeof(T)](xDataHolder, key), typeof(T));
            //    }
            //    else
            //    {
            //        xDataHolder.Write(key, defaultValue);
            //    }
            //    return defaultValue;
            //}
            Propertise<T> getValue<T>(Propertise<T> propertise)
            {
                var key = propertise.Key;
                var defaultValue = propertise.Value;

                if (xDataHolder.HasKey(key) && !reWrite)
                {
                    defaultValue = (T)Convert.ChangeType(typeSpecialAction[typeof(T)](xDataHolder, key), typeof(T));
                }
                else
                {
                    xDataHolder.Write(key, defaultValue);
                }

                return new Propertise<T>() { Key = key, Value = defaultValue };
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
    }
}
