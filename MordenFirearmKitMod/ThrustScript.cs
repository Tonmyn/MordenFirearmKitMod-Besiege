using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;

namespace ModernFirearmKitMod
{
    public class ThrustScript : MonoBehaviour
    {

        public Vector3 ThrustPoint;

        public Vector3 ThrustDirection;

        public float ThrustForce;
   
        public float ThrustTime;

        public float ThrustDelayTime;

        public bool ThrustSwitch;

        public bool isThrusted;

        public Rigidbody rigidbody;

        private CountDownScript thrustDelayCountDown;

        private CountDownScript thrustCountDown;

        void Awake()
        {
            GameObject delayCD = new GameObject("Thrust Delay Count Down Object");
            delayCD.transform.SetParent(transform);
            thrustDelayCountDown = delayCD.AddComponent<CountDownScript>();

            GameObject thrustCD = new GameObject("Trust Time Count Down Object");
            thrustCD.transform.SetParent(transform);
            thrustCountDown = thrustCD.AddComponent<CountDownScript>();
            rigidbody = GetComponent<Rigidbody>();
            ThrustPoint = Vector3.zero;
            ThrustSwitch = false;
            isThrusted = false;
        }

        void Start()
        {
            thrustDelayCountDown.Time = ThrustDelayTime;
            thrustDelayCountDown.CountDownComplete += () => { thrustCountDown.TimeSwitch = true; BesiegeConsoleController.ShowMessage("delay"); };

            thrustCountDown.Time = ThrustTime;
            thrustCountDown.CountDownComplete += () => {  BesiegeConsoleController.ShowMessage("finish"); };


        }

        public void FixedUpdate()
        {
            if (ThrustSwitch && !isThrusted)
            {

                thrustDelayCountDown.TimeSwitch = true;

                if (isThrusted == false)
                {
                    isThrusted = true;
                }
               
            }

            if (thrustCountDown.TimeSwitch )
            {
                rigidbody.AddForceAtPosition(ThrustDirection.normalized * ThrustForce, ThrustPoint, ForceMode.VelocityChange);       
            }

        }



    }
}
