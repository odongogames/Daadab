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

    public class TruckModifierHandler : MonoBehaviour, IUnitComponent
    {
        [SerializeField] private List<TruckModifierItem> activeModifiers = new();

        private Truck truck;
        private GameStateMachine gameStateMachine;
        private SFXPlayer SFXPlayer;

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
            foreach (var modifier in activeModifiers)
            {
                modifier.Modifier.FinishModifyingTruck(truck);
                modifier.Modifier.ExitTrigger(truck);
            }
            
            activeModifiers.Clear();
        }

        private void Awake()
        {
            truck = GetComponent<Truck>();
            Assert.IsNotNull(truck);

            gameStateMachine = GameStateMachine.Instance;
            Assert.IsNotNull(gameStateMachine);

            SFXPlayer = SFXPlayer.Instance;
            Assert.IsNotNull(SFXPlayer);
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

                if (modifier.GetModifier().GetAudio() != null)
                {
                    SFXPlayer.PlayClip(modifier.GetModifier().GetAudio());
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

                if (modifier.GetModifier().DisableOnUse())
                {
                    modifier.gameObject.SetActive(false);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out TruckModifier modifier))
            {
                modifier.GetModifier().ExitTrigger(truck);
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
