using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModernFirearmKitMod
{

    public interface IPoolUnit
    {
        UnitState State();
        void SetParentList(object parentList);
        void Restore();
    }

    public enum PoolUnitState
    {
        Idle,
        Work
    }

    public class UnitState
    {
        public PoolUnitState InPool { get; set; }
    }
}
