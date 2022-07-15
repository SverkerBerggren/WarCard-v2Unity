using System.Collections;
using System.Collections.Generic;

public class Militarium 
{

    public RuleManager.UnitInfo GetFootSoldier()
    {
        RuleManager.UnitInfo NewUnitInfo = new RuleManager.UnitInfo();
        NewUnitInfo.Stats.Movement = 6;
        NewUnitInfo.Stats.Damage = 6;
        NewUnitInfo.Stats.Range = 6;
        NewUnitInfo.Stats.HP = 6;
        NewUnitInfo.Stats.ActivationCost = 15;
        return (NewUnitInfo);
    }

    public RuleManager.UnitInfo GetOfficer()
    {
        RuleManager.UnitInfo ReturnValue = new RuleManager.UnitInfo();
        ReturnValue.Stats.HP = 80;
        ReturnValue.Stats.Damage = 20;
        ReturnValue.Stats.Movement = 6;
        ReturnValue.Stats.Range = 8;
        ReturnValue.Stats.ActivationCost = 15;

        RuleManager.Ability_Activated MoveAbility = new RuleManager.Ability_Activated();
        MoveAbility.ActivationTargets = new RuleManager.TargetInfo_List(new RuleManager.TargetCondition_Type(RuleManager.TargetType.Unit), new RuleManager.TargetCondition_Type(RuleManager.TargetType.Tile));

        return (ReturnValue);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
