using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    
    public class TextSequenceYesButton : ButtonBase
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

            TextSequenceRunner.Instance.ChooseYesResponse();
        }
    }
}
