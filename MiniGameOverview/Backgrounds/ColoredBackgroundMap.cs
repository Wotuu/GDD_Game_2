using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MiniGameOverview.Backgrounds.QuadTree;
using CustomLists.Lists;

namespace MiniGameOverview.Backgrounds
{
    public class ColoredBackgroundMap
    {
        public int[] textureData { get; set; }
        private Texture2D _texture { get; set; }
        public Texture2D texture
        {
            get
            {
                return _texture;

            }
            set
            {
                this.textureData = new int[value.Width * value.Height];
                this._texture = value;
                value.GetData(this.textureData);
            }
        }

        public Vector2 location { get; set; }

        public QuadRoot tree { get; set; }
        public BackgroundMap map { get; set; }


        public static Texture2D COLORED_MAP { get; set; }

        static ColoredBackgroundMap()
        {
            ColoredBackgroundMap.COLORED_MAP = MiniGameOverviewMainGame.GetInstance().game.Content.Load<Texture2D>("Map/map_kleur");
        }

        public ColoredBackgroundMap(BackgroundMap map)
        {
            this.map = map;

            this.texture = ColoredBackgroundMap.COLORED_MAP;

            this.tree = new QuadRoot(new Rectangle(0, 0,
                MiniGameOverviewMainGame.GetInstance().game.GraphicsDevice.Viewport.Width,
                MiniGameOverviewMainGame.GetInstance().game.GraphicsDevice.Viewport.Height), this);
            this.tree.CreateTree(4);
            this.tree.z = 0.88f;

            // this.tree.drawGridLines = true;
        }

        /// <summary>
        /// Gets the rectangle that you should use to draw this with.
        /// </summary>
        /// <returns>bla</returns>
        public Rectangle GetDrawRectangle()
        {
            return new Rectangle((int)(this.location.X - (this.texture.Width * this.map.scale.X)),
                (int)(this.location.Y - (this.texture.Height * this.map.scale.Y)),
                (int)(this.texture.Width * this.map.scale.X),
                (int)(this.texture.Height * this.map.scale.Y));
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch sb)
        {
            //sb.Draw(this.texture, this.GetDrawRectangle(), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.9f);

            this.tree.Draw(sb);
        }

        /// <summary>
        /// Gets texture data according to a point on the screen
        /// </summary>
        /// <param name="screenLocation">The location on the screen you want the colored data from.</param>
        /// <returns>The integer containing the color values</returns>
        public int GetTextureColorAtScreenLocation(Point screenLocation)
        {
            // Convert screen to texture location
            Point textureLocation = new Point((int)(screenLocation.X / this.map.scale.X),
                (int)(screenLocation.Y / this.map.scale.Y));
            // Return the data
            return this.textureData[(int)((textureLocation.Y * this.texture.Width) + textureLocation.X)];
        }

        /// <summary>
        /// Applies color to the map!
        /// </summary>
        /// <param name="location">The location to add color to.</param>
        /// <param name="radius">The radius to color.</param>
        public void ApplyColor(Vector2 location, int radius)
        {
            Rectangle rect = new Rectangle(
                (int)(location.X - radius), (int)(location.Y - radius),
                (int)(radius * 2), (int)(radius * 2));


            CustomArrayList<Quad> quadList = new CustomArrayList<Quad>();
            double checkInterval = 3.0;

            quadList.AddLast(this.tree.GetQuadByPoint(new Point(rect.Left, rect.Top)));
            // top line
            for (int i = 0; i < checkInterval; i++)
                quadList.AddLast(this.tree.GetQuadByPoint(new Point(rect.Left + (i * (int)(rect.Width / checkInterval)), rect.Top)));

            quadList.AddLast(this.tree.GetQuadByPoint(new Point(rect.Right, rect.Top)));

            // right line
            for (int i = 0; i < checkInterval; i++)
                quadList.AddLast(this.tree.GetQuadByPoint(new Point(rect.Right, rect.Top + (int)(i * (rect.Height / checkInterval)))));

            quadList.AddLast(this.tree.GetQuadByPoint(new Point(rect.Left, rect.Bottom)));

            // bottom line
            for (int i = 0; i < checkInterval; i++)
                quadList.AddLast(this.tree.GetQuadByPoint(new Point(rect.Left + (int)(i * (rect.Width / checkInterval)), rect.Bottom)));

            quadList.AddLast(this.tree.GetQuadByPoint(new Point(rect.Right, rect.Bottom)));

            // left line
            for (int i = 0; i < checkInterval; i++)
                quadList.AddLast(this.tree.GetQuadByPoint(new Point(rect.Left, rect.Top + (int)(i * (rect.Height / checkInterval)))));

            CustomArrayList<Quad> affectedQuads = new CustomArrayList<Quad>();

            for (int i = 0; i < quadList.Count(); i++)
            {
                Quad quad = quadList.ElementAt(i);
                if (quad != null && !affectedQuads.Contains(quad)) affectedQuads.AddLast(quad);
            }

            // Update each of the quads.
            for (int i = 0; i < affectedQuads.Count(); i++)
            {
                Quad q = affectedQuads.ElementAt(i);

                Rectangle updatedRect = Rectangle.Intersect(q.rectangle, rect);

                updatedRect.X = updatedRect.X - q.rectangle.X;
                updatedRect.Y = updatedRect.Y - q.rectangle.Y;
                q.colorTexture.ColorTexture(updatedRect);
            }
            // this.tree.
        }
    }
}
