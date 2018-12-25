using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ModernFirearmKitMod
{
    //public abstract class Pool_Base<UnitType, UnitList> : MonoBehaviour where UnitType : class, IPoolUnit where UnitList : PoolUnit_List<UnitType>, new()
    //{
    //    /// <summary>缓冲池，按类型存放各自分类列表</summary>
    //    private Dictionary<Type, UnitList> m_poolTale = new Dictionary<Type, UnitList>();
        
    //    /// <summary>获取一个空闲的单元</summary>
    //    public T takeUnit<T>() where T : class, UnitType
    //    {
    //        UnitList list = getList<T>();
    //        return list.TakeUnit<T>() as T;
    //    }

    //    /// <summary>
    //    /// 在缓冲池中获取指定单元类型的列表，
    //    /// 如果该单元类型不存在，则立刻创建。
    //    /// </summary>
    //    /// <typeparam name="T">单元类型</typeparam>
    //    /// <returns>单元列表</returns>
    //    public UnitList getList<T>() where T : UnitType
    //    {
    //        var t = typeof(T);
    //        UnitList list = null;
    //        m_poolTale.TryGetValue(t, out list);
    //        if (list == null)
    //        {
    //            list = createNewUnitList<T>();
    //            m_poolTale.Add(t, list);
    //        }
    //        return list;
    //    }
    //    protected abstract UnitList createNewUnitList<UT>() where UT : UnitType;
    //}


    //public abstract class PoolBase<Unit, UnitList> : MonoBehaviour where Unit : PoolUnit where UnitList : PoolUnitList<Unit>, new()
    //{

    //}
}
