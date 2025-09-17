// 9/17/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DynamicJoystick : MonoBehaviour
{
    private PlayerMovements mPlayerMovement;
    public RectTransform joystickBackground;
    public RectTransform joystickHandle;
    public Canvas canvas;

    private Vector2 joystickStartPosition;
    private bool mIsPressing = false;

    private void Start()
    {
        mPlayerMovement = new PlayerMovements();
        joystickBackground.gameObject.SetActive(false);
        mPlayerMovement.InGame.PressAndHold.performed += OnPressPerformed;
        mPlayerMovement.InGame.PressAndHold.canceled += OnPressCanceled;
        mPlayerMovement.InGame.PressAndHold.Enable();
    }

    public void OnPressPerformed(InputAction.CallbackContext context)
    {
        Vector2 touchPosition = Pointer.current.position.ReadValue();

        // Convert screen position to canvas position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            touchPosition,
            canvas.worldCamera,
            out joystickStartPosition
        );

        // Position the joystick and make it visible
        joystickBackground.anchoredPosition = joystickStartPosition;
        joystickBackground.gameObject.SetActive(true);
        mIsPressing = true;
    
    }

    private void Update()
    {
        if (mIsPressing)
        {
            Vector2 touchPosition = Pointer.current.position.ReadValue();

            // Convert screen position to canvas position
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                touchPosition,
                canvas.worldCamera,
                out Vector2 joystickPosition
            );

            // Calculate the direction and clamp the handle within the background
            Vector2 direction = joystickPosition - joystickStartPosition;
            float radius = joystickBackground.sizeDelta.x / 2;
            direction = Vector2.ClampMagnitude(direction, radius);

            joystickHandle.anchoredPosition = direction;
        }
    }

    public void OnPressCanceled(InputAction.CallbackContext context)
    {
        // Reset and hide the joystick
        joystickHandle.anchoredPosition = Vector2.zero;
        joystickBackground.gameObject.SetActive(false);
        mIsPressing = false;
    }
}
