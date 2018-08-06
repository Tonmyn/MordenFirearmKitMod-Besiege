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

        public Vector3 ThrustPoint { set; get; }

        private Vector3 thrustPoint;

        public Vector3 ThrustDirection { set; get; }

        private Vector3 thrustDirection;

        public float ThrustForce;
   
        public float ThrustTime;

        public float ThrustDelayTime;

        public bool ThrustSwitch;

        public bool isThrusted;

        public Rigidbody rigidbody;

        public Action OnThrustEvent;

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

            ThrustSwitch = false;
            isThrusted = false;

        }

        void Start()
        {
            thrustDelayCountDown.Time = ThrustDelayTime;
            thrustDelayCountDown.CountDownCompleteEvent += () => { thrustCountDown.TimeSwitch = true; };

            thrustCountDown.Time = ThrustTime;
            thrustCountDown.CountDownCompleteEvent += () => { };


        }

        public void FixedUpdate()
        {
            if (ThrustSwitch && !isThrusted)
            {

                thrustDelayCountDown.TimeSwitch = true;

                if (isThrusted == false)
                {
                    isThrusted = true;
                    OnThrustEvent();
                }
               
            }

            if (thrustCountDown.TimeSwitch )
            {
                thrustDirection = transform.TransformDirection(ThrustDirection);
                thrustPoint = transform.TransformPoint(ThrustPoint);

                rigidbody.AddForceAtPosition(thrustDirection.normalized * ThrustForce, thrustPoint, ForceMode.VelocityChange);

            }

        }



    }
}
