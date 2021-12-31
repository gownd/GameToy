using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] GameObject blackOut = null;

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

    public IEnumerator LoadSceneByIndex(int index)
    {
        FindObjectOfType<GametoyController>().ResetControls();

        blackOut.SetActive(true);
        
        yield return new WaitForSeconds(0.05f);
        yield return SceneManager.LoadSceneAsync(index);

        blackOut.SetActive(false);
    }

    public IEnumerator StartRamio()
    {
        FindObjectOfType<GametoyController>().ResetControls();

        yield return SceneManager.LoadSceneAsync(6);
    }
}
