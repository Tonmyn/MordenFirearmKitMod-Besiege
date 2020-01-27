using Modding;
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
            Configuration = Configuration.FormatXDataToConfig(Modding.Configuration.GetData());
        }
    }

    public class Configuration
    {
        [Modding.Serialization.CanBeEmpty]
        public Vector3 RocketTrailEffectScale = Vector3.one;
        [Modding.Serialization.CanBeEmpty]
        public Vector3 ExplosionEffectScale = Vector3.one;
        [Modding.Serialization.CanBeEmpty]
        public Vector3 GatlingFireEffectScale = Vector3.one;
        [Modding.Serialization.CanBeEmpty]
        public Vector3 MachineGunFireEffectScale = Vector3.one;
        [Modding.Serialization.CanBeEmpty]
        public Vector3 ImpactWoodEffectScale = Vector3.one;
        [Modding.Serialization.CanBeEmpty]
        public Vector3 ImpactStoneEffectScale = Vector3.one;
        [Modding.Serialization.CanBeEmpty]
        public Vector3 ImpactMetalEffectScale = Vector3.one;

        public static Configuration FormatXDataToConfig(XDataHolder xDataHolder)
        {
            var config = new Configuration();

            string[] keys = new string[] { "RocketTrail","Explosion", "GaltingFire", "MachineGunFire", "ImpactWood", "ImpactStone", "ImpactMetal" };

            config.RocketTrailEffectScale = getValue(keys[0]);
            config.ExplosionEffectScale = getValue(keys[1]) ;
            config.GatlingFireEffectScale = getValue(keys[2]);
            config.MachineGunFireEffectScale = getValue(keys[3]);
            config.ImpactWoodEffectScale = getValue(keys[4]);
            config.ImpactStoneEffectScale = getValue(keys[5]);
            config.ImpactMetalEffectScale = getValue(keys[6]);

            return config;

            Vector3 getValue(string key)
            {
                Vector3 vector3 = Vector3.one;
                if (xDataHolder.HasKey(key))
                {
                   vector3 =  xDataHolder.ReadVector3(key);
                }
                else
                {
                    xDataHolder.Write(key, Vector3.one);
                }
                return vector3;
            }
        }

        public Configuration()
        {
            ExplosionEffectScale = Vector3.one;
            GatlingFireEffectScale = Vector3.one;
            MachineGunFireEffectScale = Vector3.one;
            ImpactWoodEffectScale = Vector3.one;
            ImpactStoneEffectScale = Vector3.one;
            ImpactMetalEffectScale = Vector3.one;
        }
    }
}
