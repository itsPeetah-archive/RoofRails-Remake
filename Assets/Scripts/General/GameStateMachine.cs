using System;
using UnityEngine;

namespace RoofRails
{
    public class GameStateMachine : MonoBehaviour
    {
        private static GameStateMachine main;
        public static GameStateMachine Main => main;

        private GameState currentState = GameState.Menu;
        public GameState State => currentState;

        public bool GameOver => State == GameState.GameLost || State == GameState.GameWon;

        public event Action onGameStarted;
        public event Action onGamePause;
        public event Action onGameUnpause;
        public event Action onGameLost;
        public event Action onGameWon;
        public event Action onGameReset;
        public event Action onStateChanged;

        private void Awake()
        {
            if (main != null) Destroy(gameObject);
            else
            {
                main = this;
            }
        }

        public void TriggerState(GameState nextState)
        {
            onStateChanged?.Invoke();
            switch (nextState)
            {
                case GameState.Menu: onGameReset?.Invoke(); break;
                case GameState.Playing: (currentState == GameState.Menu ? onGameStarted : onGameUnpause)?.Invoke(); break;
                case GameState.GameLost: onGameLost?.Invoke(); break;
                case GameState.GameWon: onGameWon?.Invoke(); break;
                case GameState.Paused: onGamePause?.Invoke(); break;
            }

            currentState = nextState;
        }

        public void TogglePause() => TriggerState(currentState == GameState.Paused ? GameState.Playing : GameState.Paused);
    }

    public enum GameState
    {
        Menu,
        Playing,
        Paused,
        GameLost,
        GameWon,
    }
}