using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

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
        [SerializeField] private TextSequence textSequence;

        private int textIndex;
        
        private float lineHeight = 70;
        private float padding = 100;

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

            Assert.IsNotNull(tutorialYesButton);
            Assert.IsNotNull(tutorialNoButton);

            Instance = this;

            GameManager.OnFinishGame += GameManager_OnFinishGame;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            GameManager.OnFinishGame -= GameManager_OnFinishGame;
        }

        private void GameManager_OnSetupGame()
        {
            textSequence = introTextSequence;
        }
        
        private void GameManager_OnFinishGame()
        {
            textSequence = outroTextSequence;
        }

        public override void EnterActiveState()
        {
            base.EnterActiveState();

            if (inputActions == null)
            {
                inputActions = new PlayerInputActions();
                inputActions.Enable();
                // inputActions.Player.SetCallbacks(this);

                InputSystem.onAnyButtonPress.Call(ctx => OnAnyKeyPress());
            }

            RunTextSequence();
        }

        public override void ExitActiveState()
        {
            base.ExitActiveState();

            if (inputActions != null)
            {
                // InputSystem.onAnyButtonPress.Remove(OnAnyKeyPress);
                inputActions.Disable();
            }
        }

        public void SetIntroTextSequence()
        {
            textSequence = introTextSequence;
        }

        private void OnAnyKeyPress()
        {
            ShowNextText();
        }

        public void RunTextSequence()
        {
            Debug.Log("Run text sequence");
            
            textIndex = -1;

            ShowNextText();
        }

        private void ShowNextText()
        {
            textIndex++;

            if (textIndex >= textSequence.texts.Length)
            {
                Debug.Log("End of texts reached");
                return;
            }

            var text = Instantiate(textInstanceTemplate, textHolder);

            var substrings = textSequence.texts[textIndex].text.Split('^');
            // Debug.Log($"{substrings.Length} lines found!");

            var height = substrings.Length * lineHeight + padding * 2;

            Debug.Log($"Add height of {height}");

            text.Activate(textSequence.texts[textIndex].text, height);

            if (textIndex > 0)
            {
                height += 50;
            }

            textHolder.anchoredPosition += new Vector2(0, height);

            if (textSequence.texts[textIndex].yesResponse && textSequence.texts[textIndex].noResponse)
            {
                tutorialNoButton.SetActive(true);
                tutorialYesButton.SetActive(true);
            }
            else
            {
                tutorialNoButton.SetActive(false);
                tutorialYesButton.SetActive(false);
            }

            text.gameObject.SetActive(true);
        }
    }
}
