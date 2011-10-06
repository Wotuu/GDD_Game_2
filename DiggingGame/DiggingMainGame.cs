using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DiggingGame.SandBoard;
using ParticleEngine;
using DiggingGame.Players;
using DiggingGame.Managers;

namespace DiggingGame
{
   
    public class DiggingMainGame
    {

         #region Singleton logic
        private static DiggingMainGame instance;

        public static DiggingMainGame GetInstance()
        {
            if (instance == null) instance = new DiggingMainGame();
            return instance;
        }
        #endregion

        private DiggingMainGame() { }

        public Game game { get; set; }
        public Board board;
        Player player;

        public void Initialize(Game game)
        {
            this.game = game;
            ParticleManager.DEFAULT_TEXTURE = this.game.Content.Load<Texture2D>("Particles/default");
            this.player = new Player();
            board = new Board(4, 3);

        }

        /// <summary>
        /// Restarts the game.
        /// </summary>
        public void RestartGame()
        {

            board = null;
            player = null;
            StateManager.GetInstance().SetState(StateManager.State.Running);

            this.Initialize(this.game);
        }

        /// <summary>
        /// Updates the game.
        /// </summary>
        public void Update()
        {
            GameTimeManager.GetInstance().OnStartUpdate();
            switch (StateManager.GetInstance().GetState())
            {
                case StateManager.State.Running:
                    {
                        player.Update();
                        board.Update();
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

        public void Draw(SpriteBatch sb)
        {
            GameTimeManager.GetInstance().OnStartDraw();
            player.Draw(sb);
            board.Draw(sb);
        }
    }
}
