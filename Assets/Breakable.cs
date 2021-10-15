using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    int health = 3;

    private void OnMouseDown() 
    {
        health -= 1;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
