﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SquatBugsGame.Managers;

public delegate void OnSquatBugsGameStateChanged(StateManager.State newState);
namespace SquatBugsGame.Managers
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

        private State state { get; set; }
        public OnSquatBugsGameStateChanged onGameStateChangedListeners { get; set; }

        public enum State
        {
            Paused,
            Running,
            Victory,
            Loss
        }

        /// <summary>
        /// Sets the state of a game.
        /// </summary>
        /// <param name="state">The new state of the game.</param>
        public void SetState(State state)
        {
            this.state = state;
            if (this.onGameStateChangedListeners != null)
                onGameStateChangedListeners(state);
        }

        /// <summary>
        /// Gets the current state of the game.
        /// </summary>
        /// <returns>The current state of the game.</returns>
        public State GetState()
        {
            return this.state;
        }
    }
}
