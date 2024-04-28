using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActions : MonoBehaviour
{
    public float paddingBetweenButtons = 100;

    public float heightPadding = -270; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    [ContextMenu("Sort children")]
    public void sortChildren()
    {
        int amountOfChildren = gameObject.transform.childCount;
        float firstButtonXPosition = 0;
        if(amountOfChildren < 4)
        {
            firstButtonXPosition = (paddingBetweenButtons * (amountOfChildren - 1) / 2) * -1;
        }
        else
        {
            firstButtonXPosition = ((paddingBetweenButtons -100) * (amountOfChildren - 1) / 2) * -1;
        }
         




        for(int i = 0; i < amountOfChildren; i++)
        {
            RectTransform transform =  gameObject.transform.GetChild(i).GetComponent<RectTransform>();

            transform.anchoredPosition3D = new Vector3(firstButtonXPosition, heightPadding, 1);

            transform.localScale = new Vector3(1, 1, 1);

          //  firstButtonXPosition += paddingBetweenButtons;
            if (amountOfChildren < 4)
            {
                firstButtonXPosition += paddingBetweenButtons;
            }
            else
            {
                firstButtonXPosition += paddingBetweenButtons -100;
            }
        }



    }


}
