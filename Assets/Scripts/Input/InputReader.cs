using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Daadab
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Input Reader")]
    public class InputReader : MyScriptableObject
    {
        public bool StartMovingLeft() => inputActions.Player.MoveLeft.WasPressedThisFrame();
        public bool MoveLeft() => inputActions.Player.MoveLeft.IsPressed();

        public bool StartMovingRight() => inputActions.Player.MoveRight.WasPressedThisFrame();
        public bool MoveRight() => inputActions.Player.MoveRight.WasPressedThisFrame();

        public bool StartBraking() => inputActions.Player.Brake.WasPressedThisFrame();
        public bool Brake() => inputActions.Player.Brake.IsPressed();

        PlayerInputActions inputActions;

        private void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerInputActions();
                inputActions.Enable();
                // inputActions.Player.SetCallbacks(this);
            }
        }

        private void OnDestroy()
        {
            if (inputActions != null)
            {
                inputActions.Disable();
            }
        }

        private void OnDisable()
        {
            if (inputActions != null)
            {
                inputActions.Disable();
            }
        }
    }
}
