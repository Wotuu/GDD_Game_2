using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAInputHandler.MouseInput;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Digging.Players;
using DiggingGame.SandBoard;
using DiggingGame.Particles.Emitter;
using DiggingGame.Managers;

namespace DiggingGame.Players
{
    public class Player : MouseClickListener
    {

        public Paw paw { get; set; }

        public int FriendlyBugsLeftKill = 3;
        public int EnemieBugsLeftKill = 20;

        public Player()
        {
            MouseManager.GetInstance().mouseClickedListeners += this.OnMouseClick;
            MouseManager.GetInstance().mouseReleasedListeners += this.OnMouseRelease;
            this.paw = new Paw(this);
        }

        public void Update()
        {
            this.paw.Update();
        }

        public void Draw(SpriteBatch sb)
        {
            this.paw.Draw(sb);
        }

        public void OnMouseClick(MouseEvent m_event)
        {
            if (StateManager.GetInstance().GetState() != StateManager.State.Running) return;
            foreach (SandTile tile in DiggingMainGame.GetInstance().board.Tiles)
            {
                if (tile.DrawRectangle.Intersects(new Rectangle(m_event.location.X, m_event.location.Y, 5, 5)))
                {
                    
                    
                    if (tile.DigStatus != SandTile.DigDepth.DugThrice)
                    {
                        new DigEmitter(tile);
                        tile.DigStatus = tile.DigStatus + 1;
                        tile.DigTexture = DiggingMainGame.GetInstance().board.DigTiles[(int)tile.DigStatus -1];
                        
                    }
                    
                    //bug.Destroy();
                }
            }
        }

        public void OnMouseRelease(MouseEvent m_event)
        {

        }
    }
}
