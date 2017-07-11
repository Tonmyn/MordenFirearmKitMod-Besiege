using System;
using System.Collections.Generic;
using spaar.ModLoader;
using TheGuysYouDespise;
using UnityEngine;
using System.Collections;

namespace MordenFirearmKitMod
{

    partial class MordenFirearmKitMod
    {
        
        //机枪模块
        public Block MachineGun = new Block()
            ///ID of the Block
            .ID(652)

            ///Name of the Block
            .BlockName("MachineGun")

            ///Load the 3d model information
            .Obj(new List<Obj> { new Obj("/MordenFirearmKitMod/Barrel.obj", //Mesh name with extension (only works for .obj files)
                                         "/MordenFirearmKitMod/Butt.png", //Texture name with extension
                                         new VisualOffset(new Vector3(0.5f, 0.5f, 0.5f), //Scale
                                                          new Vector3(-0.191f,-0.4575f, 0.35f), //Position
                                                          new Vector3(90f,  0f, 180f))),//Rotation
            })

            ///For the button that we will create setup the visual offset needed
            .IconOffset(new Icon(new Vector3(0.75f, 0.75f, 0.75f),  //Scale
                              new Vector3(-0.11f, -0.13f, 0f),  //Position
                              new Vector3(  0f,   0f,   0f))) //Rotation

            ///Script, Components, etc. you want to be on your block.
            .Components(new Type[] {typeof(BulletScript),typeof(MachineGunScript),
              })

            ///Properties such as keywords for searching and setting up how how this block behaves to other elements.
            .Properties(new BlockProperties().SearchKeywords(null)
     //.Burnable(3f)
     //.CanBeDamaged(3)
     )

            ///Mass of the block 0.5 being equal to a double wooden block
            .Mass(0.3f)

            ///Display the collider while working on the block if you wish, then replace "true" with "false" when done looking at the colliders.
#if DEBUG
            .ShowCollider(true)
#endif
            ///Setup the collier of the block, which can consist of several different colliders.
            ///Therefore we have this CompoundCollider,
            .CompoundCollider(new List<ColliderComposite> {

                                ColliderComposite.Capsule(  0.2f,
                                                            3f,
                                                            Direction.Z,
                                                            new Vector3(0f, 0f, 1.65f),
                                                            new Vector3(0f, 0f, 0f)),                
                                
                              ColliderComposite.Box(new Vector3(0.35f, 0.35f, 0.15f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)).Trigger().Layer(2).IgnoreForGhost(),//   <---Example: Box Trigger on specific Layer
            })

            ///Make sure a block being placed on another block can intersect with it.
            .IgnoreIntersectionForBase()

            ///Load resources that will be needed for the block.
            .NeededResources(new List<NeededResource> {
                                new NeededResource(ResourceType.Audio, //Type of resource - types available are Audio, Texture, Mesh, and AssetBundle
                                                   "/MordenFirearmKitMod/MachineGun.ogg"),
                                new NeededResource(ResourceType.Mesh,
                                                   "/MordenFirearmKitMod/Rocket.obj")
            })

           ///Setup where on your block you can add other blocks.
           .AddingPoints(new List<AddingPoint> {
                              (AddingPoint) new BasePoint(false,true)         //The base point is unique compared to the other adding points, the two booleans represent whether you can add to the base, and whether it sticks automatically.
                                               .Motionable(true,false,false) //Set each of these booleans to "true" to Let the block rotate around X, Y, Z accordingly
                                               .SetStickyRadius(0.5f),        //Set the radius of which the base point will connect to others
           //                  //new AddingPoint(new Vector3(0f, 0f, 0.5f), new Vector3(-90f, 0f, 0f),true).SetStickyRadius(0.3f), <---Example: Top sticky adding point
           })
           ;

    }


    public class MachineGunScript: BlockScript
    {


        #region 公共变量

        //武器类型
        public enum Caliber { 机枪, 机炮, 速射炮 }

