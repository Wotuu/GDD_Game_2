using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleEngine.Emitter;
using Microsoft.Xna.Framework;
using ParticleEngine.Particles;
using Microsoft.Xna.Framework.Graphics;

namespace MiniGameOverview.Particles
{
    public class UnlockedEmitter : ParticleEmitter
    {


        public UnlockedEmitter(Vector3 location)
            : base(location.X, location.Y, location.Z)
        {
            this.ticksPerSecond = 34;

            this.fadeAccordingToLifespan = true;

            this.particleGravity = -0.6f;

            this.particleLifespanMS = 1000;

            this.particleRandomX = 20;
            this.particleRandomY = 20;

            this.particleSpeedX = 1f;
            this.particleSpeedY = 1f;

            this.particleRandomSpeedX = -2f;
            this.particleRandomSpeedY = -2f;

            this.particleColor = Color.Orange;

            this.blendState = BlendState.AlphaBlend;
        }

        protected override void CreateParticle()
        {
            new Particle(this);
        }
    }
}
