//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace ModernFirearmKitMod
//{

//    public interface IPoolUnit
//    {
//        PoolUnitState State { get; set; }

//        void SetParentList(object parentList);
//        void Restore();
//    }
//    public enum PoolUnitState
//    {
//        Idle,
//        Work
//    }

//    //public class UnitState
//    //{
//    //    public PoolUnitState InPool { get; set; }
//    //}

//    public enum UnitState
//    {
//        Idle,
//        Work
//    }
//    public abstract class PoolUnit : MonoBehaviour
//    {
       
//        public UnitState State { get; set; }

//        //父列表对象
//        PoolUnitList<PoolUnit> ParentList;

//        /// <summary>接受父列表对象的设置</summary>
//        public virtual void SetParentList(object parentList)
//        {
//            ParentList = parentList as PoolUnitList<PoolUnit>;
//        }

//        /// <summary>归还自己，即将自己回收以便再利用</summary>
//        public virtual void Restore()
//        {
//            if (ParentList != null) { ParentList.RestoreUnit(this);this.State = UnitState.Idle; }
//        }

//    }
//}
