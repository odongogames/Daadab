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

            if (inputReader.StartEscape()) GameManager.Instance.FinishIntroSequence();
            
            if (inputReader.StartMouseClick()) GameManager.Instance.FinishIntroSequence();
            else if (inputReader.Touch()) GameManager.Instance.FinishIntroSequence();

            if (inputReader.StartEnter()) GameManager.Instance.FinishIntroSequence(); 

            if (inputReader.MoveLeft()) GameManager.Instance.FinishIntroSequence(); 

            if (inputReader.MoveRight()) GameManager.Instance.FinishIntroSequence(); 
        }
    }
}
