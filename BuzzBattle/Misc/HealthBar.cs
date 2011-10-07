using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BuzzBattle.Players;
using BuzzBattle.Util;

namespace BuzzBattle.Misc
{
    public class HealthBar
    {

        public Rectangle bounds { get; set; }

        private float z { get; set; }
        public float percentage { get; set; }
        public Buzz buzz { get; set; }

        public float maxValue { get; set; }
        public float currentValue { get; set; }

        /// <summary>
        /// Set to true to dynamically color this healthbar according to its emptyness.
        /// @see fullColor, emptyColor
        /// </summary>
        public Boolean useDynamicColoring { get; set; }
        public Color fullColor { get; set; }
        public Color emptyColor { get; set; }

        public Boolean visible { get; set; }

        private Texture2D texture { get; set; }

        private enum Type
        {
            Building,
            Unit
        }

        public static Color
            BORDER_COLOR = new Color(0, 0, 0, 255),
            BACKGROUND_COLOR = new Color(255, 0, 0, 255),
            FOREGROUND_COLOR = new Color(0, 255, 0, 255);

        public HealthBar(Buzz buzz, Rectangle bounds, float maxValue)
        {
            this.buzz = buzz;
            this.bounds = bounds;

            this.maxValue = maxValue;

            this.useDynamicColoring = true;
            fullColor = new Color(0, 255, 0, 255);
            emptyColor = new Color(255, 0, 0, 255);

            this.z = this.buzz.z - 0.01f;

            this.visible = true;
        }


        internal void Draw(SpriteBatch sb)
        {
            if (!visible) return;

            this.texture = (this.texture == null) ? (DrawUtil.lineTexture = DrawUtil.GetClearTexture2D(sb)) : DrawUtil.lineTexture;

            this.percentage = ((this.currentValue / this.maxValue) * 100f);

            int innerWidth = (int)((bounds.Width / 100f) * percentage);

            sb.Draw(this.texture, new Rectangle(this.bounds.X, this.bounds.Y,
                this.bounds.Width + 2, this.bounds.Height), null, BORDER_COLOR, 0f, Vector2.Zero, SpriteEffects.None, z);

            sb.Draw(this.texture, new Rectangle(this.bounds.X + 1, this.bounds.Y + 1,
                this.bounds.Width, this.bounds.Height - 2), null, BACKGROUND_COLOR, 0f, Vector2.Zero, SpriteEffects.None, z - 0.0001f);

            Color foreground = Color.Gold;
            if (this.useDynamicColoring) foreground = this.GetDynamicColor();
            else foreground = FOREGROUND_COLOR;

            sb.Draw(this.texture, new Rectangle(this.bounds.X + 1, this.bounds.Y + 1,
                innerWidth, this.bounds.Height - 2), null, foreground, 0f, Vector2.Zero, SpriteEffects.None, z - 0.0002f);

            /*
            sb.Draw(texture, new Rectangle(x, y, w + 2, h), BORDER_COLOR);
            sb.Draw(texture, new Rectangle(x + 1, y + 1, w, h - 2), BACKGROUND_COLOR);
            sb.Draw(texture, new Rectangle(x + 1, y + 1, innerWidth, h - 2), FOREGROUND_COLOR);*/
        }

        /// <summary>
        /// Gets a dynamic color.
        /// </summary>
        /// <returns>The color.</returns>
        public Color GetDynamicColor()
        {
            int[] emptyData = new int[] { emptyColor.R, emptyColor.G, emptyColor.B, emptyColor.A };
            int[] fullData = new int[] { fullColor.R, fullColor.G, fullColor.B, fullColor.A };

            int[] differenceData = new int[] { emptyColor.R - fullColor.R, emptyColor.G - fullColor.G, 
                emptyColor.B - fullColor.B, emptyColor.A - fullColor.A };

            return Color.FromNonPremultiplied(
                emptyColor.R - (int)((differenceData[0] / 100.0) * this.percentage),
                emptyColor.G - (int)((differenceData[1] / 100.0) * this.percentage),
                emptyColor.B - (int)((differenceData[2] / 100.0) * this.percentage),
                emptyColor.A - (int)((differenceData[3] / 100.0) * this.percentage));

        }
    }
}
