using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SquatBugsGame.Players.Life
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
        private Vector2 _enemielocation { get; set; }
        public Vector2 enemielocation
        {
            get
            {
                return _enemielocation;
            }
            set
            {
                this._enemielocation = value;

                for (int i = 0; i < this.EnemieBugsLives.Count; i++)
                {
                    this.EnemieBugsLives.ElementAt(i).location = value;
                }
            }
        }
        public Vector2 scale { get; set; }
        public Vector2 enemiescale { get; set; }
        public LinkedList<BalloonLife> lives = new LinkedList<BalloonLife>();
        public LinkedList<EnemieBugsLife> EnemieBugsLives = new LinkedList<EnemieBugsLife>();

        public LifeDisplayPanel(int lives,int enemielives)
        {
            this.scale = new Vector2(0.1f, 0.3f);
            
            for (int i = 0; i < lives; i++)
            {
                this.lives.AddLast(new BalloonLife(this, i));
            }

            this.enemiescale = new Vector2(0.05f, .15f);
            for (int i = 0; i < enemielives; i++)
            {
                this.EnemieBugsLives.AddLast(new EnemieBugsLife(this, i));
            }

            BalloonLife first = this.lives.First.Value;
            this.location = new Vector2(
                SquatBugsMainGame.GetInstance().game.GraphicsDevice.Viewport.Width -
                ((first.GetDrawRectangle().Width * lives) + (first.balloonPadding * lives)), 
                (first.GetDrawRectangle().Height / 2) + first.balloonPadding);

            EnemieBugsLife firstenemie = this.EnemieBugsLives.First.Value;
            this.enemielocation = new Vector2(
                (firstenemie.balloonPadding * enemielives),
                (firstenemie.GetDrawRectangle().Height / 2) + firstenemie.balloonPadding);


        }

        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < this.lives.Count; i++)
            {
                this.lives.ElementAt(i).Draw(sb);
            }

            for (int i = 0; i < this.EnemieBugsLives.Count; i++)
            {
                this.EnemieBugsLives.ElementAt(i).Draw(sb);
            }


        }
    }
}
