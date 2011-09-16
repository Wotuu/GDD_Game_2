using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CustomLists.Lists;
using Microsoft.Xna.Framework.Graphics;
using PolygonCollision.Util;
using PolygonCollision.Managers;

namespace PolygonCollision.Polygons
{
    public class Polygon
    {
        public CustomArrayList<Point> points { get; set; }

        public Polygon(CustomArrayList<Point> points)
        {
            this.points = points;
            PolygonManager.GetInstance().polygons.AddLast(this);
        }

        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < points.Count(); i++)
            {
                Point end = Point.Zero;
                if (i == points.Count() - 1) end = points.GetFirst();
                else end = points.ElementAt(i + 1);
                DrawUtil.DrawLine(sb, points.ElementAt(i), end, Color.Red, 1, 1f);
            }
        }

        /// <summary>
        /// Checks whether or not the point is inside this polygon.
        /// </summary>
        /// <param name="p">The point to check.</param>
        /// <returns>Yes or no.</returns>
        public Boolean IsInside(Point p)
        {
            
            int intersections = 0;
            for (int i = 0; i < points.Count(); i++)
            {
                Point start = points.ElementAt(i);
                Point end = Point.Zero;
                if (i == points.Count() - 1) end = points.GetFirst();
                else end = points.ElementAt(i + 1);

                if (this.LineIntersects(start, end, p, new Point(p.X + 100000, p.Y))) intersections++;
            }

            return intersections % 2 != 0;
            // return this.LineIntersects(new Point( 0, 0) , new Point( 1024, 768), p, new Point(p.X + 100000, p.Y));
        }

        /// <summary>
        /// Checks whether two lines intersect.
        /// </summary>
        /// <param name="line1Start">Start point of line 1.</param>
        /// <param name="line1End">End point of line 1.</param>
        /// <param name="line2Start">Start point of line 2.</param>
        /// <param name="line2End">End point of line 2.</param>
        /// <returns>Yes or no.</returns>
        private Boolean LineIntersects(Point line1Start, Point line1End, Point line2Start, Point line2End)
        {
            // http://www.java-gaming.org/index.php/topic,22144.msg187344.html#msg187344
            double denom = (line2End.Y - line2Start.Y) * (line1End.X - line1Start.X) -
                (line2End.X - line2Start.X) * (line1End.Y - line1Start.Y);
            if (denom == 0.0)
            { // Lines are parallel.
                return true;
            }
            double ua = ((line2End.X - line2Start.X) * (line1Start.Y - line2Start.Y) -
                (line2End.Y - line2Start.Y) * (line1Start.X - line2Start.X)) / denom;
            double ub = ((line1End.X - line1Start.X) * (line1Start.Y - line2Start.Y) -
                (line1End.Y - line1Start.Y) * (line1Start.X - line2Start.X)) / denom;
            if (ua >= 0.0f && ua <= 1.0f && ub >= 0.0f && ub <= 1.0f)
            {
                return true;
                // Get the intersection point.
                /*
                return new Point((int)(line1Start.X + ua * (line1End.X - line1Start.X)), 
                    (int)(line1Start.Y + ua * (line1End.Y - line1Start.Y)));*/
            }

            return false;
        }
    }
}
