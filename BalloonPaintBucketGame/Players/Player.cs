using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XNAInputHandler.MouseInput;
using BalloonPaintBucketGame.Managers;
using BalloonPaintBucketGame.Balloons;
using Microsoft.Xna.Framework;
using CustomLists.Lists;

namespace BalloonPaintBucketGame.Players
{
    public class Player : MouseClickListener
    {

        public Player()
        {
            MouseManager.GetInstance().mouseClickedListeners += this.OnMouseClick;
            MouseManager.GetInstance().mouseReleasedListeners += this.OnMouseRelease;
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch sb)
        {

        }

        public void OnMouseClick(MouseEvent m_event)
        {
            CustomArrayList<Balloon> clickedBalloons = new CustomArrayList<Balloon>();
            for (int i = 0; i < BalloonManager.GetInstance().balloons.Count(); i++)
            {
                Balloon balloon = BalloonManager.GetInstance().balloons.ElementAt(i);
                if (balloon.polygon.CheckCollision(new Vector2(m_event.location.X, m_event.location.Y)))
                {
                    clickedBalloons.AddLast(balloon);
                }
            }
            if (clickedBalloons.Count() != 0)
                this.SortBalloonsByZ(clickedBalloons, false).GetFirst().OnPlayerClick();
        }

        public void OnMouseRelease(MouseEvent m_event)
        {

        }

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
    }
}
