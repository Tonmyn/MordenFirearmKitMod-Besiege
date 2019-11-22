﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;
using System.Collections;
using Modding.Blocks;

namespace ModernFirearmKitMod
{
    public class RocketScript : MonoBehaviour
    {
        private DragScript drager;
        private ExplodeScript exploder;
        private Rigidbody rigidbody;
        private BlockHealthBar healthBar;

        public float DragClamp; 
        public float ThrustForce;
        public float ThrustTime;
        public float DelayLaunchTime;
        public float DelayEnableCollisionTime;
        public Vector3 ThrustDirection;
        public Vector3 ThrustPoint;

        public  Guid Guid = Guid.NewGuid();
        public bool LaunchEnabled { get; set; } = false;
        public bool Launched { get { return isLaunched; } }
        private bool isLaunched = false;

        public bool isExplode { get { return exploder.isExplodey; } }
        public float ExplodePower;
        public float ExplodeRadius;
        public event Action OnExplode,OnExploded,OnExplodeFinal;

        public GameObject effect;
        public Vector3 effectOffset;

        #region Network
        /// <summary>RocketScriptGuid,</summary>
        public static MessageType ExplodeMessage = ModNetworking.CreateMessageType(DataType.String);
        #endregion

        private bool EnableCollision = false;
        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            rigidbody.drag = rigidbody.angularDrag = 0;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            healthBar = GetComponent<BlockHealthBar>();

            initPhysical();

            if (StatMaster.levelSimulating)
            {
                initParticle();
            }

            Debug.Log(Guid);
        }

        void initPhysical()
        {
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
            effect = (GameObject)Instantiate(AssetManager.Instance.Rocket.rocketTrailEffect);
            effect.transform.SetParent(transform);
            effect.transform.position = transform.position;
            effect.transform.rotation = transform.rotation;
            effect.transform.localPosition = effectOffset;
            effect.SetActive(false);   
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

            if (healthBar.health <= 0 && isExplode == false)
            {
                Explody();
            }                                 
        }

        void FixedUpdate()
        {
            if (!StatMaster.isClient)
            {
                if (LaunchEnabled)
                {
                    rigidbody.AddForce(transform.TransformDirection(ThrustDirection).normalized * ThrustForce * 300f);
                }
            }     
        }

        void OnCollisionEnter(Collision collision)
        {
            if (!StatMaster.isClient)
            {
                if (EnableCollision)
                {
                    Explody();
                }
            }
        }

        private IEnumerator Launch()
        {
            if (isLaunched && !effect.activeSelf)
            {
                effect.SetActive(true);
            }


            if (!EnableCollision)
            {
                StartCoroutine(CollisionTimer(DelayEnableCollisionTime));
            }

            yield return new WaitForSeconds(DelayLaunchTime);


            if (GetComponent<ConfigurableJoint>() != null)
            {
                ConfigurableJoint configurableJoint = GetComponent<ConfigurableJoint>();
                configurableJoint.breakForce = configurableJoint.breakTorque = 0;
                rigidbody.WakeUp();
            }
            isLaunched = drager.enabled = !exploder.isExplodey;


            yield return new WaitForSeconds(ThrustTime);
            LaunchEnabled = false;
            effect.SetActive(false);

            IEnumerator CollisionTimer(float time)
            {
                yield return new WaitForSeconds(time);
                EnableCollision = true;
                StopCoroutine("CollisionTimer");
            }
        }
        private void Explode_Network()
        {
            Debug.Log("Explode network");
        }

        public void Explody()
        {
            if (StatMaster.isClient)
            {
                Debug.Log("explody");
            }

            if (!StatMaster.isClient)
            {
                //var message = ExplodeMessage.CreateMessage(Guid.ToString());
                var message = ExplodeMessage.CreateMessage(GetComponent<RocketBlockScript>().Guid.ToString());
                ModNetworking.SendToAll(message);

                rigidbody.isKinematic = true;

                exploder.Position = transform.position;
                exploder.Explodey();

                healthBar.health = 0;

                gameObject.GetComponentInChildren<CapsuleCollider>().isTrigger = true;
                gameObject.GetComponentsInChildren<MeshRenderer>().ToList().Find(match => match.name == "Vis").enabled = false;
            }   
        }

        private void Explode()
        {
            //effect.GetComponent<Light>().enabled = false;
            effect.GetComponentInChildren<ParticleSystem>().Stop();
            OnExplode?.Invoke();

            //if (!StatMaster.isClient)
            //{
            //    var message = ExplodeMessage.CreateMessage(Guid.ToString());
            //    ModNetworking.SendToAll(message);


            //}
        }
        private void Exploded(Collider[] colliders)
        {
            OnExploded?.Invoke();
        }
        private void ExplodeFinal()
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

        public static void ExplodeNetworkingEvent(Message message)
        {
            if (StatMaster.isClient)
            {
                var guid = new Guid(((string)message.GetData(0)));
                Debug.Log("RocketScript " + guid);
                try
                {
                    foreach (var rs in GameObject.FindObjectsOfType<RocketScript>().ToList())
                    {
                        Debug.Log(rs.Guid);
                    }
                 
                    RocketScript rocketScript = GameObject.FindObjectsOfType<RocketScript>().ToList().Find(match => match.Guid == guid);

                    rocketScript.Explode_Network();
                }
                catch { }

                try
                {
                    foreach (var rs in GameObject.FindObjectsOfType<RocketBlockScript>().ToList())
                    {
                        Debug.Log(rs.Guid);
                    }
                    RocketScript rocketScript = GameObject.FindObjectsOfType<RocketBlockScript>().ToList().Find(match => match.rocketScript.Guid == guid).rocketScript;

                    rocketScript.Explode_Network();
                }
                catch { }
            }
        }
    }
}
