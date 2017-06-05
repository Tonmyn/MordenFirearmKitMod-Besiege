using System;
using System.Collections.Generic;
using spaar.ModLoader;
using TheGuysYouDespise;
using UnityEngine;
using System.Collections;
using System.ComponentModel;

namespace MordenFirearmKitMod
{

    partial class MordenFirearmKitMod
    {

        //声明火箭弹模块
        public Block RocketBlock = new Block()
        #region 火箭弹模块 基本属性  

           //模块 ID
           .ID(650)

           //模块 名称
           .BlockName("Rocket Block")

           //模块 模型信息
           .Obj(new System.Collections.Generic.List<Obj>
                   { new Obj(
                                "/MordenFirearmKitMod/Rocket.obj",
                                "/MordenFirearmKitMod/Rocket.png",
                                new VisualOffset(
                                                    new Vector3(    1f,    1f,    1f),
                                                    new Vector3( 1.25f,    0f,  0.3f),
                                                    new Vector3(   45f,    0f,  180f)
                                                 )
                              )
                     }
                 )

           //模块 图标
           .IconOffset(new Icon(
                                   1.3f,
                                   new Vector3(-0.25f, -0.25f, 0f),
                                   new Vector3(15f, -15f, 45f)
                                )
                       )

           //模块 组件
           .Components(new Type[] { typeof(RocketBlockScript), })

           //模块 设置模块属性
           .Properties(
                        new BlockProperties()
                       .SearchKeywords(new string[] { "Rocket", "火箭" })
                       .Burnable(0.1f)
                       )

           //模块 质量
           .Mass(0.5f)

            //模块 是否显示碰撞器
#if DEBUG
            .ShowCollider(true)
#endif
           //模块 碰撞器
           .CompoundCollider(new System.Collections.Generic.List<ColliderComposite>
                                {
                                    ColliderComposite.Capsule(
                                                                0.1f,                              //胶囊半径
                                                                2.65f,                             //胶囊高度
                                                                Direction.X,                       //胶囊方向
                                                                new Vector3(-0.125f,  0f,  0.3f),  //胶囊位置
                                                                Vector3.zero                       //胶囊旋转
                                                             )
                                }
                             )

           //模块 载入资源
           .NeededResources(new System.Collections.Generic.List<NeededResource>
                               {
                                    new NeededResource(ResourceType.Texture,"/MordenFirearmKitMod/RocketFire.png"),
                                    new NeededResource(ResourceType.Texture,"/MordenFirearmKitMod/RocketSmoke.png")
                                })

           //模块 连接点
           .AddingPoints(new System.Collections.Generic.List<AddingPoint>
                            {

                                new BasePoint(false,true) //底部连接点。第一个是指你能不能将其他模块安在该模块底部。第二个是指这个点是否是在开局时粘连其他链接点
                                                .Motionable(false,false,false) //底点在X，Y，Z轴上是否是能够活动的。
                                                .SetStickyRadius(0.25f) //粘连距离

               /* ,
               new AddingPoint(
                                   new Vector3(  0f,  0f,  0f), //位置
                                   new Vector3(-90f,  0f,  0f), //旋转
                                   false                       //这个点是否是在开局时粘连其他链接点
                               ).SetStickyRadius(0.15f)      //粘连距离
                   */
                            });

        #endregion;

        //声明火箭弹巢模块
        public Block RocketPodBlock = new Block()
        #region 火箭巢模块 基本属性  

            //模块 ID
            .ID(651)

            //模块 名称
            .BlockName("Rocket Pod Block")

            //模块 模型信息
            .Obj(new System.Collections.Generic.List<Obj>
                    { new Obj(
                                "/MordenFirearmKitMod/RocketPod.obj",
                                "/MordenFirearmKitMod/RocketPod.png",
                                new VisualOffset(
                                                    new Vector3(  0.5f,  0.5f,    1f),
                                                    new Vector3(    0f,    0f, 0.55f),
                                                    new Vector3(   90f,   90f,    0f)
                                                 )
                              )
                      }
                  )

            //模块 图标
            .IconOffset(new Icon(
                                    new Vector3(0.5f, 0.5f, 1f),
                                    new Vector3(-0f, -0f, 0f),
                                    new Vector3(-7.5f, 30f, 15f)
                                 )
                        )

            //模块 组件
            .Components(new Type[] { typeof(RocketPodBlockScript), })

