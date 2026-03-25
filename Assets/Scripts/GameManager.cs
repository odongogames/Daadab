using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        private Health playerHealth;
        private GameStateMachine gameStateMachine;

        public static Action OnResetGame;
        public static Action OnStartGame;

        private float gameTime;
        public float GetGameTime() => gameTime;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log($"Destroying {this.GetType()} as more than one instance found.");
                Destroy(this);
                return;
            }

            Instance = this;

            // didable fog while editing to improve visibility
            RenderSettings.fog = true;
        }

        private void Start()
        {
            gameStateMachine = GameStateMachine.Instance;
            Assert.IsNotNull(gameStateMachine);

            var truck = Truck.Instance;
            Assert.IsNotNull(truck);

            playerHealth = truck.GetComponent<Health>();
            Assert.IsNotNull(playerHealth);

            playerHealth.OnTakeDamage += Player_OnTakeDamage;

            StartCoroutine(StartGameCO());
        }

        private void Update()
        {
            if (gameStateMachine.GetCurrentState() == GameState.Gameplay)
            {
                gameTime += Time.deltaTime;
            }
        }

        private void OnDestroy()
        {
            playerHealth.OnTakeDamage -= Player_OnTakeDamage;
        }

        private void Player_OnTakeDamage(uint value)
        {
            if (value <= 0)
            {
                gameStateMachine.ChangeGameState(GameState.GameOver);
            }
        }

        private IEnumerator StartGameCO()
        {
            yield return new WaitForEndOfFrame();

            Debug.Log("-----------------------");
            Debug.Log("Start game");

            OnStartGame?.Invoke();
        }

        public void RestartGame()
        {
            Debug.Log("-----------------------");
            Debug.Log("Try restart game");
            OnResetGame?.Invoke();

            StartCoroutine(StartGameCO());
        }
    }
}
