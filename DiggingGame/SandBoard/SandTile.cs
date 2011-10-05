using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DiggingGame.SandBoard.DigObjects;
using DiggingGame.Managers;

namespace DiggingGame.SandBoard
{
    public class SandTile
    {
        public Rectangle DrawRectangle;
        public Texture2D DrawTexture;

        public Texture2D DigTexture;
        public Rectangle DigDrawRectangle;

        public DigObject DigObject;

        public float FadeTime = 2500;
        public double FadeTimer;

        Color DrawColor = new Color(255, 255, 255, 225);

        public DigDepth DigStatus = DigDepth.NoDig;
        public enum DigDepth
        {
            NoDig = 0,
            DugOnce = 1,
            DugTwice = 2,
            DugThrice = 3
        }
        public SandTile(Rectangle drawrectangle, Texture2D drawtexture, Rectangle digdrawrect)
        {
            FadeTimer = FadeTime;
            this.DrawRectangle = drawrectangle;
            this.DrawTexture = drawtexture;
            this.DigDrawRectangle = digdrawrect;
        }


        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this.DrawTexture, this.DrawRectangle, new Rectangle(0, 0, this.DrawTexture.Width, this.DrawTexture.Height), Color.White, 0f, Vector2.Zero, SpriteEffects.None, .9f);
            if (this.DigStatus != SandTile.DigDepth.NoDig)
            {
                sb.Draw(this.DigTexture, this.DigDrawRectangle, new Rectangle(0, 0, this.DigTexture.Width, this.DigTexture.Height), Color.White, 0f, Vector2.Zero, SpriteEffects.None, .8f);
            }

            //Draw digobject
            sb.Draw(this.DigObject.DrawTexture, this.DigObject.DrawRectangle, new Rectangle(0, 0, this.DigObject.DrawTexture.Width, this.DigObject.DrawTexture.Height), DrawColor, 0f, Vector2.Zero, SpriteEffects.None, .7f);
        }

        public void Update()
        {
            if (DigStatus == DigDepth.DugThrice)
            {
                AlphaCalcs(true);
            }
            if (this.DigObject.Init)
            {
                //Uitfaden
                AlphaCalcs(false);
            }
        }

        public void AlphaCalcs(Boolean FadeIn)
        {
            this.FadeTimer -= GameTimeManager.GetInstance().currentUpdateStartMS - GameTimeManager.GetInstance().previousUpdateStartMS;
            double AlphaValue;
            if (FadeIn)
            {
                AlphaValue = (float)255 - ((255 / FadeTime) * FadeTimer);
            }
            else
            {
                AlphaValue = (float)(255 / FadeTime) * FadeTimer;
            }
            DrawColor = new Color((int)AlphaValue, (int)AlphaValue, (int)AlphaValue, (int)AlphaValue);
            if (this.DigObject.Init && AlphaValue <= 0)
            {
                //No longer init , reset timer
                this.DigObject.Init = false;
                this.FadeTimer = FadeTime;

            }
        }

    }

}

