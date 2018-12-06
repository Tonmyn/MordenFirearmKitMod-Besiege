//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.ComponentModel;
//using UnityEngine;

//namespace MordenFirearmKitMod
//{
//    class BulletBase : BlockScript
//    {

//        #region 功能变量 声明

//        //声明 菜单 爆炸类型
//        protected MMenu explosiontype_menu;

//        //声明 滑条 爆炸威力
//        protected MSlider power_slider;

//        //声明 滑条 推力大小
//        protected MSlider thrust_slider;

//        //声明 滑条 阻力大小
//        protected MSlider drag_slider;

//        //声明 滑条 碰撞开启时间
//        protected MSlider timeopen_slider;

//        //声明 碰撞开启时间
//        public float timeopen = 2f;

//        //声明 碰撞开启
//        public bool canBeCollision = false;

//        //声明 爆炸类型
//        public int explosiontype;

//        //声明 爆炸威力
//        public float power = 5f;

//        //声明 推力大小
//        public float thrust = 4f;

//        //声明 阻力大小
//        public float drag = 0.5f;

//        //声明 是否发射
//        public bool launched = false;

//        //爆炸类型
//        public enum ExplosionType
//        {
//            炸弹 = 0,
//            手雷 = 1,
//            烟花 = 2
//        }

//        #endregion

//        #region 内部变量 声明

//        //声明 重心初始位置
//        internal Vector3 com;

//        //声明 粒子游戏组件
//        internal GameObject particle = new GameObject();

//        //声明 粒子系统
//        internal ParticleSystem ps;

//        //声明 粒子渲染器
//        internal ParticleSystemRenderer psr;


//        internal Vector3 pos_thrust;

//        internal Vector3 pos_drag;

//        #endregion

//        public override void SafeAwake()
//        {
//            base.SafeAwake();

//            //添加 菜单 参数
//            explosiontype_menu = AddMenu("ExplosionType", explosiontype, new System.Collections.Generic.List<string> { "炸弹", "手雷", "烟花" });

//            //添加 滑条 参数
//            power_slider = AddSlider("爆炸威力", "POWER", power, 3f, 8f);
//            thrust_slider = AddSlider("推力大小", "THRUST", thrust, 3f, 10f);
//            drag_slider = AddSlider("阻力大小", "DRAG", drag, 0.2f, 3f);
//            timeopen_slider = AddSlider("碰撞开启 0.05s", "TIMEOPEN", timeopen, 1f, 5f);

//            //委托 菜单改变 事件
//            explosiontype_menu.ValueChanged += new ValueHandler(explosiontype_valueChanged);

//            //委托 滑条改变 事件

//            power_slider.ValueChanged += new ValueChangeHandler(power_valueChanged);
//            thrust_slider.ValueChanged += new ValueChangeHandler(thrust_valueChanged);
//            drag_slider.ValueChanged += new ValueChangeHandler(drag_valueChanged);
//            timeopen_slider.ValueChanged += new ValueChangeHandler(timeopen_valueChanged);
//        }

//        //改变 爆炸类型 事件
//        protected void explosiontype_valueChanged(int value)
//        {
//            explosiontype = value;
//        }

//        //改变 爆炸威力 事件
//        protected void power_valueChanged(float value)
//        {
//            power = value;
//        }

//        //改变 推力大小 事件
//        protected void thrust_valueChanged(float value)
//        {
//            thrust = value;
//        }

//        //改变 阻力大小 事件
//        protected void drag_valueChanged(float value)
//        {
//            drag = value;
//        }

//        //改变 碰撞开启时间 事件
//        protected void timeopen_valueChanged(float value)
//        {
//            timeopen = value;
//        }

//        protected override void BlockPlaced()
//        {
//            base.BlockPlaced();
//            com = rigidbody.centerOfMass;

//        }

//        protected override void BuildingUpdate()
//        {
//            base.BuildingUpdate();
//            rigidbody.centerOfMass = com + new Vector3(0, 0, 0.25f * transform.localScale.z);
//        }



//        protected override void OnSimulateStart()
//        {
//            base.OnSimulateStart();
//#if DEBUG
//            //create();
//#endif
//            //初始化粒子系统
//            particlesystem_init(new Vector3(1.25f, 0, 0.3f), new Quaternion(90, 0, 90, 0), 0.5f);
//            Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

//        }

//        protected override void OnSimulateFixedStart()
//        {
//            base.OnSimulateFixedStart();

//        }

//        //爆炸事件
//        public IEnumerator Rocket_Explodey()
//        {

//            yield return new WaitForFixedUpdate();

//            //爆炸范围
//            float radius = power;

//            //爆炸位置
//            Vector3 position_hit = transform.TransformDirection(new Vector3(-1f, rigidbody.centerOfMass.y, rigidbody.centerOfMass.z)) + transform.position;

