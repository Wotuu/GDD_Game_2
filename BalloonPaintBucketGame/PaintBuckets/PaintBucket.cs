using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BalloonPaintBucketGame.Particles;
using BalloonPaintBucketGame.Particles.Particles;
using BalloonPaintBucketGame.Particles.Emitters;

namespace BalloonPaintBucketGame.PaintBuckets
{
    public class PaintBucket : RectangleCollideable
    {
        public static Texture2D PINK_PAINTBUCKET;
        public static Texture2D YELLOW_PAINTBUCKET;
        public static Texture2D BLUE_PAINTBUCKET;

        public PaintBucketColor color { get; set; }
        public Vector2 location { get; set; }
        public Texture2D texture { get; set; }
        public VerticalHealthBar progressBar { get; set; }

        public Vector2 scale { get; set; }
        public float z { get; set; }

        private float _currentValue { get; set; }
        public float currentValue
        {
            get
            {
                return _currentValue;
            }
            set
            {
                this._currentValue = MathHelper.Clamp(value, 0, maxValue);
            }
        }
        public float maxValue { get; set; }


        static PaintBucket()
        {
            PINK_PAINTBUCKET = BalloonPaintBucketMainGame.GetInstance().game.Content.Load<Texture2D>("PaintBuckets/pink");
            BLUE_PAINTBUCKET = BalloonPaintBucketMainGame.GetInstance().game.Content.Load<Texture2D>("PaintBuckets/blue");
            YELLOW_PAINTBUCKET = BalloonPaintBucketMainGame.GetInstance().game.Content.Load<Texture2D>("PaintBuckets/yellow");
        }

        public enum PaintBucketColor
        {
            Pink,
            Yellow,
            Blue
        }

        public PaintBucket(PaintBucketColor color)
        {
            Color healthBarFillColor = Color.White;
            switch (color)
            {
                case PaintBucketColor.Pink:
                    this.texture = PINK_PAINTBUCKET;
                    healthBarFillColor = new Color(252, 161, 255);
                    break;
                case PaintBucketColor.Blue:
                    this.texture = BLUE_PAINTBUCKET;
                    healthBarFillColor = new Color(132, 184, 239);
                    break;
                case PaintBucketColor.Yellow:
                    this.texture = YELLOW_PAINTBUCKET;
                    healthBarFillColor = new Color(242, 221, 95);
                    break;
            }

            this.scale = new Vector2(0.35f, 0.35f);
            this.color = color;

            this.maxValue = 100;

            this.progressBar = new VerticalHealthBar(this, healthBarFillColor);

            this.z = 0.95f;
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this.texture, this.GetDrawRectangle(), null, Color.White,
                0f, Vector2.Zero, SpriteEffects.None, z);

            progressBar.Draw(sb);
        }

        /// <summary>
        /// Gets the draw rectangle of this balloon.
        /// </summary>
        /// <returns>The rectangle that can be used for drawing.</returns>
        public Rectangle GetDrawRectangle()
        {
            return new Rectangle((int)this.location.X, (int)this.location.Y,
                (int)(this.texture.Width * this.scale.X), (int)(this.texture.Height * this.scale.Y));
        }

        /// <summary>
        /// Gets the color of this paint bucket.
        /// </summary>
        /// <returns>The color.</returns>
        public Color GetColor()
        {
            switch (this.color)
            {
                case PaintBucketColor.Pink:
                    return Color.DeepPink;
                case PaintBucketColor.Blue:
                    return Color.Blue;
                case PaintBucketColor.Yellow:
                    return Color.Yellow;
            }
            return Color.Red;
        }

        /// <summary>
        /// Gets the center of this balloon.
        /// </summary>
        /// <returns>The dead center.</returns>
        public Vector2 GetCenter()
        {
            return new Vector2(this.location.X + ((this.texture.Width * this.scale.X) / 2),
                this.location.Y + ((this.texture.Height * this.scale.Y) / 2));
        }

        /// <summary>
        /// Whether the paintbucket is filled or not.
        /// </summary>
        /// <returns>Yes or no.</returns>
        public Boolean IsFilled()
        {
            return this.currentValue >= this.maxValue;
        }

        public Rectangle GetCollisionRectangle()
        {
            return this.GetDrawRectangle();
        }

        public bool CheckCollision(RectangleCollideable collideable)
        {
            return collideable.GetCollisionRectangle().Intersects(this.GetCollisionRectangle());
        }

        public void OnCollision(RectangleCollideable collidedWith)
        {
            if (collidedWith is PaintParticle)
            {
                PaintParticle particle = (PaintParticle)collidedWith;
                Color balloonColor = ((PaintEmitter)particle.emitter).balloon.GetColor();
                if (balloonColor == this.GetColor())
                {
                    this.currentValue += 100;
                }
                else if( balloonColor != Color.DarkGray ) this.currentValue--; 
                else this.currentValue = 0;
            }
        }
    }
}
