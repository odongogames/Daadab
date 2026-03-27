using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        private Health playerHealth;
        private GameStateMachine gameStateMachine;
        private Registry registry;

        public static Action OnResetGame;
        public static Action OnSetupGame;
        public static Action OnStartGame;
        public static Action OnStartIntroConversation;
        public static Action OnFinishGame;

        /// <summary>
        /// How many times has the game been run?
        /// </summary>
        private uint runCount;
        public uint RunCount => runCount;

        private float gameTime;
        public float GetGameTime() => gameTime;

        private float timeLeft;
        public float GetTimeLeft() => timeLeft;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log($"Destroying {this.GetType()} as more than one instance found.");
                Destroy(this);
                return;
            }

            Instance = this;
        }

        private IEnumerator Start()
        {
            registry = Registry.Instance;
            Assert.IsNotNull(registry);

            RenderSettings.fog = registry.UseFog;

            gameStateMachine = GameStateMachine.Instance;
            Assert.IsNotNull(gameStateMachine);

            var truck = Truck.Instance;
            Assert.IsNotNull(truck);

            playerHealth = truck.GetComponent<Health>();
            Assert.IsNotNull(playerHealth);

            playerHealth.OnTakeDamage += Player_OnTakeDamage;

            runCount++;

            Debug.Log("-----------------------");
            Debug.Log($"Init game. Run count: {runCount}");

            if (runCount == 1)
            {
                TextSequenceRunner.Instance.SetIntroTextSequence();

                OnStartIntroConversation?.Invoke();

                yield return new WaitForSeconds(1f);

                gameStateMachine.ChangeGameState(GameState.Conversation);
            }
            else
            {
                StartCoroutine(SetupGameCO());
            }

        }

        private IEnumerator SetupGameCO()
        {
            yield return new WaitForEndOfFrame();

            Debug.Log("-----------------------");
            Debug.Log("Setup game");

            // TODO: Find out why registry.TotalGameTime isn't working
            timeLeft = registry.TotalGameTime;
            
            Debug.Log($"Game time: {timeLeft}");

            OnSetupGame?.Invoke();

            StartCoroutine(StartGameCO());
        }

        private IEnumerator StartGameCO()
        {
            yield return new WaitForEndOfFrame();

            Debug.Log("-----------------------");
            Debug.Log("Start game");

            OnStartGame?.Invoke();
        }

        private void Update()
        {
            if (gameStateMachine.GetCurrentState() == GameState.Gameplay)
            {
                gameTime += Time.deltaTime;
                timeLeft -= Time.deltaTime;

                if (timeLeft <= 0)
                {
                    Debug.Log("Time is up!");
                    GameOver();
                }
            }
        }

        public void FinishIntroSequence()
        {
            StartCoroutine(SetupGameCO());
        }

        private void OnDestroy()
        {
            playerHealth.OnTakeDamage -= Player_OnTakeDamage;
        }

        private void Player_OnTakeDamage(uint value)
        {
            if (value <= 0)
            {
                Debug.Log("Player take damage");
                GameOver();
            }
        }

        private void GameOver()
        {
            gameStateMachine.ChangeGameState(GameState.GameOver);
        }

        public void RestartGame()
        {
            Debug.Log("-----------------------");
            Debug.Log("Try restart game");
            OnResetGame?.Invoke();

            StartCoroutine(SetupGameCO());
        }

        public void FinishGame()
        {
            Debug.Log("Player has finished game");
            OnFinishGame?.Invoke();

            StartCoroutine(FinishGameCO());
        }

        private IEnumerator FinishGameCO()
        {
            yield return new WaitForSeconds(1);

            Debug.Log("Mission complete");
            Debug.Log("-----------------------");

            gameStateMachine.ChangeGameState(GameState.MissionComplete);
        }

        public void TogglePause()
        {
            if (gameStateMachine.GetCurrentState() == GameState.Gameplay)
            {
                DOTween.PauseAll();
                gameStateMachine.ChangeGameState(GameState.Pause);
            }
            else if (gameStateMachine.GetCurrentState() == GameState.Pause)
            {
                DOTween.PlayAll();
                gameStateMachine.ChangeGameState(GameState.Gameplay);
            }
        }

    }
}
