using System;
using System.Collections.Generic;
using spaar.ModLoader;
using TheGuysYouDespise;
using UnityEngine;
using System.Collections;

namespace MordenFirearmKitMod.Blocks
{

    partial class MordenFirearmKitMod
    {

        //声明火箭弹模块
        public Block RocketBlock = new Block()
        #region 火箭弹模块 基本属性  

           //模块 ID
           .ID(650)

           //模块 名称
           .BlockName("Rocket Block")

           //模块 模型信息
           .Obj(new System.Collections.Generic.List<Obj>
                   { new Obj(
                                "/RocketBlockMod/Rocket.obj",
                                "/RocketBlockMod/Rocket.png",
                                new VisualOffset(
                                                    new Vector3(    1f,    1f,    1f),
                                                    new Vector3( 1.25f,    0f,  0.3f),
                                                    new Vector3(   45f,    0f,  180f)
                                                 )
                              )
                     }
                 )

           //模块 图标
           .IconOffset(new Icon(
                                   1.3f,
                                   new Vector3(-0.25f, -0.25f, 0f),
                                   new Vector3(15f, -15f, 45f)
                                )
                       )

           //模块 组件
           .Components(new Type[] { typeof(RocketBlockScript), })

           //模块 设置模块属性
           .Properties(
                        new BlockProperties()
                       .SearchKeywords(new string[] { "Rocket", "火箭" })
                       .Burnable(0.1f)
                       )

           //模块 质量
           .Mass(0.5f)

            //模块 是否显示碰撞器
#if DEBUG
            .ShowCollider(true)
#endif
           //模块 碰撞器
           .CompoundCollider(new System.Collections.Generic.List<ColliderComposite>
                                {
                                    ColliderComposite.Capsule(
                                                                0.1f,                              //胶囊半径
                                                                2.65f,                             //胶囊高度
                                                                Direction.X,                       //胶囊方向
                                                                new Vector3(-0.125f,  0f,  0.3f),  //胶囊位置
                                                                Vector3.zero                       //胶囊旋转
                                                             )
                                }
                             )

           //模块 载入资源
           .NeededResources(new System.Collections.Generic.List<NeededResource>
                               {
                                    new NeededResource(ResourceType.Texture,"/RocketBlockMod/Rocket.png")
                                })

           //模块 连接点
           .AddingPoints(new System.Collections.Generic.List<AddingPoint>
                            {

                                new BasePoint(false,true) //底部连接点。第一个是指你能不能将其他模块安在该模块底部。第二个是指这个点是否是在开局时粘连其他链接点
                                                .Motionable(false,false,false) //底点在X，Y，Z轴上是否是能够活动的。
                                                .SetStickyRadius(0.25f) //粘连距离

               /* ,
               new AddingPoint(
                                   new Vector3(  0f,  0f,  0f), //位置
                                   new Vector3(-90f,  0f,  0f), //旋转
                                   false                       //这个点是否是在开局时粘连其他链接点
                               ).SetStickyRadius(0.15f)      //粘连距离
                   */
                            });

        #endregion;

        //声明火箭弹巢模块
        public Block RocketPodBlock = new Block()
        #region 火箭巢模块 基本属性  

            //模块 ID
            .ID(651)

            //模块 名称
            .BlockName("Rocket Pod Block")

            //模块 模型信息
            .Obj(new System.Collections.Generic.List<Obj>
                    { new Obj(
                                "/RocketBlockMod/RocketPod.obj",
                                "/RocketBlockMod/RocketPod.png",
                                new VisualOffset(
                                                    new Vector3(  0.5f,  0.5f,    1f),
                                                    new Vector3(    0f,    0f, 0.55f),
                                                    new Vector3(   90f,   90f,    0f)
                                                 )
                              )
                      }
                  )

            //模块 图标
            .IconOffset(new Icon(
                                    new Vector3(0.5f, 0.5f, 1f),
                                    new Vector3(-0f, -0f, 0f),
                                    new Vector3(-7.5f, 30f, 15f)
                                 )
                        )

            //模块 组件
            .Components(new Type[] { typeof(RocketPodBlockScript), })

            //模块 设置模块属性
            .Properties(
                         new BlockProperties()
                        .SearchKeywords(new string[] { "RocketPod", "火箭巢" })
                        )

            //模块 质量
            .Mass(0.5f)


#if DEBUG
            //模块 是否显示碰撞器
            .ShowCollider(true)
#endif
            //模块 碰撞器
            .CompoundCollider(new System.Collections.Generic.List<ColliderComposite>
                                 {
                                    ColliderComposite.Capsule(
                                                                0.475f,                            //胶囊半径
                                                                3f,                                //胶囊高度
                                                                Direction.X,                       //胶囊方向
                                                                new Vector3(   0f,  0f, 0.55f),    //胶囊位置
                                                                Vector3.zero                       //胶囊旋转
                                                             )

                                 }
                              )

            //模块 连接点
            .AddingPoints(new System.Collections.Generic.List<AddingPoint>
                             {

                                new BasePoint(false,true) //底部连接点。第一个是指你能不能将其他模块安在该模块底部。第二个是指这个点是否是在开局时粘连其他链接点
                                                .Motionable(false,false,false) //底点在X，Y，Z轴上是否是能够活动的。
                                                .SetStickyRadius(0.5f) //粘连距离
                                    
                                /* ,
                                new AddingPoint(
                                                    new Vector3(  0f,  0f,  0f), //位置
                                                    new Vector3(-90f,  0f,  0f), //旋转
                                                    false                       //这个点是否是在开局时粘连其他链接点
                                                ).SetStickyRadius(0.15f)      //粘连距离
                                    */
                             });

        #endregion;

    }


}
