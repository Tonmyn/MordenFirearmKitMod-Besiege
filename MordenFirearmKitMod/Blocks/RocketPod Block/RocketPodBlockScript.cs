using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;
using System.Collections;

namespace ModernFirearmKitMod
{
    class RocketPodBlockScript : LauncherBlockScript
    {
        //public MMenu functionPage_menu;

        public static GameObject RocketTemp;

        #region 功能变量声明

        public override float Rate { get; set; }
        public override float KnockBack { get; set; }
        public override int BulletCurrentNumber { get; set; }
        public override int BulletMaxNumber { get; set; }
        public override GameObject BulletObject { get; set; }
        public override Vector3 SpawnPoint { get; set; }
        public override bool LaunchEnable { get; set; }

        #endregion

        //声明 滑条 载弹数量
        private MSlider number_slider;
        //声明 滑条 连发间隔
        private MSlider rate_slider;

        MSlider thrustForce_slider;

        MSlider thrustTime_slider;

        //MSlider thrustDelay_slider;

        MSlider DragForce_slider;

        #region 尾焰变量 声明

        ////声明 滑条 粒子存活时间
        //MSlider lifetime_fire;

        ////声明 滑条 半径
        //MSlider radius_fire;

        ////声明 滑条 角度
        //MSlider angle_fire;

        ////声明 滑条 尺寸
        //MSlider size_fire;

        ////声明 滑条 初始尺寸
        //MSlider sizeStart_fire;

        ////声明 滑条 结束尺寸
        //MSlider sizeEnd_fire;

        //MColourSlider colorStart_fire;

        //MColourSlider colorEnd_fire;

        //MSlider colorStartTime_fire;

        //MSlider colorEndTime_fire;

        #endregion

        #region 尾烟变量 声明

        ////声明 菜单 尾烟颜色
        //protected MMenu colorsmoke_menu;

        ////声明 滑条 粒子存活时间
        //protected MSlider lifetime_smoke;

        ////声明 滑条 角度
        //protected MSlider angle_smoke;

        ////声明 滑条 尺寸
        //protected MSlider size_smoke;

        ////声明 滑条 初始尺寸
        //protected MSlider sizeStart_smoke;

        ////声明 滑条 结束尺寸
        //protected MSlider sizeEnd_smoke;

        //public int color_smoke = 0;

        #endregion



        #region 内部变量声明

        private GameObject RockectPool;

        //声明 火箭弹
        private GameObject[] Rockets = new GameObject[18];

        //声明 火箭弹标签
        //private int Label = 0;

        //声明 火箭弹实例化位置
        private Vector3[] relativePositions;


        #endregion

