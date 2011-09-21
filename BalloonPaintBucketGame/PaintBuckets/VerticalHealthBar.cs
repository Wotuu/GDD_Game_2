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
        public static Color EMPTY_COLOR = Color.Red;
        public static Color FILL_COLOR = Color.Blue;
        public static Texture2D DRAW_TEXTURE { get; set; }

        public int width { get; set; }
        public int height { get; set; }

        public PaintBucket bucket { get; set; }

        public VerticalHealthBar(PaintBucket bucket)
        {
            this.bucket = bucket;

            width = 30;
            height = bucket.GetDrawRectangle().Height - 10;
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
            return new Rectangle(drawRect.Right + 10, drawRect.Top, this.width, this.height);
        }

        public void Draw(SpriteBatch sb)
        {
            if (DRAW_TEXTURE == null)
                DRAW_TEXTURE = DrawUtil.GetClearTexture2D(sb);

            Rectangle drawRect = this.GetDrawRectangle();
            sb.Draw(DRAW_TEXTURE, drawRect, null, EMPTY_COLOR, 0f,
                Vector2.Zero, SpriteEffects.None, bucket.z - 0.001f);

            float offset = (float)Math.Ceiling((this.GetPercentageFull() / 100f) * drawRect.Height);

            drawRect = new Rectangle(drawRect.X, (int)(drawRect.Y + (drawRect.Height - offset)),
                drawRect.Width, (int)(offset));
            sb.Draw(DRAW_TEXTURE, drawRect, null, FILL_COLOR, 0f,
                Vector2.Zero, SpriteEffects.None, bucket.z - 0.002f);

        }
    }
}
