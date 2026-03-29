using UnityEngine;

namespace Daadab
{
    [CreateAssetMenu (menuName ="Scriptable Objects/Outro Go To Next Text Item")]
    public class GoToNextTextItem : TextSequenceItem
    {
        public override void CompleteResponse()
        {
            base.CompleteResponse();

            TextSequenceRunner.Instance.ShowNextText();
        }
    }
}
