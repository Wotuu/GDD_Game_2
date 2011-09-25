using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        private StateManager() { }
        #endregion

        private RunningGame game { get; set; }
        private State state { get; set; }

        public enum State
        {
            Idle,
            Running,
            Paused,
            Victory,
            Loss
        }

        public enum RunningGame
        {
            None,
            BalloonPaintBucketGame,
            SquatBugsGame
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
    }
}
