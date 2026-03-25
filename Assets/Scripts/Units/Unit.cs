using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Daadab
{
    public class Unit : GameStateSubscriber
    {
        private List<IUnitComponent> unitComponents = new List<IUnitComponent>();

        public override void Awake()
        {
            base.Awake();

            unitComponents = new List<IUnitComponent>(GetComponentsInChildren<IUnitComponent>());
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

        public override void ResetMe()
        {
            base.ResetMe();
            
            for (int i = 0; i < unitComponents.Count; i++)
            {
                unitComponents[i].ResetMe();
            }
        }
    }
}
