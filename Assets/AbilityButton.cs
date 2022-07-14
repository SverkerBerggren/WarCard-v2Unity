using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityButton : MonoBehaviour
{
    public List<RuleManager.TargetCondition> whichTargets = null;
    public MainUI mainUI; 
    RuleManager.RuleManager ruleManager;

    public bool activatedAbility = false; 

    public int abilityIndex = -1;
    // Start is called before the first frame update
    void Start()
    {

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

        if(whichTargets != null)
        {
            mainUI.requiredAbilityTargets = whichTargets; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
