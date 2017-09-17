using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MordenFirearmKitMod
{
    /// <summary>发射器基类</summary> 
    public class LauncherScript : MonoBehaviour
    {

        //子弹
        //public Bullet bullet;

        //散布
        //public float Diffuse;

        //弹药量上限
        public int bulletLimit { get; set; }

        //实际弹药量
        public int bulletNumber { get; private set; }

        //射速
        public float FireRate;

        //后座力
        public float KnockBack = 1;



        //发射时间间隔
        internal float Interval;

        //允许发射
        public bool shootable = false;

        //扳机
        public MKey Trigger;

        //枪口位置
        public Vector3 GunPoint;

        ///<summary>子弹组件</summary>
        public GameObject Bullet;

        //枪的刚体组件
        public Rigidbody rigidbody;

        //枪的关节组件
        public ConfigurableJoint CJ;


        public virtual void Awake()
        {

            rigidbody = GetComponent<Rigidbody>();
            rigidbody.mass = 0.5f;

            CJ = GetComponent<ConfigurableJoint>();

        }


        public virtual void Start()
        {
            bulletNumber = bulletLimit;
        }

        public virtual void Update()
        {

            if (StatMaster.GodTools.InfiniteAmmoMode)
            {
                bulletNumber = bulletLimit;
            }

            if (Trigger.IsDown && bulletNumber > 0 && shootable)
            {

                if (Interval >= FireRate && Time.timeScale != 0)
                {
                    Interval = 0;
                    shoot();

                    return;
                }
                else
                {
                    Interval += Time.deltaTime;
                }
            }

        }

        public virtual void shoot()
        {

            bulletNumber--;

            rigidbody.AddForce(-transform.forward * KnockBack * 2000f);

            GameObject bullet = (GameObject)Instantiate(Bullet, transform.TransformPoint(GunPoint), transform.rotation);

            bullet.transform.SetParent(Machine.Active().SimulationMachine);

            bullet.SetActive(true);

        }

    }
}

