using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAInputHandler.MouseInput;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CustomLists.Lists;
using BalloonPaintBucketGame.Managers;
using BalloonPaintBucketGame.Balloons;

namespace BalloonPaintBucketGame.Players
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
            MovingToBalloon,
            MovingFromBalloon,
            Still
        }

        public Paw(Player player)
        {
            MouseManager.GetInstance().mouseClickedListeners += this.OnMouseClick;
            MouseManager.GetInstance().mouseReleasedListeners += this.OnMouseRelease;

            MouseManager.GetInstance().mouseMotionListeners += this.OnMouseMotion;
            MouseManager.GetInstance().mouseDragListeners += this.OnMouseDrag;

            this.pawTexture = BalloonPaintBucketMainGame.GetInstance().game.Content.Load<Texture2D>
                ("Misc/poot");
            this.player = player;
            this.location = new Vector3(2000, 2000, player.z);

            this.scale = new Vector2(0.4f, 0.4f);

            this.state = MoveState.Still;
        }

        public void OnMouseMotion(MouseEvent e)
        {
            Rectangle drawRect = this.GetDrawRectangle();
            this.location = new Vector3(e.location.X - drawRect.Width,
                BalloonPaintBucketMainGame.GetInstance().game.GraphicsDevice.Viewport.Height -
            (drawRect.Height / 2),
                this.location.Z);
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
                this.state = MoveState.MovingToBalloon;
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
            CustomArrayList<Balloon> clickedBalloons = new CustomArrayList<Balloon>();
            for (int i = 0; i < BalloonManager.GetInstance().balloons.Count(); i++)
            {
                Balloon balloon = BalloonManager.GetInstance().balloons.ElementAt(i);
                if (balloon.polygon.CheckCollision(new Vector2(location.X, location.Y)))
                {
                    clickedBalloons.AddLast(balloon);
                }
            }
            if (clickedBalloons.Count() != 0)
                this.SortBalloonsByZ(clickedBalloons, false).GetFirst().OnPlayerClick();
        }

        /// <summary>
        /// Gets the draw rectangle of this .. paw.
        /// </summary>
        /// <returns>The rectangle</returns>
        public Rectangle GetDrawRectangle()
        {
            return new Rectangle((int)location.X, (int)location.Y,
                (int)(pawTexture.Width * this.scale.X), (int)(pawTexture.Height * this.scale.Y));
        }

        #region Sort balloons by Z
        /// <summary>
        /// Sorts a custom array list of balloons by their Z value.
        /// </summary>
        /// <param name="toSort">The list to sort.</param>
        /// <returns>The sorted list.</returns>
        public CustomArrayList<Balloon> SortBalloonsByZ(CustomArrayList<Balloon> toSort, Boolean asc)
        {
            // Bubble sort :]
            var sorted = false;
            if (toSort.Count() < 2) return toSort;
            while (!sorted)
            {
                Balloon previous = null;
                for (var i = 0; i < toSort.Count(); i++)
                {
                    Balloon current = toSort.ElementAt(i);

                    if (previous != null && current.z > previous.z)
                    {
                        toSort.AddAt(current, i - 1, true);
                        toSort.AddAt(previous, i, true);
                        break;
                    }


                    if (i + 1 == toSort.Count())
                    {
                        sorted = true;
                    }

                    previous = current;
                }
            }
            if (!asc) return toSort.Reverse();
            return toSort;
        }
        #endregion
    }
}