        //口径
        public Caliber caliber = Caliber.机枪;

        //射速
        public float FireRate = 0.05f;

        public MKey Fire;

        
        #endregion

        //加特林转速
        private float RotationRate = 0;


        #region 私有变量

        

        #endregion



        //通用组件 存放粒子声音等组件
        protected GameObject generic = new GameObject("通用组件");

        Ray shootRay = new Ray();
        RaycastHit shootHit;
        //int shootableMask;

        ParticleSystem gunParticles;
        LineRenderer gunLine;
        AudioSource gunAudio;
        Light gunLight;

        LauncherScript ls;

        float timer;
        float effectsDisplayTime = 0.05f;

        public GameObject bullet;


        public override void SafeAwake()
        {
            //shootableMask = LayerMask.GetMask("Shootable");
            //skin = new MVisual(VisualController,0,new List<BlockSkinLoader.SkinPack.Skin>() {resources["/MordenFirearmKitMod/Barrel.obj"].texture, });
            //Fire = AddKey("发射", "Launch", KeyCode.Y);
            GetComponent<BulletScript>().mesh = resources["/MordenFirearmKitMod/Rocket.obj"].mesh;
            Debug.Log("blockscript");
            
            
        }

        public override void OnSave(XDataHolder stream)
        {
            base.OnSave(stream);
            SaveMapperValues(stream);
        }

        public override void OnLoad(XDataHolder stream)
        {
            base.OnLoad(stream);
            LoadMapperValues(stream);
        }


        protected override void OnSimulateStart()
        {

            //GetComponentInChildren<MeshFilter>().mesh = resources["/MordenFirearmKitMod/Rocket.obj"].mesh;
            
            //bullet_init();

            Debug.Log(GetComponent<BulletScript>().bulletType);

            //MeshFilter mf = bullet.AddComponent<MeshFilter>();
            //mf.sharedMesh = resources["/MordenFirearmKitMod/Rocket.obj"].mesh;
            //MeshRenderer mr = bullet.AddComponent<MeshRenderer>();


            //c = gameObject.GetComponentInChildren<CapsuleCollider>();
            //PhysicMaterial pm = c.material = new PhysicMaterial("Ice");

            //pm.dynamicFriction = Mathf.Infinity;
            //pm.staticFriction = Mathf.Infinity;
            //pm.frictionCombine =  PhysicMaterialCombine.Maximum;



            //renderset();
            //Obj obj = new Obj("/MordenFirearmKitMod/Barrel.obj");

            // mesh = resources["/MordenFirearmKitMod/Rocket.obj"].mesh;
            //mesh.RecalculateBounds();


            //mr.material.mainTexture = resources["/MordenFirearmKitMod/RocketFire.png"].texture;
            //MeshFilter mf = test.AddComponent<MeshFilter>();
            ////mesh = mf.mesh;
            //mf.sharedMesh = resources["/MordenFirearmKitMod/Rocket.obj"].mesh;
            //MeshRenderer mr = test.AddComponent<MeshRenderer>();


            //new一个链表  
            //list = new List<Vector3>();
            //获得Mesh  
            //mesh = test.GetComponent<MeshFilter>().mesh;

            //修改Mesh的颜色  
            //test.GetComponent<MeshRenderer>().material.color = Color.green;
            //选择Mesh中的Shader  
            //test.GetComponent<MeshRenderer>().material.shader = Shader.Find("Transparent/Diffuse");
            //清空所有点，用于初始化！  
            //mesh.Clear();





        }

