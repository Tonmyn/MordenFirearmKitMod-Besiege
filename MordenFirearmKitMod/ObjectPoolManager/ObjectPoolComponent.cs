//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace ModernFirearmKitMod
//{
//    /////////////// 教程地址: https://blog.csdn.net/andrewfan/article/details/56267144

//    public class ObjectPoolComponent : Pool_Base<PoolUnitBehavior, Pool_UnitList_Comp>
//    {
//        protected Transform ObjectPool;
//        protected Transform WorkList;
//        protected Transform IdleList;

//        void Awake()
//        {
//            ObjectPool = new GameObject("Object Pool").transform;
//            ObjectPool.transform.SetParent(transform);

//            if (WorkList == null)
//            {
//                WorkList = new GameObject("work").transform;
//                WorkList.SetParent(ObjectPool.transform);
//            }
//            if (IdleList == null)
//            {
//                IdleList = new GameObject("idle").transform;
//                IdleList.SetParent(ObjectPool.transform);
//                IdleList.gameObject.SetActive(false);
//            }     
//        }

//        public void OnUnitChangePool(PoolUnitBehavior unit)
//        {
//            if (unit != null)
//            {
//                var inPool = unit.State;
//                if (inPool == PoolUnitState.Idle)
//                {
//                    unit.transform.SetParent(IdleList);
//                }
//                else if (inPool == PoolUnitState.Work)
//                {
//                    unit.transform.SetParent(WorkList);
//                }
//            }
//        }

//        protected override Pool_UnitList_Comp createNewUnitList<UT>()
//        {
//            Pool_UnitList_Comp list = new Pool_UnitList_Comp();
//            list.setPool(this);
//            return list;
//        }


//    }


//    public class ObjectPool<Unit>:MonoBehaviour where Unit: PoolUnit
//    {
//        protected Transform Pool;
//        protected Transform WorkList;
//        protected Transform IdleList;

//        BulletPoolList bulletPool;

//        void Awake()
//        {
//            Pool = new GameObject("Object Pool").transform;
//            Pool.transform.SetParent(transform);

//            if (WorkList == null)
//            {
//                WorkList = new GameObject("work").transform;
//                WorkList.SetParent(Pool.transform);
//            }
//            if (IdleList == null)
//            {
//                IdleList = new GameObject("idle").transform;
//                IdleList.SetParent(Pool.transform);
//                IdleList.gameObject.SetActive(false);
//            }

//            bulletPool = new BulletPoolList();
//        }

//        public void OnUnitChangePool(PoolUnitBehavior unit)
//        {
//            if (unit != null)
//            {
//                var inPool = unit.State;
//                if (inPool == PoolUnitState.Idle)
//                {
//                    unit.transform.SetParent(IdleList);
//                }
//                else if (inPool == PoolUnitState.Work)
//                {
//                    unit.transform.SetParent(WorkList);
//                }
//            }
//        }

//        protected override Pool_UnitList_Comp createNewUnitList<UT>()
//        {
//            Pool_UnitList_Comp list = new Pool_UnitList_Comp();
//            list.setPool(this);
//            return list;
//        }


//    }
//}
