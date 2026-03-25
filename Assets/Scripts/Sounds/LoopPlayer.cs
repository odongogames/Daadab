using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class LoopPlayer : GameStateSubscriber
    {
        private AudioSource audioSource;

        public override void Awake()
        {
            base.Awake();

            audioSource = GetComponent<AudioSource>();
            Assert.IsNotNull(audioSource);
        }

        public override void EnterActiveState()
        {
            base.EnterActiveState();

            audioSource.Play();
        }

        public override void ExitActiveState()
        {
            base.ExitActiveState();

            audioSource?.Pause();
        }
    }
}