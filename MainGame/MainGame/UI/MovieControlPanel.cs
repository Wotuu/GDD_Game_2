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
    public class MovieControlPanel : XNAPanel
    {
        public XNASlider volumeSlider { get; set; }
        public XNAButton skipBtn { get; set; }

        public SimpleMoviePlayer simpleMoviePlayer { get; set; }

        public static Texture2D SKIP_BTN_TEXTURE { get; set; }
        public static Texture2D SKIP_BTN_TEXTURE_HOVER { get; set; }

        static MovieControlPanel()
        {
            SKIP_BTN_TEXTURE = Game1.GetInstance().Content.Load<Texture2D>("UI/Interface/Media/skip_btn");
            SKIP_BTN_TEXTURE_HOVER = Game1.GetInstance().Content.Load<Texture2D>("UI/Interface/Media/skip_btn_hover");
        }


        public MovieControlPanel(SimpleMoviePlayer simpleMoviePlayer)
            : base(null,
                new Rectangle(Game1.GetInstance().graphics.PreferredBackBufferWidth - 200,
                    Game1.GetInstance().graphics.PreferredBackBufferHeight - 100,
                    200,
                    100))
        {
            // this.backgroundColor = Color.Transparent;
            this.backgroundColor = Color.Black;
            this.border = null;

            this.simpleMoviePlayer = simpleMoviePlayer;


            this.skipBtn = new XNAButton(this, new Rectangle(
                    Game1.GetInstance().graphics.PreferredBackBufferWidth - 120,
                    Game1.GetInstance().graphics.PreferredBackBufferHeight - 110,
                100, 50), "");
            this.skipBtn.z = 0.0999f;
            // this.skipBtn.backgroundColor = Color.Transparent;
            this.skipBtn.border = null;
            this.skipBtn.backgroundTexture = SKIP_BTN_TEXTURE;
            this.skipBtn.mouseoverBackgroundTexture = SKIP_BTN_TEXTURE_HOVER;
            this.skipBtn.onClickListeners += this.simpleMoviePlayer.OnVideoSkip;
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
            this.volumeSlider.onSliderValueChangedListeners += this.simpleMoviePlayer.OnVolumeValueChanged;
        }

        public override void Update()
        {
            base.Update();

            Color drawColor = Color.Black;
            double difference = GameTimeManager.GetInstance().currentUpdateStartMS - this.simpleMoviePlayer.videoPlayer.playerStartMS;
            if (difference > this.simpleMoviePlayer.videoPlayer.fadeOutAfterMS)
            {
                int color = (int)(255 -
                    (255 * (difference - this.simpleMoviePlayer.videoPlayer.fadeOutAfterMS) / 
                    this.simpleMoviePlayer.videoPlayer.fadeOutDurationMS));
                this.backgroundColor = new Color(0, 0, 0, color);
            }
        }
    }
}
