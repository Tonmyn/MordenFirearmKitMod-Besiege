using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;


namespace ModernFirearmKitMod
{
    class RocketPodBlockScript : RocketBlockScript
    {

        public static GameObject RocketTemp;

        #region 功能变量声明

        //MKey launch_key;

        //声明 滑条 载弹数量
        protected MSlider number_slider;

        //声明 载弹数量
        public int number = 18;

        //声明 滑条 连发间隔
        protected MSlider interval_slider;

        //声明 连发间隔(单位：秒)
        public float interval = 2;

        #endregion

        #region 内部变量声明

        //声明 火箭弹
        private GameObject[] Rockets = new GameObject[18];

        //声明 火箭弹标签
        private int Label = 0;

        //声明 火箭弹实例化位置
        private Vector3[] position_rocket = new Vector3[18];

        //声明 火箭弹刚体
        //private Rigidbody[] rb = new Rigidbody[18];

        //声明 连发开启
        private bool continued = false;

        private CountDownScript continuedCD;

        #endregion

        public override void SafeAwake()
        {
            

            //launch_key = AddKey("发射", "Launch", KeyCode.L);

            //添加 滑条 参数
            number_slider = AddSlider("载弹数量", "NUMBER", number, 1, 18);
            number_slider.ValueChanged += (value) => { number = (int)value; };
            interval_slider = AddSlider("连发间隔 0.1s", "INTERVAL", interval, 1f, 5f);
            interval_slider.ValueChanged += (value)=> { interval = value; };

            base.SafeAwake();
            functionPage_menu.Items.Insert(0,"火箭巢参数");
            rocketScript.enabled = false;

            continuedCD = gameObject.AddComponent<CountDownScript>();
            continuedCD.Time = interval * 10f;
            continuedCD.CountDownCompleteEvent += () => { continued = true; };     

        }

         void changedPropertise()
        {
           


        }

        public override void DisplayInMapper(int value)
        {
            base.DisplayInMapper(value);

            bool show = ("火箭巢参数" == functionPage_menu.Items[value]);

            launch_key.DisplayInMapper = show;
            number_slider.DisplayInMapper = show;
            interval_slider.DisplayInMapper = show;
            
            thrustDelay_slider.DisplayInMapper = false;
            
        }

        public override void OnSimulateStart()
        {

            number = Mathf.Clamp(number, 1, 18);
            Rocket_Position();
            for (int i = 0; i < number; i++)
            {
                Rocket_Instantiate(i);
            }

        }

        public override void SimulateUpdateHost()
        {
            Rocket_Reload();

            if (launch_key.IsPressed)
            {
                Rocket_Launch();
                continuedCD.TimeSwitch = true;
               
            }

            if (continued && launch_key.IsDown)
            {
                continued = false;
                Rocket_Launch();
                continuedCD.TimeSwitch = true;
            }

            if (launch_key.IsReleased)
            {
                continued = continuedCD.TimeSwitch = false;
            }
        }

        public override void SimulateFixedUpdateHost()
        {
   
        }

        //火箭弹实例化位置计算
        private void Rocket_Position()
        {
            int i;

            //声明 原点
            Vector2 origin = new Vector2(0.4f, 0);

            //声明 大圆半径、角度差和旋转角
            float radius_large = 0.37f, angle_large = 30f;

            //声明 小圆半径、角度差和旋转角
            float radius_little = 0.19f, angle_little = 60f;

            //外圈火箭弹位置
            for (i = 0; i < 12; i++)
            {
                position_rocket[i] = new Vector3(
                                                    0,
                                                    origin.y + radius_large * Mathf.Sin(angle_large * i * Mathf.Deg2Rad),
                                                    origin.x - radius_large * Mathf.Cos(angle_large * i * Mathf.Deg2Rad)
                                                 );
            }

            //内圈火箭弹位置
            for (i = 0; i < 6; i++)
            {
                position_rocket[i + 12] = new Vector3(
                                                        0,
                                                        origin.y + radius_little * Mathf.Sin((angle_little * i + 30) * Mathf.Deg2Rad),
                                                        origin.x - radius_little * Mathf.Cos((angle_little * i + 30) * Mathf.Deg2Rad)
                                                      );
            }
        }

