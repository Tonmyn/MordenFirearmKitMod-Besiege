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
                    if ((hitInfo.transform == gunbodyTransform && gunbodyTransform != null))
                    {
                        shootingNothing();
                    }
                    else
                    {
                        shootingSomething();
                    }               
                }
                else
                {
                    shootingNothing();
                }
            }
            else
            {
                lineRenderer.enabled = false;
                Destroy(gameObject);
            }

            void shootingSomething()
            {
                var targetType = "stone";

                lineRenderer.SetPosition(1, hitInfo.point);
                isCollision = true;

                if (!StatMaster.isClient)
                {
                    OnCollisionEvent?.Invoke(hitInfo);
                    targetType = createImpactEffect();

                    var message = ImpactMessage.CreateMessage(Guid.ToString(), targetType, hitInfo.point, hitInfo.normal);
                    ModNetworking.SendToAll(message);
                }
            }
           
            void shootingNothing()
            {
                lineRenderer.SetPosition(1, ePoint);
                sPoint = ePoint;
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

            if (hitInfo.rigidbody != null || hitInfo.collider.attachedRigidbody !=null)
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

            if (hitInfo.rigidbody != null || hitInfo.collider.attachedRigidbody!=null)
            {
                impact.transform.SetParent(hitInfo.transform);
            }
            //impact.AddComponent<DestroyIfEditMode>();
            var tsd = impact.AddComponent<TimedSelfDestruct>();
            //tsd.OnDestruct += () => { Destroy(impact); };
            tsd.lifeTime = 50f;
            tsd.Switch = true;
        }

        private delegate void ActionIfHaveComponent_Kenimatic(Rigidbody rigidbody , Vector3 vector3);
        private delegate void ActionIfHaveComponent_Unkenimatic(RaycastHit hitinfo, Vector3 vector3);

        private Dictionary<Type, ActionIfHaveComponent_Kenimatic> action_Kinematic = new Dictionary<Type, ActionIfHaveComponent_Kenimatic>()
        {
            {typeof(ConfigurableJoint),(rigidbody,f)=>{var cj = rigidbody.gameObject.GetComponent<ConfigurableJoint>();cj.breakForce -= f.magnitude;cj.breakTorque -= f.magnitude; } },
            {typeof(KillingHandler),(rigidbody,f)=>{var kh = rigidbody.gameObject.GetComponent<KillingHandler>();kh.KillUnit(true, InjuryType.Sharp);} },
            {typeof(ExplodeOnCollide),(rigidbody,f)=>{var eoc = rigidbody.gameObject.GetComponent<ExplodeOnCollide>(); eoc.Explodey(); } },
            {typeof(GibOnImpact),(rigidbody,f)=>{var goi = rigidbody.gameObject.GetComponent<GibOnImpact>(); goi.Gib(); } },

        };
        private Dictionary<Type, ActionIfHaveComponent_Unkenimatic> action_Unkinematic = new Dictionary<Type,ActionIfHaveComponent_Unkenimatic>()
        {
             {typeof(BreakOnForce),(hitinfo,f)=>{ var bof = hitinfo.rigidbody.gameObject.GetComponent<BreakOnForce>();bof.BreakExplosion(f.magnitude, hitinfo.point, bof.breakForceRadius, 0f); } },
             {typeof(DestroyOnTriggerEnter),(hitinfo,f)=>{var dote = hitinfo.rigidbody.gameObject.GetComponent<DestroyOnTriggerEnter>();dote.SendMessage("OnTriggerEnter", hitinfo.collider); } },
             {typeof(ParticleOnCollide),(hitinfo,f)=>{ var poc = hitinfo.rigidbody.gameObject.GetComponent<ParticleOnCollide>();poc.SendMessage("OnCollisionEnter", hitinfo.collider.GetComponentInChildren<Collision>());} },
             {typeof(ParticleOnTrigger),(hitinfo,f)=>{ var pot = hitinfo.rigidbody.gameObject.GetComponent<ParticleOnTrigger>();pot.SendMessage("OnTriggerEnter",hitinfo.collider); } },
        };
        private void onCollision(RaycastHit hitInfo)
        {
            try
            {
                Rigidbody rigidbody = null;

                if (hitInfo.collider.isTrigger == false)
                {
                    if (hitInfo.rigidbody != null || hitInfo.collider.attachedRigidbody != null)
                    {
                        rigidbody = hitInfo.collider.attachedRigidbody ?? hitInfo.rigidbody;
                    }
                }
                else
                {
                    var levelEntity = hitInfo.transform.GetComponentInParent<LevelEntity>() ?? hitInfo.transform.GetComponentInChildren<LevelEntity>();
                    if (levelEntity != null)
                    {
                        rigidbody = levelEntity.gameObject.GetComponent<Rigidbody>();
                    }
                }

                addForceToRigidBody(rigidbody);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }

            void addForceToRigidBody(Rigidbody rigidbody)
            {
                if (rigidbody == null) return;

                //∵MV = mv; I=Ft; F = I/t; I=Δp; Δp = mv
                //∴F=mv/t
                var f = (bulletPropertise.Velocity * bulletPropertise.Mass) / _time;

                if (rigidbody.isKinematic == false)
                {
                    if (rigidbody.gameObject.GetComponent<BlockBehaviour>() != null
                        && isWoodenBlock((BlockType)rigidbody.gameObject.GetComponent<BlockBehaviour>().BlockID)
                        || hitInfo.transform.name.ToLower().Contains("tree"))
                    {
                        rigidbody.AddForceAtPosition(f * 10f, hitInfo.point);
                    }
                    else
                    {
                        specialComponentsAction_Kinematic(action_Kinematic);
                        rigidbody.AddForceAtPosition(f, hitInfo.point);
                    }
                    Vector3 com = hitInfo.transform.TransformPoint(rigidbody.centerOfMass);
                    Vector3 vector3 = hitInfo.point - com;
                    Vector3 vector31 = f.normalized + hitInfo.point;
                    Vector3 normal = Vector3.Cross(vector3, vector31);
                    rigidbody.AddTorque(com + normal * f.magnitude * 0.008f);
                }
                else
                {
                    specialComponentsAction_Unkinematic(action_Unkinematic);
                }

                var bhb = rigidbody.gameObject.GetComponent<BlockHealthBar>();
                if (bhb != null)
                {
                    bhb.DamageBlock(f.magnitude * 0.001f);
                }

                void specialComponentsAction_Kinematic(Dictionary<Type, ActionIfHaveComponent_Kenimatic> dic)
                {
                    foreach (var com in dic.Keys)
                    {
                        if (rigidbody.gameObject.GetComponent(com) != null)
                        {
                            dic[com](rigidbody, f);
                            break;
                        }
                    }
                }
                void specialComponentsAction_Unkinematic(Dictionary<Type,ActionIfHaveComponent_Unkenimatic> dic)
                {
                    foreach (var com in dic.Keys)
                    {
                        if (rigidbody.gameObject.GetComponent(com) != null)
                        {
                            dic[com](hitInfo, f);
                            break;
                        }
                    }
                }
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

