using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class PauseButton : ButtonBase
    {
        private GameManager gameManager;

        public override void Awake()
        {
            base.Awake();

            gameManager = GameManager.Instance;
            Assert.IsNotNull(gameManager);
        }

        public override void OnClick()
        {
            base.OnClick();

            gameManager.TogglePause();
        }
    }
}
