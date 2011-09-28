using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MainGame.Backgrounds
{
    public class MainGameBackground
    {
        public MainGameBackgroundItem[] clouds = new MainGameBackgroundItem[3];
        public MainGameBackgroundItem[] bushes = new MainGameBackgroundItem[7];

        public Vector2[] bushLocations = new Vector2[]{
            new Vector2(200, 1080),
            new Vector2(321, 956),
            new Vector2(601, 1050),
            new Vector2(761, 1013),
            new Vector2(1065, 1073),
            new Vector2(1300, 976),
            new Vector2(1678, 1044)
        };

        public float[] bushZLocations = new float[]{
            0.95f,
            0.99f,
            0.96f,
            0.98f,
            0.97f,
            0.98f,
            0.96f
        };

        public MainGameBackground()
        {
            int cloudWidthStepping = Game1.GetInstance().graphics.PreferredBackBufferWidth / this.clouds.Length;
            int bushesWidthStepping = Game1.GetInstance().graphics.PreferredBackBufferWidth / this.bushes.Length;

            Vector2 screenScale = new Vector2(Game1.GetInstance().graphics.PreferredBackBufferWidth / 1920f,
                Game1.GetInstance().graphics.PreferredBackBufferHeight / 1080f);

            for (int i = 0; i < this.clouds.Length; i++)
            {
                this.clouds[i] = new MainGameBackgroundItem(new Vector3((i * cloudWidthStepping) + 100, 0,
                    0.9f),
                    MainGameBackgroundItem.BackgroundType.Cloud, i + 1);
            }
            for (int i = 0; i < this.bushes.Length; i++)
            {
                this.bushes[i] = new MainGameBackgroundItem(new Vector3(bushLocations[i].X * screenScale.X,
                    bushLocations[i].Y * screenScale.Y, this.bushZLocations[i]),
                    MainGameBackgroundItem.BackgroundType.Bush, i + 1);
            }
        }

        /// <summary>
        /// Updates this background
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < this.clouds.Length; i++)
            {
                this.clouds[i].Update();
            }
            for (int i = 0; i < this.bushes.Length; i++)
            {
                this.bushes[i].Update();
            }
        }

        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < this.clouds.Length; i++)
            {
                this.clouds[i].Draw(sb);
            }
            for (int i = 0; i < this.bushes.Length; i++)
            {
                this.bushes[i].Draw(sb);
            }
        }
    }
}
