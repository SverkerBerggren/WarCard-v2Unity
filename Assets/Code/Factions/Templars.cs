using System.Collections;
using System.Collections.Generic;


public class Templars 
{
    static RuleManager.Ability IncreaseDamage()
    {
        RuleManager.Ability_Activated ReturnValue = new RuleManager.Ability_Activated();
        RuleManager.Effect_RegisterContinousAbility EffectResult = new RuleManager.Effect_RegisterContinousAbility();
        EffectResult.ContinousEffect = new RuleManager.Effect_IncreaseDamage(50);
        EffectResult.OptionalAffectedTarget = new RuleManager.TargetRetriever_Index(0);
        
        ReturnValue.ActivatedEffect = EffectResult;
        ReturnValue.ActivationTargets = new RuleManager.TargetInfo_List(new RuleManager.TargetCondition_Type(RuleManager.TargetType.Unit));

        return (ReturnValue);
    }
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


        ReturnValue.Abilities.Add(ActivatedAbility);
        return (ReturnValue);
    }
    public static RuleManager.UnitInfo GetPriest()
    {
        RuleManager.UnitInfo ReturnValue = new RuleManager.UnitInfo();
        ReturnValue.Stats = new RuleManager.UnitStats();
        ReturnValue.Stats.ActivationCost = 40;
        ReturnValue.Stats.Damage = 10;
        ReturnValue.Stats.HP = 300;
        ReturnValue.Stats.Range = 2;
        



        return (ReturnValue);
    }
    public static  RuleManager.UnitInfo GetInquisitor()
    {
        RuleManager.UnitInfo ReturnValue = new RuleManager.UnitInfo();
        ReturnValue.Stats = new RuleManager.UnitStats();
        ReturnValue.Stats.ActivationCost = 40;
        ReturnValue.Stats.Damage = 10;
        ReturnValue.Stats.HP = 300;
        ReturnValue.Stats.Range = 2;

        ReturnValue.Abilities.Add(
                new RuleManager.Ability_Activated(new RuleManager.TargetInfo_List(new RuleManager.TargetCondition_Type(RuleManager.TargetType.Unit), new RuleManager.TargetCondition_Range(10)),
                new RuleManager.Effect_List(new RuleManager.Effect_DamageArea(new RuleManager.TargetRetriever_Index(0), 3, 80),new RuleManager.Effect_DestroyUnits(new RuleManager.TargetRetriever_Index(0)))
            ));
        ReturnValue.Abilities[0].SetDescription("Target a knight within 10 tiles: deal 80 within 4 tiles and destroy the knight");
        ReturnValue.Abilities[0].SetName("Ultima sacrificium");
        ReturnValue.Abilities[0].SetFlavour("Some decisions are to important even for knight's to decide");

        ReturnValue.Abilities.Add(new RuleManager.Ability_Activated(new RuleManager.TargetInfo_List(
            new RuleManager.TargetCondition_And(new RuleManager.TargetCondition_Type(RuleManager.TargetType.Unit), new RuleManager.TargetCondition_Range(8)),
            new RuleManager.TargetCondition_And(new RuleManager.TargetCondition_Type(RuleManager.TargetType.Unit), new RuleManager.TargetCondition_Range(0,8))),
            new RuleManager.Effect_DealDamage(new RuleManager.TargetRetriever_Index(1),80))
            );
        ReturnValue.Abilities[1].SetName("Chain smite");
        ReturnValue.Abilities[1].SetDescription("Target a priest within 8 tiles and a enemy within 8 inches of that priest: Deal 50 damage to it, and 20 damage to the priest");
        ReturnValue.Abilities[1].SetFlavour("Priest conduit nibba");

        ReturnValue.Abilities.Add(new RuleManager.Ability_Activated(new RuleManager.TargetInfo_List(),
                new RuleManager.Effect_RegisterTrigger(true,false, 
                new RuleManager.TargetRetriever_Empty(),
                new RuleManager.TriggerCondition_Type(RuleManager.TriggerType.BattleroundBegin),
                new RuleManager.Effect_GainInitiative(30))
            )
            );
        ReturnValue.Abilities[2].SetName("Scour the battlefield");
        ReturnValue.Abilities[2].SetDescription("Gain 30 initiate at the begining of the next battle round");
        ReturnValue.Abilities[2].SetFlavour("Inquisitor big brain");
        return (ReturnValue);
    }
    public static  RuleManager.UnitInfo GetRelic()
    {
        return (null);
    }

}
