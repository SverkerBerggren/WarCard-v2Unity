using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class Clickable : MonoBehaviour
{

    
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public virtual void OnClick(ClickType Type)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


public enum ClickType
{
    leftClick, rightClick,middleClick
}

