using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MainGame.Backgrounds.Birds.BannerBirds
{
    public class BannerBird : Bird
    {
        public Texture2D bannerTexture { get; set; }
        public Vector2 bannerScale { get; set; }
        public String bannerText { get; set; }
        public SpriteFont font { get; set; }

        private Vector2 textSize { get; set; }
        public float textPadding = 5;

        public static Texture2D BANNER_TEXTURE { get; set; }

        public static SpriteFont HEADER_FONT { get; set; }
        public static SpriteFont CONTENT_FONT { get; set; }

        static BannerBird()
        {
            BANNER_TEXTURE = Game1.GetInstance().Content.Load<Texture2D>("Menu/vaandel");

            HEADER_FONT = Game1.GetInstance().Content.Load<SpriteFont>("Fonts/Credits/Header");
            CONTENT_FONT = Game1.GetInstance().Content.Load<SpriteFont>("Fonts/Credits/Content");
        }

        public BannerBird(BirdFlock flock, int nrInFlock, String bannerText)
            : base(flock, 0)
        {
            bannerTexture = BANNER_TEXTURE;
            this.bannerScale = new Vector2(0.5f, 0.5f);
            this.speed = new Vector2(2.5f, 0f);

            this.location = new Vector2(
                this.location.X - (nrInFlock * 25),
                300 + (nrInFlock * this.GetDrawRectangle().Height));

            this.bannerText = bannerText;

            this.font = (nrInFlock == 0) ? HEADER_FONT : CONTENT_FONT;

            this.textSize = this.font.MeasureString(this.bannerText);
        }

        /// <summary>
        /// Gets the rectangle you should use when drawing the banner of the bird.
        /// </summary>
        /// <returns>The rectangle</returns>
        public Rectangle GetBannerDrawRectangle()
        {
            int width = (int)(this.textSize.X + (200 * this.bannerScale.X) + (this.textPadding * 2));
            //int width = (int)(this.bannerTexture.Width * this.bannerScale.X);
            return new Rectangle((int)(this.location.X - width + (
                (this.flock.direction == BirdFlock.FlyDirection.LeftToRight) ?
                // 10 to make sure the rope is in the bird
                (this.GetDrawRectangle().Width / 2) * -1 + this.textPadding :
                (this.GetDrawRectangle().Width / 2) - this.textPadding)),
                (int)(this.location.Y - ((this.bannerTexture.Height / 2) * this.bannerScale.Y)),
                width,
                (int)(this.bannerTexture.Height * this.bannerScale.Y));
        }

        public override Boolean OnScreen()
        {
            return this.GetBannerDrawRectangle().Intersects(Game1.GetInstance().GraphicsDevice.Viewport.Bounds);
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            Rectangle drawRect = this.GetBannerDrawRectangle();
            sb.Draw(this.bannerTexture, drawRect, null,
                Color.White, 0f, Vector2.Zero,
                (this.flock.direction == BirdFlock.FlyDirection.LeftToRight) ?
                SpriteEffects.FlipHorizontally : SpriteEffects.None,
                this.z + (0.001f * (this.nrInFlock + 1)));

            sb.DrawString(this.font, this.bannerText,
                new Vector2(
                /// 160 is a compensation for the length of the line
                    drawRect.X + (((drawRect.Width / 2) - (200 * this.bannerScale.X) / 2) - (this.textSize.X / 2)) + this.textPadding,
                    drawRect.Y + ((drawRect.Height / 2) - (this.textSize.Y / 2))), Color.Black);
        }
    }
}
