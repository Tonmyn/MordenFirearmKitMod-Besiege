using System;
using System.Collections.Generic;
using spaar.ModLoader;
using TheGuysYouDespise;
using UnityEngine;
using System.Collections;

namespace MordenFirearmKitMod
{

    //发射器类
    public class LauncherScript : MonoBehaviour
    {


        //子弹
        //public Bullet bullet;

        //散布
        //public float Diffuse;

        //弹药量上限
        public int bulletLimit { get; set; }

        //实际弹药量
        public int bulletNumber { get; private set; }

        //射速
        public float FireRate;

        //后座力
        public float KnockBack = 1;

        //扳机
        public MKey Trigger;

        //质量
        //public float Mass = 0.5f;

        ///<summary>子弹组件</summary>
        public GameObject Bullet;

        //子弹网格
        //public Mesh bulletMesh;

        //发射时间间隔
        internal float timer;

        //允许发射
        public bool shootable = false;

        //随机延时
        //public float randomDelay = 0.1f;

        //枪口位置
        public Vector3 GunPoint;

        //枪的刚体组件
        public Rigidbody rigidbody;

        //枪的关节组件
        public ConfigurableJoint CJ;

        public GameObject GunVis;

        public virtual void Awake()
        {
            foreach (MeshFilter mf in GetComponentsInChildren<MeshFilter>())
            {
                
                if (mf.name == "Vis")
                {
                    GunVis = mf.gameObject;break;
                }
            }

            rigidbody = GetComponent<Rigidbody>();
            rigidbody.mass = 0.5f;

            CJ = GetComponent<ConfigurableJoint>();
        }


        public virtual void Start()
        {
            bulletNumber = bulletLimit;
        }

        public virtual void Update()
        {

            if (StatMaster.GodTools.InfiniteAmmoMode)
            {
                bulletNumber = bulletLimit;
            }

            if (Trigger.IsDown && bulletNumber > 0 && shootable)
            {

                if (timer >= FireRate && Time.timeScale != 0)
                {
                    timer = 0;
                    shoot();

                    return;
                }
                else
                {
                    timer += Time.deltaTime;
                }
            }
            
        }

        public virtual void shoot()
        {

            bulletNumber--;          

            rigidbody.AddForce(-transform.forward * KnockBack * 2000f);

            //Bullet.GetComponent<BulletScript>().Velocity = rigidbody.velocity * ;

            Instantiate(Bullet,transform.TransformPoint(GunPoint),transform.rotation);

        }

    }


    ////子弹类
    //public class BulletScript : MonoBehaviour
    //{

    //    #region 物理参数

    //    /// <summary>威力</summary>
    //    public float Force;

    //    //口径
    //    public float Caliber;

    //    //后坐力
    //    public float Recoil { get; private set; }

    //    //射程
    //    public float Distance { get; }

    //    /// <summary>动能</summary>
    //    public float KineticEnergy;

    //    //初速
    //    public float MuzzleVelocity { get; }

    //    ////阻力
    //    //public float Drag { get; }

    //    //质量
    //    public float Mass { get; }

    //    #endregion


    //    #region 属性变量

    //    //类型
    //    public BulletType bulletType;

    //    //子弹种类
    //    public enum BulletType { 高爆弹, 拽光弹, 穿甲弹 }

    //    //public GameObject bullet;

    //    public Rigidbody rigidbody;

    //    public Mesh mesh;

    //    public Texture texture;

    //    public Vector3 GunPoint;

    //    private MeshFilter mFilter;

    //    private MeshRenderer mRenderer;

    //    #endregion

    //    private void Awake()
    //    {
    //        rigidbody = gameObject.AddComponent<Rigidbody>();
    //        mFilter = gameObject.AddComponent<MeshFilter>();
    //        mRenderer = gameObject.AddComponent<MeshRenderer>();
    //        gameObject.AddComponent<DestroyIfEditMode>();

    //    }

    //    private void Start()
    //    {

    //        bullet_init();

    //    }

    //    private void Update()
    //    {
    //        rigidbody.AddRelativeForce(new Vector3(0,0,1) * 1000);
    //    }



    //    public void bullet_init()
    //    {

    //        mFilter.mesh = mesh;

    //        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

    //        GameObject collider = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    //        collider.transform.parent = gameObject.transform;
    //        collider.transform.position = gameObject.transform.position;
    //        collider.transform.localEulerAngles = new Vector3(90,0,0);
    //        transform.localScale = collider.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);


    //        //gameObject.transform.position = GunPoint;

    //    }

    //    public void bullet_init(Vector3 gunPoint,Vector3 rotation)
    //    {

    //        mFilter.mesh = mesh;

    //        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

    //        GameObject collider = GameObject.CreatePrimitive(PrimitiveType.Capsule);
    //        collider.transform.parent = transform;


    //        gameObject.transform.localScale = collider.transform.localScale = new Vector3(0.25f, 0.35f, 0.25f);
    //        gameObject.transform.position = gunPoint;
    //        gameObject.transform.Rotate(rotation);
    //    }


    //    public void bullet_Destroy()
    //    {
    //        Destroy(gameObject);
    //    }

    //    private void OnCollisionEnter(Collision collision)
    //    {

    //        if (collision.gameObject.name != "MachineGun")
    //        {
    //            //StartCoroutine(Rocket_Explodey(transform.position));
    //        }


    //    }

    //    //爆炸事件
    //    public IEnumerator Rocket_Explodey(Vector3 point)
    //    {

    //        yield return new WaitForFixedUpdate();

    //        //爆炸范围
    //        float radius = 5;

    //        //爆炸位置
    //        Vector3 position_hit = point;


    //        GameObject explo = (GameObject)GameObject.Instantiate(PrefabMaster.BlockPrefabs[54].gameObject, position_hit, transform.rotation);
    //        explo.transform.localScale = Vector3.one * 0.01f;
    //        ControllableBomb ac = explo.GetComponent<ControllableBomb>();
    //        ac.radius = 2 + radius;
    //        ac.power = 30f * radius;
    //        ac.randomDelay = 0.00001f;
    //        ac.upPower = 0f;
    //        ac.StartCoroutine_Auto(ac.Explode());
    //        explo.AddComponent<TimedSelfDestruct>();
    //        explo.transform.localScale = Vector3.one * 0.1f;
    //        Destroy(gameObject);


    //    }

    //    public float getKineticEnergy()
    //    {
    //        return 0.5f * rigidbody.mass * rigidbody.velocity.sqrMagnitude;
    //    }


    //    public float getBuzzleVelocity(float force)
    //    {
    //        return Mathf.Sqrt(2 * force / rigidbody.mass);
    //    }

    //    public float getMass(float caliber)
    //    {
    //        return 0.5f * Mathf.Sqrt(caliber);
    //    }



    //}


    public class BulletScript : MonoBehaviour
    {

        public float Strength;

        private void Start()
        {
            GetComponent<Rigidbody>().velocity =  transform.forward * 300 * Strength ;
        }

    }


    /// <summary>到时自毁脚本 </summary> 
    public class TimedSelfDestruct : MonoBehaviour
    {
        float timer = 0;
        public float lifetime = 300;

        void FixedUpdate()
        {
            ++timer;
            if (timer > lifetime)
            {
                Destroy(gameObject);
                if (this.GetComponent<TimedRocket>())
                {
                    Destroy(this.GetComponent<TimedRocket>());
                }
            }
        }
    }
}
