using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MainGame.Managers;
using MainGame.Backgrounds.Birds;
using MainGame.Backgrounds.Birds.BannerBirds;
using System.Diagnostics;

namespace MainGame.UI.Credits
{
    public class CreditsPresentation
    {
        public int totalDuration = 15000;
        public int spawnInterval = 5000;

        public double creationTime { get; set; }
        public double lastSpawnMS { get; set; }

        public LinkedList<String[]> creditsStrings = new LinkedList<String[]>();

        public CreditsPresentation()
        {
            this.creationTime = GameTimeManager.GetInstance().currentUpdateStartMS;

            creditsStrings.AddLast(new String[] { "Programming", "Menno van Scheers", "Wouter Koppenol" });
            creditsStrings.AddLast(new String[] { "Design & Art", "Odette Jansen" });
        }

        public void Update()
        {
            double updateStart = GameTimeManager.GetInstance().currentUpdateStartMS;
            if (updateStart - this.lastSpawnMS > this.spawnInterval && this.creditsStrings.Count > 0)
            {
                this.lastSpawnMS = updateStart;

                BirdFlock flock = new BirdFlock(BirdFlock.FlyDirection.LeftToRight, 0);

                String[] texts = this.creditsStrings.First.Value;
                for (int i = 0; i < texts.Length; i++)
                {
                    new BannerBird(flock, i, texts[i]);
                }

                this.creditsStrings.RemoveFirst();
            }
            else if (updateStart - this.creationTime > this.totalDuration)
            {
                // Re-show the main menu
                MenuManager.GetInstance().GetCurrentMenu().visible = true;
            }
        }
    }
}
