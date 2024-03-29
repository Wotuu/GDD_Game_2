﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BalloonPaintBucketGame.Util;

namespace MiniGameOverview.Backgrounds.QuadTree
{
    public class Quad
    {
        public Quad[] children { get; set; }
        public Quad parent { get; set; }
        public Rectangle rectangle { get; set; }
        public QuadRoot tree { get; set; }
        public Boolean highlighted { get; set; }
        public Boolean isLeaf { get; set; }
        public CollisionTexture colorTexture { get; set; }

        public int imageX { get; set; }
        public int imageY { get; set; }

        /// <summary>
        /// Gets the depth of this node in the tree.
        /// </summary>
        /// <returns>The depth!</returns>
        public int GetDepth()
        {
            Quad quad = this;
            int depth = 0;
            while (quad.parent != null)
            {
                quad = quad.parent;
                depth++;
            }
            return depth;
        }

        public Quad[] Split()
        {
            Quad[] quads = new Quad[4];

            int leftWidth = this.rectangle.Width / 2;
            int rightWidth = this.rectangle.Width - leftWidth;
            int topHeight = this.rectangle.Height / 2;
            int bottomHeight = this.rectangle.Height - topHeight;
            // [*][ ]
            // [ ][ ]
            quads[0] = new Quad(this.tree, this,
                new Rectangle(this.rectangle.Left, this.rectangle.Top, leftWidth, topHeight));
            // [ ][*]
            // [ ][ ]
            quads[1] = new Quad(this.tree, this,
                new Rectangle(this.rectangle.Left + leftWidth, this.rectangle.Top, rightWidth, topHeight));
            // [ ][ ]
            // [*][ ]
            quads[2] = new Quad(this.tree, this,
                new Rectangle(this.rectangle.Left, this.rectangle.Top + topHeight, leftWidth, bottomHeight));
            // [ ][ ]
            // [ ][*]
            quads[3] = new Quad(this.tree, this,
                new Rectangle(this.rectangle.Left + rightWidth, this.rectangle.Top + topHeight, rightWidth, bottomHeight));

            this.children = quads;
            return quads;
        }

        public Rectangle GetDrawRectangle()
        {
            return new Rectangle((int)(this.rectangle.X - this.tree.drawOffset.X), (int)(this.rectangle.Y - this.tree.drawOffset.Y),
                this.rectangle.Width, this.rectangle.Height);
        }

        internal void Draw(SpriteBatch sb)
        {
            // If it ain't on the screen
            // if (!tree.collisionMap.windowSize.Intersects(this.GetDrawRectangle())) return;


            if (this.children == null)
            {
                // Draw the texture
                sb.Draw(this.colorTexture.texture, this.GetDrawRectangle(), null, this.tree.drawColor, 0f,
                    Vector2.Zero, SpriteEffects.None, this.tree.z - 0.0001f);
                // Draw the rectangle bounds
                if (this.tree.drawGridLines)
                    DrawUtil.DrawClearRectangle(sb, this.GetDrawRectangle(), this.tree.borderWidth, this.tree.borderColor, this.tree.z);

                // Draw the ????
                if (this.highlighted)
                {
                    DrawUtil.DrawLine(sb,
                        new Point(this.rectangle.Left, this.rectangle.Top),
                        new Point(this.rectangle.Right, this.rectangle.Bottom),
                        this.tree.highlightedColor,
                        this.tree.borderWidth, this.tree.z);
                    DrawUtil.DrawLine(sb,
                        new Point(this.rectangle.Right, this.rectangle.Top),
                        new Point(this.rectangle.Left, this.rectangle.Bottom),
                        this.tree.highlightedColor,
                        this.tree.borderWidth, this.tree.z);
                }
                return;
            }
            foreach (Quad quad in children)
            {
                quad.Draw(sb);
            }
        }

        /// <summary>
        /// Attempts to search a quad that contains the given point
        /// </summary>
        /// <param name="p">The point to search for</param>
        public Quad Search(Point p)
        {
            if (this.children == null)
            {
                if (this.rectangle.Contains(p)) return this;
                else return null;
            }
            foreach (Quad child in children)
            {
                if (child.rectangle.Contains(p))
                {
                    return child.Search(p);
                }
            }
            // Should never be called, but just in case ..
            return null;
        }



        public Quad(QuadRoot tree, Quad parent, Rectangle rectangle)
        {
            this.tree = tree;
            this.parent = parent;
            this.rectangle = rectangle;

            this.imageX = this.rectangle.X / this.rectangle.Width;
            this.imageY = this.rectangle.Y / this.rectangle.Height;

            // Console.Out.WriteLine("Creating quad with " + this.rectangle + " this.GetDepth() = " + this.GetDepth() + ", maxDepth = " + tree.depth);
            if (this.GetDepth() == tree.depth)
            {
                this.isLeaf = true;
                tree.leafList.AddLast(this);

                this.colorTexture = new CollisionTexture(this, new Texture2D(
                    MiniGameOverviewMainGame.GetInstance().game.GraphicsDevice,
                        this.rectangle.Width, this.rectangle.Height));
            }
        }
    }
}
