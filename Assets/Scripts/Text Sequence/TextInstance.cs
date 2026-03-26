using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class TextInstance : MonoBehaviour
    {
        [SerializeField] private TypedText text;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Transform arrowTransform;

        private void Awake()
        {
            Assert.IsNotNull(text);
            Assert.IsNotNull(rectTransform);
            Assert.IsNotNull(arrowTransform);
        }

        public void Activate(string str, float height)
        {
            rectTransform.sizeDelta = new Vector2(
                x: rectTransform.sizeDelta.x,
                y: height
            );

            text.UpdateText(str, letterByLetter: false);
        }
    }
}