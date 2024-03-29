﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using XNAInterfaceComponents.Components;
using XNAInterfaceComponents.AbstractComponents;
using MainGame.Managers;
using BalloonPaintBucketGame;
using SquatBugsGame;
using DiggingGame;

namespace MainGame.Cards.UI
{
    public class RestartGamePanel : XNAPanel
    {
        public XNAButton toOverviewBtn { get; set; }
        public XNAButton restartGameBtn { get; set; }
        public GameResultCard card { get; set; }


        public static Texture2D OVERVIEW_GAME_BACKGROUND { get; set; }
        public static Texture2D OVERVIEW_GAME_BACKGROUND_HOVER { get; set; }

        public static Texture2D RESTART_GAME_BACKGROUND { get; set; }
        public static Texture2D RESTART_GAME_BACKGROUND_HOVER { get; set; }

        static RestartGamePanel()
        {
            OVERVIEW_GAME_BACKGROUND = Game1.GetInstance().Content.Load<Texture2D>
                ("UI/Interface/Cards/backtomap_btn");
            OVERVIEW_GAME_BACKGROUND_HOVER = Game1.GetInstance().Content.Load<Texture2D>
                ("UI/Interface/Cards/backtomap_btn_hover");

            RESTART_GAME_BACKGROUND = Game1.GetInstance().Content.Load<Texture2D>
                ("UI/Interface/Cards/tryagain_btn");
            RESTART_GAME_BACKGROUND_HOVER = Game1.GetInstance().Content.Load<Texture2D>
                ("UI/Interface/Cards/tryagain_btn_hover");
        }

        public RestartGamePanel(GameResultCard card)
            : base(null,
                card.GetDrawRectangle())
        {
            // On top of card.

            this.border = null;

            this.card = card;

            this.backgroundColor = Color.Transparent;

            this.toOverviewBtn = new XNAButton(this,
                new Rectangle((int)(61 * card.scale.X), (int)(580 * card.scale.Y),
                    (int)(635 * card.scale.X), (int)(145 * card.scale.Y)), "");
            this.toOverviewBtn.onClickListeners += this.OnMiniGameOverview;
            this.toOverviewBtn.border = null;
            this.toOverviewBtn.backgroundColor = Color.Transparent;
            // this.restartGameBtn.mouseOverColor = new Color(150, 0, 0, 150);
            this.toOverviewBtn.z = this.card.z - 0.01f;
            this.toOverviewBtn.backgroundTexture = OVERVIEW_GAME_BACKGROUND;
            this.toOverviewBtn.mouseoverBackgroundTexture = OVERVIEW_GAME_BACKGROUND_HOVER;

            this.restartGameBtn = new XNAButton(this,
                new Rectangle((int)(61 * card.scale.X), (int)(744 * card.scale.Y),
                    (int)(635 * card.scale.X), (int)(145 * card.scale.Y)), "");
            this.restartGameBtn.onClickListeners += this.OnGameRestart;
            this.restartGameBtn.border = null;
            this.restartGameBtn.backgroundColor = Color.Transparent;
            // this.restartGameBtn.mouseOverColor = new Color(150, 0, 0, 150);
            this.restartGameBtn.z = this.card.z - 0.01f;
            this.restartGameBtn.backgroundTexture = RESTART_GAME_BACKGROUND;
            this.restartGameBtn.mouseoverBackgroundTexture = RESTART_GAME_BACKGROUND_HOVER;
        }

        public void OnMiniGameOverview(XNAButton source)
        {
            card.shouldBeRemoved = true;

            Game1.GetInstance().BackToMiniGameOverview();


            this.Unload();
        }

        public void OnGameRestart(XNAButton source)
        {
            card.shouldBeRemoved = true;

            // Restart the games
            switch (StateManager.GetInstance().GetRunningGame())
            {
                case StateManager.RunningGame.BalloonPaintBucketGame:
                    BalloonPaintBucketMainGame.GetInstance().RestartGame();
                    break;
                case StateManager.RunningGame.SquatBugsGame:
                    SquatBugsMainGame.GetInstance().RestartGame();
                    break;
                case StateManager.RunningGame.DigGame:
                    DiggingMainGame.GetInstance().RestartGame();
                    break;
            }

            this.Unload();
        }

        public override void Unload()
        {
            base.Unload();

            this.restartGameBtn.onClickListeners -= this.OnGameRestart;
        }
    }
}
