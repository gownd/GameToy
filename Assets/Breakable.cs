using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class Breakable : MonoBehaviour
{
    int health = 3;

    private void OnMouseDown() 
    {
        MMVibrationManager.Haptic(HapticTypes.RigidImpact);
        health -= 1;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
