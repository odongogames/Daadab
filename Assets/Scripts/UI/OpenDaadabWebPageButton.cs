using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class OpenDaadabWebPageButton : ButtonBase
    {
        public override void OnClick()
        {
            base.OnClick();

            Application.OpenURL(Registry.Instance.DaadabURL);
        }
    }
}
