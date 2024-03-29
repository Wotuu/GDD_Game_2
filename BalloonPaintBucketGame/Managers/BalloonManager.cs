﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomLists.Lists;
using BalloonPaintBucketGame.Balloons;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

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
        public int msBetweenBalloons = 3000;

        public double lastBalloonSpawnMS { get; set; }

        public int maxBlackBalloons = 4;
        public int balloonsSpawned { get; set; }

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
            balloonsSpawned++;
            Random random = new Random();

            // One in 4 chance a black balloon will spawn
            if (random.Next(4) == 0 && this.GetBlackBalloonCount() < this.maxBlackBalloons)
            {
                new Balloon(Balloon.BalloonColor.Black);
                return;
            }

            LinkedList<Balloon.BalloonColor> nonActiveBalloons = new LinkedList<Balloon.BalloonColor>();
            nonActiveBalloons.AddLast(Balloon.BalloonColor.Pink);
            nonActiveBalloons.AddLast(Balloon.BalloonColor.Pink);
            nonActiveBalloons.AddLast(Balloon.BalloonColor.Blue);
            nonActiveBalloons.AddLast(Balloon.BalloonColor.Blue);
            nonActiveBalloons.AddLast(Balloon.BalloonColor.Yellow);
            nonActiveBalloons.AddLast(Balloon.BalloonColor.Yellow);

            for (int i = 0; i < this.balloons.Count(); i++)
            {
                // Make a new node, to prevent index-based removal of the list items.
                nonActiveBalloons.Remove(this.balloons.ElementAt(i).color);
            }

            // Don't spawn if all colors are active
            if (nonActiveBalloons.Count == 0) return;

            // Random balloon of the colors we don't have
            int r = random.Next(nonActiveBalloons.Count);

            // Random color
            new Balloon(nonActiveBalloons.ElementAt(r));
        }

        /// <summary>
        /// Gets the black balloon count.
        /// </summary>
        public int GetBlackBalloonCount()
        {
            int count = 0;
            for (int i = 0; i < this.balloons.Count(); i++)
            {
                if (this.balloons.ElementAt(i).color == Balloon.BalloonColor.Black) count++;
            }
            return count;
        }
    }
}
