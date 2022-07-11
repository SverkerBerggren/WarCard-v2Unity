using System.Collections;
using System.Collections.Generic;


public class Templars 
{
    static public RuleManager.UnitInfo GetKnight()
    {
        RuleManager.UnitInfo ReturnValue = new RuleManager.UnitInfo();
        ReturnValue.Stats = new RuleManager.UnitStats();
        ReturnValue.Stats.ActivationCost = 40;
        ReturnValue.Stats.Damage = 100;
        ReturnValue.Stats.HP = 300;
        ReturnValue.Stats.Range = 2;

        RuleManager.Ability_Activated ActivatedAbility = new RuleManager.Ability_Activated();
        RuleManager.Effect_DealDamage DamageEffect = new RuleManager.Effect_DealDamage();
        DamageEffect.Damage = 50;
        DamageEffect.Targets = new RuleManager.TargetRetriever_Index(0);
        
        ActivatedAbility.ActivatedEffect = DamageEffect;
        
        RuleManager.TargetInfo_List AbilityTargetInfo = new RuleManager.TargetInfo_List();
        AbilityTargetInfo.Targets.Add(new RuleManager.TargetCondition_Type(RuleManager.TargetType.Unit));
        
        ActivatedAbility.ActivationTargets = AbilityTargetInfo;

        ReturnValue.Abilities.Add(ActivatedAbility);
        return (ReturnValue);
    }
    public static RuleManager.UnitInfo GetPriest()
    {
        return (null);
    }
    public static  RuleManager.UnitInfo GetInquisitor()
    {
        return (null);
    }
    public static  RuleManager.UnitInfo GetRelic()
    {
        return (null);
    }

}
