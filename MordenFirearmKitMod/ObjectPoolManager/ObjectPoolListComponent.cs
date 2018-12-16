using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ModernFirearmKitMod
{
  
    public class Pool_UnitList_Comp : PoolUnitList<Pooled_BehaviorUnit>
    {
        protected ObjectPoolComponent m_pool;
        public void setPool(ObjectPoolComponent pool)
        {
            m_pool = pool;
        }
        protected override Pooled_BehaviorUnit CreateNewUnit<UT>()
        {
            GameObject result_go = null;
            if (m_template != null && m_template is GameObject)
            {
                result_go = GameObject.Instantiate((GameObject)m_template);
            }
            else
            {
                result_go = new GameObject();
                result_go.name = typeof(UT).Name;
            }
            result_go.name = result_go.name + "_" + m_createdNum;
            UT comp = result_go.GetComponent<UT>();
            if (comp == null)
            {
                comp = result_go.AddComponent<UT>();
            }
            //comp.DoInit();
            return comp;
        }

        protected override void OnUnitChangePool(Pooled_BehaviorUnit unit)
        {
            if (m_pool != null)
            {
                m_pool.OnUnitChangePool(unit);
            }
        }
    }

    public abstract class Pooled_BehaviorUnit : MonoBehaviour, IPoolUnit
    {
        //单元状态对象
        protected UnitState m_unitState = new UnitState();
        //父列表对象
        PoolUnitList<Pooled_BehaviorUnit> m_parentList;
        /// <summary>
        /// 返回一个单元状态，用于控制当前单元的闲置、工作状态
        /// </summary>
        /// <returns>单元状态</returns>
        public virtual UnitState State()
        {
            return m_unitState;
        }
        /// <summary>
        /// 接受父列表对象的设置
        /// </summary>
        /// <param name="parentList">父列表对象</param>
        public virtual void SetParentList(object parentList)
        {
            m_parentList = parentList as PoolUnitList<Pooled_BehaviorUnit>;
        }
        /// <summary>
        /// 归还自己，即将自己回收以便再利用
        /// </summary>
        public virtual void Restore()
        {
            if (m_parentList != null)
            {
                m_parentList.restoreUnit(this);
            }
        }

    }

}
