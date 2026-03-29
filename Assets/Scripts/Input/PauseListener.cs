using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class PauseListener : GameStateSubscriber
    {
        private GameManager gameManager;
        private InputReader inputReader;

        public override void Awake()
        {
            base.Awake();

            gameManager = GameManager.Instance;
            Assert.IsNotNull(gameManager);
        }

        private void Start()
        {
            inputReader = InputReader.Instance;
            Assert.IsNotNull(inputReader);
        }

        public override void EnterActiveState()
        {
            base.EnterActiveState();

            enabled = true;
        }

        public override void ExitActiveState()
        {
            base.ExitActiveState();

            enabled = false;
        }

        private void Update()
        {
            if (inputReader.StartEscape()) gameManager.TogglePause();
        }
    }
}