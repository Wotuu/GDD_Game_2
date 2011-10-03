using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PolygonCollision.Polygons;
using MiniGameOverview.Backgrounds;
using MiniGameOverview.Backgrounds.QuadTree;
using System.Diagnostics;
using MiniGameOverview.Managers;

namespace MiniGameOverview.Map.Pathing
{
    public class PathItem
    {
        public static Texture2D PATH_ITEM_TEXTURE { get; set; }
        public PathItem previous { get; set; }
        public PathItem next { get; set; }

        public Vector2 location { get; set; }
        public Boolean isUnlocked { get; set; }
        public Boolean isFullyColored { get; set; }

        public StateManager.SelectedGame game { get; set; }

        private DrawableMapSection _drawableMapSection { get; set; }
        public DrawableMapSection drawableMapSection
        {
            get
            {
                return _drawableMapSection;
            }
            set
            {
                if (value.polygon != null)
                    value.polygon.z = 0.89f;

                _drawableMapSection = value;
            }

        }


        static PathItem()
        {
            PATH_ITEM_TEXTURE = MiniGameOverviewMainGame.GetInstance().game.Content.Load<Texture2D>("UI/path_item");
        }

        public PathItem(StateManager.SelectedGame game, PathItem previous)
        {
            this.game = game;

            this.previous = previous;

            if (this.previous != null)
                this.previous.next = this;
        }

        /// <summary>
        /// Gets the draw rectangle of this path item.
        /// </summary>
        /// <returns>The rectangle that you should draw.</returns>
        public Rectangle GetDrawRectangle()
        {
            return new Rectangle(
                (int)(this.location.X - (PathItem.PATH_ITEM_TEXTURE.Width / 2f)),
                (int)(this.location.Y -
                (PathItem.PATH_ITEM_TEXTURE.Height / 2f)),
                PathItem.PATH_ITEM_TEXTURE.Width, PathItem.PATH_ITEM_TEXTURE.Height);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(PathItem.PATH_ITEM_TEXTURE, this.GetDrawRectangle(), null,
                Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.9f);

            if (this.drawableMapSection != null) this.drawableMapSection.Draw(sb);
        }

        /// <summary>
        /// Checks if this item is fully colored yet.
        /// </summary>
        public void CheckIfFullyColored()
        {
            int currentColoredPixelsCount = 0;
            foreach (Quad q in MiniGameOverviewMainGame.GetInstance().map.coloredMap.tree.leafList)
            {
                if (this.drawableMapSection.polygon.IsInside(q.rectangle.Center))
                {
                    currentColoredPixelsCount += q.colorTexture.coloredPixels;
                }
            }

            if (this.drawableMapSection.maxPixelsToDraw < currentColoredPixelsCount)
            {
                this.isFullyColored = true;
                Debug.WriteLine("A pathitem is fully colored! :D");
            }
        }
    }
}
