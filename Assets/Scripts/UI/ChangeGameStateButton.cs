using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class ChangeGameStateButton : ButtonBase
    {
        [SerializeField] private GameState gameState;

        private GameStateMachine gameStateMachine;

        public override void Awake()
        {
            base.Awake();

            Assert.IsTrue(gameState != GameState.None);
        }

        public override void Start()
        {
            gameStateMachine = GameStateMachine.Instance;
            Assert.IsNotNull(gameStateMachine);

            base.Start();
        }

        public override void OnClick()
        {
            base.OnClick();

            gameStateMachine.ChangeGameState(gameState);
        }
    }
}
