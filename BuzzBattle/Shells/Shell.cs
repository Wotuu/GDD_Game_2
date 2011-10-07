using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BuzzBattle.Managers;
using XNAInputHandler.MouseInput;

namespace BuzzBattle.Shells
{
    public class Shell : MouseMotionListener, MouseClickListener
    {
        public static Texture2D PINK_SHELL;
        public static Texture2D YELLOW_SHELL;
        public static Texture2D BLUE_SHELL;
        public static Texture2D GREEN_SHELL;

        private ShellColor _color { get; set; }
        public ShellColor color
        {
            get
            {
                return _color;
            }
            set
            {
                switch (value)
                {
                    case ShellColor.Pink:
                        this.texture = PINK_SHELL;
                        break;
                    case ShellColor.Yellow:
                        this.texture = YELLOW_SHELL;
                        break;
                    case ShellColor.Blue:
                        this.texture = BLUE_SHELL;
                        break;
                    case ShellColor.Green:
                        this.texture = GREEN_SHELL;
                        break;
                }
                this._color = value;
            }
        }
        public Color selectedColor = Color.DarkRed;
        public Color mouseOverColor = Color.Yellow;
        public Color disabledColor = Color.Gray;

        public Boolean selected { get; set; }

        private Boolean _disabled { get; set; }
        public Boolean disabled
        {
            get
            {
                return _disabled;
            }
            set
            {
                if (value)
                {
                    this.disabledStartMS = GameTimeManager.GetInstance().currentUpdateStartMS;
                    this.disabledDurationMS = BuzzBattleMainGame.GetInstance().buzz.disabledDurationMS;
                    this.drawColor = this.disabledColor;
                    this.selected = false;
                }

                _disabled = value;
            }
        }

        public Color drawColor { get; set; }

        public Vector2 location { get; set; }
        public Texture2D texture { get; set; }

        public double disabledStartMS { get; set; }
        public double disabledDurationMS { get; set; }

        public Vector2 scale { get; set; }
        public float z { get; set; }

        private MouseEvent lastMouseEvent { get; set; }

        public PaintBomb paintBomb { get; set; }


        static Shell()
        {
            PINK_SHELL = BuzzBattleMainGame.GetInstance().game.Content.Load<Texture2D>
                ("BuzzGame/Shells/schelp_roze");
            YELLOW_SHELL = BuzzBattleMainGame.GetInstance().game.Content.Load<Texture2D>
                ("BuzzGame/Shells/schelp_geel");
            BLUE_SHELL = BuzzBattleMainGame.GetInstance().game.Content.Load<Texture2D>
                ("BuzzGame/Shells/schelp_blauw");
            GREEN_SHELL = BuzzBattleMainGame.GetInstance().game.Content.Load<Texture2D>
                ("BuzzGame/Shells/schelp_groen");
        }

        public enum ShellColor
        {
            None,
            Pink,
            Yellow,
            Blue,
            Green
        }

        public Shell(ShellColor color)
        {
            Color healthBarFillColor = Color.White;

            this.scale = new Vector2(0.50f, 0.50f);
            this.color = color;
            this.z = 0.98f;

            ShellManager.GetInstance().shells.AddLast(this);
            MouseManager.GetInstance().mouseMotionListeners += this.OnMouseMotion;
            MouseManager.GetInstance().mouseClickedListeners += this.OnMouseClick;
        }

        public void Update()
        {
            if (GameTimeManager.GetInstance().currentUpdateStartMS - this.disabledStartMS > this.disabledDurationMS)
            {
                this.disabled = false;
            }

            if (this.lastMouseEvent != null)
                this.CheckColors(this.lastMouseEvent.location);

            if (!(this.paintBomb == null ||
                this.paintBomb.scale == Vector2.Zero))
            {
                this.paintBomb.Update();
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this.texture, this.GetDrawRectangle(), null, this.drawColor,
                0f, Vector2.Zero, SpriteEffects.None, z);
            if (!(this.paintBomb == null ||
                this.paintBomb.scale == Vector2.Zero))
            {
                this.paintBomb.Draw(sb);
            }
        }

        /// <summary>
        /// Gets the draw rectangle of this balloon.
        /// </summary>
        /// <returns>The rectangle that can be used for drawing.</returns>
        public Rectangle GetDrawRectangle()
        {
            return new Rectangle((int)this.location.X, (int)this.location.Y,
                (int)(this.texture.Width * this.scale.X), (int)(this.texture.Height * this.scale.Y));
        }

        /// <summary>
        /// Gets the color of this paint bucket.
        /// </summary>
        /// <returns>The color.</returns>
        public Color GetColor()
        {
            switch (this.color)
            {
                case ShellColor.Pink:
                    return Color.DeepPink;
                case ShellColor.Blue:
                    return Color.Blue;
                case ShellColor.Yellow:
                    return Color.Yellow;
            }
            return Color.Red;
        }

        /// <summary>
        /// Gets the center of this balloon.
        /// </summary>
        /// <returns>The dead center.</returns>
        public Vector2 GetCenter()
        {
            return new Vector2(this.location.X + ((this.texture.Width * this.scale.X) / 2),
                this.location.Y + ((this.texture.Height * this.scale.Y) / 2));
        }

        /// <summary>
        /// Checks the colors of this shell.
        /// </summary>
        public void CheckColors(Point mouseLocation)
        {
            if (this.disabled)
            {
                this.drawColor = this.disabledColor;
                return;
            }
            if (this.GetDrawRectangle().Contains(mouseLocation))
            {
                this.drawColor = mouseOverColor;
            }
            else this.drawColor = (this.selected) ? this.selectedColor : Color.White;
        }

        public void OnMouseMotion(MouseEvent e)
        {
            this.CheckColors(e.location);
            this.lastMouseEvent = e;
        }

        public void OnMouseDrag(MouseEvent e)
        {

        }

        public void OnMouseClick(MouseEvent e)
        {
            if (!this.disabled && this.GetDrawRectangle().Contains(e.location))
            {
                foreach (Shell shell in BuzzBattleMainGame.GetInstance().shells)
                {
                    shell.selected = false;
                }
                this.selected = true;
            }

            if (this.selected && (this.paintBomb == null ||
                this.paintBomb.scale == Vector2.Zero))
            {
                foreach (Shell shell in BuzzBattleMainGame.GetInstance().shells)
                {
                    // Dont bomb our own shells.
                    if (shell.GetDrawRectangle().Contains(e.location)) return;
                }
                this.paintBomb = new PaintBomb(this, e.location);
            }
        }

        public void OnMouseRelease(MouseEvent e)
        {

        }
    }
}
