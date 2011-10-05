using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PolygonCollision.Polygons;
using MiniGameOverview.Map.Pathing;
using Microsoft.Xna.Framework.Graphics;

namespace MiniGameOverview.Backgrounds
{
    public class DrawableMapSection
    {
        public Polygon polygon { get; set; }
        public int maxPixelsToDraw { get; set; }
        public int currentPixelsDrawn { get; set; }

        public PathItem item { get; set; }

        public DrawableMapSection(PathItem item, Polygon polygon, int maxPixelsToDraw)
        {
            this.polygon = polygon;
            this.item = item;

            this.maxPixelsToDraw = maxPixelsToDraw;
        }

        public void Draw(SpriteBatch sb)
        {
            // this.polygon.Draw(sb);
        }
    }
}
