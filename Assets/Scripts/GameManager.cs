using System;
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
        }

        private void OnDestroy()
        {
            playerHealth.OnTakeDamage -= Player_OnTakeDamage;
        }

        private void Player_OnTakeDamage(uint value)
        {
            if (value <= 0)
            {
                Debug.Log($"Player is dead");
                gameStateMachine.ChangeGameState(GameState.GameOver);
            }
        }

        public void RestartGame()
        {
            Debug.Log("-----------------------");
            Debug.Log("Try restart game");
            OnResetGame?.Invoke();
        }
    }
}
