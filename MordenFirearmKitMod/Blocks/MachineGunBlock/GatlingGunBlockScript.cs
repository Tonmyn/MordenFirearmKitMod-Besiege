using UnityEngine;
using System.Collections;
using Modding;
using System.Collections.Generic;
using System;
using ModernFirearmKitMod.GenericScript.RayGun;
using Modding.Blocks;

namespace ModernFirearmKitMod
{
    public class GatlingGunBlockScript : LauncherBlockScript
    {

        public override float Rate { get; set; }
        public override float KnockBack { get; set; }
        public override int BulletCurrentNumber { get; set; }
        public override int BulletMaxNumber { get; set; }
        public override GameObject BulletObject { get; set; }
        public override Vector3 SpawnPoint { get; set; }
        public override Vector3 Direction { get; set; }
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
        //BulletPool bulletPool;

        MSlider StrengthSlider;
        MSlider bulletMassSlider;
        MSlider bulletDragSlider;
        MColourSlider bulletColorSlider;

        #region Network
        /// <summary>Block, GunbodyVelocity, BulletGuid,</summary>
        public static MessageType FireMessage = ModNetworking.CreateMessageType(DataType.Block, DataType.Vector3, DataType.String);
        #endregion

        public override void SafeAwake()
        {
            LaunchKey = AddKey(LanguageManager.Instance.CurrentLanguage.fire, "Fire", KeyCode.C);
            StrengthSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.strength, "Strength", 1f, 0.5f, 3f);
            RateSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.rate, "Rate", 0.05f, 0.01f, 0.3f);
            KnockBackSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.knockBack, "KnockBack", 1f, 0.1f, 3f);
            BulletNumberSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.bulletNumber, "Number", 200f, 1f, 500f);


            bulletMassSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.bulletMass, "Mass", 0.1f, 0.1f, 0.5f);
            bulletDragSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.bulletDrag, "Drag", 0.1f, 0.1f, 0.5f);
            bulletColorSlider = AddColourSlider(LanguageManager.Instance.CurrentLanguage.bulletTrailColor, "Color", Color.yellow, false);

            SpawnPoint = new Vector3(0, 0.125f, 3.75f);
            Direction = Vector3.forward;
            LaunchEnable = false;


            RotationRate = 0f;

            RotationAudioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
            RotationAudioSource.clip = ModResource.GetAudioClip("GatlingGun AudioClip");
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
            BulletCurrentNumber = BulletMaxNumber = (int)BulletNumberSlider.Value;
            Strength = StrengthSlider.Value * 5f;
            KnockBack = KnockBackSlider.Value * Strength * 4f;
            Rate = RateSlider.Value;

            initVFX();

            void initVFX()
            {
                EffectsObject = EffectsObject ?? (GameObject)Instantiate(AssetManager.Instance.MachineGun.fireEffect, transform);
                EffectsObject.transform.position = transform.TransformPoint(Vector3.forward * 3);
                EffectsObject.transform.localEulerAngles = new Vector3(0, 180, 0);
                EffectsObject.GetComponent<Reactivator>().TimeDelayToReactivate = Rate;
                EffectsObject.SetActive(false);

                material = GunVis.GetComponent<MeshRenderer>().material = Instantiate(AssetManager.Instance.GatlingGun.material);
                material.SetTexture("_MainTex", ModResource.GetTexture("GatlingGun Texture"));
                material.SetTexture("_EmissionMap", ModResource.GetTexture("GatlingGun-e Texture"));
            }     
        }

        //public override void SimulateUpdateHost()
        //{

        //    if (CJ == null)
        //    {
        //        return;
        //    }
        //    Reload();
        //    if (LaunchKey.IsHeld && BulletCurrentNumber > 0)
        //    {
        //        //RotationRate = Mathf.MoveTowards(RotationRate, RotationRateLimit, 105f * Time.timeScale * Time.deltaTime);
        //        //if (RotationRate >= RotationRateLimit && !LaunchEnable && Time.timeScale != 0)
        //        //{
        //        //    LaunchEnable = true;

        //        //    //StartCoroutine(Launch(bulletPool.Work.gameObject));
        //        //    StartCoroutine(Launch(InstanstiateBullet()));

        //        //    heat = Mathf.Clamp01(heat + 0.01f);
        //        //    EffectsObject.SetActive(true);
        //        //    EffectsObject.GetComponent<Reactivator>().Switch = true;
        //        //}
        //        fire();
        //    }
        //    else
        //    {
        //        LaunchEnable = false;
        //        heat = Mathf.Clamp01(heat - 0.05f * Time.deltaTime);
        //        EffectsObject.GetComponent<Reactivator>().Switch = false;
        //        RotationRate = Mathf.MoveTowards(RotationRate, 0, Time.timeScale * Time.deltaTime * 20);
        //    }

        //    GunVis.transform.Rotate(new Vector3(0, RotationRate, 0) * Time.timeScale);
        //    material.SetColor("_EmissionColor", new Color(heat, 0f, 0f, 0f));

        //    if (RotationRate != 0)
        //    {
        //        RotationAudioSource.pitch = RotationRate / RotationRateLimit;
        //        if (!RotationAudioSource.isPlaying) RotationAudioSource.Play();
        //    }
        //    else
        //    {
        //        RotationAudioSource.Stop();
        //    }
        //}

        //public override void SimulateUpdateClient()
        //{
        //    Debug.Log(RotationRate + " " + heat + " "+BulletCurrentNumber);
        //    Reload();
        //    if (LaunchKey.IsHeld && BulletCurrentNumber > 0)
        //    {
        //        RotationRate = Mathf.MoveTowards(RotationRate, RotationRateLimit, 105f * Time.timeScale * Time.deltaTime);
        //    }
        //    else
        //    {
        //        heat = Mathf.Clamp01(heat - 0.05f * Time.deltaTime);
        //        EffectsObject.GetComponent<Reactivator>().Switch = false;
        //        RotationRate = Mathf.MoveTowards(RotationRate, 0, Time.timeScale * Time.deltaTime * 20);
        //    }

        //    GunVis.transform.Rotate(new Vector3(0, RotationRate, 0) * Time.timeScale);
        //    material.SetColor("_EmissionColor", new Color(heat, 0f, 0f, 0f));

        //    if (RotationRate != 0)
        //    {
        //        RotationAudioSource.pitch = RotationRate / RotationRateLimit;
        //        if (!RotationAudioSource.isPlaying) RotationAudioSource.Play();
        //    }
        //    else
        //    {
        //        RotationAudioSource.Stop();
        //    }
        //}

        public override void SimulateUpdateAlways()
        {
            if (!StatMaster.isClient)
            {
                if (CJ == null)
                {
                    return;
                }
            }
            Reload();
            if (LaunchKey.IsHeld && BulletCurrentNumber > 0)
            {
                RotationRate = Mathf.MoveTowards(RotationRate, RotationRateLimit, 105f * Time.timeScale * Time.deltaTime);
                if (!StatMaster.isClient)
                {
                    fire();
                }
            }
            else
            {
                if (!StatMaster.isClient)
                {
                    LaunchEnable = false;
                }
                heat = Mathf.Clamp01(heat - 0.05f * Time.deltaTime);
                EffectsObject.GetComponent<Reactivator>().Switch = false;
                RotationRate = Mathf.MoveTowards(RotationRate, 0, Time.timeScale * Time.deltaTime * 20);
            }

            GunVis.transform.Rotate(new Vector3(0, RotationRate, 0) * Time.timeScale);
            material.SetColor("_EmissionColor", new Color(heat, 0f, 0f, 0f));

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
            if (/*StatMaster.GodTools.InfiniteAmmoMode*/ Machine.InfiniteAmmo)
            {
                BulletCurrentNumber = BulletMaxNumber;
            }
        }

        private void fire()
        {
           
            if (RotationRate >= RotationRateLimit && !LaunchEnable && Time.timeScale != 0)
            {
                LaunchEnable = true;

                //StartCoroutine(Launch(bulletPool.Work.gameObject));
                StartCoroutine(Launch(BulletParticleEffectEvent));
            }

             void BulletParticleEffectEvent()
            {
                var bullet = RayBulletScript.CreateBullet(Strength, transform.TransformPoint(SpawnPoint), transform.forward, Rigidbody.velocity, bulletMassSlider.Value, bulletDragSlider.Value, bulletColorSlider.Value, transform);

                var message = FireMessage.CreateMessage(BlockBehaviour, Rigidbody.velocity, bullet.GetComponent<RayBulletScript>().Guid.ToString());
                ModNetworking.SendToAll(message);

                heat = Mathf.Clamp01(heat + 0.01f);
                EffectsObject.SetActive(true);
                EffectsObject.GetComponent<Reactivator>().Switch = true;
            }
        }
        //private void fire_Network(Vector3 velocity, Guid guid)
        //{
        //    BulletCurrentNumber--;

        //    var bullet = RayBulletScript.CreateBullet(Strength, transform.TransformPoint(SpawnPoint), transform.forward, velocity, bulletMassSlider.Value, bulletDragSlider.Value, bulletColorSlider.Value, transform);
        //    bullet.GetComponent<RayBulletScript>().Guid = guid;

        //    heat = Mathf.Clamp01(heat + 0.01f);
        //    EffectsObject.SetActive(true);
        //    EffectsObject.GetComponent<Reactivator>().Switch = true;
        //}

        void fire_Network(Vector3 velocity, Guid guid)
        {
            BulletCurrentNumber--;

            var bullet = RayBulletScript.CreateBullet(Strength, transform.TransformPoint(SpawnPoint), transform.forward, velocity, bulletMassSlider.Value, bulletDragSlider.Value, bulletColorSlider.Value, transform);
            bullet.GetComponent<RayBulletScript>().Guid = guid;

            heat = Mathf.Clamp01(heat + 0.01f);
            EffectsObject.SetActive(true);
            EffectsObject.GetComponent<Reactivator>().Switch = true;
        }

        public static void FireNetworkingEvent(Message message)
        {
            if (StatMaster.isClient)
            {
                var block = ((Block)message.GetData(0));
                var velocity = (Vector3)message.GetData(1);
                var guid = new Guid(((string)message.GetData(2)));
                GameObject gameObject = block.GameObject;

                var ggbs = gameObject.GetComponent<GatlingGunBlockScript>();
                ggbs.fire_Network(velocity, guid);
            }
        }
    }
}





