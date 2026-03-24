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
        
        private Truck truck;
        private Animator animator;

        private int growHash;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            Assert.IsNotNull(animator);

            growHash = Animator.StringToHash("grow");
        }

        private void Start()
        {
            truck = Truck.Instance;
            Assert.IsNotNull(truck);

            truck.OnAddToWaterTank += Truck_OnAddToWaterTank;

            UpdateValues(0);
        }

        private void OnDestroy()
        {
            truck.OnAddToWaterTank -= Truck_OnAddToWaterTank;
        }

        private void Truck_OnAddToWaterTank(uint value)
        {
            animator.SetTrigger(growHash);
            UpdateValues(value);
        }

        private void UpdateValues(uint value)
        {
            text.text = $"{value: 00}%";

            waterTankFillAmountNormalised = value / 100f;
            waterDrop.fillAmount = waterTankFillAmountNormalised;
        }
    }
}