            //模块 设置模块属性
            .Properties(
                         new BlockProperties()
                        .SearchKeywords(new string[] { "RocketPod", "火箭巢" })
                        )

            //模块 质量
            .Mass(0.5f)


#if DEBUG
            //模块 是否显示碰撞器
            .ShowCollider(true)
#endif
            //模块 碰撞器
            .CompoundCollider(new System.Collections.Generic.List<ColliderComposite>
                                 {
                                    ColliderComposite.Capsule(
                                                                0.475f,                            //胶囊半径
                                                                3f,                                //胶囊高度
                                                                Direction.X,                       //胶囊方向
                                                                new Vector3(   0f,  0f, 0.55f),    //胶囊位置
                                                                Vector3.zero                       //胶囊旋转
                                                             )

                                 }
                              )

            //模块 连接点
            .AddingPoints(new System.Collections.Generic.List<AddingPoint>
                             {

                                new BasePoint(false,true) //底部连接点。第一个是指你能不能将其他模块安在该模块底部。第二个是指这个点是否是在开局时粘连其他链接点
                                                .Motionable(false,false,false) //底点在X，Y，Z轴上是否是能够活动的。
                                                .SetStickyRadius(0.5f) //粘连距离
                                    
                                /* ,
                                new AddingPoint(
                                                    new Vector3(  0f,  0f,  0f), //位置
                                                    new Vector3(-90f,  0f,  0f), //旋转
                                                    false                       //这个点是否是在开局时粘连其他链接点
                                                ).SetStickyRadius(0.15f)      //粘连距离
                                    */
                             });

        #endregion;

    }

    //火箭弹 模块脚本
    class RocketBlockScript : BlockScript
    {

        #region 功能变量 声明

        //声明 开关 基本功能
        protected MToggle Function_toggle;

        //声明 按键 发射火箭
        internal MKey key;

        //声明 菜单 爆炸类型
        protected MMenu explosiontype_menu;

        //声明 滑条 延时点火
        protected MSlider delay_slider;

        //声明 滑条 燃烧时间
        protected MSlider time_slider;

        //声明 滑条 爆炸威力
        protected MSlider power_slider;

        //声明 滑条 推力大小
        protected MSlider thrust_slider;

        //声明 滑条 阻力大小
        protected MSlider drag_slider;

        //声明 滑条 碰撞开启时间
        protected MSlider timeopen_slider;

        //声明 爆炸类型
        public int explosiontype = (int)ExplosionType.炸弹;

        //声明 延时点火
        public float delay = 0f;

        //声明 燃烧时间
        public float time = 3f;

        //声明 爆炸威力
        public float power = 5f;

        //声明 推力大小
        public float thrust = 4f;

        //声明 阻力大小
        public float drag = 0.5f;

        //声明 碰撞开启时间
        public float timeopen = 2f;

        //声明 碰撞开启
        public bool canBeCollision = false;

        //声明 延时完毕
        public bool delayed = false;

        //声明 燃烧完毕
        public bool fired = false;

        //声明 是否发射
        public bool launched = false;

        //爆炸类型
        public enum ExplosionType
        {
            炸弹 = 0,
            手雷 = 1,
            烟花 = 2
        }

        #endregion

        #region 尾焰变量 声明

        //声明 尾焰粒子组件
        protected GameObject particle_fire = new GameObject();

        //声明 粒子系统
        protected ParticleSystem ps_fire;

        //声明 粒子渲染器
        protected ParticleSystemRenderer psr_fire;

        //声明 开关 尾焰效果
        protected MToggle toggle_fire;




        #endregion

        #region 尾烟变量 声明

        //声明 尾烟粒子组件
        protected GameObject particle_smoke = new GameObject();

        //声明 粒子系统
        protected ParticleSystem ps_smoke;

        //声明 粒子渲染器
        protected ParticleSystemRenderer psr_smoke;

        #endregion

        #region 内部变量 声明

        //声明 重心初始位置
        internal Vector3 com;





        internal Vector3 pos_thrust;

        internal Vector3 pos_drag;

        #endregion

