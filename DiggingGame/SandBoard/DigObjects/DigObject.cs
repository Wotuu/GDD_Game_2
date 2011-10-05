using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DiggingGame.SandBoard.DigObjects
{

    public abstract class DigObject
    {
        public Texture2D DrawTexture;
        public Rectangle DrawRectangle;
        public Boolean Init = true;

        public DigObject(Rectangle drawrectangle, Texture2D drawtexture)
        {
            this.DrawRectangle = drawrectangle;
            this.DrawTexture = drawtexture;
        }

    }
}
