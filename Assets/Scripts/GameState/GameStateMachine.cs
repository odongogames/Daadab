using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class GameStateMachine : MonoBehaviour
    {
        public static GameStateMachine Instance;

        [SerializeField] private bool startAutomatically;
        [SerializeField] private GameState initialGameState;

        [Header("Runtime Only")]
        [SerializeField] private GameState currentGameState;

        [SerializeField] private List<GameStateController> gameStateControllers = new List<GameStateController>();

        // TODO: Move this to game manager
        private float gameTime;

        public GameState GetCurrentState() => currentGameState;
        public float GetGameTime() => gameTime;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("GameStateMachine already exists. Destroying self...");
                Destroy(gameObject);
                return;
            }

            Instance = this;

            Assert.IsTrue(initialGameState != GameState.None);

            gameStateControllers = new List<GameStateController>(GetComponentsInChildren<GameStateController>());

            foreach (var controller in gameStateControllers)
            {
                controller.SetStateMachine(this);
            }

            if (startAutomatically)
            {
                StartCoroutine(InitialiseCO());
            }
            else
            {
                GameManager.OnStartGame += GameManager_OnStartGame;
            }
        }

        private void OnDestroy()
        {
            GameManager.OnStartGame -= GameManager_OnStartGame;
        }

        private void GameManager_OnStartGame()
        {
            StartCoroutine(InitialiseCO());
        }

        private IEnumerator InitialiseCO()
        {
            yield return new WaitForEndOfFrame();
            
            Debug.Log($"Initialise state machine:{initialGameState}");

            ChangeGameState(initialGameState);
        }

        private void Update()
        {
            if (currentGameState == GameState.Gameplay)
            {
                gameTime += Time.deltaTime;
            }
        }

        public bool IsGamePaused()
        {
            return GetCurrentState() == GameState.Pause;
        }

        public void AddGameStateSubscriber(GameStateSubscriber subscriber)
        {
            if (TryGetGameStateController(subscriber.GetActiveState(), out GameStateController controller))
            {
                controller.AddSubscriber(subscriber);
            }
        }

        public void RemoveGameStateSubscriber(GameStateSubscriber subscriber)
        {
            if (TryGetGameStateController(subscriber.GetActiveState(), out GameStateController controller))
            {
                controller.RemoveSubscriber(subscriber);
            }
        }

        private bool TryGetGameStateController(GameState state, out GameStateController controller)
        {
            if (state == GameState.None)
            {
                // Debug.Log($"None state cannot be controlled!");
                controller = default;
                return false;
            }

            foreach (var item in gameStateControllers)
            {
                if (item.GetActiveState() == state)
                {
                    controller = item;
                    return true;
                }
            }

            controller = default;
            return false;
        }

        public void ChangeGameState(GameState gameState)
        {
            if (currentGameState == gameState)
            {
                Debug.Log("Current state: " + currentGameState + " is same as new state: " + gameState);
                return;
            }

            foreach (var controller in gameStateControllers)
            {
                if (controller.GetActiveState() == gameState)
                {
                    if (!controller.AllowTransition(currentGameState))
                    {
                        Debug.LogWarning($"{controller.name} does not allow transition from: {currentGameState}");
                        return;
                    }
                }
            }

            foreach (var controller in gameStateControllers)
            {
                controller.ChangeState(gameState, currentGameState);
            }

            currentGameState = gameState;

            Debug.Log($"Change game state to <color=yellow>{currentGameState}</color>");
        }
    }
}
