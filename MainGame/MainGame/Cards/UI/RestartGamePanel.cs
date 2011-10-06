using System;
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
        public XNAButton restartGameBtn { get; set; }
        public GameResultCard card { get; set; }


        public static SpriteFont RESTART_GAME_FONT { get; set; }
        public static Texture2D RESTART_GAME_BACKGROUND { get; set; }
        public static Texture2D RESTART_GAME_BACKGROUND_HOVER { get; set; }

        static RestartGamePanel()
        {
            RESTART_GAME_FONT = Game1.GetInstance().Content.Load<SpriteFont>("Fonts/BigMenuTextField");
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

            this.restartGameBtn = new XNAButton(this,
                new Rectangle((int)(61 * card.scale.X), (int)(744 * card.scale.Y),
                    (int)(635 * card.scale.X), (int)(145 * card.scale.Y)), "");
            this.restartGameBtn.onClickListeners += this.OnGameRestart;
            this.restartGameBtn.border = null;
            this.restartGameBtn.backgroundColor = Color.Transparent;
            // this.restartGameBtn.mouseOverColor = new Color(150, 0, 0, 150);
            this.restartGameBtn.z = this.card.z - 0.01f;
            this.restartGameBtn.font = RESTART_GAME_FONT;
            this.restartGameBtn.backgroundTexture = RESTART_GAME_BACKGROUND;
            this.restartGameBtn.mouseoverBackgroundTexture = RESTART_GAME_BACKGROUND_HOVER;
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