        protected override void OnSimulateUpdate()
        {

            //timer += Time.deltaTime;

            
            //if (Input.GetKey(KeyCode.Y) )
            //{
               
            //    RotationRate = Mathf.MoveTowards(RotationRate, 60, 1);
            //    if (RotationRate == 60 && timer >= FireRate && Time.timeScale != 0)
            //    {
            //        shoot();
            //    }
            //}
            //else
            //{
            //    RotationRate = Mathf.MoveTowards(RotationRate, 0, Time.deltaTime*10);
            //}
            //transform.Rotate(new Vector3(0, 0, RotationRate));

            //if (timer >= FireRate * effectsDisplayTime)
            //{
            //    DisableEffects();
            //}


            ////点击鼠标左键  
            //if (Input.GetMouseButton(0))
            //{
            //    //顶点数+1  
            //    count++;
            //    //将获得的鼠标坐标转换为世界坐标，然后添加到list链表中。  
            //    list.Add(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.8f)));



            //}

            ////如果顶点数>=3，那么就开始渲染Mesh  
            //if (count >= 3)
            //{
            //    //根据顶点数来计算绘制出三角形的所以顶点数  
            //    triangles = new int[3 * (count - 2)];
            //    //根据顶点数来创建记录顶点坐标  
            //    vertices = new Vector3[count];
            //    //将链表中的顶点坐标赋值给vertices  
            //    for (int i = 0; i < count; i++)
            //    {
            //        vertices[i] = list[i];

            //    }

            //    //三角形个数  
            //    int triangles_count = count - 2;
            //    //根据三角形的个数，来计算绘制三角形的顶点顺序（索引）  
            //    for (int i = 0; i < triangles_count; i++)
            //    {
            //        //这个算法好好琢磨一下吧~  
            //        triangles[3 * i] = 0;
            //        triangles[3 * i + 1] = i + 2;
            //        triangles[3 * i + 2] = i + 1;
            //    }
            //    //设置顶点坐标  
            //    mesh.vertices = vertices;
            //    //设置顶点索引  
            //    mesh.triangles = triangles;

            //}

        }

        protected override void OnSimulateExit()
        {
            //GetComponent<BulletScript>().bullet.SetActive(false);
        }

        public void DisableEffects()
        {
            gunLine.enabled = false;
            gunLight.enabled = false;
            //gunAudio.Stop();
            gunParticles.Stop();
        }

        private void shoot()
        {
            timer = 0f;

            gunAudio.volume = 5 / Vector3.Distance(this.transform.position, Camera.main.transform.position);
            gunAudio.Play();

            gunLight.enabled = true;
                 
            gunParticles.Play();

            gunLine.enabled = true;
            gunLine.SetPosition(0, transform.position);

            //gunLine.SetPosition(1, transform.position + transform.forward * 100);
            shootRay.origin = transform.TransformPoint( generic.transform.localPosition + new Vector3(0,0,1));
            shootRay.direction = transform.forward;
            

            if (Physics.Raycast(shootRay, out shootHit, 100))
            {
                
                //EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
                //if (enemyHealth != null)
                //{
                //    enemyHealth.TakeDamage(damagePerShot, shootHit.point);
                //}
                gunLine.SetPosition(1, shootHit.point);
                Debug.Log(shootHit.collider.name);
                StartCoroutine(Rocket_Explodey(shootHit.point));
                //gunLine.SetPosition(1, shootRay.origin + shootRay.direction * 100);
            }
            else
            {
                gunLine.SetPosition(1, shootRay.origin + shootRay.direction * 100);
            }
        }

