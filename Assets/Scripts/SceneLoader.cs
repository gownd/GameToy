using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void TurnOffGametoy()
    {
        FindObjectOfType<GametoyController>().ResetControls();
        SceneManager.LoadScene(0);
    }

    public void TurnOnGametoy()
    {
        FindObjectOfType<GametoyController>().ResetControls();
        SceneManager.LoadScene(1);
    }

    public void LoadProtoScene()
    {
        FindObjectOfType<GametoyController>().ResetControls();
        SceneManager.LoadScene(2);
    }

    public void LoadSceneByIndex(int index)
    {
        FindObjectOfType<GametoyController>().ResetControls();
        SceneManager.LoadScene(index);
    }
}
