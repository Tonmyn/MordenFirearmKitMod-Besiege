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
            lineRenderer.useWorldSpace = true;
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
                    impact.transform.SetParent(hitInfo.transform);
                }

                var tsd = impact.AddComponent<TimedSelfDestruct>();
                tsd.OnDestruct += () => { Destroy(impact); };
                tsd.lifeTime = 50f;
                tsd.Switch = true;
            }
        }

        private delegate void ActionIfHaveComponent(RaycastHit raycastHit, Vector3 vector3);

        private Dictionary<Type, ActionIfHaveComponent> action_Kinematic = new Dictionary<Type, ActionIfHaveComponent>()
        {
            {typeof(ConfigurableJoint),(hitInfo,f)=>{var cj = hitInfo.rigidbody.gameObject.GetComponent<ConfigurableJoint>();cj.breakForce -= f.magnitude;cj.breakTorque -= f.magnitude; } },
            {typeof(KillingHandler),(hitInfo,f)=>{var kh = hitInfo.rigidbody.gameObject.GetComponent<KillingHandler>();kh.KillUnit(true, InjuryType.Sharp);} },
            {typeof(ExplodeOnCollide),(hitInfo,f)=>{var eoc = hitInfo.rigidbody.gameObject.GetComponent<ExplodeOnCollide>(); eoc.Explodey(); } },
            {typeof(GibOnImpact),(hitInfo,f)=>{var goi = hitInfo.rigidbody.gameObject.GetComponent<GibOnImpact>(); goi.Gib(); } },

        };
        private Dictionary<Type, ActionIfHaveComponent> action_Unkinematic = new Dictionary<Type,ActionIfHaveComponent>()
        {
             {typeof(BreakOnForce),(hitInfo,f)=>{ var bof = hitInfo.rigidbody.gameObject.GetComponent<BreakOnForce>();bof.BreakExplosion(f.magnitude, hitInfo.point, bof.breakForceRadius, 0f); } },
             {typeof(DestroyOnTriggerEnter),(hitInfo,f)=>{var dote = hitInfo.rigidbody.gameObject.GetComponent<DestroyOnTriggerEnter>();dote.SendMessage("OnTriggerEnter", hitInfo.collider); } },
             {typeof(ParticleOnCollide),(hitInfo,f)=>{ var poc = hitInfo.rigidbody.gameObject.GetComponent<ParticleOnCollide>();poc.SendMessage("OnCollisionEnter", hitInfo.collider.GetComponentInChildren<Collision>());} },
             {typeof(ParticleOnTrigger),(hitInfo,f)=>{ var pot = hitInfo.rigidbody.gameObject.GetComponent<ParticleOnTrigger>();pot.SendMessage("OnTriggerEnter", hitInfo.collider); } },
        };
        private void onCollision(RaycastHit hitInfo)
        {
            try
            {
                if (hitInfo.rigidbody != null)
                {
                    //∵MV = mv; I=Ft; F = I/t; I=Δp; Δp = mv
                    //∴F=mv/t
                    var f = (Velocity * Mass) / _time;

                    if (hitInfo.rigidbody.isKinematic == false)
                    {
                        if (hitInfo.rigidbody.gameObject.GetComponent<BlockBehaviour>() != null
                            && isWoodenBlock((BlockType)hitInfo.rigidbody.gameObject.GetComponent<BlockBehaviour>().BlockID)
                            || hitInfo.transform.name.ToLower().Contains("tree"))
                        {
                            hitInfo.rigidbody.AddForceAtPosition(f * 10f, hitInfo.point);
                        }
                        else
                        {
                            specialComponentsAction(action_Kinematic);
                            hitInfo.rigidbody.AddForceAtPosition(f , hitInfo.point);
                        }
                        Vector3 com = hitInfo.transform.TransformPoint(hitInfo.rigidbody.centerOfMass);
                        Vector3 vector3 = hitInfo.point - com;
                        Vector3 vector31 = f.normalized + hitInfo.point;
                        Vector3 normal = Vector3.Cross(vector3, vector31);
                        hitInfo.rigidbody.AddTorque(com + normal * f.magnitude * 0.008f);
                    }
                    else
                    {
                        specialComponentsAction(action_Unkinematic);
                    }

                    var bhb = hitInfo.rigidbody.gameObject.GetComponent<BlockHealthBar>();
                    if (bhb != null)
                    {
                        bhb.DamageBlock(f.magnitude * 0.001f);
                    }

                    void specialComponentsAction(Dictionary<Type, ActionIfHaveComponent> dic)
                    {
                        foreach (var com in dic.Keys)
                        {
                            if (hitInfo.rigidbody.gameObject.GetComponent(com) != null)
                            {
                                dic[com](hitInfo, f);
                                break;
                            }
                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception e)
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
                blockType == BlockType.Wheel ||
                blockType == BlockType.SmallWheel ||
                blockType == BlockType.LargeWheel ||
                blockType == BlockType.WheelUnpowered ||
                blockType == BlockType.LargeWheelUnpowered ||
                blockType == BlockType.Slider ||
                blockType == BlockType.Propeller ||
                blockType == BlockType.SmallPropeller ||
                blockType == BlockType.Unused3 ||
                blockType == BlockType.Wing ||
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
            if (transform.GetComponent<BlockBehaviour>() != null && transform.GetComponent<BlockBehaviour>().fireTag == null)
            {
                value = true;
            }
            return value;
        }

    }

}

