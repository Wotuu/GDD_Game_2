﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework;
//using System.Diagnostics;
//using XNAInterfaceComponents.Components;
//using XNAInterfaceComponents.ChildComponents;
//using XNAInterfaceComponents.AbstractComponents;
//using MiniGameOverview.Map.Pathing;
//using MiniGameOverview.Managers;

//public delegate void OnGameStart(StateManager.SelectedGame game);

//namespace MiniGameOverview.UI
//{
//    public class GameInfoDisplayPanel : XNAPanel
//    {
//        public XNALabel gameNameLbl { get; set; }
//        public XNALabel gameDescriptionLbl { get; set; }

//        public XNAButton startGameBtn { get; set; }

//        public static SpriteFont GAME_NAME_FONT { get; set; }
//        public static SpriteFont GAME_DESCRIPTION_FONT { get; set; }

//        public StateManager.SelectedGame selectedGame { get; set; }
//        public OnGameStart onGameStartListeners { get; set; }

//        public String[] gameNames = new String[]{
//            "Main Menu",
//            "Balloon Popper",
//            "Sand Digger",
//            "Bugs Swatter",
//            "Buzz' Reclamation"
//        };

//        public String[] gameDescriptions = new String[]{
//            "Go back to the main menu.",

//            "Use your magic twig to pop the correct \n" +
//            "balloons at the right time!\n" +
//            "Spilling paint in a wrong bucket will\n" +
//            "decrease your progress with that bucket,\n" +
//            "so poke wisely. Beware of the black \n" +
//            "balloons, however...",

//            "No description yet.",

//            "No description yet.",

//            "No description yet."

//        };

//        public String[] startGameBtnText = new String[]{
//            "Main Menu",
//            "Start Game",
//            "Start Game",
//            "Start Game",
//            "Start Game"
//        };

//        static GameInfoDisplayPanel()
//        {
//            GAME_NAME_FONT = MiniGameOverviewMainGame.GetInstance().game.Content.Load<SpriteFont>("Fonts/MainMenu");
//            GAME_DESCRIPTION_FONT = MiniGameOverviewMainGame.GetInstance().game.Content.Load<SpriteFont>("Fonts/BigMenuTextField");
//        }

//        public GameInfoDisplayPanel(float z)
//            : base(null, GameInfoDisplayPanel.GetDrawRectangle(0))
//        {
//            this.backgroundTexture = MiniGameOverviewMainGame.GetInstance().game.Content.Load<Texture2D>("UI/Interface/uitleg_block");

//            this.z = z;
//            // this.backgroundColor = new Color(150, 0, 0, 150);
//            this.border = new Border(this, 4, Color.Black);

//            this.gameNameLbl = new XNALabel(this, new Rectangle(0, 10, this.bounds.Width, 30), "Gamename");
//            this.gameNameLbl.font = GAME_NAME_FONT;
//            this.gameNameLbl.textAlign = XNALabel.TextAlign.CENTER;


//            this.gameDescriptionLbl = new XNALabel(this,
//                new Rectangle(5, 30, this.bounds.Width - 10, this.bounds.Height - 40), gameDescriptions[0]);
//            this.gameDescriptionLbl.font = GAME_DESCRIPTION_FONT;

//            this.startGameBtn = new XNAButton(this,
//                new Rectangle(5, this.bounds.Height - 50, this.bounds.Width - 10, 40), "Start Game");
//            this.startGameBtn.font = GAME_DESCRIPTION_FONT;
//            this.startGameBtn.onClickListeners += this.OnStartGamePressed;

//            MiniGameOverviewMainGame.GetInstance().player.onPlayerReachedPathItemListeners += this.OnPlayerPathChanged;
//        }

//        /// <summary>
//        /// When the player's path has changed.
//        /// </summary>
//        /// <param name="item">The item to which the path was changed.</param>
//        public void OnPlayerPathChanged(PathItem item)
//        {
//            switch (item.game)
//            {
//                case StateManager.SelectedGame.MainMenu:
//                    this.gameNameLbl.text = gameNames[0];
//                    this.gameDescriptionLbl.text = gameDescriptions[0];
//                    this.startGameBtn.text = startGameBtnText[0];
//                    this.bounds = GameInfoDisplayPanel.GetDrawRectangle(0);
//                    break;

//                case StateManager.SelectedGame.BalloonPaintBucketGame:
//                    this.gameNameLbl.text = gameNames[1];
//                    this.gameDescriptionLbl.text = gameDescriptions[1];
//                    this.startGameBtn.text = startGameBtnText[1];
//                    this.bounds = GameInfoDisplayPanel.GetDrawRectangle(1);
//                    break;

//                case StateManager.SelectedGame.DigGame:
//                    this.gameNameLbl.text = gameNames[2];
//                    this.gameDescriptionLbl.text = gameDescriptions[2];
//                    this.startGameBtn.text = startGameBtnText[2];
//                    this.bounds = GameInfoDisplayPanel.GetDrawRectangle(2);
//                    break;

//                case StateManager.SelectedGame.SquatBugsGame:
//                    this.gameNameLbl.text = gameNames[3];
//                    this.gameDescriptionLbl.text = gameDescriptions[3];
//                    this.startGameBtn.text = startGameBtnText[3];
//                    this.bounds = GameInfoDisplayPanel.GetDrawRectangle(3);
//                    break;

//                case StateManager.SelectedGame.BuzzBattleGame:
//                    this.gameNameLbl.text = gameNames[4];
//                    this.gameDescriptionLbl.text = gameDescriptions[4];
//                    this.startGameBtn.text = startGameBtnText[4];
//                    this.bounds = GameInfoDisplayPanel.GetDrawRectangle(4);
//                    break;
//            }

//            selectedGame = item.game;
//        }

//        public void OnStartGamePressed(XNAButton source)
//        {
//            if (onGameStartListeners != null)
//                onGameStartListeners(this.selectedGame);
//            MiniGameOverviewMainGame.GetInstance().OnHide();

//            this.Unload();
//        }

//        /// <summary>
//        /// Gets the draw rectangle of this display panel according to a certain index.
//        /// </summary>
//        /// <param name="index">The index to use when calculating.</param>
//        /// <returns>The draw rectangle</returns>
//        public static Rectangle GetDrawRectangle(int index)
//        {
//            Rectangle rect = Rectangle.Empty;

//            if (index < 2)
//                rect = new Rectangle(MiniGameOverviewMainGame.GetInstance().game.GraphicsDevice.Viewport.Width - 400,
//                    30, 375, 300);
//            else
//                rect = new Rectangle(30, 30, 375, 300);

//            return rect;
//        }
//    }
//}
