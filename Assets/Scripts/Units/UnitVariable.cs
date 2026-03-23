using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Daadab
{
    [CreateAssetMenu(menuName = "Variables/Unit")]
    public class UnitVariable : MyScriptableObject
    {
        [SerializeField] Unit unit;

        public Unit GetValue() => unit;
        public void SetValue(Unit newUnit)
        {
            unit = newUnit;
        }
    }
}
