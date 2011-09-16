using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using CustomLists.Lists;
using PolygonCollision.Polygons;

namespace PolygonCollision.Managers
{
    public class PolygonManager
    {
        private static PolygonManager instance;
        public CustomArrayList<Polygon> polygons = new CustomArrayList<Polygon>();


        public static PolygonManager GetInstance()
        {
            if (instance == null) instance = new PolygonManager();
            return instance;
        }

        private PolygonManager() { }

        /// <summary>
        /// Draws all polygons on the screen.
        /// </summary>
        /// <param name="sb">The spritebatch to draw on.</param>
        public void DrawPolygons(SpriteBatch sb)
        {
            for (int i = 0; i < this.polygons.Count(); i++)
            {
                this.polygons.ElementAt(i).Draw(sb);
            }
        }

    }
}
