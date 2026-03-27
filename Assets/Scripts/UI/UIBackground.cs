using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class UIBackground : UIScreen
    {
        public static UIBackground Instance;

        [SerializeField] private CanvasGroup skipInstructions;
        private Registry registry;

        public override void Awake()
        {
            if (Instance != null)
            {
                Debug.Log($"Destroying {this.GetType()} as more than one instance found.");
                Destroy(this);
                return;
            }

            Instance = this;

            base.Awake();

            registry = Registry.Instance;
            Assert.IsNotNull(registry);

            Assert.IsNotNull(skipInstructions);

            GameManager.OnStartIntroConversation += GameManager_OnStartIntroConversation;
            GameManager.OnStartGame += GameManager_OnStartGame;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            GameManager.OnStartIntroConversation -= GameManager_OnStartIntroConversation;
            GameManager.OnStartGame -= GameManager_OnStartGame;
        }

        private void GameManager_OnStartIntroConversation()
        {
            skipInstructions.alpha = 0;
            skipInstructions.DOFade(1, registry.MediumTime);
            
            Show(instantly: true);
        }

        private void GameManager_OnStartGame()
        {
            Hide();
        }
    }
}
