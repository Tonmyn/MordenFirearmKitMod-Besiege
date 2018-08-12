using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;


namespace ModernFirearmKitMod
{
    class RocketBlockScript :BlockScript
    {
        MMenu functionPage_menu;

        Rigidbody rigidbody;

        RocketScript rocketScript;

        #region 基本功能变量声明

        MKey launch_key;

        MSlider thrustForce_slider;

        MSlider thrustTime_slider;

        MSlider thrustDelay_slider;

        MSlider DragForce_slider;

        MSlider colliderDelay_slider;

        #endregion

        #region 尾焰变量 声明

        //声明 尾焰粒子组件
        //protected GameObject particle_fire = new GameObject("尾焰粒子组件");

        //声明 尾焰粒子系统
        //protected ParticleSystem ps_fire;

        //声明 尾焰粒子渲染器
        //protected ParticleSystemRenderer psr_fire;

        //声明 尾焰粒子属性
        //public ParticleSystemProperties psp_fire = new ParticleSystemProperties().init_fire();

        //声明 滑条 粒子存活时间
        MSlider lifetime_fire;

        //声明 滑条 半径
        MSlider radius_fire;

        //声明 滑条 角度
        MSlider angle_fire;

        //声明 滑条 尺寸
        MSlider size_fire;

        //声明 滑条 初始尺寸
        MSlider sizeStart_fire;

        //声明 滑条 结束尺寸
        MSlider sizeEnd_fire;

        MColourSlider colorStart_fire;

        MColourSlider colorEnd_fire;

        MSlider colorStartTime_fire;

        MSlider colorEndTime_fire;

        #endregion

        public override void SafeAwake()
        {
            

            #region 控件初始化

            functionPage_menu = AddMenu("Function Page Menu", 0, new List<string> { "火箭参数", "尾焰参数", "尾烟参数" });
            functionPage_menu.ValueChanged += (value) => { DisplayInMapper(value); };

            #region 基本功能参数初始化

            launch_key = AddKey("发射", "Launch", KeyCode.L);

            thrustForce_slider = AddSlider("推力大小", "Thrust Force", 1, 0f, 10f);
            thrustForce_slider.ValueChanged += (value) => { changedPropertise(); };

            thrustTime_slider = AddSlider("推力时间 1s", "Thrust Time", 1, 0f, 10f);
            thrustTime_slider.ValueChanged += (value) => { changedPropertise(); };

            DragForce_slider = AddSlider("阻力大小", "DRAG", 0.5f, 0.2f, 3f);
            DragForce_slider.ValueChanged += (value) => { changedPropertise(); };

            thrustDelay_slider = AddSlider("延迟发射 0.1s", "Thrust Delay", 0, 0f, 10f);
            thrustDelay_slider.ValueChanged += (value) => { changedPropertise(); };

            colliderDelay_slider = AddSlider("碰撞开启 0.05s", "Collider Enable", 0f, 0f, 0.5f);
            colliderDelay_slider.ValueChanged += (value) => { changedPropertise(); };

            #endregion

            #region 尾焰组件初始化

            lifetime_fire = AddSlider("时间", "LifeTimeFire", 0.5f /** transform.localScale.x*/, 0, 10);
            lifetime_fire.ValueChanged += (value) => { changedPropertise(); };

            radius_fire = AddSlider("半径", "RadiusFire", 0f, 0, 2); 
            radius_fire.ValueChanged += (value) => { changedPropertise(); };

            angle_fire = AddSlider("角度", "AngleFire", 2f, 0, 60);
            angle_fire.ValueChanged += (value) => { changedPropertise(); };

            size_fire = AddSlider("尺寸", "SizeFire", 0.5f, 0, 5);
            size_fire.ValueChanged += (value) => { changedPropertise(); };

            sizeStart_fire = AddSlider("初始尺寸", "SizeStartFire", 1f, 0, 5);
            sizeStart_fire.ValueChanged += (value) => { changedPropertise(); };

            sizeEnd_fire = AddSlider("结束尺寸", "SizeEndFire", 0f, 0, 5);
            sizeEnd_fire.ValueChanged += (value) => { changedPropertise(); };

            colorStart_fire = AddColourSlider("渐变初始颜色", "ColorStartFire", Color.blue, false);
            colorStart_fire.ValueChanged += (value) => { changedPropertise(); };

            colorEnd_fire = AddColourSlider("渐变结束颜色", "ColorEndFire", Color.yellow, false);
            colorEnd_fire.ValueChanged += (value) => { changedPropertise(); };

            colorStartTime_fire = AddSlider("渐变初始时间", "ColorStartTimeFire", 0f, 0, 0.5f * transform.localScale.x);
            colorStartTime_fire.ValueChanged += (value) => { changedPropertise(); };

            colorEndTime_fire = AddSlider("渐变结束时间", "ColorEndTimeFire", 0.25f , 0, 0.5f * transform.localScale.x);
            colorEndTime_fire.ValueChanged += (value) => { changedPropertise(); };

            #endregion

            #endregion

            initRocketScript();

            changedPropertise();

            DisplayInMapper(0);
        }

        void initRocketScript()
        {

            rigidbody = GetComponent<Rigidbody>();
            rigidbody.centerOfMass += Vector3.forward * 0.5f;

            rocketScript = gameObject.AddComponent<RocketScript>();
          
        }

        void changedPropertise()
        {
            rocketScript.thrustDelay_CountDown.Time = thrustDelay_slider.Value * 500;
            rocketScript.allowCollisionsDelay_CountDown.Time = colliderDelay_slider.Value * 500;

            ThrustScript thruster = rocketScript.thruster;
            thruster.ThrustForce = thrustForce_slider.Value;
            thruster.ThrustTime = thrustTime_slider.Value * 1000;

            DragScript drager = rocketScript.drager;
            drager.DragClamp = DragForce_slider.Value;


            RocketFireScript fireScripter = rocketScript.fireScripter;
            fireScripter.LifeTime = lifetime_fire.Value;
            fireScripter.Radius = radius_fire.Value;
            fireScripter.Angle = angle_fire.Value;
            fireScripter.Size = (transform.localScale.y + transform.localScale.z) / 2 * size_fire.Value;
            fireScripter.StartSize = sizeStart_fire.Value;
            fireScripter.EndSize = sizeEnd_fire.Value;
            fireScripter.StartColor = colorStart_fire.Value;
            fireScripter.EndColor = colorEnd_fire.Value;
            fireScripter.ColorStartTime = colorStartTime_fire.Value;
            fireScripter.ColorEndTime = colorEndTime_fire.Value * transform.localScale.x;



        }

        void DisplayInMapper(int value)
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

            launch_key.DisplayInMapper = show_0;
            //explosiontype_menu.DisplayInMapper = show_0;
            //power_slider.DisplayInMapper = show_0;
            colliderDelay_slider.DisplayInMapper = show_0;
            thrustDelay_slider.DisplayInMapper = show_0;           
            thrustForce_slider.DisplayInMapper = show_0;   
            thrustTime_slider.DisplayInMapper = show_0;
            DragForce_slider.DisplayInMapper = show_0;

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
        }

        public override void SimulateFixedUpdateHost()
        {
            if (launch_key.IsPressed)
            {
                rocketScript.launched = true;
            }                     
        }

    }
}
