using Modding;
using Modding.Blocks;
using ModernFirearmKitMod.GenericScript.RayGun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ModernFirearmKitMod
{
    class QuickFireGunBlockScript : LauncherBlockScript
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
        private float trailTimeFactor = MordenFirearmKitBlockMod.Configuration.GetValue<float>("QFG-TrailLength");
        private float trailWidthFactor = MordenFirearmKitBlockMod.Configuration.GetValue<float>("QFG-TrailWidth");
        private float cetFactor = MordenFirearmKitBlockMod.Configuration.GetValue<float>("QFG-CollisionEnableTime");
        //机枪开火音效
        AudioSource fireAudioSource;

        ConfigurableJoint CJ;
        GameObject EffectsObject;

        MSlider StrengthSlider;
        MSlider bulletPowerSlider;
        MSlider bulletMassSlider;
        MSlider bulletDragSlider;
        //MSlider bulletTrailLengthSlider;
        MColourSlider bulletColorSlider;

        MToggle holdToggle;
        //MSlider spawnDistanceSlider;
        MSlider damperSlider;


        public override void SafeAwake()
        {
            LaunchKey = AddKey(LanguageManager.Instance.CurrentLanguage.fire, "Fire", KeyCode.C);
            StrengthSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.strength, "Strength", 1f, 0.5f, 3f);
            RateSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.rate, "Rate", 0.35f, 0.1f, 0.3f);

            KnockBackSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.knockBack, "KnockBack", 0.5f, 0.1f, 1f);
            BulletNumberSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.bulletNumber, "Number", 200f, 1f, 500f);

            bulletPowerSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.bulletPower, "Power", 0.3f, 0.1f, 1f);
            bulletMassSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.bulletMass, "Mass", 0.1f, 0.1f, 0.5f);
            bulletDragSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.bulletDrag, "Drag", 0.1f, 0.1f, 0.5f);
            //bulletTrailLengthSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.bulletTrailLength, "Length", 0.1f, 0f, 0.4f);
            bulletColorSlider = AddColourSlider(LanguageManager.Instance.CurrentLanguage.bulletTrailColor, "Color", Color.yellow, false);


            holdToggle = AddToggle(LanguageManager.Instance.CurrentLanguage.hold, "Hold", true);
            //spawnDistanceSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.distance, "Distance", 2f, 0f, 4f);
            //spawnDistanceSlider.ValueChanged += (value) => { SpawnPoint = new Vector3(0f, 0f, value); };
            damperSlider = AddSlider(LanguageManager.Instance.CurrentLanguage.damper, "Damper", 1f, 0f, 5f);

            SpawnPoint = new Vector3(0.0f, 0.0f, 3.5f);
            Direction = Vector3.forward;
            LaunchEnable = false;

            fireAudioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
            fireAudioSource.clip = ModResource.GetAudioClip("QuickFireGun AudioClip");
            fireAudioSource.loop = false;
            fireAudioSource.volume = 1;
            fireAudioSource.spatialBlend = 5f;
            fireAudioSource.maxDistance = 15f;

            CJ = GetComponent<ConfigurableJoint>();
            CJ.yMotion = ConfigurableJointMotion.Limited;
            var cl = CJ.linearLimit;
            cl.limit = 0.5f;
            CJ.linearLimit = cl;
            var yd = CJ.yDrive;
            yd.positionDamper = 750f * damperSlider.Value;
            yd.positionSpring = 1000f;
            CJ.yDrive = yd;


        }

        public override void OnSimulateStart()
        {
            BulletCurrentNumber = BulletMaxNumber = (int)BulletNumberSlider.Value;
            Strength = StrengthSlider.Value * 120f;
            KnockBack = KnockBackSlider.Value * Strength * 0.75f;
            Rate = RateSlider.Value;

            var yd = CJ.yDrive;
            yd.positionDamper = 500f * damperSlider.Value * StrengthSlider.Value;
            yd.positionSpring = 3000f * StrengthSlider.Value;
            CJ.yDrive = yd;

            initBullet();
            initVFX();

            void initBullet()
            {
                BulletObject = new GameObject("Bullet");
                BulletObject.transform.localScale = transform.localScale * 0.05f;
                BulletObject.AddComponent<DestroyIfEditMode>();

                var mf = BulletObject.AddComponent<MeshFilter>();
                mf.mesh = ModResource.GetMesh("Bullet Mesh");

                var mr = BulletObject.AddComponent<MeshRenderer>();
                mr.material.mainTexture = ModResource.GetTexture("Bullet Texture");

                var rigidbody = BulletObject.AddComponent<Rigidbody>();
                rigidbody.mass = bulletMassSlider.Value;
                rigidbody.drag = bulletDragSlider.Value;
                rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

                var collider = BulletObject.AddComponent<BoxCollider>();
                
                var tsd = BulletObject.AddComponent<TimedSelfDestruct>();

                var bs = BulletObject.AddComponent<BulletScript>();
                bs.Setup(Strength, cetFactor, Direction);

                var tr = BulletObject.AddComponent<TrailRenderer>();
                tr.startWidth = BulletObject.transform.localScale.magnitude * trailWidthFactor;
                tr.endWidth = BulletObject.transform.localScale.magnitude * 0.5f * trailWidthFactor;
                tr.material = new Material(Shader.Find("Particles/Additive"));
                tr.material.SetColor("_TintColor", bulletColorSlider.Value);
                tr.time = Mathf.Clamp(trailTimeFactor, 0f, 1f);

                BulletObject.SetActive(false);
            }
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
            Reload();
            if (BulletCurrentNumber > 0)
            {
                if ((holdToggle.IsActive && (LaunchKey.IsHeld||LaunchKey.EmulationHeld())) || (!holdToggle.IsActive && LaunchKey.IsPressed))
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

        void fire()
        {

            if (!LaunchEnable && Time.timeScale != 0)
            {
                LaunchEnable = true;
                StartCoroutine(Launch(BulletParticleEffectEvent));
            }

            void BulletParticleEffectEvent()
            {
                var bullet = (GameObject)Instantiate(BulletObject, transform.TransformPoint(SpawnPoint + Direction), transform.rotation);
                bullet.SetActive(true);

                var bs = bullet.GetComponent<BulletScript>();
                bs.Fire( null, (value) =>
                 {
                     bs.gameObject.AddComponent<ExplodeScript>().Explodey(ExplodeScript.explosionType.Small, bullet.transform.position, bulletPowerSlider.Value, 3f);
                     bs.GetComponent<TimedSelfDestruct>().Begin(5f);
                 }
                 );
  

                if (StatMaster.isMP && Modding.Common.Player.GetAllPlayers().Count > 1)
                {
                    var message = FireMessage.CreateMessage(BlockBehaviour, Rigidbody.velocity, bs.Guid.ToString());
                    ModNetworking.SendToAll(message);
                }
       
                fireAudioSource.PlayOneShot(fireAudioSource.clip);
                fireAudioSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);

                EffectsObject.SetActive(true);
                EffectsObject.GetComponent<Reactivator>().Switch = true;
            }
        }
        internal override void Launch_Network(Vector3 velocity, Guid guid)
        {
            var bullet = (GameObject)Instantiate(BulletObject, transform.TransformPoint(SpawnPoint + Direction), transform.rotation);
            bullet.SetActive(true);

            var bs = bullet.GetComponent<BulletScript>();
            bs.Guid = guid;
            bs.Fire(null, (value) =>
            {
                bs.GetComponent<TimedSelfDestruct>().Begin(5f);
            }
            );

            fireAudioSource.PlayOneShot(fireAudioSource.clip);

            EffectsObject.SetActive(true);
            EffectsObject.GetComponent<Reactivator>().Switch = true;
        }

        public override void Reload(bool constraint = false)
        {
            if (Machine.InfiniteAmmo)
            {
                BulletCurrentNumber = BulletMaxNumber;
            }
        }    
    }
}
