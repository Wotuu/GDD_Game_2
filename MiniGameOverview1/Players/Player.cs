using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniGameOverview.Animations;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MiniGameOverview.Managers;

namespace MiniGameOverview.Players
{
    public class Player
    {
        public Animator animator { get; set; }
        public Vector2 position { get; set; }
        public Vector2 scale { get; set; }

        public Player()
        {
            this.scale = new Vector2(0.75f, 0.75f);
            this.animator = new Animator(
                MiniGameOverviewMainGame.GetInstance().game.Content.Load<Texture2D>("Player/fuzz_sprite"),
                new Rectangle(0, 0, 267, 258), 300);
        }

        /// <summary>
        /// Gets the Draw rectangle of the player.
        /// </summary>
        /// <returns>The rectangle to draw.</returns>
        public Rectangle GetDrawRectangle()
        {
            return new Rectangle((int)(this.position.X), (int)(this.position.Y),
                (int)(267 * this.scale.X), (int)(258 * this.scale.Y));
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this.animator.sourceTexture, this.GetDrawRectangle(),
                this.animator.GetSourceRectangle(GameTimeManager.GetInstance().currentUpdateStartMS),
                Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
        }
    }
}
