using UnityEngine;
using System.Collections;
using Modding;
using System.Collections.Generic;
using System;
using ModernFirearmKitMod.GenericScript.RayGun;
using Modding.Blocks;

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
        public override Vector3 Direction { get ; set; }
        public override bool LaunchEnable { get; set; }

        public float Strength { get; set; }
        //加特林热度
        float heat = 0;
        ////加特林转速
        //float RotationRate;
        ////加特林转速限制
        //float RotationRateLimit = 60;

        //机枪开火音效
        AudioSource fireAudioSource;

        ConfigurableJoint CJ;
        GameObject EffectsObject;
        GameObject GunVis;
        Material material;
        BulletPool bulletPool;

        MSlider StrengthSlider;
        MSlider bulletMassSlider;
        MSlider bulletDragSlider;
        MColourSlider bulletColorSlider;

        //#region Network
        ///// <summary>Block, GunbodyVelocity, BulletGuid,</summary>
        //public static MessageType FireMessage = ModNetworking.CreateMessageType(DataType.Block, DataType.Vector3, DataType.String);
        //#endregion

        public override void SafeAwake()
        {
            LaunchKey = AddKey(LanguageManager.Instance.CurrentLanguage.fire, "Fire", KeyCode.C);
            StrengthSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.strength, "Strength", 1f, 0.5f, 3f);
            //StrengthSlider.ValueChanged += (value) => { Strength = value; };
            RateSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.rate, "Rate", 0.05f, 0.1f, 0.3f);
            //RateSlider.ValueChanged += (value) => { Rate = value; };
            KnockBackSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.knockBack, "KnockBack", 1f, 0.1f, 3f);
            //KnockBackSlider.ValueChanged += (value) => { KnockBack = value; };
            BulletNumberSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.bulletNumber, "Number", 200f, 1f, 500f);
            //BulletNumberSlider.ValueChanged += (value) => { BulletMaxNumber = (int)value; };

            bulletMassSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.bulletMass, "Mass", 0.1f, 0.1f, 0.5f);
            bulletDragSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.bulletDrag, "Drag", 0.1f, 0.1f, 0.5f);
            bulletColorSlider = AddColourSlider(LanguageManager.Instance.CurrentLanguage.bulletTrailColor, "Color", Color.yellow, false);

            //KnockBack = 1f;
            SpawnPoint = new Vector3(-2.65f,0f, 0.5f);
            Direction = -Vector3.right;
            //BulletObject = AssetManager.Instance.MachineGun.bulletTemp;
            //BulletObject = InstanstiateBullet();
            LaunchEnable = false;


            //RotationRate = 0f;

            fireAudioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
            fireAudioSource.clip = ModResource.GetAudioClip("MachineGun AudioClip");
            fireAudioSource.loop = false;
            fireAudioSource.volume = 1;
            fireAudioSource.spatialBlend = 5f;
            fireAudioSource.maxDistance = 15f;

            CJ = GetComponent<ConfigurableJoint>();

            List<MeshFilter> meshFilters = new List<MeshFilter>();
            GetComponentsInChildren(false, meshFilters);
            GunVis = meshFilters.Find(match => match.name == "Vis").gameObject;

        }

        public override void OnSimulateStart()
        {
            BulletCurrentNumber = BulletMaxNumber = (int)BulletNumberSlider.Value;
            SpawnPoint = this.BlockBehaviour.BuildingBlock.GetComponent<MachineGunBlockScript>().SpawnPoint;
            Strength = StrengthSlider.Value * 5f;
            KnockBack = KnockBackSlider.Value * Strength * 4f;
            Rate = RateSlider.Value;

            initVFX();

            void initVFX()
            {
                EffectsObject = EffectsObject ?? (GameObject)Instantiate(AssetManager.Instance.MachineGun.fireEffect, transform);
                EffectsObject.transform.position = transform.TransformPoint(SpawnPoint);
                EffectsObject.transform.localEulerAngles = new Vector3(0, 90, 0);
                EffectsObject.transform.localScale = Vector3.one * 0.65f;
                EffectsObject.GetComponent<Reactivator>().TimeDelayToReactivate = Rate;
                EffectsObject.SetActive(false);

                material = GunVis.GetComponent<MeshRenderer>().material = Instantiate(AssetManager.Instance.MachineGun.material);
                material.SetTexture("_MainTex", ModResource.GetTexture("MachineGun Texture"));
                material.SetTexture("_EmissionMap", ModResource.GetTexture("MachineGun-e Texture"));
            }     
        }

        //public override void SimulateUpdateHost()
        //{
          

        //}

        public override void SimulateUpdateAlways()
        {
            Reload();
            if ((LaunchKey.IsHeld||LaunchKey.EmulationHeld()) && BulletCurrentNumber > 0)
            {
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
            }

            material.SetColor("_EmissionColor", new Color(heat, 0f, 0f, 0f));

        }

        private void fire()
        {
            if (!LaunchEnable && Time.timeScale != 0)
            {
                LaunchEnable = true;
                StartCoroutine(Launch(BulletParticleEffectEvent));
            }

            void BulletParticleEffectEvent()
            {
                var bullet = RayBulletScript.CreateBullet(Strength, transform.TransformPoint(SpawnPoint), -transform.right, Rigidbody.velocity, bulletMassSlider.Value, bulletDragSlider.Value, bulletColorSlider.Value, transform);
                var message = FireMessage.CreateMessage(BlockBehaviour, Rigidbody.velocity, bullet.GetComponent<RayBulletScript>().Guid.ToString());
                ModNetworking.SendToAll(message);

                heat = Mathf.Clamp01(heat + 0.01f);
                EffectsObject.SetActive(true);
                EffectsObject.GetComponent<Reactivator>().Switch = true;

                fireAudioSource.PlayOneShot(fireAudioSource.clip);
            }
        }

        internal override void Launch_Network(Vector3 velocity, Guid guid)
        {
            var bullet = RayBulletScript.CreateBullet(Strength, transform.TransformPoint(SpawnPoint), -transform.right, velocity, bulletMassSlider.Value, bulletDragSlider.Value, bulletColorSlider.Value, transform);
            bullet.GetComponent<RayBulletScript>().Guid = guid;

            heat = Mathf.Clamp01(heat + 0.01f);
            EffectsObject.SetActive(true);
            EffectsObject.GetComponent<Reactivator>().Switch = true;

            fireAudioSource.PlayOneShot(fireAudioSource.clip);
        }

        //void fire_Network(Vector3 velocity,Guid guid)
        //{
        //    var bullet = RayBulletScript.CreateBullet(Strength, transform.TransformPoint(SpawnPoint), -transform.right, velocity, bulletMassSlider.Value, bulletDragSlider.Value, bulletColorSlider.Value, transform);
        //    bullet.GetComponent<RayBulletScript>().Guid = guid;

        //    heat = Mathf.Clamp01(heat + 0.01f);
        //    EffectsObject.SetActive(true);
        //    EffectsObject.GetComponent<Reactivator>().Switch = true;

        //    fireAudioSource.PlayOneShot(fireAudioSource.clip);
        //}

        public override void Reload(bool constraint = false)
        {
            if (/*StatMaster.GodTools.InfiniteAmmoMode*/ Machine.InfiniteAmmo)
            {
                BulletCurrentNumber = BulletMaxNumber;
            }
        }

        //public static void FireNetworkingEvent(Message message)
        //{
        //    if (StatMaster.isClient)
        //    {
        //        var block = ((Block)message.GetData(0));
        //        var velocity = (Vector3)message.GetData(1);
        //        var guid = new Guid(((string)message.GetData(2)));
        //        GameObject gameObject = block.GameObject;

        //        var mgbs = gameObject.GetComponent<MachineGunBlockScript>();
        //        mgbs.fire_Network(velocity, guid);
        //    }
        //}
    }
}