        public override void SafeAwake()
        {
            base.SafeAwake();

            //添加 按键 参数
            key = AddKey("发射", "ROCKET", KeyCode.L);

            //添加 菜单 参数
            explosiontype_menu = AddMenu("ExplosionType", explosiontype, new System.Collections.Generic.List<string> { "炸弹", "手雷", "烟花" });

            //添加 滑条 参数
            delay_slider = AddSlider("延迟发射 0.1s", "DELAY", delay, 0f, 10f);
            time_slider = AddSlider("燃烧时间 1s", "TIME", time, 1f, 10f);
            power_slider = AddSlider("爆炸威力", "POWER", power, 3f, 8f);
            thrust_slider = AddSlider("推力大小", "THRUST", thrust, 3f, 10f);
            drag_slider = AddSlider("阻力大小", "DRAG", drag, 0.2f, 3f);
            timeopen_slider = AddSlider("碰撞开启 0.05s", "TIMEOPEN", timeopen, 1f, 5f);

            //委托 菜单改变 事件
            explosiontype_menu.ValueChanged += new ValueHandler(explosiontype_valueChanged);

            //委托 滑条改变 事件
            delay_slider.ValueChanged += new ValueChangeHandler(delay_valueChanged);
            time_slider.ValueChanged += new ValueChangeHandler(time_valueChanged);
            power_slider.ValueChanged += new ValueChangeHandler(power_valueChanged);
            thrust_slider.ValueChanged += new ValueChangeHandler(thrust_valueChanged);
            drag_slider.ValueChanged += new ValueChangeHandler(drag_valueChanged);
            timeopen_slider.ValueChanged += new ValueChangeHandler(timeopen_valueChanged);

        }

        //改变 爆炸类型 事件
        protected void explosiontype_valueChanged(int value)
        {
            explosiontype = value;
        }

        //改变 延迟发射 事件
        protected void delay_valueChanged(float value)
        {
            delay = value;
        }

        //改变 燃烧时间 事件
        protected void time_valueChanged(float value)
        {
            time = value;
        }

        //改变 爆炸威力 事件
        protected void power_valueChanged(float value)
        {
            power = value;
        }

        //改变 推力大小 事件
        protected void thrust_valueChanged(float value)
        {
            thrust = value;
        }

        //改变 阻力大小 事件
        protected void drag_valueChanged(float value)
        {
            drag = value;
        }

        //改变 碰撞开启时间 事件
        protected void timeopen_valueChanged(float value)
        {
            timeopen = value;
        }

        protected virtual System.Collections.IEnumerator UpdateMapper()
        {
            if (BlockMapper.CurrentInstance == null)
                yield break;
            while (Input.GetMouseButton(0))
                yield return null;
            BlockMapper.CurrentInstance.Copy();
            BlockMapper.CurrentInstance.Paste();
            yield break;
        }

        public override void OnSave(XDataHolder stream)
        {
            base.OnSave(stream);
            SaveMapperValues(stream);
        }

        public override void OnLoad(XDataHolder stream)
        {
            base.OnLoad(stream);
            LoadMapperValues(stream);
        }


#if DEBUG

        GameObject mark;


        public void create()
        {
            mark = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Destroy(mark.GetComponent<SphereCollider>());
        }

        public void destroy()
        {
            Destroy(mark);
        }

        public void move()
        {
            mark.transform.position = transform.TransformDirection(rigidbody.centerOfMass) + transform.position;
        }
#endif

        protected override void BlockPlaced()
        {
            base.BlockPlaced();
            com = rigidbody.centerOfMass;

        }

        protected override void BuildingUpdate()
        {
            base.BuildingUpdate();
            rigidbody.centerOfMass = com + new Vector3(0, 0, 0.25f * transform.localScale.z);
        }



        protected override void OnSimulateStart()
        {
            base.OnSimulateStart();
#if DEBUG
            //create();
#endif
            //初始化粒子系统
            //particlesystem_init(new Vector3(1.25f, 0, 0.3f), new Quaternion(90, 0, 90, 0), 5f);
            fire_ps_init();
            Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        }

        protected override void OnSimulateUpdate()
        {
            base.OnSimulateUpdate();
#if DEBUG
            //move();
#endif
            if (key.IsDown && !launched)
            {
                Rocket_LaunchPlan();
            }

            if (delayed && !fired)
            {
                ps_fire.Emit(2);
            }
        }

        protected override void OnSimulateExit()
        {
            base.OnSimulateExit();

#if DEBUG
            //destroy();
#endif

        }



        protected override void OnSimulateFixedStart()
        {
            base.OnSimulateFixedStart();

        }

