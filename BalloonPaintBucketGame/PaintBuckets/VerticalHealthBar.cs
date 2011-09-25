using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BalloonPaintBucketGame.Util;

namespace BalloonPaintBucketGame.PaintBuckets
{
    public class VerticalHealthBar
    {
        public static Texture2D VIAL_TEXTURE { get; set; }

        public PaintBucket bucket { get; set; }
        public Color fillColor { get; set; }

        static VerticalHealthBar()
        {
            VIAL_TEXTURE = BalloonPaintBucketMainGame.GetInstance().game.Content.Load<Texture2D>("UI/meter");
        }

        public VerticalHealthBar(PaintBucket bucket, Color fillColor)
        {
            this.bucket = bucket;
            this.fillColor = fillColor;
        }

        /// <summary>
        /// Gets the percentage this bar is filled up.
        /// </summary>
        /// <returns>The percentage.</returns>
        public float GetPercentageFull()
        {
            return (bucket.currentValue / bucket.maxValue) * 100f;
        }

        /// <summary>
        /// Gets the draw rectangle of this healthbar.
        /// </summary>
        /// <returns>The rectangle that you can use to draw.</returns>
        public Rectangle GetDrawRectangle()
        {

            Rectangle drawRect = bucket.GetDrawRectangle();
            return new Rectangle(drawRect.Right + 10, drawRect.Top, 30, bucket.GetDrawRectangle().Height - 10);
        }

        public void Draw(SpriteBatch sb)
        {
            Rectangle drawRect = this.GetDrawRectangle();
            sb.Draw(VIAL_TEXTURE, drawRect, null, Color.White, 0f,
                Vector2.Zero, SpriteEffects.None, bucket.z - 0.001f);

            Point vialOffset = new Point(6, 1);

            float offset = (float)Math.Ceiling((this.GetPercentageFull() / 100f) * (drawRect.Height - (vialOffset.Y * 2)));

            drawRect = new Rectangle(drawRect.X + vialOffset.X, (int)(drawRect.Y + (drawRect.Height - offset)) - vialOffset.Y,
                drawRect.Width - (vialOffset.X * 2), (int)(offset) - vialOffset.Y);
            sb.Draw(DrawUtil.lineTexture, drawRect, null, this.fillColor, 0f,
                Vector2.Zero, SpriteEffects.None, bucket.z - 0.002f);
        }
    }
}
