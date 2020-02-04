using Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ModernFirearmKitMod.GenericScript.RayGun
{

    public class RayBulletScript : MonoBehaviour
    {
        //public float Strength { get; set; }
        //public Vector3 Velocity { get; set; }
        //public float Drag { get; set; } = 0.1f;
        //public float Mass { get; set; } = 0.1f;
        //public Vector3 GravityAcceleration { get; } = new Vector3(0, -23f, 0);

        public BulletPropertise bulletPropertise { get; set; } = new BulletPropertise();
        public Transform gunbodyTransform;
        public Guid Guid  = Guid.NewGuid();
        public class BulletPropertise
        {
            public float Strength { get; set; } = 0f;
            public Vector3 Velocity { get; set; } = Vector3.zero;
            public float Drag { get; set; } = 0.1f;
            public float Mass { get; set; } = 0.1f;
            public Vector3 GravityAcceleration { get; } = new Vector3(0, -0.23f, 0);
            public Vector3 orginPosition { get; set; } = Vector3.zero;
            public Vector3 direction { get; set; } = Vector3.forward;
            public Color color { get; set; } = Color.yellow;
        }

        public bool isCollision { get; private set; } = false;

        public event Action<RaycastHit> OnCollisionEvent;

        public static MessageType ImpactMessage = ModNetworking.CreateMessageType(DataType.String,DataType.String,DataType.Vector3,DataType.Vector3);    

        //public Vector3 orginPosition;
        //public Vector3 direction;
        //public Color color = Color.yellow;

        private Vector3 sPoint;
        private Vector3 ePoint;
        private RaycastHit hitInfo;
        private LineRenderer lineRenderer;
        private float _time;

        //private LineRenderer lr;

        private void Start()
        {
            bulletPropertise.direction = transform.InverseTransformDirection(bulletPropertise.direction);
            bulletPropertise.Velocity = Vector3.Project(bulletPropertise.Velocity, bulletPropertise.direction).magnitude * bulletPropertise.direction;

            sPoint = ePoint = bulletPropertise.orginPosition + bulletPropertise.Velocity * Time.deltaTime * 1.5f;

            bulletPropertise.Velocity += bulletPropertise.direction * bulletPropertise.Strength * bulletPropertise.Mass * 600f;

            lineRenderer = GetComponent<LineRenderer>() ?? gameObject.AddComponent<LineRenderer>();
            lineRenderer.material.shader = Shader.Find("Particles/Additive");
            lineRenderer.material.SetColor("_TintColor", bulletPropertise.color);
            lineRenderer.SetPosition(0, sPoint);
            lineRenderer.SetPosition(1, ePoint);
            lineRenderer.SetWidth(0.15f, 0.2f);
            lineRenderer.useWorldSpace = true;
            lineRenderer.enabled = true;

            var diem = gameObject.GetComponent<DestroyIfEditMode>() ?? gameObject.AddComponent<DestroyIfEditMode>();
            OnCollisionEvent += onCollision;

            //var go = new GameObject("test");
            //lr = go.AddComponent<LineRenderer>();
            //go.AddComponent<DestroyIfEditMode>();

            //lr.material.color = Color.blue;
            //lr.SetPosition(0, sPoint);
            //lr.SetPosition(1, ePoint);
        }

        private void Update()
        {
            if (!isCollision)
            {
                if (Time.timeScale == 0f) return;

                _time = Time.smoothDeltaTime / Time.timeScale;
                Vector3 gravityVelocity = (!StatMaster.GodTools.GravityDisabled) ? (bulletPropertise.GravityAcceleration) : Vector3.zero;
                Vector3 dragVelocity = -(bulletPropertise.Velocity.normalized * bulletPropertise.Drag);
                bulletPropertise.Velocity += gravityVelocity + dragVelocity;
                ePoint = sPoint + bulletPropertise.Velocity * _time;

                lineRenderer.SetPosition(0, sPoint);

                if (Physics.Raycast(sPoint, bulletPropertise.Velocity, out hitInfo, (sPoint - ePoint).magnitude) )
                {
                    if (hitInfo.transform == gunbodyTransform && gunbodyTransform != null)
                    {
                        lineRenderer.SetPosition(1, ePoint);
                        sPoint = ePoint;
                    }
                    else
                    {
                        var targetType = "stone";

                        lineRenderer.SetPosition(1, hitInfo.point);
                        isCollision = true;

                        if (!StatMaster.isClient)
                        {
                            try
                            {
                                OnCollisionEvent?.Invoke(hitInfo);
                                targetType = createImpactEffect();
                            }
                            catch { }

                            var message = ImpactMessage.CreateMessage(Guid.ToString(), targetType, hitInfo.point, hitInfo.normal);
                            ModNetworking.SendToAll(message);
                        }
                        //else
                        //{
                        //    Debug.Log("client " + Guid);
                        //}
                        //var go = new GameObject("test");
                        //go.AddComponent<DestroyIfEditMode>();
                        //var lr = go.AddComponent<LineRenderer>();
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
                        //lr.SetPosition(1, bulletPropertise.direction + hitInfo.point);
                    }               
                }
                else
                {

                    lineRenderer.SetPosition(1, ePoint /*+ Vector3.Project(bulletPropertise.Velocity, bulletPropertise.direction).magnitude * bulletPropertise.direction * _time * 0.5f*/);
                    sPoint = ePoint;
                }

                //bulletPropertise.direction = -(sPoint - ePoint).normalized;
            }
            else
            {
                lineRenderer.enabled = false;
                Destroy(gameObject);
            }

           
        }

        string createImpactEffect()
        {
            string targetType = "stone";
            GameObject impact;
            if (isWoodenBlock(hitInfo.transform))
            {
                impact = (GameObject)Instantiate(AssetManager.Instance.Bullet.impactWoodEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                targetType = "wooden";
            }
            else if (isMetalBlock(hitInfo.transform))
            {
                impact = (GameObject)Instantiate(AssetManager.Instance.Bullet.impactMetalEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                targetType = "metal";
            }
            else
            {
                impact = (GameObject)Instantiate(AssetManager.Instance.Bullet.impactStoneEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                targetType = "stone";
            }

            if (hitInfo.rigidbody != null)
            {
                impact.transform.SetParent(hitInfo.transform);
            }

            var tsd = impact.AddComponent<TimedSelfDestruct>();
            //tsd.OnDestruct += () => { Destroy(impact); };
            tsd.lifeTime = 50f;
            tsd.Switch = true;

            return targetType;
        }

       void createImpactEffect_Networking(string targetType,Vector3 point,Vector3 normal)
        {
            GameObject impact;
            if (targetType == "wooden")
            {
                impact = (GameObject)Instantiate(AssetManager.Instance.Bullet.impactWoodEffect, point, Quaternion.LookRotation(normal));
            }
            else if (targetType == "metal")
            {
                impact = (GameObject)Instantiate(AssetManager.Instance.Bullet.impactMetalEffect, point, Quaternion.LookRotation(normal));
            }
            else
            {
                impact = (GameObject)Instantiate(AssetManager.Instance.Bullet.impactStoneEffect, point, Quaternion.LookRotation(normal));
            }

            //if (hitInfo.rigidbody != null)
            //{
            //    impact.transform.SetParent(hitInfo.transform);
            //}
            //impact.AddComponent<DestroyIfEditMode>();
            var tsd = impact.AddComponent<TimedSelfDestruct>();
            //tsd.OnDestruct += () => { Destroy(impact); };
            tsd.lifeTime = 50f;
            tsd.Switch = true;
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
                    var f = (bulletPropertise.Velocity * bulletPropertise.Mass) / _time;

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

        public static GameObject CreateBullet(float strength,Vector3 spawnPoint,Vector3 direction,Vector3 velocity,float mass,float drag ,Color color, Transform gunbody = null, Action action = null)
        {
            var bullet = new GameObject("Bullet");
            //var mct = GameObject.Find("Main Camera").transform;
            //bullet.transform.SetParent(mct);
            //bullet.transform.position = mct.position + mct.forward * 3f;
            //bullet.transform.localScale = Vector3.one * 0.001f;
            bullet.transform.SetParent(gunbody);

            var bs = bullet.AddComponent<RayBulletScript>();
            bs.bulletPropertise.Strength = strength;
            bs.bulletPropertise.orginPosition = spawnPoint;
            bs.bulletPropertise.direction = direction;
            bs.bulletPropertise.Velocity = velocity;
            bs.bulletPropertise.Mass = mass;
            bs.bulletPropertise.Drag = drag;
            bs.bulletPropertise.color = color;

            bs.gunbodyTransform = gunbody;

            action?.Invoke();

            return bullet;
        }
        public static GameObject CreateBullet(BulletPropertise bulletPropertise, Transform gunbody = null, Action action = null)
        {
            return CreateBullet(bulletPropertise.Strength, bulletPropertise.orginPosition, bulletPropertise.direction, bulletPropertise.Velocity, bulletPropertise.Mass, bulletPropertise.Drag, bulletPropertise.color, gunbody, action);
        }

        public static void ImpactNetworkingEvent(Message message)
        {
            if (StatMaster.isClient)
            {
                var guid = new Guid((string)message.GetData(0));
                var targetType = ((string)message.GetData(1));
                var point = (Vector3)message.GetData(2);
                var normal = (Vector3)message.GetData(3);
                try
                {
                    RayBulletScript rbs = GameObject.FindObjectsOfType<RayBulletScript>().ToList().Find(match => match.Guid == guid);
                    rbs.lineRenderer.enabled = false;
                    rbs.createImpactEffect_Networking(targetType, point, normal);
                }
                catch
                {
                    
                }
            }
        }
    }

}

