using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniGameOverview.Animations;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MiniGameOverview.Managers;
using MiniGameOverview.Map.Pathing;
using XNAInputLibrary.KeyboardInput;
using Microsoft.Xna.Framework.Input;

public delegate void OnPlayerReachedPathItem(PathItem item);

namespace MiniGameOverview.Players
{
    public class Player
    {
        public Animator animator { get; set; }
        public Vector2 location { get; set; }
        public float z { get; set; }
        public Vector2 speed = new Vector2(2f, 0f);

        public OnPlayerReachedPathItem onPlayerReachedPathItemListeners { get; set; }

        public Vector2 scale { get; set; }
        private State _moveState { get; set; }
        public State moveState
        {
            get
            {
                return _moveState;
            }
            set
            {
                this._moveState = value;

                if (this.moveState == State.Moving)
                {
                    if (this.moveTarget.location.X < this.location.X)
                        this.effects = SpriteEffects.FlipHorizontally;
                    else this.effects = SpriteEffects.None;
                }

            }
        }
        private SpriteEffects effects = SpriteEffects.None;

        private PathItem moveTarget { get; set; }

        public enum State
        {
            Idle,
            Moving
        }

        public Player()
        {
            this.scale = new Vector2(0.40f, 0.40f);
            /*
            this.animator = new Animator(
                MiniGameOverviewMainGame.GetInstance().game.Content.Load<Texture2D>("Player/fuzz_sprite"),
                new Rectangle(0, 0, 267, 258), 150);
             */
            this.animator = new Animator(
                MiniGameOverviewMainGame.GetInstance().game.Content.Load<Texture2D>("Player/fuzz_sprite_walk2"),
                new Rectangle(0, 0, 267, 258), 150);
            /*
            this.animator = new Animator(
                MiniGameOverviewMainGame.GetInstance().game.Content.Load<Texture2D>("Player/buzz_sprite_walk2"),
                new Rectangle(0, 0, 270, 230), 150);*/

            this.z = 0.87f;
        }

        /// <summary>
        /// Gets the Draw rectangle of the player.
        /// </summary>
        /// <returns>The rectangle to draw.</returns>
        public Rectangle GetDrawRectangle()
        {
            return new Rectangle((int)(this.location.X - ((267 * this.scale.X) / 2f)),
                (int)(this.location.Y - ((258 * this.scale.Y) / 2f)),
                (int)(267 * this.scale.X), (int)(258 * this.scale.Y));
        }

        public void Update()
        {
            if (this.moveState == State.Idle)
            {
                KeyboardState state = Keyboard.GetState();
                if (state.IsKeyDown(Keys.Left))
                {
                    // Assignment to itsself, or the next
                    if (this.moveTarget.previous != null)
                    {
                        this.moveTarget = this.moveTarget.previous;
                        this.moveState = State.Moving;

                        this.speed = (this.speed.X > 0) ? this.speed * -1f : this.speed;
                    }
                }
                else if (state.IsKeyDown(Keys.Right))
                {
                    if (this.moveTarget.isFullyColored)
                    {
                        // Assignment to itsself, or the next
                        this.moveTarget = (this.moveTarget.next == null) ? this.moveTarget : this.moveTarget.next;

                        this.moveState = State.Moving;
                    }
                    this.speed = (this.speed.X < 0) ? this.speed * -1f : this.speed;
                }
            }

            if (this.moveState == State.Moving)
            {
                this.location += (this.speed * (float)GameTimeManager.GetInstance().time_step);
                if (Math.Abs(this.location.X - this.moveTarget.location.X) < 1f)
                {
                    this.TeleportTo(this.moveTarget);
                    this.moveState = State.Idle;
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this.animator.sourceTexture, this.GetDrawRectangle(),
                (this.moveState == State.Idle) ?
                this.animator.GetSourceRectangle(0) :
                this.animator.GetSourceRectangle(GameTimeManager.GetInstance().currentUpdateStartMS),
                Color.White, 0f, Vector2.Zero, this.effects, this.z);
        }


        /// <summary>
        /// Teleports the player a new path item (cheat away!)
        /// </summary>
        /// <param name="item">The item to teleport to</param>
        public void TeleportTo(PathItem item)
        {
            this.moveTarget = item;
            this.location = new Vector2(
                (item.location.X),
                (item.location.Y - this.GetDrawRectangle().Height / 2));

            if (this.onPlayerReachedPathItemListeners != null)
                this.onPlayerReachedPathItemListeners(item);
        }
    }
}
