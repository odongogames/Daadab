using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Daadab
{
    public class UnitAnimator : MonoBehaviour, IUnitComponent
    {
        [SerializeField] protected Animator animator;

        public Animator GetAnimator() => animator;

        public virtual void EnterActiveState()
        {
            if (animator)
                animator.enabled = true;
        }

        public virtual void ExitActiveState()
        {
            if (animator)
                animator.enabled = false;
        }

        public virtual void ResetMe()
        {
            
        }
    }
}
