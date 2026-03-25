using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class RestartGameButton : ButtonBase
    {
        private GameManager gameManager;

        public override void Start()
        {
            gameManager = GameManager.Instance;
            Assert.IsNotNull(gameManager);

            base.Start();
        }

        public override void OnClick()
        {
            base.OnClick();

            gameManager.RestartGame();
        }
    }
}
