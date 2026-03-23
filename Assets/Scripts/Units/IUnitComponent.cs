using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public interface IUnitComponent
    {
        public void EnterActiveState();
        public void ExitActiveState();
        public void ResetMe();
    }
}
