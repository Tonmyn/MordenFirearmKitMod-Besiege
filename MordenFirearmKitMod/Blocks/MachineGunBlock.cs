﻿using System;
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
            .Components(new Type[] {typeof(MachineGunScript)
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
                                
                              //ColliderComposite.Box(new Vector3(0.35f, 0.35f, 0.15f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)).Trigger().Layer(2).IgnoreForGhost(),   <---Example: Box Trigger on specific Layer
            })

            ///Make sure a block being placed on another block can intersect with it.
            .IgnoreIntersectionForBase()

            ///Load resources that will be needed for the block.
            .NeededResources(new List<NeededResource> {
                                new NeededResource(ResourceType.Audio, //Type of resource - types available are Audio, Texture, Mesh, and AssetBundle
                                                   "/MordenFirearmKitMod/MachineGun.ogg")
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


        public float timeBetweenBullets = 0.05f;
        private float rotationspeed = 0;

        protected GameObject line = new GameObject("射线组件");
        protected GameObject light = new GameObject("粒子组件");
        protected GameObject audio = new GameObject("声音组件");

        protected MVisual skin;

        Ray shootRay = new Ray();
        RaycastHit shootHit;
        //int shootableMask;
        ParticleSystem gunParticles;
        LineRenderer gunLine;
        AudioSource gunAudio;
        Light gunLight;
        float timer;
        float effectsDisplayTime = 0.05f;


        public override void SafeAwake()
        {
            base.SafeAwake();
            //shootableMask = LayerMask.GetMask("Shootable");
            //skin = new MVisual(VisualController,0,new List<BlockSkinLoader.SkinPack.Skin>() { });

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
            base.OnSimulateStart();

            //c = gameObject.GetComponentInChildren<CapsuleCollider>();
            //PhysicMaterial pm = c.material = new PhysicMaterial("Ice");
            
            //pm.dynamicFriction = Mathf.Infinity;
            //pm.staticFriction = Mathf.Infinity;
            //pm.frictionCombine =  PhysicMaterialCombine.Maximum;
            

            renderset();
            
        }

        protected override void OnSimulateUpdate()
        {
            base.OnSimulateUpdate();
            timer += Time.deltaTime;

            
            if (Input.GetKey(KeyCode.Y) )
            {
               
                rotationspeed = Mathf.MoveTowards(rotationspeed, 60, 1);
                if (rotationspeed == 60 && timer >= timeBetweenBullets && Time.timeScale != 0)
                {
                    shoot();
                }
            }
            else
            {
                rotationspeed = Mathf.MoveTowards(rotationspeed, 0, Time.deltaTime*10);
            }
            transform.Rotate(new Vector3(0, 0, rotationspeed));

            if (timer >= timeBetweenBullets * effectsDisplayTime)
            {
                DisableEffects();
            }

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
            shootRay.origin = transform.TransformPoint( light.transform.localPosition + new Vector3(0,0,1));
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
            
            gunParticles = line.AddComponent<ParticleSystem>();
            gunLight = light.AddComponent<Light>();
            gunLine = line.AddComponent<LineRenderer>();
            gunAudio = audio.AddComponent<AudioSource>();

            light.transform.SetParent(transform);
            light.transform.localPosition = new Vector3(0,0,3f);
            light.transform.localEulerAngles = new Vector3(0, 0, 180);
            //light.transform.LookAt(transform.position);
            
            
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

   
}
