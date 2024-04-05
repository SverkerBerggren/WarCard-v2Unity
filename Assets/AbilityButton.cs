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

    [SerializeField]private Image buttonImage;

    public bool activatedAbility = false; 

    public int abilityIndex = -1;

    public string abilityName;

    public string abilityFlavour;

    public string abilityDescription;

    public GameObject informationPopUp;

    private bool clickable = true;

    public TextMeshProUGUI abilityFlavourText;
    public TextMeshProUGUI abilityDescriptionText;
    public TextMeshProUGUI abilityNameText;

    public AbilityClickHandler clickHandlerAbility;
    public ClickHandlerUnitSelect clickHandlerUnitSelect;

    public int playerIndex;

    public RuleManager.UnitInfo unitInfo;


    // Start is called before the first frame update
    void Start()
    {

        //    buttonImage = GetComponent<Image>();
        clickHandlerAbility = FindObjectOfType<AbilityClickHandler>();

        informationPopUp.SetActive(false);

        abilityFlavourText.text = abilityFlavour;

        abilityDescriptionText.text = abilityDescription;


        abilityNameText.text = abilityName;

        mainUI = GameObject.Find("UI").GetComponent<MainUI>();
        clickHandlerAbility = FindObjectOfType<AbilityClickHandler>();
        clickHandlerUnitSelect = FindObjectOfType<ClickHandlerUnitSelect>();
        
        
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

        if(!clickable)
        {
            return;
        }
        clickHandlerAbility.selectedAbilityIndex = abilityIndex;
        clickHandlerUnitSelect.ActivateAbilitySelection();
        clickHandlerAbility.requiredAbilityTargets = new List<RuleManager.TargetCondition>(whichTargets);


        if(whichTargets.Count == 0)
        {
           



            RuleManager.EffectAction abilityToExecute = new RuleManager.EffectAction();

            abilityToExecute.EffectIndex = abilityIndex;
            abilityToExecute.PlayerIndex = playerIndex ;

       //     List<RuleManager.Target> argumentList = new List<RuleManager.Target>(selectedTargetsForAbilityExecution);

            abilityToExecute.Targets = new List<RuleManager.Target>();

            abilityToExecute.UnitID = unitInfo.UnitID;

            mainUI.EnqueueAction(abilityToExecute);
            unitInfo.AbilityActivated[abilityIndex] = true;
            CanvasUiScript.instance.createUnitCard(unitInfo);

        }

    //    if(whichTargets.Count != 0)
    //    {
    //        clickHandlerAbility.requiredAbilityTargets =  new List<RuleManager.TargetCondition>(whichTargets); 
    //    }
    //    else
    //    {
    //        mainUI.OnClick(ClickType.leftClick, new RuleManager.Coordinate(0, 0));
    //        clickHandlerAbility.requiredAbilityTargets = new List<RuleManager.TargetCondition>();
    //    }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetClickable(bool clickable)
    {
        this.clickable = clickable;

        if(!clickable )
        {
            buttonImage.color = new Color(255, 255, 255, 0.5f);
        }

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
