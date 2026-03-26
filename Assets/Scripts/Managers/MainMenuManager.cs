using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class MainMenuManager : MonoBehaviour
    {
        private GameStateMachine gameStateMachine;

        public static Action OnStartMainMenu;

        private void Start()
        {
            gameStateMachine = GameStateMachine.Instance;
            Assert.IsNotNull(gameStateMachine);

            StartCoroutine(StartMainMenuCO());
        }

        private IEnumerator StartMainMenuCO()
        {
            yield return new WaitForEndOfFrame();

            Debug.Log("-----------------------");
            Debug.Log($"Setup main menu.");

            OnStartMainMenu?.Invoke();
        }
    }
}
