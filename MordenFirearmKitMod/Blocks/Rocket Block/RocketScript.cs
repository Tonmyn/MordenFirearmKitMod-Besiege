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

        public bool isExplode { get { return exploder.isExplodey; } }
        public float ExplodePower;
        public float ExplodeRadius;
        public Action OnExplode,OnExploded,OnExplodeFinal;


        //public GameObject fire_ParticleObject;

        //public RocketFireScript fireScripter;

        //public GameObject smoke_ParticleObject;

        //public RocketSmokeScript smokeScripter;

        public GameObject effect;
        public Vector3 effectOffset;

        private bool EnableCollision = false;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            rigidbody.drag = rigidbody.angularDrag = 0;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            initPhysical();

            if (StatMaster.levelSimulating)
            {
                initParticle();
            }    
        }

        void initPhysical()
        {
            //ThrustDirection = transform.TransformDirection(ThrustDirection);
            //ThrustPoint = transform.TransformPoint(ThrustPoint);

            drager = GetComponent<DragScript>() ?? gameObject.AddComponent<DragScript>();
            drager.DragAxis = new Vector3(0, -1, -1);
            drager.DragPoint = rigidbody.centerOfMass - transform.InverseTransformDirection(transform.right * 0.5f);
            drager.DragClamp = DragClamp;
            drager.enabled = false;

            exploder = GetComponent<ExplodeScript>() ?? gameObject.AddComponent<ExplodeScript>();
            exploder.Power = ExplodePower;
            exploder.Radius = ExplodeRadius;         
            exploder.ExplosionType = ExplodeScript.explosionType.炸弹;
            exploder.OnExplode += Explode;
            exploder.OnExploded += Exploded;
            exploder.OnExplodeFinal += ExplodeFinal;

           
        }

        void initParticle()
        {
            //fire_ParticleObject = fire_ParticleObject ?? new GameObject("Rocket Fire Object");
            //fire_ParticleObject.transform.SetParent(transform);
            //fire_ParticleObject.transform.localPosition = new Vector3(-1.35f, 0, 0.5f);
            //fire_ParticleObject.transform.localRotation = Quaternion.AngleAxis(-90f, Vector3.up);
            //fireScripter = fire_ParticleObject.GetComponent<RocketFireScript>() ?? fire_ParticleObject.AddComponent<RocketFireScript>();

            //smoke_ParticleObject = smoke_ParticleObject ?? new GameObject("Rocket Smoke Object");
            //smoke_ParticleObject.transform.SetParent(transform);
            //smoke_ParticleObject.transform.localPosition = new Vector3(-1.35f, 0, 0.5f);
            //smoke_ParticleObject.transform.localRotation = Quaternion.AngleAxis(-90f, Vector3.up);
            //smokeScripter = smoke_ParticleObject.GetComponent<RocketSmokeScript>() ?? smoke_ParticleObject.AddComponent<RocketSmokeScript>();

            effect = (GameObject)Instantiate(AssetManager.Instance.Rocket.rocketTrailEffect);
            effect.transform.SetParent(transform);
            effect.transform.position = transform.position;
            effect.transform.rotation = transform.rotation;
            effect.transform.localPosition = effectOffset;
            effect.SetActive(false);
            //effect.GetComponent<ParticleSystem>().Stop();
          
        }

        void Update()
        {
            if (LaunchEnabled)
            {
                StartCoroutine(Launch());
                if (isLaunched && !effect.activeSelf)
                {
                    effect.SetActive(true);
                }
            }
            else
            {
                StopCoroutine(Launch());
            }
        }


        void OnCollisionEnter(Collision collision)
        {
            if (EnableCollision)
            {
                rigidbody.isKinematic = true;

                exploder.Position = transform.position;
                exploder.Explodey();

                gameObject.GetComponentInChildren<CapsuleCollider>().isTrigger = true;
                gameObject.GetComponentsInChildren<MeshRenderer>().ToList().Find(match => match.name == "Vis").enabled = false;
            }
        }

        private IEnumerator Launch()
        {
            StartCoroutine(CollisionTimer(DelayEnableCollisionTime));
            yield return new WaitForSeconds(DelayLaunchTime);

                if (GetComponent<ConfigurableJoint>() != null)
                {
                    ConfigurableJoint configurableJoint = GetComponent<ConfigurableJoint>();
                    configurableJoint.breakForce = configurableJoint.breakTorque = 0;
                    rigidbody.WakeUp();
                }

                //smokeScripter.EmitSwitch = !exploder.isExplodey;
                //fireScripter.EmitSwitch = !exploder.isExplodey;
                isLaunched = drager.enabled = !exploder.isExplodey;

            rigidbody.AddForceAtPosition(transform.TransformDirection(ThrustDirection).normalized * ThrustForce * 300f, transform.TransformPoint(ThrustPoint));
            yield return new WaitForSeconds(ThrustTime);
            //smokeScripter.EmitSwitch = false;
            //fireScripter.EmitSwitch = false;
            LaunchEnabled = false;         
        }
        private IEnumerator CollisionTimer(float time)
        {
            if (!EnableCollision)
            {
                yield return new WaitForSeconds(time);
                EnableCollision = true;
                StopCoroutine("CollisionTimer");
            }
        }

        void Explode()
        {           
            //effect.GetComponent<Light>().enabled = false;
            effect.GetComponentInChildren<ParticleSystem>().Stop();
            OnExplode?.Invoke();
        }
        void Exploded() { OnExploded?.Invoke(); }
        void ExplodeFinal()
        {
            gameObject.SetActive(false);
            OnExplodeFinal?.Invoke();
        }


        public void Reusing(float thrustForce,float thrustTime,float dragClamp)
        {
            gameObject.GetComponentInChildren<CapsuleCollider>().isTrigger = false;
            gameObject.GetComponentsInChildren<MeshRenderer>().ToList().Find(match => match.name == "Vis").enabled = true;

            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
            rigidbody.detectCollisions = false;
            rigidbody.isKinematic = true;

            LaunchEnabled = false;

            ThrustForce = thrustForce;
            ThrustTime = thrustTime;
            DragClamp = dragClamp;
            effect.SetActive(false);

            exploder.isExplodey = false;
        }
    }
}
