using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MoreMountains.NiceVibrations;
using UnityEngine.UI;

public class GametoyController : MonoBehaviour
{
    [SerializeField] GameObject buttonA = null;
    [SerializeField] GameObject buttonB = null;
    [SerializeField] GameObject buttonStart = null;
    [SerializeField] GameObject buttonSelect = null;
    [SerializeField] GameObject buttonArrow = null;


    public delegate void HandleButtonPress(InputAction.CallbackContext context);
    public HandleButtonPress HandlePressA;
    public HandleButtonPress HandlePressB;
    public HandleButtonPress HandlePressStart;
    public HandleButtonPress HandlePressSelect;

    public delegate void HandlePressArrowBy(Vector2 inputValue, InputActionPhase phase);
    public HandlePressArrowBy HandlePressArrow;

    public void OnPressA(InputAction.CallbackContext context)
    {
        AnimateButton(buttonA, context.phase);

        HandlePressA?.Invoke(context);
    }

    public void OnPressB(InputAction.CallbackContext context)
    {
        AnimateButton(buttonB, context.phase);

        HandlePressB?.Invoke(context);
    }

    public void OnPressStart(InputAction.CallbackContext context)
    {
        AnimateButton(buttonStart, context.phase);

        HandlePressStart?.Invoke(context);
    }

    public void OnPressSelect(InputAction.CallbackContext context)
    {
        AnimateButton(buttonSelect, context.phase);

        HandlePressSelect?.Invoke(context);
    }

    public void OnPressArrow(InputAction.CallbackContext context)
    {
        AnimateArrowButton(context.phase == InputActionPhase.Performed);

        HandlePressArrow?.Invoke(context.ReadValue<Vector2>(), context.phase);
    }

    public void OnPressArrowByTouch(int direction) // Event Trigger에서 실행됨
    {
        // 0:up 1:down 2:left 3:right 10:none
        Vector2 inputValue = new Vector2();
        if (direction == 0) inputValue = new Vector2(0f, 1f);
        else if (direction == 1) inputValue = new Vector2(0f, -1f);
        else if (direction == 2) inputValue = new Vector2(-1f, 0f);
        else if (direction == 3) inputValue = new Vector2(1f, 0f);
        else if (direction == 10) inputValue = new Vector2(0f, 0f);

        AnimateArrowButton(direction < 4);

        HandlePressArrow?.Invoke(inputValue, InputActionPhase.Performed);
    }

    void AnimateButton(GameObject buttonPressed, InputActionPhase phase)
    {
        if (phase == InputActionPhase.Performed)
        {
            buttonPressed.GetComponent<Animator>().SetBool("isPressed", true);
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
        }
        else if (phase == InputActionPhase.Canceled)
        {
            buttonPressed.GetComponent<Animator>().SetBool("isPressed", false);

            MMVibrationManager.Haptic(HapticTypes.Selection);
        }
    }

    public void AnimateArrowButton(bool isPressed)
    {
        if (isPressed)
        {
            buttonArrow.GetComponent<Animator>().SetBool("isPressed", true);
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
        }
        else
        {
            buttonArrow.GetComponent<Animator>().SetBool("isPressed", false);
            MMVibrationManager.Haptic(HapticTypes.Selection);
        }
    }

    public void ResetControls()
    {
        HandlePressA = null;
        HandlePressB = null;
        HandlePressStart = null;
        HandlePressSelect = null;
        HandlePressArrow = null;
    }
}
