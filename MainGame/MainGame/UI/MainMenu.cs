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
using MainGame.UI.Credits;
using Microsoft.Xna.Framework.Media;

namespace MainGame.UI
{
    public class MainMenu : XNAPanel
    {
        public XNAButton startGameBtn { get; set; }
        public XNAButton creditsBtn { get; set; }
        public XNAButton gameOverviewBtn { get; set; }
        public XNAButton kinectTestBtn { get; set; }
        public XNAButton exitGameBtn { get; set; }

        public CreditsPresentation creditsPresentation { get; set; }

        public MainMenu()
            : base(null,
                new Rectangle(Game1.GetInstance().graphics.PreferredBackBufferWidth / 2 - 150,
                Game1.GetInstance().graphics.PreferredBackBufferHeight / 2 - 105,
                300, 210))
        {
            this.backgroundColor = Color.Transparent;
            this.border = null;

            this.startGameBtn = new XNAButton(this, new Rectangle(10, 10, this.bounds.Width - 20, 40), "Start Game");
            this.startGameBtn.border = null;

            this.startGameBtn.onClickListeners += this.OnGameStartPressed;
            this.startGameBtn.mouseoverBackgroundTexture = MenuManager.BUTTON_MOUSEOVER_BACKGROUND;
            this.startGameBtn.backgroundColor = Color.Transparent;
            this.startGameBtn.mouseOverColor = new Color(242, 221, 95);
            this.startGameBtn.font = MenuManager.MAIN_MENU_BUTTON_FONT;

            this.creditsBtn = new XNAButton(this, new Rectangle(10, 60, this.bounds.Width - 20, 40), "Credits");
            this.creditsBtn.border = null;
            this.creditsBtn.onClickListeners += this.OnCreditsPressed;
            this.creditsBtn.mouseoverBackgroundTexture = MenuManager.BUTTON_MOUSEOVER_BACKGROUND;
            this.creditsBtn.backgroundColor = Color.Transparent;
            this.creditsBtn.mouseOverColor = new Color(137, 233, 172);
            this.creditsBtn.font = MenuManager.MAIN_MENU_BUTTON_FONT;


            this.kinectTestBtn = new XNAButton(this, new Rectangle(10, 110, this.bounds.Width - 20, 40), "Kinect Test");
            this.kinectTestBtn.border = null;

            this.kinectTestBtn.onClickListeners += this.OnKinectGameBtnPressed;
            this.kinectTestBtn.mouseoverBackgroundTexture = MenuManager.BUTTON_MOUSEOVER_BACKGROUND;
            this.kinectTestBtn.backgroundColor = Color.Transparent;
            this.kinectTestBtn.mouseOverColor = new Color(137, 233, 172);
            this.kinectTestBtn.font = MenuManager.MAIN_MENU_BUTTON_FONT;

            this.exitGameBtn = new XNAButton(this, new Rectangle(10, 160, this.bounds.Width - 20, 40), "Exit Game");
            this.exitGameBtn.border = null;
            this.exitGameBtn.onClickListeners += this.OnExitGameBtnPressed;
            this.exitGameBtn.mouseoverBackgroundTexture = MenuManager.BUTTON_MOUSEOVER_BACKGROUND;
            this.exitGameBtn.backgroundColor = Color.Transparent;
            this.exitGameBtn.mouseOverColor = new Color(252, 161, 255);
            this.exitGameBtn.font = MenuManager.MAIN_MENU_BUTTON_FONT;
        }

        public void OnGameStartPressed(XNAButton source)
        {
            MenuManager.GetInstance().ShowMenu(MenuManager.Menu.NoMenu);
            StateManager.GetInstance().SetRunningGame(StateManager.RunningGame.MiniGameOverview);
            StateManager.GetInstance().SetState(StateManager.State.Running);

            Game1.GetInstance().gameStartMoviePlayer = new SimpleMoviePlayer(
                Game1.GetInstance().Content.Load<Video>("Media/Video/gamestart"));
            Game1.GetInstance().gameStartMoviePlayer.videoPlayer.fadeOutAfterMS = 29000;
            Game1.GetInstance().gameStartMoviePlayer.videoPlayer.fadeOutDurationMS = 2000;


            /*
            if (Game1.GetInstance().gameStartMoviePanel == null)
            {
                Game1.GetInstance().gameStartMoviePanel = new GameStartMoviePanel();
            }*/
        }

        public override void Update()
        {
            if (this.creditsPresentation != null)
                this.creditsPresentation.Update();

            base.Update();
        }

        public void OnCreditsPressed(XNAButton source)
        {
            this.creditsPresentation = new CreditsPresentation();
            this.visible = false;

            /*
            StateManager.GetInstance().SetRunningGame(StateManager.RunningGame.MiniGameOverview);
            StateManager.GetInstance().SetState(StateManager.State.Running);*/
        }

        /*
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
        }*/

        public void OnKinectGameBtnPressed(XNAButton source)
        {
            MenuManager.GetInstance().ShowMenu(MenuManager.Menu.NoMenu);
            StateManager.GetInstance().SetRunningGame(StateManager.RunningGame.DigGame);
            //StateManager.GetInstance().SetRunningGame(StateManager.RunningGame.KinectGame);
            StateManager.GetInstance().SetState(StateManager.State.Running);
        }

        public void OnExitGameBtnPressed(XNAButton source)
        {
            Game1.GetInstance().Exit();
        }
    }
}
