using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleEngine.Particles;
using ParticleEngine.Emitter;
using BalloonPaintBucketGame.Managers;

namespace BalloonPaintBucketGame.Particles.Particles
{
    public class PaintParticle : Particle
    {
        public float gravity = 0.4f;

        public PaintParticle(ParticleEmitter emitter)
            : base(emitter)
        {
        }

        public override bool Update(float time_step)
        {
            this.speedY += (float)(gravity * GameTimeManager.GetInstance().time_step);
            return base.Update(time_step);
        }
    }
}
