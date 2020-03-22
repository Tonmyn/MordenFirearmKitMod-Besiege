using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;
using System.ComponentModel;
using Modding.Blocks;

namespace ModernFirearmKitMod
{
    class RocketBlockScript :BlockScript
    {

        public RocketScript rocketScript;
        public Guid Guid { get; private set; } = Guid.NewGuid();
        #region 基本功能变量声明

        public MKey launch_key;

        MSlider thrustForce_slider;
        MSlider thrustTime_slider;
        MSlider thrustDelay_slider;
        MSlider DragForce_slider;

        //MSlider colliderDelay_slider;

        #endregion

        #region Network
        /// <summary>block,rocket guid</summary>
        public static MessageType LaunchMessage = ModNetworking.CreateMessageType(DataType.Block, DataType.String);
        #endregion

        private BlockHealthBar healthBar;
        private bool isExploded = false;
        public override void SafeAwake()
        {


            #region 基本功能参数初始化

            launch_key = AddKey(LanguageManager.Instance.CurrentLanguage.launch, "Launch", KeyCode.L);
            launch_key.KeysChanged += () => { changedPropertise(); };

            thrustForce_slider = AddSlider(LanguageManager.Instance.CurrentLanguage.thrustForce, "Thrust Force", 1, 0f, 10f);
            thrustForce_slider.ValueChanged += (value) => { changedPropertise(); };

            thrustTime_slider = AddSlider(LanguageManager.Instance.CurrentLanguage.thrustTime, "Thrust Time", 1, 0f, 10f);
            thrustTime_slider.ValueChanged += (value) => { changedPropertise(); };

            DragForce_slider = AddSlider(LanguageManager.Instance.CurrentLanguage.drag, "DRAG", 0.5f, 0.2f, 3f);
            DragForce_slider.ValueChanged += (value) => { changedPropertise(); };

            thrustDelay_slider = AddSlider(LanguageManager.Instance.CurrentLanguage.thrustDelay, "Thrust Delay", 0, 0f, 10f);
            thrustDelay_slider.ValueChanged += (value) => { changedPropertise(); };

            //colliderDelay_slider = AddSlider("碰撞开启 0.05s", "Collider Enable", 0f, 0f, 0.5f);
            //colliderDelay_slider.ValueChanged += (value) => { changedPropertise(); };

            #endregion

            healthBar = GetComponent<BlockHealthBar>();

            initRocketScript();

            changedPropertise();

            //DisplayInMapper(0);

        }

        void initRocketScript()
        {
            Rigidbody rigidbody = Rigidbody;
            rigidbody.centerOfMass += Vector3.forward * 0.5f;
            rocketScript = GetComponent<RocketScript>() ?? gameObject.AddComponent<RocketScript>();
            rocketScript.ThrustDirection = Vector3.right;
            rocketScript.ThrustPoint = rigidbody.centerOfMass;
            rocketScript.DelayEnableCollisionTime = 0.02f;
            rocketScript.ExplodePower = 1f;
            rocketScript.ExplodeRadius = 10f;
            rocketScript.effectOffset = new Vector3(-1.4f, 0, 0.5f);
            rocketScript.OnExplodeFinal += () => { Destroy(rocketScript.transform.gameObject); };
            rocketScript.OnExplode += () => { isExploded = true; healthBar.health = 0; };
        }

        void changedPropertise()
        {
            rocketScript.ThrustForce = thrustForce_slider.Value;
            rocketScript.ThrustTime = thrustTime_slider.Value * 10;
            rocketScript.DelayLaunchTime = thrustDelay_slider.Value * 0.1f;
            rocketScript.DragClamp = DragForce_slider.Value;
        }

        public override void SimulateUpdateAlways()
        {
            if ((launch_key.IsPressed || launch_key.EmulationPressed())&& !rocketScript.Launched)
            {
                rocketScript.LaunchEnabled = true;
                if (StatMaster.isHosting)
                {
                    var message = LaunchMessage.CreateMessage(BlockBehaviour, rocketScript.Guid.ToString());
                    ModNetworking.SendToAll(message);
                }
            }

            if (healthBar.health <= 0 && isExploded == false)
            {
                rocketScript.Explody();
            }
        }


        public override void OnStartBurning()
        {
            rocketScript.Explody();
        }

        public static void LaunchNetworkEvent(Message message)
        {
            if (StatMaster.isClient)
            {
                var block = (Block)message.GetData(0);
                var guid = new Guid(message.GetData(1).ToString());

                var rs = block.GameObject.GetComponent<RocketScript>();
                rs.Guid = guid;
            }
        }
    }
}
