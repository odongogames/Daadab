using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Daadab
{
    public class DepthOfFieldManager : GameStateSubscriber
    {
        [SerializeField] private Volume globalVolume;

        private DepthOfField dof;

        public override void Awake()
        {
            base.Awake();

            Assert.IsNotNull(globalVolume);

            globalVolume.profile.TryGet(out dof);

            dof.active = true;
        }

        public override void EnterActiveState()
        {
            base.EnterActiveState();

            if(dof)
            dof.active = true;
        }

        public override void ExitActiveState()
        {
            base.ExitActiveState();

            if(dof)
            dof.active = false;
        }
    }
}