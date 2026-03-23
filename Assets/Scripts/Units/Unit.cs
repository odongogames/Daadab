using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Daadab
{
    public class Unit : GameStateSubscriber
    {
        [SerializeField] private UnitData data;

        // [Header("Runtime only")]
        [SerializeField] private List<IUnitComponent> unitComponents = new List<IUnitComponent>();

        // private Dictionary<Type, Component> cachedComponents = new Dictionary<Type, Component>();

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
            data.position = transform.localPosition;
            data.rotation = transform.rotation;
            data.scale = transform.localScale;
        }

        public void ApplyUnitData()
        {
            transform.localPosition = data.position;
            transform.rotation = data.rotation;
            transform.localScale = data.scale;
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
