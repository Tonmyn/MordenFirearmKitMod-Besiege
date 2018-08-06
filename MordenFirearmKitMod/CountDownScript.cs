using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ModernFirearmKitMod
{
    public class CountDownScript : MonoBehaviour
    {
        /// <summary>
        /// 倒计时时间ms
        /// </summary>
        public float Time;

        /// <summary>
        /// 倒计时开关
        /// </summary>
        public bool TimeSwitch = false;

        /// <summary>
        /// 倒计时完成要做的事
        /// </summary>
        public Action CountDownCompleteEvent;

        private bool isClick = false;

        void Awake()
        {
            CountDownCompleteEvent += () => { BesiegeConsoleController.ShowMessage("Count Down Complete..."); };
        }


        void Update()
        {
            if (TimeSwitch && isClick == false)
            {
                isClick = true;
                StartCoroutine(Timer(Time));
            }

        }

        IEnumerator Timer(float time)
        {
            //延时发射
            while (time-- > 0 && TimeSwitch)
            {
                yield return new WaitForSeconds(0.001f);
            }
            if (time <= 0)
            {
                TimeSwitch = false;
                CountDownCompleteEvent();
            }
        }

    }
}
