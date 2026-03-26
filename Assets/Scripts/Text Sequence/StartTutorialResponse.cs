using UnityEngine;

namespace Daadab
{
    [CreateAssetMenu (menuName ="Scriptable Objects/Intro Start Tutorial Response")]
    public class StartTutorialResponse : TextSequenceItem
    {
        public override void CompleteResponse()
        {
            base.CompleteResponse();

            GameManager.Instance.StartTutorial();
        }
    }
}
