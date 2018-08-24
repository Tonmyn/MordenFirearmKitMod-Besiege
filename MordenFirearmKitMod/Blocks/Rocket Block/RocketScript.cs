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
        //public GameObject thrustObject;

        public ThrustScript thruster;

        public DragScript drager;

        public ExplodeScript exploder;

        public Rigidbody rigidbody;

        public ConfigurableJoint configurableJoint;

        public CountDownScript thrustDelay_CountDown;

        public CountDownScript allowCollisionsDelay_CountDown;

        public bool allowCollision;

        public bool launched;

        public GameObject fire_ParticleObject;

        public RocketFireScript fireScripter;

        public GameObject smoke_ParticleObject;

        public RocketSmokeScript smokeScripter;

        void Awake()
        {

            launched = false;
            allowCollision = false;
            rigidbody = GetComponent<Rigidbody>();
            rigidbody.drag = rigidbody.angularDrag = 0;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    
            initPhysical();

            initParticle();

        }

        void initPhysical()
        {
            thruster = gameObject.AddComponent<ThrustScript>();
            thruster.ThrustDirection = Vector3.right;
            thruster.ThrustPoint = rigidbody.centerOfMass;
            thruster.OnThrustedEvent += () => { fireScripter.EmitSwitch = false; smokeScripter.EmitSwitch = false; };

            GameObject delayCD = new GameObject("Delay Thrust Count Down Object");
            delayCD.transform.SetParent(transform);
            thrustDelay_CountDown = delayCD.AddComponent<CountDownScript>();
            thrustDelay_CountDown.CountDownCompleteEvent += () => 
            {
                thruster.ThrustSwitch = drager.enabled = true;
                fireScripter.EmitSwitch = true;
                smokeScripter.EmitSwitch = true;
                if (GetComponent<ConfigurableJoint>() != null)
                {
                    configurableJoint = GetComponent<ConfigurableJoint>();
                    configurableJoint.breakForce = configurableJoint.breakTorque = 0;
                }
                
            };

            GameObject delayAC = new GameObject("Delay Allow Collider Count Down Object");
            delayAC.transform.SetParent(transform);
            allowCollisionsDelay_CountDown = delayAC.AddComponent<CountDownScript>();
            allowCollisionsDelay_CountDown.CountDownCompleteEvent += () => { allowCollision = true; };

            drager = gameObject.AddComponent<DragScript>();
            drager.DragAxis = new Vector3(0, -1, -1);
            drager.DragPoint = rigidbody.centerOfMass - transform.InverseTransformDirection(transform.right * 0.5f);
            drager.enabled = false;

            exploder = gameObject.AddComponent<ExplodeScript>();
            exploder.Power = 1;
            exploder.Radius = 5;
            exploder.ExplosionType = ExplodeScript.explosionType.炸弹;

        }

        void initParticle()
        {
            fire_ParticleObject = new GameObject("Rocket Fire Object");
            fire_ParticleObject.transform.SetParent(transform);
            fire_ParticleObject.transform.localPosition = new Vector3(-1.35f, 0, 0.5f);
            fire_ParticleObject.transform.localRotation = Quaternion.AngleAxis(-90f, Vector3.up);
            fireScripter = fire_ParticleObject.AddComponent<RocketFireScript>();

            smoke_ParticleObject = new GameObject("Rocket Smoke Object");
            smoke_ParticleObject.transform.SetParent(transform);
            smoke_ParticleObject.transform.localPosition = new Vector3(-1.35f, 0, 0.5f);
            smoke_ParticleObject.transform.localRotation = Quaternion.AngleAxis(-90f, Vector3.up);
            smokeScripter = smoke_ParticleObject.AddComponent<RocketSmokeScript>();
        }

        void Update()
        {
            if (launched && !thruster.isThrusted)
            {
                thrustDelay_CountDown.TimeSwitch = true;
            }
            else if (thruster.isThrusted && !allowCollisionsDelay_CountDown.TimeSwitch)
            {
                allowCollisionsDelay_CountDown.TimeSwitch = true;
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (allowCollision)
            {
                exploder.Explodey();

                var v = gameObject.AddComponent<ExplodeOnCollide>()/*.OnExplode(5000,300,500,transform.position,5,0)*/;
                v.power = 5000;
                v.torquePower = 1000;
                v.upPower = 1000;
                v.radius = 10;
                v.parentObj = transform;
                v.OnExplode(5000, 300, 500, transform.position, 5, 0);
                
                //v.Explodey();


                Debug.Log("boom");
            }
        }
    }
}
