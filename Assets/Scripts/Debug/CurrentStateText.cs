using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class CurrentStateText : MonoBehaviour
    {
        private TextMeshProUGUI text;
        private GameStateMachine gameStateMachine;

        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
            Assert.IsNotNull(text);
        }

        private void Start()
        {
            gameStateMachine = GameStateMachine.Instance;
            Assert.IsNotNull(gameStateMachine);
        }

        private void Update()
        {
            text.text = $"Current state:\n{gameStateMachine.GetCurrentState()}";
        }
    }
}
