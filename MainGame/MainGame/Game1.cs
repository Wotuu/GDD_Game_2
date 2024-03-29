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
using KinectTest;
using SquatBugsGame;
using MiniGameOverview;
using DiggingGame;
using MainGame.Backgrounds;
using MainGame.Cards;
using ParticleEngine;
using MainGame.Backgrounds.Birds;
using MainGame.Media;
using MainGame.UI;
using BuzzBattle;

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

        /*
        public IntroMoviePanel introMoviePanel { get; set; }
        public GameStartMoviePanel gameStartMoviePanel { get; set; }
         */

        public SimpleMoviePlayer introMoviePlayer { get; set; }
        public SimpleMoviePlayer gameStartMoviePlayer { get; set; }
        public SimpleMoviePlayer finalMovieMoviePlayer { get; set; }

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
            this.IsFixedTimeStep = true;
            instance = this;

            this.graphics.PreferredBackBufferWidth = 1024;
            this.graphics.PreferredBackBufferHeight = 768;
            this.graphics.IsFullScreen = false;
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
            //BalloonPaintBucketMainGame.GetInstance().Initialize(this);
            //SquatBugsMainGame.GetInstance().Initialize(this);
            //KinectTestMainGame.GetInstance().Initialize(this);



            MenuManager.GetInstance().ShowMenu(MenuManager.Menu.MainMenu);
            StateManager.GetInstance().SetState(StateManager.State.MainMenu);
            KeyboardManager.GetInstance().keyPressedListeners += this.OnKeyPressed;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            GameTimeManager.GetInstance().OnStartUpdate();
            // Create a new SpriteBatch, which can be used to draw textures.
            sb = new SpriteBatch(GraphicsDevice);
            SpriteFont font = Content.Load<SpriteFont>("Fonts/Arial");
            ChildComponent.DEFAULT_FONT = font;

            // this.introMoviePanel = new IntroMoviePanel();

            this.introMoviePlayer = new SimpleMoviePlayer(Content.Load<Video>("Media/Video/gameintro"));

            // TODO: use this.Content to load your game content here
            DrawUtil.lineTexture = DrawUtil.GetClearTexture2D(sb);
            this.background = new MainGameBackground();

            ParticleManager.DEFAULT_TEXTURE = this.Content.Load<Texture2D>("Particles/default");

            // new GameResultCard(GameResultCard.CardColor.LostCard);
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

            if (this.introMoviePlayer != null &&
                this.introMoviePlayer.videoPlayer.IsPlaying())
            {
                // That's it.
                base.Update(gameTime);
                return;
            }
            else if (this.gameStartMoviePlayer != null &&
                this.gameStartMoviePlayer.videoPlayer.IsPlaying())
            {
                // That's it.
                base.Update(gameTime);
                return;
            }
            // Updates all interface components
            ComponentManager.GetInstance().Update();

            /*
            if (this.introMoviePanel != null &&
                this.introMoviePanel.videoPlayer.IsPlaying())
            {
                // That's it.
                base.Update(gameTime);
                return;
            }
            else if (this.gameStartMoviePanel != null &&
                this.gameStartMoviePanel.videoPlayer.IsPlaying())
            {
                // That's it.
                base.Update(gameTime);
                return;
            }*/

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
                            case StateManager.RunningGame.KinectGame:
                                KinectTestMainGame.GetInstance().Update();
                                break;
                            case StateManager.RunningGame.DigGame:
                                DiggingMainGame.GetInstance().Update();
                                break;
                            case StateManager.RunningGame.BuzzBattleGame:
                                BuzzBattleMainGame.GetInstance().Update();
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


            if (this.introMoviePlayer != null &&
                this.introMoviePlayer.videoPlayer.IsPlaying())
            {
                GraphicsDevice.Clear(Color.Black);
                GameTimeManager.GetInstance().OnStartDraw();

                sb.Begin(SpriteSortMode.BackToFront, null);

                // That's it.
                // ComponentManager.GetInstance().Draw(sb);
                this.introMoviePlayer.Draw(sb);
                sb.End();
                base.Draw(gameTime);
                return;
            }
            else if (this.gameStartMoviePlayer != null &&
                    this.gameStartMoviePlayer.videoPlayer.IsPlaying())
            {
                GraphicsDevice.Clear(Color.Black);
                GameTimeManager.GetInstance().OnStartDraw();

                sb.Begin(SpriteSortMode.BackToFront, null);

                // That's it.
                // ComponentManager.GetInstance().Draw(sb);
                this.gameStartMoviePlayer.Draw(sb);
                sb.End();
                base.Draw(gameTime);
                return;
            }
            else if (this.finalMovieMoviePlayer != null &&
              this.finalMovieMoviePlayer.videoPlayer.IsPlaying())
            {
                GraphicsDevice.Clear(Color.Black);
                GameTimeManager.GetInstance().OnStartDraw();

                sb.Begin(SpriteSortMode.BackToFront, null);

                // That's it.
                // ComponentManager.GetInstance().Draw(sb);
                this.finalMovieMoviePlayer.Draw(sb);
                sb.End();
                base.Draw(gameTime);
                return;
            }

            GraphicsDevice.Clear(new Color(188, 215, 237));
            GameTimeManager.GetInstance().OnStartDraw();

            sb.Begin(SpriteSortMode.BackToFront, null);


            /*
            if (this.introMoviePanel != null &&
                this.introMoviePanel.videoPlayer.IsPlaying())
            {
                // That's it.
                ComponentManager.GetInstance().Draw(sb);
                sb.End();
                base.Draw(gameTime);
                return;
            }
            else if (this.gameStartMoviePanel != null &&
                this.gameStartMoviePanel.videoPlayer.IsPlaying())
            {
                // That's it.
                ComponentManager.GetInstance().Draw(sb);
                sb.End();
                base.Draw(gameTime);
                return;
            }*/

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
                case StateManager.RunningGame.KinectGame:
                    KinectTestMainGame.GetInstance().Draw(sb);
                    break;
                case StateManager.RunningGame.MiniGameOverview:
                    MiniGameOverviewMainGame.GetInstance().Draw(sb);
                    break;
                case StateManager.RunningGame.DigGame:
                    DiggingMainGame.GetInstance().Draw(sb);
                    break;
                case StateManager.RunningGame.BuzzBattleGame:
                    BuzzBattleMainGame.GetInstance().Draw(sb);
                    break;
            }

            CardManager.GetInstance().Draw(sb);
            // Draws all interface components
            ComponentManager.GetInstance().Draw(sb);

            ParticleManager.GetInstance().Draw(sb);

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
                case MiniGameOverview.Managers.StateManager.SelectedGame.MainMenu:
                    MenuManager.GetInstance().ShowMenu(MenuManager.Menu.MainMenu);
                    StateManager.GetInstance().SetRunningGame(StateManager.RunningGame.None);
                    StateManager.GetInstance().SetState(StateManager.State.MainMenu);
                    break;
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
                case MiniGameOverview.Managers.StateManager.SelectedGame.DigGame:
                    MenuManager.GetInstance().ShowMenu(MenuManager.Menu.NoMenu);
                    StateManager.GetInstance().SetRunningGame(StateManager.RunningGame.DigGame);
                    StateManager.GetInstance().SetState(StateManager.State.Running);
                    break;
            }
        }

        /// <summary>
        /// Call this when the game is won!
        /// </summary>
        public void GameWon()
        {
            switch (StateManager.GetInstance().GetRunningGame())
            {
                case StateManager.RunningGame.BalloonPaintBucketGame:
                    new GameResultCard(GameResultCard.CardColor.Pink);
                    break;
                case StateManager.RunningGame.SquatBugsGame:
                    new GameResultCard(GameResultCard.CardColor.Blue);
                    break;
                case StateManager.RunningGame.DigGame:
                    new GameResultCard(GameResultCard.CardColor.Yellow, GameResultCard.CardPosition.Left);
                    new GameResultCard(GameResultCard.CardColor.Green, GameResultCard.CardPosition.Right);
                    break;
                case StateManager.RunningGame.BuzzBattleGame:
                    Game1.GetInstance().finalMovieMoviePlayer = new SimpleMoviePlayer(
                        Game1.GetInstance().Content.Load<Video>("Media/Video/finalmovie"));
                    Game1.GetInstance().finalMovieMoviePlayer.videoPlayer.fadeOutAfterMS = 54000;
                    Game1.GetInstance().finalMovieMoviePlayer.videoPlayer.fadeOutDurationMS = 2000;
                    Game1.GetInstance().finalMovieMoviePlayer.videoPlayer.onVideoStoppedPlayingListeners +=
                        this.OnGameCompleted;


                    // PLAY MOVIE
                    break;
            }
        }

        /// <summary>
        /// Call this when the game is lost!
        /// </summary>
        public void GameLost()
        {
            if (StateManager.GetInstance().GetRunningGame() == StateManager.RunningGame.BuzzBattleGame)
            {
                new GameResultCard(GameResultCard.CardColor.BuzzLostCard);
            }
            else
            {
                new GameResultCard(GameResultCard.CardColor.LostCard);
            }
        }


        /// <summary>
        /// Orders the game to move back to the mini game overview
        /// </summary>
        public void BackToMiniGameOverview()
        {
            //Unload Mousehandlers
            switch (StateManager.GetInstance().GetRunningGame())
            {
                case StateManager.RunningGame.BalloonPaintBucketGame:
                    BalloonPaintBucketMainGame.GetInstance().CleanUp();
                    BalloonPaintBucketGame.Managers.StateManager.GetInstance().
                        SetState(BalloonPaintBucketGame.Managers.StateManager.State.Paused);
                    break;

                case StateManager.RunningGame.SquatBugsGame:
                    SquatBugsMainGame.GetInstance().CleanUp();
                    SquatBugsGame.Managers.StateManager.GetInstance().
                        SetState(SquatBugsGame.Managers.StateManager.State.Paused);
                    break;

                case StateManager.RunningGame.DigGame:
                    DiggingMainGame.GetInstance().CleanUp();
                    DiggingGame.Managers.StateManager.GetInstance().
                        SetState(DiggingGame.Managers.StateManager.State.Paused);
                    break;

                case StateManager.RunningGame.BuzzBattleGame:
                    //BuzzBattleMainGame.GetInstance().CleanUp();
                    BuzzBattle.Managers.StateManager.GetInstance().
                        SetState(BuzzBattle.Managers.StateManager.State.Paused);
                    break;

            }
            MediaPlayer.Stop();
            StateManager.GetInstance().SetRunningGame(StateManager.RunningGame.MiniGameOverview);
            MiniGameOverviewMainGame.GetInstance().OnShow();
        }

        /// <summary>
        /// When the game is completed, bounce the player back to the mini game overview.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="panel"></param>
        public void OnGameCompleted(XNAVideoPlayer source, MovieControlPanel panel)
        {
            StateManager.GetInstance().SetRunningGame(StateManager.RunningGame.MiniGameOverview);
            MiniGameOverviewMainGame.GetInstance().OnShow();

            BuzzBattleMainGame.GetInstance().Unload();
        }
    }
}
