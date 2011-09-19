using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SquatBugsGame.Managers;

namespace SquatBugsGame.Bugs
{
    abstract class Bug
    {
        public Vector2 Speed;
        public int Score;
        public double ChangeDirectionTimer;
        public Color color;
        public static Texture2D BUG;
        public Vector2 location;
        public Vector2 scale;
        private float rotation;
        private Viewport viewport = SquatBugsMainGame.GetInstance().game.GraphicsDevice.Viewport;
        public Rectangle drawRectangle;
        public Boolean IsDead = false;


        public void Draw(SpriteBatch sb)
        {
            //sb.Draw(BUG, GetDrawRectangle(), this.color);
            sb.Draw(BUG, location, new Rectangle(0,0,BUG.Width,BUG.Height), this.color, rotation, GetCenter(),scale, SpriteEffects.None, 0f);
        }

        public void DrawDebug(SpriteBatch sb, int i)
        {
            //sb.DrawString(SquatBugsMainGame.GetInstance().font, i.ToString(), location, Color.Red,0f,Vector2.Zero,1f,SpriteEffects.None,1f);
        }

        public void Update(Random random)
        {

            ChangeDirectionTimer -= GameTimeManager.GetInstance().currentUpdateStartMS - GameTimeManager.GetInstance().previousUpdateStartMS;
            if (ChangeDirectionTimer <= 0)
            {
                Speed = getRandomSpeed(random);
                ChangeDirectionTimer = random.Next(1000, 20000);
            }

            if (Speed != Vector2.Zero)
            {
                rotation = (float)Math.Atan2((Speed.Y + location.Y) - location.Y, (Speed.X + location.X) - location.X);
                rotation = rotation + 1.3f;
            }
            else
            {
                Speed = getRandomSpeed(random);
            }

            if (NotOutOfBounds())
            {
                location += Speed * (float)GameTimeManager.GetInstance().time_step;
            }
            else
            {
                Speed = getRandomSpeed(random);
                //ChangeDirectionTimer = random.Next(1000, 20000);
            }
            drawRectangle = new Rectangle((int)location.X, (int)location.Y, BUG.Width, BUG.Height);

        }


        /// <summary>
        /// Gets the draw rectangle of this bug.
        /// </summary>
        /// <returns>The rectangle that can be used for drawing.</returns>
        public Rectangle GetDrawRectangle()
        {
            return new Rectangle((int)this.location.X, (int)this.location.Y,
                (int)(BUG.Width * this.scale.X), (int)(BUG.Height * this.scale.Y));
        }

        /// <summary>
        /// Gets the center of this Bug.
        /// </summary>
        /// <returns>The dead center.</returns>
        public Vector2 GetCenter()
        {
            //return new Vector2(this.location.X + ((BUG.Width * this.scale.X) / 2),
            //    this.location.Y + ((BUG.Height * this.scale.Y) / 2));
            return new Vector2(BUG.Width / 2, BUG.Height / 2);
        }

        public Boolean NotOutOfBounds()
        {
            if ((location.X  + Speed.X) > viewport.Width  || (location.X + Speed.X)  < 0 || (location.Y + Speed.Y)  > viewport.Height || (location.Y + Speed.Y) < 0)
            {
                return false;
            }
            return true;
        }

        public Vector2 getRandomSpeed(Random random)
        {
            return new Vector2((float)random.Next(-15, 15) / 10, (float)random.Next(-15, 15) / 10);
        }

        public void Destroy()
        {
            IsDead = true;
        }

    }
}
