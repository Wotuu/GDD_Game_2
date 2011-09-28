using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MainGame.Managers;

namespace MainGame.Backgrounds
{
    public class MainGameBackgroundItem
    {
        public Texture2D texture { get; set; }

        public Vector3 location { get; set; }
        private Vector2 originalScale { get; set; }
        public Vector2 scale { get; set; }

        public Boolean doScaleFlash { get; set; }
        private Boolean scalingBigger { get; set; }

        public float scaleSpeed = 0.0001f;
        public float maxScaleDeviation = 0.02f;

        public enum BackgroundType
        {
            Cloud,
            Bush
        }

        public MainGameBackgroundItem(Vector3 location, BackgroundType type, int textureIndex)
        {
            switch (type)
            {
                case BackgroundType.Cloud:
                    this.doScaleFlash = true;
                    this.texture = Game1.GetInstance().Content.Load<Texture2D>("Menu/wolk" + textureIndex);
                    this.originalScale = new Vector2(0.40f, 0.40f);
                    break;

                case BackgroundType.Bush:
                    this.texture = Game1.GetInstance().Content.Load<Texture2D>("Menu/bos" + textureIndex);

                    this.originalScale = new Vector2(0.60f, 0.60f);
                    break;
            }

            this.location = location;
            this.scale = this.originalScale;
        }

        public void Update()
        {
            if (this.doScaleFlash)
            {
                if (scalingBigger)
                {
                    this.scale = new Vector2(this.scale.X + (this.scaleSpeed * (float)GameTimeManager.GetInstance().time_step),
                        this.scale.Y + (this.scaleSpeed * (float)GameTimeManager.GetInstance().time_step));

                    if (this.scale.X > this.originalScale.X + this.maxScaleDeviation)
                    {
                        this.scalingBigger = !this.scalingBigger;
                        this.scale = new Vector2(this.originalScale.X + this.maxScaleDeviation,
                            this.originalScale.Y + this.maxScaleDeviation);
                    }
                }
                else
                {
                    this.scale = new Vector2(this.scale.X - (this.scaleSpeed * (float)GameTimeManager.GetInstance().time_step),
                        this.scale.Y - (this.scaleSpeed * (float)GameTimeManager.GetInstance().time_step));

                    if (this.scale.X < this.originalScale.X - this.maxScaleDeviation)
                    {
                        this.scalingBigger = !this.scalingBigger;
                        this.scale = new Vector2(this.originalScale.X - this.maxScaleDeviation,
                            this.originalScale.Y - this.maxScaleDeviation);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the draw rectangle of this background item.
        /// </summary>
        /// <returns>The rectangle that you should use to draw this thing with.</returns>
        public Rectangle GetDrawRectangle()
        {
            Vector2 dimension = new Vector2((int)(this.texture.Width * this.scale.X),
                (int)(this.texture.Height * this.scale.Y));
            return new Rectangle((int)(this.location.X - (dimension.X / 2)),
                (int)(this.location.Y - (dimension.Y / 2)),
                (int)dimension.X,
                (int)dimension.Y);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this.texture,
                this.GetDrawRectangle(), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, this.location.Z);
        }
    }
}
