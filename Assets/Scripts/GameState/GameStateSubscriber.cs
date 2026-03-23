using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class GameStateSubscriber : MonoBehaviour
    {
        [SerializeField] protected GameState activeGameState;

        protected GameStateMachine gameStateMachine;

        public GameState GetActiveState() => activeGameState;

        public virtual void Awake()
        {
            if (activeGameState == GameState.None)
            {
                Debug.LogError(name + " does not have any active state!", this);
                return;
            }
    
            gameStateMachine = GameStateMachine.Instance;

            gameStateMachine.AddGameStateSubscriber(this);
        }

        public virtual void OnDestroy()
        {
            gameStateMachine.RemoveGameStateSubscriber(this);
        }


        public virtual void EnterActiveState()
        {
        }

        public virtual void ExitActiveState()
        {
        }

        protected bool IsInActiveGameState()
        {
            return activeGameState == gameStateMachine.GetCurrentState();
        }
    }
}
