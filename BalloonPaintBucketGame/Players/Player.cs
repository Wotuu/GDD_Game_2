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
    public class Player
    {
        public Paw paw { get; set; }
        public float z { get; set; }

        public Player()
        {
            this.z = 0.8f;
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
    }
}
