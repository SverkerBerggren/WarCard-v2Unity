using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] int timer;
    [SerializeField] float heightGain;
    [SerializeField] TextMeshProUGUI text;
    private RectTransform rectTransform;
    private float alphaReduction;
    private float alphaToSet =1; 

    private void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        alphaReduction =(float) 1f/ timer;
    }
    // Update is called once per frame
    void FixedUpdate()
    {   
        timer -= 1;
        if(timer <= 0 )
        {
            Destroy( gameObject);
        }
        else
        {
            alphaToSet -= alphaReduction;
            Color colorToSet = new Color(text.color.r, text.color.g, text.color.b, alphaToSet);
          //  Color32 colorToSet32 = new Color32(((byte)text.color.r),((byte)text.color.g), ((byte)text.color.b), ((byte)(text.color.a -alphaReduction)));
            text.color = colorToSet;
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + heightGain);   
        }


    }
}
