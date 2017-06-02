using System;
using System.Collections.Generic;
using spaar.ModLoader;
using TheGuysYouDespise;
using UnityEngine;

namespace MordenFirearmKitMod
{

    // If you need documentation about any of these values or the mod loader
    // in general, take a look at https://spaar.github.io/besiege-modloader.


    public partial class MordenFirearmKitMod : Mod
    {
        public override string Name { get; } = "MordenFirearmKitMod";
        public override string DisplayName { get; } = "Morden Firearm Kit";
        public override string Author { get; } = "XultimateX";
        public override Version Version => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        // You don't need to override this, if you leavie it out it will default
        // to an empty string.
        public override string VersionExtra { get; } = "";

        // You don't need to override this, if you leave it out it will default
        // to the current version.
        public override string BesiegeVersion { get; } = "v0.45";

        // You don't need to override this, if you leave it out it will default
        // to false.
        public override bool CanBeUnloaded { get; } = false;


        /// <Block-loading-info>
        /// Place .obj file in Mods/Blocks/Obj
        /// Place texture in Mods/Blocks/Textures
        /// Place any additional resources in Mods/Blocks/Resources
        /// </Block-loading-info>


        // TEMPLATE: You will need to do even more adjusting here than in normal mods, make sure you check every property
        // and set it to something sensible for your block. For some things it's fine to leave them out if you don't need them.
        // Place your .obj file in the Resources/obj/ directory, your texture in Resources/tex/ and any other resources (that you use
        // with the "Needed Resources" feature) in Resources/other/. They will then be copied automatically to your Besiege
        // installation on build.
        // Additionally, when you build using the "Release" configuration (drop-down in the toolbar at the top, "Debug" by default)
        // a directory called "Release" will be created in your project directory. You just need to put into a .zip file or similiar
        // archive and you're ready to publish your mod. (Put the _contents_ of the folder into a .zip file, not the Release folder itself)

      



        private GameObject updater = new GameObject();

        public override void OnLoad()
        {
            // Your initialization code here
            updater.AddComponent<Updater>().Url("XultimateX", "MordenFirearmKitMod");
        }


        public override void OnUnload()
        {
            // Your code here
            // e.g. save configuration, destroy your objects if CanBeUnloaded is true etc

        }

    }

}
