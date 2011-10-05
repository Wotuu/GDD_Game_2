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
using Microsoft.Xna.Framework.Input;

namespace BalloonPaintBucketGame.Players
{
    public class Paw : MouseClickListener
    {
        public Player player { get; set; }
        public Texture2D pawTexture { get; set; }
        public Vector3 location { get; set; }
        public Vector2 scale { get; set; }

        public float moveSpeed { get; set; }
        //public float yRestLocation { get; set; }

        public Point clickedLocation { get; set; }
        /*
         * public MoveState state { get; set; }
        public enum MoveState
        {
            MovingToBalloon,
            MovingFromBalloon,
            Still
        }*/

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

            this.scale = new Vector2(0.7f, 0.7f);

            /*
            this.yRestLocation = BalloonPaintBucketMainGame.GetInstance().game.GraphicsDevice.Viewport.Height -
                (this.GetDrawRectangle().Height / 2);*/
            // this.state = MoveState.Still;

            this.moveSpeed = 20f;
        }

        /// <summary>
        /// Moves the paw to the correct location, given the mouse location.
        /// </summary>
        /// <param name="mouseLocation">The mouse location.</param>
        private void MovePaw(Point mouseLocation)
        {
            if (StateManager.GetInstance().GetState() == StateManager.State.Running /*&& this.state == MoveState.Still*/)
            {
                Rectangle drawRect = this.GetDrawRectangle();
                this.location = new Vector3(mouseLocation.X - drawRect.Width, mouseLocation.Y, this.location.Z);
            }
        }

        public void OnMouseMotion(MouseEvent e)
        {
            this.MovePaw(e.location);
        }

        public void OnMouseDrag(MouseEvent e)
        {
            this.MovePaw(e.location);
        }

        /// <summary>
        /// Updates the paw.
        /// </summary>
        public void Update()
        {
            /*
            switch (this.state)
            {
                case MoveState.MovingToBalloon:
                    this.location = new Vector3(this.location.X, this.location.Y -
                        (float)(moveSpeed * GameTimeManager.GetInstance().time_step), this.location.Z);

                    if (this.location.Y < this.clickedLocation.Y)
                    {
                        this.PerformSting(this.clickedLocation);
                        this.state = MoveState.MovingFromBalloon;
                    }
                    break;

                case MoveState.MovingFromBalloon:
                    this.location = new Vector3(this.location.X, this.location.Y +
                        (float)(moveSpeed * GameTimeManager.GetInstance().time_step), this.location.Z);

                    if (this.location.Y > this.yRestLocation)
                    {
                        this.state = MoveState.Still;
                        this.location = new Vector3(this.location.X, this.yRestLocation, this.location.Z);
                        this.MovePaw(new Point(Mouse.GetState().X, Mouse.GetState().Y));
                    }
                    break;
            }*/
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this.pawTexture, this.GetDrawRectangle(), null,
                Color.White, 0f, Vector2.Zero, SpriteEffects.None, location.Z);
        }

        public void OnMouseClick(MouseEvent m_event)
        {
            this.PerformSting(m_event.location);
            /*
            if (this.state == MoveState.Still)
            {
                this.state = MoveState.MovingToBalloon;
                this.clickedLocation = m_event.location;
            }*/
        }

        public void OnMouseRelease(MouseEvent m_event)
        {

        }

        /// <summary>
        /// Performs a sting on the given location, popping balloons or not.
        /// </summary>
        /// <param name="location">The location to sting at.</param>
        public void PerformSting(Point location)
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
