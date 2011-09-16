using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XNAInputHandler.MouseInput;
using BalloonPaintBucketGame.Managers;
using BalloonPaintBucketGame.Balloons;
using Microsoft.Xna.Framework;

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
            for (int i = 0; i < BalloonManager.GetInstance().balloons.Count(); i++)
            {
                Balloon balloon = BalloonManager.GetInstance().balloons.ElementAt(i);
                if (balloon.polygon.CheckCollision(new Vector2(m_event.location.X, m_event.location.Y)))
                {
                    balloon.OnPlayerClick();
                    return;
                }
            }
        }

        public void OnMouseRelease(MouseEvent m_event)
        {

        }
    }
}
