using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    [Serializable]
    public struct TruckModifierItem
    {
        public AbstractTruckModifier Modifier;

        public float StartTime;
        public float LifeTime;
    }

    public class TruckModifierHandler : MonoBehaviour
    {
        [SerializeField] private List<TruckModifierItem> activeModifiers = new();

        private Truck truck;
        private GameStateMachine gameStateMachine;

        private void Awake()
        {
            truck = GetComponent<Truck>();
            Assert.IsNotNull(truck);

            gameStateMachine = GameStateMachine.Instance;
            Assert.IsNotNull(gameStateMachine);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out TruckModifier modifier))
            {
                for (int i = activeModifiers.Count - 1; i > -1; i--)
                {
                    if (activeModifiers[i].Modifier.IsCancelledBy(modifier.GetModifier()))
                    {
                        activeModifiers.RemoveAt(i);
                    }
                    else if (activeModifiers[i].Modifier == modifier.GetModifier())
                    {
                        activeModifiers.RemoveAt(i);
                    }
                }

                modifier.GetModifier().ModifyTruck(truck);

                if (modifier.GetModifier().GetLifeTime() > 0)
                {
                    activeModifiers.Add(new TruckModifierItem
                    {
                        Modifier = modifier.GetModifier(),
                        StartTime = gameStateMachine.GetGameTime(),
                        LifeTime = modifier.GetModifier().GetLifeTime()
                    });
                }
            }
        }

        private void Update()
        {
            for (int i = activeModifiers.Count - 1; i > -1; i--)
            {
                if (gameStateMachine.GetGameTime() > activeModifiers[i].StartTime + activeModifiers[i].LifeTime)
                {
                    activeModifiers[i].Modifier.FinishModifyingTruck(truck);
                    activeModifiers.RemoveAt(i);
                }
            }
        }
    }
}
