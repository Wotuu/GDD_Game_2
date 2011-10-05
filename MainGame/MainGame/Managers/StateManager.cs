using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MainGame.Managers;
using BalloonPaintBucketGame;
using SquatBugsGame;
using MiniGameOverview;
using DiggingGame;

namespace MainGame.Managers
{
    public class StateManager
    {

        #region Singleton logic
        private static StateManager instance;

        public static StateManager GetInstance()
        {
            if (instance == null) instance = new StateManager();
            return instance;
        }

        private StateManager()
        {
            // Listen to the game state of the balloon manager
            BalloonPaintBucketGame.Managers.StateManager.GetInstance().onGameStateChangedListeners += this.OnBalloonGameStateChanged;
        }
        #endregion

        private RunningGame game { get; set; }
        private State _state { get; set; }
        private State state
        {
            get
            {
                return _state;
            }
            set
            {
                if (_state != value) this.OnStateChanged(_state, value);
                this._state = value;
            }
        }

        public enum State
        {
            Running,
            Paused,
            Victory,
            Loss,
            Idle,
            MainMenu
        }

        public enum RunningGame
        {
            None,
            BalloonPaintBucketGame,
            SquatBugsGame,
            MiniGameOverview,
            BuzzBattleGame,
            KinectGame,
            DigGame
        }

        /// <summary>
        /// Sets the state of a game.
        /// </summary>
        /// <param name="state">The new state of the game.</param>
        public void SetState(State state)
        {
            if (this.state == State.Paused && state == State.Running)
            {
                // No time stepping for one frame
                GameTimeManager.GetInstance().time_step = 1f;
                GameTimeManager.GetInstance().currentUpdateStartMS = new TimeSpan(DateTime.UtcNow.Ticks).TotalMilliseconds;
            }
            this.state = state;
        }

        /// <summary>
        /// Gets the current state of the game.
        /// </summary>
        /// <returns>The current state of the game.</returns>
        public State GetState()
        {
            return this.state;
        }

        /// <summary>
        /// Sets the currently running game.
        /// </summary>
        /// <param name="game">The game that is currently running.</param>
        public void SetRunningGame(RunningGame game)
        {
            switch (game)
            {
                case RunningGame.BalloonPaintBucketGame:
                    BalloonPaintBucketMainGame.GetInstance().Initialize(Game1.GetInstance());
                    break;
                case RunningGame.SquatBugsGame:
                    SquatBugsMainGame.GetInstance().Initialize(Game1.GetInstance());
                    break;
                case RunningGame.MiniGameOverview:
                    MiniGameOverviewMainGame.GetInstance().Initialize(Game1.GetInstance());
                    MiniGameOverviewMainGame.GetInstance().gameInfoPanel.onGameStartListeners += Game1.GetInstance().OnGameStart;
                    break;
                case RunningGame.DigGame:
                    DiggingMainGame.GetInstance().Initialize(Game1.GetInstance());
                    break;
            }
            this.game = game;
        }

        /// <summary>
        /// Gets the currently running game.
        /// </summary>
        /// <returns>The game! trolololol</returns>
        public RunningGame GetRunningGame()
        {
            return this.game;
        }

        /// <summary>
        /// Called when the state has changed.
        /// </summary>
        /// <param name="old">The old state.</param>
        /// <param name="newState">The new state.</param>
        public void OnStateChanged(State old, State newState)
        {
            switch (this.GetRunningGame())
            {
                case RunningGame.BalloonPaintBucketGame:
                    BalloonPaintBucketGame.Managers.StateManager.GetInstance().SetState(
                        (BalloonPaintBucketGame.Managers.StateManager.State)((int)newState));

                    break;
            }
        }

        /// <summary>
        /// When the game state of the balloon game has changed.
        /// </summary>
        /// <param name="newState">The new state the game is in.</param>
        public void OnBalloonGameStateChanged(BalloonPaintBucketGame.Managers.StateManager.State newState)
        {
            switch (newState)
            {
                case BalloonPaintBucketGame.Managers.StateManager.State.Loss:
                    Game1.GetInstance().GameLost();
                    break;

                case BalloonPaintBucketGame.Managers.StateManager.State.Victory:
                    Game1.GetInstance().GameWon();
                    break;
            }
        }
    }
}