        protected override void OnSimulateFixedUpdate()
        {
            base.OnSimulateFixedUpdate();


            if (launched && delayed)
            {
                Rocket_Launch();

            }

        }



        protected override void StartedBurning()
        {
            base.StartedBurning();
            //Rocket_Explodey();
            StartCoroutine(Rocket_Explodey());
        }

        protected override void OnSimulateCollisionEnter(Collision collision)
        {
            base.OnSimulateCollisionEnter(collision);

            if (launched && canBeCollision)
            {

                //Rocket_Explodey();
                StartCoroutine(Rocket_Explodey());
            }

        }


        //发射准备
        public void Rocket_LaunchPlan()
        {
            //发射许可
            launched = true;

            //计时协同函数
            StartCoroutine(Timer(timeopen, time, delay));

            //阻力角阻力设为0和3
            rigidbody.drag = 0f;
            rigidbody.angularDrag = 3f;

            //推力位置
            pos_thrust = transform.TransformDirection(new Vector3(1f, rigidbody.centerOfMass.y, rigidbody.centerOfMass.z)) + transform.position;

            //阻力位置
            pos_drag = transform.TransformDirection(new Vector3(0.5f, rigidbody.centerOfMass.y, rigidbody.centerOfMass.z)) + transform.position;
        }

        //发射函数
        public void Rocket_Launch()
        {



            //推力
            if (!fired)
            {
                Vector3 force_thrust = -transform.right * thrust;
                rigidbody.AddForceAtPosition(force_thrust, pos_thrust, ForceMode.VelocityChange);
            }

            //阻力
            Vector3 force_drag = transform.TransformDirection(Vector3.Scale(this.transform.InverseTransformDirection(-rigidbody.velocity), new Vector3(0, 1, 1))) * Mathf.Clamp(rigidbody.velocity.sqrMagnitude, 0, drag);
            rigidbody.AddForceAtPosition(force_drag, pos_drag);

        }

        //计时函数
        protected IEnumerator Timer(float topen, float t, float d)
        {

            //延时发射
            while (d-- > 0 && !delayed)
            {
                yield return new WaitForSeconds(0.1f);
                //time--;
            }
            if (d <= 0)
            {
                if (GetComponent<ConfigurableJoint>())
                {
                    //销毁连接点
                    Destroy(GetComponent<ConfigurableJoint>());
                }
                yield return new WaitForSeconds(0.01f);
                delayed = true;
            }

            //碰撞开启
            while (topen-- > 0 && !canBeCollision && delayed)
            {
                yield return new WaitForSeconds(0.05f);
                //time--;
            }
            if (topen <= 0)
            {
                canBeCollision = true;
            }

            //燃烧时间
            while (t-- > 0 && !fired && delayed)
            {
                yield return new WaitForSeconds(1f);
                //time--;
            }
            if (t <= 0)
            {
                fired = true;
            }

        }

        //爆炸事件
        public IEnumerator Rocket_Explodey()
        {

            yield return new WaitForFixedUpdate();

            //爆炸范围
            float radius = power;

            //爆炸位置
            Vector3 position_hit = transform.TransformDirection(new Vector3(-1f, rigidbody.centerOfMass.y, rigidbody.centerOfMass.z)) + transform.position;

            //爆炸类型 炸弹
            if (explosiontype == (int)ExplosionType.炸弹)
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
            else if (explosiontype == (int)ExplosionType.手雷)
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
            else if (explosiontype == (int)ExplosionType.烟花)
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

            Destroy(gameObject);


        }

        ////粒子系统初始化
        //protected void particlesystem_init(Vector3 position, Quaternion rotation, float lifetime, [DefaultValue("Local")] ParticleSystemSimulationSpace space)
        //{

        //    ps = particle.AddComponent<ParticleSystem>();
        //    ps.Stop(true);
        //    particle.transform.parent = transform;
        //    particle.transform.localPosition = position;
        //    particle.transform.localRotation = rotation;
        //    ps.startLifetime = lifetime;
        //    ps.time = 0.1f;

        //    ParticleSystem.ShapeModule sm = ps.shape;
        //    sm.shapeType = ParticleSystemShapeType.Cone;
        //    sm.radius = 0f;
        //    sm.angle = 3;
        //    sm.randomDirection = false;
        //    sm.enabled = true;

