using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class TutorialInput : GameStateSubscriber
    {
        private InputReader inputReader;

        private void Start()
        {
            inputReader = InputReader.Instance;
            Assert.IsNotNull(inputReader);
        }

        private void Update()
        {
            if (!enabled) return;

            if (inputReader.Escape()) GameManager.Instance.FinishIntroSequence();
            
            if (inputReader.StartMouseClick()) GameManager.Instance.FinishIntroSequence();

            
            if (inputReader.Enter()) GameManager.Instance.FinishIntroSequence(); 
        }
    }
}
