using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using DiggingGame.SandBoard.DigObjects;
using DiggingGame.Managers;

namespace DiggingGame.SandBoard
{
    public class Board
    {
        public List<SandTile> Tiles = new List<SandTile>();
        Viewport viewport;
        #region Win Detection
        int nrGoodObjectsFound = 0;
        int nrBadObjectsFound = 0;
        #endregion

        #region Textures
        public List<Texture2D> SandTiles = new List<Texture2D>();
        public List<Texture2D> DigTiles = new List<Texture2D>();
        public List<Texture2D> SchelpTextures = new List<Texture2D>();
        public List<Texture2D> EvilDigTextures = new List<Texture2D>();
        #endregion

        #region DigObjects
        public List<DigObject> DigObjects = new List<DigObject>();
        #endregion



        public Board(int width,int height)
        {
            viewport = DiggingMainGame.GetInstance().game.GraphicsDevice.Viewport;
            Tiles = new List<SandTile>();
            FillDigObjectsTextureList();
            FillSandTiles();
            FillDigTiles();
            InitTiles(width, height);
            FillTilesWithRandomDigObject();
        }   

        #region Init
        public void InitTiles(int width, int height)
        {
            // We willen een 4 x 3 grid hebben 
            //int OffSetX = (int)(viewport.Width * 0.9);
            int OffSetX = (int)(viewport.Width);
            int TileWidth = OffSetX / 4;
            //int OffSetY = (int)(viewport.Height * 0.8);
            int OffSetY = (int)(viewport.Height);
            int TileHeight = OffSetY / 3;

            Random random = new Random();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Rectangle drawrect = new Rectangle(((viewport.Width - OffSetX) / 2) + (x *TileWidth),(viewport.Height - OffSetY) + (y * TileHeight),TileWidth,TileHeight);
                    Rectangle digdrawrect = drawrect;
                    int rand = random.Next(SandTiles.Count );
                    Tiles.Add(new SandTile(drawrect, SandTiles[rand],digdrawrect));
                }
            }

        }

        public void FillTilesWithRandomDigObject()
        {
            Random rand = new Random();
            foreach (SandTile tile in Tiles )
            {
                //Alles vullen met schelpen
                Rectangle DigRect = new Rectangle(tile.DrawRectangle.X + tile.DrawRectangle.Width / 4, tile.DrawRectangle.Y + tile.DrawRectangle.Height / 4,tile.DrawRectangle.Width / 2, tile.DrawRectangle.Height / 2);
                tile.DigObject = new Schelp(DigRect, SchelpTextures[rand.Next(SchelpTextures.Count)]);
            }

            // Nu random badguys neerzetten
            for (int i = 0; i < 4; i++)
            {
                int tilenr = rand.Next(Tiles.Count);
                while ((Tiles[tilenr].DigObject is BadGuy))
                {
                    tilenr = rand.Next(Tiles.Count);
                }

                Rectangle DigRect = new Rectangle(Tiles[tilenr].DrawRectangle.X + Tiles[tilenr].DrawRectangle.Width / 4, Tiles[tilenr].DrawRectangle.Y + Tiles[tilenr].DrawRectangle.Height / 4,Tiles[tilenr].DrawRectangle.Width / 2, Tiles[tilenr].DrawRectangle.Height / 2);
                Tiles[tilenr].DigObject = new BadGuy(DigRect, EvilDigTextures[rand.Next(EvilDigTextures.Count)]);
            }
        }

        public void FillDigObjectsTextureList()
        {
            SchelpTextures.Add(DiggingMainGame.GetInstance().game.Content.Load<Texture2D>("DigObjects/schelp_geel"));
            SchelpTextures.Add(DiggingMainGame.GetInstance().game.Content.Load<Texture2D>("DigObjects/schelp_blauw"));
            //SchelpTextures.Add(DiggingMainGame.GetInstance().game.Content.Load<Texture2D>("DigObjects/schelp_groen"));
            SchelpTextures.Add(DiggingMainGame.GetInstance().game.Content.Load<Texture2D>("DigObjects/schelp_roze"));

            EvilDigTextures.Add(DiggingMainGame.GetInstance().game.Content.Load<Texture2D>("DigObjects/krab"));
            EvilDigTextures.Add(DiggingMainGame.GetInstance().game.Content.Load<Texture2D>("DigObjects/schorpion"));
        }

        public void FillSandTiles()
        {
            SandTiles.Add(DiggingMainGame.GetInstance().game.Content.Load<Texture2D>("SandTiles/zand_1"));
            SandTiles.Add(DiggingMainGame.GetInstance().game.Content.Load<Texture2D>("SandTiles/zand_2"));
            SandTiles.Add(DiggingMainGame.GetInstance().game.Content.Load<Texture2D>("SandTiles/zand_3"));
        }

        public void FillDigTiles()
        {
            DigTiles.Add(DiggingMainGame.GetInstance().game.Content.Load<Texture2D>("SandTiles/kuil_1_transp"));
            DigTiles.Add(DiggingMainGame.GetInstance().game.Content.Load<Texture2D>("SandTiles/kuil_2_transp"));
            DigTiles.Add(DiggingMainGame.GetInstance().game.Content.Load<Texture2D>("SandTiles/kuil_3_transp"));
        }
        #endregion

        #region Draw
        public void Draw(SpriteBatch sb)
        {
            foreach (SandTile tile in Tiles)
            {
                tile.Draw(sb);
            }
        }


        #endregion

        #region Update
        public void Update()
        {
            nrGoodObjectsFound = 0;
            nrBadObjectsFound = 0;
            foreach (SandTile tile in Tiles)
            {
                tile.Update();
                if (StateManager.GetInstance().GetState() == StateManager.State.Running)
                {
                    CheckWin(tile);
                }
                
            }
        }
        #endregion

        public void CheckWin(SandTile tile)
        {
            if(tile.DigStatus == DiggingGame.SandBoard.SandTile.DigDepth.DugThrice){
                if (tile.DigObject is Schelp)
                {
                    nrGoodObjectsFound++;
                }
                else
                {
                    nrBadObjectsFound++;
                }
            
            }
            if (nrGoodObjectsFound == 8)
            {
                //WIN
                StateManager.GetInstance().SetState(StateManager.State.Victory);
                return;
            }
            else if(nrBadObjectsFound == 3)
            {
                //LOSS
                StateManager.GetInstance().SetState(StateManager.State.Loss);
               
            }
            
        }
    }
}
