using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class TextInstance : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TypedText text;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Transform arrowTransform;

        public TypedText Text => text;

        private void Awake()
        {
            Assert.IsNotNull(text);
            Assert.IsNotNull(rectTransform);
            Assert.IsNotNull(arrowTransform);

            canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(canvasGroup);

            canvasGroup.alpha = 0;
            // canvasGroup.interactable = false;
            // canvasGroup.blocksRaycasts = false;
        }

        public void Activate(string str, float height)
        {
            canvasGroup.DOFade(1, .5f);

            rectTransform.sizeDelta = new Vector2(
                x: rectTransform.sizeDelta.x,
                y: height
            );

            text.UpdateText(str, letterByLetter: true);
        }

        public void DisableArrow() => arrowTransform.gameObject.SetActive(false);
    }
}