using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modding;
using UnityEngine;

namespace ModernFirearmKitMod
{
    public abstract class LauncherBlockScript : BlockScript
    {

        /// <summary>发射按键</summary>
        public MKey LaunchKey;

        /// <summary>子弹物体</summary>
        public abstract GameObject Bullet { get; set; }
        /// <summary>枪口位置</summary>
        public abstract Vector3 SpawnPoint { get; set; }
        /// <summary>最大弹药数</summary>
        public abstract int BulletMaxNumber { get; set; }
        /// <summary>当前弹药数</summary>
        public abstract int BulletCurrentNumber { get; set; }
        /// <summary>射速</summary>
        public abstract float Rate { get; set; }
        /// <summary>后座</summary>
        public abstract float KnockBack { get; set; }

        public abstract bool ShootEnable { get; set; }

        public Action LaunchEvent;

        public IEnumerator Launch()
        {
            if (BulletCurrentNumber <= 0||!ShootEnable) yield break;

            LaunchEvent?.Invoke();

            BulletCurrentNumber--;

            Rigidbody.AddForce(-transform.forward * KnockBack /** 4000f*/, ForceMode.Impulse);

            GameObject bullet = (GameObject)Instantiate(Bullet, transform.TransformPoint(SpawnPoint), transform.rotation);

            bullet.SetActive(true);

            yield return new WaitForSeconds(Rate);

            ShootEnable = false;
        }

    }
}
