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
            Explosion = new Asset_Explosion(modAssetBundle);
            MachineGun = new Asset_MachineGun(modAssetBundle);
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
        GameObject fireEffect;

        public Asset_MachineGun(ModAssetBundle modAssetBundle)
        {
            fireEffect = modAssetBundle.LoadAsset<GameObject>("MachineGunEffect");
            fireEffect.AddComponent<DestroyIfEditMode>();

            FPSDemoReactivator fPSDemo = fireEffect.AddComponent<FPSDemoReactivator>();
            fPSDemo.StartDelay = 0.2f;
            fPSDemo.TimeDelayToReactivate = 1;
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
                    Debug.Log("add");
                }
                else if (go.name == "MuzzleFlash0")
                {
                    go.AddComponent<FPSRandomRotateAngle>().RotateZ = true;
                }
                else if (go.name == "MuzzleFlash")
                {
                    go.transform.localPosition = new Vector3(0, 0, -0.2f);
                    go.transform.localScale = new Vector3(0.1f, 0.1f, 0.07f);
                }
                else if (go.name == "Distortion")
                {
                    go.transform.localPosition = new Vector3(0, 0, -0.5f);
                }
            }
        }
    }
}
