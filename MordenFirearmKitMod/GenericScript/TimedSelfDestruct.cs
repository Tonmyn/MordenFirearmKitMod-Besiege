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
            yield return new WaitForSeconds(t * 0.1f);
            if (gameObject)
            {
                
                OnDestruct?.Invoke();
                Destroy(gameObject);
            }
        }
        /// <summary>
        /// 开始销毁倒计时(0.1s)
        /// </summary>
        /// <param name="lifeTime">单位0.1s</param>
        public void Begin(float lifeTime)
        {
            StartCoroutine(Timer(lifeTime));
        }
    }
}
