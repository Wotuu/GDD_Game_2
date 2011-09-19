using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Polygons.Polygons;
using Microsoft.Xna.Framework.Graphics;
using CustomLists.Lists;
using Microsoft.Xna.Framework;
using BalloonPaintBucketGame.Managers;
using BalloonPaintBucketGame.Particles.Emitters;
using PolygonCollision.Managers;

namespace BalloonPaintBucketGame.Balloons
{
    public class Balloon
    {
        public CollideablePolygon polygon { get; set; }
        private Texture2D texture { get; set; }
        public BalloonColor color { get; set; }

        private Vector2 _location { get; set; }
        public Vector2 location
        {
            get
            {
                return this._location;
            }
            set
            {
                this._location = value;
                this.polygon.offset = value;
            }
        }
        /// <summary>
        /// The Y at which the balloon will stop descending, and start moving horizontally.
        /// </summary>
        public float targetY { get; set; }
        /// <summary>
        /// Used for checking if this balloon is still descending to its targetY.
        /// </summary>
        public Boolean descending { get; set; }

        private Vector2 _scale { get; set; }
        public Vector2 scale
        {
            get
            {
                return _scale;
            }
            set
            {
                this._scale = value;
                this.polygon.scale = value;
            }
        }
        public float z { get; set; }

        public static Texture2D PINK_BALLOON { get; set; }

        public static Texture2D BLUE_BALLOON { get; set; }
        public static Texture2D YELLOW_BALLOON { get; set; }
        public static Texture2D BLACK_BALLOON { get; set; }

        public Vector2 speed { get; set; }

        public enum BalloonColor
        {
            Pink = 0,
            Blue = 1,
            Yellow = 2,
            Black = 3
        }

        static Balloon()
        {
            PINK_BALLOON = BalloonPaintBucketMainGame.GetInstance().game.Content.Load<Texture2D>("Balloons/balon1");
            BLUE_BALLOON = BalloonPaintBucketMainGame.GetInstance().game.Content.Load<Texture2D>("Balloons/balon2");
            YELLOW_BALLOON = BalloonPaintBucketMainGame.GetInstance().game.Content.Load<Texture2D>("Balloons/balon3");
            BLACK_BALLOON = BalloonPaintBucketMainGame.GetInstance().game.Content.Load<Texture2D>("Balloons/balon4");
        }

        public Balloon(BalloonColor color)
        {
            CustomArrayList<Vector2> vertices = new CustomArrayList<Vector2>();

            vertices.AddLast(new Vector2(223, 3));
            vertices.AddLast(new Vector2(83, 50));
            vertices.AddLast(new Vector2(18, 138));
            vertices.AddLast(new Vector2(2, 240));
            vertices.AddLast(new Vector2(19, 337));
            vertices.AddLast(new Vector2(88, 444));
            vertices.AddLast(new Vector2(270, 512));
            vertices.AddLast(new Vector2(371, 459));
            vertices.AddLast(new Vector2(439, 337));
            vertices.AddLast(new Vector2(444, 231));
            vertices.AddLast(new Vector2(407, 104));
            vertices.AddLast(new Vector2(316, 23));

            this.polygon = new CollideablePolygon(vertices);

            this.color = color;
            switch (color)
            {
                case BalloonColor.Pink:
                    this.texture = PINK_BALLOON;
                    break;
                case BalloonColor.Blue:
                    this.texture = BLUE_BALLOON;
                    break;
                case BalloonColor.Yellow:
                    this.texture = YELLOW_BALLOON;
                    break;
                case BalloonColor.Black:
                    this.texture = BLACK_BALLOON;
                    break;
            }

            Random random = new Random();
            this.location = new Vector2(
                random.Next(BalloonPaintBucketMainGame.GetInstance().game.GraphicsDevice.Viewport.Width -
                this.texture.Width), -300);
            this.targetY = 25 + random.Next(50);
            this.descending = true;

            this.scale = new Vector2(0.35f, 0.35f);
            BalloonManager.GetInstance().balloons.AddLast(this);

            this.z = 0.9f - (BalloonManager.GetInstance().balloons.Count() * 0.001f);
            this.polygon.z = this.z - 0.001f;

            this.speed = new Vector2(4 * this.scale.X, 8 * this.scale.Y);
        }

        public void Update()
        {
            if (this.descending)
            {
                this.location = new Vector2(this.location.X,
                    this.location.Y + (this.speed.Y * (float)GameTimeManager.GetInstance().time_step));

                if (this.location.Y > this.targetY)
                {
                    this.location = new Vector2(this.location.X, this.targetY);
                    this.descending = false;
                }
            }
            else
            {
                this.location += (this.speed * (float)GameTimeManager.GetInstance().time_step);
                Rectangle drawRect = this.GetDrawRectangle();
                float newXSpeed = this.speed.X;
                float newYSpeed =
                    (this.speed.X > 0) ?
                    // Normal rotation
                    (float)Math.Sin(MathHelper.ToRadians(this.location.X)) :
                    // Inverse otherwise
                    (float)Math.Sin(MathHelper.ToRadians(this.location.X - (
                    BalloonPaintBucketMainGame.GetInstance().game.GraphicsDevice.Viewport.Width -
                    drawRect.Width)));
                if (drawRect.Left < 0)
                {
                    newXSpeed *= -1;
                    this.location = new Vector2(0, this.location.Y);
                }
                else if (
                  drawRect.Right > BalloonPaintBucketMainGame.GetInstance().game.GraphicsDevice.Viewport.Width)
                {
                    newXSpeed *= -1;
                    this.location = new Vector2(drawRect.Left, this.location.Y);
                }
                this.speed = new Vector2(newXSpeed, newYSpeed);
            }
        }


        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this.texture, this.GetDrawRectangle(), null, Color.White,
                0f, Vector2.Zero, SpriteEffects.None, z);
        }

        /// <summary>
        /// When the player has clicked on this balloon.
        /// </summary>
        public void OnPlayerClick()
        {
            if (this.descending) return;
            BalloonManager.GetInstance().balloons.Remove(this);
            new BalloonDeathEmitter(this);
            new PaintEmitter(this);
            this.polygon.Destroy();
        }

        /// <summary>
        /// Gets the draw rectangle of this balloon.
        /// </summary>
        /// <returns>The rectangle that can be used for drawing.</returns>
        public Rectangle GetDrawRectangle()
        {
            return new Rectangle((int)this.location.X, (int)this.location.Y,
                (int)(this.texture.Width * this.scale.X), (int)(this.texture.Height * this.scale.Y));
        }

        /// <summary>
        /// Gets the color of this balloon.
        /// </summary>
        /// <returns>The color.</returns>
        public Color GetColor()
        {
            switch (this.color)
            {
                case BalloonColor.Pink:
                    return Color.DeepPink;

                case BalloonColor.Blue:
                    return Color.Blue;

                case BalloonColor.Yellow:
                    return Color.Yellow;

                case BalloonColor.Black:
                    return Color.DarkGray;
            }
            return Color.Red;
        }

        /// <summary>
        /// Gets the center of this balloon.
        /// </summary>
        /// <returns>The dead center.</returns>
        public Vector2 GetCenter()
        {
            return new Vector2(this.location.X + ((this.texture.Width * this.scale.X) / 2),
                this.location.Y + ((this.texture.Height * this.scale.Y) / 2));
        }
    }
}
