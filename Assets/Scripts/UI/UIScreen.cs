using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIScreen : GameStateSubscriber
    {
        [SerializeField] private bool showInstantly;

        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;

        private float fadeTime = 0.3f;

        public override void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            Assert.IsNotNull(rectTransform);

            canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(canvasGroup);

            base.Awake();
        }

        public override void EnterActiveState()
        {
            base.EnterActiveState();

            Show();
        }

        public override void ExitActiveState()
        {
            base.ExitActiveState();

            Hide();
        }

        private void Show()
        {
            rectTransform.anchoredPosition = Vector2.zero;

            canvasGroup.DOKill();

            if (showInstantly)
                canvasGroup.alpha = 1;
            else
                canvasGroup.DOFade(1, fadeTime);

            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        private void Hide()
        {
            canvasGroup.DOKill();

            canvasGroup.DOFade(0, fadeTime);
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
