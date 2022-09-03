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
        float firstButtonXPosition = 0;
        if(amountOfChildren < 4)
        {
            firstButtonXPosition = (paddingBetweenButtons * (amountOfChildren - 1) / 2) * -1;
        }
        else
        {
            firstButtonXPosition = ((paddingBetweenButtons -100) * (amountOfChildren - 1) / 2) * -1;
        }
         



        print(amountOfChildren);

        for(int i = 0; i < amountOfChildren; i++)
        {
            RectTransform transform =  gameObject.transform.GetChild(i).GetComponent<RectTransform>();

            transform.anchoredPosition3D = new Vector3(firstButtonXPosition, heightPadding, 1);

            transform.localScale = new Vector3(2, 2, 2);

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

    public void clearAbilityButtons()
    {
        List<GameObject> buttonsToDestroy = new List<GameObject>();
        for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            if(i == 0 || i == 1)
            {

            }
            else
            {
                buttonsToDestroy.Add(gameObject.transform.GetChild(i).gameObject);
            }
        }

        foreach(GameObject obj in buttonsToDestroy)
        {
            Destroy(obj);
        }

        buttonsToDestroy.Clear();
    }
}
