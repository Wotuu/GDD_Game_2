using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BuzzBattle.Shells;
using BuzzBattle.Misc;
using BuzzBattle.Managers;

namespace BuzzBattle.Shells
{
    public class PaintBomb
    {
        public Texture2D texture { get; set; }

        public Vector2 location { get; set; }
        public float z { get; set; }
        public Vector2 scale { get; set; }
        public Vector2 scaleDiminishRate = new Vector2(0.001f, 0.001f);
        public float speed { get; set; }
        // Direction used for travelling
        public Vector2 direction { get; set; }

        private Shell.ShellColor _color { get; set; }
        public Shell.ShellColor color
        {
            get
            {
                return _color;
            }
            set
            {
                switch (value)
                {
                    case Shell.ShellColor.Blue:
                        this.texture = VulnerabilityIndicator.BLUE_TEXTURE;
                        break;
                    case Shell.ShellColor.Pink:
                        this.texture = VulnerabilityIndicator.PINK_TEXTURE;
                        break;
                    case Shell.ShellColor.Green:
                        this.texture = VulnerabilityIndicator.GREEN_TEXTURE;
                        break;
                    case Shell.ShellColor.Yellow:
                        this.texture = VulnerabilityIndicator.YELLOW_TEXTURE;
                        break;
                }
                this._color = value;
            }
        }

        public Vector2 target { get; set; }

        public PaintBomb(Shell shell, Point target)
        {
            this.scale = new Vector2(0.2f, 0.2f);
            this.speed = 4f;

            this.location = new Vector2(shell.GetDrawRectangle().Center.X, shell.GetDrawRectangle().Center.Y);
            this.z = shell.z - 0.001f;
            this.color = shell.color;

            this.target = new Vector2(target.X, target.Y);
        }

        /// <summary>
        /// Gets the rectangle that you should use to draw this with.
        /// </summary>
        /// <returns>bla</returns>
        public Rectangle GetDrawRectangle()
        {
            return new Rectangle((int)(this.location.X - ((this.texture.Width / 2f) * this.scale.X)),
                (int)(this.location.Y - ((this.texture.Height / 2f) * this.scale.Y)),
                (int)(this.texture.Width * this.scale.X),
                (int)(this.texture.Height * this.scale.Y));
        }

        public void Update()
        {
            this.scale -= (this.scaleDiminishRate * ((float)GameTimeManager.GetInstance().time_step));

            if (this.scale.X < 0.03f)
            {
                this.scale = Vector2.Zero;
            }

            this.Move();
            this.CheckCollision();
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this.texture, this.GetDrawRectangle(), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, this.z);
        }

        /// <summary>
        /// Set the point this Unit has to move to.
        /// direction != direction is used for checking NaNExceptions.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private float GetRotation(Vector2 target)
        {
            Point center = this.GetDrawRectangle().Center;
            double a = Math.Abs(center.X - target.X);
            double b = Math.Abs(center.Y - target.Y);
            return (float)Math.Atan(a / b);
        }

        /// <summary>
        /// Moves the bomb.
        /// </summary>
        public void Move()
        {
            Point hookLocation = this.GetDrawRectangle().Center;

            float direction = this.GetRotation(target);

            float speed = Math.Max(this.speed, this.speed);

            Boolean invertX = false;
            Boolean invertY = false;
            if (hookLocation.X < target.X && hookLocation.Y > target.Y)
            {
                invertY = true;
            }
            else if (hookLocation.X > target.X && hookLocation.Y < target.Y)
            {
                invertX = true;
            }
            else if (hookLocation.X > target.X && hookLocation.Y > target.Y)
            {
                invertX = true;
                invertY = true;
            }
            else if (hookLocation.X > target.X && hookLocation.Y == target.Y)
            {
                invertX = true;
            }
            else if (hookLocation.X == target.X && hookLocation.Y > target.Y)
            {
                invertY = true;
            }

            this.direction = new Vector2(
                (invertX) ? (speed * (float)Math.Sin(direction)) * -1 : (speed * (float)Math.Sin(direction)),
                (invertY) ? (speed * (float)Math.Cos(direction)) * -1 : (speed * (float)Math.Cos(direction))
                );

            this.location += this.direction;

            // If we're almost there
            if (Math.Abs(this.location.Length() - this.target.Length()) < 5f)
            {
                this.scale = Vector2.Zero;
            }
        }

        /// <summary>
        /// Checks collision with buzz!
        /// </summary>
        public void CheckCollision()
        {
            if (/*this.scale.X < 0.20f && */this.GetDrawRectangle().Intersects(
                BuzzBattleMainGame.GetInstance().buzz.GetDrawRectangle()))
            {
                if (BuzzBattleMainGame.GetInstance().buzz.vulnerabilityIndicator.vulnerability == this.color)
                {
                    // You rule!
                    BuzzBattleMainGame.GetInstance().buzz.currentHealth -= 1;
                    this.scale = Vector2.Zero;
                }
            }
        }
    }
}
