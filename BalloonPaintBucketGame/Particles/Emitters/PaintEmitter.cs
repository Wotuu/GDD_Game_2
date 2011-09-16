using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleEngine.Emitter;
using BalloonPaintBucketGame.Balloons;
using ParticleEngine.Particles;

namespace BalloonPaintBucketGame.Particles.Emitters
{
    public class PaintEmitter : ParticleEmitter
    {
        public PaintEmitter(Balloon balloon)
            : base(balloon.GetCenter().X, balloon.GetCenter().Y, balloon.z - 0.001f)
        {
            this.particleRandomX = 30;

            this.particleSpeedX = -0.3f;
            this.particleRandomSpeedX = 0.66f;

            this.particleSpeedY = 5f;
            this.particleRandomSpeedY = 2f;

            this.lifespanMS = 10000;

            this.lifespanMS = 900;
            this.particlesPerTick = 60;

            this.ticksPerSecond = 1;

            this.particleColor = balloon.GetColor();
        }

        protected override void CreateParticle()
        {
            new Particle(this);
        }
    }
}
