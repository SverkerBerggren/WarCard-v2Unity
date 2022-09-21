using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class ObjectiveContestionIndicator : MonoBehaviour
{
    public TextMeshProUGUI player1text;
    public TextMeshProUGUI player2text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setPoints(int playerIndex, int points)
    {
        if(playerIndex == 0)
        {
            player1text.text = "Player 1: " + points;
        }
        else
        {
            player2text.text = "Player 2: " + points;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
