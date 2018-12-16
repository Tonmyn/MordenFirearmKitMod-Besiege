using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ModernFirearmKitMod
{
    /////////////// 教程地址: https://blog.csdn.net/andrewfan/article/details/56267144

    public class ObjectPoolComponent : Pool_Base<Pooled_BehaviorUnit, Pool_UnitList_Comp>
    {
        [SerializeField]
        [Tooltip("运行父节点")]
        protected Transform m_work;
        [SerializeField]
        [Tooltip("闲置父节点")]
        protected Transform m_idle;

        private GameObject ObjectPool;

        void Awake()
        {
            ObjectPool = new GameObject("Object Pool");

            if (m_work == null)
            {
                m_work = new GameObject("work").transform;
                m_work.SetParent(ObjectPool.transform);
            }
            if (m_idle == null)
            {
                m_idle = new GameObject("idle").transform;
                m_idle.SetParent(ObjectPool.transform);
                m_idle.gameObject.SetActive(false);
            }
        }

        public void OnUnitChangePool(Pooled_BehaviorUnit unit)
        {
            if (unit != null)
            {
                var inPool = unit.State().InPool;
                if (inPool == PoolUnitState.Idle)
                {
                    unit.transform.SetParent(m_idle);
                }
                else if (inPool == PoolUnitState.Work)
                {
                    unit.transform.SetParent(m_work);
                }
            }
        }
        protected override Pool_UnitList_Comp createNewUnitList<UT>()
        {
            Pool_UnitList_Comp list = new Pool_UnitList_Comp();
            list.setPool(this);
            return list;
        }


    }
}
