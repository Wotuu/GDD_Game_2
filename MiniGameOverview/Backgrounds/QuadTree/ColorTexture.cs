using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MiniGameOverview.Backgrounds.QuadTree
{
    public class CollisionTexture
    {
        public Quad quad { get; set; }
        public Texture2D texture { get; set; }
        public int coloredPixels { get; set; }

        private int[] textureData { get; set; }

        /// <summary>
        /// Returns the index of a certain point.
        /// </summary>
        /// <param name="p">The point.</param>
        /// <returns>The index.</returns>
        public int PointToIndex(int x, int y)
        {
            return x + y * this.texture.Width;
        }



        /// <summary>
        /// Updates the texture of this colortexture.
        /// </summary>
        private void UpdateTexture()
        {
            this.texture.SetData(textureData);
        }

        /// <summary>
        ///  Updates the collision of this texture, and update the texture as well.
        /// </summary>
        /// <param name="rect">The rectangle that is to be updated.</param>
        /// <param name="add">Whether to add or remove the rectangle.</param>
        public void ColorTexture(Rectangle rect)
        {
            // Update collisionmap data
            for (int i = rect.Left; i < rect.Right; i++)
            {
                for (int j = rect.Top; j < rect.Bottom; j++)
                {
                    this.textureData[PointToIndex(i, j)] =
                        this.quad.tree.map.GetTextureColorAtScreenLocation(
                        new Point(
                            i + this.quad.rectangle.X,
                            j + this.quad.rectangle.Y));
                }
            }

            UpdateTexture();
            this.coloredPixels = 0;
            foreach (int i in this.textureData)
            {
                if (i != 0) this.coloredPixels++;
            }
        }

        /// <summary>
        /// Converts a texture to a boolean array
        /// </summary>
        /// <returns>The boolean array of the texture of this quad</returns>
        public Boolean[] TextureToBoolean()
        {
            Boolean[] data = new Boolean[this.texture.Width * this.texture.Height];
            int[] intData = new int[this.texture.Width * this.texture.Height];
            this.texture.GetData(intData);
            for (int i = 0; i < intData.Length; i++)
            {
                //if (i == 0) Console.Out.WriteLine(intData[i] + "");
                // 0 = no collision, 1 = collision
                data[i] = (intData[i] != 0);
                //if (i == 0) Console.Out.WriteLine(data[i] + "");
            }
            return data;
        }

        public CollisionTexture(Quad quad, Texture2D texture)
        {
            this.quad = quad;
            this.texture = texture;

            this.textureData = new int[quad.rectangle.Width * quad.rectangle.Height];
        }
    }
}
