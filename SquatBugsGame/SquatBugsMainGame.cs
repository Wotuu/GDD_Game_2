using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SquatBugsGame.Managers;
using SquatBugsGame.Players;
using ParticleEngine;

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
        public Texture2D background { get; set; }
       

        public void Initialize(Game game)
        {
            this.game = game;
            font = game.Content.Load<SpriteFont>("Fonts/Arial");
            this.background = this.game.Content.Load<Texture2D>("Backgrounds/squatbugs");
            ParticleManager.DEFAULT_TEXTURE = this.game.Content.Load<Texture2D>("Particles/default");
            this.player = new Player();
        }

        /// <summary>
        /// Updates the game.
        /// </summary>
        public void Update()
        {
            GameTimeManager.GetInstance().OnStartUpdate();
            BugManager.GetInstance().UpdateBugs();
            ParticleManager.GetInstance().Update((float)GameTimeManager.GetInstance().time_step);
            this.player.Update();
        }

        /// <summary>
        /// Draws the game.
        /// </summary>
        /// <param name="sb">The spritebatch to draw on.</param>
        public void Draw(SpriteBatch sb)
        {
            if (Util.lineTexture == null)
            {
                Util.lineTexture = Util.GetClearTexture2D(sb);
            }
            sb.Draw(this.background, new Rectangle(0, 0, this.game.GraphicsDevice.Viewport.Width,
            this.game.GraphicsDevice.Viewport.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            GameTimeManager.GetInstance().OnStartDraw();
            ParticleManager.GetInstance().Draw(sb);
            BugManager.GetInstance().DrawBugs(sb);
            this.player.Draw(sb);
        }
    }
}
