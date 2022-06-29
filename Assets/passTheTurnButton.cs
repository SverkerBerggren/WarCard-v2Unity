using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class passTheTurnButton : MonoBehaviour
{
    public MainUI mainUI;
   // public 
    public int currentPlayer = 0;

    public TextMeshProUGUI currentPlayerText; 
    // Start is called before the first frame update
    void Start()
    {

        mainUI = GameObject.Find("UI").GetComponent<MainUI>();
        //    GetComponent<Button>().
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void PassTheTurnButtonClick()
    {
        if(currentPlayer == mainUI.m_playerid)
        {
           if(currentPlayer == 0)
            {
                currentPlayer = 1;
                mainUI.SwitchPlayerPriority();
               
            }
           else
            {
                currentPlayer = 0;
                mainUI.SwitchPlayerPriority();
                //  mainUI.m_playerid = 0;
            }

            currentPlayerText.text = "Current PLayer: " + currentPlayer;

        }
    }
}
