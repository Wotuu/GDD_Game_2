using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using MainGame.Media;
using MainGame.Managers;
using MainGame.UI;

public delegate void OnVideoStoppedPlaying(XNAVideoPlayer source, MovieControlPanel panel);
namespace MainGame.Media
{
    public class XNAVideoPlayer
    {
        public Rectangle bounds { get; set; }
        public float z { get; set; }

        public Video video { get; set; }

        private VideoPlayer player { get; set; }
        public OnVideoStoppedPlaying onVideoStoppedPlayingListeners { get; set; }
        private MediaState previousState { get; set; }

        public float fadeOutAfterMS { get; set; }
        public float fadeOutDurationMS { get; set; }

        public Boolean fadingOut { get; set; }

        public double playerStartMS { get; set; }

        public MovieControlPanel panel { get; set; }

        public XNAVideoPlayer(/*MovieControlPanel panel,*/ Rectangle bounds, Video video, float z)
        {
            this.panel = panel;
            this.bounds = bounds;
            this.video = video;

            this.fadeOutAfterMS = Int32.MaxValue;

            this.player = new VideoPlayer();

            this.z = z;
        }

        /// <summary>
        /// Gets whether or not the player is currently playing.
        /// </summary>
        /// <returns>Yes or no.</returns>
        public Boolean IsPlaying()
        {
            if (this.player.IsDisposed) return false;
            return this.player.State == MediaState.Playing;
        }

        /// <summary>
        /// Sets the volume of this player.
        /// </summary>
        /// <param name="volume">The volume, float between 0.0f and 1.0f</param>
        public void SetVolume(float volume)
        {
            this.player.Volume = volume;
        }

        /// <summary>
        /// Plays the current video.
        /// </summary>
        public void StartPlaying()
        {
            this.player.Play(video);
            this.playerStartMS = GameTimeManager.GetInstance().currentUpdateStartMS;
        }

        /// <summary>
        /// Pauses current playback.
        /// </summary>
        public void Pause()
        {
            this.player.Pause();
        }

        /// <summary>
        /// Stops current playback.
        /// </summary>
        public void Stop()
        {
            this.player.Stop();
            onVideoStoppedPlayingListeners(this, this.panel);
            this.player.Dispose();
        }

        public void Draw(SpriteBatch sb)
        {

            if (this.player.State == MediaState.Stopped && this.previousState == MediaState.Playing &&
                this.onVideoStoppedPlayingListeners != null)
            {
                onVideoStoppedPlayingListeners(this, this.panel);
                this.player.Dispose();
                return;
            }

            //if (this.player.State == MediaState.Playing)
            //{
            Color drawColor = Color.White;
            double difference = GameTimeManager.GetInstance().currentUpdateStartMS - this.playerStartMS;
            if (difference > this.fadeOutAfterMS)
            {
                int color = (int)(255 - (255 * (difference - fadeOutAfterMS) / this.fadeOutDurationMS));
                drawColor = new Color(color, color, color, color);
                this.fadingOut = true;

                if (color <= 0)
                {
                    this.Stop();
                    return;
                }
            }
            sb.Draw(this.player.GetTexture(), this.bounds, null,
                drawColor, 0f, Vector2.Zero, SpriteEffects.None, this.z);
            //}



            this.previousState = this.player.State;
        }
    }
}
