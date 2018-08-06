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
        
        MKey launch_key;

        MSlider thrustForce_slider;

        MSlider thrustTime_slider;

        MSlider thrustDelay_slider;

        MSlider colliderDelay_slider;

        RocketScript rocketScript;

        public override void SafeAwake()
        {
            launch_key = AddKey("发射", "Launch", KeyCode.L);
            thrustForce_slider = AddSlider("推力大小", "Thrust Force", 1, 0f, 10f);
            thrustForce_slider.ValueChanged += (value) => { changedPropertise(); };
            thrustTime_slider = AddSlider("推力时间 1s", "Thrust Time", 1, 0f, 10f);
            thrustTime_slider.ValueChanged += (value) => { changedPropertise(); };
            thrustDelay_slider = AddSlider("延迟发射 0.1s", "Thrust Delay", 0, 0f, 10f);
            thrustDelay_slider.ValueChanged += (value) => { changedPropertise(); };
            colliderDelay_slider = AddSlider("碰撞开启 0.05s", "Collider Enable", 1f, 1f, 5f);
            colliderDelay_slider.ValueChanged += (value) => { changedPropertise(); };

            initRocketScript();

        }

        void initRocketScript()
        {
            rocketScript = gameObject.AddComponent<RocketScript>();

            changedPropertise();
        }

        void changedPropertise()
        {
            ThrustScript thruster = rocketScript.thruster;
            thruster.ThrustForce = thrustForce_slider.Value;
            thruster.ThrustTime = thrustTime_slider.Value * 1000;
            thruster.ThrustDelayTime = thrustDelay_slider.Value * 500;

            //rocketScript.delayDetectCollisionsTime = colliderDelay_slider.Value*100;
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
