using UnityEngine;
using UnityEngine.InputSystem;

namespace CIDemo.Input
{
    public class StandaloneInputController : MonoBehaviour, InputSystem_Actions.IPlayerActions, IPlayerInput
    {
        public Vector2 Move { get => _movementValue; }
        public Vector2 Look { get => _lookValue; }
        public bool JumpPressed { get => _jumpPressed; }
        public bool CrouchPressed { get => _crouchPressed; }
        public bool AttackPressed { get => _shootPressed; }
        public bool SprintPressed { get => _sprintPressed; }
        private Vector2 _movementValue, _lookValue;
        private bool _jumpPressed, _crouchPressed, _shootPressed, _sprintPressed;

        private InputSystem_Actions inputActions;

        private void OnEnable()
        {
            inputActions = new InputSystem_Actions();
            inputActions.Player.AddCallbacks(this);
            inputActions.Player.Enable();
        }
        private void OnDisable()
        {
            inputActions.Player.Disable();
        }

        // Input System callback implementations
        public void OnMove(InputAction.CallbackContext context) { _movementValue = context.ReadValue<Vector2>(); }
        public void OnLook(InputAction.CallbackContext context) { _lookValue = context.ReadValue<Vector2>(); }
        public void OnAttack(InputAction.CallbackContext context) { _shootPressed = context.performed; }
        public void OnInteract(InputAction.CallbackContext context) { }
        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                // Toggle crouch state on button press
                _crouchPressed = !_crouchPressed;
            }
        }
        public void OnJump(InputAction.CallbackContext context) { _jumpPressed = context.performed; }
        public void OnPrevious(InputAction.CallbackContext context) { }
        public void OnNext(InputAction.CallbackContext context) { }
        public void OnSprint(InputAction.CallbackContext context) { _sprintPressed = context.performed; }
    }
}