using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleEngine.Emitter;
using ParticleEngine.Particles;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MainGame.Cards
{
    public class CardEmitter : ParticleEmitter
    {
        public static Texture2D STAR_TEXTURE { get; set; }
        public GameResultCard card { get; set; }

        static CardEmitter()
        {
            STAR_TEXTURE = Game1.GetInstance().Content.Load<Texture2D>("Particles/ster");
        }

        public CardEmitter(GameResultCard card)
            : base(card.GetDrawRectangle().X, card.GetDrawRectangle().Y, card.z)
        {
            this.particleRandomX = card.GetDrawRectangle().Width;
            this.particleRandomY = card.GetDrawRectangle().Height;

            this.particleLifespanMS = 4000;
            this.particleSpeedX = -1;
            this.particleRandomSpeedX = 2;

            this.particleSpeedY = -3;
            this.particleRandomSpeedY = 3;

            this.particleGravity = 0.5f;
            this.particleTerminalVelocity = 10;
            this.ticksPerSecond = 40;

            this.particleRadianRotationSpeed = MathHelper.ToRadians(-3);
            this.particleRadianRotationSpeedRandom = MathHelper.ToRadians(6);

            this.particleColor = Color.Yellow;

            this.particleTexture = CardEmitter.STAR_TEXTURE;

            this.particleScale = 0.25f;

            this.fadeAccordingToLifespan = true;

            this.blendState = BlendState.AlphaBlend;

            // Draw behind the card
            this.z = card.z + 0.1f;

        }

        public override void Update(float time_step)
        {
            base.Update(time_step);
        }

        protected override void CreateParticle()
        {
            new Particle(this);
        }
    }
}
