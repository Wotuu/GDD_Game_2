using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuzzBattle.Shells;
using Microsoft.Xna.Framework.Graphics;

namespace BuzzBattle.Managers
{
    public class ShellManager
    {

        #region Singleton logic
        private static ShellManager instance;

        public static ShellManager GetInstance()
        {
            if (instance == null) instance = new ShellManager();
            return instance;
        }

        private ShellManager() { }
        #endregion

        public LinkedList<Shell> shells = new LinkedList<Shell>();
        
        /// <summary>
        /// Updates all the shells!
        /// </summary>
        public void UpdateShells()
        {
            foreach (Shell shell in this.shells)
            {
                shell.Update();
            }
        }

        /// <summary>
        /// Draws all the shells!
        /// </summary>
        /// <param name="sb"></param>
        public void DrawShells(SpriteBatch sb)
        {
            foreach (Shell shell in this.shells)
            {
                shell.Draw(sb);
            }
        }
    }
}
