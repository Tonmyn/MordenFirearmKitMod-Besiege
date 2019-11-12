using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ModernFirearmKitMod.GenericScript.RayGun
{




    public class RayBulletScript : MonoBehaviour
    {
        public float Strength { get; set; }
        public Vector3 Velocity { get; set; }
        public float Drag { get; set; } = 0.1f;
        public float Mass { get; set; } = 0.1f;
        public Vector3 GravityAcceleration { get; } = new Vector3(0, -23f, 0);

        public bool isCollision { get; private set; } = false;

        public event Action<RaycastHit> OnCollisionEvent;

        public Vector3 orginPosition;
        public Vector3 direction;
        public Color color = Color.yellow;

        private Vector3 sPoint;
        private Vector3 ePoint;
        private RaycastHit hitInfo;
        private LineRenderer lineRenderer;
        private float _time;

        private void Start()
        {
            Velocity = transform.InverseTransformDirection(direction) * Strength * Mass * 600f + Velocity;

            sPoint = ePoint = orginPosition;

            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material.shader = Shader.Find("Particles/Additive");
            lineRenderer.material.SetColor("_TintColor", color);
            lineRenderer.SetPosition(0, sPoint);
            lineRenderer.SetPosition(1, ePoint);
            lineRenderer.SetWidth(0.15f, 0.2f);
            lineRenderer.enabled = true;

            gameObject.AddComponent<DestroyIfEditMode>();
            OnCollisionEvent += onCollision;
        }
        private void Update()
        {
            if (!isCollision)
            {
                if (Time.timeScale == 0f) return;

                _time = Time.smoothDeltaTime / Time.timeScale;
                Vector3 gravityVelocity = (!StatMaster.GodTools.GravityDisabled) ? (GravityAcceleration * _time) : Vector3.zero;
                Vector3 dragVelocity = (-(direction * Drag) / Mass) * _time;
                Velocity = Velocity + gravityVelocity + dragVelocity;
                ePoint = sPoint + Velocity * _time;
                direction = -(sPoint - ePoint).normalized;
                lineRenderer.SetPosition(0, sPoint);

                if (Physics.Raycast(sPoint, direction, out hitInfo, (sPoint - ePoint).magnitude))
                {
                    lineRenderer.SetPosition(1, hitInfo.point);
                    OnCollisionEvent?.Invoke(hitInfo);
                    isCollision = true;

                    //var go = new GameObject("test");
                    //go.AddComponent<DestroyIfEditMode>();
                    //var lr =go.AddComponent<LineRenderer>();
                    //lr.material.color = Color.red;
                    //lr.SetWidth(0.2f, 0.2f);
                    //lr.SetPosition(0, hitInfo.point);
                    //lr.SetPosition(1, hitInfo.normal + hitInfo.point);

                    //go = new GameObject("test");
                    //go.AddComponent<DestroyIfEditMode>();
                    //lr = go.AddComponent<LineRenderer>();
                    //lr.material.color = Color.red;
                    //lr.SetWidth(0.2f, 0.2f);
                    //lr.SetPosition(0, hitInfo.point);
                    //lr.SetPosition(1, direction + hitInfo.point);
                   
                    try
                    {
                        createImpactEffect();
                    }
                    catch { }                 
                }
                else
                {
                    lineRenderer.SetPosition(1, ePoint);
                    sPoint = ePoint;
                }
            }
            else
            {
                lineRenderer.enabled = false;
                Destroy(gameObject);
            }

            void createImpactEffect()
            {
                GameObject impact;
                if (isWoodenBlock(hitInfo.transform))
                {
                    impact = (GameObject)Instantiate(AssetManager.Instance.Bullet.impactWoodEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));                  
                }
                else if (isMetalBlock(hitInfo.transform))
                {
                    impact = (GameObject)Instantiate(AssetManager.Instance.Bullet.impactMetalEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));                 
                }
                else 
                {
                    impact = (GameObject)Instantiate(AssetManager.Instance.Bullet.impactStoneEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                }

                if (hitInfo.rigidbody != null)
                {               
                    impact.transform.parent = hitInfo.transform;
                } 

                var tsd = impact.AddComponent<TimedSelfDestruct>();
                tsd.OnDestruct += () => { Destroy(impact); };
                tsd.lifeTime = 50f;
                tsd.Switch = true;
            }
        }

        private void onCollision(RaycastHit hitInfo)
        {
            try
            {
                if (hitInfo.rigidbody != null)
                {
                    //MV = mv ;v = MV/m; I=Ft; F = I/t;
                    var v = (Velocity * Mass) / (hitInfo.rigidbody.mass);
                    var f = v / _time;
                    if (hitInfo.rigidbody.isKinematic == false)
                    {


                        if (hitInfo.rigidbody.gameObject.GetComponent<BlockBehaviour>() != null 
                            && isWoodenBlock((BlockType)hitInfo.rigidbody.gameObject.GetComponent<BlockBehaviour>().BlockID) 
                            || hitInfo.transform.name.ToLower().Contains("tree"))
                        {
                            hitInfo.rigidbody.AddForceAtPosition(f * 10f, hitInfo.point);
                            hitInfo.rigidbody.AddRelativeTorque(f * 10f, ForceMode.Impulse);
                        }        
                        else
                        {
                            if (hitInfo.rigidbody.gameObject.GetComponent<KillingHandler>() != null)
                            {
                                var kh = hitInfo.rigidbody.gameObject.GetComponent<KillingHandler>();
                                //kh.SendMessage("OnCollisionEnter", hitInfo.rigidbody.GetComponentInChildren<Collision>());
                                kh.KillUnit(true, InjuryType.Blunt);
                                Debug.Log("kill");
                            }


                            hitInfo.rigidbody.AddForceAtPosition(f, hitInfo.point);
                            hitInfo.rigidbody.AddRelativeTorque(f, ForceMode.Impulse);

                            if (hitInfo.rigidbody.gameObject.GetComponent<ConfigurableJoint>() != null)
                            {
                                var cj = hitInfo.rigidbody.gameObject.GetComponent<ConfigurableJoint>();
                                cj.breakForce -= f.magnitude;
                                cj.breakTorque -= f.magnitude;
                            }
                        }
                    }
                    else
                    {
                        if (hitInfo.rigidbody.gameObject.GetComponent<BreakOnForce>() != null)
                        {
                            var bof = hitInfo.rigidbody.gameObject.GetComponent<BreakOnForce>();
                            bof.BreakExplosion(f.magnitude, hitInfo.point, bof.breakForceRadius, 0f);
                        }

                        if (hitInfo.rigidbody.gameObject.GetComponent<DestroyOnTriggerEnter>() != null)
                        {
                            var dote = hitInfo.rigidbody.gameObject.GetComponent<DestroyOnTriggerEnter>();
                            dote.SendMessage("OnTriggerEnter", hitInfo.collider);
                        }

                        if (hitInfo.rigidbody.gameObject.GetComponent<ParticleOnCollide>() != null)
                        {
                            var poc = hitInfo.rigidbody.gameObject.GetComponent<ParticleOnCollide>();
                            poc.SendMessage("OnCollisionEnter", hitInfo.collider.GetComponentInChildren<Collision>());
                        }

                        if (hitInfo.rigidbody.gameObject.GetComponent<ParticleOnTrigger>() != null)
                        {
                            var pot = hitInfo.rigidbody.gameObject.GetComponent<ParticleOnTrigger>();
                            pot.SendMessage("OnTriggerEnter", hitInfo.collider);                      
                        }
                    }
                }
                else
                {
                    
                }
            }
            catch(Exception e)
            {
                Debug.Log(e.Message);
            }

          
        }


        private bool isWoodenBlock(BlockType blockType) 
        {
            bool value = false;

            if (blockType == BlockType.Log ||
                blockType == BlockType.WoodenPole || 
                blockType == BlockType.WoodenPanel || 
                blockType == BlockType.SingleWoodenBlock || 
                blockType == BlockType.DoubleWoodenBlock ||               
                blockType == BlockType.Wheel||
                blockType == BlockType.SmallWheel||
                blockType == BlockType.LargeWheel||
                blockType == BlockType.WheelUnpowered||
                blockType == BlockType.LargeWheelUnpowered||
                blockType == BlockType.Slider||
                blockType == BlockType.Propeller||
                blockType == BlockType.SmallPropeller||
                blockType == BlockType.Unused3||
                blockType == BlockType.Wing||
                blockType == BlockType.WingPanel

                )
            {
                value = true;
                return value;
            }
            else
            {
                return value;
            }
        }
        private bool isWoodenBlock(Transform transform)
        {
            var value = false;
            if (transform.gameObject.GetComponent<BlockBehaviour>() != null || transform.name.ToLower().Contains("tree"))
            {
                if (transform.gameObject.GetComponent<BlockBehaviour>().fireTag != null)
                {
                    value = true;
                }
            }
            return value;
        }
        private bool isMetalBlock(Transform transform)
        {
            var value = false;
            if (transform.GetComponent<BlockBehaviour>()!= null && transform.GetComponent<BlockBehaviour>().fireTag == null)
            {
                value = true;
            }
            return value;
        }

    }

}

