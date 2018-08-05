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

        public ThrustScript thruster;

        public Rigidbody rigidbody;

        void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();

            initThrustrt();


        }


        void initThrustrt()
        {
            thruster = gameObject.AddComponent<ThrustScript>();
            
           
        }


        void FixedUpdate()
        {
            if (thruster.isThrusted)
            {
                thruster.ThrustDirection = transform.right;
                thruster.ThrustPoint = transform.TransformDirection(new Vector3(1f, rigidbody.centerOfMass.y, rigidbody.centerOfMass.z)) + transform.position; ;
            }
           

        }
    }
}
