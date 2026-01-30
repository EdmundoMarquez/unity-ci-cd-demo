using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CIDemo.Input;

namespace CIDemo.FPCharacter
{

    [RequireComponent(typeof(CharacterController))]
    public class MovementController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float sprintSpeed = 10f;
        [SerializeField] private float crouchSpeed = 2.5f;
        [SerializeField] private float jumpHeight = 2f;
        [SerializeField] private float gravity = -9.81f;

        [Header("Crouch Settings")]
        [SerializeField] private float standingHeight = 2f;
        [SerializeField] private float crouchingHeight = 1f;
        [SerializeField] private float crouchSpeedSmooth = 10f;

        private CharacterController characterController;
        private IPlayerInput playerInput;

        private Vector3 moveDirection = Vector3.zero;
        private Vector3 playerVelocity;
        private bool isCrouching = false;
        private bool isSprinting = false;
        private float currentHeight;
        private bool canTick = false;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();

            // Initialize height
            currentHeight = standingHeight;
            characterController.height = currentHeight;
        }

        public void Init(IPlayerInput input)
        {
            playerInput = input;
            canTick = true;
        }

        public void Tick()
        {
            if (!canTick) return;

            HandleGroundCheck();
            HandleSprint();
            HandleCrouching();
            HandleMovement();
        }

        private void HandleMovement()
        {
            // Calculate movement direction
            Vector2 input = playerInput.Move;
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            // Remove vertical component from forward vector
            forward.y = 0;
            right.y = 0;

            // Normalize vectors
            forward.Normalize();
            right.Normalize();

            // Calculate movement direction
            Vector3 targetDirection = (forward * input.y + right * input.x).normalized;

            // Determine speed based on input
            float currentSpeed = walkSpeed;
            if (isSprinting && input.magnitude > 0.1f)
            {
                currentSpeed = sprintSpeed;
            }
            else if (isCrouching)
            {
                currentSpeed = crouchSpeed;
            }

            // Apply movement
            if (targetDirection != Vector3.zero)
            {
                moveDirection.x = targetDirection.x * currentSpeed;
                moveDirection.z = targetDirection.z * currentSpeed;
            }
            else
            {
                // Smoothly stop when no input
                moveDirection.x = Mathf.Lerp(moveDirection.x, 0, Time.deltaTime * 10f);
                moveDirection.z = Mathf.Lerp(moveDirection.z, 0, Time.deltaTime * 10f);
            }

            // Apply ground check calculations
            moveDirection.y = playerVelocity.y;

            // Apply movement to character controller
            characterController.Move(moveDirection * Time.deltaTime);
        }

        private void HandleCrouching()
        {
            isCrouching = playerInput.CrouchPressed;

            // Smoothly interpolate height
            float targetHeight = isCrouching ? crouchingHeight : standingHeight;
            currentHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * crouchSpeedSmooth);

            // Update character controller height
            characterController.height = currentHeight;
        }

        private void HandleGroundCheck()
        {
            // Check if the character is grounded
            if (characterController.isGrounded)
            {
                // Reset vertical velocity to a small negative value to ensure the character stays grounded
                moveDirection.y = -2f;

                if (playerInput.JumpPressed)
                {
                    Debug.Log("Jump!");
                    // Calculate the upward velocity needed to reach the desired jump height
                    // Formula: Mathf.Sqrt(jumpHeight * -2f * gravity)
                    playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }
            }
            else
            {
                // Apply gravity when not grounded
                playerVelocity.y += gravity * Time.deltaTime;
            }
        }

        public void HandleSprint()
        {
            isSprinting = playerInput.SprintPressed;
        }
    }
}