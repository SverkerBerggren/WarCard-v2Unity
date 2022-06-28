using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MoveButtonClick : MonoBehaviour
{
    public MainUI mainUI;
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

    public void moveButtonClick()
    {
        //print("hej");
        mainUI.MoveActionSelected = true; 
    }
}
