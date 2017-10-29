using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MordenFirearmKitMod
{
    /// <summary>到时自毁脚本 </summary> 
    public class TimedSelfDestruct : MonoBehaviour
    {
        float timer = 0;

        /// <summary>存活时间 0.1秒</summary>
        public float lifeTime = 300;

        private void Start()
        {
            StartCoroutine(Timer(lifeTime));
        }

        public void TimedDestroySelf(float t)
        {
            StartCoroutine(Timer(t));
        }

        IEnumerator Timer(float t)
        {

            yield return new WaitForSeconds(t / 10f);
            if (gameObject)
            {
                Destroy(gameObject);
            }

            if (GetComponent<TimedRocket>())
            {
                Destroy(GetComponent<TimedRocket>());
            }

        }
    }
}
