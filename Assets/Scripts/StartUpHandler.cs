using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUpHandler : MonoBehaviour
{
    [SerializeField] float timeToWait = 4.8f;

    private void Start()
    {
        StartCoroutine(HandleStartUpScene());
    }

    IEnumerator HandleStartUpScene()
    {
        yield return new WaitForSeconds(timeToWait);

        FindObjectOfType<SceneLoader>().LoadProtoScene();
    }
}
