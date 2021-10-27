using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestDisplay : MonoBehaviour
{
    [SerializeField] GameObject green = null;
    [SerializeField] GameObject color = null;

    private void Start()
    {
        AddControls();
    }

    void AddControls()
    {
        FindObjectOfType<GametoyController>().HandlePressSelect += ShowGreen;
        FindObjectOfType<GametoyController>().HandlePressStart += ShowColor;
    }

    void ShowGreen(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Canceled)
        {
            color.SetActive(false);
            green.SetActive(!green.activeSelf);
        }


    }

    void ShowColor(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Canceled)
        {
            green.SetActive(false);
            color.SetActive(!color.activeSelf);
        }

    }
}
