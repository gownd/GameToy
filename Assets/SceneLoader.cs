using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.NiceVibrations;

public class SceneLoader : MonoBehaviour
{
    public void PowerGameToy()
    {
        MMVibrationManager.Haptic(HapticTypes.Success);

        SceneManager.LoadScene(1);
    }
}