        private void renderset()
        {
            
            gunParticles = generic.AddComponent<ParticleSystem>();
            gunLight = generic.AddComponent<Light>();
            gunLine = generic.AddComponent<LineRenderer>();
            gunAudio = generic.AddComponent<AudioSource>();

            generic.transform.SetParent(transform);
            generic.transform.localPosition = new Vector3(0,0,3f);
            generic.transform.localEulerAngles = new Vector3(0, 0, 180);
            //generic.transform.LookAt(transform.position);
            
            
            //line.AddComponent<TrailRenderer>();

            gunLine.SetVertexCount(2);
            gunLine.useWorldSpace = true;
            gunLine.SetWidth(0.15f, 0.15f);
            gunLine.sharedMaterial = new Material(Shader.Find("Unlit/Color"));
            gunLine.SetColors(new Color32(250, 135, 0, 255), new Color(0,0,0,0));

            
            gunLight.range = 10;
            gunLight.type = LightType.Spot;
            gunLight.spotAngle = 85;
            gunLight.color = new Color32(250, 135, 0, 255);
            gunLight.intensity = 100f;
            gunLight.shadows = LightShadows.Hard;
            gunLight.enabled = true;

            
            
            gunAudio.clip = resources["/MordenFirearmKitMod/MachineGun.ogg"].audioClip;
            gunAudio.playOnAwake = false;
            gunAudio.loop = false;
            gunAudio.enabled = true;

            gunParticles.transform.SetParent(transform);
            gunParticles.transform.position = transform.TransformVector(transform.position + new Vector3(0,0,4.75f));
            //gunParticles.transform.rotation = Quaternion.identity;
            gunParticles.playOnAwake = false;
            gunParticles.Stop();
            gunParticles.loop = false;
            gunParticles.startSize = 5;
            gunParticles.startSpeed = 4;
            gunParticles.maxParticles = 25;
            gunParticles.startLifetime = 0.1f;
            gunParticles.startColor = new Color32(250,135,0,255);

            ParticleSystem.EmissionModule em = gunParticles.emission;
            em.rate = new ParticleSystem.MinMaxCurve(100);
            em.SetBursts(new ParticleSystem.Burst[] {new ParticleSystem.Burst(0,8,30)});
            em.enabled = true;

            ParticleSystem.ShapeModule sm = gunParticles.shape;
            sm.shapeType = ParticleSystemShapeType.Cone;
            sm.radius = 0.01f;
            sm.angle = 4.65f;
            sm.randomDirection = false;
            sm.enabled = true;

            ParticleSystem.VelocityOverLifetimeModule volm = gunParticles.velocityOverLifetime;
            volm.z = 2;
            volm.space = ParticleSystemSimulationSpace.Local;
            volm.enabled = true;

            ParticleSystem.ColorOverLifetimeModule colm = gunParticles.colorOverLifetime;
            colm.color = new Gradient()
            {
                alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(255, gunParticles.startLifetime *0.65f), new GradientAlphaKey(70, gunParticles.startLifetime) },

                colorKeys = new GradientColorKey[] { new GradientColorKey(Color.white, gunParticles.startLifetime * 0.35f), new GradientColorKey(new Color32(250, 135, 0, 255), gunParticles.startLifetime * 0.65f), new GradientColorKey(new Color(0,0,0), gunParticles.startLifetime) }
            };
            colm.enabled = true;

            ParticleSystem.SizeOverLifetimeModule solm = gunParticles.sizeOverLifetime;
            solm.separateAxes = false;
            solm.size = new ParticleSystem.MinMaxCurve(1, new AnimationCurve(new Keyframe[] {new Keyframe(0,0.15f), new Keyframe(0.17f,0.9f), new Keyframe(0.25f,0.8f), new Keyframe(1,0) }));
            solm.enabled = true;

            ParticleSystemRenderer psr = gunParticles.GetComponent<ParticleSystemRenderer>();
            psr.renderMode = ParticleSystemRenderMode.Billboard;
            psr.normalDirection = 1;
            psr.material = new Material(Shader.Find("Particles/Additive"));
            psr.material.mainTexture = resources["/MordenFirearmKitMod/RocketSmoke.png"].texture;
            psr.sortMode = ParticleSystemSortMode.None;
            psr.sortingFudge = 0;
            psr.minParticleSize = 0;
            psr.maxParticleSize = 1;
            psr.alignment = ParticleSystemRenderSpace.View;
            psr.pivot = Vector3.zero;
            psr.motionVectors = true;
            psr.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            psr.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.BlendProbes;
            

        }

        //private void test()
        //{
        //    Launcher launcher = new Launcher();
        //}

