using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityButton : MonoBehaviour
{
    public RuleManager.Ability_Activated abilityInfo;
    public MainUI mainUI; 
    RuleManager.RuleManager ruleManager;
    // Start is called before the first frame update
    void Start()
    {
       // ruleManager.GetPossibleTargets(abilityInfo.ActivationTargets).;

        foreach( RuleManager.Target target in ruleManager.GetPossibleTargets(abilityInfo.ActivationTargets))
        {
            //target.
            if(target.Type == RuleManager.TargetType.Unit)
            {
                
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
