using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MainGame.Animations;
using MainGame.Managers;

namespace MainGame.Backgrounds.Birds
{
    public class Bird
    {
        public Animator animator { get; set; }
        public BirdFlock flock { get; set; }

        public Vector2 location { get; set; }
        public float z { get; set; }
        public Vector2 scale { get; set; }
        public Vector2 speed { get; set; }

        public int nrInFlock { get; set; }

        public static Texture2D BIRD_TEXTURE { get; set; }

        static Bird()
        {
            BIRD_TEXTURE = Game1.GetInstance().Content.Load<Texture2D>("Menu/Vogel");
        }

        public Bird(BirdFlock flock, int nrInFlock)
        {
            this.flock = flock;

            this.nrInFlock = nrInFlock;

            this.scale = new Vector2(1f, 1f);
            this.speed = new Vector2(5f, 0f);

            this.animator = new Animator(BIRD_TEXTURE, new Rectangle(0, 0, 236, 288), 100);

            int offset = (nrInFlock % 2 != 0) ? 200 : 235;

            Random random = new Random();
            offset += random.Next(300);

            switch (this.flock.direction)
            {
                case BirdFlock.FlyDirection.LeftToRight:
                    this.location = new Vector2(-200 + (70 * nrInFlock), offset);
                    break;
                case BirdFlock.FlyDirection.RightToLeft:
                    this.location = new Vector2(Game1.GetInstance().graphics.PreferredBackBufferWidth + 100 + (70 * nrInFlock), offset);
                    this.speed *= -1;
                    break;
            }

            float tempScale = 0.2f;//(0.2f + (0.005f * random.Next(10)));
            this.scale = new Vector2(tempScale - 0.05f, tempScale);

            this.z = 0.99f;

            BirdManager.GetInstance().birds.AddLast(this);
        }

        public void Update()
        {
            this.location += (this.speed * (float)GameTimeManager.GetInstance().time_step);

            if (((this.flock.direction == BirdFlock.FlyDirection.LeftToRight && this.location.X > 200) ||
                (this.flock.direction == BirdFlock.FlyDirection.RightToLeft && this.location.X < 200)) &&
                !this.GetDrawRectangle().Intersects(Game1.GetInstance().GraphicsDevice.Viewport.Bounds))
            {
                BirdManager.GetInstance().birds.Remove(this);
            }
        }

        /// <summary>
        /// Gets the rectangle that you should use to draw this with.
        /// </summary>
        /// <returns>bla</returns>
        public Rectangle GetDrawRectangle()
        {
            return new Rectangle((int)(this.location.X - ((this.animator.sourceTexture.Width / 2) * this.scale.X)),
                (int)(this.location.Y - ((this.animator.sourceTexture.Height / 2) * this.scale.Y)),
                (int)(this.animator.sourceTexture.Width * this.scale.X),
                (int)(this.animator.sourceTexture.Height * this.scale.Y));
        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(this.animator.sourceTexture, this.GetDrawRectangle(),
                this.animator.GetSourceRectangle(GameTimeManager.GetInstance().currentUpdateStartMS),
                Color.White, 0f, Vector2.Zero,
                (this.flock.direction == BirdFlock.FlyDirection.LeftToRight) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 
                this.z - ( 0.01f * this.nrInFlock ));
        }
    }
}
