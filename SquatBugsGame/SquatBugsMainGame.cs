using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SquatBugsGame.Managers;
using SquatBugsGame.Players;

namespace SquatBugsGame
{
   public class SquatBugsMainGame
    {

        #region Singleton logic
        private static SquatBugsMainGame instance;

        public static SquatBugsMainGame GetInstance()
        {
            if (instance == null) instance = new SquatBugsMainGame();
            return instance;
        }
        #endregion

        private SquatBugsMainGame() { }

        public Game game { get; set; }
        public SpriteFont font;
        public Player player;

        public void Initialize(Game game)
        {
            this.game = game;
            font = game.Content.Load<SpriteFont>("Fonts/Arial");
            player = new Player();
        }

        /// <summary>
        /// Updates the game.
        /// </summary>
        public void Update()
        {
            GameTimeManager.GetInstance().OnStartUpdate();
            BugManager.GetInstance().UpdateBugs(); 
        }

        /// <summary>
        /// Draws the game.
        /// </summary>
        /// <param name="sb">The spritebatch to draw on.</param>
        public void Draw(SpriteBatch sb)
        {
            GameTimeManager.GetInstance().OnStartDraw();
            BugManager.GetInstance().DrawBugs(sb); 
        }
    }
}
