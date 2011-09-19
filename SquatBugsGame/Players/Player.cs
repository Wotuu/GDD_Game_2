using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAInputHandler.MouseInput;
using SquatBugsGame.Bugs;
using SquatBugsGame.Managers;
using Microsoft.Xna.Framework;

namespace SquatBugsGame.Players
{
    public class Player : MouseClickListener
    {

        public Player()
        {
            MouseManager.GetInstance().mouseClickedListeners += this.OnMouseClick;
            MouseManager.GetInstance().mouseReleasedListeners += this.OnMouseRelease;
        }


        public void OnMouseClick(MouseEvent m_event)
        {
            foreach (Bug bug in BugManager.GetInstance().BugList)
            {
                if(bug.drawRectangle.Intersects(new Rectangle(m_event.location.X,m_event.location.Y,5,5)))
                {
                    bug.Destroy();
                }
            }
        }

        public void OnMouseRelease(MouseEvent m_event)
        {

        }
    }
}
