using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class GameStateController : MonoBehaviour
    {
        [SerializeField] private GameState activeGameState;
        public GameState GetActiveState() => activeGameState;

        /// <summary>
        /// Which states are allowed to transition to this one?
        /// </summary>
        [SerializeField] private List<GameState> allowedAncestors;

        [Header("Runtime Only")]
        [SerializeField] private List<GameStateSubscriber> gameStateSubscribers = new List<GameStateSubscriber>();

        private GameStateMachine gameStateMachine;

        // public override void InstallServices(WakeObjectsUpEvent evt)
        // {
        //     base.InstallServices(evt);

        //     gameStateMachine = ServiceLocator.GetService<GameStateMachine>();
        //     Assert.IsNotNull(gameStateMachine);
        // }

        public void SetActiveGameState(GameState state) => activeGameState = state;

        public void AddSubscriber(GameStateSubscriber subscriber)
        {
            if (subscriber.GetActiveState() == activeGameState &&
                !gameStateSubscribers.Contains(subscriber))
            {
                gameStateSubscribers.Add(subscriber);

                // Debug.Log($"{name} add subscriber {subscriber.name}", subscriber);

                if (subscriber.GetActiveState() == GameState.None) return;

                if (gameStateMachine.GetCurrentState() == activeGameState)
                {
                    subscriber.EnterActiveState();
                    // Debug.Log($"{subscriber.name} enter active state");
                }
                else
                {
                    subscriber.ExitActiveState();
                }
            }
        }

        public void RemoveSubscriber(GameStateSubscriber subscriber)
        {
            if (subscriber.GetActiveState() == activeGameState &&
                gameStateSubscribers.Contains(subscriber))
            {
                gameStateSubscribers.Remove(subscriber);
            }
        }

        public void ChangeState(GameState newGameState, GameState previousGameState)
        {
            if (activeGameState == newGameState)
            {
                EnterActiveState(newGameState);
            }
            else // if(activeGameState == previousGameState)
            {
                ExitActiveState(previousGameState);
            }
        }

        public bool AllowTransition(GameState state)
        {
            return allowedAncestors != null && allowedAncestors.Contains(state);
        }

        public void EnterActiveState(GameState gameState)
        {
            // Debug.Log("Controller: " + name + " enter active state");

            foreach (var subscriber in gameStateSubscribers)
            {
                subscriber.EnterActiveState();
            }
        }

        public void ExitActiveState(GameState gameState)
        {
            // Debug.Log("Controller: " + name + " exit active state");

            foreach (var subscriber in gameStateSubscribers)
            {
                subscriber.ExitActiveState();
            }
        }

        public void SetStateMachine(GameStateMachine gameStateMachine)
        {
            this.gameStateMachine = gameStateMachine;
        }
    }
}
