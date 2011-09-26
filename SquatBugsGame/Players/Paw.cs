using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAInputHandler.MouseInput;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SquatBugsGame.Managers;
using SquatBugsGame.Bugs;
using SquatBugsGame.Players;
using SquatBugsGame;

namespace SquatBugsGame.Players
{
    public class Paw : MouseClickListener
    {
        public Player player { get; set; }
        public Texture2D pawTexture { get; set; }
        public Vector3 location { get; set; }
        public Vector2 scale { get; set; }

        public MoveState state { get; set; }

        public enum MoveState
        {
            MovingToBug,
            MovingFromBug,
            Still
        }

        public Paw(Player player)
        {
            MouseManager.GetInstance().mouseClickedListeners += this.OnMouseClick;
            MouseManager.GetInstance().mouseReleasedListeners += this.OnMouseRelease;

            MouseManager.GetInstance().mouseMotionListeners += this.OnMouseMotion;
            MouseManager.GetInstance().mouseDragListeners += this.OnMouseDrag;

            this.pawTexture = SquatBugsMainGame.GetInstance().game.Content.Load<Texture2D>
                ("Misc/vliegenmepper");
            this.player = player;
            this.location = new Vector3(2000, 2000, 0.001f);

            this.scale = new Vector2(0.4f, 0.4f);

            this.state = MoveState.Still;
        }

        public void OnMouseMotion(MouseEvent e)
        {
            Rectangle drawRect = this.GetDrawRectangle();
            //this.location = new Vector3(e.location.X - drawRect.Width,
            //    SquatBugsMainGame.GetInstance().game.GraphicsDevice.Viewport.Height -
            //(drawRect.Height / 2),
            //    this.location.Z);
            this.location = new Vector3(e.location.X - (drawRect.Width - 20),
                e.location.Y - 10, this.location.Z);
        }

        public void OnMouseDrag(MouseEvent e)
        {

        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this.pawTexture, this.GetDrawRectangle(), null,
                Color.White, 0f, Vector2.Zero, SpriteEffects.None, location.Z);
        }

        public void OnMouseClick(MouseEvent m_event)
        {
            if (this.state == MoveState.Still)
                this.state = MoveState.MovingToBug;
        }

        public void OnMouseRelease(MouseEvent m_event)
        {

        }

        /// <summary>
        /// Performs a sting on the given location, popping balloons or not.
        /// </summary>
        /// <param name="location">The location to sting at.</param>
        public void PerformSting(Vector2 location)
        {
            //CustomArrayList<Balloon> clickedBalloons = new CustomArrayList<Balloon>();
            //for (int i = 0; i < BalloonManager.GetInstance().balloons.Count(); i++)
            //{
            //    Balloon balloon = BalloonManager.GetInstance().balloons.ElementAt(i);
            //    if (balloon.polygon.CheckCollision(new Vector2(location.X, location.Y)))
            //    {
            //        clickedBalloons.AddLast(balloon);
            //    }
            //}
            //if (clickedBalloons.Count() != 0)
            //    this.SortBalloonsByZ(clickedBalloons, false).GetFirst().OnPlayerClick();
        }

        /// <summary>
        /// Gets the draw rectangle of this .. paw.
        /// </summary>
        /// <returns>The rectangle</returns>
        public Rectangle GetDrawRectangle()
        {
            return new Rectangle((int)location.X, (int)location.Y,
                (int)(pawTexture.Width * this.scale.X * 1.5), (int)(pawTexture.Height * this.scale.Y * 1.5));
        }


        
    }
}
