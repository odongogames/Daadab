using UnityEngine;

namespace Daadab
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Text Sequence")]
    public class TextSequence : MyScriptableObject
    {
        public TextSequenceItem[] texts;
    }
}
