//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace ModernFirearmKitMod
//{
  
//    public class Pool_UnitList_Comp : PoolUnit_List<PoolUnitBehavior>
//    {
//        protected ObjectPoolComponent m_pool;

//        public void setPool(ObjectPoolComponent pool)
//        {
//            m_pool = pool;
//        }

//        protected override PoolUnitBehavior CreateNewUnit<UT>()
//        {
//            GameObject result_go = null;
//            if (m_template != null && m_template is GameObject)
//            {
//                result_go = UnityEngine.Object.Instantiate((GameObject)m_template);
//            }
//            else
//            {
//                result_go = new GameObject();
//                result_go.name = typeof(UT).Name;
//            }
//            result_go.name = result_go.name + "_" + m_createdNum;
//            UT comp = result_go.GetComponent<UT>();
//            if (comp == null)
//            {
//                comp = result_go.AddComponent<UT>();
//            }
//            //comp.DoInit();
//            return comp;
//        }

//        public Pool_UnitList_Comp() : base()
//        {
//            OnUnitChangePool += (unit) => { if (m_pool != null) m_pool.OnUnitChangePool(unit); };
//        }

//        //protected void OnUnitChangePool(Pooled_BehaviorUnit unit)
//        //{
//        //    if (m_pool != null)
//        //    {
//        //        m_pool.OnUnitChangePool(unit);
//        //    }
//        //}
//    }

//    public class BulletPoolList : PoolUnitList<PoolUnit>
//    {
//        public BulletPoolList() : base()
//        {
//            base.PoolUnitList<PoolUnit>();
//        }

//        protected override PoolUnit CreateNewUnit<UT>()
//        {
//            GameObject result_go = null;
//            if (m_template != null && m_template is GameObject)
//            {
//                result_go = UnityEngine.Object.Instantiate((GameObject)m_template);
//            }
//            else
//            {
//                result_go = new GameObject();
//                result_go.name = typeof(UT).Name;
//            }
//            result_go.name = result_go.name + "_" + m_createdNum;
//            UT comp = result_go.GetComponent<UT>();
//            if (comp == null)
//            {
//                comp = result_go.AddComponent<UT>();
//            }
//            //comp.DoInit();
//            return comp;
//        }

//    }


//}
