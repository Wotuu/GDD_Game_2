using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SquatBugsGame.Managers;

namespace SquatBugsGame.Bugs
{
    class FriendlyBug : Bug
    {
        

        public FriendlyBug(Vector2 speed, int score, double changedirectiontimer, Vector2 location,BugColor color)
        {
            
            getBugTexture(color);
            this.Speed = speed;
            this.Score = score;
            this.NrOfFrames = 3;
            //this.RotationCorrection = -.5f;
            this.RotationCorrection = -1.7f;
            this.ChangeDirectionTimer = changedirectiontimer;
            this.scale = new Vector2(0.35f, 0.35f);
            this.location = location;
            this.EndLocation = BugManager.GetInstance().GetRandomLocation();
            this.FadeTime = 1500;
            this.FadeTimer = FadeTime;
        }

        public void getBugTexture(BugColor color)
        {
           this.bugcolor = color;
           switch (color){
               case BugColor.Pink :
                   BUG = SquatBugsMainGame.GetInstance().game.Content.Load<Texture2D>("Bugs/lieveheersb3_sprite");
                   
                   break;
               case BugColor.Blue:
                   BUG = SquatBugsMainGame.GetInstance().game.Content.Load<Texture2D>("Bugs/lieveheersb2_sprite");
                   break;
               case BugColor.Green:
                   BUG = SquatBugsMainGame.GetInstance().game.Content.Load<Texture2D>("Bugs/lieveheersb1_sprite");
                   break;
           }
        }

    }
}
