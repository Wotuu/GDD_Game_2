using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MainGame.Backgrounds.Birds;
using Microsoft.Xna.Framework.Graphics;

namespace MainGame.Managers
{
    public class BirdManager
    {

        #region Singleton logic
        private static BirdManager instance;

        public static BirdManager GetInstance()
        {
            if (instance == null) instance = new BirdManager();
            return instance;
        }

        private BirdManager() { }
        #endregion

        public LinkedList<Bird> birds = new LinkedList<Bird>();

        public double lastBirdSpawnMS { get; set; }
        public int birdSpawnMS = 13000;

        public void UpdateBirds()
        {
            if (GameTimeManager.GetInstance().currentUpdateStartMS - this.lastBirdSpawnMS > birdSpawnMS)
            {
                this.lastBirdSpawnMS = GameTimeManager.GetInstance().currentUpdateStartMS;

                new BirdFlock(
                    (new Random().Next(300) % 2 == 0) ? BirdFlock.FlyDirection.LeftToRight : BirdFlock.FlyDirection.RightToLeft,
                    new Random().Next(2, 5));
            }
            for (int i = 0; i < this.birds.Count; i++)
            {
                this.birds.ElementAt(i).Update();
            }
        }

        public void DrawBirds(SpriteBatch sb)
        {
            for (int i = 0; i < this.birds.Count; i++)
            {
                this.birds.ElementAt(i).Draw(sb);
            }
        }
    }
}
