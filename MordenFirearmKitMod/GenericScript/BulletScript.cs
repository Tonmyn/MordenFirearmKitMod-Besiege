using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MordenFirearmKitMod
{
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

    /// <summary>子弹基类</summary> 
    public class BulletScript : MonoBehaviour
    {

        /// <summary>威力</summary>
        public float Strength;

        /// <summary>推力</summary>
        public float Thrust = 0;

        /// <summary>碰撞</summary>
        public bool Collisioned { get; private set; } = false;

        /// <summary>子弹刚体</summary>
        public Rigidbody rigidbody;

        /// <summary>子弹种类</summary>
        public enum BulletKind
        {
            破坏弹 = 0,
            高爆弹 = 1,
            云爆弹 = 2,
            发烟弹 = 3
        }

        public BulletKind bulletKind = 0;

        void Awake()
        {

            gameObject.AddComponent<Rigidbody>();
            gameObject.AddComponent<DestroyIfEditMode>();
            gameObject.AddComponent<TimedSelfDestruct>().lifeTime = 100f;

            rigidbody = GetComponent<Rigidbody>();
            rigidbody.drag = 0.2f;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            
        }

        void Start()
        {
            rigidbody.velocity = transform.forward * 300 * Strength;
        }

        void FixedUpdate()
        {
            if (!Collisioned)
            {
                rigidbody.AddForce(transform.forward * 300 * Thrust);
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            Collisioned = true;
            if (GetComponent<TimedSelfDestruct>() != null)
                gameObject.GetComponent<TimedSelfDestruct>().TimedDestroySelf(10f);

            
        }

        public IEnumerator Explodey()
        {
            if (Collisioned)
            {
                yield break;
            }

            yield return new WaitForFixedUpdate();

            Collisioned = true;

            //爆炸范围
            float radius = Strength;

            //爆炸位置
            Vector3 position_hit = transform.TransformDirection(new Vector3(-1f, rigidbody.centerOfMass.y, rigidbody.centerOfMass.z)) + transform.position;

            //爆炸类型 炸弹
            if (bulletKind == BulletKind.云爆弹)
            {
                GameObject explo = (GameObject)Instantiate(PrefabMaster.BlockPrefabs[23].gameObject, position_hit, transform.rotation);
                explo.transform.localScale = Vector3.one * 0.01f;
                ExplodeOnCollideBlock ac = explo.GetComponent<ExplodeOnCollideBlock>();
                ac.radius = 2 + radius;
                ac.power = 3000f * radius;
                ac.torquePower = 5000f * radius;
                ac.upPower = 0;
                ac.Explodey();
                explo.AddComponent<TimedSelfDestruct>();
            }
            else if (bulletKind == BulletKind.高爆弹)
            {
                GameObject explo = (GameObject)GameObject.Instantiate(PrefabMaster.BlockPrefabs[54].gameObject, position_hit, transform.rotation);
                explo.transform.localScale = Vector3.one * 0.01f;
                ControllableBomb ac = explo.GetComponent<ControllableBomb>();
                ac.radius = 2 + radius;
                ac.power = 3000f * radius;
                ac.randomDelay = 0.00001f;
                ac.upPower = 0f;
                ac.StartCoroutine_Auto(ac.Explode());
                explo.AddComponent<TimedSelfDestruct>();
            }
            else if (bulletKind == BulletKind.发烟弹)
            {
                GameObject explo = (GameObject)GameObject.Instantiate(PrefabMaster.BlockPrefabs[59].gameObject, position_hit, transform.rotation);
                explo.transform.localScale = Vector3.one * 0.01f;
                TimedRocket ac = explo.GetComponent<TimedRocket>();
                ac.SetSlip(Color.white);
                ac.radius = 2 + radius;
                ac.power = 3000f * radius;
                ac.randomDelay = 0.000001f;
                ac.upPower = 0;
                ac.StartCoroutine(ac.Explode(0.01f));
                explo.AddComponent<TimedSelfDestruct>();
            }

            foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>())
            {
                if (r.name == "Vis")
                {
                    r.enabled = false;


                }
            }

            //transform.localScale = Vector3.zero;
            //rigidbody.isKinematic = true;
            //rigidbody.detectCollisions = false;
            //Destroy(gameObject.GetComponentInChildren<FireController>());
            //psp_fire.lifetime = 0;
            //ps_fire.Stop();
            //ps_smoke.Stop();


            //gameObject.AddComponent<TimedSelfDestruct>().lifeTime = psp_smoke.lifetime * 120;
        }

    }
}
