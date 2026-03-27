// original: Google Gemini
// edited by: Odongo Games

using UnityEngine;
using DG.Tweening;
using UnityEngine.Assertions;
using System;

namespace Daadab
{
    public class SpeedLines : GameStateSubscriber
    {
        [Header("Particle Systems")]
        private ParticleSystem speedLines;

        [Header("Settings")]
        [SerializeField] private float activeEmissionRate = 50f;
        [SerializeField] private float transitionDuration = 0.2f;

        private ParticleSystem.EmissionModule linesEmission;
        private float currentRate = 0f;

        private void Start()
        {
            speedLines = GetComponent<ParticleSystem>();
            Assert.IsNotNull(speedLines);

            // Cache the emission modules
            linesEmission = speedLines.emission;

            Setup();

            SpeedBooster.OnStartBoost += SpeedBooster_OnStartBoost;
            SpeedBooster.OnFinishBoost += SpeedBooster_OnFinishBoost;
            GameManager.OnSetupGame += GameManager_OnSetupGame;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            SpeedBooster.OnStartBoost -= SpeedBooster_OnStartBoost;
            SpeedBooster.OnFinishBoost -= SpeedBooster_OnFinishBoost;
            GameManager.OnSetupGame -= GameManager_OnSetupGame;
        }

        private void Setup()
        {
            // Ensure they start at zero
            linesEmission.rateOverTime = 0f;
        }

        private void GameManager_OnSetupGame()
        {
            Setup();
        }

        private void SpeedBooster_OnFinishBoost()
        {
            ToggleBoost(false);
        }

        private void SpeedBooster_OnStartBoost()
        {
            ToggleBoost(true);
        }

        public override void EnterActiveState()
        {
            base.EnterActiveState();

            enabled = true;
        }

        public override void ExitActiveState()
        {
            base.ExitActiveState();

            enabled = false;
        }

        public void ToggleBoost(bool isBoosting)
        {
            float targetRate = isBoosting ? activeEmissionRate : 0f;

            // Use DOTween to smoothly ramp the emission rate
            // This prevents particles from just "popping" in instantly
            DOTween.To(() => currentRate, x => currentRate = x, targetRate, transitionDuration)
                .OnUpdate(() =>
                {
                    linesEmission.rateOverTime = currentRate;
                });
        }
    }
}