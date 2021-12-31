using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;

public class PowerSlider : MonoBehaviour
{
    [SerializeField] Slider powerSldier = null;
    bool isPowerOn = false;

    bool isPulling = false;
    bool hasPowerChanged = false;

    int firstPullCounter = 0;
    float waitTimeToSwitch = 1f;
    float currentPulledTime = 0f;



    private void Update()
    {
        HandlePullSlider();
        SwitchGametoy();
    }

    private void HandlePullSlider()
    {
        if (powerSldier.value >= 1f)
        {
            isPulling = true;


            if (firstPullCounter < 1)
            {
                MMVibrationManager.Haptic(HapticTypes.LightImpact);
                firstPullCounter++;
            }
        }
        else
        {
            isPulling = false;
            firstPullCounter = 0;
        }
    }

    private void SwitchGametoy()
    {
        if (hasPowerChanged) return;

        if (isPulling)
        {
            if (currentPulledTime >= waitTimeToSwitch)
            {
                if (isPowerOn)
                {
                    FindObjectOfType<SceneLoader>().TurnOffGametoy();

                    currentPulledTime = 0f;
                    hasPowerChanged = true;
                    isPowerOn = false;
                }
                else
                {
                    MMVibrationManager.Haptic(HapticTypes.Success);
                    FindObjectOfType<SceneLoader>().TurnOnGametoy();

                    currentPulledTime = 0f;
                    hasPowerChanged = true;
                    isPowerOn = true;
                }
            }
            else currentPulledTime += Time.deltaTime;
        }
    }

    public void OnPointerDown()
    {
        MMVibrationManager.Haptic(HapticTypes.Selection);
    }

    public void OnPointerUp()
    {
        MMVibrationManager.Haptic(HapticTypes.LightImpact);

        currentPulledTime = 0f;
        powerSldier.value = 0f;
        hasPowerChanged = false;
    }
}
