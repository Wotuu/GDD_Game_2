using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SquatBugsGame.Players.Life
{
    public class BalloonLife
    {
        public Texture2D texture { get; set; }

        public Vector2 location { get; set; }
        public Vector2 scale { get; set; }

        public LifeDisplayPanel panel { get; set; }
        public int index { get; set; }
        public int balloonPadding = 5;

        public BalloonLife(LifeDisplayPanel panel, int index)
        {
            this.panel = panel;
            this.index = index;

            this.scale = panel.scale;

            this.texture = SquatBugsMainGame.GetInstance().game.Content.Load<Texture2D>("Bugs/lieveheers_blauw_sprite");
        }

        /// <summary>
        /// Gets the rectangle that you should use to draw this with.
        /// </summary>
        /// <returns>bla</returns>
        public Rectangle GetDrawRectangle()
        {
            int width = (int)(this.texture.Width * this.scale.X);
            return new Rectangle((int)(this.location.X + (
                (width * index) +
                (index * balloonPadding))),
                (int)(this.location.Y - (this.texture.Height / 2 * this.scale.Y)),
                width,
                (int)(this.texture.Height * scale.Y));
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this.texture, this.GetDrawRectangle(), new Rectangle(0,0,texture.Width / 3 ,texture.Height), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.9f);
        }
    }
}