        //爆炸事件
        public IEnumerator Rocket_Explodey(Vector3 point)
        {

            yield return new WaitForFixedUpdate();

            //爆炸范围
            float radius = 5;

            //爆炸位置
            Vector3 position_hit = point;


                GameObject explo = (GameObject)GameObject.Instantiate(PrefabMaster.BlockPrefabs[54].gameObject, position_hit, transform.rotation);
                explo.transform.localScale = Vector3.one * 0.01f;
                ControllableBomb ac = explo.GetComponent<ControllableBomb>();
                ac.radius = 2 + radius;
                ac.power = 3000f * radius;
                ac.randomDelay = 0.00001f;
                ac.upPower = 0f;
                ac.StartCoroutine_Auto(ac.Explode());
                explo.AddComponent<TimedSelfDestruct>();


            //Destroy(gameObject);


        }


    }


    //武器类
    public class Weapon
    {
        //发射器
        //public Launcher launcher;

        //子弹
        //public Bullet bullet;


    }
    
    
    //发射器类
    public class LauncherScript : BlockBehaviour
    {
       

        //子弹
        //public Bullet bullet;

        //散布
        //public float Diffuse;

        //弹药量
        public int bulletNumber;

        //射速
        public float FireRate;

        //扳机
        public KeyCode Trigger;

        //质量
        public float Mass;

        public MKey key;

        public override void OnLoad(XDataHolder data)
        {
            throw new NotImplementedException();
        }

        public override void OnSave(XDataHolder data)
        {
            throw new NotImplementedException();
        }

        public override int GetBlockID()
        {
            throw new NotImplementedException();
        }

        public void Awake()
        {
            Debug.Log(name);
            
        }
    }

    //子弹类
    public class BulletScript : MonoBehaviour
    {

        #region 物理参数

        //威力
        public float Force;

        //口径
        public float Caliber;

        //后坐力
        public float Recoil { get; }

        //射程
        public float Distance { get; } 

        //动能
        public float KineticEnergy { get; }

        //初速
        public float MuzzleVelocity { get; }

        ////阻力
        //public float Drag { get; }

        ////质量
        //public float Mass { get; }

        #endregion



        #region 属性变量

        //类型
        public BulletType bulletType;

        //子弹种类
        public enum BulletType { 高爆弹, 拽光弹, 穿甲弹 }

        public GameObject bullet = new GameObject("bullet");

        protected Rigidbody rigidbody;

        public Mesh mesh;

        public Texture texture;


        #endregion

        private void Awake()
        {
            if (StatMaster.isSimulating && !bullet.GetComponent<MeshFilter>())
            {
                bullet_init();
            }
            
            Debug.Log("bullet");
            
        }

        private void Update()
        {

            if (!StatMaster.isSimulating)
            {
                bullet_Destroy();
            }
            else
            {

                bullet.SetActive(true);
            }

        }

        

        public void bullet_init()
        {

            bullet.AddComponent<MeshFilter>().mesh = mesh;
            bullet.AddComponent<MeshRenderer>();

            bullet.AddComponent<Rigidbody>();
            GameObject collider = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            collider.transform.parent = bullet.transform;
            
            bullet.transform.position = new Vector3(0, 5, 0);

            
        }


        public void bullet_Destroy()
        {
            bullet.SetActive(false);

        }
        //public Bullet()
        //{



        //    #region 物理参数

        //    Force = 1;

        //    Caliber = 1;

        //    Recoil = 1;

        //    Distance = 20;

        //    #endregion

        //    rigidbody = gameobject.AddComponent<Rigidbody>();

        //    gameobject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //}

        public float getKineticEnergy()
        {
            return 0.5f * rigidbody.mass * rigidbody.velocity.sqrMagnitude;
        }

        public float getBuzzleVelocity(float force)
        {
            return Mathf.Sqrt(2 * force / rigidbody.mass);
        }

        public float getMass(float caliber)
        {
            return 0.5f * Mathf.Sqrt(caliber);
        }
        


    }   

 
}
