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

        public RocketScript rocketScript;

        #region 基本功能变量声明

        public MKey launch_key;

        MSlider thrustForce_slider;

        MSlider thrustTime_slider;

        MSlider thrustDelay_slider;

        MSlider DragForce_slider;

        //MSlider colliderDelay_slider;

        #endregion

        public override void SafeAwake()
        {
           

            #region 基本功能参数初始化

            launch_key = AddKey(LanguageManager.Instance.CurrentLanguage.launch, "Launch", KeyCode.L);
            launch_key.KeysChanged += () => { changedPropertise(); };

            thrustForce_slider = AddSlider(LanguageManager.Instance.CurrentLanguage.thrustForce, "Thrust Force", 1, 0f, 10f);
            thrustForce_slider.ValueChanged += (value) => { changedPropertise(); };

            thrustTime_slider = AddSlider(LanguageManager.Instance.CurrentLanguage.thrustTime, "Thrust Time", 1, 0f, 10f);
            thrustTime_slider.ValueChanged += (value) => { changedPropertise(); };

            DragForce_slider = AddSlider(LanguageManager.Instance.CurrentLanguage.drag, "DRAG", 0.5f, 0.2f, 3f);
            DragForce_slider.ValueChanged += (value) => { changedPropertise(); };

            thrustDelay_slider = AddSlider(LanguageManager.Instance.CurrentLanguage.thrustDelay, "Thrust Delay", 0, 0f, 10f);
            thrustDelay_slider.ValueChanged += (value) => { changedPropertise(); };

            //colliderDelay_slider = AddSlider("碰撞开启 0.05s", "Collider Enable", 0f, 0f, 0.5f);
            //colliderDelay_slider.ValueChanged += (value) => { changedPropertise(); };

            #endregion

            initRocketScript();

            changedPropertise();

            //DisplayInMapper(0);

        }

        void initRocketScript()
        {
            Rigidbody rigidbody = Rigidbody;
            rigidbody.centerOfMass += Vector3.forward * 0.5f;
            rocketScript = GetComponent<RocketScript>() ?? gameObject.AddComponent<RocketScript>();
            rocketScript.ThrustDirection = Vector3.right;
            rocketScript.ThrustPoint = rigidbody.centerOfMass;
            rocketScript.DelayEnableCollisionTime = 0.02f;
            rocketScript.ExplodePower = 1f;
            rocketScript.ExplodeRadius = 10f;
            rocketScript.effectOffset = new Vector3(-1.4f, 0, 0.5f);
            rocketScript.OnExplodeFinal += () => { Destroy(rocketScript.transform.gameObject); };
        }

        void changedPropertise()
        {
            rocketScript.ThrustForce = thrustForce_slider.Value;
            rocketScript.ThrustTime = thrustTime_slider.Value * 10;
            rocketScript.DelayLaunchTime = thrustDelay_slider.Value * 0.1f;
            rocketScript.DragClamp = DragForce_slider.Value;  
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
