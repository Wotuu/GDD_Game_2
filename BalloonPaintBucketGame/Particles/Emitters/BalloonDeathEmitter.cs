using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleEngine.Emitter;
using ParticleEngine.Particles;
using BalloonPaintBucketGame.Balloons;
using Microsoft.Xna.Framework.Graphics;

namespace BalloonPaintBucketGame.Particles.Emitters
{
    public class BalloonDeathEmitter : ParticleEmitter
    {
        public Balloon balloon { get; set; }

        public static Texture2D BALLOON_DEATH_PARTICLE_BLUE { get; set; }
        public static Texture2D BALLOON_DEATH_PARTICLE_YELLOW { get; set; }
        public static Texture2D BALLOON_DEATH_PARTICLE_PINK { get; set; }
        public static Texture2D BALLOON_DEATH_PARTICLE_BLACK { get; set; }


        static BalloonDeathEmitter()
        {
            BALLOON_DEATH_PARTICLE_BLUE =
                BalloonPaintBucketMainGame.GetInstance().game.Content.Load<Texture2D>("Particles/Balloons/blauw");
            BALLOON_DEATH_PARTICLE_YELLOW =
                BalloonPaintBucketMainGame.GetInstance().game.Content.Load<Texture2D>("Particles/Balloons/geel");
            BALLOON_DEATH_PARTICLE_PINK =
                BalloonPaintBucketMainGame.GetInstance().game.Content.Load<Texture2D>("Particles/Balloons/roze");
            BALLOON_DEATH_PARTICLE_BLACK =
                BalloonPaintBucketMainGame.GetInstance().game.Content.Load<Texture2D>("Particles/Balloons/zwart");
        }

        public BalloonDeathEmitter(Balloon balloon)
            : base(balloon.GetCenter().X + 25, balloon.GetCenter().Y + 25, balloon.z - 0.1f)
        {
            this.particleScale = 2f;
            this.particlesPerTick = 25;
            this.lifespanMS = 900;
            this.ticksPerSecond = 1;

            this.particleSpeedX = -2f;
            this.particleSpeedY = -1.33f;

            this.particleRandomSpeedX = 4f;
            this.particleRandomSpeedY = 6f;

            this.particleLifespanMS = 500;

            this.fadeAccordingToLifespan = true;

            this.particleGravity = 0.75f;
            this.particleScale = 0.15f;

            switch (balloon.color)
            {
                case Balloon.BalloonColor.Black:
                    this.particleTexture = BALLOON_DEATH_PARTICLE_BLACK;
                    break;
                case Balloon.BalloonColor.Blue:
                    this.particleTexture = BALLOON_DEATH_PARTICLE_BLUE;
                    break;
                case Balloon.BalloonColor.Yellow:
                    this.particleTexture = BALLOON_DEATH_PARTICLE_YELLOW;
                    break;
                case Balloon.BalloonColor.Pink:
                    this.particleTexture = BALLOON_DEATH_PARTICLE_PINK;
                    break;
            }

            this.balloon = balloon;

            this.blendState = BlendState.AlphaBlend;
        }

        protected override void CreateParticle()
        {
            new Particle(this);
        }
    }
}
