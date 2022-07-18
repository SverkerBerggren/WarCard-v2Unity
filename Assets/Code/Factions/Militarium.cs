using System.Collections;
using System.Collections.Generic;

public class Militarium 
{

    public RuleManager.UnitInfo GetFootSoldier()
    {
        RuleManager.UnitInfo NewUnitInfo = new RuleManager.UnitInfo();
        NewUnitInfo.Stats.Movement = 6;
        NewUnitInfo.Stats.Damage = 35;
        NewUnitInfo.Stats.Range = 6;
        NewUnitInfo.Stats.HP = 100;
        NewUnitInfo.Stats.ActivationCost = 15;
        return (NewUnitInfo);
    }

    public RuleManager.UnitInfo GetMeleeSoldier()
    {
        RuleManager.UnitInfo NewUnitInfo = new RuleManager.UnitInfo();
        NewUnitInfo.Stats.Movement = 7;
        NewUnitInfo.Stats.Damage = 50;
        NewUnitInfo.Stats.Range = 1;
        NewUnitInfo.Stats.HP = 100;
        NewUnitInfo.Stats.ActivationCost = 15;
        return (NewUnitInfo);
    }

    public RuleManager.UnitInfo GetArtillery()
    {
        RuleManager.UnitInfo NewUnitInfo = new RuleManager.UnitInfo();
        NewUnitInfo.Stats.Movement = 4;
        NewUnitInfo.Stats.Damage = 25;
        NewUnitInfo.Stats.Range = 30;
        NewUnitInfo.Stats.HP = 200;
        NewUnitInfo.Stats.ActivationCost = 30;

        RuleManager.Ability_Activated BarrageAbility = new RuleManager.Ability_Activated(
                new RuleManager.TargetInfo_List(new RuleManager.TargetCondition_And(new RuleManager.TargetCondition_Type(RuleManager.TargetType.Tile),new RuleManager.TargetCondition_Range(30))),
                new RuleManager.Effect_RegisterTrigger(true,false,new RuleManager.TargetRetriever_Index(0),new RuleManager.TriggerCondition_Type(RuleManager.TriggerType.BattleroundBegin),
                        new RuleManager.Effect_DamageArea(new RuleManager.TargetRetriever_Index(0),2,50)
                    )
            );
        NewUnitInfo.Abilities.Add(BarrageAbility);
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
        MoveAbility.ActivationTargets = 
            new RuleManager.TargetInfo_List(
            new RuleManager.TargetCondition_And(new RuleManager.TargetCondition_Type(RuleManager.TargetType.Unit),new RuleManager.TargetCondition_Range(6)), 
            new RuleManager.TargetCondition_And(new RuleManager.TargetCondition_Type(RuleManager.TargetType.Tile),new RuleManager.TargetCondition_Range(0,3)));
        MoveAbility.ActivatedEffect = new RuleManager.Effect_MoveUnit(new RuleManager.TargetRetriever_Index(0), new RuleManager.TargetRetriever_Index(1));

        RuleManager.Ability_Activated IncreaseDamage = new RuleManager.Ability_Activated();
        RuleManager.Effect_RegisterContinousAbility EffectResult = new RuleManager.Effect_RegisterContinousAbility();
        EffectResult.ContinousEffect = new RuleManager.Effect_IncreaseDamage(50);
        EffectResult.OptionalAffectedTarget = new RuleManager.TargetRetriever_Index(0);

        IncreaseDamage.ActivatedEffect = EffectResult;
        IncreaseDamage.ActivationTargets = new RuleManager.TargetInfo_List(new RuleManager.TargetCondition_Type(RuleManager.TargetType.Unit));

        RuleManager.Ability_Activated IncreaseMovement = new RuleManager.Ability_Activated(
                new RuleManager.TargetInfo_List(new RuleManager.TargetCondition_And(new RuleManager.TargetCondition_Type(RuleManager.TargetType.Unit), new RuleManager.TargetCondition_Range(6))),
                new RuleManager.Effect_RegisterContinousAbility(new RuleManager.TargetRetriever_Index(0), new RuleManager.TargetCondition_True(),new RuleManager.Effect_IncreaseMovement(5))
            );

        ReturnValue.Abilities.Add(IncreaseMovement);
        ReturnValue.Abilities.Add(IncreaseDamage);
        ReturnValue.Abilities.Add(MoveAbility);
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
