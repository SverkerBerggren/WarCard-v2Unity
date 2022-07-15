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

        mainUI.SwitchPlayerPriority();

            currentPlayerText.text = "Current PLayer: " + currentPlayer;

        
    }
}
