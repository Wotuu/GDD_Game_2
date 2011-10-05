using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiniGameOverview.Animations
{
    public class Animator
    {
        public Texture2D sourceTexture { get; set; }
        public Rectangle sourceRectangle { get; set; }
        public int msBetweenFrames { get; set; }
        public int yOffset { get; set; }

        private double lastFrameChangeMS { get; set; }
        private int _currentFrame { get; set; }
        private int currentFrame
        {
            get
            {
                return _currentFrame;
            }
            set
            {
                if (value >= maxFrames) value %= maxFrames;

                this._currentFrame = value;
            }
        }
        private int maxFrames { get; set; }

        public Animator(Texture2D sourceTexture, Rectangle sourceRectangle, int msBetweenFrames)
        {
            if (sourceTexture.Width % sourceRectangle.Width != 0)
            {
                throw new ArgumentException("Cannot get an integer amount of textures out of sourceTexture according to sourceRectangle.");
            }

            this.maxFrames = sourceTexture.Width / sourceRectangle.Width;

            this.sourceTexture = sourceTexture;
            this.sourceRectangle = sourceRectangle;

            this.msBetweenFrames = msBetweenFrames;

            // Start at -1, because the next tick will increase the frame counter, and set it to 0
            this.currentFrame = -1;
        }

        /// <summary>
        /// Gets the destinatino rectangle based on the current time, etc.
        /// </summary>
        /// <param name="updateStartMS">The time at which this frame was started</param>
        /// <returns>The rectangle you should use to grab the source.</returns>
        public Rectangle GetSourceRectangle(double updateStartMS)
        {
            if (updateStartMS - this.lastFrameChangeMS > msBetweenFrames)
            {
                this.lastFrameChangeMS = updateStartMS;

                this.currentFrame++;
            }

            return new Rectangle(this.currentFrame * this.sourceRectangle.Width,
                this.yOffset, this.sourceRectangle.Width, this.sourceRectangle.Height);
        }
    }
}
