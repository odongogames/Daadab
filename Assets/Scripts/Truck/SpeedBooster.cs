using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class SpeedBooster : MonoBehaviour, IUnitComponent
    {
        [SerializeField] private uint maxBoostCount = 3;
        [SerializeField] private float boostDuration = 4;

        [Header("Runtime Only")]
        /// <summary>
        /// How many boosts does the player have?
        /// </summary>
        [SerializeField] private uint boostCount;

        private float lastBoostTime;
        private bool isBoosting;

        private GameManager gameManager;
        private Truck truck;

        public static Action<uint> OnAddBoost;
        public static Action OnStartBoost;
        public static Action OnFinishBoost;

        public bool IsBoosting() => isBoosting;

        private void Awake()
        {
            gameManager = GameManager.Instance;
            Assert.IsNotNull(gameManager);

            truck = GetComponent<Truck>();
            Assert.IsNotNull(truck);

        }

        private void Update()
        {
            if (isBoosting && HasFinishedBoost())
            {
                FinishBoost();
            }
        }

        private bool HasFinishedBoost() => gameManager.GetGameTime() > lastBoostTime + boostDuration;

        public void AddBoost()
        {
            if (boostCount >= maxBoostCount)
            {
                Debug.Log("Truck already has maxx boosts");
                return;
            }

            boostCount++;
            OnAddBoost?.Invoke(boostCount);
        }

        public void StartBoost()
        {
            // if (boostCount <= 0)
            // {
            //     Debug.Log("Truck does not have any boosts");
            //     return false;
            // }

            // if (isBoosting)
            // {
            //     Debug.Log("Truck is already boosting");
            //     return false;
            // }

            lastBoostTime = gameManager.GetGameTime();

            isBoosting = true;
            
            OnStartBoost?.Invoke();
        }

        private void FinishBoost()
        {
            OnFinishBoost?.Invoke();

            isBoosting = false;

            truck.RestoreSpeed();
        }

        public void EnterActiveState()
        {
            enabled = true;
        }

        public void ExitActiveState()
        {
            enabled = false;
        }

        public void ResetMe()
        {
            if (isBoosting)
            {
                FinishBoost();
            }

            boostCount = 0;
            lastBoostTime = 0;
        }
    }
}
