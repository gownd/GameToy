using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSelector : MonoBehaviour
{
    [SerializeField] GameObject selected = null;
    [SerializeField] int sceneToLoad;

    public void SwitchSelector(bool on)
    {
        selected.SetActive(on);
    }

    public int GetSceneToLoad()
    {
        return sceneToLoad;
    }
}
