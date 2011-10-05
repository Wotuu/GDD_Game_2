using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleEngine.Emitter;
using DiggingGame.SandBoard;
using Microsoft.Xna.Framework;
using ParticleEngine.Particles;

namespace DiggingGame.Particles.Emitter
{
    class DigEmitter : ParticleEmitter
    {
       

        public DigEmitter(SandTile tile)
            : base(tile.DrawRectangle.X + tile.DrawRectangle.Width / 2, tile.DrawRectangle.Y + tile.DrawRectangle.Height / 2, 0.0001f)
        {
            this.particleScale = 2f;
            this.particlesPerTick = 50;
            this.lifespanMS = 900;
            this.ticksPerSecond = 1;

            this.particleSpeedX = -2f;
            this.particleSpeedY = -1.77f;

            this.particleRandomSpeedX = 6f;
            this.particleRandomSpeedY = 6f;

            this.particleLifespanMS = 500;

            this.particleColor = Color.SandyBrown;
            this.fadeAccordingToLifespan = true;
        }

        protected override void CreateParticle()
        {
            new Particle(this);
        }
    }
}
