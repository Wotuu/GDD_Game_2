using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MiniGameOverview.Map.Pathing;
using XNAInputHandler.MouseInput;
using PolygonCollision.Polygons;
using CustomLists.Lists;
using MiniGameOverview.Backgrounds.QuadTree;
using MiniGameOverview.Managers;

namespace MiniGameOverview.Backgrounds
{
    public class BackgroundMap : MouseMotionListener
    {
        public Texture2D emptyMapTexture { get; set; }

        public Vector2 location { get; set; }
        public float z { get; set; }
        public Vector2 scale { get; set; }

        public static Texture2D EMPTY_MAP { get; set; }
        public static Texture2D COLORED_MAP { get; set; }

        public PathItem[] paths { get; set; }

        public ColoredBackgroundMap coloredMap { get; set; }
        public Pencil pencil { get; set; }

        static BackgroundMap()
        {
            BackgroundMap.EMPTY_MAP = MiniGameOverviewMainGame.GetInstance().game.Content.Load<Texture2D>("Map/map");
        }


        private StateManager.SelectedGame[] gameSequence = new StateManager.SelectedGame[]{
            StateManager.SelectedGame.MainMenu,
            StateManager.SelectedGame.BalloonPaintBucketGame,
            StateManager.SelectedGame.DigGame,
            StateManager.SelectedGame.SquatBugsGame,
            StateManager.SelectedGame.BuzzBattleGame,
        };

        private Vector3[] pathLocations = new Vector3[]{
            new Vector3( 130, 820, 0.9f), 
            new Vector3( 593, 820, 0.9f),
            new Vector3( 1102, 820, 0.9f),
            new Vector3( 1441, 820, 0.9f),
            new Vector3( 1789, 820, 0.9f)
        };
        private Polygon[] pathDrawPolygons { get; set; }
        private int[] pathDrawPolygonsMaxPixels = new int[5]{
            1,
            1,
            1,
            1,
            1
        };
        /*
        private int[] pathDrawPolygonsMaxPixels = new int[5]{
            350000,
            350000,
            470000,
            220000,
            320000
        };*/

        public BackgroundMap()
        {

            this.emptyMapTexture = BackgroundMap.EMPTY_MAP;

            Viewport viewport = MiniGameOverviewMainGame.GetInstance().game.GraphicsDevice.Viewport;
            this.scale = new Vector2(
                viewport.Width / (float)this.emptyMapTexture.Width,
                viewport.Height / (float)this.emptyMapTexture.Height);
            this.location = new Vector2(viewport.Width / 2, viewport.Height / 2);

            this.paths = new PathItem[5];

            this.InitPathDrawPolygons();
            for (int i = 0; i < this.paths.Length; i++)
            {
                this.paths[i] = new PathItem(this.gameSequence[i], ((i == 0) ? null : this.paths[i - 1]));
                this.paths[i].location = new Vector3(
                    pathLocations[i].X * this.scale.X, 
                    pathLocations[i].Y * this.scale.Y, 
                    pathLocations[i].Z);
                this.paths[i].drawableMapSection = new DrawableMapSection(this.paths[i], this.pathDrawPolygons[i],
                    (int)(pathDrawPolygonsMaxPixels[i] * this.scale.X * this.scale.Y));
                this.paths[i].drawableMapSection.polygon.scale = this.scale;
            }

            /// Main menu
            this.paths[0].isUnlocked = true;
            /// Balloon game
            this.paths[1].isUnlocked = true;
            this.paths[2].isUnlocked = true;
            this.paths[3].isUnlocked = true;

            MiniGameOverviewMainGame.GetInstance().player.TeleportTo(this.paths[0]);

            this.coloredMap = new ColoredBackgroundMap(this);
            this.pencil = new Pencil();

            this.z = 0.89f;

            MouseManager.GetInstance().mouseDragListeners += this.OnMouseDrag;
            MouseManager.GetInstance().mouseMotionListeners += this.OnMouseMotion;
        }

