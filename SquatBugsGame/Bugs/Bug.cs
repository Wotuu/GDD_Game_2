using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SquatBugsGame.Managers;
using SquatBugsGame.Particles.Emitters;

namespace SquatBugsGame.Bugs
{
    abstract class Bug
    {

        public enum BugColor
        {
            Pink = 0,
            Green = 1,
            Blue = 2,
            Black = 3
        }
        public Vector2 Speed;
        public int Score;
        public double ChangeDirectionTimer;
        public Texture2D BUG;
        public Vector2 location;
        public Vector2 scale;
        private float rotation;
        public float RotationCorrection;
        private Viewport viewport = SquatBugsMainGame.GetInstance().game.GraphicsDevice.Viewport;
        public Rectangle drawRectangle;
        public Rectangle locendrect;
        public Boolean IsDead = false;
        public Random random;
        public bool NewBug = true;
        public BugColor bugcolor;
        public Color DrawColor = Color.White;

        public float FadeTime = 1500;
        public double FadeTimer;


        public Vector2 EndLocation;

        protected int TimePerSpriteFrame = 100;
        public Double SpriteTimer = 100;
        private int spritenr = 1;
        public int NrOfFrames;



        public void Draw(SpriteBatch sb)
        {
            Rectangle source = this.GetSourceRectangle();
            //sb.Draw(BUG, GetDrawRectangle(), this.color);
            sb.Draw(BUG, this.GetDrawRectangle(),
                source,
                DrawColor, rotation, new Vector2(source.Width / 2f, source.Height / 2f), SpriteEffects.None, 0.9f);


            // Util.DrawClearRectangle(sb, this.GetCollisionRectangle(), 1, Color.Red, 0.9f);
        }

        public void DrawDebug(SpriteBatch sb, int i)
        {
            sb.DrawString(SquatBugsMainGame.GetInstance().font, i.ToString(), location, Color.Red, 0f, Vector2.Zero, 10f, SpriteEffects.None, .01f);
            sb.DrawString(SquatBugsMainGame.GetInstance().font, i.ToString(), EndLocation, Color.Red, 0f, Vector2.Zero, 10f, SpriteEffects.None, .01f);
        }

        public void Update(Random random)
        {
            if (!this.IsDead)
            {

                this.random = random;
                ChangeDirectionTimer -= GameTimeManager.GetInstance().currentUpdateStartMS - GameTimeManager.GetInstance().previousUpdateStartMS;
                if (ChangeDirectionTimer <= 0 && !NewBug)
                {
                    Speed = getRandomSpeed(random);
                    ChangeDirectionTimer = random.Next(1000, 20000);
                }

                if (Speed != Vector2.Zero)
                {
                    CalculateRotation();
                }
                else
                {
                    //Speed = getRandomSpeed(random);
                }
                DoFrameCalculations();
                SpeedCalculations();
            }
            else
            {
                DeathCalculations();
            }
            //drawRectangle = new Rectangle((int)location.X + (int)Math.Sin(rotation), (int)location.Y - (int)Math.Cos(rotation), (int)((float)BUG.Width / NrOfFrames * scale.X), (int)((float)BUG.Height * scale.Y));
            drawRectangle = new Rectangle((int)(location.X - (BUG.Width / (2 * NrOfFrames) * scale.X)), (int)(location.Y - BUG.Height / 2 * scale.Y), (int)((BUG.Width / NrOfFrames) * scale.X), (int)(BUG.Height * scale.Y));


        }

        private void DoFrameCalculations()
        {
            SpriteTimer -= GameTimeManager.GetInstance().currentUpdateStartMS - GameTimeManager.GetInstance().previousUpdateStartMS;
            if (SpriteTimer <= 0)
            {
                if (spritenr == NrOfFrames)
                {
                    spritenr = 1;
                }
                else
                {
                    spritenr++;
                }

                SpriteTimer = TimePerSpriteFrame;
            }
        }

        /// <summary>
        /// Gets the source rectangle of the bug.
        /// </summary>
        /// <returns>The source rectangle you should use when drawing!</returns>
        public Rectangle GetSourceRectangle()
        {
            return new Rectangle((BUG.Width / NrOfFrames) * (spritenr - 1), 0, (BUG.Width / NrOfFrames), BUG.Height);
        }

