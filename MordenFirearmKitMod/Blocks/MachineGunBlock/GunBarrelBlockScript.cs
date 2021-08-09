﻿using Modding;
using Modding.Blocks;
using ModernFirearmKitMod.GenericScript.RayGun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ModernFirearmKitMod
{
    class GunBarrelBlockScript:LauncherBlockScript
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

        //机枪开火音效
        AudioSource fireAudioSource;

        ConfigurableJoint CJ;
        GameObject EffectsObject;
       
        MSlider StrengthSlider;
        MSlider bulletMassSlider;
        MSlider bulletDragSlider;
        MColourSlider bulletColorSlider;
        MSlider bulletCollisionEnableTimeSlider;

        MToggle holdToggle;
        MSlider spawnDistanceSlider;
        MSlider damperSlider;

        //#region Network
        ///// <summary>Block, GunbodyVelocity, BulletGuid</summary>
        //public static MessageType FireMessage = ModNetworking.CreateMessageType(DataType.Block, DataType.Vector3, DataType.String);
        //#endregion

        public override void SafeAwake()
        {
            LaunchKey = AddKey(LanguageManager.Instance.CurrentLanguage.fire, "Fire", KeyCode.C);
            StrengthSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.strength, "Strength", 1f, 0.5f, 3f);
            RateSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.rate, "Rate", 0.05f, 0.1f, 0.3f);
 
            KnockBackSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.knockBack, "KnockBack", 1f, 0.1f, 3f);
            BulletNumberSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.bulletNumber, "Number", 200f, 1f, 500f);

            bulletMassSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.bulletMass, "Mass", 0.1f, 0.1f, 0.5f);
            bulletDragSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.bulletDrag, "Drag", 0.1f, 0.1f, 0.5f);
            bulletColorSlider = AddColourSlider(LanguageManager.Instance.CurrentLanguage.bulletTrailColor, "Color", Color.yellow, false);
            bulletCollisionEnableTimeSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.bulletCollisionEnableTime, "Collision Enable Time", 0.01f, 0f, 0.1f);

            holdToggle = AddToggle(LanguageManager.Instance.CurrentLanguage.hold, "Hold", true);
            spawnDistanceSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.distance, "Distance", 2f, 0f, 4f);
            spawnDistanceSlider.ValueChanged += (value) => { SpawnPoint = new Vector3(0f, 0f, value); };
            damperSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.damper, "Damper", 1f, 0f, 5f);
           
            SpawnPoint = new Vector3(0.0f, 0.0f, spawnDistanceSlider.Value);
            Direction = Vector3.forward;
            LaunchEnable = false;

            fireAudioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
            fireAudioSource.clip = ModResource.GetAudioClip("GunBarrel AudioClip");
            fireAudioSource.loop = false;
            fireAudioSource.volume = 1;
            fireAudioSource.spatialBlend = 5f;
            fireAudioSource.maxDistance = 15f;

            CJ = GetComponent<ConfigurableJoint>();
            CJ.yMotion = ConfigurableJointMotion.Free;
            var yd = CJ.yDrive;
            yd.positionDamper = 750f * damperSlider.Value;
            yd.positionSpring = 1000f;
            CJ.yDrive = yd;

     
        }

        public override void OnSimulateStart()
        {
            BulletCurrentNumber = BulletMaxNumber = (int)BulletNumberSlider.Value;
            Strength = StrengthSlider.Value * 5f;
            KnockBack = KnockBackSlider.Value * Strength * 4f;
            Rate = RateSlider.Value;

            var yd = CJ.yDrive;
            yd.positionDamper = 500f * damperSlider.Value;
            yd.positionSpring = 3000f;
            CJ.yDrive = yd;

            initVFX();
   
            void initVFX()
            {
                EffectsObject = EffectsObject ?? (GameObject)Instantiate(AssetManager.Instance.MachineGun.fireEffect, transform);
                EffectsObject.transform.position = transform.TransformPoint(SpawnPoint);
                EffectsObject.transform.localEulerAngles = new Vector3(0, 180f, 0);
                EffectsObject.transform.localScale = Vector3.one * 0.65f;
                EffectsObject.GetComponent<Reactivator>().TimeDelayToReactivate = Rate;
                EffectsObject.SetActive(false);
            }
        }

        public override void SimulateUpdateAlways()
        {

            if (StatMaster.isClient) return;
            Reload();
            if ( BulletCurrentNumber > 0)
            {
                if ((holdToggle.IsActive && (LaunchKey.IsHeld|| LaunchKey.EmulationHeld()))|| (!holdToggle.IsActive&& LaunchKey.IsPressed))
                {
                    if (!StatMaster.isClient)
                    {
                        fire();
                    }         
                }         
            }
            else
            {
                if (!StatMaster.isClient)
                {
                    LaunchEnable = false;
                }
                EffectsObject.GetComponent<Reactivator>().Switch = false;
            }     
        }

        private void fireBaseMethod(Action<GameObject> additiveAction = null)
        {
            var bulletPropertise = new RayBulletScript.BulletPropertise()
            {
                Strength = this.Strength,
                orginPosition = this.transform.TransformPoint(SpawnPoint),
                direction = this.transform.forward,
                Velocity = StatMaster.isClient ? Vector3.zero : this.Rigidbody.velocity,
                Mass = this.bulletMassSlider.Value,
                Drag = this.bulletDragSlider.Value,
                color = this.bulletColorSlider.Value,
                ColliderEnableTime = this.bulletCollisionEnableTimeSlider.Value
            };
            var bullet = RayBulletScript.CreateBullet(bulletPropertise, transform);

            additiveAction.Invoke(bullet);

            EffectsObject.SetActive(true);
            EffectsObject.GetComponent<Reactivator>().Switch = true;
        }
        void fire()
        {
   
            if (!LaunchEnable && Time.timeScale != 0)
            {
                LaunchEnable = true;                
                StartCoroutine(Launch(BulletParticleEffectEvent));
            }

            void BulletParticleEffectEvent()
            {
                fireBaseMethod((bullet) =>
                {
                    var message = FireMessage.CreateMessage(BlockBehaviour, Rigidbody.velocity, bullet.GetComponent<RayBulletScript>().Guid.ToString());
                    ModNetworking.SendToAll(message);

                    fireAudioSource.PlayOneShot(fireAudioSource.clip);
                });
            }
        }
        internal override void Launch_Network(Vector3 velocity, Guid guid)
        {
            fireBaseMethod((bullet) =>
            {
                bullet.GetComponent<RayBulletScript>().Guid = guid;
                bullet.GetComponent<RayBulletScript>().bulletPropertise.Velocity = velocity;
                fireAudioSource.PlayOneShot(fireAudioSource.clip);
            });
        }

        //public override void Reload(bool constraint = false)
        //{
        //    if (/*StatMaster.GodTools.InfiniteAmmoMode*/Machine.InfiniteAmmo)
        //    {
        //        BulletCurrentNumber = BulletMaxNumber;
        //    }
        //}

        //public static void FireNetworkingEvent(Message message)
        //{
        //    if (StatMaster.isClient)
        //    {
        //        var block = ((Block)message.GetData(0));
        //        var velocity = (Vector3)message.GetData(1);
        //        var guid = new Guid(((string)message.GetData(2)));
        //        GameObject gameObject = block.GameObject;

        //        var gbbs = gameObject.GetComponent<GunBarrelBlockScript>();
        //        gbbs.fire_Network(velocity, guid);
        //    }
        //}

    }
}
