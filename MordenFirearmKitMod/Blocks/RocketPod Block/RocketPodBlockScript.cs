using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;
using System.Collections;

namespace ModernFirearmKitMod
{
    class RocketPodBlockScript : LauncherBlockScript
    {

        public static GameObject RocketTemp;

        #region 功能变量声明

        public override float Rate { get; set; }
        public override float KnockBack { get; set; }
        public override int BulletCurrentNumber { get; set; }
        public override int BulletMaxNumber { get; set; }
        public override GameObject BulletObject { get; set; }
        public override Vector3 SpawnPoint { get; set; }
        public override bool LaunchEnable { get; set; }

        #endregion

        //声明 滑条 载弹数量
        private MSlider number_slider;
        //声明 滑条 连发间隔
        private MSlider rate_slider;

        MSlider thrustForce_slider;

        MSlider thrustTime_slider;

        //MSlider thrustDelay_slider;

        MSlider DragForce_slider;


        #region 内部变量声明

        private GameObject RocketPool;
        private GameObject RocketPool_Idle;

        //声明 火箭弹
        private GameObject[] Rockets = new GameObject[18];

        //声明 火箭弹标签
        //private int Label = 0;

        //声明 火箭弹实例化位置
        private Vector3[] relativePositions;


        #endregion

        public override void SafeAwake()
        {

            LaunchKey = AddKey("发射", "Launch", KeyCode.L);

            KnockBack = 0f;
            SpawnPoint = new Vector3(0, 0, 3.5f);
            BulletObject = RocketTemp;
            
            BulletCurrentNumber = BulletMaxNumber = 18;
            Rate = 2f;
            LaunchEnable = false;

            LaunchEvent += delayLaunch;

            relativePositions = GetRelativePositions();

            #region 基本功能参数初始化

            //添加 滑条 参数
            number_slider = AddSlider("载弹数量", "Number", BulletMaxNumber, 1, 18);
            number_slider.ValueChanged += (value) => { BulletMaxNumber = (int)value; };
            rate_slider = AddSlider("射速 0.1s", "Rate", Rate, 1f, 5f);
            rate_slider.ValueChanged += (value)=> { Rate = value * 0.1f; };

            thrustForce_slider = AddSlider("推力大小", "Thrust Force", 1, 0f, 10f);
            thrustForce_slider.ValueChanged += (value) => { changedPropertise(); };

            thrustTime_slider = AddSlider("推力时间 10s", "Thrust Time", 1, 0f, 10f);
            thrustTime_slider.ValueChanged += (value) => { changedPropertise(); };

            DragForce_slider = AddSlider("阻力大小", "DRAG", 0.5f, 0.2f, 3f);
            DragForce_slider.ValueChanged += (value) => { changedPropertise(); };

            #endregion

            changedPropertise();

        }

        void changedPropertise()
        {


        }

        public void DisplayInMapper(int value)
        {
            bool show_0 = true;

            #region 页码0控件

            LaunchKey.DisplayInMapper = show_0;
            //explosiontype_menu.DisplayInMapper = show_0;
            //power_slider.DisplayInMapper = show_0;
            //colliderDelay_slider.DisplayInMapper = show_0;
            //thrustDelay_slider.DisplayInMapper = show_0;
            number_slider.DisplayInMapper = show_0;
            rate_slider.DisplayInMapper = show_0;
            thrustForce_slider.DisplayInMapper = show_0;
            thrustTime_slider.DisplayInMapper = show_0;
            DragForce_slider.DisplayInMapper = show_0;

            #endregion
        }

        public override void OnSimulateStart()
        {
            InitPool();

            BulletMaxNumber = Mathf.Clamp(BulletMaxNumber, 1, 18);

            Reload(true);

            void InitPool()
            {
                RocketPool = new GameObject("Rocket Pool");
                RocketPool.transform.SetParent(transform);
                RocketPool.transform.position = transform.position;
                RocketPool.transform.rotation = transform.rotation;

                RocketPool_Idle = MordenFirearmKitBlockMod.RocketPool_Idle;
            }
        }

