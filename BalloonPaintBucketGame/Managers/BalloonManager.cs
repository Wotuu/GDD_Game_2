using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomLists.Lists;
using BalloonPaintBucketGame.Balloons;
using Microsoft.Xna.Framework.Graphics;

namespace BalloonPaintBucketGame.Managers
{
    public class BalloonManager
    {

        #region Singleton logic
        private static BalloonManager instance;

        public static BalloonManager GetInstance()
        {
            if (instance == null) instance = new BalloonManager();
            return instance;
        }

        private BalloonManager() { }
        #endregion

        public CustomArrayList<Balloon> balloons = new CustomArrayList<Balloon>();
        public int msBetweenBalloons = 5000;

        public double lastBalloonSpawnMS { get; set; }

        public void DrawBalloons(SpriteBatch sb)
        {
            for (int i = 0; i < this.balloons.Count(); i++)
            {
                this.balloons.ElementAt(i).Draw(sb);
            }
        }

        public void UpdateBalloons()
        {
            double currTime = GameTimeManager.GetInstance().currentUpdateStartMS;
            if (currTime - lastBalloonSpawnMS > this.msBetweenBalloons)
            {
                this.SpawnNewBalloon();

                this.lastBalloonSpawnMS = currTime;
            }

            for (int i = 0; i < this.balloons.Count(); i++)
            {
                this.balloons.ElementAt(i).Update();
            }
        }

        /// <summary>
        /// Spawns a new balloon!
        /// </summary>
        public void SpawnNewBalloon()
        {
            // 0 = Pink, 1 = Blue, 2 = Yellow, 3 = Black
            Boolean[] activeColors = new Boolean[4];
            for (int i = 0; i < this.balloons.Count(); i++)
            {
                activeColors[(int)this.balloons.ElementAt(i).color] = true;
            }

            Random random = new Random();

            if (random.Next(5) == 1)
            {
                new Balloon(Balloon.BalloonColor.Black);
                return;
            }

            for (int i = 0; i < activeColors.Length; i++)
            {
                if (!activeColors[i] && i != 3 && !BalloonPaintBucketMainGame.GetInstance().paintBuckets[i].IsFilled())
                {
                    new Balloon((Balloon.BalloonColor)i);
                    return;
                }
            }
        }
    }
}
