using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SquatBugsGame.Managers;

namespace SquatBugsGame.Bugs
{
    class EnemyBug : Bug
    {
        public EnemyBug(Vector2 speed, int score,double changedirectiontimer,Vector2 location)
        {
            BUG = SquatBugsMainGame.GetInstance().game.Content.Load<Texture2D>("Bugs/insect");
            this.Speed = speed;
            this.Score = score;
            this.NrOfFrames = 4;
            this.RotationCorrection = 1.5f;
            this.ChangeDirectionTimer = changedirectiontimer;
            this.scale = new Vector2(0.35f, 0.35f);
            this.location = location;
            this.EndLocation = BugManager.GetInstance().GetRandomLocation();
            this.FadeTime = 1500;
            this.FadeTimer = FadeTime;
            this.bugcolor = BugColor.Black;
        }

      
    }
}
