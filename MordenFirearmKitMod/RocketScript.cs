using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;

namespace ModernFirearmKitMod
{
    public class RocketScript : MonoBehaviour
    {
        public GameObject thrustObject;

        public ThrustScript thruster;

        public Rigidbody rigidbody;

        public CountDownScript delayAllowCollisionsCountDown;

        public float delayAllowCollisionsTime;

        public bool allowCollision;

        public bool launched;

        public GameObject trailParticle;

        public ParticleSystem trailParticleSystem;

        void Awake()
        {
            launched = false;
            allowCollision = false;

            rigidbody = GetComponent<Rigidbody>();

            thruster = gameObject.AddComponent<ThrustScript>();
            thruster.ThrustDirection = Vector3.right;
            thruster.ThrustPoint = transform.InverseTransformPoint(new Vector3(0, 0.5f, 0) + rigidbody.centerOfMass + transform.position);

            delayAllowCollisionsCountDown = gameObject.AddComponent<CountDownScript>();
            delayAllowCollisionsCountDown.CountDownCompleteEvent += () => { allowCollision = true; };

        }

        void Update()
        {
            if (launched && !thruster.isThrusted)
            {
                thruster.ThrustSwitch = true;
            }
            else if (thruster.isThrusted && !delayAllowCollisionsCountDown.TimeSwitch)
            {
                delayAllowCollisionsCountDown.TimeSwitch = true;
            }
        }

        void FixedUpdate()
        {

        }

        void OnCollisionEnter(Collision collision)
        {
            if (allowCollision)
            {

            }
        }
    }
}
