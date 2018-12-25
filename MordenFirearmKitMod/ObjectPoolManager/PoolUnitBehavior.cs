//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace ModernFirearmKitMod
//{
//    public abstract class PoolUnitBehavior : MonoBehaviour, IPoolUnit
//    {
//        //父列表对象
//        PoolUnit_List<PoolUnitBehavior> m_parentList;

//        /// <summary>返回一个单元状态，用于控制当前单元的闲置、工作状态</summary>
//        public PoolUnitState State { get; set; }

//        /// <summary>接受父列表对象的设置</summary>
//        public virtual void SetParentList(object parentList)
//        {
//            m_parentList = parentList as PoolUnit_List<PoolUnitBehavior>;
//        }

//        /// <summary>归还自己，即将自己回收以便再利用</summary>
//        public virtual void Restore()
//        {
//            if (m_parentList != null) { m_parentList.RestoreUnit(this); }
//        }
//    }
//}
