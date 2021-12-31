using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RamioTitleHandler : MonoBehaviour
{
    private void Start() 
    {
        FindObjectOfType<GametoyController>().HandlePressStart += HandlePressStart;
    }

    void HandlePressStart(InputAction.CallbackContext context)
    {
        FindObjectOfType<SceneLoader>().StartCoroutine(FindObjectOfType<SceneLoader>().StartRamio());
    }
}
