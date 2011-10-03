using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using PolygonCollision.Util;
using PolygonCollision.Managers;
using PolygonCollision.Polygons;
using CustomLists.Lists;
using XNAInputHandler.MouseInput;
using XNAInputLibrary.KeyboardInput;
using XNAInterfaceComponents.ChildComponents;
using XNAInterfaceComponents.AbstractComponents;
using XNAInterfaceComponents.Managers;
using XNAInterfaceComponents.Components;
using MainGame.Managers;
using BalloonPaintBucketGame;
using BalloonPaintBucketGame.Balloons;
using BalloonPaintBucketGame.Players;
using SquatBugsGame;
using MiniGameOverview;
using MainGame.Backgrounds;
using MainGame.Cards;
using ParticleEngine;
using MainGame.Backgrounds.Birds;

namespace MainGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game, MouseMotionListener, KeyboardListener
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch sb;

        public XNALabel displayLbl { get; set; }
        public MainGameBackground background { get; set; }

        private static Game1 instance { get; set; }
        public static Game1 GetInstance()
        {
            return instance;
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.IsMouseVisible = true;
            this.IsFixedTimeStep = false;
            instance = this;

            this.graphics.PreferredBackBufferWidth = 1024;
            this.graphics.PreferredBackBufferHeight = 768;
            this.graphics.ApplyChanges();

            this.InactiveSleepTime = new System.TimeSpan(0);

            MouseManager.GetInstance().mouseMotionListeners += this.OnMouseMotion;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();

            XNAPanel parent = new XNAPanel(null, new Rectangle(0, 0, 1024, 768));
            parent.backgroundColor = Color.Transparent;
            parent.border = null;
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.ApplyChanges();
            //this.displayLbl = new XNALabel(parent, new Rectangle(5, 5, 200, 20), "");
            //this.displayLbl.backgroundColor = Color.Transparent;

            MenuManager.GetInstance().ShowMenu(MenuManager.Menu.MainMenu);
            StateManager.GetInstance().SetState(StateManager.State.MainMenu);
            KeyboardManager.GetInstance().keyPressedListeners += this.OnKeyPressed;

            new BirdFlock(BirdFlock.FlyDirection.LeftToRight, 2);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            sb = new SpriteBatch(GraphicsDevice);
            SpriteFont font = Content.Load<SpriteFont>("Fonts/Arial");
            ChildComponent.DEFAULT_FONT = font;

            // TODO: use this.Content to load your game content here
            DrawUtil.lineTexture = DrawUtil.GetClearTexture2D(sb);
            this.background = new MainGameBackground();

            ParticleManager.DEFAULT_TEXTURE = this.Content.Load<Texture2D>("Particles/default");

            new WinningCard(WinningCard.CardColor.Green);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            GameTimeManager.GetInstance().OnStartUpdate();

            MouseManager.GetInstance().Update(this);
            KeyboardManager.GetInstance().Update(Keyboard.GetState());
            // Updates all interface components
            ComponentManager.GetInstance().Update();

            CardManager.GetInstance().Update();

            // TODO: Add your update logic here

            StateManager sm = StateManager.GetInstance();
            switch (sm.GetState())
            {
                case StateManager.State.MainMenu:
                    {
                        this.background.Update();

                        BirdManager.GetInstance().UpdateBirds();
                        break;
                    }
                case StateManager.State.Idle:
                    {
                        break;
                    }
                case StateManager.State.Running:
                    {
                        switch (StateManager.GetInstance().GetRunningGame())
                        {
                            case StateManager.RunningGame.BalloonPaintBucketGame:
                                BalloonPaintBucketMainGame.GetInstance().Update();
                                break;
                            case StateManager.RunningGame.SquatBugsGame:
                                SquatBugsMainGame.GetInstance().Update();
                                break;
                            case StateManager.RunningGame.MiniGameOverview:
                                MiniGameOverviewMainGame.GetInstance().Update();
                                break;

                        }
                        break;
                    }
                case StateManager.State.Paused:
                    {

                        break;
                    }
                case StateManager.State.Loss:
                    {

                        break;
                    }
                case StateManager.State.Victory:
                    {

                        break;
                    }
            }

            ParticleManager.GetInstance().Update((float)GameTimeManager.GetInstance().time_step);
            // SquatBugsMainGame.GetInstance().Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(188, 215, 237));
            GameTimeManager.GetInstance().OnStartDraw();

            sb.Begin(SpriteSortMode.BackToFront, null);
            // Draws all interface components
            ComponentManager.GetInstance().Draw(sb);

            ParticleManager.GetInstance().Draw(sb);

            CardManager.GetInstance().Draw(sb);

            // PolygonManager.GetInstance().DrawPolygons(spriteBatch);
            // TODO: Add your drawing code here

            switch (StateManager.GetInstance().GetRunningGame())
            {
                case StateManager.RunningGame.None:
                    this.background.Draw(sb);

                    BirdManager.GetInstance().DrawBirds(sb);
                    break;
                case StateManager.RunningGame.BalloonPaintBucketGame:
                    BalloonPaintBucketMainGame.GetInstance().Draw(sb);
                    break;
                case StateManager.RunningGame.SquatBugsGame:
                    SquatBugsMainGame.GetInstance().Draw(sb);
                    break;
                case StateManager.RunningGame.MiniGameOverview:
                    MiniGameOverviewMainGame.GetInstance().Draw(sb);
                    break;
            }
            // SquatBugsMainGame.GetInstance().Draw(spriteBatch);
            sb.End();

            base.Draw(gameTime);
        }



        public void OnMouseMotion(MouseEvent e)
        {
            /*
            if (PolygonManager.GetInstance().polygons.Count() > 0)
                this.displayLbl.text = "Inside: " + PolygonManager.GetInstance().polygons.GetFirst().IsInside(
                new Vector2(e.location.X, e.location.Y));*/
        }

        public void OnMouseDrag(MouseEvent e)
        {

        }

        public void OnKeyPressed(KeyEvent e)
        {
            StateManager sm = StateManager.GetInstance();
            if (sm.GetRunningGame() != StateManager.RunningGame.None && e.key == Keys.Escape)
            {
                if (sm.GetState() != StateManager.State.Paused)
                {
                    MenuManager.GetInstance().ShowMenu(MenuManager.Menu.IngameMenu);
                    StateManager.GetInstance().SetState(StateManager.State.Paused);
                }
                else
                {
                    MenuManager.GetInstance().ShowMenu(MenuManager.Menu.NoMenu);
                    StateManager.GetInstance().SetState(StateManager.State.Running);
                }
            }
        }

        public void OnKeyTyped(KeyEvent e)
        {

        }

        public void OnKeyReleased(KeyEvent e)
        {

        }

        public void OnGameStart(MiniGameOverview.Managers.StateManager.SelectedGame game)
        {
            switch (game)
            {
                case MiniGameOverview.Managers.StateManager.SelectedGame.BalloonPaintBucketGame:

                    MenuManager.GetInstance().ShowMenu(MenuManager.Menu.NoMenu);
                    StateManager.GetInstance().SetRunningGame(StateManager.RunningGame.BalloonPaintBucketGame);
                    StateManager.GetInstance().SetState(StateManager.State.Running);
                    break;
                case MiniGameOverview.Managers.StateManager.SelectedGame.SquatBugsGame:

                    MenuManager.GetInstance().ShowMenu(MenuManager.Menu.NoMenu);
                    StateManager.GetInstance().SetRunningGame(StateManager.RunningGame.SquatBugsGame);
                    StateManager.GetInstance().SetState(StateManager.State.Running);
                    break;
                case MiniGameOverview.Managers.StateManager.SelectedGame.BuzzBattleGame:

                    MenuManager.GetInstance().ShowMenu(MenuManager.Menu.NoMenu);
                    StateManager.GetInstance().SetRunningGame(StateManager.RunningGame.BuzzBattleGame);
                    StateManager.GetInstance().SetState(StateManager.State.Running);
                    break;
            }
        }
    }
}
