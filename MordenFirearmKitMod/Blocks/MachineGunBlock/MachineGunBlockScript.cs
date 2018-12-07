using UnityEngine;
using System.Collections;
using Modding;
using System.Collections.Generic;

namespace ModernFirearmKitMod
{
    public class MachineGunBlockScript : LauncherBlockScript
    {

        public override float Rate { get; set; }
        public override float KnockBack { get; set; }
        public override int BulletCurrentNumber { get; set; }
        public override int BulletMaxNumber { get; set; }
        public override GameObject Bullet { get; set; }
        public override Vector3 SpawnPoint { get; set; }
        public override bool ShootEnable { get; set; }

        //加特林转速
        float RotationRate;

        //加特林转速限制
        float RotationRateLimit = 60;

        //加特林转动声音
        public AudioSource RotationAudioSource;

        //加特林转动音量
        public static float Volume = 5;

        ConfigurableJoint CJ;

        GameObject GunVis;

        public override void SafeAwake()
        {
            LaunchKey = AddKey("Fire", "Fire", KeyCode.C);

            KnockBack = 1f;
            SpawnPoint = new Vector3(0, 0, 3.5f);
            Bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            BulletCurrentNumber = BulletMaxNumber = 500;
            Rate = 0.2f;
            ShootEnable = false;


            RotationRate = 0f;

            RotationAudioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
            RotationAudioSource.clip = ModResource.GetAudioClip("MachineGun AudioClip");
            RotationAudioSource.loop = true;

            CJ = GetComponent<ConfigurableJoint>();

            List<MeshFilter> meshFilters = new List<MeshFilter>();
            GetComponentsInChildren(false, meshFilters);
            GunVis = meshFilters.Find(match => match.name == "Vis").gameObject;
        }

        public override void SimulateUpdateHost()
        {
            if (CJ == null)
            {
                return;
            }

            if (LaunchKey.IsDown)
            {
                RotationRate = Mathf.MoveTowards(RotationRate, RotationRateLimit, 20 * Time.timeScale * Time.deltaTime);
                if (RotationRate >= RotationRateLimit && !ShootEnable && Time.timeScale !=0)
                {
                    ShootEnable = true;
                    StartCoroutine(Launch());
                }           
            }
            else
            {
                ShootEnable = false;
                RotationRate = Mathf.MoveTowards(RotationRate, 0, Time.timeScale * Time.deltaTime * 10);
            }

            GunVis.transform.Rotate(new Vector3(0, RotationRate, 0) * Time.timeScale);

            if (RotationRate != 0)
            {
                RotationAudioSource.volume = RotationRate / (Vector3.Distance(transform.position, Camera.main.transform.position) * RotationRateLimit) * Volume;
                RotationAudioSource.pitch = RotationRate / RotationRateLimit;
                if (!RotationAudioSource.isPlaying) RotationAudioSource.Play();
            }
            else
            {
                RotationAudioSource.Stop();
            }
        }


     


    }
}





