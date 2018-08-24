using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;

namespace ModernFirearmKitMod
{
    public class RocketSmokeScript:MonoBehaviour
    {
        ParticleSystem particleSystem;

        public float Radius { get; set; } = 0.1f;
        public float Angle { get; set; } = 15f;
        public float LifeTime { get; set; } = 3f;
        public float Size { get; set; } = 1f;
        public float StartSize { get; set; } = 1f;
        public float EndSize { get; set; } = 3f;
        public Color StartColor { get; set; } = Color.gray;
        public Color EndColor { get; set; } = Color.gray;
        public float StartColorTime { get; set; } = 0;
        public float EndColorTime { get; set; } = 0.15f;
        public float StartAlpha { get; set; } = 1f;
        public float EndAlpha { get; set; } = 0f;
        public float StartAlphaTime { get; set; } = 0;
        public float EndAlphaTime { get; set; } = 2.4f;

        public bool EmitSwitch { get; set; } = false;

        void Start()
        {

            particleSystem = GetComponent<ParticleSystem>() ?? gameObject.AddComponent<ParticleSystem>();
            particleSystem.playOnAwake = false;
            particleSystem.Stop();
            particleSystem.startLifetime = LifeTime;
            particleSystem.loop = true;
            particleSystem.startColor = StartColor;
            particleSystem.simulationSpace = ParticleSystemSimulationSpace.World;
            particleSystem.maxParticles = 10240;
            particleSystem.gravityModifier = -0.02f;
            particleSystem.scalingMode = ParticleSystemScalingMode.Shape;


            ParticleSystem.ShapeModule sm = particleSystem.shape;
            sm.shapeType = ParticleSystemShapeType.ConeShell;
            sm.radius = Radius;
            sm.angle = Angle;
            sm.length = 1;

            sm.randomDirection = true;
            sm.enabled = true;

            ParticleSystem.SizeOverLifetimeModule sl = particleSystem.sizeOverLifetime;
            sl.size = new ParticleSystem.MinMaxCurve(Size, AnimationCurve.Linear(0f, StartSize, LifeTime, EndSize));
            sl.enabled = true;

            ParticleSystem.RotationOverLifetimeModule rolm = particleSystem.rotationOverLifetime;
            rolm.enabled = true;
            rolm.x = new ParticleSystem.MinMaxCurve(0, 360);

            ParticleSystem.ColorOverLifetimeModule colm = particleSystem.colorOverLifetime;
            colm.color = new Gradient()
            {

                alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(StartAlpha, StartAlphaTime), new GradientAlphaKey(EndAlpha, EndAlphaTime) },

                colorKeys = new GradientColorKey[] { new GradientColorKey(StartColor, StartColorTime), new GradientColorKey(EndColor, EndColorTime) }

            };
            colm.enabled = true;

            ParticleSystem seb = particleSystem.subEmitters.birth0;
            seb = GetComponent<ParticleSystem>();

            ParticleSystemRenderer particleSystemRenderer = particleSystem.GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.sortMode = ParticleSystemSortMode.Distance;
            particleSystemRenderer.sortingFudge = 80;
            particleSystemRenderer.receiveShadows = true;
            particleSystemRenderer.sharedMaterial = new Material(Shader.Find("Particles/Alpha Blended"));
            particleSystemRenderer.sharedMaterial.mainTexture = (ModResource.GetTexture("Rocket Smoke Texture"));
        }

        void Update()
        {
            if (EmitSwitch)
            {
                particleSystem.Emit(10);
            }
        }
    }
}
