// original: Google Gemini
// edited by: Odongo Games

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using UnityEngine.Assertions;
using System; // Import DOTween

namespace Daadab
{
    public class CarBoostEffects : GameStateSubscriber
    {
        [Header("References")]
        [SerializeField] private Volume globalVolume;

        [Header("Settings")]
        private float boostFOV = 70f;
        private float normalFOV = 60f;
        private float effectDuration = 0.5f;

        private ChromaticAberration chromatic;
        private MotionBlur motionBlur;
        private LensDistortion lens;
        private Camera mainCamera;


        public override void Awake()
        {
            base.Awake();

            Assert.IsNotNull(globalVolume);

            mainCamera = Camera.main;
            Assert.IsNotNull(mainCamera);
            
            // Get the effects from the Volume Profile
            globalVolume.profile.TryGet(out chromatic);
            globalVolume.profile.TryGet(out lens);
            globalVolume.profile.TryGet(out motionBlur);

            SpeedBooster.OnStartBoost += SpeedBooster_OnStartBoost;
            SpeedBooster.OnFinishBoost += SpeedBooster_OnFinishBoost;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            SpeedBooster.OnStartBoost -= SpeedBooster_OnStartBoost;
            SpeedBooster.OnFinishBoost -= SpeedBooster_OnFinishBoost;
        }

        private void SpeedBooster_OnStartBoost()
        {
            StartBoost();
        }

        private void SpeedBooster_OnFinishBoost()
        {
            FinishBoost();
        }

        public void StartBoost()
        {
            // 1. Camera FOV "Kick"
            mainCamera.DOFieldOfView(boostFOV, effectDuration).SetEase(Ease.OutExpo);

            // 2. Chromatic Aberration (Color Fringing)
            // We use DOTween.To to target the 'intensity' value inside the URP effect
            DOTween.To(() => chromatic.intensity.value, x => chromatic.intensity.value = x, 1f, effectDuration);

            // 3. Lens Distortion (Pincushion effect)
            DOTween.To(() => lens.intensity.value, x => lens.intensity.value = x, -0.4f, effectDuration);

            DOTween.To(() => motionBlur.intensity.value, x => motionBlur.intensity.value = x, 0.7f, effectDuration);

            // Add this inside StartBoost for a physical impact feel
            mainCamera.transform.DOShakePosition(0.7f, strength: 0.5f, vibrato: 20);
        }

        public void FinishBoost()
        {
            // Smoothly return everything to normal
            mainCamera.DOFieldOfView(normalFOV, effectDuration * 2).SetEase(Ease.InSine);
            DOTween.To(() => chromatic.intensity.value, x => chromatic.intensity.value = x, 0f, effectDuration * 2);
            DOTween.To(() => lens.intensity.value, x => lens.intensity.value = x, 0f, effectDuration * 2);
            DOTween.To(() => motionBlur.intensity.value, x => motionBlur.intensity.value = x, 0f, effectDuration * 2);

        }
    }
}