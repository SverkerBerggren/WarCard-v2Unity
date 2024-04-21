using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{

    //private RuleManager.UnitInfo connectedUnit;
    private int maxHp = -1; 
    private int currentHp = -1; 
    [SerializeField] private Slider slider;
    private RectTransform rectTransform;
    [SerializeField] private float OffsetY = 0;
    [SerializeField] private float OffsetX = 0;



    public void SetUp(int maxHp)
    {
        this.maxHp = maxHp;
        currentHp = maxHp;
        rectTransform = GetComponent<RectTransform>();


    }

    public void AddOffset() 
    {
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + OffsetX, rectTransform.anchoredPosition.y + OffsetY);

    }
    public void SetFromDamage(int damageAmount)
    {
        currentHp -= damageAmount;
        slider.value =  (float)currentHp / (float)maxHp;
    }

 
}
