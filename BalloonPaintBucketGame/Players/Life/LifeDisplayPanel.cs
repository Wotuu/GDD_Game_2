using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BalloonPaintBucketGame.Players.Life
{
    public class LifeDisplayPanel
    {
        private Vector2 _location { get; set; }
        public Vector2 location
        {
            get
            {
                return _location;
            }
            set
            {
                this._location = value;

                for (int i = 0; i < this.lives.Count; i++)
                {
                    this.lives.ElementAt(i).location = value;
                }
            }
        }
        public Vector2 scale { get; set; }

        public LinkedList<BalloonLife> lives = new LinkedList<BalloonLife>();

        public LifeDisplayPanel(int lives)
        {
            this.scale = new Vector2(0.1f, 0.1f);

            for (int i = 0; i < lives; i++)
            {
                this.lives.AddLast(new BalloonLife(this, i));
            }

            BalloonLife first = this.lives.First.Value;
            this.location = new Vector2(
                BalloonPaintBucketMainGame.GetInstance().game.GraphicsDevice.Viewport.Width -
                ((first.GetDrawRectangle().Width * lives) + (first.balloonPadding * lives)), 
                (first.GetDrawRectangle().Height / 2) + first.balloonPadding);
        }

        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < this.lives.Count; i++)
            {
                this.lives.ElementAt(i).Draw(sb);
            }
        }
    }
}
