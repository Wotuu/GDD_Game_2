using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using ParticleEngine;
using ParticleEngine.Emitter;
using ParticleEngine.Particles;
using PolygonCollision.Managers;
using BuzzBattle.Util;
using XNAInterfaceComponents.ParentComponents;
using BuzzBattle.Managers;
using BuzzBattle.Shells;
using BuzzBattle.Players;
using BuzzBattle.UI;
using XNAInputHandler.MouseInput;

namespace BuzzBattle
{
    public class BuzzBattleMainGame
    {
        #region Singleton logic
        private static BuzzBattleMainGame instance;

        public static BuzzBattleMainGame GetInstance()
        {
            if (instance == null) instance = new BuzzBattleMainGame();
            return instance;
        }

        private BuzzBattleMainGame() { }
        #endregion

        public Game game { get; set; }

        public int collisionCheckMS = 25;
        public Double lastCollisionCheckMS { get; set; }

        public Texture2D background { get; set; }
        public Shell[] shells { get; set; }

        public Buzz buzz { get; set; }
        public BuzzTimer buzzTimer { get; set; }

        public void Initialize(Game game)
        {
            this.game = game;

            ParticleManager.DEFAULT_TEXTURE = this.game.Content.Load<Texture2D>("Particles/default");
            GameTimeManager.GetInstance().OnStartUpdate();

            this.background = this.game.Content.Load<Texture2D>("Backgrounds/lair");

            this.shells = new Shell[4];

            Viewport viewPort = this.game.GraphicsDevice.Viewport;

            this.shells[0] = new Shell(Shell.ShellColor.Blue);
            this.shells[0].location = new Vector2(
                (viewPort.Width / 8) - this.shells[0].GetDrawRectangle().Width / 2,
                viewPort.Height - this.shells[0].GetDrawRectangle().Height - 10);

            this.shells[1] = new Shell(Shell.ShellColor.Yellow);
            this.shells[1].location = new Vector2(
                (viewPort.Width / 8) * 3 - this.shells[1].GetDrawRectangle().Width / 2,
                viewPort.Height - this.shells[1].GetDrawRectangle().Height - 10);

            this.shells[2] = new Shell(Shell.ShellColor.Pink);
            this.shells[2].location = new Vector2(
                (viewPort.Width / 8) * 5 - this.shells[2].GetDrawRectangle().Width / 2,
                viewPort.Height - this.shells[2].GetDrawRectangle().Height - 10);

            this.shells[3] = new Shell(Shell.ShellColor.Green);
            this.shells[3].location = new Vector2(
                (viewPort.Width / 8) * 7 - this.shells[2].GetDrawRectangle().Width / 2,
                viewPort.Height - this.shells[2].GetDrawRectangle().Height - 10);

            XNAMessageDialog.CLIENT_WINDOW_WIDTH = game.GraphicsDevice.Viewport.Width;
            XNAMessageDialog.CLIENT_WINDOW_HEIGHT = game.GraphicsDevice.Viewport.Height;
            StateManager.GetInstance().SetState(StateManager.State.Running);

            this.buzz = new Buzz();
            this.buzzTimer = new BuzzTimer();
            /*
            this.game.Content.Load<Texture2D>("Balloons/balon1");
            this.game.Content.Load<Texture2D>("Balloons/balon2");
            this.game.Content.Load<Texture2D>("Balloons/balon3");
            this.game.Content.Load<Texture2D>("Balloons/balon4");
            */
        }

        /// <summary>
        /// Restarts the game.
        /// </summary>
        public void RestartGame()
        {

            ParticleManager.GetInstance().RemoveAllEmitters(10);
            StateManager.GetInstance().SetState(StateManager.State.Running);
            this.buzzTimer.Unload();

            this.Initialize(this.game);
        }

        public void Update()
        {
            GameTimeManager.GetInstance().OnStartUpdate();

            ShellManager.GetInstance().UpdateShells();

            // Always check for collision
            if (GameTimeManager.GetInstance().currentUpdateStartMS - this.lastCollisionCheckMS >
                this.collisionCheckMS)
            {
                this.lastCollisionCheckMS = GameTimeManager.GetInstance().currentUpdateStartMS;
            }

            switch (StateManager.GetInstance().GetState())
            {
                case StateManager.State.Running:
                    {
                        this.WinCheck();
                        this.buzz.Update();
                        break;
                    }
                case StateManager.State.Paused:
                    {

                        break;
                    }
                case StateManager.State.Loss:
                    {

                        break;
                    }
                case StateManager.State.Victory:
                    {

                        break;
                    }
            }
        }

        /// <summary>
        /// Draws the game.
        /// </summary>
        /// <param name="sb">The spritebatch to draw on.</param>
        public void Draw(SpriteBatch sb)
        {
            if (DrawUtil.lineTexture == null)
                DrawUtil.lineTexture = DrawUtil.GetClearTexture2D(sb);
            GameTimeManager.GetInstance().OnStartDraw();

            sb.Draw(this.background, new Rectangle(0, 0, this.game.GraphicsDevice.Viewport.Width,
            this.game.GraphicsDevice.Viewport.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);


            this.buzz.Draw(sb);

            ShellManager.GetInstance().DrawShells(sb);

            ParticleManager.GetInstance().Draw(sb);
            // PolygonManager.GetInstance().DrawPolygons(sb);
        }

        /// <summary>
        /// Checks if the current user has won.
        /// </summary>
        public void WinCheck()
        {
            if (this.buzzTimer.timeRanOut)
            {
                StateManager.GetInstance().SetState(StateManager.State.Loss);
            }
            else if (this.buzz.currentHealth <= 0)
            {
                StateManager.GetInstance().SetState(StateManager.State.Victory);
            }
        }

        public void Unload()
        {
            foreach (Shell shell in this.shells)
            {
                MouseManager.GetInstance().mouseClickedListeners -= shell.OnMouseClick;
                MouseManager.GetInstance().mouseReleasedListeners -= shell.OnMouseRelease;
                MouseManager.GetInstance().mouseMotionListeners -= shell.OnMouseMotion;
                MouseManager.GetInstance().mouseDragListeners -= shell.OnMouseDrag;
            }
            this.buzz = null;

            this.buzzTimer.Unload();
        }

        /// <summary>
        /// Gets a random shell, that is not disabled.
        /// </summary>
        /// <returns>The shell, or null if no such shell exists</returns>
        public Shell GetRandomNonDisabledShell()
        {
            Boolean hasEnabledShell = false;
            foreach (Shell shell in this.shells)
            {
                if (!shell.disabled) hasEnabledShell = true;
            }
            if (!hasEnabledShell) return null;

            Random r = new Random();
            int random = r.Next(0, 4);
            Shell randomShell = null;
            while ((randomShell = this.shells[random]).disabled) { random = r.Next(0, 4); }
            return randomShell;
        }
    }
}
