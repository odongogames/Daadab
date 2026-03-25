using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class SpeedBooster : MonoBehaviour, IUnitComponent
    {
        [SerializeField] private uint maxBoostCount = 3;
        [SerializeField] private float boostDurationShort = 1.5f;
        [SerializeField] private float boostDurationMedium = 3f;
        [SerializeField] private float boostDurationLong = 6f;

        [Header("Runtime Only")]
        [SerializeField] private float boostDuration;
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

        public bool StartBoost()
        {
            if (boostCount <= 0)
            {
                Debug.Log("Truck does not have any boosts");
                return false;
            }

            if (isBoosting)
            {
                Debug.Log("Truck is already boosting");
                return false;
            }

            lastBoostTime = gameManager.GetGameTime();

            isBoosting = true;

            if (boostCount == 1)
            {
                boostDuration = boostDurationShort;
            }
            else if (boostCount == 2)
            {
                boostDuration = boostDurationMedium;
            }
            else if(boostCount >= 3)
            {
                boostDuration = boostDurationLong;
            }

            boostCount = 0;
            OnStartBoost?.Invoke();

            return true;
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
