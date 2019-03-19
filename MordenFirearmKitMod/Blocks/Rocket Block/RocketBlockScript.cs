using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;
using System.ComponentModel;

namespace ModernFirearmKitMod
{
    class RocketBlockScript :BlockScript
    {
        //public MMenu functionPage_menu;

        public RocketScript rocketScript;

        #region 基本功能变量声明

        public MKey launch_key;

        MSlider thrustForce_slider;

        MSlider thrustTime_slider;

        MSlider thrustDelay_slider;

        MSlider DragForce_slider;

        //MSlider colliderDelay_slider;

        #endregion

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

        //GameObject effect;

        public override void SafeAwake()
        {
           
            #region 控件初始化

            //functionPage_menu = AddMenu("Function Page Menu", 0, new List<string> { "火箭参数","尾烟参数","尾烟参数"});
            //functionPage_menu.ValueChanged += (value) => { DisplayInMapper(value); };

            #region 基本功能参数初始化

            launch_key = AddKey("发射", "Launch", KeyCode.L);
            launch_key.KeysChanged += () => { changedPropertise(); };

            thrustForce_slider = AddSlider("推力大小", "Thrust Force", 1, 0f, 10f);
            thrustForce_slider.ValueChanged += (value) => { changedPropertise(); };

            thrustTime_slider = AddSlider("推力时间 10s", "Thrust Time", 1, 0f, 10f);
            thrustTime_slider.ValueChanged += (value) => { changedPropertise(); };

            DragForce_slider = AddSlider("阻力大小", "DRAG", 0.5f, 0.2f, 3f);
            DragForce_slider.ValueChanged += (value) => { changedPropertise(); };

            thrustDelay_slider = AddSlider("延迟发射 0.1s", "Thrust Delay", 0, 0f, 10f);
            thrustDelay_slider.ValueChanged += (value) => { changedPropertise(); };

            //colliderDelay_slider = AddSlider("碰撞开启 0.05s", "Collider Enable", 0f, 0f, 0.5f);
            //colliderDelay_slider.ValueChanged += (value) => { changedPropertise(); };

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

            //colorStartTime_fire = AddSlider("渐变初始时间", "ColorStartTimeFire", 0f, 0, 0.5f * transform.localScale.x);
            //colorStartTime_fire.ValueChanged += (value) => { changedPropertise(); };

            //colorEndTime_fire = AddSlider("渐变结束时间", "ColorEndTimeFire", 0.25f , 0, 0.5f * transform.localScale.x);
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

            #endregion

            initRocketScript();

            changedPropertise();

            DisplayInMapper(0);

        }

        void initRocketScript()
        {
            Rigidbody rigidbody = Rigidbody;
            rigidbody.centerOfMass += Vector3.forward * 0.5f;
            rocketScript = GetComponent<RocketScript>() ?? gameObject.AddComponent<RocketScript>();
            rocketScript.ThrustDirection = /*transform.InverseTransformDirection(Vector3.right)*/Vector3.right;
            rocketScript.ThrustPoint = rigidbody.centerOfMass;
            rocketScript.DelayEnableCollisionTime = 0.02f;
            rocketScript.ExplodePower = 1f;
            rocketScript.ExplodeRadius = 10f;
            rocketScript.effectOffset = new Vector3(-1.4f, 0, 0.5f);
            rocketScript.OnExplode += () => { Destroy( rocketScript.effect);  };
            rocketScript.OnExplodeFinal += () => { Destroy(rocketScript.transform.gameObject); };
        }

        void changedPropertise()
        {
            rocketScript.ThrustForce = thrustForce_slider.Value;
            rocketScript.ThrustTime = thrustTime_slider.Value * 10;
            rocketScript.DelayLaunchTime = thrustDelay_slider.Value * 0.1f;
            rocketScript.DragClamp = DragForce_slider.Value;

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
            //        case 0:color = Color.gray;break;
            //        case 1: color = Color.white; break;
            //        default: color = Color.black; break;
            //    }

            //    return color;
            //}
        }

        virtual public void DisplayInMapper(int value)
        {
            bool show_0 = true/*, show_1, show_2*/;

            //if ()
            //{
                //show_0 = "火箭参数" == functionPage_menu.Items[value];
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

            launch_key.DisplayInMapper = show_0;
            //explosiontype_menu.DisplayInMapper = show_0;
            //power_slider.DisplayInMapper = show_0;
            //colliderDelay_slider.DisplayInMapper = show_0;
            thrustDelay_slider.DisplayInMapper = show_0;           
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

        public override void SimulateUpdateHost()
        {
            if (launch_key.IsPressed)
            {
                rocketScript.LaunchEnabled = true;
            }         
        }

    }
}
