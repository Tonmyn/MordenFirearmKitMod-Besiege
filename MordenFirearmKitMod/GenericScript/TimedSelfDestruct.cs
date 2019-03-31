using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ModernFirearmKitMod
{
    class TimedSelfDestruct:MonoBehaviour
    {
        /// <summary>存活时间 0.1秒</summary>
        public float lifeTime { get; set; } 
        public bool Switch { get; set; } = false;

        public Action OnDestruct;

        void Awake()
        {
            lifeTime = 300;
        }

        void Update()
        {
            if (Switch)
            {
                Switch = false;
                StartCoroutine(Timer(lifeTime));
            }
        }

        IEnumerator Timer(float t)
        {
            yield return new WaitForSeconds(t / 10f);
            if (gameObject)
            {
                OnDestruct?.Invoke();
            }
        }
    }
}
