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

    static public RuleManager.UnitInfo GetHorseKnight()
    {
        RuleManager.UnitInfo ReturnValue = new RuleManager.UnitInfo();
        ReturnValue.Stats = new RuleManager.UnitStats();
        ReturnValue.Stats.ActivationCost = 40;
        ReturnValue.Stats.Damage = 100;
        ReturnValue.Stats.HP = 300;
        ReturnValue.Stats.Range = 2;
        ReturnValue.Stats.Movement = 12;
        ReturnValue.Stats.ObjectiveControll = 50;
        ReturnValue.Tags.Add("Knight");

        RuleManager.Ability_Activated ActivatedAbility = new RuleManager.Ability_Activated();
        RuleManager.Effect_DealDamage DamageEffect = new RuleManager.Effect_DealDamage();
        DamageEffect.Damage = 50;
        DamageEffect.Targets = new RuleManager.TargetRetriever_Index(0);

        ActivatedAbility.Speed = RuleManager.SpellSpeed.Speed2;
        ActivatedAbility.ActivatedEffect = DamageEffect;
        ActivatedAbility.ActivationTargets = new RuleManager.TargetInfo_List(
            new RuleManager.TargetCondition_And(
                new RuleManager.TargetCondition_Range(8),
                new RuleManager.TargetCondition_Type(RuleManager.TargetType.Unit),
                new RuleManager.TargetCondition_Enemy()));

        ActivatedAbility.SetDescription("Target a enemy unit within 8 tiles: Deal 50 damage to it");
        ActivatedAbility.SetFlavour("Templars be smighting");
        ActivatedAbility.SetName("Smite");

        ResourceManager.UnitUIInfo UIINfo = MainUI.g_ResourceManager.GetUnitResource("Knight").UIInfo;
        //ActivatedAbility.Animation = new RuleManager.Animation_List(new RuleManager.Animation_AbilityTarget(-1, UIINfo.OtherAnimations["LightningPose"].VisualInfo),
    //new RuleManager.Animation_AbilityTarget(0, UIINfo.OtherAnimations["LightningStrike"].VisualInfo));
        ReturnValue.Abilities.Add(ActivatedAbility);

        ReturnValue.TopLeftCorner = new RuleManager.Coordinate(0, 0);
        ReturnValue.UnitTileOffsets.Add(new RuleManager.Coordinate(0, 0));
        ReturnValue.UnitTileOffsets.Add(new RuleManager.Coordinate(1, 0));
        ReturnValue.UnitTileOffsets.Add(new RuleManager.Coordinate(2, 0));
        ReturnValue.UnitTileOffsets.Add(new RuleManager.Coordinate(0, 1));
        ReturnValue.UnitTileOffsets.Add(new RuleManager.Coordinate(1, 1));
        ReturnValue.UnitTileOffsets.Add(new RuleManager.Coordinate(2, 1));
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
        ReturnValue.Stats.Movement = 8; 
        ReturnValue.Stats.ObjectiveControll = 50;
        ReturnValue.Tags.Add("Knight");

        RuleManager.Ability_Activated ActivatedAbility = new RuleManager.Ability_Activated();
        RuleManager.Effect_DealDamage DamageEffect = new RuleManager.Effect_DealDamage();
        DamageEffect.Damage = 50;
        DamageEffect.Targets = new RuleManager.TargetRetriever_Index(0);

        ActivatedAbility.Speed = RuleManager.SpellSpeed.Speed2;
        ActivatedAbility.ActivatedEffect = DamageEffect;
        ActivatedAbility.ActivationTargets = new RuleManager.TargetInfo_List(
            new RuleManager.TargetCondition_And(
                new RuleManager.TargetCondition_Range(8),
                new RuleManager.TargetCondition_Type(RuleManager.TargetType.Unit),
                new RuleManager.TargetCondition_Enemy()));

        ActivatedAbility.SetDescription("Target a enemy unit within 8 tiles: Deal 50 damage to it");
        ActivatedAbility.SetFlavour("Templars be smighting");
        ActivatedAbility.SetName("Smite");
        //MainUI.g_ResourceManager.GetUnitResource("Knight")
        ResourceManager.UnitUIInfo UIINfo = MainUI.g_ResourceManager.GetUnitResource("Knight").UIInfo;
        //ActivatedAbility.Animation = new RuleManager.Animation_List(new RuleManager.Animation_AbilityTarget(-1, UIINfo.OtherAnimations["LightningPose"].VisualInfo),
         //   new RuleManager.Animation_AbilityTarget(0,UIINfo.OtherAnimations["LightningStrike"].VisualInfo));

        ReturnValue.Abilities.Add(ActivatedAbility);

        ReturnValue.TopLeftCorner = new RuleManager.Coordinate(0, 0);
        return (ReturnValue);
    }
    static public RuleManager.UnitInfo GetHorse()
    {
        RuleManager.UnitInfo ReturnValue = new RuleManager.UnitInfo();
        ReturnValue.Stats = new RuleManager.UnitStats();
        ReturnValue.Stats.ActivationCost = 40;
        ReturnValue.Stats.Damage = 100;
        ReturnValue.Stats.HP = 300;
        ReturnValue.Stats.Range = 2;
        ReturnValue.Stats.Movement = 12;
        ReturnValue.Stats.ObjectiveControll = 50;
        ReturnValue.Tags.Add("Knight");

        RuleManager.Ability_Activated ActivatedAbility = new RuleManager.Ability_Activated();
        RuleManager.Effect_DealDamage DamageEffect = new RuleManager.Effect_DealDamage();
        DamageEffect.Damage = 50;
        DamageEffect.Targets = new RuleManager.TargetRetriever_Index(0);

        ActivatedAbility.Speed = RuleManager.SpellSpeed.Speed2;
        ActivatedAbility.ActivatedEffect = DamageEffect;
        ActivatedAbility.ActivationTargets = new RuleManager.TargetInfo_List(
            new RuleManager.TargetCondition_And(
                new RuleManager.TargetCondition_Range(8),
                new RuleManager.TargetCondition_Type(RuleManager.TargetType.Unit),
                new RuleManager.TargetCondition_Enemy()));

        ActivatedAbility.SetDescription("Target a enemy unit within 8 tiles: Deal 50 damage to it");
        ActivatedAbility.SetFlavour("Templars be smighting");
        ActivatedAbility.SetName("Smite");

        ReturnValue.Abilities.Add(ActivatedAbility);

        return (ReturnValue);
    }
    public static RuleManager.UnitInfo GetPriest()
    {
        RuleManager.UnitInfo ReturnValue = new RuleManager.UnitInfo();
        ReturnValue.Stats = new RuleManager.UnitStats();
        ReturnValue.Stats.ActivationCost = 40;
        ReturnValue.Stats.Damage = 10;
        ReturnValue.Stats.HP = 100;
        ReturnValue.Stats.Range = 4;
        ReturnValue.Stats.Movement = 7;
        ReturnValue.Stats.ObjectiveControll = 20;
        ReturnValue.Tags.Add("Priest");

        ReturnValue.Abilities.Add(
                new RuleManager.Ability_Activated(RuleManager.SpellSpeed.Speed2,
                        new RuleManager.TargetInfo_List(
                        new RuleManager.TargetCondition_And(
                            new RuleManager.TargetCondition_Enemy(),
                            new RuleManager.TargetCondition_Type(RuleManager.TargetType.Unit),
                            new RuleManager.TargetCondition_Range(12))
                        ),
                        new RuleManager.Effect_RegisterContinousAbility(new RuleManager.TargetRetriever_Index(0),new RuleManager.TargetCondition_True(),new RuleManager.Effect_IncreaseMovement(-5),0,1)
                    ).SetName("Blinding light").SetDescription("Target an enemy unit within 12 tiles: decrease it's movement by 5 for the battle round").SetFlavour("bright light difficult see")
            );
        ReturnValue.Abilities.Add(
        new RuleManager.Ability_Activated(RuleManager.SpellSpeed.Speed2,
                new RuleManager.TargetInfo_List(
                new RuleManager.TargetCondition_And(
                    new RuleManager.TargetCondition_Enemy(), 
                    new RuleManager.TargetCondition_Type(RuleManager.TargetType.Unit), 
                    new RuleManager.TargetCondition_Range(12))
                ),
                new RuleManager.Effect_RegisterContinousAbility(
                    new RuleManager.TargetRetriever_Index(0), new RuleManager.TargetCondition_True(), new RuleManager.Effect_SilenceUnit(), 6)
            ).SetName("Silence the heretics").SetDescription("Target an enemy unit within 12 tiles: silence it for 6 passes").SetFlavour("Heretical opinions are cringe yo")
        );

        return (ReturnValue);
    }
    public static  RuleManager.UnitInfo GetInquisitor()
    {
        RuleManager.UnitInfo ReturnValue = new RuleManager.UnitInfo();
        ReturnValue.Stats = new RuleManager.UnitStats();
        ReturnValue.Stats.ActivationCost = 40;
        ReturnValue.Stats.Damage = 10;
        ReturnValue.Stats.HP = 200;
        ReturnValue.Stats.Range = 2;
        ReturnValue.Stats.Movement = 7;
        ReturnValue.Stats.ObjectiveControll = 10;
        ReturnValue.Tags.Add("Inquisitor");

        ReturnValue.Abilities.Add(
                new RuleManager.Ability_Activated(RuleManager.SpellSpeed.Speed2,
                    new RuleManager.TargetInfo_List(
                        new RuleManager.TargetCondition_And(
                            new RuleManager.TargetCondition_UnitTag("Knight"),
                            new RuleManager.TargetCondition_Type(RuleManager.TargetType.Unit), 
                            new RuleManager.TargetCondition_Range(10),
                            new RuleManager.TargetCondition_Friendly())
                        ),
                new RuleManager.Effect_List(new RuleManager.Effect_DamageArea(new RuleManager.TargetRetriever_Index(0), 3, 80),new RuleManager.Effect_DestroyUnits(new RuleManager.TargetRetriever_Index(0)))
            ));
        ReturnValue.Abilities[0].SetDescription("Target a knight within 10 tiles: deal 80 within 4 tiles and destroy the knight");
        ReturnValue.Abilities[0].SetName("Ultima sacrificium");
        ReturnValue.Abilities[0].SetFlavour("Some decisions are to important even for knight's to decide");

        ReturnValue.Abilities.Add(new RuleManager.Ability_Activated(RuleManager.SpellSpeed.Speed2, new RuleManager.TargetInfo_List(
            new RuleManager.TargetCondition_And(
                new RuleManager.TargetCondition_UnitTag("Priest"),
                new RuleManager.TargetCondition_Type(RuleManager.TargetType.Unit), 
                new RuleManager.TargetCondition_Range(8),
                new RuleManager.TargetCondition_Friendly()),
            new RuleManager.TargetCondition_And(new RuleManager.TargetCondition_Type(RuleManager.TargetType.Unit), new RuleManager.TargetCondition_Range(0,8),new RuleManager.TargetCondition_Enemy())),
            new RuleManager.Effect_DealDamage(new RuleManager.TargetRetriever_Index(1),80))
            );
        ReturnValue.Abilities[1].SetName("Chain smite");
        ReturnValue.Abilities[1].SetDescription("Target a priest within 8 tiles and a enemy within 8 tiles of that priest: Deal 80 damage to it");
        ReturnValue.Abilities[1].SetFlavour("Priest conduit nibba");

        ReturnValue.Abilities.Add(new RuleManager.Ability_Activated(RuleManager.SpellSpeed.Speed1,
                new RuleManager.TargetInfo_List(),
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
        RuleManager.UnitInfo ReturnValue = new RuleManager.UnitInfo();
        ReturnValue.Stats = new RuleManager.UnitStats();
        ReturnValue.Stats.ActivationCost = 20;
        ReturnValue.Stats.Damage = 0;
        ReturnValue.Stats.HP = 100;
        ReturnValue.Stats.Range = 0;
        ReturnValue.Stats.Movement = 7;
        ReturnValue.Stats.ObjectiveControll = 20;

        ReturnValue.Abilities.Add(
                new RuleManager.Ability_Activated(RuleManager.SpellSpeed.Speed1,
                        new RuleManager.TargetInfo_List(
                        new RuleManager.TargetCondition_And(
                            new RuleManager.TargetCondition_Or(new RuleManager.TargetCondition_UnitTag("Inquisitor"), new RuleManager.TargetCondition_UnitTag("Priest")),
                            new RuleManager.TargetCondition_Friendly(), 
                            new RuleManager.TargetCondition_Type(RuleManager.TargetType.Unit), 
                            new RuleManager.TargetCondition_Range(10))
                        ),
                        new RuleManager.Effect_List(
                            new RuleManager.Effect_DestroyUnits(new RuleManager.TargetRetriever_Index(-1)), new RuleManager.Effect_RefreshUnit(new RuleManager.TargetRetriever_Index(0)))
                    ).SetName("Final rites").SetDescription("Target a friendly inquisitor or priest within 10 tiles: Destroy this unit and refresh that unit").
                    SetDescription("Rites be final")
            );
        return (ReturnValue);
    }

}
