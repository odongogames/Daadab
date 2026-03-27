using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Daadab
{
    public class WaterMeter : MonoBehaviour
    {
        [SerializeField] private Image waterDrop;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] [Range(0,1)] private float waterTankFillAmountNormalised;

        private Registry registry;
        private WaterTank waterTank;
        private Animator animator;

        private int growHash;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            Assert.IsNotNull(animator);

            registry = Registry.Instance;
            Assert.IsNotNull(registry);

            growHash = Animator.StringToHash("grow");
        }

        private void Start()
        {
            registry = Registry.Instance;
            Assert.IsNotNull(registry);

            var truck = Truck.Instance;
            Assert.IsNotNull(truck);

            waterTank = truck.GetComponent<WaterTank>();
            Assert.IsNotNull(waterTank);

            waterTank.OnAddToWaterTank += Truck_OnAddToWaterTank;

            GameManager.OnStartGame += GameManager_OnStartGame;
        }

        private void OnDestroy()
        {
            waterTank.OnAddToWaterTank -= Truck_OnAddToWaterTank;

            GameManager.OnStartGame -= GameManager_OnStartGame;
        }

        private void Truck_OnAddToWaterTank(uint value)
        {
            animator.SetTrigger(growHash);
            UpdateValues(value);
        }

        private void GameManager_OnStartGame()
        {
            UpdateValues(0);
        }
        
        private void UpdateValues(uint value)
        {
            var fillAmount = value / (float) registry.TotalWaterCount;
            text.text = $"{fillAmount * 100: 00}%";

            waterTankFillAmountNormalised = fillAmount;
            waterDrop.fillAmount = waterTankFillAmountNormalised;
        }
    }
}
