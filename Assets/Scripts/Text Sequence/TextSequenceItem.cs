using UnityEngine;

namespace Daadab
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Text Sequence Item")]
    public class TextSequenceItem : MyScriptableObject
    {
        [TextArea(5, 10)]
        public string text;
        public TextSequenceItem yesResponse;
        public TextSequenceItem noResponse;

        /// <summary>
        ///  What happens after this text is shown?
        /// </summary>
        public virtual void CompleteResponse()
        {

        }
    }
}