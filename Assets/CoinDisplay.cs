using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinDisplay : MonoBehaviour
{
    [SerializeField] Text coinText = null;

    GameData gameData;

    private void Awake() 
    {
        gameData = FindObjectOfType<GameData>();    
    }

    private void Update() 
    {
        coinText.text = gameData.GetScore().ToString();    
    }
}
