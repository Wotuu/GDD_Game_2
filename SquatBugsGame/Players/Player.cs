using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAInputHandler.MouseInput;
using SquatBugsGame.Bugs;
using SquatBugsGame.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SquatBugsGame.Players;
using SquatBugsGame.Players.Life;

namespace SquatBugsGame.Players
{
    public class Player : MouseClickListener
    {

        public Paw paw { get; set; }

        public LifeDisplayPanel lifeDisplayPanel { get; set; }

        public int FriendlyBugsLeftKill = 3;
        public int EnemyBugsLeftKill = 20;

        public Player()
        {
            MouseManager.GetInstance().mouseClickedListeners += this.OnMouseClick;
            MouseManager.GetInstance().mouseReleasedListeners += this.OnMouseRelease;
            this.paw = new Paw(this);

            this.lifeDisplayPanel = new LifeDisplayPanel(3, EnemyBugsLeftKill);
        }

        public void Update()
        {
            this.paw.Update();
        }

        public void Draw(SpriteBatch sb)
        {
            this.paw.Draw(sb);
            this.lifeDisplayPanel.Draw(sb);
        }

        public void OnMouseClick(MouseEvent m_event)
        {
            if (StateManager.GetInstance().GetState() == StateManager.State.Running)
            {
                SquatBugsMainGame.GetInstance().AudioManager.PlaySwatSound();
            }
            
            foreach (Bug bug in BugManager.GetInstance().BugList)
            {
                if (bug.GetCollisionRectangle().Intersects(new Rectangle(m_event.location.X, m_event.location.Y, 5, 5)))
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