        public override void SafeAwake()
        {

            //functionPage_menu = AddMenu("Function Page Menu", 0, new List<string> { "火箭巢参数", "尾焰参数", "尾烟参数" });
            //functionPage_menu.ValueChanged += (value) => { DisplayInMapper(value); };

            LaunchKey = AddKey("发射", "Launch", KeyCode.L);

            KnockBack = 0f;
            SpawnPoint = new Vector3(0, 0, 3.5f);
            BulletObject = RocketTemp;
            
            BulletCurrentNumber = BulletMaxNumber = 18;
            Rate = 2f;
            LaunchEnable = false;

            LaunchEvent += delayLaunch;

            relativePositions = GetRelativePositions();

            #region 基本功能参数初始化

            //添加 滑条 参数
            number_slider = AddSlider("载弹数量", "Number", BulletMaxNumber, 1, 18);
            number_slider.ValueChanged += (value) => { BulletMaxNumber = (int)value; };
            rate_slider = AddSlider("射速 0.1s", "Rate", Rate, 1f, 5f);
            rate_slider.ValueChanged += (value)=> { Rate = value * 0.1f; };

            thrustForce_slider = AddSlider("推力大小", "Thrust Force", 1, 0f, 10f);
            thrustForce_slider.ValueChanged += (value) => { changedPropertise(); };

            thrustTime_slider = AddSlider("推力时间 10s", "Thrust Time", 1, 0f, 10f);
            thrustTime_slider.ValueChanged += (value) => { changedPropertise(); };

            DragForce_slider = AddSlider("阻力大小", "DRAG", 0.5f, 0.2f, 3f);
            DragForce_slider.ValueChanged += (value) => { changedPropertise(); };

            #endregion

            #region 尾焰组件初始化

            //lifetime_fire = AddSlider("时间", "LifeTimeFire", 0.5f /** transform.localScale.x*/, 0, 10);
            //lifetime_fire.ValueChanged += (value) => { changedPropertise(); };

            //radius_fire = AddSlider("半径", "RadiusFire", 0f, 0, 2);
            //radius_fire.ValueChanged += (value) => { changedPropertise(); };

            //angle_fire = AddSlider("角度", "AngleFire", 2f, 0, 60);
            //angle_fire.ValueChanged += (value) => { changedPropertise(); };

            //size_fire = AddSlider("尺寸", "SizeFire", 0.5f, 0, 5);
            //size_fire.ValueChanged += (value) => { changedPropertise(); };

            //sizeStart_fire = AddSlider("初始尺寸", "SizeStartFire", 1f, 0, 5);
            //sizeStart_fire.ValueChanged += (value) => { changedPropertise(); };

            //sizeEnd_fire = AddSlider("结束尺寸", "SizeEndFire", 0f, 0, 5);
            //sizeEnd_fire.ValueChanged += (value) => { changedPropertise(); };

            //colorStart_fire = AddColourSlider("渐变初始颜色", "ColorStartFire", Color.blue, false);
            //colorStart_fire.ValueChanged += (value) => { changedPropertise(); };

            //colorEnd_fire = AddColourSlider("渐变结束颜色", "ColorEndFire", Color.yellow, false);
            //colorEnd_fire.ValueChanged += (value) => { changedPropertise(); };

            //colorStartTime_fire = AddSlider("渐变初始时间", "ColorStartTimeFire", 0f, 0, 0.5f /** transform.localScale.x*/);
            //colorStartTime_fire.ValueChanged += (value) => { changedPropertise(); };

            //colorEndTime_fire = AddSlider("渐变结束时间", "ColorEndTimeFire", 0.25f, 0, 0.5f/* * transform.localScale.x*/);
            //colorEndTime_fire.ValueChanged += (value) => { changedPropertise(); };

            #endregion

            #region 尾烟组件初始化

            //colorsmoke_menu = AddMenu("尾烟颜色", color_smoke, new List<string> { "灰色", "白色", "黑色" });
            //colorsmoke_menu.ValueChanged += (value) => { changedPropertise(); };
            //lifetime_smoke = AddSlider("时间", "LifeTimeSmoke", 3f, 0, 5);
            //lifetime_smoke.ValueChanged += (value) => { changedPropertise(); };
            //angle_smoke = AddSlider("角度", "AngleSmoke", 15f, 0, 60);
            //angle_smoke.ValueChanged += (value) => { changedPropertise(); };
            //size_smoke = AddSlider("尺寸", "SizeSmoke", 1f, 0, 3);
            //size_smoke.ValueChanged += (value) => { changedPropertise(); };
            //sizeStart_smoke = AddSlider("初始尺寸", "SizeStartSmoke", 1f, 0, 1);
            //sizeStart_smoke.ValueChanged += (value) => { changedPropertise(); };
            //sizeEnd_smoke = AddSlider("结束尺寸", "SizeEndSmoke", 3f, 0, 10);
            //sizeEnd_smoke.ValueChanged += (value) => { changedPropertise(); };

            #endregion

            changedPropertise();

        }

        void changedPropertise()
        {


        }

