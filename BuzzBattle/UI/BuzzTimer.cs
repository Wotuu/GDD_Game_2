using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using XNAInterfaceComponents.Components;
using XNAInterfaceComponents.ChildComponents;
using BuzzBattle.Managers;

namespace BuzzBattle.UI
{
    public class BuzzTimer : XNAPanel
    {
        public XNALabel label { get; set; }
        public double creationTimeMS { get; set; }

        public double timeTillDeathMS = 300000;
        public Boolean timeRanOut { get; set; }

        public BuzzTimer()
            : base(null, new Rectangle(
                0, 100,
                BuzzBattleMainGame.GetInstance().game.GraphicsDevice.Viewport.Width, 40))
        {
            this.border = null;
            this.backgroundColor = Color.Transparent;

            this.creationTimeMS = GameTimeManager.GetInstance().currentUpdateStartMS;
            this.label = new XNALabel(this, new Rectangle(0, 0, this.bounds.Width, 40), "");
            this.label.border = null;
            this.label.backgroundColor = Color.Transparent;
            this.label.font = BuzzBattleMainGame.GetInstance().game.Content.Load<SpriteFont>("Fonts/BigMenuTextField");
            this.label.textAlign = XNALabel.TextAlign.CENTER;
        }

        public override void Update()
        {
            base.Update();

            this.UpdateTimer();
        }

        /// <summary>
        /// Updates the timer!
        /// </summary>
        public void UpdateTimer()
        {
            this.label.text = "Time left: " + this.MSToTimeStamp(
               (this.creationTimeMS + this.timeTillDeathMS) - GameTimeManager.GetInstance().currentUpdateStartMS);
            this.timeRanOut = ((this.creationTimeMS + this.timeTillDeathMS) - GameTimeManager.GetInstance().currentUpdateStartMS) < 0;
        }

        /// <summary>
        /// Converts MS to a timestamp.
        /// </summary>
        /// <param name="ms">The MS you want converted to a timestamp</param>
        /// <returns>The String</returns>
        public String MSToTimeStamp(double ms)
        {
            int minutes = (int)(ms / (double)(1000 * 60));
            ms -= (minutes * 1000 * 60);

            int seconds = (int)(ms / 1000.0);

            return minutes + ":" + ((seconds > 9) ? "" + seconds : "0" + seconds);
        }
    }
}
