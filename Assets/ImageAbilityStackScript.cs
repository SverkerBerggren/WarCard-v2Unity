using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class ImageAbilityStackScript : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public string abilityEffectString;

    public GameObject informationPopUp;

    public TextMeshProUGUI descriptionText; 
    // Start is called before the first frame update
    void Start()
    {
        informationPopUp.SetActive(false);


        descriptionText.text = abilityEffectString; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right && !informationPopUp.activeInHierarchy )
        {
            informationPopUp.SetActive(true);
        }
        else
        {
            informationPopUp.SetActive(false);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        informationPopUp.SetActive(false);
    }
}
