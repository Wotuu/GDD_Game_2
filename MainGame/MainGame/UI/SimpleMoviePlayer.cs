using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using MainGame.Media;
using Microsoft.Xna.Framework;
using XNAInterfaceComponents.ParentComponents;
using XNAInterfaceComponents.AbstractComponents;
using Microsoft.Xna.Framework.Graphics;

namespace MainGame.UI
{
    public class SimpleMoviePlayer
    {
        public XNAVideoPlayer videoPlayer { get; set; }
        public MovieControlPanel panel { get; set; }

        public SimpleMoviePlayer(Video video)
        {
            float xFactor = video.Height / (float)Game1.GetInstance().graphics.PreferredBackBufferWidth;

            // this.panel = new MovieControlPanel(this);

            int videoHeight = (int)(Game1.GetInstance().graphics.PreferredBackBufferHeight * xFactor);
            this.videoPlayer = new XNAVideoPlayer(/*this.panel,*/
                new Rectangle(0,
                    (Game1.GetInstance().graphics.PreferredBackBufferHeight - videoHeight) / 2,
                    Game1.GetInstance().graphics.PreferredBackBufferWidth,
                    videoHeight),
                video, 0.1f);

            this.videoPlayer.StartPlaying();
            this.videoPlayer.onVideoStoppedPlayingListeners += this.OnVideoStoppedPlaying;
            this.videoPlayer.SetVolume(0.1f);
            this.videoPlayer.fadeOutAfterMS = 17000;
            this.videoPlayer.fadeOutDurationMS = 3000;
        }

        /// <summary>
        /// When the video should be skipped.
        /// </summary>
        /// <param name="source">The source button.</param>
        public void OnVideoSkip(XNAButton source)
        {
            this.videoPlayer.Stop();
        }

        /// <summary>
        /// Called when the volume has changed.
        /// </summary>
        /// <param name="slider">The slider that this all came from.</param>
        public void OnVolumeValueChanged(XNASlider slider)
        {
            this.videoPlayer.SetVolume(slider.currentValue);
        }

        /// <summary>
        /// Called when the video has stopped playing.
        /// </summary>
        /// <param name="player"></param>
        public void OnVideoStoppedPlaying(XNAVideoPlayer player, MovieControlPanel panel)
        {
            // panel.Unload();
        }

        public void Draw(SpriteBatch sb)
        {
            this.videoPlayer.Draw(sb);
            /*
            // Dirty hack :(
            if (this.videoPlayer.fadingOut)
            {
                /// Setting this Z in the constructor gives a gray screen I can't explain :(
                this.videoPlayer.z = 0.1001f;
                Game1.GetInstance().background.Draw(sb);
            }*/
        }
    }
}