        public void DisplayInMapper(int value)
        {
            bool show_0 = true/*, show_1, show_2*/;


            //if ()
            //{
            //show_0 = "火箭巢参数" == functionPage_menu.Items[value];
            //show_1 = "尾焰参数" == functionPage_menu.Items[value];
            //show_2 = "尾烟参数" == functionPage_menu.Items[value];
            //}
            //else if ()
            //{
            //    show_0 = false;
            //    show_1 = true;
            //    show_2 = false;
            //}
            //else if()
            //{
            //    show_0 = false;
            //    show_1 = false;
            //    show_2 = true;
            //}

            #region 页码0控件

            LaunchKey.DisplayInMapper = show_0;
            //explosiontype_menu.DisplayInMapper = show_0;
            //power_slider.DisplayInMapper = show_0;
            //colliderDelay_slider.DisplayInMapper = show_0;
            //thrustDelay_slider.DisplayInMapper = show_0;
            number_slider.DisplayInMapper = show_0;
            rate_slider.DisplayInMapper = show_0;
            thrustForce_slider.DisplayInMapper = show_0;
            thrustTime_slider.DisplayInMapper = show_0;
            DragForce_slider.DisplayInMapper = show_0;

            #endregion

            #region 页码1控件   

            ////toggle_fire.DisplayInMapper = show_1;

            //lifetime_fire.DisplayInMapper = show_1;

            //radius_fire.DisplayInMapper = show_1;

            //angle_fire.DisplayInMapper = show_1;

            //size_fire.DisplayInMapper = show_1;

            //sizeStart_fire.DisplayInMapper = show_1;

            //sizeEnd_fire.DisplayInMapper = show_1;

            //colorStart_fire.DisplayInMapper = show_1;

            //colorEnd_fire.DisplayInMapper = show_1;

            //colorStartTime_fire.DisplayInMapper = show_1;

            //colorEndTime_fire.DisplayInMapper = show_1;

            ////alphaStart_fire.DisplayInMapper = show_1;

            ////alphaEnd_fire.DisplayInMapper = show_1;

            ////alphaStartTime_fire.DisplayInMapper = show_1;

            ////alphaEndTime_fire.DisplayInMapper = show_1;

            #endregion

            #region 页码2控件   

            //colorsmoke_menu.DisplayInMapper = show_2;

            ////toggle_smoke.DisplayInMapper = show_2;

            //lifetime_smoke.DisplayInMapper = show_2;

            ////radius_smoke.DisplayInMapper = show_2;

            //angle_smoke.DisplayInMapper = show_2;

            //size_smoke.DisplayInMapper = show_2;

            //sizeStart_smoke.DisplayInMapper = show_2;

            //sizeEnd_smoke.DisplayInMapper = show_2;

            ////colorStart_smoke.DisplayInMapper = show_2;

            ////colorEnd_smoke.DisplayInMapper = show_2;

            ////colorStartTime_smoke.DisplayInMapper = show_2;

            ////colorEndTime_smoke.DisplayInMapper = show_2;

            ////alphaStart_smoke.DisplayInMapper = show_2;

            ////alphaEnd_smoke.DisplayInMapper = show_2;

            ////alphaStartTime_smoke.DisplayInMapper = show_2;

            ////alphaEndTime_smoke.DisplayInMapper = show_2;

            #endregion
        }

        public override void OnSimulateStart()
        {
            InitPool();

            BulletMaxNumber = Mathf.Clamp(BulletMaxNumber, 1, 18);
            //GetRelativePositions();

            Reload(true);

            void InitPool()
            {
                RockectPool = new GameObject("Rocket Pool");
                RockectPool.transform.SetParent(transform);
                RockectPool.transform.position = transform.position;
                RockectPool.transform.rotation = transform.rotation;
            }
        }

        public override void SimulateUpdateHost()
        {
            Reload();

            if (LaunchKey.IsDown )
            {
                if (!LaunchEnable&& RockectPool.transform.childCount > 0)
                {
                    LaunchEnable = true;
                    StartCoroutine(Launch(RockectPool.transform.GetChild(0).gameObject));
                }
            }
        }

