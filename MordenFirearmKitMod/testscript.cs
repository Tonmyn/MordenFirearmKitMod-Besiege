using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;

namespace ModernFirearmKitMod
{
    class testscript :BlockScript
    {

        public override void SimulateUpdateHost()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                BesiegeConsoleController.ShowMessage("test script");
            }
        }

    }
}
