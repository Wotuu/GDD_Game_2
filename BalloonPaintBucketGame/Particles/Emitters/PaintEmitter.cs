using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleEngine.Emitter;
using BalloonPaintBucketGame.Balloons;
using ParticleEngine.Particles;
using BalloonPaintBucketGame.Particles.Particles;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BalloonPaintBucketGame.Particles.Emitters
{
    public class PaintEmitter : ParticleEmitter
    {
        public static Texture2D BLUE_PAINT_TEXTURE { get; set; }
        public static Texture2D PINK_PAINT_TEXTURE { get; set; }
        public static Texture2D YELLOW_PAINT_TEXTURE { get; set; }
        public static Texture2D BLACK_PAINT_TEXTURE { get; set; }

        public Balloon balloon { get; set; }

        static PaintEmitter()
        {
            BLUE_PAINT_TEXTURE = BalloonPaintBucketMainGame.GetInstance().game.Content.Load<Texture2D>("Particles/Paint/druppel");
            PINK_PAINT_TEXTURE = BalloonPaintBucketMainGame.GetInstance().game.Content.Load<Texture2D>("Particles/Paint/hartje");
            YELLOW_PAINT_TEXTURE = BalloonPaintBucketMainGame.GetInstance().game.Content.Load<Texture2D>("Particles/Paint/smiley");
            BLACK_PAINT_TEXTURE = BalloonPaintBucketMainGame.GetInstance().game.Content.Load<Texture2D>("Particles/Paint/druppel-zwart");
        }

        public PaintEmitter(Balloon balloon)
            // -25 to compensate for the random X and Y
            : base(balloon.GetCenter().X - 25, balloon.GetCenter().Y - 25, balloon.z - 0.1f)
        {
            this.particleRandomX = 30;

            this.particleSpeedX = -0.6f;
            this.particleRandomSpeedX = 1.33f;

            this.particleSpeedY = 2f;
            this.particleRandomSpeedY = 2f;

            this.particleGravity = 0.75f;

            this.particleRandomX = 50;
            this.particleRandomY = 50;

            this.lifespanMS = 10000;
            this.particlesPerTick = 20;

            this.ticksPerSecond = 0.1f;

            this.balloon = balloon;

            this.blendState = BlendState.AlphaBlend;

            // this.particleColor = balloon.GetColor();
        }

        protected override void CreateParticle()
        {
            Particle part = new PaintParticle(this);
            Color color = this.balloon.GetColor();
            if (color == Color.DeepPink)
                part.texture = PINK_PAINT_TEXTURE;
            else if (color == Color.Blue)
                part.texture = BLUE_PAINT_TEXTURE;
            else if (color == Color.Yellow)
                part.texture = YELLOW_PAINT_TEXTURE;
            else if (color == Color.DarkGray)
                part.texture = BLACK_PAINT_TEXTURE;
            part.scale = 0.1f;
        }
    }
}
