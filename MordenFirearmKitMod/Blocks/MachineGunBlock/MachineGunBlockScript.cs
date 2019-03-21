using UnityEngine;
using System.Collections;
using Modding;
using System.Collections.Generic;
using System;

namespace ModernFirearmKitMod
{
    public class MachineGunBlockScript : LauncherBlockScript
    {

        public override float Rate { get; set; }
        public override float KnockBack { get; set; }
        public override int BulletCurrentNumber { get; set; }
        public override int BulletMaxNumber { get; set; }
        public override GameObject BulletObject { get; set; }
        public override Vector3 SpawnPoint { get; set; }
        public override bool LaunchEnable { get; set; }

        public float Strength { get; set; }
        //加特林热度
        float heat = 0;
        //加特林转速
        float RotationRate;
        //加特林转速限制
        float RotationRateLimit = 60;

        //加特林转动声音
        AudioSource RotationAudioSource;

        ConfigurableJoint CJ;
        GameObject EffectsObject;
        GameObject GunVis;    
        Material material;

        MSlider StrengthSlider;

        public override void SafeAwake()
        {
            LaunchKey = AddKey("Fire", "Fire", KeyCode.C);
            StrengthSlider = AddSlider("Strength", "Strength", 1f, 0.5f, 3f);
            //StrengthSlider.ValueChanged += (value) => { Strength = value; };
            RateSlider = AddSlider("Rate", "Rate", 0.2f, 0.1f, 0.3f);
            //RateSlider.ValueChanged += (value) => { Rate = value; };
            KnockBackSlider = AddSlider("KnockBack", "KnockBack", 3f, 1f, 3f);
            //KnockBackSlider.ValueChanged += (value) => { KnockBack = value; };
            BulletNumberSlider = AddSlider("Bullet" + Environment.NewLine + "Number", "Number", 200f, 1f, 500f);
            //BulletNumberSlider.ValueChanged += (value) => { BulletMaxNumber = (int)value; };

            //KnockBack = 1f;
            SpawnPoint = new Vector3(0, 0.125f, 3.5f);

            BulletObject = AssetManager.Instance.MachineGun.bulletTemp;
            BulletScript bulletScript = BulletObject.GetComponent<BulletScript>();
            //bulletScript.Strength = Strength*200f;
            bulletScript.Direction = transform.InverseTransformDirection(transform.forward);
            //BulletCurrentNumber = BulletMaxNumber = 500;
            //Rate = 0.2f;
            LaunchEnable = false;


            RotationRate = 0f;

            RotationAudioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
            RotationAudioSource.clip = ModResource.GetAudioClip("MachineGun AudioClip");
            RotationAudioSource.loop = true;
            RotationAudioSource.volume = 1;
            RotationAudioSource.spatialBlend = 1;
            RotationAudioSource.maxDistance = 15;

            CJ = GetComponent<ConfigurableJoint>();

            List<MeshFilter> meshFilters = new List<MeshFilter>();
            GetComponentsInChildren(false, meshFilters);
            GunVis = meshFilters.Find(match => match.name == "Vis").gameObject;
           
        }

        public override void OnSimulateStart()
        {
            BulletObject.GetComponent<BulletScript>().Strength = Strength = StrengthSlider.Value * 200f;
            Rate = RateSlider.Value;
            KnockBack = KnockBackSlider.Value;
            BulletCurrentNumber = BulletMaxNumber = (int)BulletNumberSlider.Value;

            initVFX();
        }

        public override void SimulateUpdateHost()
        {
            
            if (CJ == null)
            {
                return;
            }
            Reload();
            if (LaunchKey.IsDown && BulletCurrentNumber > 0)
            {
                RotationRate = Mathf.MoveTowards(RotationRate, RotationRateLimit, 20 * Time.timeScale * Time.deltaTime);
                if (RotationRate >= RotationRateLimit && !LaunchEnable && Time.timeScale != 0)
                {
                    LaunchEnable = true;                  
                    StartCoroutine(Launch());
                    heat=Mathf.Clamp01(heat + 0.01f);
                    EffectsObject.SetActive(true);
                    EffectsObject.GetComponent<Reactivator>().Switch = true;
                }
            }
            else
            {
                LaunchEnable = false;
                heat = Mathf.Clamp01(heat - 0.05f * Time.deltaTime);
                EffectsObject.GetComponent<Reactivator>().Switch = false;       
                RotationRate = Mathf.MoveTowards(RotationRate, 0, Time.timeScale * Time.deltaTime * 10);         
            }

            GunVis.transform.Rotate(new Vector3(0, RotationRate, 0) * Time.timeScale);
            material.SetColor("_EmissionColor",new Color(heat,0f,0f,0f));
           
            if (RotationRate != 0)
            {
                RotationAudioSource.pitch = RotationRate / RotationRateLimit;
                if (!RotationAudioSource.isPlaying) RotationAudioSource.Play();
            }
            else
            {
                RotationAudioSource.Stop();
            }
        }

        public override void Reload(bool constraint = false)
        {
            if (StatMaster.GodTools.InfiniteAmmoMode) BulletCurrentNumber = BulletMaxNumber;
        }

        void initVFX()
        {
            EffectsObject = EffectsObject ?? (GameObject)Instantiate(AssetManager.Instance.MachineGun.fireEffect, transform);
            EffectsObject.transform.position = transform.TransformPoint(Vector3.forward * 3);
            EffectsObject.transform.localEulerAngles = new Vector3(0, 180, 0);
            EffectsObject.GetComponent<Reactivator>().TimeDelayToReactivate = Rate;
            EffectsObject.SetActive(false);

            material = GunVis.GetComponent<MeshRenderer>().material = AssetManager.Instance.MachineGun.material;
            material.SetTexture("_MainTex", ModResource.GetTexture("MachineGun Texture"));
            material.SetTexture("_EmissionMap", ModResource.GetTexture("MachineGun-e Texture"));
        }
    }
}





