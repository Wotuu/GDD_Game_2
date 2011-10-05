using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using XNAInterfaceComponents.Components;
using XNAInterfaceComponents.ChildComponents;
using XNAInterfaceComponents.AbstractComponents;
using MiniGameOverview.Map.Pathing;
using MiniGameOverview.Managers;

public delegate void OnGameStart(StateManager.SelectedGame game);

namespace MiniGameOverview.UI
{
    public class GameInfoDisplayPanel : XNAPanel
    {
        public XNAButton startGameBtn { get; set; }

        public StateManager.SelectedGame selectedGame { get; set; }
        public OnGameStart onGameStartListeners { get; set; }

        public static Texture2D MAIN_MENU_BUTTON { get; set; }
        public static Texture2D MAIN_MENU_BUTTON_HOVER { get; set; }

        public static Texture2D START_GAME_BUTTON { get; set; }
        public static Texture2D START_GAME_BUTTON_HOVER { get; set; }

        public static Texture2D MAIN_MENU_DESCRIPTION { get; set; }
        public static Texture2D BALLOON_POPPER_DESCRIPTION { get; set; }
        public static Texture2D BUG_SQUAT_DESCRIPTION { get; set; }
        public static Texture2D DIG_DESCRIPTION { get; set; }
        public static Texture2D BUZZ_DESCRIPTION { get; set; }

        static GameInfoDisplayPanel()
        {

            MAIN_MENU_BUTTON = MiniGameOverviewMainGame.GetInstance().game.Content.Load<Texture2D>
                ("UI/Interface/Overview/startgame_btn");
            MAIN_MENU_BUTTON_HOVER = MiniGameOverviewMainGame.GetInstance().game.Content.Load<Texture2D>
                ("UI/Interface/Overview/startgame_btn_hover");
            START_GAME_BUTTON = MiniGameOverviewMainGame.GetInstance().game.Content.Load<Texture2D>
                ("UI/Interface/Overview/startgame_btn");
            START_GAME_BUTTON_HOVER = MiniGameOverviewMainGame.GetInstance().game.Content.Load<Texture2D>
                ("UI/Interface/Overview/startgame_btn_hover");

            MAIN_MENU_DESCRIPTION = MiniGameOverviewMainGame.GetInstance().game.Content.Load<Texture2D>
                ("UI/Interface/Overview/balloon_popper");
            BALLOON_POPPER_DESCRIPTION = MiniGameOverviewMainGame.GetInstance().game.Content.Load<Texture2D>
                ("UI/Interface/Overview/balloon_popper");
            BUG_SQUAT_DESCRIPTION = MiniGameOverviewMainGame.GetInstance().game.Content.Load<Texture2D>
                ("UI/Interface/Overview/bug_squat");
            DIG_DESCRIPTION = MiniGameOverviewMainGame.GetInstance().game.Content.Load<Texture2D>
                ("UI/Interface/Overview/dig");
            BUZZ_DESCRIPTION = MiniGameOverviewMainGame.GetInstance().game.Content.Load<Texture2D>
                ("UI/Interface/Overview/buzz");
        }

        public GameInfoDisplayPanel(float z)
            : base(null, GameInfoDisplayPanel.GetDrawRectangle(0))
        {
            this.z = z;
            // this.backgroundColor = new Color(150, 0, 0, 150);
            this.border = new Border(this, 4, Color.Black);

            this.backgroundTextureScale = Vector2.Zero;

            this.startGameBtn = new XNAButton(this,
                new Rectangle(7, this.bounds.Height - 75, this.bounds.Width - 15, 65), "");
            this.startGameBtn.onClickListeners += this.OnStartGamePressed;
            this.startGameBtn.backgroundTexture = MAIN_MENU_BUTTON;
            this.startGameBtn.mouseoverBackgroundTexture = MAIN_MENU_BUTTON_HOVER;
            this.startGameBtn.backgroundTextureScale = Vector2.Zero;

            MiniGameOverviewMainGame.GetInstance().player.onPlayerReachedPathItemListeners += this.OnPlayerPathChanged;
        }

