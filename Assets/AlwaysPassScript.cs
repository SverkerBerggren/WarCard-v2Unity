using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysPassScript : MonoBehaviour
{

    public MainUI mainUi;

    public GameObject checkmark; 
    // Start is called before the first frame update
    void Start()
    {
        mainUi = FindObjectOfType<MainUI>();

        checkmark.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void alwaysPassClick()
    {
        mainUi.enableAlwaysPass();
        
        if(mainUi.alwaysPassPriority)
        {
            checkmark.SetActive(true);
        }
        else
        {
            checkmark.SetActive(false);
        }
        
        
    }
}
