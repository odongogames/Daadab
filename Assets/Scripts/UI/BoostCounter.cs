using System;
using UnityEngine;
using UnityEngine.UI;

namespace Daadab
{
    public class BoostCounter : MonoBehaviour
    {
        [SerializeField] private Image[] boostIcon;

        private void Awake()
        {
            
            SpeedBooster.OnAddBoost += SpeedBooster_OnAddBoost;
            SpeedBooster.OnStartBoost += SpeedBooster_OnStartBoost;
            GameManager.OnSetupGame += GameManager_OnSetupGame;
        }

        private void OnDestroy()
        {
            SpeedBooster.OnAddBoost -= SpeedBooster_OnAddBoost;
            SpeedBooster.OnStartBoost -= SpeedBooster_OnStartBoost;
            GameManager.OnSetupGame -= GameManager_OnSetupGame;
        }

        private void SpeedBooster_OnAddBoost(uint count)
        {
            for (int i = 0; i < boostIcon.Length; i++)
            {
                boostIcon[i].color = i + 1 <= count ? Color.red : Color.clear;
            }
        }

        private void GameManager_OnSetupGame()
        {
            ResetMe();
        }

        private void SpeedBooster_OnStartBoost()
        {
            ResetMe();
        }

        private void ResetMe()
        {
            for (int i = 0; i < boostIcon.Length; i++)
            {
                boostIcon[i].color = Color.clear;
            }
        }
    }
}