        public override void SimulateUpdateHost()
        {
            Reload();

            if (LaunchKey.IsDown )
            {
                if (!LaunchEnable&& RocketPool.transform.childCount > 0)
                {
                    LaunchEnable = true;
                    StartCoroutine(Launch(RocketPool.transform.GetChild(0).gameObject));
                }
            }
        }

        //火箭弹实例化位置计算
        private static Vector3[] GetRelativePositions()
        {
            int i;

            Vector3[] Positions = new Vector3[18];

            //声明 原点
            Vector2 origin = new Vector2(0.4f, 0);

            //声明 大圆半径、角度差和旋转角
            float radius_large = 0.37f, angle_large = 30f;

            //声明 小圆半径、角度差和旋转角
            float radius_little = 0.19f, angle_little = 60f;

            //外圈火箭弹位置
            for (i = 0; i < 12; i++)
            {
                Positions[i] = new Vector3(
                                                    0,
                                                    origin.y + radius_large * Mathf.Sin(angle_large * i * Mathf.Deg2Rad),
                                                    origin.x - radius_large * Mathf.Cos(angle_large * i * Mathf.Deg2Rad)
                                                 );
            }

            //内圈火箭弹位置
            for (i = 0; i < 6; i++)
            {
                Positions[i + 12] = new Vector3(
                                                        0,
                                                        origin.y + radius_little * Mathf.Sin((angle_little * i + 30) * Mathf.Deg2Rad),
                                                        origin.x - radius_little * Mathf.Cos((angle_little * i + 30) * Mathf.Deg2Rad)
                                                      );
            }

            return Positions;
        }
        //火箭弹发射世界位置
        private Vector3 getRealPosition(int label,Vector3 offset)
        {
            return transform.TransformVector(transform.InverseTransformVector(Rigidbody.position) + relativePositions[label] + offset);
        }

        //火箭弹实例化
        private void Rocket_Instantiate(int index)
        {

            //火箭弹安装位置 本地坐标转世界坐标
            Vector3 offset = new Vector3(-0.375f, 0f, 0.15f);
            Vector3 pos = getRealPosition(index, offset);

            //火箭弹实例化
            GameObject rocket = (GameObject)Instantiate(RocketTemp, pos, RocketPool.transform.rotation,RocketPool.transform);
            rocket.name = "Rocket " + index;
            rocket.SetActive(true);
            Rockets[index] = rocket;

            //火箭弹刚体 不开启碰撞 不受物理影响
            Rigidbody rigidbody = rocket.GetComponent<Rigidbody>();
            rigidbody.detectCollisions = false;
            rigidbody.isKinematic = true;
            //火箭弹脚本 初始化
            RocketScript rocketScript = rocket.GetComponent<RocketScript>();
            rocketScript.ThrustDirection = Vector3.right;
            rocketScript.ThrustPoint = rigidbody.centerOfMass;
            rocketScript.ThrustForce = thrustForce_slider.Value;
            rocketScript.ThrustTime = thrustTime_slider.Value*10f;
            rocketScript.DelayLaunchTime = 0f;
            rocketScript.DelayEnableCollisionTime = 0.02f;
            rocketScript.ExplodePower = 1f;
            rocketScript.ExplodeRadius = 10f;
            rocketScript.DragClamp = DragForce_slider.Value;

            rocketScript.effectOffset = new Vector3(-1.15f, 0, 0f);
            rocketScript.OnExplodeFinal += () => 
            {
                Debug.Log(" rocket final");
                string i = rocketScript.gameObject.name;
                i = i.Substring(i.LastIndexOf(' '));
                Rockets[int.Parse(i)] = null;
                rocketScript.gameObject.transform.SetParent(RocketPool_Idle.transform);
            };
        }

