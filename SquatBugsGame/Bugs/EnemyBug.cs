using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquatBugsGame.Bugs
{
    class EnemyBug : Bug
    {
        public EnemyBug(Vector2 speed, int score,double changedirectiontimer,Vector2 location)
        {
            BUG = SquatBugsMainGame.GetInstance().game.Content.Load<Texture2D>("Bugs/bug");
            this.Speed = speed;
            this.Score = score;
            this.ChangeDirectionTimer = changedirectiontimer;
            this.scale = new Vector2(0.35f, 0.35f);
            this.location = location;
            this.color = Color.Black;
        }

      
    }
}
