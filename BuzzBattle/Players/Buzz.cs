using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BuzzBattle.Animations;
using BuzzBattle.Managers;
using BuzzBattle.Shells;
using BuzzBattle.Misc;
using BuzzBattle.Util;
using System.Diagnostics;

namespace BuzzBattle.Players
{
    public class Buzz
    {
        public Texture2D frontTexture { get; set; }
        public Animator animator { get; set; }

        public Vector2 location { get; set; }
        public float z { get; set; }
        public Vector2 scale { get; set; }
        public Vector2 speed { get; set; }

        public Rectangle frontSourceRectangle { get; set; }
        public Rectangle animationSourceRectangle = new Rectangle(0, 0, 270, 230);

        public MoveState moveState { get; set; }

        public float maxHealth { get; set; }

        public float _currentHealth { get; set; }
        public float currentHealth
        {
            get
            {
                return _currentHealth;
            }
            set
            {
                this.healthBar.currentValue = value;
                this._currentHealth = value;
            }
        }

        public float leftXBarrier = 100;
        public float rightXBarrier = BuzzBattleMainGame.GetInstance().game.GraphicsDevice.Viewport.Width - 200;

        public double lastMoveMS { get; set; }
        public double moveCooldownMS = 12000;

        public HealthBar healthBar { get; set; }

        public VulnerabilityIndicator vulnerabilityIndicator { get; set; }

        public enum MoveState
        {
            Still,
            MovingLeft,
            MovingRight
        }

        public Vector2[] eyeLocations = new Vector2[]{
            new Vector2(69, 88),
            new Vector2(94, 88)
        };

        public LinkedList<Shell> eyeBeamTargets = new LinkedList<Shell>();

        public float disabledDurationMS = 2000f;
        public float disabledDurationIncrease = 200f;

        public Buzz()
        {
            this.scale = new Vector2(0.50f, 0.50f);

            this.speed = new Vector2(2f, 0);

            this.frontTexture = BuzzBattleMainGame.GetInstance().game.Content.Load<Texture2D>("BuzzGame/Buzz/buzz_front");
            this.frontSourceRectangle = new Rectangle(0, 0, this.frontTexture.Width, this.frontTexture.Height);


            this.animator = new Animator(
                BuzzBattleMainGame.GetInstance().game.Content.Load<Texture2D>("BuzzGame/Buzz/buzz_sprite_walk2"),
                animationSourceRectangle, 150);

            this.location = new Vector2(this.leftXBarrier, 450);

            this.moveState = MoveState.Still;


            this.z = 0.9f;
            this.maxHealth = 20;
            this.healthBar = new HealthBar(this,
                new Rectangle(0, 0,
                    BuzzBattleMainGame.GetInstance().game.GraphicsDevice.Viewport.Width,
                    40), this.maxHealth);
            this.currentHealth = 2;

            this.vulnerabilityIndicator = new VulnerabilityIndicator(
                new Vector3(this.location.X, this.GetDrawRectangle().Top - 10, this.z), Shell.ShellColor.Blue);
        }

        /// <summary>
        /// Gets the rectangle that you should use to draw this with.
        /// </summary>
        /// <returns>bla</returns>
        public Rectangle GetDrawRectangle()
        {
            switch (this.moveState)
            {
                case MoveState.Still:
                    return new Rectangle((int)(this.location.X - ((this.frontSourceRectangle.Width / 2f) * this.scale.X)),
                        (int)(this.location.Y - ((this.frontSourceRectangle.Height / 2f) * this.scale.Y)),
                        (int)(this.frontSourceRectangle.Width * this.scale.X),
                        (int)(this.frontSourceRectangle.Height * this.scale.Y));
                case MoveState.MovingLeft:
                case MoveState.MovingRight:
                    return new Rectangle((int)(this.location.X - ((this.animationSourceRectangle.Width / 2f) * this.scale.X)),
                        (int)(this.location.Y - ((this.animationSourceRectangle.Height / 2f) * this.scale.Y)),
                        (int)(this.animationSourceRectangle.Width * this.scale.X),
                        (int)(this.animationSourceRectangle.Height * this.scale.Y));
            }
            return Rectangle.Empty;
        }

