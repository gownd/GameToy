using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActivator : MonoBehaviour
{
    bool isActive = false;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            isActive = true;
        }
    }

    public bool IsActive()
    {
        return isActive;
    }
}
