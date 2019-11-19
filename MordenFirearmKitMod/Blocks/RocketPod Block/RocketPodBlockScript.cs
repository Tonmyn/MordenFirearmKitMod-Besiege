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

        #region 功能变量声明

        public override float Rate { get; set; }
        public override float KnockBack { get; set; }
        public override int BulletCurrentNumber { get; set; }
        public override int BulletMaxNumber { get; set; }
        public override GameObject BulletObject { get; set; }
        public override Vector3 SpawnPoint { get; set; }
        public override Vector3 Direction { get; set; }
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

        private BulletPool rocketPool;

        //声明 火箭弹
        private GameObject[] Rockets = new GameObject[18];

        //声明 火箭弹实例化位置
        private Vector3[] relativePositions;


        #endregion

        public override void SafeAwake()
        {
            Rate = 2f;
            KnockBack = 0f;
            LaunchEnable = false;
            LaunchEvent += delayLaunch;
            SpawnPoint = new Vector3(0, 0, 3.5f);
            Direction = transform.right;
            BulletCurrentNumber = BulletMaxNumber =18;
            BulletObject = BulletObject ?? AssetManager.Instance.Rocket.rocketTemp;

            relativePositions = GetRelativePositions();

            #region 基本功能参数初始化

            LaunchKey = AddKey(LanguageManager.Instance.CurrentLanguage.launch, "Launch", KeyCode.L);

            //添加 滑条 参数
            number_slider = AddSlider(LanguageManager.Instance.CurrentLanguage.bulletNumber, "Number", BulletMaxNumber, 1, 18);
            number_slider.ValueChanged += (value) => { BulletMaxNumber = (int)value; };
            rate_slider = AddSlider(LanguageManager.Instance.CurrentLanguage.rate, "Rate", Rate, 1f, 5f);
            rate_slider.ValueChanged += (value)=> { Rate = value * 0.1f; };

            thrustForce_slider = AddSlider(LanguageManager.Instance.CurrentLanguage.thrustForce, "Thrust Force", 1, 0f, 10f);
            thrustForce_slider.ValueChanged += (value) =>{ changedPropertise(); };

            thrustTime_slider = AddSlider(LanguageManager.Instance.CurrentLanguage.thrustTime, "Thrust Time", 1, 0f, 10f);
            thrustTime_slider.ValueChanged += (value) => { changedPropertise(); };

            DragForce_slider = AddSlider(LanguageManager.Instance.CurrentLanguage.drag, "DRAG", 0.5f, 0.2f, 3f);
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
           
            BulletCurrentNumber = BulletMaxNumber = Mathf.Clamp(BulletMaxNumber, 1, 18);
            rocketPool = new BulletPool(transform, MordenFirearmKitBlockMod.RocketPool_Idle, BulletMaxNumber);
         
            Reload(true);
        }

        public override void SimulateUpdateHost()
        {
            Reload();

            if (LaunchKey.IsHeld )
            {
                if (!LaunchEnable && BulletCurrentNumber > 0)
                {
                    LaunchEnable = true;
                    StartCoroutine(Launch(rocketPool.Work.GetChild(0).gameObject));
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
            GameObject rocket = (GameObject)Instantiate(BulletObject, pos, transform.rotation,rocketPool.Work);
            rocket.transform.localScale = Vector3.Scale(rocket.transform.localScale, transform.localScale);
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
            rocketScript.DelayEnableCollisionTime = 0.04f;
            rocketScript.ExplodePower = 1f;
            rocketScript.ExplodeRadius = 10f;
            rocketScript.DragClamp = DragForce_slider.Value;

            rocketScript.effectOffset = new Vector3(-1.15f, 0, 0f);
            rocketScript.OnExplodeFinal += () => 
            {
                string i = rocketScript.gameObject.name;
                i = i.Substring(i.LastIndexOf(' '));
                Rockets[int.Parse(i)] = null;
                rocketScript.gameObject.transform.SetParent(rocketPool.Idle);
            };
        }

        private void Rocket_Reusing(int index)
        {
            //火箭弹安装位置 本地坐标转世界坐标
            Vector3 offset = new Vector3(-0.375f, 0f, 0.15f);
            Vector3 pos = getRealPosition(index, offset);
            //火箭弹重新设置
            GameObject rocket = Rockets[index] = rocketPool.Idle.GetChild(0).gameObject;
            rocket.transform.SetParent(rocketPool.Work);
            rocket.transform.position = pos;
            rocket.transform.rotation = transform.rotation;         
            rocket.name = "Rocket " + index;
            rocket.SetActive(true);
            //火箭弹脚本 参数重新设置
            RocketScript rocketScript = rocket.GetComponent<RocketScript>();
            rocketScript.Reusing(thrustForce_slider.Value, thrustTime_slider.Value * 10f, DragForce_slider.Value);
        }

        private void delayLaunch(GameObject gameObject)
        {                  
            gameObject.transform.localPosition += Vector3.right * 3.25f * transform.localScale.x;
            gameObject.transform.SetParent(transform.parent);
            gameObject.SetActive(true);
     
            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();      
            rigidbody.isKinematic = false;
            rigidbody.velocity = Rigidbody.velocity;
            rigidbody.AddRelativeForce(Vector3.right * 25f, ForceMode.Impulse);
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
            if (StatMaster.GodTools.InfiniteAmmoMode)
            {
                BulletCurrentNumber = BulletMaxNumber;
            }

            if (constraint)
            {
                for (int i = 0; i < BulletMaxNumber; i++)
                {
                    Rocket_Instantiate(i);
                }
            }

            if ((rocketPool.WorkCount < BulletCurrentNumber) && (rocketPool.WorkCount < rocketPool.Volume))
            {
                for (int i = 0; i < BulletMaxNumber; i++)
                {
                    if (!Rockets[i] || Rockets[i].GetComponent<RocketScript>().Launched)
                    {
                        if (rocketPool.IdleCount > 0)
                        {
                            Rocket_Reusing(i);
                        }
                        else
                        {
                            Rocket_Instantiate(i);
                        }
                        //BulletCurrentNumber = (int)Mathf.MoveTowards(BulletCurrentNumber, BulletMaxNumber, 1);
                    }
                }
            }



        }

        void fire_Network(Vector3 velocity, Guid guid)
        {
            throw new NotImplementedException();
        }

    }
}