        public void Update()
        {
            if (this.currentHealth <= 0) return;

            double currTime = GameTimeManager.GetInstance().currentUpdateStartMS;
            switch (this.moveState)
            {
                case MoveState.Still:
                    if (currTime - this.lastMoveMS > this.moveCooldownMS)
                    {
                        if (this.location.X == this.leftXBarrier)
                        {
                            this.moveState = MoveState.MovingRight;
                            // Speed must be positive now
                            if (this.speed.X < 0) this.speed *= -1;
                        }
                        else
                        {
                            this.moveState = MoveState.MovingLeft;
                            // Speed must be negative now
                            if (this.speed.X > 0) this.speed *= -1;
                        }
                        this.lastMoveMS = currTime;
                        this.eyeBeamTargets.Clear();
                    }
                    else if (eyeBeamTargets.Count == 0)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            Shell randomShell = BuzzBattleMainGame.GetInstance().GetRandomNonDisabledShell();
                            eyeBeamTargets.AddLast(randomShell);
                            randomShell.disabled = true;
                        }
                        // Switch vulnerability
                        int random = new Random().Next(1, 5);
                        this.vulnerabilityIndicator = new VulnerabilityIndicator(
                            new Vector3(this.location.X, this.GetDrawRectangle().Top - 10, this.z),
                            (Shell.ShellColor)(random));
                        Debug.WriteLine(random);
                        // Increase disable duration
                        this.disabledDurationMS += this.disabledDurationIncrease;
                    }
                    break;
                case MoveState.MovingLeft:
                    this.location += (this.speed * (float)GameTimeManager.GetInstance().time_step);

                    if (this.location.X < this.leftXBarrier)
                    {
                        this.location = new Vector2(this.leftXBarrier, this.location.Y);
                        this.moveState = MoveState.Still;
                    }
                    break;
                case MoveState.MovingRight:
                    this.location += (this.speed * (float)GameTimeManager.GetInstance().time_step);

                    if (this.location.X > this.rightXBarrier)
                    {
                        this.location = new Vector2(this.rightXBarrier, this.location.Y);
                        this.moveState = MoveState.Still;
                    }
                    break;
            }

            // Stay on top of Buzz
            this.vulnerabilityIndicator.location = new Vector2(this.location.X, this.vulnerabilityIndicator.location.Y);
            this.vulnerabilityIndicator.Update();
        }

        public void Draw(SpriteBatch sb)
        {
            Rectangle drawRectangle = this.GetDrawRectangle();
            switch (this.moveState)
            {
                case MoveState.Still:
                    sb.Draw(this.frontTexture, drawRectangle, null, Color.White, 0f,
                        Vector2.Zero, SpriteEffects.None, this.z);
                    break;
                case MoveState.MovingLeft:
                case MoveState.MovingRight:

                    SpriteEffects effect = SpriteEffects.None;

                    if (this.moveState == MoveState.MovingLeft && this.currentHealth > 0)
                    {
                        effect = SpriteEffects.FlipHorizontally;
                    }
                    else if (this.currentHealth <= 0)
                    {
                        // Dead
                        effect = SpriteEffects.FlipVertically;
                    }
                    sb.Draw(this.animator.sourceTexture, drawRectangle,
                        // If buzz's dead, stop him walking
                        (this.currentHealth > 0 ) ?
                        this.animator.GetSourceRectangle(GameTimeManager.GetInstance().currentUpdateStartMS) :
                        this.animator.GetSourceRectangle(0),
                        Color.White, 0f, Vector2.Zero, effect, this.z);
                    break;
            }

            this.healthBar.Draw(sb);

            if (this.currentHealth <= 0) return;

            this.vulnerabilityIndicator.Draw(sb);
            int count = 0;
            foreach (Shell shell in this.eyeBeamTargets)
            {
                Vector2 location = new Vector2(shell.GetDrawRectangle().Center.X,
                                    shell.GetDrawRectangle().Center.Y);
                DrawUtil.DrawLine(sb, location,
                    (this.eyeLocations[count] * this.scale) + new Vector2(drawRectangle.X, drawRectangle.Y),
                    Color.Red, 2, this.z - 0.001f);
                count++;
                shell.disabled = true;
            }
        }
    }
}
