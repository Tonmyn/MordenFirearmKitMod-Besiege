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
           .Components(new Type[] { typeof(RocketBlockScript),typeof(ParticleSystemComponent), })

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

      
        //声明 菜单 功能页
        protected MMenu page_menu ;

        //声明 功能页码
        public int page = 0;

        #region 功能变量 声明

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

        //声明 是否爆炸
        public bool isExplodey = false;

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

        //声明 尾焰粒子系统
        protected ParticleSystem ps_fire;

        //声明 尾焰粒子渲染器
        protected ParticleSystemRenderer psr_fire;

        //声明 尾焰粒子属性
        public ParticleSystemProperties psp_fire = new ParticleSystemProperties().init_fire();

        //声明 滑条 粒子存活时间
        protected MSlider lifetime_fire;

        //声明 滑条 半径
        protected MSlider radius_fire;

        //声明 滑条 角度
        protected MSlider angle_fire;

        //声明 滑条 尺寸
        protected MSlider size_fire;

        //声明 滑条 初始尺寸
        protected MSlider sizeStart_fire;

        //声明 滑条 结束尺寸
        protected MSlider sizeEnd_fire;

        protected MColourSlider colorStart_fire;

        protected MColourSlider colorEnd_fire;

        protected MSlider colorStartTime_fire;

        protected MSlider colorEndTime_fire;

        #endregion

        #region 尾烟变量 声明

        //声明 尾烟粒子组件
        protected GameObject particle_smoke = new GameObject();

        //声明 粒子系统
        protected ParticleSystem ps_smoke;

        //声明 粒子渲染器
        protected ParticleSystemRenderer psr_smoke;

        //声明 尾焰粒子属性
        public ParticleSystemProperties psp_smoke = new ParticleSystemProperties().init_smoke();

        //声明 菜单 尾烟颜色
        protected MMenu colorsmoke_menu;

        //声明 滑条 粒子存活时间
        protected MSlider lifetime_smoke;

        //声明 滑条 角度
        protected MSlider angle_smoke;

        //声明 滑条 尺寸
        protected MSlider size_smoke;

        //声明 滑条 初始尺寸
        protected MSlider sizeStart_smoke;

        //声明 滑条 结束尺寸
        protected MSlider sizeEnd_smoke;

        public int color_smoke = 0;

        #endregion

        #region 内部变量 声明

        //声明 重心初始位置
        internal Vector3 com;

        //声明 推力位置
        internal Vector3 pos_thrust;

        //声明 阻力位置
        internal Vector3 pos_drag;
        
        //声明 粒子属性结构体
        internal struct ParticleSystemProperties
        {

            public float radius;
            public float angle;
            public float lifetime;

            public float size;
            public float size_start;
            public float size_end;

            public float alpha_start;
            public float alpha_end;
            public float alpha_startTime;
            public float alpha_endTime;

            public Color color_start;
            public Color color_end;
            public float color_startTime;
            public float color_endTime;

            //尾焰数据初始化
            public ParticleSystemProperties init_fire()
            {
                ParticleSystemProperties psp;

                psp.radius = 0;
                psp.angle = 2;
                psp.lifetime = 0.5f;

                psp.size = 0.5f;
                psp.size_start = 1;
                psp.size_end = 0;

                psp.color_start = Color.blue;
                psp.color_end = Color.yellow;
                psp.color_startTime = 0;
                psp.color_endTime = lifetime;

                psp.alpha_start = 1;
                psp.alpha_end = 0;
                psp.alpha_startTime = 0;
                psp.alpha_endTime = lifetime;

                return psp;
            }

            //尾烟数据初始化
            public ParticleSystemProperties init_smoke()
            {
                ParticleSystemProperties psp;
                psp.radius = 0.1f;
                psp.angle = 15;
                psp.lifetime = 3f;

                psp.size = 1;
                psp.size_start = 1;
                psp.size_end = 3f;

                psp.color_start = Color.gray;
                psp.color_end = psp.color_start;
                psp.color_startTime = 0;
                psp.color_endTime = 0.15f;

                psp.alpha_start = 1f;
                psp.alpha_end = 0;
                psp.alpha_startTime = 0;
                psp.alpha_endTime = psp.lifetime*0.8f;

                return psp;
            }

        }

        #endregion

        public override void SafeAwake()
        {
            base.SafeAwake();

            //添加 菜单 功能页
            page_menu = AddMenu("Page",page,new List<string> { "火箭参数","尾焰参数","尾烟参数"});
            
            //委托 页码改变 事件
            page_menu.ValueChanged += new ValueHandler(page_valuechanged);

            #region 基本参数初始化

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

            #endregion

            #region 尾焰组件初始化

            lifetime_fire = AddSlider("时间", "LifeTimeFire", psp_fire.lifetime, 0, 1);

            radius_fire = AddSlider("半径", "RadiusFire", psp_fire.radius, 0, 2);

            angle_fire = AddSlider("角度", "AngleFire", psp_fire.angle, 0, 5);

            size_fire = AddSlider("尺寸", "SizeFire", psp_fire.size, 0, 5);

            sizeStart_fire = AddSlider("初始尺寸", "SizeStartFire", psp_fire.size_start, 0, 5);

            sizeEnd_fire = AddSlider("结束尺寸", "SizeEndFire", psp_fire.size_end, 0, 5);

            colorStart_fire = AddColourSlider("渐变初始颜色", "ColorStartFire", psp_fire.color_start);

            colorEnd_fire = AddColourSlider("渐变结束颜色", "ColorEndFire", psp_fire.color_end);

            colorStartTime_fire = AddSlider("渐变初始时间", "ColorStartTimeFire", psp_fire.color_startTime, 0, psp_fire.lifetime);

            colorEndTime_fire = AddSlider("渐变结束时间", "ColorEndTimeFire", psp_fire.color_endTime, 0, psp_fire.lifetime);


            lifetime_fire.ValueChanged += new ValueChangeHandler(lifeTimeFire_valueChanged);

            radius_fire.ValueChanged += new ValueChangeHandler(radiusFire_valueChanged);

            angle_fire.ValueChanged += new ValueChangeHandler(angleFire_valueChanged);

            size_fire.ValueChanged += new ValueChangeHandler(sizeFire_valueChanged);

            sizeStart_fire.ValueChanged += new ValueChangeHandler(sizeStartFire_valueChanged);

            sizeEnd_fire.ValueChanged += new ValueChangeHandler(sizeEndFire_valueChanged);

            colorStart_fire.ValueChanged += new ColourChangeHandler(colorStartFire_valueChanged);

            colorEnd_fire.ValueChanged += new ColourChangeHandler(colorEndFire_valueChanged);

            colorStartTime_fire.ValueChanged += new ValueChangeHandler(colorStartTimeFire_valueChanged);

            colorEndTime_fire.ValueChanged += new ValueChangeHandler(colorEndTimeFire_valueChanged);

            #endregion

            #region 尾烟组件初始化

            colorsmoke_menu = AddMenu("尾烟颜色", color_smoke, new System.Collections.Generic.List<string> { "灰色", "白色", "黑色" });

            lifetime_smoke = AddSlider("时间", "LifeTimeSmoke", psp_smoke.lifetime, 0, 5);

            angle_smoke = AddSlider("角度", "AngleSmoke", psp_smoke.angle, 0, 60);

            size_smoke = AddSlider("尺寸", "SizeSmoke", psp_smoke.size, 0, 3);

            sizeStart_smoke = AddSlider("初始尺寸", "SizeStartSmoke", psp_smoke.size_start, 0, 1);

            sizeEnd_smoke = AddSlider("结束尺寸", "SizeEndSmoke", psp_smoke.size_end, 0, 10);


            colorsmoke_menu.ValueChanged += new ValueHandler(colorSmoke_valueChanged);

            lifetime_smoke.ValueChanged += new ValueChangeHandler(lifeTimeSmoke_valueChanged);

            angle_smoke.ValueChanged += new ValueChangeHandler(angleSmoke_valueChanged);

            size_smoke.ValueChanged += new ValueChangeHandler(sizeSmoke_valueChanged);

            sizeStart_smoke.ValueChanged += new ValueChangeHandler(sizeStartSmoke_valueChanged);

            sizeEnd_smoke.ValueChanged += new ValueChangeHandler(sizeEndSmoke_valueChanged);

            #endregion

            page_valuechanged(0);

        }
        
        //改变 功能开关 事件
        protected virtual void page_valuechanged(int value)
        {
            bool show_0, show_1, show_2;

            if (value == 0)
            {
                show_0 = true;
                show_1 = false;
                show_2 = false;
            }
            else if (value == 1)
            {
                show_0 = false;
                show_1 = true;
                show_2 = false;
            }
            else
            {
                show_0 = false;
                show_1 = false;
                show_2 = true;
            }

            #region 页码0控件

            key.DisplayInMapper = show_0;

            explosiontype_menu.DisplayInMapper = show_0;

            delay_slider.DisplayInMapper = show_0;

            time_slider.DisplayInMapper = show_0;

            power_slider.DisplayInMapper = show_0;

            thrust_slider.DisplayInMapper = show_0;

            drag_slider.DisplayInMapper = show_0;

            timeopen_slider.DisplayInMapper = show_0;

            #endregion

            #region 页码1控件   

            //toggle_fire.DisplayInMapper = show_1;

            lifetime_fire.DisplayInMapper = show_1;

            radius_fire.DisplayInMapper = show_1;

            angle_fire.DisplayInMapper = show_1;

            size_fire.DisplayInMapper = show_1;

            sizeStart_fire.DisplayInMapper = show_1;

            sizeEnd_fire.DisplayInMapper = show_1;

            colorStart_fire.DisplayInMapper = show_1;

            colorEnd_fire.DisplayInMapper = show_1;

            colorStartTime_fire.DisplayInMapper = show_1;

            colorEndTime_fire.DisplayInMapper = show_1;

            //alphaStart_fire.DisplayInMapper = show_1;

            //alphaEnd_fire.DisplayInMapper = show_1;

            //alphaStartTime_fire.DisplayInMapper = show_1;

            //alphaEndTime_fire.DisplayInMapper = show_1;

            #endregion

            #region 页码2控件   

            colorsmoke_menu.DisplayInMapper = show_2;

            //toggle_smoke.DisplayInMapper = show_2;

            lifetime_smoke.DisplayInMapper = show_2;

            //radius_smoke.DisplayInMapper = show_2;

            angle_smoke.DisplayInMapper = show_2;

            size_smoke.DisplayInMapper = show_2;

            sizeStart_smoke.DisplayInMapper = show_2;

            sizeEnd_smoke.DisplayInMapper = show_2;

            //colorStart_smoke.DisplayInMapper = show_2;

            //colorEnd_smoke.DisplayInMapper = show_2;

            //colorStartTime_smoke.DisplayInMapper = show_2;

            //colorEndTime_smoke.DisplayInMapper = show_2;

            //alphaStart_smoke.DisplayInMapper = show_2;

            //alphaEnd_smoke.DisplayInMapper = show_2;

            //alphaStartTime_smoke.DisplayInMapper = show_2;

            //alphaEndTime_smoke.DisplayInMapper = show_2;

            #endregion
        }

        #region 基本参数事件



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

        #endregion

        #region 尾焰参数事件

        public void lifeTimeFire_valueChanged(float value)
        {
            psp_fire.lifetime = value;
        }

        public void radiusFire_valueChanged(float value)
        {
            psp_fire.radius = value;
        }

        public void angleFire_valueChanged(float value)
        {
            psp_fire.angle = value;
        }

        public void sizeFire_valueChanged(float value)
        {
            psp_fire.size = value;
        }

        public void sizeStartFire_valueChanged(float value)
        {
            psp_fire.size_start = value;
        }

        public void sizeEndFire_valueChanged(float value)
        {
            psp_fire.size_end = value;
        }

        public void colorStartFire_valueChanged(Color value)
        {
            psp_fire.color_start = value;
        }

        public void colorEndFire_valueChanged(Color value)
        {
            psp_fire.color_end = value;
        }

        public void colorStartTimeFire_valueChanged(float value)
        {
            psp_fire.color_startTime = value;
        }

        public void colorEndTimeFire_valueChanged(float value)
        {
            psp_fire.color_endTime = value;
        }

        public void alphaStartFire_valueChanged(float value)
        {
            psp_fire.alpha_start = value;
        }

        public void alphaEndFire_valueChanged(float value)
        {
            psp_fire.alpha_end = value;
        }

        public void alphaStartTimeFire_valueChanged(float value)
        {
            psp_fire.alpha_startTime = value;
        }

        public void alphaEndTimeFire_valueChanged(float value)
        {
            psp_fire.alpha_endTime = value;
        }

        #endregion

        #region 尾烟参数事件

        public virtual void colorSmoke_valueChanged(int value)
        {
            color_smoke = value;
            if (value == 0)
            {
                psp_smoke.color_start = psp_smoke.color_end = Color.gray;
            }
            else if (value == 1)
            {
                psp_smoke.color_start = psp_smoke.color_end = Color.white;
            }
            else
            {
                psp_smoke.color_start = psp_smoke.color_end = Color.black;
            }
        }

        public void lifeTimeSmoke_valueChanged(float value)
        {
            psp_smoke.lifetime = value;
            psp_smoke.alpha_endTime = value * 0.8f;
        }

        public void radiusSmoke_valueChanged(float value)
        {
            psp_smoke.radius = value;
        }

        public void angleSmoke_valueChanged(float value)
        {
            psp_smoke.angle = value;
        }

        public void sizeSmoke_valueChanged(float value)
        {
            psp_smoke.size = value;
        }

        public void sizeStartSmoke_valueChanged(float value)
        {
            psp_smoke.size_start = value;
        }

        public void sizeEndSmoke_valueChanged(float value)
        {
            psp_smoke.size_end = value;
        }

        public void colorStartSmoke_valueChanged(Color value)
        {
            psp_smoke.color_start = value;
        }

        public void colorEndSmoke_valueChanged(Color value)
        {
            psp_smoke.color_end = value;
        }

        public void colorStartTimeSmoke_valueChanged(float value)
        {
            psp_smoke.color_startTime = value;
        }

        public void colorEndTimeSmoke_valueChanged(float value)
        {
            psp_smoke.color_endTime = value;
        }

        public void alphaStartSmoke_valueChanged(float value)
        {
            psp_smoke.alpha_start = value;
        }

        public void alphaEndSmoke_valueChanged(float value)
        {
            psp_smoke.alpha_end = value;
        }

        public void alphaStartTimeSmoke_valueChanged(float value)
        {
            psp_smoke.alpha_startTime = value;
        }

        public void alphaEndTimeSmoke_valueChanged(float value)
        {
            psp_smoke.alpha_endTime = value;
        }

        #endregion

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
            fire_ps_init(psp_fire);
            smoke_ps_init(psp_smoke);
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

            if (delayed && !fired && !isExplodey)
            {
                ps_fire.Emit(2);
                ps_smoke.Emit(10); 
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
            if (delayed && !fired && !isExplodey)
            {
                //ps_fire.Emit(2);
                ps_smoke.Emit(10);
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
        public virtual void Rocket_Launch()
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

            if (isExplodey)
            {
                yield break;
            }

            yield return new WaitForFixedUpdate();

            isExplodey = true;

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

            foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>())
            {
                if (r.name == "Vis")
                {
                    r.enabled = false;


                }
            }

            transform.localScale = Vector3.zero;
            Rigidbody.isKinematic = true;
            rigidbody.detectCollisions = false;
            Destroy(gameObject.GetComponentInChildren<FireController>());
            psp_fire.lifetime = 0;
            ps_fire.Stop();
            ps_smoke.Stop();
            

            gameObject.AddComponent<TimedSelfDestruct>().lifetime = psp_smoke.lifetime * 120;

        }

        protected void fire_ps_init(ParticleSystemProperties psp)
        {
            ps_fire = particle_fire.AddComponent<ParticleSystem>();
            ps_fire.Stop();
            ps_fire.startLifetime = psp.lifetime;
            particle_fire.transform.parent = transform;
            particle_fire.transform.localPosition = new Vector3(1.25f, 0, 0.3f);
            particle_fire.transform.localRotation = new Quaternion(90, 0, 90, 0);
            

            ParticleSystem.ShapeModule sm = ps_fire.shape;
            sm.shapeType = ParticleSystemShapeType.Cone;
            sm.radius = 0f;
            sm.angle = 2;
            //sm.randomDirection = true;
            sm.enabled = true;
            

            ParticleSystem.SizeOverLifetimeModule sl = ps_fire.sizeOverLifetime;
            //float size = (transform.localScale.y + transform.localScale.z) / 2;
            sl.size = new ParticleSystem.MinMaxCurve(psp.size, AnimationCurve.Linear(0f, psp.size_start, psp.lifetime, psp.size_end));
            sl.enabled = true;

            ParticleSystem.ColorOverLifetimeModule colm = ps_fire.colorOverLifetime;
            colm.color = new Gradient()
            {

                alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(psp.alpha_start, psp.alpha_startTime), new GradientAlphaKey(psp.alpha_end, psp.alpha_endTime) },

                colorKeys = new GradientColorKey[] { new GradientColorKey(psp.color_start, psp.color_startTime), new GradientColorKey(psp.color_end, psp.color_endTime) }

            };
            colm.enabled = true;

            psr_fire = particle_fire.GetComponent<ParticleSystemRenderer>();
            psr_fire.sharedMaterial = new Material(Shader.Find("Particles/Additive"));
            psr_fire.sharedMaterial.mainTexture = (resources["/MordenFirearmKitMod/RocketFire.png"].texture);
        }

        protected void smoke_ps_init(ParticleSystemProperties psp)
        {
            ps_smoke = particle_smoke.AddComponent<ParticleSystem>();
            ps_smoke.Stop();
            ps_smoke.startLifetime = psp.lifetime;
            particle_smoke.transform.parent = transform;
            particle_smoke.transform.localPosition = new Vector3(2.5f, 0, 0.3f);
            particle_smoke.transform.localRotation = new Quaternion(90, 0, 90, 0);
            ps_smoke.loop = true;
            ps_smoke.startColor = psp_smoke.color_start;
            ps_smoke.simulationSpace = ParticleSystemSimulationSpace.World;
            ps_smoke.maxParticles = 10240;
            ps_smoke.gravityModifier = -0.02f;
            ps_smoke.scalingMode = ParticleSystemScalingMode.Shape;
            

            ParticleSystem.ShapeModule sm = ps_smoke.shape;
            sm.shapeType = ParticleSystemShapeType.ConeShell;
            sm.radius = psp_smoke.radius;
            sm.angle = psp_smoke.angle;
            sm.length = 1;
            
            //sm.arc = 360 ;
            sm.randomDirection = true;
            sm.enabled = true;


            ParticleSystem.SizeOverLifetimeModule sl = ps_smoke.sizeOverLifetime;
            //float size = (transform.localScale.y + transform.localScale.z) / 2;
            sl.size = new ParticleSystem.MinMaxCurve(psp.size, AnimationCurve.Linear(0f, psp.size_start, psp.lifetime, psp.size_end));
            sl.enabled = true;

            ParticleSystem.RotationOverLifetimeModule rolm = ps_smoke.rotationOverLifetime;
            rolm.enabled = true;
            rolm.x = new ParticleSystem.MinMaxCurve(0, 360);

            ParticleSystem.ColorOverLifetimeModule colm = ps_smoke.colorOverLifetime;
            colm.color = new Gradient()
            {

                alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(psp.alpha_start, psp.alpha_startTime), new GradientAlphaKey(psp.alpha_end, psp.alpha_endTime) },

                colorKeys = new GradientColorKey[] { new GradientColorKey(psp.color_start, psp.color_startTime), new GradientColorKey(psp.color_end, psp.color_endTime) }

            };
            colm.enabled = true;

            ParticleSystem seb = ps_smoke.subEmitters.birth0;
            //sem.birth0.simulationSpace = ParticleSystemSimulationSpace.World;
            seb = GetComponent<ParticleSystem>();
            //sem.enabled = true;

            psr_smoke = particle_smoke.GetComponent<ParticleSystemRenderer>();
            psr_smoke.sortMode = ParticleSystemSortMode.Distance;
            psr_smoke.sortingFudge = 80;
            psr_smoke.receiveShadows = true;
            psr_smoke.sharedMaterial = new Material(Shader.Find("Particles/Alpha Blended"));
            psr_smoke.sharedMaterial.mainTexture = (resources["/MordenFirearmKitMod/RocketSmoke.png"].texture);
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

        protected override void page_valuechanged(int value)
        {
            base.page_valuechanged(value);
            bool show = false;
            if (value == 0)
            {
                show = true;
            }

            number_slider.DisplayInMapper = show;
            interval_slider.DisplayInMapper = show;
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
            //for (int i = 0; i < number; i++)
            //{
            //    if (!rocket[i].GetComponent<RocketBlockScript>().launched)
            //    rocket[i].transform.position = transform.TransformVector(transform.InverseTransformVector(rigidbody.position) + position_rocket[i]);
            //}
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
            rocket[label] = (GameObject)Instantiate(PrefabMaster.BlockPrefabs[650].gameObject, transform.position, transform.rotation, transform);
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

            //火箭弹尾焰初始化
            rbs.psp_fire.lifetime = lifetime_fire.Value;
            rbs.psp_fire.radius = radius_fire.Value;
            rbs.psp_fire.angle = angle_fire.Value;
            rbs.psp_fire.size = size_fire.Value;
            rbs.psp_fire.size_start = sizeStart_fire.Value;
            rbs.psp_fire.size_end = sizeEnd_fire.Value;
            rbs.psp_fire.color_start = colorStart_fire.Value;
            rbs.psp_fire.color_end = colorEnd_fire.Value;
            rbs.psp_fire.color_startTime = colorStartTime_fire.Value;
            rbs.psp_fire.color_endTime = colorEndTime_fire.Value;

            //火箭弹尾烟初始化
            rbs.psp_smoke.color_start = rbs.psp_smoke.color_end = colorSmoke_valueChanged(colorsmoke_menu.Value);
            rbs.psp_smoke.lifetime = lifetime_smoke.Value;
            rbs.psp_smoke.angle = angle_smoke.Value;
            rbs.psp_smoke.size = size_smoke.Value;
            rbs.psp_smoke.size_start = sizeStart_smoke.Value;
            rbs.psp_smoke.size_end = sizeEnd_smoke.Value;
        }

        public Color colorSmoke_valueChanged(int value)
        {
            color_smoke = value;
            if (value == 0)
            {
                return Color.gray;
            }
            else if (value == 1)
            {
                return Color.white;
            }
            else
            {
                return Color.black;
            }
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
        public override void Rocket_Launch()
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
        public float lifetime = 260;

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

    public class ParticleSystemComponent : MonoBehaviour
    {
        public MSlider test;

        void Start()
        {

            //test = AddSlider("test", "TEST", 0, 0, 1);
            test = new MSlider("test","test",0,0,1);
            test.DisplayInMapper = true;
            Debug.Log("!!|" );
            test.ValueChanged += new ValueChangeHandler(valueChanged);
        }

        public bool HasSliders(BlockBehaviour block)
        {
            return block.MapperTypes.Exists(match => match.Key == "TEST");
        }

        public void valueChanged(float value)
        {

        }
    }
}
