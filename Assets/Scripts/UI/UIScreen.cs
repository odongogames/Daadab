using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIScreen : GameStateSubscriber
    {
        [SerializeField] private bool showInstantly;
        [SerializeField] private bool hideInstantly;

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

        protected void Show(bool instantly = false)
        {
            rectTransform.anchoredPosition = Vector2.zero;

            canvasGroup.DOKill();

            if (instantly)
                canvasGroup.alpha = 1;
            else
                canvasGroup.DOFade(1, fadeTime);

            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        protected void Hide(bool instantly = false)
        {
            canvasGroup.DOKill();

            if (instantly)
                canvasGroup.alpha = 0;
            else
                canvasGroup.DOFade(0, fadeTime);
                
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