        //    ParticleSystem.SizeOverLifetimeModule sl = ps.sizeOverLifetime;
        //    float size = (transform.localScale.y + transform.localScale.z) / 2;
        //    sl.size = new ParticleSystem.MinMaxCurve(size, AnimationCurve.Linear(0f, 1f, lifetime, lifetime));
        //    sl.enabled = true;

        //    ParticleSystem.ColorOverLifetimeModule colm = ps.colorOverLifetime;
        //    colm.color = new Gradient() { alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(0.5f, 0), new GradientAlphaKey(0.8f, 0.2f), new GradientAlphaKey(1f, lifetime) }, colorKeys = new GradientColorKey[] { new GradientColorKey(Color.blue, 0), new GradientColorKey(ps.startColor, 0.1f), new GradientColorKey(Color.gray, 0.11f) } };
        //    colm.enabled = true;

        //    psr = particle.GetComponent<ParticleSystemRenderer>();
        //    psr.sharedMaterial = new Material(Shader.Find("Particles/Additive"));
        //    psr.sharedMaterial.mainTexture = (resources["/RocketBlockMod/Rocket.png"].texture);
        //    psr.sharedMaterial.color = new Color(0, 0, 0);


        //}

        ////粒子系统初始化
        //protected void particlesystem_init(Vector3 position, Quaternion rotation, float lifetime)
        //{
        //    ps = particle.AddComponent<ParticleSystem>();
        //    ps.Stop(true);
        //    particle.transform.parent = transform;
        //    particle.transform.localPosition = position;
        //    particle.transform.localRotation = rotation;
        //    // ParticleSystemCurveMode.TwoConstants
        //    ps.loop = true;
        //    ps.startLifetime = lifetime;
        //    ps.startColor = Color.white;
        //    ps.simulationSpace = ParticleSystemSimulationSpace.World;
        //    ps.maxParticles = 10240;
        //    //ps.emission.rate.mode = ParticleSystemCurveMode.TwoConstants;

        //    ps.gravityModifier = -0.02f;
        //    ps.scalingMode = ParticleSystemScalingMode.Shape;

        //    ParticleSystem.ShapeModule sm = ps.shape;
        //    sm.shapeType = ParticleSystemShapeType.Cone;
        //    sm.radius = 0.1f;
        //    sm.angle = 5;
        //    //sm.randomDirection = true;

        //    sm.arc = 360;
        //    sm.enabled = true;

        //    ParticleSystem.SizeOverLifetimeModule sl = ps.sizeOverLifetime;
        //    float size = (transform.localScale.y + transform.localScale.z) / 2;
        //    sl.size = new ParticleSystem.MinMaxCurve(size, AnimationCurve.Linear(0f, 1f, 0.2f, 5));
        //    sl.enabled = true;

        //    ParticleSystem.ColorOverLifetimeModule colm = ps.colorOverLifetime;
        //    colm.color = new Gradient() { alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(0.2f, 0f), new GradientAlphaKey(0f, lifetime * 0.6f) }, colorKeys = new GradientColorKey[] { new GradientColorKey(Color.yellow, 0), new GradientColorKey(Color.white, 0.1f) } };
        //    colm.enabled = true;

        //    psr = particle.GetComponent<ParticleSystemRenderer>();
        //    psr.sortMode = ParticleSystemSortMode.Distance;
        //    psr.sortingFudge = 80;
        //    psr.receiveShadows = true;

        //    psr.sharedMaterial = new Material(Shader.Find("Particles/Alpha Blended"));
        //    psr.sharedMaterial.mainTexture = (resources["/RocketBlockMod/Rocket.png"].texture);

        //}

        protected void fire_ps_init()
        {
            float lifetime = 0.5f;

            ps_fire = particle_fire.AddComponent<ParticleSystem>();
            ps_fire.Stop();
            ps_fire.startLifetime = lifetime;
            particle_fire.transform.parent = transform;
            particle_fire.transform.localPosition = new Vector3(1.25f, 0, 0.3f);
            particle_fire.transform.localRotation = new Quaternion(90, 0, 90, 0);
            

            ParticleSystem.ShapeModule sm = ps_fire.shape;
            sm.shapeType = ParticleSystemShapeType.Cone;
            sm.radius = 0f;
            sm.angle = 2;
            sm.randomDirection = false;
            sm.enabled = true;

            ParticleSystem.SizeOverLifetimeModule sl = ps_fire.sizeOverLifetime;
            float size = (transform.localScale.y + transform.localScale.z) / 2;
            sl.size = new ParticleSystem.MinMaxCurve(size, AnimationCurve.Linear(0f, 1f, lifetime, 0));
            sl.enabled = true;

            ParticleSystem.ColorOverLifetimeModule colm = ps_fire.colorOverLifetime;
            colm.color = new Gradient() { alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(0.2f, 0f), new GradientAlphaKey(0.8f, lifetime) }, colorKeys = new GradientColorKey[] { new GradientColorKey(Color.blue, 0), new GradientColorKey(ps_fire.startColor, lifetime) } };
            colm.enabled = true;

