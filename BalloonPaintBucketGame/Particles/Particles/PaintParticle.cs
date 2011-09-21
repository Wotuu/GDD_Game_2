using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleEngine.Particles;
using ParticleEngine.Emitter;
using BalloonPaintBucketGame.Managers;
using Microsoft.Xna.Framework;
using BalloonPaintBucketGame.Particles.Emitters;

namespace BalloonPaintBucketGame.Particles.Particles
{
    public class PaintParticle : Particle, RectangleCollideable
    {
        public float gravity = 0.15f;

        public PaintParticle(PaintEmitter emitter)
            : base(emitter)
        {

        }

        public override bool Update(float time_step)
        {
            this.speedY += (float)(gravity * GameTimeManager.GetInstance().time_step);
            return base.Update(time_step);
        }

        public Rectangle GetCollisionRectangle()
        {
            return this.lastDrawRectangle;
        }

        public bool CheckCollision(RectangleCollideable collideable)
        {
            return collideable.GetCollisionRectangle().Intersects(this.GetCollisionRectangle());
        }

        public void OnCollision(RectangleCollideable collidedWith)
        {
            this.alive = false;
        }
    }
}
