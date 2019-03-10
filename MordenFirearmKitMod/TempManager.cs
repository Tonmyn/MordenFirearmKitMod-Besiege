using Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ModernFirearmKitMod
{
    public class TempManager : MonoBehaviour
    {

        public static GameObject mgb;

        public static IEnumerator createVFX()
        {
            ModAssetBundle modAssetBundle = ModResource.GetAssetBundle("Effect");
            yield return new WaitUntil(() => modAssetBundle.Available);
            mgb = modAssetBundle.LoadAsset<GameObject>("MachineGunEffect");
            FPSDemoReactivator fPSDemo = mgb.AddComponent<FPSDemoReactivator>();
            fPSDemo.StartDelay = 0.2f;
            fPSDemo.TimeDelayToReactivate = 1;
            for (int i = 0; i < mgb.transform.childCount; i++)
            {
                var go = mgb.transform.GetChild(i).gameObject;
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
