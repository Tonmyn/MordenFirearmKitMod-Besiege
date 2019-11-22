using Modding;
using ModernFirearmKitMod.GenericScript.RayGun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ModernFirearmKitMod
{
    public class NetworkMessageManager:SingleInstance<NetworkMessageManager>
    {
        //public static Dictionary<int, string> keyValuePairs = new Dictionary<int, string>();

        public override string Name { get; } = "Network Message Manager";
        void Awake()
        {
            ModNetworking.Callbacks[GatlingGunBlockScript.FireMessage] += GatlingGunBlockScript.FireNetworkingEvent;
            ModNetworking.Callbacks[GunBarrelBlockScript.FireMessage] += GunBarrelBlockScript.FireNetworkingEvent;
            ModNetworking.Callbacks[MachineGunBlockScript.FireMessage] += MachineGunBlockScript.FireNetworkingEvent;
            ModNetworking.Callbacks[RayBulletScript.ImpactMessage] += RayBulletScript.ImpactNetworkingEvent;

            ModNetworking.Callbacks[RocketScript.ExplodeMessage] += RocketScript.ExplodeNetworkingEvent;
            ModNetworking.Callbacks[RocketBlockScript.LaunchMessage] += RocketBlockScript.LaunchNetworkEvent;
            ModNetworking.Callbacks[ExplodeScript.ExplodyMessage] += ExplodeScript.Explody_Network;
        }     
    }
}
