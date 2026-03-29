using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class BoostSound : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        private void Awake()
        {
            Assert.IsNotNull(audioSource);
            
            SpeedBooster.OnStartBoost += SpeedBooster_OnStartBoost;
            SpeedBooster.OnFinishBoost += SpeedBooster_OnFinishBoost;
        }

        private void OnDestroy()
        {
            SpeedBooster.OnStartBoost -= SpeedBooster_OnStartBoost;
            SpeedBooster.OnFinishBoost -= SpeedBooster_OnFinishBoost;
        }

        private void SpeedBooster_OnStartBoost()
        {
            audioSource.Play();
        }

        private void SpeedBooster_OnFinishBoost()
        {
            audioSource.Pause();
        }
    }
}
