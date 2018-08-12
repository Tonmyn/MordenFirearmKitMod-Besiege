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

        public bool ThrustSwitch;

        public bool isThrusted;

        public Rigidbody rigidbody;

        public Action OnThrustEvent;
        public Action OnThrustingEvent;
        public Action OnThrustedEvent;

        private CountDownScript thrust_CountDown;

        void Awake()
        {
            thrust_CountDown = gameObject.AddComponent<CountDownScript>();

            rigidbody = GetComponent<Rigidbody>();

            ThrustSwitch = false;
            isThrusted = false;
        }

        void Start()
        {

            thrust_CountDown.Time = ThrustTime;
            thrust_CountDown.CountDownCompleteEvent +=  OnThrustedEvent;
            OnThrustEvent += () => { };
            OnThrustingEvent += () => { };
            OnThrustedEvent += () => { };

        }

        public void FixedUpdate()
        {
            if (ThrustSwitch && !isThrusted)
            {

                thrust_CountDown.TimeSwitch = true;

                if (isThrusted == false)
                {
                    isThrusted = true;
                    OnThrustEvent();
                }
               
            }

            if (thrust_CountDown.TimeSwitch )
            {
                OnThrustingEvent();

                thrustDirection = transform.TransformDirection(ThrustDirection);
                thrustPoint = transform.TransformPoint(ThrustPoint);

                rigidbody.AddForceAtPosition(thrustDirection.normalized * ThrustForce, thrustPoint, ForceMode.VelocityChange);

            }

        }



    }
}
