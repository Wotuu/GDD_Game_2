using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using MainGame.Media;

public delegate void OnVideoStoppedPlaying(XNAVideoPlayer source);
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

        public XNAVideoPlayer(Rectangle bounds, Video video, float z)
        {
            this.bounds = bounds;
            this.video = video;

            this.player = new VideoPlayer();
        }

        /// <summary>
        /// Gets whether or not the player is currently playing.
        /// </summary>
        /// <returns>Yes or no.</returns>
        public Boolean IsPlaying()
        {
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
        }

        public void Draw(SpriteBatch sb)
        {
            if (this.player.State == MediaState.Stopped && this.previousState == MediaState.Playing &&
                this.onVideoStoppedPlayingListeners != null)
                onVideoStoppedPlayingListeners(this);

            if (this.player.State == MediaState.Playing)
                sb.Draw(this.player.GetTexture(), this.bounds, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, this.z);

            this.previousState = this.player.State;
        }
    }
}
