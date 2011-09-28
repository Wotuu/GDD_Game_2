using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MiniGameOverview.Managers;
using MiniGameOverview.Players;

namespace MiniGameOverview
{
    public class MiniGameOverviewMainGame
    {

        #region Singleton logic
        private static MiniGameOverviewMainGame instance;

        public static MiniGameOverviewMainGame GetInstance()
        {
            if (instance == null) instance = new MiniGameOverviewMainGame();
            return instance;
        }

        private MiniGameOverviewMainGame() { }
        #endregion

        public Game game { get; set; }
        public Player player { get; set; }

        public void Initialize(Game game)
        {
            this.game = game;
            this.player = new Player();
        }


        public void Update()
        {
            GameTimeManager.GetInstance().OnStartUpdate();

            this.player.Update();
        }

        public void Draw(SpriteBatch sb)
        {
            GameTimeManager.GetInstance().OnStartDraw();

            this.player.Draw(sb);
        }
    }
}
