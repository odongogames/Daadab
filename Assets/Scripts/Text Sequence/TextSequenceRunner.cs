using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using DG.Tweening;

namespace Daadab
{
    public class TextSequenceRunner : GameStateSubscriber
    {
        public static TextSequenceRunner Instance;

        [SerializeField] private TextSequence introTextSequence;
        [SerializeField] private TextSequence outroTextSequence;
        [SerializeField] private TextInstance textInstanceTemplate;
        [SerializeField] private RectTransform textHolder;
        [SerializeField] private GameObject tutorialYesButton;
        [SerializeField] private GameObject tutorialNoButton;


        [Header("Runtime Only")]
        [SerializeField] private TextInstance activeText;
        [SerializeField] private TextSequence textSequence;
        [SerializeField] private TextSequenceItem yesResponse;
        [SerializeField] private TextSequenceItem noResponse;
        [SerializeField] private float speechHolderPositionY;
        [SerializeField] private float origSpeechHolderPositionY;

        private int textIndex;

        private float lineHeight = 70;
        private float padding = 100;

        private RectTransform tutorialYesButtonRectTransform;
        private RectTransform tutorialNoButtonRectTransform;

        private InputReader inputReader;
        PlayerInputActions inputActions;

        public override void Awake()
        {
            if (Instance != null)
            {
                Debug.Log($"Destroying {this.GetType()} as more than one instance found.");
                Destroy(this);
                return;
            }

            base.Awake();

            Assert.IsNotNull(introTextSequence);
            Assert.IsNotNull(outroTextSequence);
            Assert.IsNotNull(textHolder);

            origSpeechHolderPositionY = textHolder.anchoredPosition.y;

            Assert.IsNotNull(tutorialYesButton);
            Assert.IsNotNull(tutorialNoButton);

            tutorialNoButton.SetActive(false);
            tutorialYesButton.SetActive(false);

            tutorialYesButtonRectTransform = tutorialYesButton.GetComponent<RectTransform>();
            tutorialNoButtonRectTransform = tutorialNoButton.GetComponent<RectTransform>();

            Assert.IsNotNull(tutorialYesButtonRectTransform);
            Assert.IsNotNull(tutorialNoButtonRectTransform);

            Instance = this;

            GameManager.OnFinishGame += GameManager_OnFinishGame;
        }

        private void Start()
        {
            inputReader = InputReader.Instance;
            Assert.IsNotNull(inputReader);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            GameManager.OnFinishGame -= GameManager_OnFinishGame;
        }

        private void GameManager_OnFinishGame()
        {
            textSequence = outroTextSequence;
            ResetTextHolderPosition();
        }

        private void Update()
        {
            if (!enabled) return;

            if (inputReader.StartEnter()) ShowNextText();

            if (inputReader.StartMouseClick()) ShowNextText();

            if (inputReader.StartEscape()) FinishTextSequence();
        }

        public override void EnterActiveState()
        {
            base.EnterActiveState();

            RunTextSequence();
        }

        public void SetIntroTextSequence()
        {
            textSequence = introTextSequence;
            ResetTextHolderPosition();
        }

        private void ResetTextHolderPosition()
        {
            speechHolderPositionY = origSpeechHolderPositionY;
            textHolder.anchoredPosition = new Vector2(
                textHolder.anchoredPosition.x,
                textHolder.anchoredPosition.y
            );
        }

        public void RunTextSequence()
        {
            Assert.IsNotNull(textSequence.finishSequenceResponse);

            Debug.Log($"Run text sequence: {textSequence.name}");

            textIndex = -1;

            ShowNextText();
        }

        public void ShowNextText()
        {
            if (activeText && activeText.Text.IsRevealingText)
            {
                activeText.Text.FinishRevealingText();
                return;
            }

            textIndex++;

            Debug.Log($"Show text: {textIndex}");

            if (textIndex >= textSequence.texts.Length)
            {
                Debug.Log("End of texts reached");

                if (noResponse == null && yesResponse == null)
                {
                    FinishTextSequence();
                }
                return;
            }

            var text = Instantiate(textInstanceTemplate, textHolder);

            var substrings = textSequence.texts[textIndex].text.Split('^');
            // Debug.Log($"{substrings.Length} lines found!");

            var height = substrings.Length * lineHeight + padding * 2;

            Debug.Log($"Add height of {height}");

            text.Activate(textSequence.texts[textIndex].text, height);

            activeText = text;

            if (textIndex > 0)
            {
                height += 50;
            }

            speechHolderPositionY += height;

            textHolder.DOKill();

            textHolder.DOAnchorPosY(speechHolderPositionY, .5f);

            // textHolder.anchoredPosition += new Vector2(0, height);

            yesResponse = textSequence.texts[textIndex].yesResponse;
            noResponse = textSequence.texts[textIndex].noResponse;

            if (yesResponse != null && noResponse != null)
            {
                tutorialNoButtonRectTransform.localScale = Vector2.zero;
                tutorialYesButtonRectTransform.localScale = Vector2.zero;

                tutorialNoButton.SetActive(true);
                tutorialYesButton.SetActive(true);

                tutorialNoButtonRectTransform.DOKill();
                tutorialYesButtonRectTransform.DOKill();

                tutorialNoButtonRectTransform.DOScale(1, .5f).SetDelay(.5f);
                tutorialYesButtonRectTransform.DOScale(1, .5f).SetDelay(.5f);
            }
            else
            {
                tutorialNoButton.SetActive(false);
                tutorialYesButton.SetActive(false);
            }

            text.gameObject.SetActive(true);
        }

        public void ChooseYesResponse()
        {
            if (yesResponse != null)
                yesResponse.CompleteResponse();
        }

        public void ChooseNoResponse()
        {
            if (noResponse != null)
                noResponse.CompleteResponse();
        }

        private void FinishTextSequence()
        {
            textSequence.finishSequenceResponse.CompleteResponse();
        }
    }
}
