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
    public class IngameMenu : XNAPanel
    {
        public XNAButton resumeGameBtn { get; set; }
        public XNAButton exitGameBtn { get; set; }

        public IngameMenu()
            : base(null,
                new Rectangle(Game1.GetInstance().graphics.PreferredBackBufferWidth / 2 - 150,
                Game1.GetInstance().graphics.PreferredBackBufferHeight / 2 - 75,
                300, 150))
        {
            this.resumeGameBtn = new XNAButton(this, new Rectangle(10, 10, this.bounds.Width - 20, 30), "Resume Game");
            this.resumeGameBtn.onClickListeners += this.OnResumeGameBtnClicked;

            this.exitGameBtn = new XNAButton(this, new Rectangle(10, 50, this.bounds.Width - 20, 30), "Exit Game");
            this.exitGameBtn.onClickListeners += this.OnExitGameBtnClicked;
        }

        /// <summary>
        /// Resumes the game.
        /// </summary>
        /// <param name="button"></param>
        public void OnResumeGameBtnClicked(XNAButton button)
        {
            MenuManager.GetInstance().ShowMenu(MenuManager.Menu.NoMenu);
            StateManager.GetInstance().SetState(StateManager.State.Running);
        }

        /// <summary>
        /// Exits the game.
        /// </summary>
        /// <param name="source"></param>
        public void OnExitGameBtnClicked(XNAButton source)
        {
            Game1.GetInstance().Exit();
        }
    }
}
