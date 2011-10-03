using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MiniGameOverview;
using MainGame.Managers;

namespace MainGame.Cards
{
    public class WinningCard
    {
        public Texture2D texture { get; set; }

        public Vector2 location { get; set; }
        public float z { get; set; }

        public Vector2 originalScale { get; set; }
        public Vector2 scale { get; set; }
        public Vector2 speed { get; set; }

        public Boolean hasAnimated { get; set; }
        public Boolean isScalingUp { get; set; }
        public Boolean isBackground { get; set; }
        public Boolean isShowingBackground { get; set; }
        private Boolean isAlmostFinished { get; set; }
        public Boolean shouldBeRemoved { get; set; }

        private CardEmitter emitter { get; set; }

        private double finishedRotatingMS { get; set; }
        private int fadeOutWaitMS = 5000;
        private double fadeOutMS = 1500;


        private CardColor _color { get; set; }
        public CardColor color
        {
            get
            {
                return _color;
            }
            set
            {
                switch (value)
                {
                    case CardColor.Blue:
                        this.texture = WinningCard.BLUE_CARD;
                        break;
                    case CardColor.Green:
                        this.texture = WinningCard.GREEN_CARD;
                        break;
                    case CardColor.Pink:
                        this.texture = WinningCard.PINK_CARD;
                        break;
                    case CardColor.Purple:
                        this.texture = WinningCard.PURPLE_CARD;
                        break;
                    case CardColor.Red:
                        this.texture = WinningCard.RED_CARD;
                        break;
                    case CardColor.Yellow:
                        this.texture = WinningCard.YELLOW_CARD;
                        break;
                }

                _color = value;
            }
        }

        public static Texture2D BACK_CARD { get; set; }
        public static Texture2D BLUE_CARD { get; set; }
        public static Texture2D YELLOW_CARD { get; set; }
        public static Texture2D PINK_CARD { get; set; }
        public static Texture2D GREEN_CARD { get; set; }
        public static Texture2D PURPLE_CARD { get; set; }
        public static Texture2D RED_CARD { get; set; }

        public static String BLUE_CARD_TEXT = "Congratulations! You've unlocked the power of the blue ";
        public static String YELLOW_CARD_TEXT = "";
        public static String PINK_CARD_TEXT = "";

        static WinningCard()
        {
            BACK_CARD = Game1.GetInstance().Content.Load<Texture2D>("Cards/winning_back");

            BLUE_CARD = Game1.GetInstance().Content.Load<Texture2D>("Cards/winning_blauw");
            YELLOW_CARD = Game1.GetInstance().Content.Load<Texture2D>("Cards/winning_geel");
            PINK_CARD = Game1.GetInstance().Content.Load<Texture2D>("Cards/winning_roze");
            GREEN_CARD = Game1.GetInstance().Content.Load<Texture2D>("Cards/winning_groen");
            PURPLE_CARD = Game1.GetInstance().Content.Load<Texture2D>("Cards/winning_paars");
            RED_CARD = Game1.GetInstance().Content.Load<Texture2D>("Cards/winning_rood");
        }

        public enum CardColor
        {
            Blue,
            Yellow,
            Pink,
            Green,
            Purple,
            Red
        }

        public WinningCard(CardColor color)
        {
            this.color = color;
            this.location = new Vector2(
                Game1.GetInstance().graphics.PreferredBackBufferWidth / 2f,
                Game1.GetInstance().graphics.PreferredBackBufferHeight / 2f);

            this.originalScale = new Vector2(0.35f, 0.35f);
            this.scale = this.originalScale;
            this.speed = new Vector2(0.097f, 0);

            this.z = 0.5f;

            CardManager.GetInstance().drawnCards.AddLast(this);
            if (!CardManager.GetInstance().IsAwarded(this))
            {
                CardManager.GetInstance().awardedCards.AddLast(this);
                this.emitter = new CardEmitter(this);
            }

        }

        /// <summary>
        /// Gets the rectangle that you should use to draw this with.
        /// </summary>
        /// <returns>bla</returns>
        public Rectangle GetDrawRectangle()
        {
            return new Rectangle((int)(this.location.X - ((this.texture.Width / 2) * this.scale.X)),
                (int)(this.location.Y - ((this.texture.Height / 2) * this.scale.Y)),
                (int)(this.texture.Width * this.scale.X),
                (int)(this.texture.Height * this.scale.Y));
        }

        public void Update()
        {

            if (hasAnimated) return;

            this.scale = new Vector2(
                (this.isScalingUp) ?
                // Scaling up
                this.scale.X + (float)(this.speed.X * GameTimeManager.GetInstance().time_step) :
                // Scaling down
                this.scale.X - (float)(this.speed.X * GameTimeManager.GetInstance().time_step), this.scale.Y);

            if (this.scale.X < 0.01f)
            {
                this.scale = new Vector2(Math.Abs(this.scale.X) +
                    (float)(this.speed.X * GameTimeManager.GetInstance().time_step), this.scale.Y);
                this.isScalingUp = !this.isScalingUp;
                this.isShowingBackground = !this.isShowingBackground;
            }
            else if (this.scale.X > this.originalScale.X)
            {
                this.scale = new Vector2(this.scale.X - (this.scale.X - this.originalScale.X), this.scale.Y);
                this.isScalingUp = !this.isScalingUp;

                if (isAlmostFinished && !this.isShowingBackground)
                {
                    this.hasAnimated = true;
                    this.scale = this.originalScale;
                    this.emitter.stopSpawningParticles = true;

                    this.finishedRotatingMS = GameTimeManager.GetInstance().currentUpdateStartMS;
                }
            }

            if (this.speed.X < 0.012f)
                isAlmostFinished = true;
            else
                this.speed = new Vector2(this.speed.X - ((float)(0.0005 * GameTimeManager.GetInstance().time_step)), this.speed.Y);
            // Console.Out.WriteLine(this.speed);
        }

        public void Draw(SpriteBatch sb)
        {

            if (!isShowingBackground)
            {
                Color drawColor = Color.White;
                double diff = 0;
                if (this.emitter.stopSpawningParticles &&
                    (diff = GameTimeManager.GetInstance().previousUpdateStartMS - this.finishedRotatingMS) > this.fadeOutWaitMS)
                {
                    int color = 255 - (int)(255 * ((diff - this.fadeOutWaitMS) / this.fadeOutMS));
                    drawColor = new Color(color, color, color, color);
                    if (color <= 0)
                        this.shouldBeRemoved = true;
                }

                sb.Draw(this.texture, this.GetDrawRectangle(), null, drawColor, 0f,
                    Vector2.Zero, SpriteEffects.None, this.z);
            }
            else
                sb.Draw(WinningCard.BACK_CARD, this.GetDrawRectangle(), null, Color.White, 0f,
                    Vector2.Zero, SpriteEffects.None, this.z);
        }
    }
}
