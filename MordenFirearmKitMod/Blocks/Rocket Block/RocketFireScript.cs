using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;

namespace ModernFirearmKitMod
{
    public class RocketFireScript : MonoBehaviour
    {

        public ParticleSystem particleSystem;

        public float LifeTime { get; set; } /*= 0f;*/

        public float Radius { get; set; }/* = 1f;*/

        public float Angle { get; set; }/* = 2f;*/

        public float Size { get; set; } /*= 0.5f;*/

        public float StartSize { get; set; } /*= 1f;*/

        public float EndSize { get; set; } /*= 0f;*/

        public Color StartColor { get; set; } /*= Color.blue;*/

        public Color EndColor { get; set; } /*= Color.yellow;*/

        public float ColorStartTime { get; set; } /*= 0f;*/

        public float ColorEndTime { get; set; }/* = 0f;*/

        public bool EmitSwitch { get; set; } = false;

        void Start()
        {

            particleSystem = GetComponent<ParticleSystem>() ?? gameObject.AddComponent<ParticleSystem>();

            particleSystem.playOnAwake = false;
            particleSystem.Stop();
            particleSystem.startLifetime = LifeTime;

            particleSystem.scalingMode = ParticleSystemScalingMode.Hierarchy;

            ParticleSystem.ShapeModule sm = particleSystem.shape;
            sm.shapeType = ParticleSystemShapeType.Cone;
            sm.radius = Radius;
            sm.angle = Angle;
            sm.randomDirection = false;
            sm.enabled = true;


            ParticleSystem.SizeOverLifetimeModule sl = particleSystem.sizeOverLifetime;
            sl.size = new ParticleSystem.MinMaxCurve(Size, AnimationCurve.Linear(0f, StartSize, LifeTime, EndSize));
            sl.enabled = true;

            ParticleSystem.ColorOverLifetimeModule colm = particleSystem.colorOverLifetime;
            colm.color = new Gradient()
            {

                alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(0, LifeTime) },

                colorKeys = new GradientColorKey[] { new GradientColorKey(StartColor, ColorStartTime), new GradientColorKey(EndColor, ColorEndTime) }

            };
            colm.enabled = true;

            ParticleSystemRenderer particleSystemRenderer = GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.sharedMaterial = new Material(Shader.Find("Particles/Additive"));
            particleSystemRenderer.sharedMaterial.mainTexture = (ModResource.GetTexture("Rocket Fire Texture"));
        }

        void Update()
        {
            if (EmitSwitch)
            {
                particleSystem.Emit(2);
            }      
        }
    }
}
