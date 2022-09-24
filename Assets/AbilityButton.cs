using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AbilityButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public List<RuleManager.TargetCondition> whichTargets = null;
    public MainUI mainUI; 
    RuleManager.RuleManager ruleManager;

    public bool activatedAbility = false; 

    public int abilityIndex = -1;

    public string abilityName;

    public string abilityFlavour;

    public string abilityDescription;

    public GameObject informationPopUp;

    public TextMeshProUGUI abilityFlavourText;
    public TextMeshProUGUI abilityDescriptionText;
    public TextMeshProUGUI abilityNameText;

    // Start is called before the first frame update
    void Start()
    {
        informationPopUp.SetActive(false);

        abilityFlavourText.text = abilityFlavour;

        abilityDescriptionText.text = abilityDescription;


        print("vad heter abilityn i knappen");

        abilityNameText.text = abilityName;

        mainUI = GameObject.Find("UI").GetComponent<MainUI>();

        
        
    // ruleManager.GetPossibleTargets(abilityInfo.ActivationTargets).;

    //     foreach( RuleManager.Target target in ruleManager.GetPossibleTargets(abilityInfo.ActivationTargets))
    //     {
    //         //target.
    //         if(target.Type == RuleManager.TargetType.Unit)
    //         {
    //             
    //         }
    //
    //     }
    }   


    public void AbilityButtonClick()
    {
        mainUI.abilitySelectionStarted = true;

        mainUI.selectedAbilityIndex = abilityIndex;

        if(whichTargets.Count != 0)
        {
            mainUI.requiredAbilityTargets =  new List<RuleManager.TargetCondition>(whichTargets); 
        }
        else
        {
            mainUI.OnClick(ClickType.leftClick, new RuleManager.Coordinate(0, 0));
            mainUI.requiredAbilityTargets = new List<RuleManager.TargetCondition>();
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
