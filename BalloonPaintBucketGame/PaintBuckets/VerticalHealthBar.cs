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
        public static Color FILL_COLOR = Color.Green;
        public static Texture2D DRAW_TEXTURE { get; set; }

        public int width { get; set; }
        public int height { get; set; }

        public PaintBucket bucket { get; set; }

        public VerticalHealthBar(PaintBucket bucket)
        {
            this.bucket = bucket;

            width = 40;
            height = 200;
        }

        /// <summary>
        /// Gets the percentage this bar is filled up.
        /// </summary>
        /// <returns>The percentage.</returns>
        public float GetPercentageFull()
        {
            return (float)((bucket.currentValue / bucket.maxValue) * 100.0);
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
            if( DRAW_TEXTURE == null )
               DRAW_TEXTURE = DrawUtil.GetClearTexture2D(sb);

            sb.Draw(DRAW_TEXTURE, this.GetDrawRectangle(), null, EMPTY_COLOR, 0f,
                Vector2.Zero, SpriteEffects.None, bucket.z - 0.001f);

            sb.Draw(DRAW_TEXTURE, this.GetDrawRectangle(), null, FILL_COLOR, 0f,
                Vector2.Zero, SpriteEffects.None, bucket.z - 0.001f);

        }
    }
}
