using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ModernFirearmKitMod
{
    class BulletPool
    {
        public int Volume { get; set; }
        public GameObject BulletTemp { get; set; }

        public int WorkCount { get { return Work.transform.childCount; } }
        public int IdleCount { get { return Idle.transform.childCount; } }

        public Transform Work;
        public Transform Idle;

        public BulletPool(Transform parent, GameObject bulletPool_Idle,int volume)
        {
            Work = new GameObject("Bullet Pool").transform;
            Work.transform.SetParent(parent);
            Work.transform.position = parent.position;
            Work.transform.rotation = parent.rotation;
            Idle = bulletPool_Idle.transform;
            Volume = volume;
        }

    }
}
