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

        public MainMenu()
            : base(null,
                new Rectangle(Game1.GetInstance().graphics.PreferredBackBufferWidth / 2 - 150,
                Game1.GetInstance().graphics.PreferredBackBufferHeight / 2 - 100,
                300, 200), MenuManager.PANEL_BACKGROUND)
        {
            this.balloonGameBtn = new XNAButton(this, new Rectangle(10, 10, this.bounds.Width - 20, 30),
                MenuManager.BUTTON_BACKGROUND, "Balloon Game");
            this.balloonGameBtn.onClickListeners += this.OnBallonGameBtnPressed;

            this.bugsGameBtn = new XNAButton(this, new Rectangle(10, 50, this.bounds.Width - 20, 30), 
                MenuManager.BUTTON_BACKGROUND, "Bugs Game");
            this.bugsGameBtn.onClickListeners += this.OnBugsGameBtnPressed;
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
    }
}
