using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectGameHandler : MonoBehaviour
{
    [SerializeField] GameSelector[] gameSelectors = null;
    [SerializeField] float timeToLoad = 1f;
    int currentIndex = 0;

    bool isLoadingScene = false;

    private void Start()
    {
        AddControls();
        UpdateSelectors();
    }

    void AddControls()
    {
        GametoyController gametoyController = FindObjectOfType<GametoyController>();

        gametoyController.HandlePressArrow += HandleChangeTarget;
        gametoyController.HandlePressA += HandleSelect;
    }

    void HandleChangeTarget(Vector2 inputValue, InputActionPhase phase)
    {
        if (phase == InputActionPhase.Performed)
        {
            ChangeSelectedTarget(inputValue.y);
        }
    }

    void HandleSelect(InputAction.CallbackContext context)
    {
        if (isLoadingScene) return;

        if (context.performed)
        {
            StartCoroutine(LoadSelectedGame(gameSelectors[currentIndex].GetSceneToLoad()));
        }
    }

    void ChangeSelectedTarget(float direction)
    {
        if (isLoadingScene) return;

        currentIndex -= (int)direction;

        if (currentIndex < 0)
        {
            currentIndex = gameSelectors.Length - 1;
        }
        else if (currentIndex >= gameSelectors.Length)
        {
            currentIndex = 0;
        }

        UpdateSelectors();
    }

    void UpdateSelectors()
    {
        foreach (GameSelector selector in gameSelectors)
        {
            selector.SwitchSelector(false);
        }

        gameSelectors[currentIndex].SwitchSelector(true);
    }

    IEnumerator LoadSelectedGame(int sceneIndex)
    {
        yield return new WaitForSeconds(timeToLoad);

        FindObjectOfType<SceneLoader>().LoadSceneByIndex(sceneIndex);
    }
}
