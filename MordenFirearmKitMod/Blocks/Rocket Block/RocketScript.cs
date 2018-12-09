using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;
using System.Collections;

namespace ModernFirearmKitMod
{
    public class RocketScript : MonoBehaviour
    {
        private DragScript drager;

        private ExplodeScript exploder;

        private Rigidbody rigidbody;

        public float DragClamp; 
        public float ThrustForce;
        public float ThrustTime;
        public float DelayLaunchTime;
        public float DelayEnableCollisionTime;
        public Vector3 ThrustDirection;
        public Vector3 ThrustPoint;

        public bool LaunchEnabled { get; set; } = false;
        public bool Launched { get { return isLaunched; } }
        private bool isLaunched = false;

        public GameObject fire_ParticleObject;

        public RocketFireScript fireScripter;

        public GameObject smoke_ParticleObject;

        public RocketSmokeScript smokeScripter;

        void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();        
            rigidbody.drag = rigidbody.angularDrag = 0;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            initPhysical();

            initParticle();

        }

        void initPhysical()
        {
            ThrustDirection = transform.TransformDirection(ThrustDirection);

            drager = GetComponent<DragScript>() ?? gameObject.AddComponent<DragScript>();
            drager.DragAxis = new Vector3(0, -1, -1);
            drager.DragPoint = rigidbody.centerOfMass - transform.InverseTransformDirection(transform.right * 0.5f);
            drager.DragClamp = DragClamp;
            drager.enabled = false;

            exploder = GetComponent<ExplodeScript>() ?? gameObject.AddComponent<ExplodeScript>();
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
            fireScripter = fire_ParticleObject.GetComponent<RocketFireScript>() ?? fire_ParticleObject.AddComponent<RocketFireScript>();

            smoke_ParticleObject = new GameObject("Rocket Smoke Object");
            smoke_ParticleObject.transform.SetParent(transform);
            smoke_ParticleObject.transform.localPosition = new Vector3(-1.35f, 0, 0.5f);
            smoke_ParticleObject.transform.localRotation = Quaternion.AngleAxis(-90f, Vector3.up);
            smokeScripter = smoke_ParticleObject.GetComponent<RocketSmokeScript>() ?? smoke_ParticleObject.AddComponent<RocketSmokeScript>();
        }

        void Update()
        {
            if (LaunchEnabled)
            {
                StartCoroutine(Launch());
            }
            else
            {
                StopCoroutine(Launch());
            }
        }


        void OnCollisionEnter(Collision collision)
        {
            if (rigidbody.detectCollisions)
            {
                //exploder.Explodey();

                //var v = gameObject.AddComponent<ExplodeOnCollide>()/*.OnExplode(5000,300,500,transform.position,5,0)*/;
                //v.power = 5000;
                //v.torquePower = 1000;
                //v.upPower = 1000;
                //v.radius = 10;
                //v.parentObj = transform;
                //v.OnExplode(5000, 300, 500, transform.position, 5, 0);
                
                ////v.Explodey();


                Debug.Log("boom");
            }
        }

        private IEnumerator Launch()
        {
            yield return new WaitForSeconds(DelayLaunchTime);
          
            if (GetComponent<ConfigurableJoint>() != null)
            {
                ConfigurableJoint configurableJoint = GetComponent<ConfigurableJoint>();
                configurableJoint.breakForce = configurableJoint.breakTorque = 0;
                rigidbody.WakeUp();
            }
            
            smokeScripter.EmitSwitch = true;
            fireScripter.EmitSwitch = true;
            isLaunched = drager.enabled = true;
            rigidbody.AddForceAtPosition(transform.TransformDirection(ThrustDirection).normalized * ThrustForce, transform.TransformPoint(ThrustPoint));
            yield return new WaitForSeconds(ThrustTime);
            smokeScripter.EmitSwitch = false;
            fireScripter.EmitSwitch = false;
            LaunchEnabled = false;         
        }

    }
}
