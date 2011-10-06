using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MiniGameOverview.Managers;
using MiniGameOverview.Players;
using MiniGameOverview.Map.Pathing;
using XNAInputLibrary.KeyboardInput;
using Microsoft.Xna.Framework.Input;
using XNAInputHandler.MouseInput;
using MiniGameOverview.Backgrounds;
using BalloonPaintBucketGame.Util;
using XNAInterfaceComponents.Components;
using MiniGameOverview.UI;
using XNAInterfaceComponents.Managers;

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

        public BackgroundMap backgroundMap { get; set; }
        public Game game { get; set; }
        public Player player { get; set; }
        public GameInfoDisplayPanel gameInfoPanel { get; set; }

        public void Initialize(Game game)
        {
            // We've already initialized!!
            if (this.player != null)
            {
                // But we need to re-initialise the info panel, as it is unloaded on game-load.
                // The GameInfoDisplayPanel needs a player, so please leave the 2 seperate calls in this function
                this.gameInfoPanel = new GameInfoDisplayPanel(0.8f);
                this.player.TeleportTo(this.player.moveTarget);
                return;
            }
            this.game = game;

            this.player = new Player();
            this.gameInfoPanel = new GameInfoDisplayPanel(0.8f);

            this.backgroundMap = new BackgroundMap();

            StateManager.GetInstance().SetState(StateManager.State.Running);
        }

        public void Update()
        {
            GameTimeManager.GetInstance().OnStartUpdate();
            KeyboardManager.GetInstance().Update(Keyboard.GetState());
            MouseManager.GetInstance().Update(this.game);
            // ComponentManager.GetInstance().Update();

            this.player.Update();
            this.backgroundMap.Update();
        }

        public void Draw(SpriteBatch sb)
        {
            if (DrawUtil.lineTexture == null)
                DrawUtil.lineTexture = DrawUtil.GetClearTexture2D(sb);
            if (PolygonCollision.Util.DrawUtil.lineTexture == null)
                PolygonCollision.Util.DrawUtil.lineTexture = PolygonCollision.Util.DrawUtil.GetClearTexture2D(sb);

            GameTimeManager.GetInstance().OnStartDraw();
            KeyboardManager.GetInstance().Update(Keyboard.GetState());
            MouseManager.GetInstance().Update(this.game);
            // ComponentManager.GetInstance().Draw(sb);

            this.player.Draw(sb);
            this.backgroundMap.Draw(sb);
        }

        /// <summary>
        /// Should be called when the interface should be hidden.
        /// </summary>
        public void OnHide()
        {
            foreach (PathItem item in this.backgroundMap.paths)
            {
                if (item.emitter != null)
                {
                    item.emitter.QuicklyDie(10);
                    item.emitter = null;
                }
            }

            StateManager.GetInstance().SetState(StateManager.State.Paused);
        }

        /// <summary>
        /// Should be called when the interface should be shown again.
        /// </summary>
        public void OnShow()
        {
            foreach (PathItem item in this.backgroundMap.paths)
            {
                // Intended
                item.isUnlocked = item.isUnlocked;
            }

            StateManager.GetInstance().SetState(StateManager.State.Running);
        }
    }
}
