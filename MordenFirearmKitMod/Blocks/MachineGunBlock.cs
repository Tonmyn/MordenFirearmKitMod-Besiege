using System;
using System.Collections.Generic;
using spaar.ModLoader;
using TheGuysYouDespise;
using UnityEngine;

namespace MordenFirearmKitMod
{
    partial class MordenFirearmKitMod
    {

        public Block Butt = new Block()
            ///ID of the Block
            .ID(606)

            ///Name of the Block
            .BlockName("Butt")

            ///Load the 3d model information
            .Obj(new List<Obj> { new Obj("/MordenFirearmKitMod/Butt.obj", //Mesh name with extension (only works for .obj files)
                                         "/MordenFirearmKitMod/Butt.png", //Texture name with extension
                                         new VisualOffset(new Vector3(0.45f, 0.5f, 0.45f), //Scale
                                                          new Vector3(-0.1f, 0.5f, 1f), //Position
                                                          new Vector3(0f, 0f, 180f)))//Rotation
            })

            ///For the button that we will create setup the visual offset needed
            .IconOffset(new Icon(new Vector3(0.75f, 0.75f, 0.75f),  //Scale
                              new Vector3(-0.11f, -0.13f, 0f),  //Position
                              new Vector3( 0f,  0f,  0f))) //Rotation

            ///Script, Components, etc. you want to be on your block.
            .Components(new Type[] {
                                    typeof(BulletBase),
              })

            ///Properties such as keywords for searching and setting up how how this block behaves to other elements.
            .Properties(new BlockProperties().SearchKeywords(null)
     //.Burnable(3f)
     //.CanBeDamaged(3)
     )

            ///Mass of the block 0.5 being equal to a double wooden block
            .Mass(0.3f)

            ///Display the collider while working on the block if you wish, then replace "true" with "false" when done looking at the colliders.
#if DEBUG
            .ShowCollider(true)
#endif
            ///Setup the collier of the block, which can consist of several different colliders.
            ///Therefore we have this CompoundCollider,
            .CompoundCollider(new List<ColliderComposite> {                            
                        
                                ColliderComposite.Box(new Vector3(0.65f, 0.65f, 0.25f),        //scale
                                                      new Vector3(0f, 0f, 0.25f),              //position
                                                      new Vector3(0f, 0f, 0f)),                //rotation
                              //ColliderComposite.Box(new Vector3(0.35f, 0.35f, 0.15f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)).Trigger().Layer(2).IgnoreForGhost(),   <---Example: Box Trigger on specific Layer
            })

            ///Make sure a block being placed on another block can intersect with it.
            .IgnoreIntersectionForBase()

            ///Load resources that will be needed for the block.
            .NeededResources(new List<NeededResource> {
                                new NeededResource(ResourceType.Audio, //Type of resource - types available are Audio, Texture, Mesh, and AssetBundle
                                                   "<placeholder>")
            })

           ///Setup where on your block you can add other blocks.
           //.AddingPoints(new List<AddingPoint> {
           //                   (AddingPoint) new BasePoint(false, true)         //The base point is unique compared to the other adding points, the two booleans represent whether you can add to the base, and whether it sticks automatically.
           //                                    .Motionable(false,false,false) //Set each of these booleans to "true" to Let the block rotate around X, Y, Z accordingly
           //                                    .SetStickyRadius(0.5f),        //Set the radius of which the base point will connect to others
           //                  //new AddingPoint(new Vector3(0f, 0f, 0.5f), new Vector3(-90f, 0f, 0f),true).SetStickyRadius(0.3f), <---Example: Top sticky adding point
           //})
           ;

    }
}
