using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;

namespace ModernFirearmKitMod
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

    ///// <summary>子弹基类</summary> 
    //public class BulletScript : MonoBehaviour
    //{

    //    /// <summary>威力</summary>
    //    public float Strength;

    //    /// <summary>推力</summary>
    //    public float Thrust = 0;

    //    /// <summary>子弹刚体</summary>
    //    public Rigidbody rigidbody;

    //    /// <summary>子弹种类</summary>
    //    public enum BulletKind
    //    {
    //        破坏弹 = 1,
    //        曳光弹 = 2,
    //        高爆弹 = 3,
    //        云爆弹 = 4,
    //        发烟弹 = 5,
    //        曳光高爆弹 = 6,
    //        曳光云爆弹 = 7,
    //        曳光发烟弹 = 8
    //    }

    //    public BulletKind bulletKind = BulletKind.破坏弹;


    //    /// <summary>子弹拖尾</summary>
    //    public TrailRenderer BulletTrail;

    //    /// <summary>拖尾长度</summary>
    //    public float TrailLength = 1;

    //    /// <summary>拖尾颜色</summary>
    //    public Color TrailColor = new Color(255f, 255f, 0f);

    //    /// <summary>碰撞开启时间(0.1s)</summary>
    //    public float CollisionEnableTime = 2;

    //    public bool LaunchEnabled { get; set; } = false;

    //    void Awake()
    //    {

    //        gameObject.AddComponent<DestroyIfEditMode>();
    //        //gameObject.AddComponent<TimedSelfDestruct>().lifeTime = 100f;

    //        rigidbody = GetComponent<Rigidbody>();
    //        rigidbody.drag = 0.2f;
    //        rigidbody.detectCollisions = false;
    //        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

    //        if (bulletKind == BulletKind.曳光弹 || bulletKind == BulletKind.曳光高爆弹 || bulletKind == BulletKind.曳光云爆弹 || bulletKind == BulletKind.曳光发烟弹)
    //        {
    //            BulletTrail = gameObject.AddComponent<TrailRenderer>();
    //            BulletTrail.endWidth = 0.1f;
    //            BulletTrail.startWidth = 0.5f;
    //            BulletTrail.time = TrailLength * 0.1f;
    //            BulletTrail.material.shader = Shader.Find("Particles/Additive");
    //            BulletTrail.material.SetColor("_TintColor", TrailColor);
    //            //BulletTrail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    //        }
    //    }

    //    void Update()
    //    {

    //        if (LaunchEnabled)
    //        {
    //            //rigidbody.velocity = transform.forward * 350 * Strength;

    //            LaunchEnabled = false;

    //            StartCoroutine(Launch());


    //        }

    //    }

    //    void OnCollisionEnter(Collision collision)
    //    {
    //        //StartCoroutine(Explodey());

    //        if (bulletKind == BulletKind.曳光弹)
    //            BulletTrail.enabled = false;


    //        //if (GetComponent<TimedSelfDestruct>() != null)
    //        //    gameObject.GetComponent<TimedSelfDestruct>().TimedDestroySelf(10f);


    //    }

    //    //public IEnumerator Explodey()
    //    //{
    //    //    if (Collisioned)
    //    //    {
    //    //        yield break;
    //    //    }

    //    //    yield return new WaitForFixedUpdate();

    //    //    Collisioned = true;

    //    //    //爆炸范围
    //    //    float radius = Strength;

    //    //    //爆炸位置
    //    //    Vector3 position_hit = transform.TransformDirection(new Vector3(-1f, rigidbody.centerOfMass.y, rigidbody.centerOfMass.z)) + transform.position;

    //    //    //爆炸类型 炸弹
    //    //    if (bulletKind == BulletKind.云爆弹 || bulletKind == BulletKind.曳光云爆弹)
    //    //    {
    //    //        GameObject explo = (GameObject)Instantiate(PrefabMaster.BlockPrefabs[23].gameObject, position_hit, transform.rotation);
    //    //        explo.transform.localScale = Vector3.one * 0.01f;
    //    //        ExplodeOnCollideBlock ac = explo.GetComponent<ExplodeOnCollideBlock>();
    //    //        ac.radius = 2 + radius;
    //    //        ac.power = 3000f * radius;
    //    //        ac.torquePower = 5000f * radius;
    //    //        ac.upPower = 0;
    //    //        ac.Explodey();
    //    //        explo.AddComponent<TimedSelfDestruct>();
    //    //    }
    //    //    else if (bulletKind == BulletKind.高爆弹 || bulletKind == BulletKind.曳光高爆弹)
    //    //    {
    //    //        GameObject explo = (GameObject)GameObject.Instantiate(PrefabMaster.BlockPrefabs[54].gameObject, position_hit, transform.rotation);
    //    //        explo.transform.localScale = Vector3.one * 0.01f;
    //    //        ControllableBomb ac = explo.GetComponent<ControllableBomb>();
    //    //        ac.radius = 2 + radius;
    //    //        ac.power = 3000f * radius;
    //    //        ac.randomDelay = 0.00001f;
    //    //        ac.upPower = 0f;
    //    //        ac.StartCoroutine_Auto(ac.Explode());
    //    //        explo.AddComponent<TimedSelfDestruct>();
    //    //    }
    //    //    else if (bulletKind == BulletKind.发烟弹 || bulletKind == BulletKind.曳光发烟弹)
    //    //    {
    //    //        GameObject explo = (GameObject)GameObject.Instantiate(PrefabMaster.BlockPrefabs[59].gameObject, position_hit, transform.rotation);
    //    //        explo.transform.localScale = Vector3.one * 0.01f;
    //    //        TimedRocket ac = explo.GetComponent<TimedRocket>();
    //    //        ac.SetSlip(Color.white);
    //    //        ac.radius = 2 + radius;
    //    //        ac.power = 3000f * radius;
    //    //        ac.randomDelay = 0.000001f;
    //    //        ac.upPower = 0;
    //    //        ac.StartCoroutine(ac.Explode(0.01f));
    //    //        explo.AddComponent<TimedSelfDestruct>();
    //    //    }
    //    //    else if (bulletKind == BulletKind.破坏弹 || bulletKind == BulletKind.曳光弹)
    //    //    {
    //    //        yield break;
    //    //    }
    //    //    foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>())
    //    //    {
    //    //        if (r.name == "Vis")
    //    //        {
    //    //            r.enabled = false;


    //    //        }
    //    //    }
    //    //    if (GetComponent<TrailRenderer>())
    //    //        BulletTrail.enabled = false;

    //    //    //transform.localScale = Vector3.zero;
    //    //    //rigidbody.isKinematic = true;
    //    //    //rigidbody.detectCollisions = false;
    //    //    //Destroy(gameObject.GetComponentInChildren<FireController>());
    //    //    //psp_fire.lifetime = 0;
    //    //    //ps_fire.Stop();
    //    //    //ps_smoke.Stop();


    //    //    //gameObject.AddComponent<TimedSelfDestruct>().lifeTime = psp_smoke.lifetime * 120;
    //    //}

    //    public IEnumerator Launch()
    //    {
    //        rigidbody.AddForce(transform.forward * 300 * Thrust, ForceMode.Impulse);
    //        yield return new WaitForSeconds(CollisionEnableTime * 0.1f * Time.timeScale);
    //        rigidbody.detectCollisions = true;
    //    }

    //    /// <summary>
    //    /// 获取子弹类型
    //    /// </summary>
    //    /// <param name="BulletKindNumber">子弹类型编号</param>
    //    /// <returns>返回BulletKind格式的子弹类型</returns>
    //    public static BulletKind GetBulletKind(int BulletKindNumber)
    //    {
    //        return (BulletKind)Enum.ToObject(typeof(BulletKind), BulletKindNumber);
    //    }

    //    /// <summary>
    //    /// 获取子弹类型
    //    /// </summary>
    //    /// <param name="Kind">子弹类型</param>
    //    /// <returns>返回int格式的子弹类型</returns>
    //    public static int GetBulletKind(BulletKind Kind)
    //    {
    //        return (int)Enum.Parse(typeof(BulletKind), Kind.ToString());
    //    }

    //    /// <summary>
    //    /// 获取子弹种类总数
    //    /// </summary>
    //    /// <returns>子弹种类数</returns>
    //    public static int GetBulletKindNumber()
    //    {
    //        return Enum.GetNames(typeof(BulletKind)).GetLength(0);
    //    }

    //    public class BulletBelt
    //    {
    //        public BulletScript bs;

    //        public int CurrentBelt = 0;

    //        public int BeltLength = 1;

    //        public List<int> KindGroup = new List<int>();


    //        public BulletBelt(GameObject bullet, float belt)
    //        {
    //            bs = bullet.GetComponent<BulletScript>();

    //            BeltLength = Mathf.Abs((int)belt).ToString().Length;

    //            for (int i = 0; i < BeltLength; i++)
    //            {
    //                KindGroup.Add(Mathf.Clamp(int.Parse(belt.ToString().Substring(i, 1)), 1, GetBulletKindNumber()));
    //            }
    //        }

    //        public void ChangedBulletKind()
    //        {
    //            bs.bulletKind = GetBulletKind(KindGroup[CurrentBelt++]);
    //            CurrentBelt = CurrentBelt >= BeltLength ? 0 : CurrentBelt;
    //        }
    //    }
    //}

    ///// <summary>子弹类</summary> 
    //public class Bullet
    //{
    //    public GameObject gameObject;

    //    /// <summary>威力</summary>
    //    public float Strength { get; set; }

    //    /// <summary>碰撞开启时间(0.1s)</summary>
    //    public float CollisionEnableTime;
    //    /// <summary>发射使能</summary>
    //    public bool LaunchEnabled { get; set; } = false;
    //    /// <summary>已经发射</summary>
    //    public bool Launched { get; private set; } = false;
    //    /// <summary>发射方向</summary>
    //    public Vector3 Direction;

    //    /// <summary>子弹刚体</summary>
    //    public Rigidbody rigidbody;
    //    /// <summary>碰撞事件</summary>
    //    public event Action OnCollisionEvent;

    //    public BulletScript Script;

    //    public Bullet(GameObject go, float strength, float collisionEnableTime, Vector3 direction, Action onCollision)
    //    {
    //        gameObject = go;
    //        Strength = strength;
    //        CollisionEnableTime = collisionEnableTime;
    //        Direction = direction;

    //        rigidbody = gameObject.GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();
    //        Script = gameObject.AddComponent<BulletScript>();
    //        Script.OnCollisionEvent += onCollision; 
    //    }
    //}


    /// <summary>子弹脚本</summary> 
    public class BulletScript : MonoBehaviour
    {
        public enum BulletKind
        {
            BaseBullet = 0,
            ThrustBullet = 1
        }

        //public BulletKind Kind { get; set; } = BulletScript.BulletKind.BaseBullet;
        public Guid Guid { get; } = Guid.NewGuid();

        /// <summary>推力</summary>
        public float Strength;

        /// <summary>碰撞开启时间(0.01s)</summary>
        public float ColliderEnableTime;
        /// <summary>已经发射</summary>
        public bool Fired { get { return isFired; } } 
        private bool isFired = false;
        public bool Collisioned { get; private set; }
        /// <summary>发射方向</summary>
        public Vector3 Direction;

        /// <summary>子弹刚体</summary>
        public Rigidbody rigidbody;
        /// <summary>碰撞事件</summary>
        private event Action<Collision> OnCollisionEvent;
        /// <summary>开火事件</summary>
        private event Action OnFireEvent;
        Collider collider;


        void Awake()
        {
            collider = GetComponent<Collider>();
            rigidbody = GetComponent<Rigidbody>();
        }
        void OnEnable()
        {
            isFired = false;
            Collisioned = false;
            collider.enabled = false;
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collider.enabled == true && !Collisioned)
            {
                Collisioned = true;
                OnCollisionEvent?.Invoke(collision);
            }
        }

        public BulletScript Setup(float strength, float collisionEnableTime, Vector3 direction, Action onFire=null,Action<Collision> onCollision = null)
        {
            Strength = strength;
            ColliderEnableTime = collisionEnableTime;
            Direction = direction;
            OnFireEvent += onFire;
            OnCollisionEvent += onCollision;

            return this;
        }
        public void Fire()
        {
            if (!isFired)
            {
                isFired = true;
                OnFireEvent?.Invoke();
                StartCoroutine(fire());
            }
        }
        private IEnumerator fire()
        {
            rigidbody.AddRelativeForce(Direction * Strength, ForceMode.Impulse);
            yield return new WaitForSeconds(ColliderEnableTime * 0.01f);
            collider.enabled = true;
        }
    }
}