        private void Rocket_Reusing(int index)
        {
            //火箭弹安装位置 本地坐标转世界坐标
            Vector3 offset = new Vector3(-0.375f, 0f, 0.15f);
            Vector3 pos = getRealPosition(index, offset);
            //火箭弹重新设置
            GameObject rocket = Rockets[index] = RocketPool_Idle.transform.GetChild(0).gameObject;
            rocket.transform.SetParent(RocketPool.transform);
            rocket.transform.position = pos;
            rocket.transform.rotation = RocketPool.transform.rotation;         
            rocket.name = "Rocket " + index;
            rocket.SetActive(true);
            //火箭弹脚本 参数重新设置
            RocketScript rocketScript = rocket.GetComponent<RocketScript>();
            rocketScript.Reusing(thrustForce_slider.Value, thrustTime_slider.Value * 10f, DragForce_slider.Value);
        }

        private void delayLaunch(GameObject gameObject)
        {          
            gameObject.transform.localPosition += Vector3.right * 3.25f;
            gameObject.transform.SetParent(transform.parent.transform);
     
            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();      
            rigidbody.isKinematic = false;
            rigidbody.AddRelativeForce(Vector3.right * 20f, ForceMode.Impulse);
            RocketScript rocketScript = gameObject.GetComponent<RocketScript>();
            rocketScript.LaunchEnabled = true;

            StartCoroutine(delay());
            IEnumerator delay()
            {
                yield return new WaitForSeconds(rocketScript.DelayEnableCollisionTime);
                rigidbody.detectCollisions = true;
                gameObject.GetComponentInChildren<CapsuleCollider>().isTrigger = false;
                yield break;
            }
        }   
        //火箭弹重装
        public override void Reload(bool constraint = false)
        {
            //火箭弹在无限弹药模式下 有空位的情况下实例化新火箭弹
            if ((StatMaster.GodTools.InfiniteAmmoMode || constraint) && RocketPool.transform.childCount < BulletMaxNumber)
            {
                for (int i = 0; i < BulletMaxNumber; i++)
                {
                    if (!Rockets[i] || Rockets[i].GetComponent<RocketScript>().Launched)
                    {
                        if (RocketPool_Idle.transform.childCount > 0)
                        {
                            Rocket_Reusing(i);
                        }
                        else
                        {
                            Rocket_Instantiate(i);
                        }
                        BulletCurrentNumber = (int)Mathf.MoveTowards(BulletCurrentNumber, BulletMaxNumber, 1);
                    }
                }
            }
        }

       


        internal static void CreateRocketBlockTemp()
        {
            RocketTemp = new GameObject("Rocket Temp");
            RocketTemp.transform.localScale = new Vector3(1f, 0.75f, 0.75f);

            GameObject vis = new GameObject("Vis");
            vis.transform.SetParent(RocketTemp.transform);
            vis.transform.localPosition -= RocketTemp.transform.right;
            vis.transform.localScale = RocketTemp.transform.localScale;
            vis.AddComponent<MeshFilter>().mesh = ModResource.GetMesh("Rocket Mesh");
            vis.AddComponent<MeshRenderer>().material.mainTexture = ModResource.GetTexture("Rocket Texture");

            GameObject collider = new GameObject("Collider");
            collider.transform.SetParent(RocketTemp.transform);
            collider.transform.localScale = RocketTemp.transform.localScale;
            CapsuleCollider capsuleCollider = collider.AddComponent<CapsuleCollider>();            
            capsuleCollider.radius = 0.15f;
            capsuleCollider.height = 2.5f;
            capsuleCollider.direction = 0;
            capsuleCollider.isTrigger = true;

            Rigidbody rigidbody = RocketTemp.AddComponent<Rigidbody>();
            rigidbody.mass = 0.25f;

            RocketTemp.AddComponent<RocketScript>();
            RocketTemp.AddComponent<DestroyIfEditMode>();
            RocketTemp.SetActive(false); 


        }

    }
}
