using UnityEngine;

namespace Daadab
{
    [CreateAssetMenu (menuName ="Scriptable Objects/Intro Start Game Response")]
    public class StartGameResponse : TextSequenceItem
    {
        public override void CompleteResponse()
        {
            base.CompleteResponse();

            GameManager.Instance.FinishIntroSequence();
        }
    }
}
