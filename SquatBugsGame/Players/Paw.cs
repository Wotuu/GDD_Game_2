using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAInputHandler.MouseInput;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SquatBugsGame.Managers;
using SquatBugsGame.Bugs;
using SquatBugsGame.Players;
using SquatBugsGame;

namespace SquatBugsGame.Players
{
    public class Paw : MouseClickListener
    {
        public Player player { get; set; }
        public Texture2D pawTexture { get; set; }
        public Vector3 location { get; set; }

        public Vector2 IdleScale { get; set; }
        public Vector2 scale { get; set; }
        public Vector2 SquatScale { get; set; }

        public double SquatTimer { get; set; }
        public double SquatTime { get; set; }

        public MoveState state { get; set; }

        private MouseEvent mousePosition;

        public enum MoveState
        {
            MovingToBug,
            MovingFromBug,
            Still
        }

        public Paw(Player player)
        {
            MouseManager.GetInstance().mouseClickedListeners += this.OnMouseClick;
            MouseManager.GetInstance().mouseReleasedListeners += this.OnMouseRelease;

            MouseManager.GetInstance().mouseMotionListeners += this.OnMouseMotion;
            MouseManager.GetInstance().mouseDragListeners += this.OnMouseDrag;

            this.pawTexture = SquatBugsMainGame.GetInstance().game.Content.Load<Texture2D>
                ("Misc/vliegenmepper");
            this.player = player;
            this.location = new Vector3(2000, 2000, 0.001f);

            this.scale = new Vector2(0.5f, 0.5f);
            this.IdleScale = scale;
            this.SquatScale = new Vector2(0.2f, 0.4f);

            this.SquatTime = 100;
            this.SquatTimer = SquatTime;

            this.state = MoveState.Still;
        }

        public void OnMouseMotion(MouseEvent e)
        {
            if (state == MoveState.Still)
            {
                mousePosition = e;
                Rectangle drawRect = this.GetDrawRectangle();
                //this.location = new Vector3(e.location.X - drawRect.Width,
                //    SquatBugsMainGame.GetInstance().game.GraphicsDevice.Viewport.Height -
                //(drawRect.Height / 2),
                //    this.location.Z);
                this.location = new Vector3(e.location.X - (drawRect.Width - (100 * scale.X)),
                    e.location.Y - (drawRect.Height - (455 * scale.Y)), this.location.Z);
            }
        }

        public void OnMouseDrag(MouseEvent e)
        {

        }

        public void Update()
        {

            if (this.state == MoveState.MovingToBug)
            {
                //Scale the flyswatter to make it look like its hitting the bug
                double updatetime = GameTimeManager.GetInstance().currentUpdateStartMS - GameTimeManager.GetInstance().previousUpdateStartMS;
                SquatTimer -= updatetime;

                if (SquatTimer <= 0)
                {
                    this.SquatTimer = SquatTime;
                    this.state = MoveState.MovingFromBug;
                }
                float scaleXValue = (float)((IdleScale.X - SquatScale.X) / SquatTime) * (float)updatetime;
                float scaleYValue = (float)((IdleScale.Y - SquatScale.Y) / SquatTime) * (float)updatetime;
                this.scale = new Vector2(scale.X - scaleXValue, scale.Y - scaleYValue);

            }

            if (this.state == MoveState.MovingFromBug)
            {

                double updatetime = GameTimeManager.GetInstance().currentUpdateStartMS - GameTimeManager.GetInstance().previousUpdateStartMS;
                SquatTimer -= updatetime;
                if (SquatTimer <= 0)
                {
                    this.SquatTimer = SquatTime;
                    this.state = MoveState.Still;
                }

                float scaleXValue = (float)((IdleScale.X - SquatScale.X) / SquatTime) * (float)updatetime;
                float scaleYValue = (float)((IdleScale.Y - SquatScale.Y) / SquatTime) * (float)updatetime;
                this.scale = new Vector2(scale.X + scaleXValue, scale.Y + scaleYValue);

            }

            if (this.state == MoveState.Still)
            {
                scale = IdleScale;
            }

            //Make the scale move on center of squater

            Rectangle drawRect = this.GetDrawRectangle();
            if (mousePosition != null)
                this.location = new Vector3(mousePosition.location.X - (drawRect.Width - (100 * scale.X)),
                    mousePosition.location.Y - (drawRect.Height - (455 * scale.Y)), this.location.Z);

        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this.pawTexture, this.GetDrawRectangle(), null,
                Color.White, 0f, Vector2.Zero, SpriteEffects.None, location.Z);
        }

        public void OnMouseClick(MouseEvent m_event)
        {
            if (this.state == MoveState.Still)
                this.state = MoveState.MovingToBug;
        }

        public void OnMouseRelease(MouseEvent m_event)
        {

        }



        /// <summary>
        /// Gets the draw rectangle of this .. paw.
        /// </summary>
        /// <returns>The rectangle</returns>
        public Rectangle GetDrawRectangle()
        {
            return new Rectangle((int)location.X, (int)location.Y,
                (int)(pawTexture.Width * this.scale.X), (int)(pawTexture.Height * this.scale.Y));
        }



    }
}
