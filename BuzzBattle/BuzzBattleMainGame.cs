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

        public void Initialize(Game game)
        {
            this.game = game;

            ParticleManager.DEFAULT_TEXTURE = this.game.Content.Load<Texture2D>("Particles/default");

                this.background = this.game.Content.Load<Texture2D>("Backgrounds/lair");



            XNAMessageDialog.CLIENT_WINDOW_WIDTH = game.GraphicsDevice.Viewport.Width;
            XNAMessageDialog.CLIENT_WINDOW_HEIGHT = game.GraphicsDevice.Viewport.Height;
            StateManager.GetInstance().SetState(StateManager.State.Running);
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

            this.Initialize(this.game);
        }

        public void Update()
        {
            GameTimeManager.GetInstance().OnStartUpdate();

            // Always check for collision
            if (GameTimeManager.GetInstance().currentUpdateStartMS - this.lastCollisionCheckMS >
                this.collisionCheckMS)
            {
                this.CheckCollision();
                this.lastCollisionCheckMS = GameTimeManager.GetInstance().currentUpdateStartMS;
            }

            switch (StateManager.GetInstance().GetState())
            {
                case StateManager.State.Running:
                    {
                        this.WinCheck();
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

            ParticleManager.GetInstance().Draw(sb);
            // PolygonManager.GetInstance().DrawPolygons(sb);
        }

        /// <summary>
        /// Checks collision of the particles with the buckets.
        /// </summary>
        public void CheckCollision()
        {
        }

        /// <summary>
        /// Checks if the current user has won.
        /// </summary>
        public void WinCheck()
        {
            
        }
    }
}
