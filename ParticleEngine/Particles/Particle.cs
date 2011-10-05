using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ParticleEngine.Emitter;
using ParticleEngine.Gametime;

namespace ParticleEngine.Particles
{
    public class Particle
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }

        public float speedX { get; set; }
        public float speedY { get; set; }

        public float scale { get; set; }

        public float lifespanMS { get; set; }
        public double creationTime { get; set; }

        public float radianRotation { get; set; }
        public float radianRotationSpeed { get; set; }

        public float gravity { get; set; }
        public float terminalVelocity { get; set; }

        public Boolean alive { get; set; }

        public Texture2D texture { get; set; }

        public Color color { get; set; }

        public ParticleEmitter emitter { get; set; }

        private Vector2 drawOffset { get; set; }

        public Boolean fadeAccordingToLifespan { get; set; }
        public Boolean inverseFade { get; set; }

        public Boolean scaleAccordingToLifespan { get; set; }
        public Boolean inverseScale { get; set; }

        public static Random RANDOM { get; set; }
        public Rectangle lastDrawRectangle { get; set; }

        static Particle()
        {
            RANDOM = new Random();
        }

        public Particle(ParticleEmitter emitter)
        {
            this.alive = true;
            this.creationTime = GameTimeManager.GetInstance().currentUpdateStartMS;

            if (emitter.particleTexture == null)
                this.texture = ParticleManager.DEFAULT_TEXTURE;
            else this.texture = emitter.particleTexture;

            this.x = emitter.x + ((RANDOM.Next(0, 10000) / 10000f) * emitter.particleRandomX);
            this.y = emitter.y + ((RANDOM.Next(0, 10000) / 10000f) * emitter.particleRandomY);
            this.z = emitter.z;

            this.scale = emitter.particleScale + ((RANDOM.Next(0, 10000) / 10000f) * emitter.particleScaleRandom);

            this.speedX = emitter.particleSpeedX + ((RANDOM.Next(0, 10000) / 10000f) * emitter.particleRandomSpeedX);
            this.speedY = emitter.particleSpeedY + ((RANDOM.Next(0, 10000) / 10000f) * emitter.particleRandomSpeedY);

            this.lifespanMS = emitter.particleLifespanMS + ((RANDOM.Next(0, 10000) / 10000f) * emitter.particleLifespanRandomMS);

            this.radianRotation = emitter.particleRadianRotation + ((RANDOM.Next(0, 10000) / 10000f) * emitter.particleRadianRotationRandom);
            this.radianRotationSpeed = emitter.particleRadianRotationSpeed + ((RANDOM.Next(0, 10000) / 10000f) * emitter.particleRadianRotationSpeedRandom);

            this.gravity = emitter.particleGravity;
            this.terminalVelocity = emitter.particleTerminalVelocity;

            this.color = emitter.particleColor;

            this.fadeAccordingToLifespan = emitter.fadeAccordingToLifespan;
            this.inverseFade = emitter.inverseFade;

            this.scaleAccordingToLifespan = emitter.scaleAccordingToLifespan;
            this.inverseScale = emitter.inverseScale;

            this.emitter = emitter;

            this.emitter.particles.AddLast(this);
        }


        /// <summary>
        /// Determines if the particle should die.
        /// </summary>
        /// <returns>Yes or no. Chose. Wisely.</returns>
        public Boolean ShouldDie()
        {
            return GameTimeManager.GetInstance().currentUpdateStartMS - this.creationTime > lifespanMS;
        }

        /// <summary>
        /// Updates the particle.
        /// </summary>
        /// <param name="time_step">FPS timestep</param>
        /// <returns>True if the update was successful, false if the particle should be removed!</returns>
        public virtual Boolean Update(float time_step)
        {
            if (!alive) return alive;
            // Check if we should die
            if (this.ShouldDie())
            {
                // Yes
                this.alive = false;
            }
            else
            {
                // No
                this.x += ((speedX /*+= (float)((this.gravity / 2) * GameTimeManager.GetInstance().time_step)*/) * time_step);
                this.y += ((speedY += (float)((this.gravity / 10) * GameTimeManager.GetInstance().time_step)) * time_step);

                this.radianRotation += (radianRotationSpeed * time_step);
            }

            if (this.speedY > this.terminalVelocity)
                this.speedY = this.terminalVelocity;
            // Return
            return alive;
        }

        /// <summary>
        /// Gets the percentage of the lifespan the particle
        /// </summary>
        /// <returns></returns>
        public int GetPercentageOfLifespan()
        {
            return Math.Min(100, (int)(((GameTimeManager.GetInstance().currentUpdateStartMS - this.creationTime) /
                this.lifespanMS) * 100f));
        }

        public void Draw(SpriteBatch sb)
        {
            if (!alive) return;
            Color drawColor = this.color;
            if (fadeAccordingToLifespan)
            {
                int percentageOfLifespan = this.GetPercentageOfLifespan();
                if (inverseFade)
                    drawColor = new Color(this.color.R, this.color.G, this.color.B, (int)(percentageOfLifespan * 2.55f));
                else
                    drawColor = new Color(this.color.R, this.color.G, this.color.B, 255 - (int)(percentageOfLifespan * 2.55f));
            }

            sb.Draw(this.texture, (this.lastDrawRectangle = this.CalculateDrawRectangle()), null, drawColor,
                this.radianRotation, //Vector2.Zero
                new Vector2((this.lastDrawRectangle.Width / 2f), (this.lastDrawRectangle.Height / 2f)),//*/,
                SpriteEffects.None, this.z);
        }

        /// <summary>
        /// Calculates the current draw rectangle, and updates the current offset (to make sure the particle
        /// stays in the middle when the scale has changed).
        /// </summary>
        /// <returns>The rectangle.</returns>
        public Rectangle CalculateDrawRectangle()
        {
            float newScale = this.scale;
            if (scaleAccordingToLifespan)
            {
                int percentageOfLifespan = this.GetPercentageOfLifespan();
                if (inverseScale)
                    newScale = this.scale * (1f - (percentageOfLifespan / 100f));
                else
                    newScale = this.scale * (percentageOfLifespan / 100f);
            }

            this.drawOffset = new Vector2(((this.texture.Width * newScale) / 2f),
                ((this.texture.Height * newScale) / 2f));

            return new Rectangle((int)(this.x - this.drawOffset.X), (int)(this.y - this.drawOffset.Y),
                (int)(texture.Width * newScale), (int)(texture.Height * newScale));
        }

        /// <summary>
        /// Gets the location of this particle on the screen.
        /// </summary>
        /// <returns>The point.</returns>
        public Point GetLocation()
        {
            return new Point((int)x, (int)y);
        }
    }
}
