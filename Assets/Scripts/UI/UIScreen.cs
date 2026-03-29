using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Daadab
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIScreen : GameStateSubscriber
    {
        /// <summary>
        /// This button will be selected when UI screen is opened
        /// </summary>
        [SerializeField] private Button firstButton;
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

        private void OnApplicationFocus(bool focus)
        {
            if(firstButton)
                firstButton.Select();            
        }

        public void Show(bool instantly = false)
        {
            rectTransform.anchoredPosition = Vector2.zero;

            canvasGroup.DOKill();

            if (instantly)
                canvasGroup.alpha = 1;
            else
                canvasGroup.DOFade(1, fadeTime);

            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            if (firstButton)
                firstButton.Select();
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