//            //爆炸类型 炸弹
//            if (explosiontype == (int)ExplosionType.炸弹)
//            {
//                GameObject explo = (GameObject)Instantiate(PrefabMaster.BlockPrefabs[23].gameObject, position_hit, transform.rotation);
//                explo.transform.localScale = Vector3.one * 0.01f;
//                ExplodeOnCollideBlock ac = explo.GetComponent<ExplodeOnCollideBlock>();
//                ac.radius = 2 + radius;
//                ac.power = 3000f * radius;
//                ac.torquePower = 5000f * radius;
//                ac.upPower = 0;
//                ac.Explodey();
//                explo.AddComponent<TimedSelfDestruct>();
//            }
//            else if (explosiontype == (int)ExplosionType.手雷)
//            {
//                GameObject explo = (GameObject)GameObject.Instantiate(PrefabMaster.BlockPrefabs[54].gameObject, position_hit, transform.rotation);
//                explo.transform.localScale = Vector3.one * 0.01f;
//                ControllableBomb ac = explo.GetComponent<ControllableBomb>();
//                ac.radius = 2 + radius;
//                ac.power = 3000f * radius;
//                ac.randomDelay = 0.00001f;
//                ac.upPower = 0f;
//                ac.StartCoroutine_Auto(ac.Explode());
//                explo.AddComponent<TimedSelfDestruct>();
//            }
//            else if (explosiontype == (int)ExplosionType.烟花)
//            {
//                GameObject explo = (GameObject)GameObject.Instantiate(PrefabMaster.BlockPrefabs[59].gameObject, position_hit, transform.rotation);
//                explo.transform.localScale = Vector3.one * 0.01f;
//                TimedRocket ac = explo.GetComponent<TimedRocket>();
//                ac.SetSlip(Color.white);
//                ac.radius = 2 + radius;
//                ac.power = 3000f * radius;
//                ac.randomDelay = 0.000001f;
//                ac.upPower = 0;
//                ac.StartCoroutine(ac.Explode(0.01f));
//                explo.AddComponent<TimedSelfDestruct>();
//            }

//            Destroy(gameObject);


//        }

//        //粒子系统初始化
//        protected void particlesystem_init(Vector3 position, Quaternion rotation, float lifetime, [DefaultValue("Local")] ParticleSystemSimulationSpace space)
//        {

//            ps = particle.AddComponent<ParticleSystem>();
//            ps.Stop(true);
//            particle.transform.parent = transform;
//            particle.transform.localPosition = position;
//            particle.transform.localRotation = rotation;
//            ps.startLifetime = lifetime;
//            ps.time = 0.1f;

//            ParticleSystem.ShapeModule sm = ps.shape;
//            sm.shapeType = ParticleSystemShapeType.Cone;
//            sm.radius = 0f;
//            sm.angle = 3;
//            sm.randomDirection = false;
//            sm.enabled = true;

//            ParticleSystem.SizeOverLifetimeModule sl = ps.sizeOverLifetime;
//            float size = (transform.localScale.y + transform.localScale.z) / 2;
//            sl.size = new ParticleSystem.MinMaxCurve(size, AnimationCurve.Linear(0f, 1f, lifetime, lifetime));
//            sl.enabled = true;

//            ParticleSystem.ColorOverLifetimeModule colm = ps.colorOverLifetime;
//            colm.color = new Gradient() { alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(0.5f, 0), new GradientAlphaKey(0.8f, 0.2f), new GradientAlphaKey(1f, lifetime) }, colorKeys = new GradientColorKey[] { new GradientColorKey(Color.blue, 0), new GradientColorKey(ps.startColor, 0.1f), new GradientColorKey(Color.gray, 0.11f) } };
//            colm.enabled = true;

//            psr = particle.GetComponent<ParticleSystemRenderer>();
//            psr.sharedMaterial = new Material(Shader.Find("Particles/Additive"));
//            psr.sharedMaterial.mainTexture = (resources["/RocketBlockMod/Rocket.png"].texture);
//            psr.sharedMaterial.color = new Color(0, 0, 0);


//        }

//        //粒子系统初始化
//        protected void particlesystem_init(Vector3 position, Quaternion rotation, float lifetime)
//        {
//            ps = particle.AddComponent<ParticleSystem>();
//            ps.Stop(true);
//            particle.transform.parent = transform;
//            particle.transform.localPosition = position;
//            particle.transform.localRotation = rotation;
//            ps.startLifetime = lifetime;
//            ps.simulationSpace = ParticleSystemSimulationSpace.Local;

//            ParticleSystem.ShapeModule sm = ps.shape;
//            sm.shapeType = ParticleSystemShapeType.Cone;
//            sm.radius = 0f;
//            sm.angle = 2;
//            sm.randomDirection = false;
//            sm.enabled = true;

//            ParticleSystem.SizeOverLifetimeModule sl = ps.sizeOverLifetime;
//            float size = (transform.localScale.y + transform.localScale.z) / 2;
//            sl.size = new ParticleSystem.MinMaxCurve(size, AnimationCurve.Linear(0f, 1f, lifetime, 0));
//            sl.enabled = true;

//            ParticleSystem.ColorOverLifetimeModule colm = ps.colorOverLifetime;
//            colm.color = new Gradient() { alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(0.2f, 0f), new GradientAlphaKey(0.8f, lifetime) }, colorKeys = new GradientColorKey[] { new GradientColorKey(Color.blue, 0), new GradientColorKey(ps.startColor, lifetime) } };
//            colm.enabled = true;

//            psr = particle.GetComponent<ParticleSystemRenderer>();
//            psr.sharedMaterial = new Material(Shader.Find("Particles/Additive"));
//            psr.sharedMaterial.mainTexture = (resources["/RocketBlockMod/Rocket.png"].texture);

//        }


//        //到时自毁脚本
//        public class TimedSelfDestruct : MonoBehaviour
//        {
//            float timer = 0;
//            void FixedUpdate()
//            {
//                ++timer;
//                if (timer > 260)
//                {
//                    Destroy(gameObject);
//                    if (this.GetComponent<TimedRocket>())
//                    {
//                        Destroy(this.GetComponent<TimedRocket>());
//                    }
//                }
//            }
//        }

//    }
//}
