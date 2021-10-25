using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MoreMountains.NiceVibrations;

public class GametoyController : MonoBehaviour
{
    [SerializeField] GameObject buttonA = null;
    [SerializeField] GameObject buttonB = null;
    [SerializeField] GameObject buttonStart = null;
    [SerializeField] GameObject buttonSelect = null;
    [SerializeField] GameObject buttonArrow = null;

    public void OnPressA(InputAction.CallbackContext context)
    {
        HandleButtonPress(buttonA, context.phase);
    }

    public void OnPressB(InputAction.CallbackContext context)
    {
        HandleButtonPress(buttonB, context.phase);
    }

        public void OnPressStart(InputAction.CallbackContext context)
    {
        HandleButtonPress(buttonStart, context.phase);
    }

        public void OnPressSelect(InputAction.CallbackContext context)
    {
        HandleButtonPress(buttonSelect, context.phase);
    }

    public void OnPressArrow(InputAction.CallbackContext context)
    {
        HandleButtonPress(buttonArrow, context.phase);
    }

    void HandleButtonPress(GameObject buttonPressed, InputActionPhase phase)
    {
        if(phase == InputActionPhase.Performed)
        {
            buttonPressed.GetComponent<Animator>().SetBool("isPressed", true);
            MMVibrationManager.Haptic(HapticTypes.Selection);
        }
        else if(phase == InputActionPhase.Canceled)
        {
            buttonPressed.GetComponent<Animator>().SetBool("isPressed", false);
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
        }
    }
}
