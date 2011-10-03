using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using XNAInputHandler.MouseInput;

namespace MiniGameOverview.Backgrounds
{
    public class Pencil : MouseMotionListener
    {
        public Texture2D texture { get; set; }

        public Vector2 location { get; set; }
        public Vector2 scale { get; set; }

        public Pencil()
        {
            this.scale = new Vector2(0.5f, 0.5f);

            this.texture = MiniGameOverviewMainGame.GetInstance().game.Content.Load<Texture2D>("Misc/penseel");

            MouseManager.GetInstance().mouseMotionListeners += this.OnMouseMotion;
            MouseManager.GetInstance().mouseDragListeners += this.OnMouseDrag;
        }

        /// <summary>
        /// Gets the rectangle that you should use to draw this with.
        /// </summary>
        /// <returns>bla</returns>
        public Rectangle GetDrawRectangle()
        {
            return new Rectangle((int)(this.location.X - ((this.texture.Width / 2) * this.scale.X)),
                (int)(this.location.Y),
                (int)(this.texture.Width * this.scale.X),
                (int)(this.texture.Height * this.scale.Y));
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this.texture, this.GetDrawRectangle(), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.85f);
        }

        public void OnMouseMotion(MouseEvent e)
        {
            this.location = new Vector2(e.location.X, e.location.Y);
        }

        public void OnMouseDrag(MouseEvent e)
        {
            this.location = new Vector2(e.location.X, e.location.Y);
        }
    }
}
