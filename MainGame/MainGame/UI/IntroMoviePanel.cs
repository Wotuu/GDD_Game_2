using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using XNAInterfaceComponents.Components;
using XNAInterfaceComponents.AbstractComponents;
using Microsoft.Xna.Framework.Media;
using MainGame.Media;
using XNAInterfaceComponents.ParentComponents;

namespace MainGame.UI
{
    public class IntroMoviePanel : XNAPanel
    {
        public XNASlider volumeSlider { get; set; }

        public XNAVideoPlayer introVideoPlayer { get; set; }

        public IntroMoviePanel()
            : base(null,
                new Rectangle(0, 0, Game1.GetInstance().graphics.PreferredBackBufferWidth,
                    Game1.GetInstance().graphics.PreferredBackBufferHeight))
        {
            // this.backgroundColor = Color.Transparent;
            this.backgroundColor = Color.Black;

            Video video = Game1.GetInstance().Content.Load<Video>("Media/Video/startgame");
            float xFactor = video.Height / (float)Game1.GetInstance().graphics.PreferredBackBufferWidth;

            int videoHeight = (int)(Game1.GetInstance().graphics.PreferredBackBufferHeight * xFactor);
            this.introVideoPlayer = new XNAVideoPlayer(
                new Rectangle(0,
                    (Game1.GetInstance().graphics.PreferredBackBufferHeight - videoHeight) / 2,
                    Game1.GetInstance().graphics.PreferredBackBufferWidth,
                    videoHeight),
                video, 0.1f);

            this.introVideoPlayer.StartPlaying();
            this.introVideoPlayer.onVideoStoppedPlayingListeners += this.OnVideoStoppedPlaying;
            this.introVideoPlayer.SetVolume(0.1f);



            this.volumeSlider = new XNASlider(this,
                new Rectangle(
                    Game1.GetInstance().graphics.PreferredBackBufferWidth - 200,
                    Game1.GetInstance().graphics.PreferredBackBufferHeight - 50,
                180, 50));
            this.volumeSlider.z = 0.0999f;
            this.volumeSlider.sliderButton.z = 0.0998f;
            this.volumeSlider.minValue = 0f;
            this.volumeSlider.maxValue = 1f;
            this.volumeSlider.currentValue = 0.1f;
            this.volumeSlider.lineColor = Color.White;
            this.volumeSlider.backgroundColor = Color.White;
            this.volumeSlider.sliderButton.backgroundColor = Color.White;
            this.volumeSlider.SetSliderButtonWidth(25);

            this.volumeSlider.sliderTextField.visible = false;
            this.volumeSlider.onSliderValueChangedListeners += this.OnVolumeValueChanged;
        }

        /// <summary>
        /// Called when the volume has changed.
        /// </summary>
        /// <param name="slider">The slider that this all came from.</param>
        public void OnVolumeValueChanged(XNASlider slider)
        {
            this.introVideoPlayer.SetVolume(this.volumeSlider.currentValue);
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            this.introVideoPlayer.Draw(sb);
        }

        /// <summary>
        /// Called when the video has stopped playing.
        /// </summary>
        /// <param name="player"></param>
        public void OnVideoStoppedPlaying(XNAVideoPlayer player)
        {
            this.Unload();
        }
    }
}
