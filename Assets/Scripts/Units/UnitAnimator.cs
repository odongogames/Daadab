using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class UnitAnimator : MonoBehaviour, IUnitComponent
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            Assert.IsNotNull(animator);
        }

        public void EnterActiveState()
        {
            if (animator)
                animator.enabled = true;
        }

        public void ExitActiveState()
        {
            if (animator)
                animator.enabled = false;
        }

        public void ResetMe()
        {
            
        }
    }
}
