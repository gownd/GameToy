using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] PickUpType type;
    [SerializeField] int num = 1;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(!other.CompareTag("Player")) return;

        if(type == PickUpType.Coin)
        {
            FindObjectOfType<GameData>().AddScore(num);
        }
        else if(type == PickUpType.BigCoin)
        {
            FindObjectOfType<GameData>().AddScore(num*10);
        }

        Destroy(gameObject);
    }
}

public enum PickUpType
{
    Coin, BigCoin
}
