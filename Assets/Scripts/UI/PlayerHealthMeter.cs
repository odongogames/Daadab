using System;
using System.Runtime.InteropServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class PlayerHealthMeter : GameStateSubscriber
    {
        [SerializeField] private float smoothTime = 1f;
        [SerializeField] private float yOffset = -0.1f;
        [SerializeField] private float yOffsetBoosted = -0.3f;
        [SerializeField] private float xOffset = 0.075f;

        [SerializeField] private HeartContainer[] heartContainers;

        private bool isTruckBoosting;

        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;
        private Health playerHealth;
        private Transform truckTransform;
        private Truck truck;
        private Camera mainCamera;
        private Registry registry;

        public override void Awake()
        {
            base.Awake();

            canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(canvasGroup);

            Assert.IsTrue(heartContainers.Length == 3);

            rectTransform = GetComponent<RectTransform>();

            registry = Registry.Instance;
            Assert.IsNotNull(registry);

            mainCamera = Camera.main;
            Assert.IsNotNull(mainCamera);

            truck = Truck.Instance;
            Assert.IsNotNull(truck);

            truckTransform = truck.transform;

            playerHealth = truck.GetComponent<Health>();
            Assert.IsNotNull(playerHealth);

            playerHealth.OnTakeDamage += Player_OnTakeDamage;
            playerHealth.OnAddHealth += Player_OnAddHealth;

            GameManager.OnSetupGame += GameManager_OnSetupGame;

            SpeedBooster.OnStartBoost += SpeedBooster_OnStartBoost;
            SpeedBooster.OnFinishBoost += SpeedBooster_OnFinishBoost;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            playerHealth.OnTakeDamage -= Player_OnTakeDamage;
            playerHealth.OnAddHealth -= Player_OnAddHealth;

            GameManager.OnSetupGame -= GameManager_OnSetupGame;

            SpeedBooster.OnStartBoost -= SpeedBooster_OnStartBoost;
            SpeedBooster.OnFinishBoost -= SpeedBooster_OnFinishBoost;
        }

        private void LateUpdate()
        {
            var position = mainCamera.WorldToViewportPoint(truckTransform.position);
            position.y += isTruckBoosting ? yOffsetBoosted : yOffset;
            position.x += xOffset * (int)truck.Lane;

            rectTransform.anchorMin = Vector2.Lerp(
                a: rectTransform.anchorMin,
                b: position,
                t: Time.deltaTime * smoothTime
            );

            // rectTransform.anchorMin = position;
            rectTransform.anchorMax = rectTransform.anchorMin;
        }

        private void GameManager_OnSetupGame()
        {
            for (int i = 0; i < heartContainers.Length; i++)
            {
                heartContainers[i].Activate();
            }

            canvasGroup.alpha = 0;

            canvasGroup.DOFade(1, registry.LongTime).SetDelay(1);
        }

        private void Player_OnTakeDamage(uint value)
        {
            heartContainers[value].Deactivate();
        }

        private void Player_OnAddHealth(uint value)
        {
            heartContainers[value - 1].Activate();
        }

        private void SpeedBooster_OnFinishBoost()
        {
            isTruckBoosting = false;
        }

        private void SpeedBooster_OnStartBoost()
        {
            isTruckBoosting = true;
        }
    }
}