        /// <summary>
        /// When the player's path has changed.
        /// </summary>
        /// <param name="item">The item to which the path was changed.</param>
        public void OnPlayerPathChanged(PathItem item)
        {
            switch (item.game)
            {
                case StateManager.SelectedGame.MainMenu:
                    this.backgroundTexture = MAIN_MENU_DESCRIPTION;
                    this.startGameBtn.backgroundTexture = MAIN_MENU_BUTTON;
                    this.startGameBtn.mouseoverBackgroundTexture = MAIN_MENU_BUTTON_HOVER;
                    this.bounds = GameInfoDisplayPanel.GetDrawRectangle(0);
                    break;

                case StateManager.SelectedGame.BalloonPaintBucketGame:
                    this.backgroundTexture = BALLOON_POPPER_DESCRIPTION;
                    this.startGameBtn.backgroundTexture = START_GAME_BUTTON;
                    this.startGameBtn.mouseoverBackgroundTexture = START_GAME_BUTTON_HOVER;
                    this.bounds = GameInfoDisplayPanel.GetDrawRectangle(1);
                    break;

                case StateManager.SelectedGame.DigGame:
                    this.backgroundTexture = DIG_DESCRIPTION;
                    this.startGameBtn.backgroundTexture = START_GAME_BUTTON;
                    this.startGameBtn.mouseoverBackgroundTexture = START_GAME_BUTTON_HOVER;
                    this.bounds = GameInfoDisplayPanel.GetDrawRectangle(2);
                    break;

                case StateManager.SelectedGame.SquatBugsGame:
                    this.backgroundTexture = BUG_SQUAT_DESCRIPTION;
                    this.startGameBtn.backgroundTexture = START_GAME_BUTTON;
                    this.startGameBtn.mouseoverBackgroundTexture = START_GAME_BUTTON_HOVER;
                    this.bounds = GameInfoDisplayPanel.GetDrawRectangle(3);
                    break;

                case StateManager.SelectedGame.BuzzBattleGame:
                    this.backgroundTexture = BUZZ_DESCRIPTION;
                    this.startGameBtn.backgroundTexture = START_GAME_BUTTON;
                    this.startGameBtn.mouseoverBackgroundTexture = START_GAME_BUTTON_HOVER;
                    this.bounds = GameInfoDisplayPanel.GetDrawRectangle(4);
                    break;
            }

            selectedGame = item.game;
        }

        public void OnStartGamePressed(XNAButton source)
        {
            if (onGameStartListeners != null)
                onGameStartListeners(this.selectedGame);
            MiniGameOverviewMainGame.GetInstance().OnHide();

            this.Unload();
        }

        /// <summary>
        /// Gets the draw rectangle of this display panel according to a certain index.
        /// </summary>
        /// <param name="index">The index to use when calculating.</param>
        /// <returns>The draw rectangle</returns>
        public static Rectangle GetDrawRectangle(int index)
        {
            Rectangle rect = Rectangle.Empty;
            Vector2 scale = new Vector2(0.35f, 0.35f);
            if (index < 2)
                rect = new Rectangle(MiniGameOverviewMainGame.GetInstance().game.GraphicsDevice.Viewport.Width -
                    (int)(MAIN_MENU_DESCRIPTION.Width * scale.X) - 20,
                    30, (int)(MAIN_MENU_DESCRIPTION.Width * scale.X),
                    (int)(MAIN_MENU_DESCRIPTION.Height * scale.Y));
            else
                rect = new Rectangle(30, 30, (int)(MAIN_MENU_DESCRIPTION.Width * scale.X),
                    (int)(MAIN_MENU_DESCRIPTION.Height * scale.Y));

            return rect;
        }
    }
}
