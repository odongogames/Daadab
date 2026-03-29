using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Daadab
{
    public class InputReader : MonoBehaviour
    {
        public static InputReader Instance;

        // public bool StartTouch() => inputActions.Player.Touch.IsPressed();
        public Vector2 TouchPosition() => inputActions.Player.TouchPosition.ReadValue<Vector2>();
        public bool Touch() => inputActions.Player.Touch.WasPressedThisFrame();

        public bool StartMovingLeft() => inputActions.Player.MoveLeft.WasPressedThisFrame();
        public bool MoveLeft() => inputActions.Player.MoveLeft.IsPressed();

        public bool StartMovingRight() => inputActions.Player.MoveRight.WasPressedThisFrame();
        public bool MoveRight() => inputActions.Player.MoveRight.IsPressed();

        public bool StartBoost() => inputActions.Player.Boost.WasPressedThisFrame();
        public bool Boost() => inputActions.Player.Boost.IsPressed();

        public bool StartBraking() => inputActions.Player.Brake.WasPressedThisFrame();
        public bool Brake() => inputActions.Player.Brake.IsPressed();

        public bool StartEnter() => inputActions.Player.Enter.WasPressedThisFrame();
        public bool Enter() => inputActions.Player.Enter.IsPressed();
        
        public bool StartEscape() => inputActions.Player.Escape.WasPressedThisFrame();
        public bool Escape() => inputActions.Player.Escape.IsPressed();

        public bool StartMouseClick() => inputActions.Player.MouseClick.WasPressedThisFrame();
        public bool MouseClick() => inputActions.Player.MouseClick.IsPressed();

        public Vector2 MousePosition() => inputActions.Player.MousePosition.ReadValue<Vector2>();

        PlayerInputActions inputActions;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log($"Destroying {this.GetType()} as more than one instance found.");
                Destroy(this);
                return;
            }

            Instance = this;
        }

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
