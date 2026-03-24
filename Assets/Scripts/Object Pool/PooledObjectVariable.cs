using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Daadab
{
    [CreateAssetMenu(menuName = "Variables/Pooled Object")]
    public class PooledObjectVariable : MyScriptableObject
    {
        [SerializeField] PooledObject obj;

        public PooledObject GetValue() => obj;
        public void SetValue(PooledObject newObj)
        {
            obj = newObj;
        }
    }
}