        //火箭弹实例化
        private void Rocket_Instantiate(int label)
        {

            //火箭弹安装位置 本地坐标转世界坐标
            Vector3 offset = new Vector3(-1.375f, 0f, 0.15f);
            Vector3 pos = transform.TransformVector(transform.InverseTransformVector(rigidbody.position) + offset + position_rocket[label]);

            //火箭弹实例化 设置连接点失效
            GameObject Rocket = /*new GameObject("Rocket")*/(GameObject)Instantiate(RocketTemp, pos, transform.rotation,transform);
            Rocket.SetActive(true);
            //Rockets.transform.SetParent(transform);
            //Rockets.AddComponent<Rigidbody>();
            //Rockets.AddComponent<RocketScript>();
            
            //Destroy(Rockets.GetComponent<ConfigurableJoint>());

            //火箭弹刚体 不开启碰撞 不受物理影响
            Rigidbody rb = Rocket.GetComponent<Rigidbody>();
            rb.detectCollisions = false;
            rb.isKinematic = true;

            //设置火箭弹大小
            Rocket.transform.localScale = new Vector3(1f, 0.5f, 0.5f);

            //火箭弹脚本 初始化
            RocketScript rs = Rocket.GetComponent<RocketScript>();
            rs = rocketScript;
            //rs.thruster = rocketScript.thruster;

            Rockets[label] = Rocket;

            //rs.enabled = true;
            ////rs.explosiontype = explosiontype;
            //rs.thruster.ThrustTime = time;
            //rs.power = power;
            //rs.thrust = thrust;
            //rs.drag = drag;
            //rs.timeopen = timeopen;

            ////火箭弹尾焰初始化
            //rbs.psp_fire.lifetime = lifetime_fire.Value;
            //rbs.psp_fire.radius = radius_fire.Value;
            //rbs.psp_fire.angle = angle_fire.Value;
            //rbs.psp_fire.size = size_fire.Value;
            //rbs.psp_fire.size_start = sizeStart_fire.Value;
            //rbs.psp_fire.size_end = sizeEnd_fire.Value;
            //rbs.psp_fire.color_start = colorStart_fire.Value;
            //rbs.psp_fire.color_end = colorEnd_fire.Value;
            //rbs.psp_fire.color_startTime = colorStartTime_fire.Value;
            //rbs.psp_fire.color_endTime = colorEndTime_fire.Value;

            ////火箭弹尾烟初始化
            //rbs.psp_smoke.color_start = rbs.psp_smoke.color_end = colorSmoke_valueChanged(colorsmoke_menu.Value);
            //rbs.psp_smoke.lifetime = lifetime_smoke.Value;
            //rbs.psp_smoke.angle = angle_smoke.Value;
            //rbs.psp_smoke.size = size_smoke.Value;
            //rbs.psp_smoke.size_start = sizeStart_smoke.Value;
            //rbs.psp_smoke.size_end = sizeEnd_smoke.Value;

            //Rockets[label] = Instantiate(PrefabMaster.BlockPrefabs[1000].gameObject);
        }    

        //火箭弹发射
        public void Rocket_Launch()
        {

            Rocket_LaunchPlan(Label);

            //火箭弹标签回零
            if (++Label > number - 1)
            {
                Label = 0;
            }

            //火箭弹发射准备
            void Rocket_LaunchPlan(int label)
            {
                GameObject Rocket = Rockets[label];

                //火箭弹不存在即返回
                if (Rocket == null || Rocket.GetComponent<RocketScript>().launched) return;

                //火箭弹发射位置
                Vector3 pos = transform.TransformVector(transform.InverseTransformVector(rigidbody.position) + position_rocket[label] + new Vector3(3f, 0, 0));

                //火箭弹移动到发射位置
                Rocket.transform.position = pos;

                //火箭巢本地速度
                Vector3 local_velocity = transform.InverseTransformDirection(rigidbody.velocity);
                Rigidbody rb = Rocket.GetComponent<Rigidbody>();

                //火箭弹继承火箭巢速度
                rb.velocity = transform.TransformDirection(Vector3.Scale(local_velocity, new Vector3(1.2f * -Mathf.Sign(local_velocity.x), 0.15f, 0.15f)));

                //火箭弹受物理影响
                rb.isKinematic = false;

                //火箭弹开启碰撞
                rb.detectCollisions = true;

                RocketScript rocketScript = Rocket.GetComponent<RocketScript>();
                rocketScript.enabled = true;
                rocketScript = this.rocketScript;
                rocketScript.launched = true;
                //Debug.Log(rocketScript.thruster.ThrustTime);

            }
        }

        //火箭弹重装
        private void Rocket_Reload()
        {
            //火箭弹在无限弹药模式下 有空位的情况下实例化新火箭弹
            if (StatMaster.GodTools.InfiniteAmmoMode)
            {
                for (int i = 0; i < number; i++)
                {
                    if (!Rockets[i] || Rockets[i].GetComponent<RocketBlockScript>().rocketScript.launched)
                    {
                        Rocket_Instantiate(i);
                    }
                }
            }
        }


        internal static void CreateRocketBlockTemp()
        {
            RocketTemp = new GameObject("Rocket Temp");
            RocketTemp.AddComponent<MeshFilter>().mesh = ModResource.GetMesh("Rocket Mesh");
            RocketTemp.AddComponent<MeshRenderer>().material.mainTexture = ModResource.GetTexture("Rocket Texture");
            RocketTemp.AddComponent<Rigidbody>();
            CapsuleCollider capsuleCollider = RocketTemp.AddComponent<CapsuleCollider>();
            capsuleCollider.radius = 0.15f;
            capsuleCollider.height = 2.5f;
            capsuleCollider.direction = 0;
            RocketTemp.AddComponent<RocketScript>();
            RocketTemp.SetActive(false);

            Debug.Log("create");
        }

    }
}