        //火箭弹实例化位置计算
        private static Vector3[] GetRelativePositions()
        {
            int i;

            Vector3[] Positions = new Vector3[18];

            //声明 原点
            Vector2 origin = new Vector2(0.4f, 0);

            //声明 大圆半径、角度差和旋转角
            float radius_large = 0.37f, angle_large = 30f;

            //声明 小圆半径、角度差和旋转角
            float radius_little = 0.19f, angle_little = 60f;

            //外圈火箭弹位置
            for (i = 0; i < 12; i++)
            {
                Positions[i] = new Vector3(
                                                    0,
                                                    origin.y + radius_large * Mathf.Sin(angle_large * i * Mathf.Deg2Rad),
                                                    origin.x - radius_large * Mathf.Cos(angle_large * i * Mathf.Deg2Rad)
                                                 );
            }

            //内圈火箭弹位置
            for (i = 0; i < 6; i++)
            {
                Positions[i + 12] = new Vector3(
                                                        0,
                                                        origin.y + radius_little * Mathf.Sin((angle_little * i + 30) * Mathf.Deg2Rad),
                                                        origin.x - radius_little * Mathf.Cos((angle_little * i + 30) * Mathf.Deg2Rad)
                                                      );
            }

            return Positions;
        }
        //火箭弹发射世界位置
        private Vector3 getRealPosition(int label,Vector3 offset)
        {
            return transform.TransformVector(transform.InverseTransformVector(Rigidbody.position) + relativePositions[label] + offset);
        }

        //火箭弹实例化
        private void Rocket_Instantiate(int label)
        {

            //火箭弹安装位置 本地坐标转世界坐标
            Vector3 offset = new Vector3(-0.375f, 0f, 0.15f);
            Vector3 pos = getRealPosition(label, offset);

            //火箭弹实例化
            GameObject Rocket = (GameObject)Instantiate(RocketTemp, pos, RockectPool.transform.rotation,RockectPool.transform);
            Rocket.name = "Rocket " + label;
            Rocket.SetActive(true);
            Rockets[label] = Rocket;
            //Rocket.transform.localScale = new Vector3(1f, 0.5f, 0.5f);

            //火箭弹刚体 不开启碰撞 不受物理影响
            Rigidbody rigidbody = Rocket.GetComponent<Rigidbody>();
            rigidbody.detectCollisions = false;
            rigidbody.isKinematic = true;
            //火箭弹脚本 初始化
            RocketScript rocketScript = Rocket.GetComponent<RocketScript>();
            rocketScript.ThrustDirection = Vector3.right;
            rocketScript.ThrustPoint = rigidbody.centerOfMass;
            rocketScript.ThrustForce = thrustForce_slider.Value;
            rocketScript.ThrustTime = thrustTime_slider.Value*10f;
            rocketScript.DelayLaunchTime = 0f;
            rocketScript.DelayEnableCollisionTime = 0.02f;
            rocketScript.ExplodePower = 1f;
            rocketScript.ExplodeRadius = 10f;
            rocketScript.DragClamp = DragForce_slider.Value;

            rocketScript.effectOffset = new Vector3(-1.15f, 0, 0f);
            rocketScript.OnExplode += () => { Destroy(rocketScript.effect); };
            rocketScript.OnExplodeFinal += () => { Destroy(rocketScript.transform.gameObject); };

            //RocketFireScript fireScripter = rocketScript.fireScripter;
            //fireScripter.LifeTime = lifetime_fire.Value;
            //fireScripter.Radius = radius_fire.Value;
            //fireScripter.Angle = angle_fire.Value;
            //fireScripter.Size = (transform.localScale.y + transform.localScale.z) / 2 * size_fire.Value;
            //fireScripter.StartSize = sizeStart_fire.Value;
            //fireScripter.EndSize = sizeEnd_fire.Value;
            //fireScripter.StartColor = colorStart_fire.Value;
            //fireScripter.EndColor = colorEnd_fire.Value;
            //fireScripter.ColorStartTime = colorStartTime_fire.Value;
            //fireScripter.ColorEndTime = colorEndTime_fire.Value * transform.localScale.x;

            //RocketSmokeScript smokeScript = rocketScript.smokeScripter;
            //smokeScript.StartColor = smokeScript.EndColor = getColor(colorsmoke_menu.Value);
            //smokeScript.LifeTime = lifetime_smoke.Value;
            //smokeScript.Angle = angle_smoke.Value;
            //smokeScript.Size = size_smoke.Value * (transform.localScale.y + transform.localScale.z) / 3;
            //smokeScript.StartSize = sizeStart_smoke.Value;
            //smokeScript.EndSize = sizeEnd_smoke.Value;

            //Color getColor(int index)
            //{
            //    List<string> colorList = new List<string> { "灰色", "白色", "黑色" };

            //    Color color;
            //    switch (index)
            //    {
            //        case 0: color = Color.gray; break;
            //        case 1: color = Color.white; break;
            //        default: color = Color.black; break;
            //    }

            //    return color;
            //}         

            //effect = (GameObject)Instantiate(AssetManager.Instance.Rocket.rocketTrailEffect);
            //effect.transform.SetParent(Rocket.transform);
            //effect.transform.position = Rocket.transform.position;
            //effect.transform.rotation = Rocket.transform.rotation;
            //effect.transform.localPosition = new Vector3(-1.4f, 0, 0f);
            //effect.SetActive(false);

        }

