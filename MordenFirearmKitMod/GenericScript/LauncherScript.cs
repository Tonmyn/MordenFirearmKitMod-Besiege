//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace MordenFirearmKitMod
//{
//    /// <summary>发射器基类</summary> 
//    public class LauncherScript : MonoBehaviour
//    {

//        //子弹
//        //public Bullet bullet;

//        //散布
//        //public float Diffuse;

//        ///<summary>弹药量上限</summary>
//        public int bulletLimit { get; set; }

//        ///<summary>实际弹药量</summary>
//        public int bulletNumber { get; private set; }

//        ///<summary>射速</summary>
//        public float FireRate;

//        ///<summary>后座力</summary>
//        public float KnockBack = 1;

//        /// <summary>弹链</summary>
//        public float Belt = 0;

//        //发射时间间隔
//        internal float Interval;

//        //允许发射
//        public bool shootable = false;

//        ///<summary>扳机</summary>
//        public MKey Trigger;

//        ///<summary>枪口位置</summary>
//        public Vector3 GunPoint = new Vector3(0, 0, 3.5f);

//        ///<summary>子弹组件</summary>
//        public GameObject Bullet;

//        ///<summary>子弹弹链组件</summary>
//        public BulletScript.BulletBelt BulletBelt;

//        ///<summary>枪的刚体组件</summary>
//        public Rigidbody rigidbody;

//        ///<summary>枪的关节组件</summary>
//        public ConfigurableJoint CJ;

//        //通用组件
//        public GameObject GenericObject;

//        //亮光组件
//        public Light gunLight;

//        //音频组件
//        public AudioSource gunAudio;

//        public MuzzleFlash gunMuzzle;


//        public virtual void Awake()
//        {
//            GenericObject = new GameObject("GenericObject");
//            GenericObject.transform.parent = transform;
//            GenericObject.transform.localPosition = GunPoint;          


//            gunLight = GenericObject.AddComponent<Light>();
//            gunAudio = GenericObject.AddComponent<AudioSource>();
//            gunMuzzle = GenericObject.AddComponent<MuzzleFlash>();
//            gunMuzzle.transform.localEulerAngles = new Vector3(180, 180, 0);
//            gunMuzzle.GunPoint = GunPoint;

//            rigidbody = GetComponent<Rigidbody>();
//            rigidbody.mass = 0.5f;

//            CJ = GetComponent<ConfigurableJoint>();

//        }


//        public virtual void Start()
//        {
//            bulletNumber = bulletLimit;

//            gunLight.range = 6;
//            gunLight.type = LightType.Point;
//            gunLight.intensity = 2f;
//            gunLight.bounceIntensity = 1f;
//            gunLight.shadows = LightShadows.None;
//            gunLight.spotAngle = 135;
//            gunLight.color = new Color32(250, 100, 0, 255);
//            gunLight.renderMode = LightRenderMode.Auto;    
//            gunLight.enabled = false;

//            //gunAudio.clip = gunAudioClip;
//            gunAudio.playOnAwake = false;
//            gunAudio.loop = false;
//            gunAudio.volume = 0.1f;
//            gunAudio.enabled = true;

//            BulletBelt = new BulletScript.BulletBelt(Bullet, Belt);
//        }

       

//        public virtual void Update()
//        {

//            if (StatMaster.GodTools.InfiniteAmmoMode)
//            {
//                bulletNumber = bulletLimit;
//            }

//            //if (Trigger.IsDown && bulletNumber > 0 && shootable)
//            //{
//            //    shootable = false;
//            //    if (Interval >= FireRate && Time.timeScale != 0)
//            //    {
//            //        Interval = 0;
//            //        StartCoroutine(shoot());

//            //        return;
//            //    }
//            //    else
//            //    {
//            //        Interval += Time.deltaTime * Time.timeScale;
//            //    }
//            //}

//            if (shootable)
//            {
//                shootable = false;
//                StartCoroutine(shoot());
//            }
            
//        }

//        public virtual IEnumerator shoot()
//        {

//            bulletNumber--;

//            BulletBelt.ChangedBulletKind();

//            rigidbody.AddForce(-transform.forward * KnockBack * 4000f);

//            GameObject bullet = (GameObject)Instantiate(Bullet, transform.TransformPoint(GunPoint), transform.rotation);

//            bullet.transform.SetParent(Machine.Active().SimulationMachine);

//            bullet.SetActive(true);

//            gunLight.enabled = true;
//            gunAudio.PlayOneShot(gunAudio.clip);
//            gunMuzzle.Play();

//            yield return 0;

//            gunLight.enabled = false;



//        }

//    }
//}

