using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleEngine.Particles;
using ParticleEngine.Emitter;
using SquatBugsGame.Bugs;

namespace SquatBugsGame.Particles.Emitters
{
    class BugDeathEmitter : ParticleEmitter
    {
        public Bug bug { get; set; }

        public BugDeathEmitter(Bug bug)
            : base(bug.location.X, bug.location.Y, 0.1f)
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

            this.particleColor = bug.GetColor();
            this.fadeAccordingToLifespan = true;
        }

        protected override void CreateParticle()
        {
           new Particle(this);
        }
    }
}
