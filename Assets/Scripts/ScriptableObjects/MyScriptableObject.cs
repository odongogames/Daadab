using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Daadab
{
    public class MyScriptableObject : ScriptableObject
    {
        [SerializeField] private ScriptableObject me;

        public virtual void OnValidate()
        {
            me = this;
        }
    }
}
