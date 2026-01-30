using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CIDemo.Input;

namespace CIDemo.FPCharacter
{

    public class CameraController : MonoBehaviour
    {
        [Header("Look Settings")]
        [SerializeField] private float lookSensitivity = 2f;
        [SerializeField] private float lookXLimit = 80f;

        private IPlayerInput playerInput;
        private Camera playerCamera;
        private float rotationX = 0f;
        private bool canTick = false;

        public void Init(IPlayerInput input)
        {
            playerInput = input;
            playerCamera = Camera.main;
            canTick = true;

            //Setup cursor when using camera
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Tick()
        {
            if (!canTick) return;
            HandleRotation();
        }

        private void HandleRotation()
        {
            // Get mouse input
            Vector2 lookInput = playerInput.Look;

            // Rotate character horizontally (parent object)
            transform.Rotate(0, lookInput.x * lookSensitivity, 0);

            // Rotate camera vertically
            rotationX += lookInput.y * lookSensitivity;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(-rotationX, 0, 0);
        }


    }
}