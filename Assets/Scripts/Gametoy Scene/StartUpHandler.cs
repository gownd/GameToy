using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUpHandler : MonoBehaviour
{
    [SerializeField] float timeToWaitAnimation = 4.8f;
    [SerializeField] float timeToWaitLoad = 0.5f;
    [SerializeField] GameObject canvas = null;

    private void Start()
    {
        StartCoroutine(HandleStartUpScene());
    }

    IEnumerator HandleStartUpScene()
    {
        yield return new WaitForSeconds(timeToWaitAnimation);

        canvas.SetActive(false);
        yield return new WaitForSeconds(timeToWaitLoad);

        FindObjectOfType<SceneLoader>().LoadProtoScene();
    }
}