        /// <summary>
        /// Gets the rectangle that you should use to draw this with.
        /// </summary>
        /// <returns>bla</returns>
        public Rectangle GetDrawRectangle()
        {
            return new Rectangle((int)(this.location.X - ((this.emptyMapTexture.Width * this.scale.X) / 2)),
                (int)(this.location.Y - ((this.emptyMapTexture.Height * this.scale.Y) / 2)),
                (int)(this.emptyMapTexture.Width * this.scale.X),
                (int)(this.emptyMapTexture.Height * this.scale.Y));
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this.emptyMapTexture, this.GetDrawRectangle(), null, Color.White, 0f,
                Vector2.Zero, SpriteEffects.None, this.z);

            for (int i = 0; i < this.paths.Length; i++)
            {
                this.paths[i].Draw(sb);
            }

            this.coloredMap.Draw(sb);
            this.pencil.Draw(sb);
        }

        public void OnMouseMotion(MouseEvent e)
        {

        }

        public void OnMouseDrag(MouseEvent e)
        {
            foreach (PathItem item in this.paths)
            {
                if (item.isUnlocked && item.drawableMapSection.polygon != null &&
                    item.drawableMapSection.polygon.IsInside(new Vector2(e.location.X, e.location.Y)))
                {
                    this.coloredMap.ApplyColor(new Vector2(e.location.X, e.location.Y), 25);

                    if (!item.isFullyColored)
                        item.CheckIfFullyColored();
                }
            }
        }

        /// <summary>
        /// Gets a path item by selected game!
        /// </summary>
        /// <param name="game">The game you want to have the pathitem from.</param>
        /// <returns>The path item!</returns>
        public PathItem GetPathByGame(StateManager.SelectedGame game)
        {
            for (int i = 0; i < this.paths.Length; i++)
            {
                PathItem path = this.paths.ElementAt(i);
                if (path.game == game) return path;
            }
            return null;
        }

        #region Init draw polygon locations
        /// <summary>
        /// Inits the draw locations of the polygons
        /// </summary>
        private void InitPathDrawPolygons()
        {
            this.pathDrawPolygons = new Polygon[5];

            CustomArrayList<Vector2> list = new CustomArrayList<Vector2>();
            list.AddLast(new Vector2(0, 0));
            list.AddLast(new Vector2(0, 1080));
            list.AddLast(new Vector2(400, 1080));
            list.AddLast(new Vector2(349, 729));
            list.AddLast(new Vector2(389, 415));
            list.AddLast(new Vector2(273, 0));
            this.pathDrawPolygons[0] = new Polygon(list);

            list = new CustomArrayList<Vector2>();
            list.AddLast(new Vector2(273, 0));
            list.AddLast(new Vector2(389, 415));
            list.AddLast(new Vector2(349, 729));
            list.AddLast(new Vector2(400, 1080));
            list.AddLast(new Vector2(789, 1080));
            list.AddLast(new Vector2(741, 700));
            list.AddLast(new Vector2(743, 241));
            list.AddLast(new Vector2(613, 0));
            this.pathDrawPolygons[1] = new Polygon(list);


            list = new CustomArrayList<Vector2>();
            list.AddLast(new Vector2(613, 0));
            list.AddLast(new Vector2(743, 241));
            list.AddLast(new Vector2(741, 700));
            list.AddLast(new Vector2(789, 1080));
            list.AddLast(new Vector2(1317, 1080));
            list.AddLast(new Vector2(1265, 825));
            list.AddLast(new Vector2(1309, 203));
            list.AddLast(new Vector2(1269, 0));
            this.pathDrawPolygons[2] = new Polygon(list);


            list = new CustomArrayList<Vector2>();
            list.AddLast(new Vector2(1269, 0));
            list.AddLast(new Vector2(1309, 203));
            list.AddLast(new Vector2(1265, 825));
            list.AddLast(new Vector2(1317, 1080));
            list.AddLast(new Vector2(1579, 1080));
            list.AddLast(new Vector2(1575, 795));
            list.AddLast(new Vector2(1523, 0));
            this.pathDrawPolygons[3] = new Polygon(list);

            list = new CustomArrayList<Vector2>();
            list.AddLast(new Vector2(1523, 0));
            list.AddLast(new Vector2(1575, 795));
            list.AddLast(new Vector2(1579, 1080));
            list.AddLast(new Vector2(1920, 1080));
            list.AddLast(new Vector2(1920, 0));

            this.pathDrawPolygons[4] = new Polygon(list);
        }
        #endregion
    }
}
