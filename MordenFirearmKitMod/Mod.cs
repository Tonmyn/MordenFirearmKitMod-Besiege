using Modding;
using System;
using System.Collections.Generic;
using UnityEngine;
using Modding.Common;
using Modding.Blocks;
using System.Collections;

namespace ModernFirearmKitMod
{

    // If you need documentation about any of these values or the mod loader
    // in general, take a look at http://wiki.spiderlinggames.co.uk/besiege/modding-documentation/articles/introduction.html.

    public enum BlockList
    {
        火箭弹模块 = 650,
        火箭巢模块 = 651,
        机枪模块 = 652,
        指向模块 = 653
    }

    public class MordenFirearmKitBlockMod : ModEntryPoint
    {

        public static GameObject Mod;
        public static GameObject RocketPool_Idle;

        public override void OnLoad()
        {
            // Your initialization code here

            //添加MOD更新推送功能
            //new GameObject("Mod更新组件").AddComponent<Updater>().SetUrl(Author, Name);

            //注册控制台指令
            //Commands.RegisterCommand("mfk", ConfigurationCommand, "MordernFirearmKit Mod configuration command.\n");

            //LoadBlock(RocketBlock);
            //LoadBlock(RocketPodBlock);
            //LoadBlock(MachineGun);
            //LoadBlock(direction);

            Mod = new GameObject("Morden Firearm Kit Mod");
            UnityEngine.Object.DontDestroyOnLoad(Mod);
            RocketPool_Idle = new GameObject("Rocket Pool Idle");
            RocketPool_Idle.transform.SetParent(Mod.transform);

            AssetManager.Instance.transform.SetParent(Mod.transform);

            if (RocketPodBlockScript.RocketTemp == null)
            {
                RocketPodBlockScript.CreateRocketBlockTemp();
            }

            //new GameObject("test").AddComponent<test>();



            //增加灯关渲染数量
            //QualitySettings.pixelLightCount += 10;

        }
    

        /// <summary>
        /// MOD控制台指令
        /// </summary>
        /// <param name="args">输入的字符串</param>
        /// <param name="namedArgs"></param>
        /// <returns></returns>
        //public static string ConfigurationCommand(string[] args, IDictionary<string, string> namedArgs)
        //{
        //    if (args.Length <= 0)
        //        return "可用命令:\n" +
        //               "  mfk 子弹类型  \t 列出所有子弹类型\n" +
        //               "  mfk 弹链      \t 如何使用弹链滑条\n" +
        //               "  mfk 音量      \t 设置加特林转动音量\n";
        //    switch (args[0].ToLower())
        //    {
        //        case "子弹类型":
        //            string str = "\n";
        //            for (int i = 0; i < BulletScript.GetBulletKindNumber(); i++)
        //            {
        //                str += (i + 1).ToString() + "." + BulletScript.GetBulletKind(i + 1).ToString() + "\n";
        //            }
        //            return str;

        //        case "弹链":
        //            return "\n输入子弹类型相应数值即可调整弹链\n比如输入: 112\n弹链即设置为两发破坏弹一发曳光弹不断循环发射\n如果你不知道有哪些子弹类型输入:mfk 子弹类型 来查看\n";

        //        case "音量":
        //            if (args.Length > 1)
        //            {
        //                try
        //                {
        //                    float volume = Mathf.Clamp01(Convert.ToSingle(args[1]));
        //                    MGLauncher.Volume = volume;
        //                    return "\n音量调整已生效\n";
        //                }
        //                catch
        //                {
        //                    return "\n输入的什么鬼! 输入:mfk 音量 0-1 之间的数来调整加特林转动音量...\n";
        //                }
        //            }
        //            return "\n输入:mfk 音量 0-1 之间的数来调整加特林转动音量\n";
        //        default:
        //            return "\n输入mfk查看可用指令";

        //    }
        //}

    }

    public class test : MonoBehaviour
    {
        GameObject Cube;
        GameObject Cube1;

        public float StartDelay = 0;
        public float TimeDelayToReactivate = 3;

        void Start()
        {
            //InvokeRepeating("Reactivate", StartDelay, TimeDelayToReactivate);

          

            //StartCoroutine(AssetManager.createVFX());
            //StartCoroutine(load());
        }

        void Reactivate()
        {
            if (Cube)
            {
                Cube.gameObject.SetActive(false);
                Cube.gameObject.SetActive(true);
            }
   
        }

        void Update()
        {

            if (Input.GetKeyDown(KeyCode.I))
            {
                Debug.Log("input");

                //AssetBundle assetBundle = ModResource.GetAssetBundle("Effect");
                //Cube = (GameObject)Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>(/*"example"*//*"M4A1"*/"MachineGunEffect"));
                
                //Cube = (GameObject)Instantiate<GameObject>(TempManager.mgb);
                //Cube.transform.position = new Vector3(0, 3, 0);

                Instantiate(Cube1).transform.position = new Vector3(3, 3, 0);

            }

            //if(Input.)

        }

        IEnumerator load()
        {
            var assetBundle = ModResource.GetAssetBundle("Effect");
            yield return new WaitUntil(() => assetBundle.Available);
            Cube1 = assetBundle.LoadAsset<GameObject>(/*"example"*//*"M4A1"*/"BigExplosion");
            Cube1.AddComponent<MeshFilter>().mesh = ModResource.GetMesh("Rocket Mesh");
            Cube1.AddComponent<MeshRenderer>().material.color = Color.red;
            Cube1.AddComponent<Rigidbody>();
            Cube1.AddComponent<BoxCollider>();
            Cube1.AddComponent<explosion>();

        }
    }

    class explosion : MonoBehaviour
    {
        /// <summary>爆炸半径</summary>
        public float Radius { get; set; } = 3f;
        /// <summary>爆炸威力</summary>
        public float Force { get; set; } = 10000f;


        public IEnumerator explode()
        {
            int index = 0;


            //定义爆炸位置为炸弹位置
            Vector3 explosionPos = transform.position;
            //这个方法用来反回球型半径之内（包括半径）的所有碰撞体collider[]
            Collider[] colliders = Physics.OverlapSphere(explosionPos, Radius);

            //遍历返回的碰撞体，如果是刚体，则给刚体添加力
            foreach (Collider hit in colliders)
            {
                if (hit.attachedRigidbody != null)
                {
                    float force = UnityEngine.Random.Range(10000f, 50000f);
                    hit.attachedRigidbody.AddExplosionForce(Force, explosionPos, Radius);
                    if (++index > 30) { yield return 0; }
                }
         
            }

            //销毁炸弹和小球
            //Destroy(col.gameObject);
            Destroy(gameObject);
        }

        void OnCollisionEnter(Collision col)
        {

            //StartCoroutine(explode());

        }
    }

}