        /// <summary>
        /// Gets the rectangle you should use for the collision.
        /// </summary>
        /// <returns>The collision rectangle</returns>
        public Rectangle GetCollisionRectangle()
        {
            Rectangle source = this.GetSourceRectangle();
            return new Rectangle((int)(this.location.X - (source.Width * this.scale.X)),
                (int)(this.location.Y - (source.Height * this.scale.Y)),
                (int)(source.Width * this.scale.X), (int)(source.Height * this.scale.Y));
        }

        /// <summary>
        /// Gets the draw rectangle of this bug.
        /// </summary>
        /// <returns>The rectangle that can be used for drawing.</returns>
        public Rectangle GetDrawRectangle()
        {
            Rectangle source = this.GetSourceRectangle();
            return new Rectangle((int)(this.location.X - (source.Width * this.scale.X) / 2),
                (int)(this.location.Y - (source.Height * this.scale.Y) / 2),
                (int)(source.Width * this.scale.X), (int)(source.Height * this.scale.Y));
        }

        /// <summary>
        /// Gets the center of this Bug.
        /// </summary>
        /// <returns>The dead center.</returns>
        public Vector2 GetCenter()
        {
            Rectangle source = this.GetSourceRectangle();
            return new Vector2(this.location.X + ((source.Width * this.scale.X) / 2),
                this.location.Y + ((source.Height * this.scale.Y) / 2));
            //return new Vector2((BUG.Width / 4) / 2, BUG.Height / 2);
        }

        public Boolean NotOutOfBounds()
        {
            Rectangle location = this.GetCollisionRectangle();
            if ((location.Center.X + Speed.X) > viewport.Width || (location.Center.X + Speed.X) < 0 ||
                (location.Center.Y + Speed.Y) > viewport.Height || (location.Center.Y + Speed.Y) < 0)
            {
                return false;
            }
            return true;
        }

        public Boolean IsBugAtEndPoint(Vector2 endlocation)
        {

            locendrect = new Rectangle((int)endlocation.X - (drawRectangle.Width), (int)endlocation.Y - (drawRectangle.Height), drawRectangle.Width * 2, drawRectangle.Height * 2);
            if (locendrect.Intersects(drawRectangle))
            {
                return true;
            }

            return false;
        }

        public void CheckBounds()
        {
            if (NotOutOfBounds())
            {
                location += Speed * (float)GameTimeManager.GetInstance().time_step;
            }
            else
            {
                Speed = getRandomSpeed(random);
            }

        }

        private void SpeedCalculations()
        {

            if (NewBug)
            {
                //Move bug towards screen
                //float Rotation = 
                float xSpeedDirection = 4 * (float)GameTimeManager.GetInstance().time_step * (float)Math.Sin(rotation - RotationCorrection + 1.5f);
                float ySpeedDirection = 4 * (float)GameTimeManager.GetInstance().time_step * (float)Math.Cos(rotation - RotationCorrection + 1.5f);

                location.Y -= ySpeedDirection;
                location.X += xSpeedDirection;

                if (IsBugAtEndPoint(EndLocation))
                {
                    NewBug = false;
                    locendrect = Rectangle.Empty;

                }
            }
            else
            {
                CheckBounds();
            }
        }

        private void CalculateRotation()
        {
            if (!NewBug)
            {
                rotation = (float)Math.Atan2((Speed.Y + location.Y) - location.Y, (Speed.X + location.X) - location.X);
            }
            else
            {
                rotation = (float)Math.Atan2((EndLocation.Y) - location.Y, (EndLocation.X) - location.X);
            }
            rotation = rotation + RotationCorrection;
        }

        public Vector2 getRandomSpeed(Random random)
        {
            return new Vector2((float)random.Next(-15, 15) / 10, (float)random.Next(-15, 15) / 10);
        }

        public void Destroy()
        {
            new BugDeathEmitter(this);
            IsDead = true;
        }

        public Color GetColor()
        {
            switch (bugcolor)
            {
                case BugColor.Pink:
                    return Color.Pink;
                case BugColor.Blue:
                    return Color.Blue;
                case BugColor.Green:
                    return Color.Green;
                default:
                    return Color.SlateGray;
            }
        }

        public void DeathCalculations()
        {
            this.FadeTimer -= GameTimeManager.GetInstance().currentUpdateStartMS - GameTimeManager.GetInstance().previousUpdateStartMS;

            double AlphaValue = (float)(255 / FadeTime) * FadeTimer;

            DrawColor = new Color((int)AlphaValue, (int)AlphaValue, (int)AlphaValue, (int)AlphaValue);
        }

    }
}
