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
        public CustomArrayList<Vector2> vertices { get; set; }

        /// <summary>
        /// The Z used for drawing.
        /// </summary>
        public float z { get; set; }
        public Vector2 scale { get; set; }
        public Vector2 offset { get; set; }

        public Polygon(CustomArrayList<Vector2> vertices)
        {
            this.vertices = vertices;
            PolygonManager.GetInstance().polygons.AddLast(this);

            this.offset = Vector2.Zero;
            this.scale = new Vector2(1f, 1f);

            this.z = 0.9f;
        }

        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < vertices.Count(); i++)
            {
                Vector2 start = vertices.ElementAt(i) * this.scale;
                Vector2 end = Vector2.Zero;
                if (i == vertices.Count() - 1) end = vertices.GetFirst();
                else end = vertices.ElementAt(i + 1);

                end *= this.scale;

                start += this.offset;
                end += this.offset;

                DrawUtil.DrawLine(sb, start, end, Color.Red, 1, this.z);
            }
        }

        /// <summary>
        /// Checks whether or not the point is inside this polygon.
        /// </summary>
        /// <param name="p">The point to check.</param>
        /// <returns>Yes or no.</returns>
        public Boolean IsInside(Point p)
        {
            return this.IsInside(new Vector2(p.X, p.Y));
        }

        /// <summary>
        /// Checks whether or not the point is inside this polygon.
        /// </summary>
        /// <param name="p">The point to check.</param>
        /// <returns>Yes or no.</returns>
        public Boolean IsInside(Vector2 p)
        {

            int intersections = 0;
            for (int i = 0; i < vertices.Count(); i++)
            {
                Vector2 start = vertices.ElementAt(i) * this.scale;
                Vector2 end = Vector2.Zero;
                if (i == vertices.Count() - 1) end = vertices.GetFirst();
                else end = vertices.ElementAt(i + 1);


                end *= this.scale;

                start += this.offset;
                end += this.offset;

                if (this.LineIntersects(start, end, p, new Vector2(p.X + 100000, p.Y))) intersections++;
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
        private Boolean LineIntersects(Vector2 line1Start, Vector2 line1End, Vector2 line2Start, Vector2 line2End)
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

        /// <summary>
        /// Unloads and destroys this polygon.
        /// </summary>
        public void Destroy()
        {
            this.vertices.Clear();
            PolygonManager.GetInstance().polygons.Remove(this);
        }
    }
}
