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
using MainGame.Managers;
using XNAInterfaceComponents.ParentComponents;

namespace MainGame.UI
{
    public class IntroMoviePanel : XNAPanel
    {
        public XNASlider volumeSlider { get; set; }
        public XNAButton skipBtn { get; set; }

        public XNAVideoPlayer introVideoPlayer { get; set; }

        public static Texture2D SKIP_BTN_TEXTURE { get; set; }
        public static Texture2D SKIP_BTN_TEXTURE_HOVER { get; set; }

        static IntroMoviePanel()
        {
            SKIP_BTN_TEXTURE = Game1.GetInstance().Content.Load<Texture2D>("UI/Interface/Media/skip_btn");
            SKIP_BTN_TEXTURE_HOVER = Game1.GetInstance().Content.Load<Texture2D>("UI/Interface/Media/skip_btn_hover");
        }


        public IntroMoviePanel()
            : base(null,
                new Rectangle(0, 0, Game1.GetInstance().graphics.PreferredBackBufferWidth,
                    Game1.GetInstance().graphics.PreferredBackBufferHeight))
        {
            // this.backgroundColor = Color.Transparent;
            this.backgroundColor = Color.Black;
            this.border = null;

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
            this.introVideoPlayer.fadeOutAfterMS = 17000;
            this.introVideoPlayer.fadeOutDurationMS = 3000;


            this.skipBtn = new XNAButton(this, new Rectangle(
                    Game1.GetInstance().graphics.PreferredBackBufferWidth - 120,
                    Game1.GetInstance().graphics.PreferredBackBufferHeight - 110,
                100, 50), "");
            this.skipBtn.z = 0.0999f;
            // this.skipBtn.backgroundColor = Color.Transparent;
            this.skipBtn.border = null;
            this.skipBtn.backgroundTexture = SKIP_BTN_TEXTURE;
            this.skipBtn.mouseoverBackgroundTexture = SKIP_BTN_TEXTURE_HOVER;
            this.skipBtn.onClickListeners += this.OnVideoSkip;
            this.skipBtn.backgroundColor = Color.White;


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
        /// When the video should be skipped.
        /// </summary>
        /// <param name="source">The source button.</param>
        public void OnVideoSkip(XNAButton source)
        {
            this.introVideoPlayer.Stop();
        }

        /// <summary>
        /// Called when the volume has changed.
        /// </summary>
        /// <param name="slider">The slider that this all came from.</param>
        public void OnVolumeValueChanged(XNASlider slider)
        {
            this.introVideoPlayer.SetVolume(this.volumeSlider.currentValue);
        }

        public override void Update()
        {
            base.Update();

            Color drawColor = Color.Black;
            double difference = GameTimeManager.GetInstance().currentUpdateStartMS - this.introVideoPlayer.playerStartMS;
            if (difference > this.introVideoPlayer.fadeOutAfterMS)
            {
                int color = (int)(255 -
                    (255 * (difference - this.introVideoPlayer.fadeOutAfterMS) / this.introVideoPlayer.fadeOutDurationMS));
                this.backgroundColor = new Color(0, 0, 0, color);
            }
        }

        public override void Draw(SpriteBatch sb)
        {

            this.introVideoPlayer.Draw(sb);

            // Dirty hack :(
            if (this.introVideoPlayer.fadingOut)
            {
                /// Setting this Z in the constructor gives a gray screen I can't explain :(
                this.z = 0.1001f;
                Game1.GetInstance().background.Draw(sb);
            }

            base.Draw(sb);
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
