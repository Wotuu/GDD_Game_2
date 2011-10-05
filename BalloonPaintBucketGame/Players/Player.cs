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
using BalloonPaintBucketGame.Players.Life;

namespace BalloonPaintBucketGame.Players
{
    public class Player
    {
        public Paw paw { get; set; }
        public LifeDisplayPanel lifeDisplayPanel { get; set; }
        public float z { get; set; }

        public Player()
        {
            this.z = 0.8f;
            this.paw = new Paw(this);

            this.lifeDisplayPanel = new LifeDisplayPanel(3);
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

        /// <summary>
        /// Oh no! The player has lost a life!
        /// </summary>
        public void LostLife()
        {
            this.lifeDisplayPanel.lives.RemoveLast();

            if (this.lifeDisplayPanel.lives.Count == 0)
            {
                StateManager.GetInstance().SetState(StateManager.State.Loss);
            }
        }
    }
}
