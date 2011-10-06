using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SquatBugsGame.Managers;
using SquatBugsGame.Players;
using ParticleEngine;
using XNAInterfaceComponents.ParentComponents;
using SquatBugs.Audio;
using XNAInputHandler.MouseInput;

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
        public Viewport viewport;
        public AudioManager AudioManager;

        public void Initialize(Game game)
        {
            this.game = game;
            font = game.Content.Load<SpriteFont>("Fonts/Arial");
            this.background = this.game.Content.Load<Texture2D>("Backgrounds/squatbugs");
            ParticleManager.DEFAULT_TEXTURE = this.game.Content.Load<Texture2D>("Particles/default");
            this.player = new Player();
            this.viewport = game.GraphicsDevice.Viewport;
            AudioManager = new AudioManager();
            AudioManager.PlayAmbientBackGroundMusic();
            StateManager.GetInstance().SetState(StateManager.State.Running);
        }

        /// <summary>
        /// Restarts the game.
        /// </summary>
        public void RestartGame()
        {

            ParticleManager.GetInstance().RemoveAllEmitters(10);
            StateManager.GetInstance().SetState(StateManager.State.Running);
            BugManager.GetInstance().BugList.Clear();

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
                        BugManager.GetInstance().UpdateBugs();
                        this.player.Update();
                        WinCheck();
                        
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
            
            
            if (Util.lineTexture == null)
            {
                Util.lineTexture = Util.GetClearTexture2D(sb);
            }
            sb.Draw(this.background, new Rectangle(0, 0, this.game.GraphicsDevice.Viewport.Width,
            this.game.GraphicsDevice.Viewport.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            GameTimeManager.GetInstance().OnStartDraw();
            BugManager.GetInstance().DrawBugs(sb);
            this.player.Draw(sb);
            
        }


        /// <summary>
        /// Checks if the current user has won.
        /// </summary>
        public void WinCheck()
        {
            if (player.FriendlyBugsLeftKill == 0) StateManager.GetInstance().SetState(StateManager.State.Loss);
            else if (player.EnemyBugsLeftKill <= 0) StateManager.GetInstance().SetState(StateManager.State.Victory);
        }


        /// <summary>
        /// Will clean up all remeaning resources of this instance eg: MouseListeners,Clear Lists
        /// </summary>
        public void CleanUp()
        {
            //Remove MouseListeners
            MouseManager.GetInstance().mouseClickedListeners -= this.player.paw.OnMouseClick;
            MouseManager.GetInstance().mouseReleasedListeners -= this.player.paw.OnMouseRelease;

            MouseManager.GetInstance().mouseMotionListeners -= this.player.paw.OnMouseMotion;
            MouseManager.GetInstance().mouseDragListeners -= this.player.paw.OnMouseDrag;

            //Clear Bugs
            BugManager.GetInstance().BugList.Clear();
        }
    }
}
