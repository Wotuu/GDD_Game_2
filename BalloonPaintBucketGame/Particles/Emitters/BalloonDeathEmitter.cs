using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleEngine.Emitter;
using ParticleEngine.Particles;
using BalloonPaintBucketGame.Balloons;

namespace BalloonPaintBucketGame.Particles.Emitters
{
    public class BalloonDeathEmitter : ParticleEmitter
    {
        public Balloon balloon { get; set; }

        public BalloonDeathEmitter(Balloon balloon)
            : base(balloon.GetCenter().X, balloon.GetCenter().Y, balloon.z - 0.001f)
        {
            this.particleScale = 2f;
            this.particlesPerTick = 50;
            this.lifespanMS = 900;
            this.ticksPerSecond = 1;

            this.particleSpeedX = -2f;
            this.particleSpeedY = -1.77f;

            this.particleRandomSpeedX = 4f;
            this.particleRandomSpeedY = 6f;

            this.particleLifespanMS = 500;

            this.particleColor = balloon.GetColor();
            this.fadeAccordingToLifespan = true;
        }

        protected override void CreateParticle()
        {
            new Particle(this);
        }
    }
}
