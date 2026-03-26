using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Daadab
{
    public class MissionCompleteManager : GameStateSubscriber
    {
        [SerializeField] private TextMeshProUGUI bannerText;
        [SerializeField] private CanvasGroup statsCanvasGroup;
        [SerializeField] private TextMeshProUGUI waterMeterText;
        [SerializeField] private Image waterMeterImage;

        private RectTransform statsCanvasRectTransform;

        private Registry registry;
        private WaterTank waterTank;

        private float waterAmount;

        public override void Awake()
        {
            base.Awake();

            Assert.IsNotNull(bannerText);
            Assert.IsNotNull(statsCanvasGroup);
            Assert.IsNotNull(waterMeterText);
            Assert.IsNotNull(waterMeterImage);

            statsCanvasRectTransform = statsCanvasGroup.GetComponent<RectTransform>();
            Assert.IsNotNull(statsCanvasRectTransform);

            registry = Registry.Instance;

            var truck = Truck.Instance;
            Assert.IsNotNull(truck);

            waterTank = truck.GetComponent<WaterTank>();
            Assert.IsNotNull(waterTank);

            GameManager.OnSetupGame += GameManager_OnSetupGame;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            GameManager.OnSetupGame -= GameManager_OnSetupGame;
        }

        private void GameManager_OnSetupGame()
        {
            waterMeterImage.fillAmount = 0;

            bannerText.gameObject.SetActive(false);
            statsCanvasGroup.gameObject.SetActive(false);
            statsCanvasGroup.alpha = 0;
            statsCanvasGroup.interactable = false;
            statsCanvasGroup.blocksRaycasts = false;
        }

        public override void EnterActiveState()
        {
            base.EnterActiveState();

            statsCanvasGroup.gameObject.SetActive(true);
            bannerText.gameObject.SetActive(true);

            bannerText.rectTransform.DOScale(1.25f, .5f).OnComplete(() => { ReduceBannerTextSize(); });
        }

        private void ReduceBannerTextSize()
        {
            bannerText.rectTransform.DOScale(1, 2f).OnComplete(() => { ShowStats(); });
        }

        private void ShowStats()
        {
            bannerText.DOFade(0, .25f);

            var scale = 0.8f;
            statsCanvasRectTransform.localScale = new Vector3(scale, scale, scale);
            statsCanvasRectTransform.DOScale(1, .75f);

            statsCanvasGroup.DOFade(1, .75f).OnComplete(() => { StartAnimatingWaterMeter(); });
            statsCanvasGroup.interactable = true;
            statsCanvasGroup.blocksRaycasts = true;
        }

        private void StartAnimatingWaterMeter()
        {
            StartCoroutine(AnimateWaterMeterCO());
        }

        private void AnimateWaterMeter()
        {
            var percentage = waterAmount / registry.TotalWaterCount;

            waterMeterText.text = (percentage * 100).ToString("00");

            waterMeterImage.fillAmount = percentage;
        }

        private IEnumerator AnimateWaterMeterCO()
        {
            var done = false;

            while (!done)
            {
                waterAmount = Mathf.MoveTowards(waterAmount, waterTank.GetWaterAmount(), 20 * Time.deltaTime);

                AnimateWaterMeter();

                if (waterAmount == waterTank.GetWaterAmount())
                {
                    done = true;
                }
                yield return null;
            }
        }
    }
}
