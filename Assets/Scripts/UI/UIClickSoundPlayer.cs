using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class UIClickSoundPlayer : ButtonBase
    {
        private SFXPlayer SFXPlayer;

        public override void Start()
        {
            base.Start();

            SFXPlayer = SFXPlayer.Instance;
            Assert.IsNotNull(SFXPlayer);
        }

        public override void OnClick()
        {
            base.OnClick();

            SFXPlayer.PlayClickSound();
        }
    }
}
