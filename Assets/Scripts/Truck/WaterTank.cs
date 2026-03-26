using System;
using UnityEngine;

namespace Daadab
{

    public class WaterTank : MonoBehaviour, IUnitComponent
    {
        [SerializeField] private uint waterTank;

        public Action<uint> OnAddToWaterTank;

        public uint GetWaterAmount() => waterTank;

        public void AddToWaterTank()
        {
            waterTank++;
            Debug.Log($"Add to watertank: {waterTank}");
            OnAddToWaterTank?.Invoke(waterTank);
        }

        public void EnterActiveState()
        {
        }

        public void ExitActiveState()
        {
        }

        public void ResetMe()
        {
            waterTank = 0;
        }
    }
}
