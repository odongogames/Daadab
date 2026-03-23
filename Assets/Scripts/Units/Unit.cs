using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Daadab
{
    public class Unit : GameStateSubscriber
    {
        [SerializeField] private UnitData data;

        private List<IUnitComponent> unitComponents = new List<IUnitComponent>();

        // TODO: Only save data that is needed.
        // Consider changing it to ModifierData and moving it to teh truck modifier class
        public UnitData GetUnitData() => data;

        private Transform originalParent;

        public override void Awake()
        {
            base.Awake();

            SaveUnitData();
            originalParent = transform.parent;

            unitComponents = new List<IUnitComponent>(GetComponentsInChildren<IUnitComponent>());
        }


        [ContextMenu("Save Unit Data")]
        public void SaveUnitData()
        {
            data.Position = transform.localPosition;
            data.Rotation = transform.rotation;
            data.Scale = transform.localScale;
        }

        public void SetUnitData(UnitData newData)
        {
            data = newData;
        }

        public void ApplyUnitData()
        {
            transform.localPosition = data.Position;
            transform.rotation = data.Rotation;
            transform.localScale = data.Scale;
        }

        public virtual void ResetMe()
        {
            for (int i = 0; i < unitComponents.Count; i++)
            {
                unitComponents[i].ResetMe();
            }

            transform.SetParent(originalParent, worldPositionStays: false);

            ApplyUnitData();
        }

        public override void EnterActiveState()
        {
            base.EnterActiveState();

            for (int i = 0; i < unitComponents.Count; i++)
            {
                unitComponents[i].EnterActiveState();
            }
        }

        public override void ExitActiveState()
        {
            base.ExitActiveState();

            for (int i = 0; i < unitComponents.Count; i++)
            {
                unitComponents[i].ExitActiveState();
            }
        }

    }
}
