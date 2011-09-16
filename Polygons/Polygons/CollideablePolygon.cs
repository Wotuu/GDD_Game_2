using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PolygonCollision.Polygons;
using Microsoft.Xna.Framework;
using CustomLists.Lists;

namespace Polygons.Polygons
{
    public class CollideablePolygon : Polygon
    {
        public CollideablePolygon(CustomArrayList<Vector2> vertices)
            : base(vertices)
        {

        }

        public Boolean CheckCollision(Vector2 point)
        {
            return this.IsInside(point);
        }

        public Boolean CheckCollision(Polygon polygon)
        {
            for (int i = 0; i < this.vertices.Count(); i++)
            {
                if (polygon.IsInside(this.vertices.ElementAt(i))) return true;
            }
            return false;
        }
    }
}
