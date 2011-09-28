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
        private int MaximumBugs = 30;
        private int MaximumEvilBugs = 8;
        private int MaximumFriendlyBugs = 22;
        private Random random = new Random();
        private Viewport viewport = SquatBugsMainGame.GetInstance().game.GraphicsDevice.Viewport;
        public void DrawBugs(SpriteBatch sb)
        {
            for (int i = 0; i < this.BugList.Count(); i++)
            {
                BugList[i].Draw(sb);
                //BugList[i].DrawDebug(sb, i);
                //Util.DrawClearRectangle(sb, BugList[i].drawRectangle, 1, Color.Red, 1);
                //Util.DrawClearRectangle(sb, BugList[i].locendrect, 1, Color.Red, 1);
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
                if (BugList[i].IsDead && BugList[i].FadeTimer <= 0)
                {
                    if (BugList[i] is FriendlyBug)
                    {
                        SquatBugsMainGame.GetInstance().player.FriendlyBugsLeftKill--;
                    }
                    else
                    {
                        SquatBugsMainGame.GetInstance().player.EnemieBugsLeftKill--;
                    }
                    BugList.Remove(BugList[i]);
                    // Remove bug

                    
                }
                try
                {
                    BugList[i].Update(random);
                }
                catch (Exception)
                { 
                }
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
                BugList.Add(new EnemyBug(GetRandomSpeed(), 12, GetRandomTime(), GetRandomLocation(true)));
            }

            for (int i = 0; i < MaximumFriendlyBugs - FriendlyBugsCount; i++)
            {
                BugList.Add(new FriendlyBug(GetRandomSpeed(), 12, GetRandomTime(), GetRandomLocation(true),(FriendlyBug.BugColor)random.Next(3)));
            }
        }

        public Vector2 GetRandomLocation(bool newbug = false)
        {
            if (!newbug)
            {
                return new Vector2(random.Next(200, viewport.Width - 200), random.Next(200, viewport.Height - 200));
            }
            else
            {
                int perc = random.Next(100);
                if (perc <= 25)
                {
                    return new Vector2(random.Next(-viewport.Width, 0), random.Next(-viewport.Height, 0));
                }
                else if(perc <= 50)
                {
                    return new Vector2(random.Next(viewport.Width, viewport.Width * 2), random.Next(viewport.Height, viewport.Height *2 ));
                }
                else if (perc <= 75)
                {
                    return new Vector2(random.Next(-viewport.Width, 0), random.Next(viewport.Height, viewport.Height * 2));
                }
                else
                {
                    return new Vector2(random.Next(viewport.Width, viewport.Width * 2), random.Next(-viewport.Height, 0));
                }
            }
        }

        public double GetRandomTime()
        {
            return random.Next(1000, 20000);
        }

        public Vector2 GetRandomSpeed()
        {
            return new Vector2(random.Next(-4, 4), random.Next(-4, 4));
        }
    }
}