        private void delayLaunch(GameObject gameObject)
        {          
            gameObject.transform.localPosition += Vector3.right * 3.25f;
            gameObject.transform.SetParent(transform.parent.transform);
            RocketScript rocketScript = gameObject.GetComponent<RocketScript>();
            rocketScript.LaunchEnabled = true;        
            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;

            StartCoroutine(delay());
            IEnumerator delay()
            {
                yield return new WaitForSeconds(rocketScript.DelayEnableCollisionTime);
                rigidbody.detectCollisions = true;
                gameObject.GetComponentInChildren<CapsuleCollider>().isTrigger = false;
                yield break;
            }
        }   
        //火箭弹重装
        public override void Reload(bool constraint = false)
        {
            //火箭弹在无限弹药模式下 有空位的情况下实例化新火箭弹
            if (StatMaster.GodTools.InfiniteAmmoMode || constraint)
            {
                for (int i = 0; i < BulletMaxNumber; i++)
                {
                    if (!Rockets[i] || Rockets[i].GetComponent<RocketScript>().Launched)
                    {
                        Rocket_Instantiate(i);
                        BulletCurrentNumber = Mathf.Clamp(BulletCurrentNumber + 1, 1, BulletMaxNumber);
                    }
                }
            }
        }

       


        internal static void CreateRocketBlockTemp()
        {
            RocketTemp = new GameObject("Rocket Temp");
            RocketTemp.transform.localScale = new Vector3(1f, 0.75f, 0.75f);

            GameObject vis = new GameObject("Vis");
            vis.transform.SetParent(RocketTemp.transform);
            vis.transform.localPosition -= RocketTemp.transform.right;
            vis.transform.localScale = RocketTemp.transform.localScale;
            vis.AddComponent<MeshFilter>().mesh = ModResource.GetMesh("Rocket Mesh");
            vis.AddComponent<MeshRenderer>().material.mainTexture = ModResource.GetTexture("Rocket Texture");

            GameObject collider = new GameObject("Collider");
            collider.transform.SetParent(RocketTemp.transform);
            collider.transform.localScale = RocketTemp.transform.localScale;
            CapsuleCollider capsuleCollider = collider.AddComponent<CapsuleCollider>();            
            capsuleCollider.radius = 0.15f;
            capsuleCollider.height = 2.5f;
            capsuleCollider.direction = 0;
            capsuleCollider.isTrigger = true;

            Rigidbody rigidbody = RocketTemp.AddComponent<Rigidbody>();
            rigidbody.mass = 0.25f;

            RocketTemp.AddComponent<RocketScript>();
            RocketTemp.AddComponent<DestroyIfEditMode>();
            RocketTemp.SetActive(false); 


        }

    }
}
