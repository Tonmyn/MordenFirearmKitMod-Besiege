using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModernFirearmKitMod
{
    //public abstract class PoolUnit_List<T> where T : class , IPoolUnit
    //{
    //    protected object m_template;
    //    protected List<T> m_idleList;
    //    protected List<T> m_workList;
    //    protected int m_createdNum = 0;

    //    protected event Action<T> OnUnitChangePool;

    //    public PoolUnit_List()
    //    {
    //        m_idleList = new List<T>();
    //        m_workList = new List<T>();
    //    }

    //    /// <summary>获取一个闲置的单元，如果不存在则创建一个新的</summary>
    //    public virtual T TakeUnit<UT>() where UT : T
    //    {
    //        T unit;
    //        if (m_idleList.Count > 0)
    //        {
    //            unit = m_idleList[0];
    //            m_idleList.RemoveAt(0);
    //        }
    //        else
    //        {
    //            unit = CreateNewUnit<UT>();
    //            unit.SetParentList(this);
    //            m_createdNum++;
    //        }
    //        m_workList.Add(unit);
    //        unit.State = PoolUnitState.Work;
    //        OnUnitChangePool?.Invoke(unit);
    //        return unit;
    //    }
    //    /// <summary>归还某个单元</summary>
    //    public virtual void RestoreUnit(T unit)
    //    {
    //        if (unit != null && unit.State == PoolUnitState.Work)
    //        {
    //            m_workList.Remove(unit);
    //            m_idleList.Add(unit);
    //            unit.State = PoolUnitState.Idle;
    //            OnUnitChangePool?.Invoke(unit);
    //        }
    //    }

    //    /// <summary>设置模板</summary>
    //    public void SetTemplate(object template)
    //    {
    //        m_template = template;
    //    }

    //    protected abstract T CreateNewUnit<UT>() where UT : T;
    //}

    //public abstract class PoolUnitList<T> where T : PoolUnit
    //{
    //    protected object m_template;
    //    protected List<T> m_idleList;
    //    protected List<T> m_workList;
    //    protected int m_createdNum = 0;

    //    protected event Action<T> OnUnitChangePool;

    //    public PoolUnitList()
    //    {
    //        m_idleList = new List<T>();
    //        m_workList = new List<T>();
    //    }

    //    /// <summary>获取一个闲置的单元，如果不存在则创建一个新的</summary>
    //    public virtual T TakeUnit<UT>() where UT : T
    //    {
    //        T unit;
    //        if (m_idleList.Count > 0)
    //        {
    //            unit = m_idleList[0];
    //            m_idleList.RemoveAt(0);
    //        }
    //        else
    //        {
    //            unit = CreateNewUnit<UT>();
    //            unit.SetParentList(this);
    //            m_createdNum++;
    //        }
    //        m_workList.Add(unit);
    //        unit.State = UnitState.Work;
    //        OnUnitChangePool?.Invoke(unit);
    //        return unit;
    //    }
    //    /// <summary>归还某个单元</summary>
    //    public virtual void RestoreUnit(T unit)
    //    {
    //        if (unit != null && unit.State == UnitState.Work)
    //        {
    //            m_workList.Remove(unit);
    //            m_idleList.Add(unit);
    //            unit.Restore();
    //            OnUnitChangePool?.Invoke(unit);
    //        }
    //    }

    //    /// <summary>设置模板</summary>
    //    public void SetTemplate(object template)
    //    {
    //        m_template = template;
    //    }

    //    protected abstract T CreateNewUnit<UT>() where UT : T;
    //}


}
