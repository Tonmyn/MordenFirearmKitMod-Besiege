using Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ModernFirearmKitMod
{
    public class AssetManager : SingleInstance<AssetManager>
    {
        public override string Name { get; } = "Asset Manager";

        public Asset_Rocket Rocket { get; private set; }
        public Asset_Explosion Explosion { get; private set; }
        public Asset_MachineGun MachineGun { get; private set; }
              
        private void Awake()
        {          
            StartCoroutine(LoadAssetBundle());        
        }
        private IEnumerator LoadAssetBundle()
        {
            ModResource.CreateAssetBundleResource("Effect", @"Resources/bundle");
            ModAssetBundle modAssetBundle = ModResource.GetAssetBundle("Effect");
            yield return new WaitUntil(() => modAssetBundle.Available);
            Rocket = new Asset_Rocket(modAssetBundle);
            Explosion = new Asset_Explosion(modAssetBundle);
            MachineGun = new Asset_MachineGun(modAssetBundle);
        }     
    }

    public class Asset_Rocket
    {
        public GameObject rocketTrailEffect;

        public Asset_Rocket(ModAssetBundle modAssetBundle)
        {
            rocketTrailEffect = modAssetBundle.LoadAsset<GameObject>("RocketTrailEffect");
        }
    }
    public class Asset_Explosion 
    {
        //爆炸特效
        public GameObject explosionEffect;

        public Asset_Explosion(ModAssetBundle modAssetBundle)
        {
            explosionEffect = modAssetBundle.LoadAsset<GameObject>("BigExplosion");
            explosionEffect.AddComponent<DestroyIfEditMode>();
        }
    }
    public class Asset_MachineGun
    {
        //开火特效
        public GameObject fireEffect;
        public Material material;

        public Asset_MachineGun(ModAssetBundle modAssetBundle)
        {
            material = modAssetBundle.LoadAsset<Material>("MachineGunMat.mat");

            fireEffect = modAssetBundle.LoadAsset<GameObject>("MachineGunFireEffect");
            fireEffect.AddComponent<DestroyIfEditMode>();

            Reactivator fPSDemo = fireEffect.AddComponent<Reactivator>();
            for (int i = 0; i < fireEffect.transform.childCount; i++)
            {
                var go = fireEffect.transform.GetChild(i).gameObject;
                if (go.name == "Point light")
                {
                    FPSLightCurves fPSLight = go.AddComponent<FPSLightCurves>();
                    fPSLight.GraphTimeMultiplier = 0.15f;
                    fPSLight.GraphIntensityMultiplier = 1;
                    Vector3 vector3 = go.transform.localPosition;
                    vector3.z = -0.5f;
                    go.transform.localPosition = vector3;
                }
                else if (go.name == "MuzzleFlash0")
                {
                    go.AddComponent<FPSRandomRotateAngle>().RotateZ = true;
                    go.transform.localScale = new Vector3(0.6f, 0.6f, 0f);
                }
                else if (go.name == "MuzzleFlash")
                {
                    go.transform.localPosition = new Vector3(0, 0, -0.2f);
                    go.transform.localScale = new Vector3(0.1f, 0.1f, 0.065f);
                }
                else if (go.name == "Distortion")
                {
                    go.transform.localPosition = new Vector3(0, 0, -0.5f);
                }
            }
        }
    }
}
