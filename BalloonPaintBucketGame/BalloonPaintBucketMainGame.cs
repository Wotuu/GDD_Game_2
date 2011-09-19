using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using BalloonPaintBucketGame.Managers;
using ParticleEngine;
using BalloonPaintBucketGame.Balloons;
using BalloonPaintBucketGame.Players;
using BalloonPaintBucketGame.PaintBuckets;
using ParticleEngine.Emitter;
using ParticleEngine.Particles;
using BalloonPaintBucketGame.Particles.Particles;
using BalloonPaintBucketGame.Particles;
using PolygonCollision.Managers;

namespace BalloonPaintBucketGame
{
    public class BalloonPaintBucketMainGame
    {
        #region Singleton logic
        private static BalloonPaintBucketMainGame instance;

        public static BalloonPaintBucketMainGame GetInstance()
        {
            if (instance == null) instance = new BalloonPaintBucketMainGame();
            return instance;
        }

        private BalloonPaintBucketMainGame() { }
        #endregion

        public Game game { get; set; }
        public Player player { get; set; }
        public PaintBucket[] paintBuckets { get; set; }

        public int collisionCheckMS = 25;
        public Double lastCollisionCheckMS { get; set; }

        public void Initialize(Game game)
        {
            this.game = game;

            ParticleManager.DEFAULT_TEXTURE = this.game.Content.Load<Texture2D>("Particles/default");



            // new Balloon(Balloon.BalloonColor.Pink);
            this.player = new Player();

            this.paintBuckets = new PaintBucket[3];
            this.paintBuckets[0] = new PaintBucket(PaintBucket.PaintBucketColor.Blue);

            Viewport viewPort = this.game.GraphicsDevice.Viewport;
            this.paintBuckets[0].location = new Vector2(
                (viewPort.Width / 6) - this.paintBuckets[0].GetDrawRectangle().Width / 2,
                viewPort.Height - this.paintBuckets[0].GetDrawRectangle().Height);

            this.paintBuckets[1] = new PaintBucket(PaintBucket.PaintBucketColor.Pink);
            this.paintBuckets[1].location = new Vector2(
                (viewPort.Width / 6) * 3 - this.paintBuckets[1].GetDrawRectangle().Width / 2,
                viewPort.Height - this.paintBuckets[1].GetDrawRectangle().Height);

            this.paintBuckets[2] = new PaintBucket(PaintBucket.PaintBucketColor.Yellow);
            this.paintBuckets[2].location = new Vector2(
                (viewPort.Width / 6) * 5 - this.paintBuckets[2].GetDrawRectangle().Width / 2,
                viewPort.Height - this.paintBuckets[2].GetDrawRectangle().Height);

            /*
            this.game.Content.Load<Texture2D>("Balloons/balon1");
            this.game.Content.Load<Texture2D>("Balloons/balon2");
            this.game.Content.Load<Texture2D>("Balloons/balon3");
            this.game.Content.Load<Texture2D>("Balloons/balon4");
            */
        }

        public void Update()
        {
            GameTimeManager.GetInstance().OnStartUpdate();
            BalloonManager.GetInstance().UpdateBalloons();
            ParticleManager.GetInstance().Update((float)GameTimeManager.GetInstance().time_step);

            if ( GameTimeManager.GetInstance().currentUpdateStartMS - this.lastCollisionCheckMS > this.collisionCheckMS)
            {
                this.CheckCollision();
                this.lastCollisionCheckMS = GameTimeManager.GetInstance().currentUpdateStartMS;
            }

            this.player.Update();
        }

        /// <summary>
        /// Draws the game.
        /// </summary>
        /// <param name="sb">The spritebatch to draw on.</param>
        public void Draw(SpriteBatch sb)
        {
            GameTimeManager.GetInstance().OnStartDraw();
            BalloonManager.GetInstance().DrawBalloons(sb);
            ParticleManager.GetInstance().Draw(sb.GraphicsDevice);
            // PolygonManager.GetInstance().DrawPolygons(sb);

            foreach (PaintBucket bucket in this.paintBuckets)
            {
                bucket.Draw(sb);
            }

            this.player.Draw(sb);
        }

        /// <summary>
        /// Checks collision of the particles with the buckets.
        /// </summary>
        public void CheckCollision()
        {
            for (int i = 0; i < ParticleManager.GetInstance().emitterList.Count(); i++)
            {
                ParticleEmitter emitter = ParticleManager.GetInstance().emitterList.ElementAt(i);

                for (int j = 0; j < emitter.particles.Count(); j++)
                {
                    Particle particle = emitter.particles.ElementAt(j);

                    if (particle is RectangleCollideable)
                    {
                        foreach (PaintBucket bucket in this.paintBuckets)
                        {
                            RectangleCollideable collideable = ((RectangleCollideable)particle);
                            if (collideable.CheckCollision(bucket))
                            {
                                collideable.OnCollision(bucket);
                                bucket.OnCollision(collideable);
                            }
                        }
                    }
                }
            }
        }
    }
}
