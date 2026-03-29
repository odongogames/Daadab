using UnityEngine;

namespace Daadab
{
    [CreateAssetMenu (menuName ="Scriptable Objects/Outro Open Daadab Web Page")]
    public class OpenDaadabWebPage : TextSequenceItem
    {
        public override void CompleteResponse()
        {
            base.CompleteResponse();

            Application.OpenURL(Registry.Instance.DaadabURL);

            TextSequenceRunner.Instance.FinishTextSequence();
        }
    }
}
