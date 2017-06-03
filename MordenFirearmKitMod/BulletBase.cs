using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace MordenFirearmKitMod
{
    class BulletBase : BlockScript
    {

        #region 功能变量 声明

        //声明 菜单 爆炸类型
        protected MMenu explosiontype_menu;

        //声明 滑条 爆炸威力
        protected MSlider power_slider;

        //声明 滑条 阻力大小
        protected MSlider drag_slider;

        //声明 爆炸类型
        public int explosiontype;

        //声明 爆炸威力
        public float power = 5f;

        //声明 阻力大小
        public float drag = 0.5f;

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

        #region 内部变量 声明

        //声明 重心初始位置
        internal Vector3 com;

        //声明 粒子游戏组件
        internal GameObject particle = new GameObject();

        //声明 粒子系统
        internal ParticleSystem ps;

        //声明 粒子渲染器
        internal ParticleSystemRenderer psr;


        internal Vector3 pos_thrust;

        internal Vector3 pos_drag;

        #endregion


        //改变 爆炸类型 事件
        protected void explosiontype_valueChanged(int value)
        {
            explosiontype = value;
        }

        //改变 爆炸威力 事件
        protected void power_valueChanged(float value)
        {
            power = value;
        }

        //改变 阻力大小 事件
        protected void drag_valueChanged(float value)
        {
            drag = value;
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
            particlesystem_init(new Vector3(1.25f, 0, 0.3f), new Quaternion(90, 0, 90, 0), 0.5f);
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
                ps.Emit(2);
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
                StartCoroutine(Rocket_Launch());
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
        public IEnumerator Rocket_Launch()
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

            yield return new WaitForFixedUpdate();
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

        //粒子系统初始化
        protected void particlesystem_init(Vector3 position, Quaternion rotation, float lifetime, [DefaultValue("Local")] ParticleSystemSimulationSpace space)
        {

            ps = particle.AddComponent<ParticleSystem>();
            ps.Stop(true);
            particle.transform.parent = transform;
            particle.transform.localPosition = position;
            particle.transform.localRotation = rotation;
            ps.startLifetime = lifetime;
            ps.time = 0.1f;

            ParticleSystem.ShapeModule sm = ps.shape;
            sm.shapeType = ParticleSystemShapeType.Cone;
            sm.radius = 0f;
            sm.angle = 3;
            sm.randomDirection = false;
            sm.enabled = true;

            ParticleSystem.SizeOverLifetimeModule sl = ps.sizeOverLifetime;
            float size = (transform.localScale.y + transform.localScale.z) / 2;
            sl.size = new ParticleSystem.MinMaxCurve(size, AnimationCurve.Linear(0f, 1f, lifetime, lifetime));
            sl.enabled = true;

            ParticleSystem.ColorOverLifetimeModule colm = ps.colorOverLifetime;
            colm.color = new Gradient() { alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(0.5f, 0), new GradientAlphaKey(0.8f, 0.2f), new GradientAlphaKey(1f, lifetime) }, colorKeys = new GradientColorKey[] { new GradientColorKey(Color.blue, 0), new GradientColorKey(ps.startColor, 0.1f), new GradientColorKey(Color.gray, 0.11f) } };
            colm.enabled = true;

            psr = particle.GetComponent<ParticleSystemRenderer>();
            psr.sharedMaterial = new Material(Shader.Find("Particles/Additive"));
            psr.sharedMaterial.mainTexture = (resources["/RocketBlockMod/Rocket.png"].texture);
            psr.sharedMaterial.color = new Color(0, 0, 0);


        }

        //粒子系统初始化
        protected void particlesystem_init(Vector3 position, Quaternion rotation, float lifetime)
        {
            ps = particle.AddComponent<ParticleSystem>();
            ps.Stop(true);
            particle.transform.parent = transform;
            particle.transform.localPosition = position;
            particle.transform.localRotation = rotation;
            ps.startLifetime = lifetime;
            ps.simulationSpace = ParticleSystemSimulationSpace.Local;

            ParticleSystem.ShapeModule sm = ps.shape;
            sm.shapeType = ParticleSystemShapeType.Cone;
            sm.radius = 0f;
            sm.angle = 2;
            sm.randomDirection = false;
            sm.enabled = true;

            ParticleSystem.SizeOverLifetimeModule sl = ps.sizeOverLifetime;
            float size = (transform.localScale.y + transform.localScale.z) / 2;
            sl.size = new ParticleSystem.MinMaxCurve(size, AnimationCurve.Linear(0f, 1f, lifetime, 0));
            sl.enabled = true;

            ParticleSystem.ColorOverLifetimeModule colm = ps.colorOverLifetime;
            colm.color = new Gradient() { alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(0.2f, 0f), new GradientAlphaKey(0.8f, lifetime) }, colorKeys = new GradientColorKey[] { new GradientColorKey(Color.blue, 0), new GradientColorKey(ps.startColor, lifetime) } };
            colm.enabled = true;

            psr = particle.GetComponent<ParticleSystemRenderer>();
            psr.sharedMaterial = new Material(Shader.Find("Particles/Additive"));
            psr.sharedMaterial.mainTexture = (resources["/RocketBlockMod/Rocket.png"].texture);

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
}
