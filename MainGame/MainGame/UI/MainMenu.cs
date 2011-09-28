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

namespace MainGame.UI
{
    public class MainMenu : XNAPanel
    {
        public XNAButton balloonGameBtn { get; set; }
        public XNAButton bugsGameBtn { get; set; }
        public XNAButton gameOverviewBtn { get; set; }
        public XNAButton exitGameBtn { get; set; }

        public MainMenu()
            : base(null,
                new Rectangle(Game1.GetInstance().graphics.PreferredBackBufferWidth / 2 - 150,
                Game1.GetInstance().graphics.PreferredBackBufferHeight / 2 - 105,
                300, 210))
        {
            this.backgroundColor = Color.Transparent;
            this.border = null;

            this.balloonGameBtn = new XNAButton(this, new Rectangle(10, 10, this.bounds.Width - 20, 40), "Balloon Game");
            this.balloonGameBtn.border = null;
            this.balloonGameBtn.onClickListeners += this.OnBallonGameBtnPressed;
            this.balloonGameBtn.mouseoverBackgroundTexture = MenuManager.BUTTON_MOUSEOVER_BACKGROUND;
            this.balloonGameBtn.backgroundColor = Color.Transparent;
            this.balloonGameBtn.mouseOverColor = new Color(242, 221, 95);
            this.balloonGameBtn.font = MenuManager.MAIN_MENU_BUTTON_FONT;

            this.bugsGameBtn = new XNAButton(this, new Rectangle(10, 60, this.bounds.Width - 20, 40), "Bugs Game");
            this.bugsGameBtn.border = null;
            this.bugsGameBtn.onClickListeners += this.OnBugsGameBtnPressed;
            this.bugsGameBtn.mouseoverBackgroundTexture = MenuManager.BUTTON_MOUSEOVER_BACKGROUND;
            this.bugsGameBtn.backgroundColor = Color.Transparent;
            this.bugsGameBtn.mouseOverColor = new Color(137, 233, 172);
            this.bugsGameBtn.font = MenuManager.MAIN_MENU_BUTTON_FONT;

            this.gameOverviewBtn = new XNAButton(this, new Rectangle(10, 110, this.bounds.Width - 20, 40), "Game Overview");
            this.gameOverviewBtn.border = null;
            this.gameOverviewBtn.onClickListeners += this.OnGameOverviewBtnPressed;
            this.gameOverviewBtn.mouseoverBackgroundTexture = MenuManager.BUTTON_MOUSEOVER_BACKGROUND;
            this.gameOverviewBtn.backgroundColor = Color.Transparent;
            this.gameOverviewBtn.mouseOverColor = new Color(132, 184, 239);
            this.gameOverviewBtn.font = MenuManager.MAIN_MENU_BUTTON_FONT;

            this.exitGameBtn = new XNAButton(this, new Rectangle(10, 160, this.bounds.Width - 20, 40), "Exit Game");
            this.exitGameBtn.border = null;
            this.exitGameBtn.onClickListeners += this.OnExitGameBtnPressed;
            this.exitGameBtn.mouseoverBackgroundTexture = MenuManager.BUTTON_MOUSEOVER_BACKGROUND;
            this.exitGameBtn.backgroundColor = Color.Transparent;
            this.exitGameBtn.mouseOverColor = new Color(252, 161, 255);
            this.exitGameBtn.font = MenuManager.MAIN_MENU_BUTTON_FONT;
        }

        public void OnBallonGameBtnPressed(XNAButton source)
        {
            MenuManager.GetInstance().ShowMenu(MenuManager.Menu.NoMenu);
            StateManager.GetInstance().SetRunningGame(StateManager.RunningGame.BalloonPaintBucketGame);
            StateManager.GetInstance().SetState(StateManager.State.Running);
        }

        public void OnBugsGameBtnPressed(XNAButton source)
        {
            MenuManager.GetInstance().ShowMenu(MenuManager.Menu.NoMenu);
            StateManager.GetInstance().SetRunningGame(StateManager.RunningGame.SquatBugsGame);
            StateManager.GetInstance().SetState(StateManager.State.Running);
        }

        public void OnGameOverviewBtnPressed(XNAButton source)
        {
            MenuManager.GetInstance().ShowMenu(MenuManager.Menu.NoMenu);
            StateManager.GetInstance().SetRunningGame(StateManager.RunningGame.MiniGameOverview);
            StateManager.GetInstance().SetState(StateManager.State.Running);
        }

        public void OnExitGameBtnPressed(XNAButton source)
        {
            Game1.GetInstance().Exit();
        }
    }
}
