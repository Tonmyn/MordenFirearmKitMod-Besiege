using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MordenFirearmKitMod
{
    /// <summary>发射器基类</summary> 
    public class LauncherScript : MonoBehaviour
    {

        //子弹
        //public Bullet bullet;

        //散布
        //public float Diffuse;

        ///<summary>弹药量上限</summary>
        public int bulletLimit { get; set; }

        ///<summary>实际弹药量</summary>
        public int bulletNumber { get; private set; }

        ///<summary>射速</summary>
        public float FireRate;

        ///<summary>后座力</summary>
        public float KnockBack = 1;



        //发射时间间隔
        internal float Interval;

        //允许发射
        public bool shootable = false;

        ///<summary>扳机</summary>
        public MKey Trigger;

        ///<summary>枪口位置</summary>
        public Vector3 GunPoint = new Vector3(0, 0, 3.5f);

        ///<summary>子弹组件</summary>
        public GameObject Bullet;

        ///<summary>枪的刚体组件</summary>
        public Rigidbody rigidbody;

        ///<summary>枪的关节组件</summary>
        public ConfigurableJoint CJ;

        //通用组件
        public GameObject GenericObject;

        //亮光组件
        public Light gunLight;

        //音频组件
        public AudioSource gunAudio;

        //粒子组件
        public ParticleSystem[] gunParticles = new ParticleSystem[3];

        ////音频剪辑数据
        //internal AudioClip gunAudioClip;

        //贴图数据
        internal Texture gunParticleTexture;

        internal Mesh gunParticleMesh;

        internal Texture gunFlame;

        public virtual void Awake()
        {
            GenericObject = new GameObject();
            GenericObject.transform.parent = transform;
            GenericObject.transform.localPosition = GunPoint;
            GenericObject.transform.localEulerAngles = Vector3.zero;

            gunLight = GenericObject.AddComponent<Light>();
            gunAudio = GenericObject.AddComponent<AudioSource>();
            gunParticles[0] = new GameObject().AddComponent<ParticleSystem>();
            gunParticles[1] = new GameObject().AddComponent<ParticleSystem>();
            gunParticles[0].gameObject.transform.SetParent(GenericObject.transform);
            gunParticles[1].gameObject.transform.SetParent(GenericObject.transform);
            gunParticles[0].gameObject.transform.position = GenericObject.transform.position;
            gunParticles[1].gameObject.transform.position = GenericObject.transform.TransformPoint(GenericObject.transform.localPosition + new Vector3(0, -0.1f, -2.9f));

            rigidbody = GetComponent<Rigidbody>();
            rigidbody.mass = 0.5f;

            CJ = GetComponent<ConfigurableJoint>();

        }


        public virtual void Start()
        {
            bulletNumber = bulletLimit;

            gunLight.range = 6;
            gunLight.type = LightType.Point;
            gunLight.intensity = 2f;
            gunLight.bounceIntensity = 1f;
            gunLight.shadows = LightShadows.None;
            gunLight.spotAngle = 135;
            gunLight.color = new Color32(250, 100, 0, 255);
            gunLight.renderMode = LightRenderMode.Auto;    
            gunLight.enabled = false;

            //gunAudio.clip = gunAudioClip;
            gunAudio.playOnAwake = false;
            gunAudio.loop = false;
            gunAudio.volume = 0.1f;
            gunAudio.enabled = true;

            //粒子[0]为枪口光效1
            //gunParticles[0].playOnAwake = false;
            gunParticles[0].Stop();
            gunParticles[0].loop = false;
            gunParticles[0].startSize = 0.5f;
            gunParticles[0].startSpeed = 0;
            gunParticles[0].maxParticles = 100;
            gunParticles[0].startLifetime = 0.05f;
            gunParticles[0].startColor = new Color32(250, 100, 0, 255);
            gunParticles[0].scalingMode = ParticleSystemScalingMode.Shape;


            ParticleSystem.EmissionModule em = gunParticles[0].emission;
            em.rate = new ParticleSystem.MinMaxCurve { constant = 0, mode = ParticleSystemCurveMode.Constant };
            em.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0, 15,15) });
            em.enabled = true;

            ParticleSystem.ShapeModule sm = gunParticles[0].shape;
            sm.meshShapeType = ParticleSystemMeshShapeType.Edge;
            sm.shapeType = ParticleSystemShapeType.Mesh;
            sm.mesh = gunParticleMesh;
            sm.enabled = true;

            ParticleSystem.ColorOverLifetimeModule colm = gunParticles[0].colorOverLifetime;
            colm.color = new Gradient()
            {
                alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(0,0), new GradientAlphaKey(255,0.2f),new GradientAlphaKey(255,0.8f),new GradientAlphaKey(0,1) },

                //colorKeys = new GradientColorKey[] { new GradientColorKey(Color.white,0),new GradientColorKey(Color.white,1) }
            };
            colm.enabled = false;

            ParticleSystem.SizeBySpeedModule sbsm = gunParticles[0].sizeBySpeed;
            sbsm.range = new Vector2(0, 1);
            sbsm.size = new ParticleSystem.MinMaxCurve(1, 0);
            sbsm.enabled = false;

            ParticleSystemRenderer psr = gunParticles[0].GetComponent<ParticleSystemRenderer>();
            psr.renderMode = ParticleSystemRenderMode.Billboard;
            psr.normalDirection = 1;
            psr.material = new Material(Shader.Find("Particles/Additive"));
            psr.material.mainTexture = gunParticleTexture;
            //psr.material.color = new Color(70, 35, 20, 255);        
            psr.sortMode = ParticleSystemSortMode.Distance;
            psr.sortingFudge = 0;
            psr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            psr.receiveShadows = true;
            psr.minParticleSize = 0;
            psr.maxParticleSize = 0.5f;
            //psr.alignment = ParticleSystemRenderSpace.View;

            gunParticles[1].Stop();
            gunParticles[1].loop = false;
            gunParticles[1].startSize = 1f;
            gunParticles[1].startSpeed = -0.1f;
            gunParticles[1].maxParticles = 100;
            gunParticles[1].startLifetime = 0.5f;
            gunParticles[1].startColor = new Color32(250, 100, 0, 255);
            gunParticles[1].scalingMode = ParticleSystemScalingMode.Shape;

            em = gunParticles[1].emission;
            em.rate = new ParticleSystem.MinMaxCurve { constant = 0, mode = ParticleSystemCurveMode.Constant };
            em.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0, 1) });
            em.enabled = true;

            sm = gunParticles[1].shape;
            sm.shapeType = ParticleSystemShapeType.Cone;
            sm.radius = 0.01f;
            sm.angle = 5f;
            sm.length = 5;
            sm.randomDirection = false;
            sm.enabled = true;

            ParticleSystem.TextureSheetAnimationModule tsam = gunParticles[1].textureSheetAnimation;
            tsam.numTilesX = tsam.numTilesY = 4;
            tsam.animation = ParticleSystemAnimationType.WholeSheet;
            tsam.frameOverTime = new ParticleSystem.MinMaxCurve() {  curve = new AnimationCurve {  keys = new Keyframe[] {new Keyframe(0,0),new Keyframe(0.15f,0.5f),new Keyframe(1,1) } } };
            tsam.cycleCount = 1;
            tsam.enabled = true;

            psr = gunParticles[1].GetComponent<ParticleSystemRenderer>();
            psr.renderMode = ParticleSystemRenderMode.Stretch;
            psr.lengthScale = 2.5f;
            psr.normalDirection = 1;
            psr.material = new Material(Shader.Find("Particles/Additive"));
            psr.material.mainTexture = gunFlame;
            psr.material.mainTextureOffset = new Vector2(4, 0);
            //psr.sortMode = ParticleSystemSortMode.Distance;
            //psr.sortingFudge = 0;
            //psr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            //psr.receiveShadows = true;
            //psr.minParticleSize = 0;
            //psr.maxParticleSize = 0.5f;
            //psr.alignment = ParticleSystemRenderSpace.View;

        }

        public virtual void Update()
        {

            if (StatMaster.GodTools.InfiniteAmmoMode)
            {
                bulletNumber = bulletLimit;
            }

            if (Trigger.IsDown && bulletNumber > 0 && shootable)
            {
                shootable = false;
                if (Interval >= FireRate && Time.timeScale != 0)
                {
                    Interval = 0;
                    StartCoroutine(shoot());

                    return;
                }
                else
                {
                    Interval += Time.deltaTime * Time.timeScale;
                }
            }

        }

        public virtual IEnumerator shoot()
        {

            bulletNumber--;

            rigidbody.AddForce(-transform.forward * KnockBack * 4000f);

            GameObject bullet = (GameObject)Instantiate(Bullet, transform.TransformPoint(GunPoint), transform.rotation);

            bullet.transform.SetParent(Machine.Active().SimulationMachine);

            bullet.SetActive(true);

            gunLight.enabled = true;
            gunAudio.PlayOneShot(gunAudio.clip);
            gunParticles[0].Play();
            gunParticles[1].Emit(1);

            yield return 0;

            gunLight.enabled = false;
            //gunParticles.Stop();


        }

    }
}

