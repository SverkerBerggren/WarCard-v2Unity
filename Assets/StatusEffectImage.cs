using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using RuleManager;
using UnitScript;

public class StatusEffectImage : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    RuleManager.RuleManager ruleManager;

    [SerializeField] private Image buttonImage;



    public GameObject informationPopUp;

    private bool clickable = true;

    [SerializeField] private TextMeshProUGUI abilityFlavourText;
    [SerializeField] private TextMeshProUGUI abilityDescriptionText;
    [SerializeField] private TextMeshProUGUI abilityNameText;

    public int playerIndex;

    public RuleManager.UnitInfo unitInfo;




    public void Setup(AppliedContinousInfo effectInfo)
    {
        ruleManager = MainUI.instance.ruleManager;
        RuleManager.Ability ability = ruleManager.GetUnitInfo(effectInfo.UnitID).Abilities[effectInfo.AbilityIndex];

        informationPopUp.SetActive(false);
        ResourceManager.UnitResource unitResource = MainUI.g_ResourceManager.GetUnitResource(effectInfo.UnitID);
        AbilityInformation abilityInformation = unitResource.TotalAbilities[effectInfo.AbilityIndex];
        if(abilityInformation.Icon != null)
        {
            buttonImage.sprite = ((ResourceManager.Visual_Image)abilityInformation.Icon.VisualInfo).Sprite;
        }
        abilityFlavourText.text = abilityInformation.Ability.GetFlavourText();
        abilityDescriptionText.text = abilityInformation.Ability.GetDescription();
     //   abilityNameText.text = abilityInformation.Ability.GetName();

    }


    public void AbilityButtonClick()
    {

        if (!clickable)
        {
            return;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && !informationPopUp.activeInHierarchy)
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
