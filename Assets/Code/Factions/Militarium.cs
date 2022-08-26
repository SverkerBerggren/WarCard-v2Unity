using System.Collections;
using System.Collections.Generic;

public class Militarium 
{

    public static RuleManager.UnitInfo GetFootSoldier()
    {
        RuleManager.UnitInfo NewUnitInfo = new RuleManager.UnitInfo();
        NewUnitInfo.Stats.Movement = 6;
        NewUnitInfo.Stats.Damage = 35;
        NewUnitInfo.Stats.Range = 6;
        NewUnitInfo.Stats.HP = 100;
        NewUnitInfo.Stats.ActivationCost = 15;
        NewUnitInfo.Stats.ObjectiveControll = 10;

        NewUnitInfo.Tags.Add("Infantry");
        return (NewUnitInfo);
    }
    public static RuleManager.UnitInfo GetHeavyWeapons()
    {
        RuleManager.UnitInfo NewUnitInfo = new RuleManager.UnitInfo();
        NewUnitInfo.Stats.Movement = 5;
        NewUnitInfo.Stats.Damage = 65;
        NewUnitInfo.Stats.Range = 15;
        NewUnitInfo.Stats.HP = 200;
        NewUnitInfo.Stats.ObjectiveControll = 5;
        NewUnitInfo.Stats.ActivationCost = 15;
        
        
        return (NewUnitInfo);
    }

    public static RuleManager.UnitInfo GetMeleeSoldier()
    {
        RuleManager.UnitInfo NewUnitInfo = new RuleManager.UnitInfo();
        NewUnitInfo.Stats.Movement = 7;
        NewUnitInfo.Stats.Damage = 50;
        NewUnitInfo.Stats.Range = 1;
        NewUnitInfo.Stats.HP = 100;
        NewUnitInfo.Stats.ActivationCost = 15;
        NewUnitInfo.Stats.ObjectiveControll = 10;
        NewUnitInfo.Tags.Add("Infantry");
        return (NewUnitInfo);
    }

    public static RuleManager.UnitInfo GetArtillery()
    {
        RuleManager.UnitInfo NewUnitInfo = new RuleManager.UnitInfo();
        NewUnitInfo.Stats.Movement = 4;
        NewUnitInfo.Stats.Damage = 25;
        NewUnitInfo.Stats.Range = 30;
        NewUnitInfo.Stats.HP = 200;
        NewUnitInfo.Stats.ActivationCost = 30;
        NewUnitInfo.Stats.ObjectiveControll = 5;

        RuleManager.Ability_Activated BarrageAbility = new RuleManager.Ability_Activated(RuleManager.SpellSpeed.Speed1,
                new RuleManager.TargetInfo_List(new RuleManager.TargetCondition_And(new RuleManager.TargetCondition_Type(RuleManager.TargetType.Tile),new RuleManager.TargetCondition_Range(30))),
                new RuleManager.Effect_RegisterTrigger(true,false,new RuleManager.TargetRetriever_Index(0),new RuleManager.TriggerCondition_Type(RuleManager.TriggerType.BattleroundBegin),
                        new RuleManager.Effect_DamageArea(new RuleManager.TargetRetriever_Index(0),2,50)
                    )
            );

        BarrageAbility.SetName("Creeping barrage");
        BarrageAbility.SetFlavour("Shooty shooty");
        BarrageAbility.SetDescription("Target a tile within 30 tiles: Deal 25 damage to all units within 2 tiles of that tile at the beginning of the next turn. Activate only as the first ability each turn");
        NewUnitInfo.Abilities.Add(BarrageAbility);
        return (NewUnitInfo);
    }

    public static RuleManager.UnitInfo GetOfficer()
    {
        RuleManager.UnitInfo ReturnValue = new RuleManager.UnitInfo();
        ReturnValue.Stats.HP = 80;
        ReturnValue.Stats.Damage = 20;
        ReturnValue.Stats.Movement = 6;
        ReturnValue.Stats.Range = 8;
        ReturnValue.Stats.ActivationCost = 15;
        ReturnValue.Stats.ObjectiveControll = 5;


        RuleManager.Ability_Activated MoveAbility = new RuleManager.Ability_Activated();
        MoveAbility.Speed = RuleManager.SpellSpeed.Speed2;
        MoveAbility.ActivationTargets = 
            new RuleManager.TargetInfo_List(
            new RuleManager.TargetCondition_And(new RuleManager.TargetCondition_Friendly(),new RuleManager.TargetCondition_Type(RuleManager.TargetType.Unit),new RuleManager.TargetCondition_Range(6)), 
            new RuleManager.TargetCondition_And(new RuleManager.TargetCondition_Type(RuleManager.TargetType.Tile),new RuleManager.TargetCondition_Range(0,3)));
        MoveAbility.ActivatedEffect = new RuleManager.Effect_MoveUnit(new RuleManager.TargetRetriever_Index(0), new RuleManager.TargetRetriever_Index(1));

        RuleManager.Ability_Activated IncreaseDamage = new RuleManager.Ability_Activated();
        RuleManager.Effect_RegisterContinousAbility EffectResult = new RuleManager.Effect_RegisterContinousAbility();
        EffectResult.ContinousEffect = new RuleManager.Effect_IncreaseDamage(50);
        EffectResult.OptionalAffectedTarget = new RuleManager.TargetRetriever_Index(0);

        IncreaseDamage.ActivatedEffect = EffectResult;
        IncreaseDamage.Speed = RuleManager.SpellSpeed.Speed1;
        IncreaseDamage.ActivationTargets = 
            new RuleManager.TargetInfo_List( 
                new RuleManager.TargetCondition_And(
                    new RuleManager.TargetCondition_UnitTag("Infantry"),
                    new RuleManager.TargetCondition_Friendly(),
                    new RuleManager.TargetCondition_Range(6),
                    new RuleManager.TargetCondition_Type(RuleManager.TargetType.Unit)));

        RuleManager.Ability_Activated IncreaseMovement = new RuleManager.Ability_Activated(RuleManager.SpellSpeed.Speed1,
                new RuleManager.TargetInfo_List(
                    new RuleManager.TargetCondition_And(
                        new RuleManager.TargetCondition_UnitTag("Infantry"),
                        new RuleManager.TargetCondition_Friendly(),
                        new RuleManager.TargetCondition_Type(RuleManager.TargetType.Unit), 
                        new RuleManager.TargetCondition_Range(6))),
                new RuleManager.Effect_RegisterContinousAbility(new RuleManager.TargetRetriever_Index(0), new RuleManager.TargetCondition_True(),new RuleManager.Effect_IncreaseMovement(5))
            );

        IncreaseMovement.SetName("Move Move Move!");
        IncreaseMovement.SetDescription("Target a friendly infantry within 6 tiles: Make it Move Move Move");
        IncreaseMovement.SetFlavour("In the grimn dark future, moving is important");

        IncreaseDamage.SetName("First rank second rank");
        IncreaseDamage.SetDescription("Target a friendly unit within 6 tiles: Increase it's damage with 50 until end of turn");
        IncreaseDamage.SetFlavour("People need to be told of to shoot efficiently");

        MoveAbility.SetName("Move");
        MoveAbility.SetDescription("Target a friendly unit within 6 tiles: Move it 3 tiles. Spell speed 2");
        MoveAbility.SetFlavour("Move but instant");

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
