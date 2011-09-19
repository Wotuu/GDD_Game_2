using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SquatBugsGame.Bugs;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SquatBugsGame.Managers
{
    class BugManager
    {
        #region Singleton logic
        private static BugManager instance;

        public static BugManager GetInstance()
        {
            if (instance == null) instance = new BugManager();
            return instance;
        }

        private BugManager() { }
        #endregion

        public List<Bug> BugList = new List<Bug>();
        private int MaximumBugs = 20;
        private int MaximumEvilBugs = 10;
        private int MaximumFriendlyBugs = 10;
        private Random random = new Random();
        private Viewport viewport = SquatBugsMainGame.GetInstance().game.GraphicsDevice.Viewport;
        public void DrawBugs(SpriteBatch sb)
        {
            for (int i = 0; i < this.BugList.Count(); i++)
            {
                BugList[i].Draw(sb);
                BugList[i].DrawDebug(sb, i);
            }
        }

        public void UpdateBugs()
        {
            if (BugList.Count < MaximumBugs)
            {
                this.SpawnBug();
            }
            for (int i = 0; i < this.BugList.Count(); i++)
            {
                if (BugList[i].IsDead)
                {
                    BugList.Remove(BugList[i]);
                    i--;
                }
                BugList[i].Update(random);
            }

        }

        /// <summary>
        /// Counts how many bugs are on the screen, and of what type they are.
        /// Accordingly adding the correct bugs.
        /// </summary>
        public void SpawnBug()
        {
            int EvilBugsCount = 0;
            int FriendlyBugsCount = 0;

            foreach (Bug bug in BugList)
            {
                if (bug is EnemyBug)
                {
                    EvilBugsCount++;
                }
                else
                {
                    FriendlyBugsCount++;
                }
            }



            //Spawnen
            for (int i = 0; i < MaximumEvilBugs - EvilBugsCount; i++)
            {
                BugList.Add(new EnemyBug(new Vector2(random.Next(-2,2),random.Next(-2,2)),12,random.Next(1000,20000),new Vector2(random.Next(0,viewport.Width),random.Next(0,viewport.Height))));
            }

            for (int i = 0; i < MaximumFriendlyBugs - FriendlyBugsCount; i++)
            {
                BugList.Add(new FriendlyBug(new Vector2(random.Next(-2, 2) , random.Next(-2, 2) ), 12, random.Next(1000, 20000), new Vector2(random.Next(0, viewport.Width), random.Next(0, viewport.Height))));
            }
        }
    }
}