            psr_fire = particle_fire.GetComponent<ParticleSystemRenderer>();
            psr_fire.sharedMaterial = new Material(Shader.Find("Particles/Additive"));
            psr_fire.sharedMaterial.mainTexture = (resources["/MordenFirearmKitMod/RocketFire.png"].texture);
        }
    }

    //火箭巢 模块脚本
    class RocketPodBlockScript : RocketBlockScript
    {
        #region 功能变量声明

        //声明 滑条 载弹数量
        protected MSlider number_slider;

        //声明 载弹数量
        public int number = 18;

        //声明 滑条 连发间隔
        protected MSlider interval_slider;

        //声明 连发间隔
        public float interval = 2;

        #endregion

        #region 内部变量声明

        //声明 火箭弹
        private GameObject[] rocket = new GameObject[18];

        //声明 火箭弹标签
        private int Label = 0;

        //声明 火箭弹实例化位置
        private Vector3[] position_rocket = new Vector3[18];

        //声明 火箭弹刚体
        private Rigidbody[] rb = new Rigidbody[18];

        //声明 火箭弹脚本
        private RocketBlockScript rbs;

        //声明 连发开启
        private bool continued = false;
        #endregion

        public override void SafeAwake()
        {


            //添加 滑条 参数
            number_slider = AddSlider("载弹数量", "NUMBER", number, 1, 18);
            interval_slider = AddSlider("连发间隔 0.1s", "INTERVAL", interval, 1f, 5f);

            //委托 滑条改变 事件
            number_slider.ValueChanged += new ValueChangeHandler(number_valueChanged);
            interval_slider.ValueChanged += new ValueChangeHandler(interval_valueChanged);

            base.SafeAwake();
            delay_slider.DisplayInMapper = false;
            fired = true;
        }

        //改变 载弹数量 事件
        protected void number_valueChanged(float value)
        {
            number = (int)value;
        }

        //改变 连发间隔时间 事件
        protected void interval_valueChanged(float value)
        {
            interval = value;
        }

        public override void OnSave(XDataHolder stream)
        {
            base.OnSave(stream);
            SaveMapperValues(stream);
        }

        public override void OnLoad(XDataHolder stream)
        {
            base.OnLoad(stream);
            LoadMapperValues(stream);
        }

        protected override void OnSimulateStart()
        {
            base.OnSimulateStart();
            number = Mathf.Clamp(number, 1, 18);
            Rocket_Position();
            for (int i = 0; i < number; i++)
            {
                Rocket_Instantiate(i);
            }

        }

        protected override void OnSimulateUpdate()
        {
            //base.OnSimulateUpdate();

            //火箭弹重装
            Rocket_Reload();

            //发射键按下 执行发射准备
            if (key.IsPressed)
            {
                //火箭弹发射
                Rocket_Launch();

                //开始协同程序
                StartCoroutine(Timer(interval));
            }

            //连发开启时 执行发射准备
            if (continued)
            {
                //连发关闭
                continued = false;

                //火箭弹发射
                Rocket_Launch();

                //开始协同程序
                StartCoroutine(Timer(interval));
            }

        }

        protected override void OnSimulateFixedUpdate()
        {
            //base.OnSimulateFixedUpdate();
        }

        //火箭弹实例化位置计算
        public void Rocket_Position()
        {
            int i;

            //声明 原点
            Vector2 origin = new Vector2(0.4f, 0);

            //声明 大圆半径、角度差和旋转角
            float radius_large = 0.37f, angle_large = 30f;

            //声明 小圆半径、角度差和旋转角
            float radius_little = 0.19f, angle_little = 60f;

            //外圈火箭弹位置
            for (i = 0; i < 12; i++)
            {
                position_rocket[i] = new Vector3(
                                                    0,
                                                    origin.y + radius_large * Mathf.Sin(angle_large * i * Mathf.Deg2Rad),
                                                    origin.x - radius_large * Mathf.Cos(angle_large * i * Mathf.Deg2Rad)
                                                 );
            }

            //内圈火箭弹位置
            for (i = 0; i < 6; i++)
            {
                position_rocket[i + 12] = new Vector3(
                                                        0,
                                                        origin.y + radius_little * Mathf.Sin((angle_little * i + 30) * Mathf.Deg2Rad),
                                                        origin.x - radius_little * Mathf.Cos((angle_little * i + 30) * Mathf.Deg2Rad)
                                                      );
            }
        }

        //火箭弹实例化
        public void Rocket_Instantiate(int label)
        {

            //火箭弹安装位置 本地坐标转世界坐标
            Vector3 pos = transform.TransformVector(transform.InverseTransformVector(rigidbody.position) + position_rocket[label]);

            //火箭弹实例化 设置连接点失效
            rocket[label] = (GameObject)Instantiate(PrefabMaster.BlockPrefabs[650].gameObject, pos, transform.rotation, transform);
            Destroy(rocket[label].GetComponent<ConfigurableJoint>());

            //火箭弹刚体 不开启碰撞 不受物理影响
            rb[label] = rocket[label].GetComponent<Rigidbody>();
            rb[label].detectCollisions = false;
            rb[label].isKinematic = true;

            //设置火箭弹大小
            rocket[label].transform.localScale = new Vector3(1f, 0.5f, 0.5f);

            //火箭弹脚本 初始化
            rbs = rocket[label].GetComponent<RocketBlockScript>();
            rbs.key.AddOrReplaceKey(0, KeyCode.None);
            rbs.explosiontype = explosiontype;
            rbs.time = time;
            rbs.power = power;
            rbs.thrust = thrust;
            rbs.drag = drag;
            rbs.timeopen = timeopen;

        }

        //火箭弹发射准备
        public void Rocket_LaunchPlan(int label)
        {
            //火箭弹不存在即返回
            if (!rocket[label] || rocket[label].GetComponent<RocketBlockScript>().launched) return;

            //火箭弹发射位置
            Vector3 pos = transform.TransformVector(transform.InverseTransformVector(rigidbody.position) + position_rocket[label] + new Vector3(-3f, 0, 0));

            //火箭弹移动到发射位置
            rocket[label].transform.position = pos;

            //火箭巢本地速度
            Vector3 local_velocity = transform.InverseTransformDirection(rigidbody.velocity);

            //火箭弹继承火箭巢速度
            rb[label].velocity = transform.TransformDirection(Vector3.Scale(local_velocity, new Vector3(1.2f * -Mathf.Sign(local_velocity.x), 0.15f, 0.15f)));

            //火箭弹受物理影响
            rb[label].isKinematic = false;

            //火箭弹开启碰撞
            rb[label].detectCollisions = true;

            //火箭弹发射准备
            rocket[label].GetComponent<RocketBlockScript>().Rocket_LaunchPlan();

        }

        //火箭弹发射
        public void Rocket_Launch()
        {

            Rocket_LaunchPlan(Label);

            //火箭弹标签回零
            if (++Label > number - 1)
            {
                Label = 0;
            }
        }

        //火箭弹重装
        public void Rocket_Reload()
        {
            //火箭弹在无限弹药模式下 有空位的情况下实例化新火箭弹
            if (StatMaster.GodTools.InfiniteAmmoMode)
            {
                for (int i = 0; i < number; i++)
                {
                    if (!rocket[i] || rocket[i].GetComponent<RocketBlockScript>().launched)
                    {
                        Rocket_Instantiate(i);
                    }
                }
            }
        }

        //计时函数
        protected IEnumerator Timer(float t)
        {

            t *= 10;
            //等待0.t秒
            while (t-- > 0 && !key.IsReleased)
            {
                yield return new WaitForSeconds(0.01f);
            }

            if (key.IsDown)
            {
                continued = true;
            }

        }

    }

    //到时自毁脚本
    public class TimedSelfDestruct : MonoBehaviour
    {
        float timer = 0;
        void FixedUpdate()
        {
            ++timer;
            if (timer > 260)
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
