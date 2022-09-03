using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActions : MonoBehaviour
{
    public float paddingBetweenButtons = 400;

    public float heightPadding = -300; 

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

        float firstButtonXPosition = (paddingBetweenButtons * (amountOfChildren -1) / 2) * -1;

        print(amountOfChildren);

        for(int i = 0; i < amountOfChildren; i++)
        {
            RectTransform transform =  gameObject.transform.GetChild(i).GetComponent<RectTransform>();

            transform.anchoredPosition3D = new Vector3(firstButtonXPosition, heightPadding, 1);

            transform.localScale = new Vector3(2, 2, 2);

            firstButtonXPosition += paddingBetweenButtons; 
        }



    }
}
