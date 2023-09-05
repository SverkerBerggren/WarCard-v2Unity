using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace RuleManager
{   
    public enum TargetType
    {
        Player,
        Unit,
        Tile,
        Null
    }
    public enum AbilityType
    {
        Continous,
        Triggered,
        Activated,
        Null
    }
    public class Effect
    {
        int PlayerIndex = 0;
        string m_Text = "test";
        public AnimationSpecification Animation = null;
        public virtual string GetText()
        {
            return m_Text;
        }
        public void SetText(string NewText)
        {
            m_Text = NewText;
        }
    }

    public class UnitIdentifier
    {
        public int ID = 0;
        public UnitIdentifier(){}
        public UnitIdentifier(int ID)
        {
            this.ID = ID;   
        }
    }

    public interface AnimationPlayer
    {
        void PlayAnimation(Coordinate AnimationCoordinate,object Animation);
        void PlayAnimation(int UnitID,object Animation);
    }

    public class AnimationSpecification
    {

    };
    public class Animation_List : AnimationSpecification
    {
        public List<AnimationSpecification> AnimationsToPlay;

        public Animation_List()
        {

        }
        public Animation_List(params AnimationSpecification[] AnimationSpecifications)
        {
            AnimationsToPlay = new List<AnimationSpecification>(AnimationSpecifications);
        }
    }
    public class Animation_AbilityTarget : AnimationSpecification
    {
        //-1 means the source, depends on the type of target
        public int TargetIndex = -1;
        public object AnimationToPlay = null;
        public Animation_AbilityTarget() { }
        public Animation_AbilityTarget(int NewTargetIndex, object NewAnimation)
        {
            TargetIndex = NewTargetIndex;
            AnimationToPlay = NewAnimation;
        }
    }
    //public class Effect_DestroyTargets : Effect
    //{
    //    TargetRetriever Targets;
    //}
    
    public class Effect_List : Effect
    {
        public List<Effect> EffectsToExecute = new List<Effect>();

        public Effect_List()
        {

        }
        public Effect_List(params Effect[] Effects)
        {
            EffectsToExecute = new List<Effect>(Effects);
        }
    }
    public class Effect_UnitScript : Effect
    {
        public UnitScript.Expression Expr;
    }
    public class Effect_ContinousUnitScript : Effect
    {
        public string UnitName;
        public UnitScript.Expression Expr;
    }
    public class Effect_GainInitiative : Effect
    {
        public int InitiativeGain = 0;
        public Effect_GainInitiative()
        {

        }
        public Effect_GainInitiative(int NewInitiative) 
        {
            InitiativeGain = NewInitiative;
        }
    }

    public class Effect_DealDamage : Effect
    {
        public TargetRetriever Targets;
        public int Damage = 0;

        public Effect_DealDamage()
        {
            
        }
        public Effect_DealDamage(TargetRetriever TargetsToUse,int NewDamage)
        {
            Targets = TargetsToUse;
            Damage = NewDamage;
        }
    }
    public class Effect_MoveUnit : Effect
    {
        public TargetRetriever UnitToMove;
        public TargetRetriever TargetPosition;
        
        public Effect_MoveUnit()
        {

        }
        public Effect_MoveUnit(TargetRetriever Unit,TargetRetriever Position)
        {
            UnitToMove = Unit;
            TargetPosition = Position;
        }
    }

    public class Effect_RegisterContinousAbility : Effect
    {
        public TargetRetriever OptionalAffectedTarget;
        public int PassDuration = 1;
        public int TurnDuration = 1;
        public TargetCondition ContinousCondition = new TargetCondition_True();
        public Effect ContinousEffect;

        public Effect_RegisterContinousAbility()
        {

        }
        public Effect_RegisterContinousAbility(TargetRetriever Retriever,TargetCondition Condition,Effect NewEffect,int NewPassDuration = 1,int NewTurnDuration = 1)
        {
            OptionalAffectedTarget = Retriever;
            ContinousCondition = Condition;
            ContinousEffect = NewEffect;
            PassDuration = NewPassDuration;
            TurnDuration = NewTurnDuration;
        }
    }
    public class Effect_RegisterTrigger : Effect
    {
        public bool IsOneshot = false;
        public bool IsEndOfTurn = false;
        public TargetRetriever OptionalAffectedTarget;
        public TriggerCondition Condition;
        public Effect TriggerEffect;

        public Effect_RegisterTrigger()
        {

        }
        public Effect_RegisterTrigger(bool OneShot,bool EndOfTurn,TargetRetriever Targets,TriggerCondition NewCondition,Effect EffectToResolve)
        {
            IsOneshot = OneShot;
            IsEndOfTurn = EndOfTurn;
            OptionalAffectedTarget = Targets;
            Condition = NewCondition;
            TriggerEffect = EffectToResolve;
        }
    }
    public class Effect_DestroyUnits : Effect
    {
        public TargetRetriever TargetsToDestroy;
        public Effect_DestroyUnits(TargetRetriever Targets)
        {
            TargetsToDestroy = Targets;
        }
    }
    public class Effect_DamageArea : Effect
    {
        public TargetRetriever Origin;
        public int Range = 0;
        public int Damage = 0;

        public Effect_DamageArea()
        {

        }
        public Effect_DamageArea(TargetRetriever NewOrigin,int RangeToUse,int DamageToDeal)
        {
            Origin = NewOrigin;
            Range = RangeToUse;
            Damage = DamageToDeal;
        }
    }
    public class Effect_AttackUnit : Effect
    {
        public TargetRetriever Attacker;
        public TargetRetriever Defender;
    }

    //Borde vara modifier istället
    public class Effect_HeavyAttack : Effect
    {

    }
    public class Effect_IncreaseDamage : Effect
    {
        public int DamageIncrease = 0;
        public Effect_IncreaseDamage(int NewDamage)
        {
            DamageIncrease = NewDamage;
        }
    }
    public class Effect_IncreaseMovement : Effect
    {
        public int MovementIncrease = 0;
        public Effect_IncreaseMovement(int Increase)
        {
            MovementIncrease = Increase;
        }
        public Effect_IncreaseMovement()
        {

        }
    }
    public class Effect_SilenceUnit : Effect
    {
        public Effect_SilenceUnit()
        {

        }
    }

    public class Effect_RefreshUnit : Effect
    {
        public TargetRetriever TargetsToRefresh;
        public Effect_RefreshUnit(TargetRetriever RetrieverToUse)
        {
            TargetsToRefresh = RetrieverToUse;
        }
    }

    public enum RetrieverType
    {
        Index,
        Choose,
        Literal,
        Empty,
        Null
    }
    public class TargetRetriever
    {

        public RetrieverType Type = RetrieverType.Null;
        protected TargetRetriever(RetrieverType NewType)
        {
            Type = NewType;
        }
        public TargetRetriever()
        {

        }
    }
    public class TargetRetriever_Empty : TargetRetriever
    {
        public TargetRetriever_Empty () : base(RetrieverType.Empty)
        {
            
        }

    }

    public class TargetRetriever_Index : TargetRetriever
    {
        public int Index = 0;
        public TargetRetriever_Index() : base(RetrieverType.Index)
        {

        }
        public TargetRetriever_Index(int NewIndex) : base(RetrieverType.Index)
        {
            Index = NewIndex;
        }
    }
    public class TargetRetriever_Choose : TargetRetriever
    {
        TargetInfo TargetsToChoose;
        TargetRetriever_Choose() : base(RetrieverType.Choose)
        {

        }
    }
    public class TargetRetriever_Literal : TargetRetriever
    {
        public List<Target> Targets = new List<Target>();
        public TargetRetriever_Literal() : base(RetrieverType.Literal)
        {

        }
        public TargetRetriever_Literal(IEnumerable<Target> NewTargets) : base(RetrieverType.Literal)
        {
            foreach(Target NewTarget in NewTargets)
            {
                Targets.Add(NewTarget);
            }
        }
        public TargetRetriever_Literal(Target NewTarget) : base(RetrieverType.Literal)
        {
            Targets.Add(NewTarget);
        }
    }

    public class Ability
    {
        public readonly AbilityType Type = AbilityType.Null;

        protected Ability(AbilityType NewType)
        {
            Type = NewType;
        }
        public Ability()
        {

        }

        string m_Name = "TestNamn";
        string m_Flavor = "TestFlavour";
        string m_Description = "TestDescription";

        public virtual string GetName()
        {
            return m_Name;
        }
        public virtual string GetFlavourText()
        {
            return m_Flavor;
        }
        public virtual string GetDescription()
        {
            return m_Description;
        }
        public Ability SetName(string NewName)
        {
            m_Name = NewName;
            return (this);
        }
        public Ability SetFlavour(string NewFlavour)
        {
            m_Flavor = NewFlavour;
            return (this);
        }
        public Ability SetDescription(string NewDescription)
        {
            m_Description = NewDescription;
            return (this);
        }
    }

    public enum SpellSpeed
    {
        Speed0,
        Speed1,
        Speed2,
    }


    public class ActivationCondition
    {

    }

    public class ActivationCondition_True : ActivationCondition
    {

    }
    public class ActivationCondition_FirstTurn : ActivationCondition
    {

    }

    public class Ability_Activated : Ability
    {

        public SpellSpeed Speed = SpellSpeed.Speed1;
        public ActivationCondition Conditions = new ActivationCondition_True();
        public TargetInfo ActivationTargets;
        public Effect ActivatedEffect;

        public AnimationSpecification Animation = null;

        public Ability_Activated() : base(AbilityType.Activated)
        {
            
        }
        public Ability_Activated(SpellSpeed NewSpeed,TargetInfo Targets, Effect EffectToUse) : base(AbilityType.Activated)
        {
            Speed = NewSpeed;
            ActivationTargets = Targets;
            ActivatedEffect = EffectToUse;
        }
    }
    public class Ability_Triggered : Ability
    {
        public TriggerCondition Condition;
        public Effect TriggeredEffect;
        public Ability_Triggered() : base(AbilityType.Triggered)
        {

        }
    }
    public class Ability_Continous : Ability
    {
        public TargetCondition AffectedEntities = new TargetCondition_Self();
        public Effect EffectToApply;
        public Ability_Continous(TargetCondition NewAffectedEntities,Effect NewEffectToApply)
        {
            AffectedEntities = NewAffectedEntities;
            EffectToApply = NewEffectToApply;
        }
        public Ability_Continous() : base(AbilityType.Continous)
        {

        }
    }


    public class Target : MBJson.JSONTypeConverter,MBJson.JSONDeserializeable
    {
        public Type GetType(int IntegerToConvert)
        {
            Type ReturnValue = null;
            TargetType SerializedType = (TargetType)IntegerToConvert;
            if (SerializedType == TargetType.Player)
            {
                ReturnValue = typeof(Target_Player);
            }
            else if(SerializedType == TargetType.Tile)
            {
                ReturnValue = typeof(Target_Tile);
            }
            else if (SerializedType == TargetType.Unit)
            {
                ReturnValue = typeof(Target_Unit);
            }
            else
            {
                throw new Exception("Invalid target type to serialize: " + Type);
            }
            return (ReturnValue);
        }

        public object Deserialize(MBJson.JSONObject ObjectToDeserialize)
        {
            object ReturnValue = null;
            MBJson.DynamicJSONDeserializer Deserializer = new MBJson.DynamicJSONDeserializer(this);
            ReturnValue = Deserializer.Deserialize(ObjectToDeserialize);
            return (ReturnValue);
        }

        public TargetType Type = TargetType.Null;

        public virtual bool Equals(Target OtherTarget)
        {
            return (false);
        }
        public Target()
        {

        }
        public Target(TargetType TypeToUse)
        {
            Type = TypeToUse;
        }

    }
    public class Target_Player : Target
    {
        public int PlayerIndex = 0;
        public Target_Player() : base(TargetType.Player)
        {

        }
        public Target_Player(int NewPlayerIndex) : base(TargetType.Player)
        {
            PlayerIndex = NewPlayerIndex;
        }
        public override bool Equals(Target obj)
        {
            return (obj.Type == Type && ((Target_Player)obj).PlayerIndex == PlayerIndex);
        }
    }
    public class Target_Unit : Target
    {
        public int UnitID = 0;
        public Target_Unit() : base(TargetType.Unit)
        {

        }
        public Target_Unit(int NewID) : base(TargetType.Unit)
        {
            UnitID = NewID;
        }
        public override bool Equals(Target obj)
        {
            return (obj.Type == Type && ((Target_Unit)obj).UnitID == UnitID);
        }
    }
    public class Target_Tile : Target
    {
        public Coordinate TargetCoordinate;
        public Target_Tile() : base(TargetType.Tile)
        {

        }

        public Target_Tile(Coordinate cord) : base(TargetType.Tile)
        {
            TargetCoordinate = cord;
        }
        public override bool Equals(Target obj)
        {
            return (obj.Type == Type && ((Target_Tile)obj).TargetCoordinate.Equals(TargetCoordinate));
        }
    }

    public class TargetInfo
    {
        public virtual bool IsEmpty()
        {
            return (false);
        }
    }
    public class TargetInfo_List  : TargetInfo
    {
        public List<TargetCondition> Targets = new List<TargetCondition>();

        public TargetInfo_List()
        {

        }
        public TargetInfo_List(params TargetCondition[] Arguments)
        {
            Targets = new List<TargetCondition>(Arguments);
        }
        public override bool IsEmpty() 
        {
            return (Targets.Count == 0);
        }
    }
    public class UnitScriptTarget 
    {
        public TargetType Type;
        public string Name;
        public UnitScript.Expression Condition;
    }
    public class TargetInfo_UnitScript : TargetInfo
    {
        public List<UnitScriptTarget> Targets;
    }
    public class TargetCondition
    {
    }
    //duplicated for each target
    public class TargetCondition_UnitScript : TargetCondition
    {
        public List<UnitScriptTarget> Targets;
    }
    public class TargetCondition_Type : TargetCondition
    {
        public TargetType ValidType = TargetType.Null;
        public TargetCondition_Type(TargetType TypeToUse)
        {
            ValidType = TypeToUse;
        }
    }
    public class TargetCondition_Enemy : TargetCondition
    {
        
    }
    public class TargetCondition_Friendly : TargetCondition
    {

    }
    public class TargetCondition_Range : TargetCondition
    {
        //hacky af, -1 means the source, otherwise range from appropriate target is used
        public int TargetIndex = -1;
        public int Range = 0;
        public TargetCondition_Range()
        {

        }
        public TargetCondition_Range(int RangeToUse)
        {
            Range = RangeToUse;
        }
        public TargetCondition_Range(int Index,int NewRange)
        {
            TargetIndex = Index;
            Range = NewRange;
        }
    }
    public class TargetCondition_Target : TargetCondition
    {
        public Target NeededTarget;
        public TargetCondition_Target(Target TargetToStore)
        {
            NeededTarget = TargetToStore;
        }
    }
    public class TargetCondition_Self : TargetCondition
    {

    }
    public class TargetCondition_And : TargetCondition
    {
        public List<TargetCondition> Conditions = new List<TargetCondition>();

        public TargetCondition_And(params TargetCondition[] NewConditions)
        {
            Conditions = new List<TargetCondition>(NewConditions);
        }
    }
    public class TargetCondition_Or : TargetCondition
    {
        public List<TargetCondition> Conditions = new List<TargetCondition>();

        public TargetCondition_Or(params TargetCondition[] NewConditions)
        {
            Conditions = new List<TargetCondition>(NewConditions);
        }
    }
    public class TargetCondition_UnitTag : TargetCondition
    {
        public string TagToContain = "";
        public TargetCondition_UnitTag()
        {

        }
        public TargetCondition_UnitTag(string TagToUse)
        {
            TagToContain = TagToUse;
        }
    }
    public class TargetCondition_True : TargetCondition
    {

    }
    public class EffectSource
    {
        public int PlayerIndex = -1;
        protected EffectSource()
        {

        }
    }
    public class EffectSource_Empty : EffectSource
    {

    }
    public class EffectSource_Unit : EffectSource
    {
        public int UnitID = 0;
        public int EffectIndex = -1;
        public EffectSource_Unit(int NewPlayerIndex,int NewUnitID,int NewEffectIndex)
        {
            PlayerIndex = NewPlayerIndex;
            UnitID = NewUnitID;
            EffectIndex = NewEffectIndex;
        }
    }
    public class EffectSource_Player : EffectSource
    {
    }

    public enum TriggerType
    {
        BattleroundBegin,
        BattleroundEnd,
        Null
    }

    public class TriggerEvent
    {
        public readonly TriggerType Type = TriggerType.Null;
        public TriggerEvent()
        {

        }
        protected TriggerEvent(TriggerType TypeToUse)
        {
            Type = TypeToUse;
        }
    }
    public class TriggerEvent_RoundBegin : TriggerEvent
    {
        public TriggerEvent_RoundBegin() : base(TriggerType.BattleroundBegin)
        {

        }
    }

    public class TriggerCondition
    {

    }
    public class TriggerCondition_And : TriggerCondition
    {
        public List<TriggerCondition> ConditionsToSatisfy = new List<TriggerCondition>();
        public TriggerCondition_And()
        {

        }
        public TriggerCondition_And(params TriggerCondition[] Conditions)
        {
            ConditionsToSatisfy = new List<TriggerCondition>(Conditions);
        }
    }
    public class TriggerCondition_Type : TriggerCondition
    {
        public TriggerType ApplicableType = TriggerType.Null;
        public TriggerCondition_Type()
        {
            
        }
        public TriggerCondition_Type(TriggerType NewType)
        {
            ApplicableType = NewType;
        }
    }


    [Serializable]
    public class Coordinate : IEquatable<Coordinate>, IComparable<Coordinate>
    {

        public override int GetHashCode()
        {
            return (HashCode.Combine(X, Y));
        }
        public int CompareTo(Coordinate rhs)
        {
            int ReturnvValue = 1;
            if(X < rhs.X)
            {
                ReturnvValue = -1;
            }
            else if(X == rhs.X)
            {
                if(Y < rhs.Y)
                {
                    ReturnvValue = -1;
                }
                else if(Y == rhs.Y)
                {
                    ReturnvValue = 0; 
                }
            }
            return (ReturnvValue);
        }

        public bool Equals(Coordinate rhs)
        {
            return (rhs.X == X && rhs.Y == Y);
        }
        public int X = 0;
        public int Y = 0;
        public Coordinate()
        {
            X = 0;
            Y = 0;
        }
        public Coordinate(int NewX, int NewY)
        {
            X = NewX;
            Y = NewY;
        }
        public Coordinate(Coordinate Copy)
        {
            X = Copy.X;
            Y = Copy.Y;
        }
        public static Coordinate operator+(Coordinate LHS,Coordinate RHS)
        {
            Coordinate ReturnValue = new Coordinate();
            ReturnValue.X = LHS.X + RHS.X;
            ReturnValue.Y = LHS.Y + RHS.Y;
            return (ReturnValue);
        }
        public static Coordinate operator -(Coordinate LHS, Coordinate RHS)
        {
            Coordinate ReturnValue = new Coordinate();
            ReturnValue.X = LHS.X - RHS.X;
            ReturnValue.Y = LHS.Y - RHS.Y;
            return (ReturnValue);
        }
        public static int Distance(Coordinate LeftCoordinate, Coordinate RightCoordinate)
        {
            return (Math.Abs(LeftCoordinate.X - RightCoordinate.X) + Math.Abs(LeftCoordinate.Y - RightCoordinate.Y));
        }

        public override string ToString()
        {
            return ("X: " + X + " Y: " + Y);
        }
    }
    public enum ActionType
    {
        Null,
        Move,
        Attack,
        UnitEffect,
        Stratagem,
        Pass,
        Rotate
    }
    [Serializable]
    public class Action  : MBJson.JSONDeserializeable,MBJson.JSONTypeConverter
    {
        public Action()
        {

        }
        protected Action(ActionType TypeToUse)
        {
            Type = TypeToUse;
        }
        public ActionType Type = ActionType.Null;
        public int PlayerIndex = -1;

        public Type GetType(int IntegerToConvert)
        {
            Type ReturnValue = null;
            ActionType SerializedType = (ActionType) IntegerToConvert;
            if (SerializedType == ActionType.Move)
            {
                ReturnValue = typeof(MoveAction);
            }
            else if (SerializedType == ActionType.Attack)
            {
                ReturnValue = typeof(AttackAction);
            }
            else if (SerializedType == ActionType.Pass)
            {
                ReturnValue = typeof(PassAction);
            }
            else if(SerializedType == ActionType.UnitEffect)
            {
                ReturnValue = typeof(EffectAction);
            }
            else
            {
                throw new Exception("Invalid Action type in when deserizalizing action");
            }
            return (ReturnValue);
        }
        public object Deserialize(MBJson.JSONObject ObjectToDeserialize)
        {
            return (new MBJson.DynamicJSONDeserializer(this).Deserialize(ObjectToDeserialize));
        }
    }
    [Serializable]
    public class MoveAction : Action
    {
        public MoveAction() : base(ActionType.Move) {}
        public int UnitID = 0;
        //always assumes that the position moved is the top left corner
        public Coordinate NewPosition;
    }
    [Serializable]
    public class RotateAction : Action
    {
        public RotateAction() : base(ActionType.Rotate) { }
        public int UnitID = 0;
        //always assumes that the position moved is the top left corner
        public Coordinate NewRotation = new Coordinate(1,0);
    }
    [Serializable]
    public class AttackAction : Action
    {
        public AttackAction() : base(ActionType.Attack) { }
        public AttackAction(int NewAttackID,int NewDefenderID) : base(ActionType.Attack) 
        {
            AttackerID = NewAttackID;
            DefenderID = NewDefenderID;
        }
        public int AttackerID = 0;
        public int DefenderID = 0;
    }
    [Serializable]
    public class PassAction : Action
    {
       public PassAction(int NewPlayerIndex)
       {
            PlayerIndex = NewPlayerIndex;
       }
       public PassAction() : base(ActionType.Pass) { }
    }
    public class EffectAction : Action
    {
        public List<Target> Targets = new List<Target>();
        public int UnitID = 0;
        public int EffectIndex = -1;

        public EffectAction() : base(ActionType.UnitEffect)
        {

        }
        public EffectAction(IEnumerable<Target> NewTargets,int NewUnitID,int NewEffectIndex) : base(ActionType.UnitEffect)
        {
            Targets = new List<Target>(NewTargets);
            UnitID = NewUnitID;
            EffectIndex = NewEffectIndex;
        }
        public EffectAction(Target NewTarget, int NewUnitID, int NewEffectIndex) : base(ActionType.UnitEffect)
        {
            Targets.Add(NewTarget);
            UnitID = NewUnitID;
            EffectIndex = NewEffectIndex;
        }
    }

    public class UnitStats
    {
        public int HP = 0;
        public int Movement = 0;
        public int Range = 0;
        public int Damage = 0;
        public int ActivationCost = 0;
        public int ObjectiveControll = 0;
        public UnitStats(UnitStats StatToCopy)
        {
            HP = StatToCopy.HP;
            Movement = StatToCopy.Movement;
            Range = StatToCopy.Range;
            Damage = StatToCopy.Damage;
            ActivationCost = StatToCopy.ActivationCost;
            ObjectiveControll = StatToCopy.ObjectiveControll;
        }
        public UnitStats()
        {

        }
    }
    public enum UnitFlags : uint
    {
        Silenced = 1,
        CantAttack = 1<<1,
        HasMoved = 1<<2,
        HasAttacked = 1<<3,
        IsActivated = 1<<4,
    }
    [Serializable]
    public class UnitInfo
    {
        //public EffectType Temp = EffectType.Activated;

        //Static stuff
        public int UnitID = 0;
        public int PlayerIndex = 0;
        public int OpaqueInteger = -1;
        public List<Ability> Abilities = new List<Ability>();
        public UnitStats Stats = new UnitStats();
        public HashSet<string> Tags = new HashSet<string>();

        public UnitScript.EvaluationEnvironment Envir = new UnitScript.EvaluationEnvironment();

        //Dynamic stuff
        public List<bool> AbilityActivated = new List<bool>();
        public Coordinate Direction = new Coordinate(1, 0);
        //used so that positions can be calculated with a simple diff
        public Coordinate TopLeftCorner = new Coordinate(0, 0);
        public List<Coordinate> UnitTileOffsets = new List<Coordinate>(new Coordinate[] { new Coordinate(0,0)});
        //public bool IsActivated = false;
        //public bool HasMoved = false;
        //public bool HasAttacked = false;
        public UnitFlags Flags = 0;
        //
        public UnitInfo()
        {

        }
        int p_GetRotCount(Coordinate Rotation)
        {
            int ReturnValue = 0;
            if(Rotation.X == 1)
            {
                ReturnValue = 0;
            }
            else if(Rotation.X == -1)
            {
                ReturnValue = 2;
            }
            else if(Rotation.Y == 1)
            {
                ReturnValue = 1;
            }
            else if(Rotation.Y == -1)
            {
                ReturnValue = 3;
            }
            return (ReturnValue);
        }
        int[,] p_GetRotationMatrix(int RotCount)
        {
            int[,] RotMatrix = new int[2, 2] { { 0, 1 }, { -1, 0 } };
            int[,] ReturnValue = new int[2, 2] { { 0, 1 }, { -1, 0 } };
            if(RotCount == 0)
            {
                return (new int[,] { { 1, 0 }, { 0, 1 } });
            }
            for(int i = 0; i < RotCount-1;i++)
            {
                int[,] NewReturnValue =(int[,]) ReturnValue.Clone();
                NewReturnValue[0,0] = ReturnValue[0,0]*RotMatrix[0,0]+ReturnValue[0,1]*RotMatrix[1,0];
                NewReturnValue[0,1] = ReturnValue[0,0]*RotMatrix[0,1]+ReturnValue[0,1]*RotMatrix[1,1];
                NewReturnValue[1,0] = ReturnValue[1,0]*RotMatrix[0,0]+ReturnValue[1,1]*RotMatrix[1,0];
                NewReturnValue[1,1] = ReturnValue[1,0]*RotMatrix[0,1]+ReturnValue[1,1]*RotMatrix[1,1];
                ReturnValue = NewReturnValue;
            }
            return (ReturnValue);
        }
        public List<Coordinate> GetRotatedOffsets(Coordinate NewDirection)
        {
            List<Coordinate> ReturnValue = new List<Coordinate>();
            if((!(NewDirection.X == 1 || NewDirection.X == -1 || NewDirection.X == 0)) || !(NewDirection.Y == 1 || NewDirection.Y == -1 || NewDirection.Y == 0))
            {
                throw new Exception("Invalid rotation, X,Y must be 1 or -1: "+NewDirection.X + " "+NewDirection.Y);
            }
            int CurrentRotCount= p_GetRotCount(Direction);
            int TargetCount = p_GetRotCount(NewDirection);
            int RotCount = TargetCount - CurrentRotCount;
            if(RotCount < 0)
            {
                RotCount += 4;
            }
            //ghetto ass manual matrix rotation
            int[,] RotMatrix = p_GetRotationMatrix(RotCount);
            foreach (Coordinate Offset in UnitTileOffsets)
            {
                Coordinate NewCoord = new Coordinate();
                NewCoord.X = RotMatrix[0, 0] * Offset.X + RotMatrix[0, 1] * Offset.Y;
                NewCoord.Y = RotMatrix[1, 0] * Offset.X + RotMatrix[1, 1] * Offset.Y;
                ReturnValue.Add(NewCoord);
            }
            return (ReturnValue);
        }
        public UnitInfo(UnitInfo InfoToCopy)
        {
            UnitID = InfoToCopy.UnitID;
            PlayerIndex = InfoToCopy.PlayerIndex;
            OpaqueInteger = InfoToCopy.OpaqueInteger;
            Abilities = new List<Ability>(InfoToCopy.Abilities);
            Stats = new UnitStats(InfoToCopy.Stats);
            Tags = new HashSet<string>(InfoToCopy.Tags);
            //IsActivated =   InfoToCopy.IsActivated;
            //HasMoved    =   InfoToCopy.HasMoved   ;
            //HasAttacked =   InfoToCopy.HasAttacked;
            //Position = new Coordinate(InfoToCopy.Position);
            UnitTileOffsets = new List<Coordinate>();
            Direction = new Coordinate(InfoToCopy.Direction);
            TopLeftCorner = new Coordinate(InfoToCopy.TopLeftCorner);
            Envir = InfoToCopy.Envir;
            foreach(Coordinate CoordToCopy in InfoToCopy.UnitTileOffsets)
            {
                UnitTileOffsets.Add(new Coordinate(CoordToCopy));
            }
            AbilityActivated = new List<bool>(InfoToCopy.AbilityActivated);
            Flags = InfoToCopy.Flags;

        }
    }
    public enum TileFlags
    {
        Null = 0,
        Impassable = 1<<1,
        Obscuring = 1<<2,
        LightCover = 1<<3,
        Breachable = 1<<4
    }
    public class TileInfo
    {
        public int StandingUnitID = 0;
        public bool HasObjective = false;
        public TileFlags Flags = TileFlags.Null;
        public TileInfo()
        {

        }
        public TileInfo(TileInfo TileToCopy)
        {
            StandingUnitID = TileToCopy.StandingUnitID;
        }
    }

    public interface RuleEventHandler
    {
        void OnStackPush(StackEntity NewEntity);
        void OnStackPop(StackEntity PoppedEntity);
        void OnUnitMove(int UnitID, Coordinate PreviousPosition, Coordinate NewPosition);
        void OnUnitRotation(int UnitID, Coordinate NewDirection);
        void OnUnitAttack(int AttackerID, int DefenderID);
        void OnUnitDestroyed(int UnitID);
        void OnTurnChange(int CurrentPlayerTurnIndex,int CurrentTurnCount);
        void OnRoundChange(int CurrentPlayerTurnIndex,int CurrentRoundCount);
        void OnUnitCreate(UnitInfo NewUnit);

        void OnInitiativeChange(int newInitiativen, int whichPlayer ); 

        void OnPlayerPassPriority(int currentPlayerString);

        void OnPlayerWin(int WinningPlayerIndex);
        void OnScoreChange(int PlayerIndex, int NewScore);


    }
    public class StackEntity
    {
        public List<Target> Targets;
        public EffectSource Source;
        public Effect EffectToResolve;
    }
    public class RuleManager
    {
        //retunrs UnitInfo with invalid UnitID on error, that is to say UnitID = 0
        private Dictionary<int,UnitInfo> m_UnitInfos;
        private List<List<TileInfo>> m_Tiles;

        private readonly int m_PlayerCount = 2;
        private int m_CurrentTurn = 0;
        private int m_CurrentPlayerTurn = 0;
        private int m_CurrentPlayerPriority = 0;

        private int m_CurrentRoundCount = 1;
        private int m_BattleRoundCount = 0;
        
        private bool m_EndOfTurnPass = false;

        class RegisteredContinousEffect
        {
            //public bool IsEndOfTurn = false;//hacky, but encapsulates important and very common functionality
            public int PassDuration = 0;
            public int TurnDuration = 0;
            public EffectSource AbilitySource;
            public TargetCondition AffectedEntities;
            public Effect EffectToApply;
        }
        class RegisteredTrigger
        {
            public bool IsOneShot = false;//hacky, but encapsulates important and very common functionality
            public bool IsEndOfTurn = false;//hacky, but encapsulates important and very common functionality
            public TriggerCondition TriggerCondition;
            public EffectSource TriggerSource;
            public TargetRetriever AffectedEntities;
            public Effect TriggerEffect;
        }
        int m_CurrentTriggerID = 1000; 
        int p_RegisterTrigger(RegisteredTrigger Trigger)
        {
            int ReturnValue = m_CurrentTriggerID;
            m_RegisteredTriggeredAbilities.Add(m_CurrentTriggerID, Trigger);
            m_CurrentTriggerID += 1;
            return (ReturnValue);
        }
        int m_CurrentContinousID = 1000000;
        int p_RegisterContinousEffect(RegisteredContinousEffect ContinousEffect)
        {
            int ReturnValue = m_CurrentContinousID;
            m_RegisteredContinousAbilities.Add(ReturnValue, ContinousEffect);
            m_CurrentContinousID += 1;
            return (m_CurrentContinousID);
        }
        //effects to remove on unit killed, continous id to remove
        Dictionary<int, int> m_UnitRegisteredContinousAbilityMap = new Dictionary<int, int>();

        const int m_PlayerMaxInitiative = 150;
        const int m_PlayerTurnInitiativeGain = 100;
        const int m_PlayerInitiativeRetain = 40;
        const int m_ObjectiveScoreGain = 15;

        const int m_PlayerWinThreshold = 100;

        bool m_GameFinished = false;


        bool m_ActionIsPlayed = false;

        List<int> m_PlayerPoints = new List<int>();
        List<int> m_PlayerIntitiative = new List<int>();
        Dictionary<int, RegisteredContinousEffect> m_RegisteredContinousAbilities = new Dictionary<int, RegisteredContinousEffect>();
        Dictionary<int, RegisteredTrigger> m_RegisteredTriggeredAbilities = new Dictionary<int, RegisteredTrigger>();

        Stack<StackEntity> m_TheStack = new Stack<StackEntity>();
        IEnumerator m_CurrentResolution = null;

        List<bool> m_EmptyPassed = new List<bool>();

        private int m_CurrentID = 0;
        RuleEventHandler m_EventHandler;
        AnimationPlayer m_AnimationPlayer;
        UnitScript.UnitConverter m_ScriptHandler;

        List<Target> m_ChoosenTargets = null;

        private int m_FirstRound = 2;
        

        bool p_TriggerIsTriggered(TriggerEvent Event,TriggerCondition ConditionToVerify)
        {
            bool ReturnValue = true;
            if(ConditionToVerify is TriggerCondition_And)
            {
                TriggerCondition_And AndCondition = (TriggerCondition_And)ConditionToVerify;
                foreach(TriggerCondition SubCondition in AndCondition.ConditionsToSatisfy)
                {
                    if(!p_TriggerIsTriggered(Event,SubCondition))
                    {
                        ReturnValue = false;
                        break;
                    }
                }
            }
            else if(ConditionToVerify is TriggerCondition_Type)
            {
                TriggerCondition_Type TypeCondition = (TriggerCondition_Type)ConditionToVerify;
                if(TypeCondition.ApplicableType != Event.Type)
                {
                    ReturnValue = false;
                }
            }
            else
            {
                throw new Exception("Invalid trigger condition type");
            }
            return (ReturnValue);
        }
        IEnumerator p_AddTriggers(TriggerEvent NewEvent)
        {
            List<int> TriggersToRemove = new List<int>();
            foreach(KeyValuePair<int,RegisteredTrigger> Trigger in m_RegisteredTriggeredAbilities)
            {
                if(p_TriggerIsTriggered(NewEvent,Trigger.Value.TriggerCondition))
                {
                    IEnumerator TriggerTargetsRetriever = p_RetrieveTargets(new List<Target>(),Trigger.Value.TriggerSource, Trigger.Value.AffectedEntities);
                    while(TriggerTargetsRetriever.MoveNext())
                    {
                        if(TriggerTargetsRetriever.Current == null)
                        {
                            yield return null;
                        }
                        else
                        {
                            break;
                        }
                    }
                    List<Target> Targets = (List<Target>)TriggerTargetsRetriever.Current;
                    StackEntity NewEntity = new StackEntity();
                    NewEntity.EffectToResolve = Trigger.Value.TriggerEffect;
                    NewEntity.Source = Trigger.Value.TriggerSource;
                    NewEntity.Targets = Targets;

                    m_TheStack.Push(NewEntity);
                    if(m_EventHandler != null)
                    {
                        m_EventHandler.OnStackPush(NewEntity);
                    }
                    if(Trigger.Value.IsOneShot)
                    {
                        TriggersToRemove.Add(Trigger.Key);
                    }
                }
            }
            foreach(int TriggerKey in TriggersToRemove)
            {
                m_RegisteredTriggeredAbilities.Remove(TriggerKey);
            }
            yield break;
        }   


        public void SetEventHandler(RuleEventHandler NewHandler)
        {
            m_EventHandler = NewHandler;
        }
        public void SetAnimationPlayer(AnimationPlayer Player)
        {
            m_AnimationPlayer = Player;
        }

        public int getPlayerPriority()
        {
            return m_CurrentPlayerPriority; 
        }
        public int GetPlayerTurn()
        {
            return m_CurrentPlayerTurn;
        }
        public RuleManager(uint Width,uint Height)
        {
            m_Tiles = new List<List<TileInfo>>((int)Height);
            for(int i = 0; i < Height;i++)
            {
                List<TileInfo> NewList = new List<TileInfo>();
                for(int j = 0; j < Width;j++)
                {
                    NewList.Add(new TileInfo());
                }
                m_Tiles.Add(NewList);
            }
            m_FirstRound = m_PlayerCount;
            m_UnitInfos = new Dictionary<int, UnitInfo>();
            m_PlayerIntitiative = new List<int>(m_PlayerCount);
            for(int i = 0; i < m_PlayerCount;i++)
            {
                m_EmptyPassed.Add(false);
                m_PlayerIntitiative.Add(0);
                m_PlayerPoints.Add(0);
            }
            m_PlayerIntitiative[0] = m_PlayerTurnInitiativeGain;
        }
        public int RegisterUnit(UnitInfo NewUnit,int PlayerIndex)
        {
            NewUnit = new UnitInfo(NewUnit);
            int NewID = m_CurrentID + 1;
            m_CurrentID++;
            NewUnit.UnitID = NewID;
            NewUnit.PlayerIndex = PlayerIndex;
            m_UnitInfos[NewID] = NewUnit;

            for(int i = 0; i < NewUnit.Abilities.Count;i++)
            {
                NewUnit.AbilityActivated.Add(false);
                //if ability är continous
                if(NewUnit.Abilities[i] is Ability_Continous)
                {
                    Ability_Continous AbilityToRegister = (Ability_Continous)NewUnit.Abilities[i];
                    RegisteredContinousEffect EffectToRegister = new RegisteredContinousEffect();
                    EffectToRegister.AffectedEntities = AbilityToRegister.AffectedEntities;
                    EffectToRegister.AbilitySource = new EffectSource_Unit(PlayerIndex, NewID, i);
                    EffectToRegister.EffectToApply = AbilityToRegister.EffectToApply;
                    m_UnitRegisteredContinousAbilityMap.Add(NewID,p_RegisterContinousEffect(EffectToRegister));
                }
            }
            foreach(Coordinate Coord in NewUnit.UnitTileOffsets)
            {
                Coordinate CurrentTile = NewUnit.TopLeftCorner + Coord; 
                m_Tiles[CurrentTile.Y][CurrentTile.X].StandingUnitID = NewID;
            }

            return (NewID);
        }

        void p_RefreshUnit(UnitInfo UnitToRefresh)
        {
            UnitToRefresh.Flags = 0;
            //UnitToRefresh.Flags &= ~(UnitFlags.HasAttacked);
            //UnitToRefresh.Flags &= ~(UnitFlags.HasMoved);
            //UnitToRefresh.HasAttacked = false;
            //UnitToRefresh.HasMoved = false;
            //UnitToRefresh.IsActivated = false;
            for(int i = 0; i < UnitToRefresh.AbilityActivated.Count;i++)
            {
                UnitToRefresh.AbilityActivated[i] = false;
            }
        }

        //NOTE this function doesnt provide any checks, just move it as specified

        bool p_MoveUnit(int UnitID, Coordinate TilePosition)
        {
            bool ReturnValue = true;
            UnitInfo AssociatedInfo = m_UnitInfos[UnitID];
            //Coordinate Diff = TilePosition - AssociatedInfo.TopLeftCorner;
            Coordinate PrevPos = AssociatedInfo.TopLeftCorner;
            for(int i = 0; i < AssociatedInfo.UnitTileOffsets.Count;i++)
            {
                Coordinate OldCoord = AssociatedInfo.UnitTileOffsets[i] + PrevPos;
                m_Tiles[OldCoord.Y][OldCoord.X].StandingUnitID = 0;
                Coordinate NewCoord = AssociatedInfo.UnitTileOffsets[i] + TilePosition;
                //AssociatedInfo.Position[i] += Diff;
                m_Tiles[NewCoord.Y][NewCoord.X].StandingUnitID = UnitID;
            }
            AssociatedInfo.TopLeftCorner = TilePosition;
            if (m_EventHandler != null)
            {
                m_EventHandler.OnUnitMove(UnitID, PrevPos, TilePosition);
            }
            return (ReturnValue);
        }
        void p_RotateUnit(int UnitID,Coordinate NewRotation)
        {
            UnitInfo AssociatedInfo = m_UnitInfos[UnitID];
            List<Coordinate> NewCoords = AssociatedInfo.GetRotatedOffsets(NewRotation);
            foreach(Coordinate Offset in AssociatedInfo.UnitTileOffsets)
            {
                Coordinate CurrentCoord = Offset + AssociatedInfo.TopLeftCorner;
                m_Tiles[CurrentCoord.Y][CurrentCoord.X].StandingUnitID = 0;
            }
            foreach(Coordinate Offset in NewCoords)
            {
                Coordinate CurrentCoord = Offset + AssociatedInfo.TopLeftCorner;
                m_Tiles[CurrentCoord.Y][CurrentCoord.X].StandingUnitID = UnitID;
            }
            AssociatedInfo.Direction = NewRotation;
            AssociatedInfo.UnitTileOffsets = NewCoords;
            if(m_EventHandler != null)
            {
                m_EventHandler.OnUnitRotation(UnitID, NewRotation);
            }
        }
        IEnumerator p_RetrieveTargets(List<Target> Targets,EffectSource Source,TargetRetriever Retriever)
        {
            if(Retriever is TargetRetriever_Index)
            {
                TargetRetriever_Index IndexRetriever = (TargetRetriever_Index)Retriever;
                List<Target> NewTargets = new List<Target>();
                if(IndexRetriever.Index == -1)
                {
                    if(Source is EffectSource_Player)
                    {
                        NewTargets.Add(new Target_Player(((EffectSource_Player)Source).PlayerIndex));
                    }
                    else if(Source is EffectSource_Unit)
                    {
                        NewTargets.Add(new Target_Unit(((EffectSource_Unit)Source).UnitID));
                    }
                    else if(Source is EffectSource_Empty)
                    {
                    }
                    else
                    {
                        throw new Exception("Invalid effect source when retrieving index target");
                    }
                }
                else
                {
                    NewTargets.Add(Targets[IndexRetriever.Index]);
                }
                yield return (NewTargets);
            }
            else if(Retriever is TargetRetriever_Choose)
            {
                yield return null;
                if(m_ChoosenTargets == null)
                {
                    throw new Exception("Targets need to be choosen to continue resolution");
                }
                List<Target> Result = m_ChoosenTargets;
                m_ChoosenTargets = null;
                yield return (Result);
            }
            else if(Retriever is TargetRetriever_Literal)
            {
                TargetRetriever_Literal Literal = (TargetRetriever_Literal)Retriever;
                yield return Literal.Targets;
            }
            else if(Retriever is TargetRetriever_Empty)
            {
                yield return new List<Target>();
            }
            else
            {
                throw new Exception("Invalid target retriever type " + Retriever.GetType().Name);
            }
            yield break;
        }
        List<Coordinate> p_GetAbsolutePositions(Coordinate TopLeftCorner,List<Coordinate> CoordsToTransform)
        {
            List<Coordinate> ReturnValue = new List<Coordinate>();
            foreach(Coordinate Coord in CoordsToTransform)
            {
                ReturnValue.Add(Coord + TopLeftCorner);
            }
            return (ReturnValue);
        }

        //can assume that m_AnimationPlayer != null
        void p_PlayAnimations(List<Target> Targets, EffectSource Source, AnimationSpecification AnimationToPlay)
        {
            if(AnimationToPlay is Animation_List)
            {
                foreach(AnimationSpecification Animation in ((Animation_List) AnimationToPlay).AnimationsToPlay)
                {
                    p_PlayAnimations(Targets,Source, Animation);
                }
            }
            else if(AnimationToPlay is Animation_AbilityTarget)
            {
                Animation_AbilityTarget AbilityTarget = (Animation_AbilityTarget)AnimationToPlay;
                if(AbilityTarget.TargetIndex == -1)
                {
                    if(Source is EffectSource_Unit)
                    {
                        EffectSource_Unit UnitSource = (EffectSource_Unit)Source;
                        m_AnimationPlayer.PlayAnimation(UnitSource.UnitID, AbilityTarget.AnimationToPlay);
                    }
                }
                else
                {
                    Target TargetToAnimate = Targets[AbilityTarget.TargetIndex];
                    Coordinate CoordinateToPlay = new Coordinate(0,0);
                    if(TargetToAnimate is Target_Tile)
                    {
                        CoordinateToPlay = ((Target_Tile)TargetToAnimate).TargetCoordinate;
                    }
                    else if(TargetToAnimate is Target_Unit)
                    {
                        CoordinateToPlay = m_UnitInfos[((Target_Unit)TargetToAnimate).UnitID].TopLeftCorner;
                    }
                    m_AnimationPlayer.PlayAnimation(CoordinateToPlay, AbilityTarget.AnimationToPlay);
                }
            }
        }

        IEnumerator p_ResolveEffect(List<Target> Targets,EffectSource Source,Effect EffectToResolve)
        {
            if(m_AnimationPlayer != null && EffectToResolve.Animation != null)
            {
                p_PlayAnimations(Targets, Source, EffectToResolve.Animation);
            }
            if(EffectToResolve is Effect_UnitScript)
            {
                Effect_UnitScript UnitEffect = (Effect_UnitScript)EffectToResolve; 
                if( !(Source is EffectSource_Unit))
                {
                    throw new Exception("Effect_UnitScript assumes that the source is a Unit");
                }
                EffectSource_Unit UnitSource = (EffectSource_Unit)Source;
                UnitInfo AssociatedUnit = m_UnitInfos[UnitSource.UnitID];
                AssociatedUnit.Envir.AddVar("SOURCE",UnitSource);
                m_ScriptHandler.Eval(AssociatedUnit.Envir,UnitEffect.Expr);
            }
            else if (EffectToResolve is Effect_DealDamage)
            {
                Effect_DealDamage DamageEffect = (Effect_DealDamage)EffectToResolve;
                IEnumerator RetrievedTargets = p_RetrieveTargets(Targets,Source, DamageEffect.Targets);
                RetrievedTargets.MoveNext();
                while(RetrievedTargets.Current == null)
                {
                    yield return null;
                    RetrievedTargets.MoveNext();
                }
                foreach(Target CurrentTarget in (List<Target>) RetrievedTargets.Current)
                {
                    if(CurrentTarget.Type != TargetType.Unit)
                    {
                        throw new Exception("Can only damage units");
                    }
                    Target_Unit Unit = (Target_Unit)CurrentTarget;
                    if(m_UnitInfos.ContainsKey(Unit.UnitID) == false)
                    {
                        throw new Exception("Invalid UnitID for target");
                    }
                    UnitInfo UnitToDamage = m_UnitInfos[Unit.UnitID];
                    UnitToDamage.Stats.HP -= DamageEffect.Damage;
                }
            }
            else if(EffectToResolve is Effect_MoveUnit)
            {
                Effect_MoveUnit MoveEffect = (Effect_MoveUnit)EffectToResolve;
                IEnumerator RetrievedMoveTargets = p_RetrieveTargets(Targets,Source, MoveEffect.UnitToMove);
                RetrievedMoveTargets.MoveNext();
                while (RetrievedMoveTargets.Current == null)
                {
                    yield return null;
                    RetrievedMoveTargets.MoveNext();
                }
                List<Target> MoveTargets = (List<Target>)RetrievedMoveTargets.Current;
                if(MoveTargets.Count != 1)
                {
                    throw new Exception("Can only move 1 unit to a tile");
                }
                if(MoveTargets[0].Type != TargetType.Unit)
                {
                    throw new Exception("Can only move a unit");
                }
                Target_Unit UnitToMove = (Target_Unit)MoveTargets[0];


                IEnumerator RetrievedTileTargets = p_RetrieveTargets(Targets,Source, MoveEffect.TargetPosition);
                RetrievedTileTargets.MoveNext();
                while(RetrievedTileTargets.Current == null)
                {
                    yield return null;
                    RetrievedTileTargets.MoveNext();
                }
                List<Target> TileTargets = (List<Target>)RetrievedTileTargets.Current;
                if(TileTargets.Count != 1)
                {
                    throw new Exception("Cannot target multiple tiles for move");
                }
                if(TileTargets[0].Type != TargetType.Tile)
                {
                    throw new Exception("Can only move to a tile target");
                }
                Target_Tile TileToMoveTo = (Target_Tile)TileTargets[0];

                bool Result = p_MoveUnit(UnitToMove.UnitID, TileToMoveTo.TargetCoordinate);
            }
            else if(EffectToResolve is Effect_RefreshUnit)
            {
                Effect_RefreshUnit RefreshEffect = (Effect_RefreshUnit)EffectToResolve;
                IEnumerator RetrievedAffectedTargets = p_RetrieveTargets(Targets,Source, RefreshEffect.TargetsToRefresh);
                RetrievedAffectedTargets.MoveNext();
                while (RetrievedAffectedTargets.Current == null)
                {
                    yield return null;
                    RetrievedAffectedTargets.MoveNext();
                }
                List<Target> AffectedTargets = (List<Target>)RetrievedAffectedTargets.Current;
                foreach(Target CurrentTarget in AffectedTargets)
                {
                    if(CurrentTarget.Type != TargetType.Unit)
                    {
                        throw new Exception("Can only refresh units");
                    }
                    Target_Unit UnitTarget = (Target_Unit)CurrentTarget;
                    if (!m_UnitInfos.ContainsKey(UnitTarget.UnitID))
                    {
                        throw new Exception("Unit to refresh doesn't exist");
                    }
                    UnitInfo UnitToRefresh = m_UnitInfos[UnitTarget.UnitID];
                    p_RefreshUnit(UnitToRefresh);
                }
            }
            else if (EffectToResolve is Effect_RegisterTrigger)
            {
                Effect_RegisterTrigger TriggerEffect = (Effect_RegisterTrigger)EffectToResolve;
                IEnumerator RetrievedAffectedTargets = p_RetrieveTargets(Targets,Source,TriggerEffect.OptionalAffectedTarget);
                RetrievedAffectedTargets.MoveNext();
                while (RetrievedAffectedTargets.Current == null)
                {
                    yield return null;
                    RetrievedAffectedTargets.MoveNext();
                }
                List<Target> AffectedTargets = (List<Target>)RetrievedAffectedTargets.Current;
                TriggerCondition ConditionToAdd = TriggerEffect.Condition;
                RegisteredTrigger TriggerToRegister = new RegisteredTrigger();
                if(AffectedTargets.Count != 0)
                {
                    TriggerToRegister.AffectedEntities = new TargetRetriever_Literal(AffectedTargets);
                }
                else
                {
                    TriggerToRegister.AffectedEntities = new TargetRetriever_Empty();                    
                }
                TriggerToRegister.TriggerCondition = ConditionToAdd;
                TriggerToRegister.TriggerEffect = TriggerEffect.TriggerEffect;
                TriggerToRegister.TriggerSource = Source;
                TriggerToRegister.IsOneShot= TriggerEffect.IsOneshot;
                TriggerToRegister.IsEndOfTurn = TriggerEffect.IsEndOfTurn;
                p_RegisterTrigger(TriggerToRegister);
            }
            else if(EffectToResolve is Effect_RegisterContinousAbility)
            {
                Effect_RegisterContinousAbility ContinousEffect = (Effect_RegisterContinousAbility)EffectToResolve;
                IEnumerator RetrievedAffectedTargets = p_RetrieveTargets(Targets,Source, ContinousEffect.OptionalAffectedTarget);
                RetrievedAffectedTargets.MoveNext();
                while (RetrievedAffectedTargets.Current == null)
                {
                    yield return null;
                    RetrievedAffectedTargets.MoveNext();
                }
                List<Target> AffectedTargets = (List<Target>)RetrievedAffectedTargets.Current;
                TargetCondition ConditionToAdd = ContinousEffect.ContinousCondition;
                if (AffectedTargets.Count != 0)
                {
                    ConditionToAdd = new TargetCondition_And(ConditionToAdd, new TargetCondition_Target(AffectedTargets[0]));
                }
                RegisteredContinousEffect ContinousEffectToRegister = new RegisteredContinousEffect();
                ContinousEffectToRegister.AbilitySource = Source;
                ContinousEffectToRegister.AffectedEntities = ConditionToAdd;
                ContinousEffectToRegister.EffectToApply = ContinousEffect.ContinousEffect;
                ContinousEffectToRegister.PassDuration = ContinousEffect.PassDuration;
                ContinousEffectToRegister.TurnDuration = ContinousEffect.TurnDuration;
                p_RegisterContinousEffect(ContinousEffectToRegister);
            }
            else if(EffectToResolve is Effect_DamageArea)
            {
                Effect_DamageArea AreaDamageEffect = (Effect_DamageArea)EffectToResolve;
                IEnumerator RetrievedOrigin = p_RetrieveTargets(Targets,Source, AreaDamageEffect.Origin);
                RetrievedOrigin.MoveNext();
                while (RetrievedOrigin.Current == null)
                {
                    yield return null;
                    RetrievedOrigin.MoveNext();
                }
                List<Target> Origins = (List<Target>)RetrievedOrigin.Current;
                List<Coordinate> OriginTile = null;
                if(Origins.Count != 1)
                {
                    throw new Exception("Need exactly 1 origin to resolve DamageArea");
                }
                if(Origins[0].Type == TargetType.Tile)
                {
                    OriginTile = new List<Coordinate>();
                    OriginTile.Add(((Target_Tile)Origins[0]).TargetCoordinate);
                }
                if (Origins[0].Type == TargetType.Unit)
                {
                    OriginTile =p_GetAbsolutePositions(m_UnitInfos[((Target_Unit)Origins[0]).UnitID].TopLeftCorner, m_UnitInfos[((Target_Unit)Origins[0]).UnitID].UnitTileOffsets);
                }

                HashSet<int> AffectedUnits = new HashSet<int>();
                foreach(Coordinate CurrentTile in p_GetTiles(AreaDamageEffect.Range,OriginTile))
                {
                    if(m_Tiles[CurrentTile.Y][CurrentTile.X].StandingUnitID == 0 || AffectedUnits.Contains(m_Tiles[CurrentTile.Y][CurrentTile.X].StandingUnitID))
                    {
                        continue;
                    }
                    AffectedUnits.Add(m_Tiles[CurrentTile.Y][CurrentTile.X].StandingUnitID);
                    UnitInfo UnitToDamage = p_GetProcessedUnitInfo(m_Tiles[CurrentTile.Y][CurrentTile.X].StandingUnitID);
                    if(UnitToDamage.PlayerIndex == Source.PlayerIndex)
                    {
                        continue;
                    }
                    p_DealDamage(m_Tiles[CurrentTile.Y][CurrentTile.X].StandingUnitID, AreaDamageEffect.Damage);
                }
            }
            else if(EffectToResolve is Effect_List)
            {
                Effect_List ListToResolve = (Effect_List)EffectToResolve;
                foreach(Effect NewEffect in ListToResolve.EffectsToExecute)
                {
                    IEnumerator CurrentEffect = p_ResolveEffect(Targets, Source, NewEffect);
                    while (CurrentEffect.MoveNext())
                    {
                        yield return null;
                    }
                }
            }
            else if (EffectToResolve is Effect_GainInitiative)
            {
                Effect_GainInitiative InitiativeToResolve = (Effect_GainInitiative)EffectToResolve;
                m_PlayerIntitiative[Source.PlayerIndex] += InitiativeToResolve.InitiativeGain;
                if(m_EventHandler != null)
                {
                    m_EventHandler.OnInitiativeChange(m_PlayerIntitiative[Source.PlayerIndex], Source.PlayerIndex);
                }
            }
            else if(EffectToResolve is Effect_DestroyUnits)
            {
                Effect_DestroyUnits DestroyToResolve = (Effect_DestroyUnits)EffectToResolve;
                IEnumerator RetrievedOrigin = p_RetrieveTargets(Targets,Source, DestroyToResolve.TargetsToDestroy);
                RetrievedOrigin.MoveNext();
                while (RetrievedOrigin.Current == null)
                {
                    yield return null;
                    RetrievedOrigin.MoveNext();
                }
                List<Target> UnitsToDestroy = (List<Target>)RetrievedOrigin.Current;
                foreach(Target CurrentTarget in UnitsToDestroy)
                {
                    if(CurrentTarget.Type != TargetType.Unit)
                    {
                        throw new Exception("Can only destroy units");
                    }
                    Target_Unit UnitToDestroy = (Target_Unit)CurrentTarget;
                    p_DestroyUnit(UnitToDestroy.UnitID);
                }
            }
            yield break;
        }
        IEnumerator p_ResolveTopOfStack()
        {
            if(m_TheStack.Count == 0)
            {
                throw new Exception("Can't resolve empty stack");
            }
            StackEntity TopOfStack = m_TheStack.Peek();

            IEnumerator ResolveEnumerator = p_ResolveEffect(TopOfStack.Targets,TopOfStack.Source, TopOfStack.EffectToResolve);
            while(ResolveEnumerator.MoveNext() == true)
            {
                yield return null;
            }
            m_TheStack.Pop();
            if(m_EventHandler != null)
            {
                m_EventHandler.OnStackPop(TopOfStack);
            }
            p_CheckStateBasedAction();
            yield break;
        }
        public bool p_VerifyActivationConditions(ActivationCondition ConditionToVerify,out string OutError)
        {
            bool ReturnValue = true;
            string ErrorString = "";
            if(ConditionToVerify is ActivationCondition_True)
            {

            }
            else if(ConditionToVerify is ActivationCondition_FirstTurn)
            {
                ActivationCondition_FirstTurn FirstTurnCondition = (ActivationCondition_FirstTurn)ConditionToVerify;
                if(m_FirstRound <= 0)
                {
                    ErrorString = "Not the first turn of the battleround";
                    ReturnValue = false;
                }
            }
            else
            {
                throw new Exception("Invalid activation condition type: " + ConditionToVerify.GetType().Name);
            }
            OutError = ErrorString;
            return (ReturnValue);
        }
        private bool p_VerifyTarget(Target TargetToVerify,out string OutError)
        {
            bool ReturnValue = true;
            if(TargetToVerify is Target_Unit)
            {
                Target_Unit UnitTarget = (Target_Unit)TargetToVerify;
                if(!m_UnitInfos.ContainsKey(UnitTarget.UnitID))
                {
                    OutError = "Can't target unit that doesn't exist";
                    return (false);
                }
            }
            else if(TargetToVerify is Target_Tile)
            {
                Target_Tile TileTarget = (Target_Tile)TargetToVerify;
                if ((TileTarget.TargetCoordinate.Y >= m_Tiles.Count || TileTarget.TargetCoordinate.Y < 0) || (TileTarget.TargetCoordinate.X >= m_Tiles[0].Count || TileTarget.TargetCoordinate.X < 0))
                {
                    OutError = "Tile coordinate x=" + TileTarget.TargetCoordinate.X + " y=" + TileTarget.TargetCoordinate.Y + " is out of range for battlefield";
                    return (false);
                }
            }
            else if(TargetToVerify is Target_Player)
            {
                Target_Player PlayerTarget = (Target_Player)TargetToVerify;
                if(PlayerTarget.PlayerIndex < 0 ||  PlayerTarget.PlayerIndex >= m_PlayerCount)
                {
                    OutError = "Invalid player index";
                    return (false);
                }
            }
            OutError = "";
            return (ReturnValue);
        }

        public bool p_VerifyTarget(TargetCondition Condition,EffectSource Source,List<Target> CurrentTargets,Target TargetToVerify,out string OutError)
        {
            bool ReturnValue = true;
            string Error = "";
            
            if(!p_VerifyTarget(TargetToVerify,out OutError))
            {
                return (false);
            }
            if(Condition is TargetCondition_UnitScript)
            {
                //ASSUMPTION: always the top of the "call stack".
                TargetCondition_UnitScript UnitScriptTarget = (TargetCondition_UnitScript)Condition;
                if(!(Source is EffectSource_Unit))
                {
                    throw new Exception("Effect for TargetCondition_UnitScript must be a unit");
                }
                EffectSource_Unit Unit = (EffectSource_Unit)Source;
                UnitInfo SourceUnit = m_UnitInfos[Unit.UnitID];
                int CurrentIndex = 0;
                foreach(Target PrevTarget in CurrentTargets)
                {
                    if(PrevTarget is Target_Unit)
                    {
                        SourceUnit.Envir.AddVar(UnitScriptTarget.Targets[CurrentIndex].Name,new UnitIdentifier( ((Target_Unit)PrevTarget).UnitID));
                    }
                    else if(PrevTarget is Target_Tile)
                    {
                        SourceUnit.Envir.AddVar(UnitScriptTarget.Targets[CurrentIndex].Name,((Target_Tile)PrevTarget).TargetCoordinate);
                    }
                    CurrentIndex++;
                }
                if(UnitScriptTarget.Targets[CurrentIndex].Type != TargetToVerify.Type)
                {
                    Error = "Invalid type for target: type needs to be " + UnitScriptTarget.Targets[CurrentIndex].Type.ToString();
                    return false;
                }
                if(TargetToVerify is Target_Unit)
                {
                    SourceUnit.Envir.AddVar(UnitScriptTarget.Targets[CurrentIndex].Name,new UnitIdentifier( ((Target_Unit)TargetToVerify).UnitID));
                }
                else if(TargetToVerify is Target_Tile)
                {
                    SourceUnit.Envir.AddVar(UnitScriptTarget.Targets[CurrentIndex].Name,((Target_Tile)TargetToVerify).TargetCoordinate);
                }
                UnitScript.Expression ExprToEvaluate = UnitScriptTarget.Targets[CurrentIndex].Condition;
                object Result = m_ScriptHandler.Eval(SourceUnit.Envir,ExprToEvaluate);
                if( !(Result is bool) || !((bool)Result))
                {
                    ReturnValue = false;
                    if(SourceUnit.Envir.HasVar("Error"))
                    {
                        Error = (string)SourceUnit.Envir.GetVar("Error");
                    }
                }
            }
            else if(Condition is TargetCondition_Type)
            {
                TargetCondition_Type TypeCondition = (TargetCondition_Type)Condition;
                ReturnValue = TypeCondition.ValidType == TargetToVerify.Type;
                if(!ReturnValue)
                {
                    Error = "Invalid type for target: type needs to be " + TypeCondition.ValidType.ToString();
                }
            }
            else if(Condition is TargetCondition_Enemy)
            {
                ReturnValue = true;
                if(TargetToVerify is Target_Unit)
                {
                    ReturnValue = m_UnitInfos[((Target_Unit)TargetToVerify).UnitID].PlayerIndex != Source.PlayerIndex;
                    if(!ReturnValue)
                    {
                        Error = "Target needs to be an enemy unit";
                    }
                }
                else if(TargetToVerify is Target_Player)
                {
                    ReturnValue = ((Target_Player)TargetToVerify).PlayerIndex != Source.PlayerIndex;
                    if (!ReturnValue)
                    {
                        Error = "Target needs to be an enemy player";
                    }
                }
            }
            else if(Condition is TargetCondition_Friendly)
            {
                ReturnValue = true;
                if (TargetToVerify is Target_Unit)
                {
                    ReturnValue = m_UnitInfos[((Target_Unit)TargetToVerify).UnitID].PlayerIndex == Source.PlayerIndex;
                    if(!ReturnValue)
                    {
                        Error = "Target needs to be a friendly unit";
                    }
                }
                else if (TargetToVerify is Target_Player)
                {
                    ReturnValue = ((Target_Player)TargetToVerify).PlayerIndex == Source.PlayerIndex;
                    if (!ReturnValue)
                    {
                        Error = "Target needs to be a friendly player";
                    }
                }
            }
            else if(Condition is TargetCondition_Range)
            {
                TargetCondition_Range RangeCondition = (TargetCondition_Range)Condition;
                List<Coordinate> SourceCoordinate = null;
                if(RangeCondition.TargetIndex == -1)
                {
                    SourceCoordinate = p_GetAbsolutePositions(m_UnitInfos[((EffectSource_Unit)Source).UnitID].TopLeftCorner,m_UnitInfos[((EffectSource_Unit)Source).UnitID].UnitTileOffsets);
                }
                else
                {
                    Target PreviousTarget = CurrentTargets[RangeCondition.TargetIndex];
                    if(PreviousTarget is Target_Unit)
                    {
                        SourceCoordinate = p_GetAbsolutePositions(m_UnitInfos[((Target_Unit)PreviousTarget).UnitID].TopLeftCorner, m_UnitInfos[((Target_Unit)PreviousTarget).UnitID].UnitTileOffsets);
                    }
                    else if(PreviousTarget is Target_Tile)
                    {
                        SourceCoordinate = new List<Coordinate>();
                        SourceCoordinate.Add(((Target_Tile)PreviousTarget).TargetCoordinate);
                    }
                }
                List<Coordinate> TargetCoordinate = null;
                if(TargetToVerify is Target_Unit)
                {
                    TargetCoordinate = p_GetAbsolutePositions(m_UnitInfos[((Target_Unit)TargetToVerify).UnitID].TopLeftCorner, m_UnitInfos[((Target_Unit)TargetToVerify).UnitID].UnitTileOffsets);
                }
                else if(TargetToVerify is Target_Tile)
                {
                    TargetCoordinate = new List<Coordinate>();
                    TargetCoordinate .Add(((Target_Tile)TargetToVerify).TargetCoordinate);
                }
                else
                {
                    throw new Exception("Target condition only applies to targets of type Unit and Tile");
                }
                if(!(p_CalculateTileDistance(SourceCoordinate,TargetCoordinate) <= RangeCondition.Range))
                {
                    ReturnValue = false;
                }
                if(!ReturnValue)
                {
                    Error = "Target out of range";
                }

            }
            else if(Condition is TargetCondition_Target)
            {
                TargetCondition_Target TargetCondition = (TargetCondition_Target)Condition;
                ReturnValue = TargetCondition.NeededTarget.Equals(TargetToVerify);
                if(!ReturnValue)
                {
                    Error = "This should not be written";
                }
            }
            else if(Condition is TargetCondition_Self)
            {
                if(!(Source is EffectSource_Unit))
                {
                    Error = "self can only refer to a unit";
                    return (false);
                }
                EffectSource_Unit UnitSource = (EffectSource_Unit)Source;
                if(!(TargetToVerify is Target_Unit))
                {
                    Error = "Self can only refer to a unit";
                    return (false);
                }
                Target_Unit UnitTarget = (Target_Unit)TargetToVerify;
                if(!(UnitTarget.UnitID == UnitSource.UnitID))
                {
                    Error = "Target isn't itself";
                    return (false);
                }
                ReturnValue = true;
            }
            else if(Condition is TargetCondition_UnitTag)
            {
                TargetCondition_UnitTag TagCondition = (TargetCondition_UnitTag)Condition;
                if(TargetToVerify.Type != TargetType.Unit)
                {
                    ReturnValue = false;
                    Error = "Target needs to be a unit";
                }
                else
                {
                    int UnitToInspect = ((Target_Unit)TargetToVerify).UnitID;
                    if(!m_UnitInfos.ContainsKey(UnitToInspect))
                    {
                        Error = "Unit doesn't exist";
                        OutError = Error;
                        return (false);
                    }
                    ReturnValue = m_UnitInfos[UnitToInspect].Tags.Contains(TagCondition.TagToContain);
                    if(!ReturnValue)
                    {
                        Error = "Unit doesn't have the required tag: " + TagCondition.TagToContain;
                    }
                }
            }
            else if(Condition is TargetCondition_And)
            {
                TargetCondition_And AndCondition = (TargetCondition_And)Condition;
                foreach(TargetCondition AndClause in AndCondition.Conditions)
                {
                    if(!p_VerifyTarget(AndClause,Source,CurrentTargets,TargetToVerify,out Error))
                    {
                        ReturnValue = false;
                        break;
                    }
                }
            }
            else if (Condition is TargetCondition_Or)
            {
                TargetCondition_Or OrCondition = (TargetCondition_Or)Condition;
                ReturnValue = false;
                foreach (TargetCondition OrClause in OrCondition.Conditions)
                {
                    if (p_VerifyTarget(OrClause, Source, CurrentTargets, TargetToVerify,out Error))
                    {
                        ReturnValue = true;
                        break;
                    }
                }
            }
            else if(Condition is TargetCondition_True)
            {
                return (true);
            }
            else
            {
                throw new Exception("Invalid target condition type: "+Condition.GetType().Name);
            }
            OutError = Error;
            return (ReturnValue);
        }

        public bool p_VerifyTargets(TargetInfo Info,EffectSource Source,List<Target> TargetsToVerify,out string OutError)
        {
            bool ReturnValue = true;

            if(Info is TargetInfo_List)
            {
                TargetInfo_List ListToVerify = (TargetInfo_List)Info;
                if(ListToVerify.Targets.Count != ListToVerify.Targets.Count)
                {
                    ReturnValue = false;
                    OutError = "Invalid number of targets";
                    return (ReturnValue);
                }
                List<Target> CurrentTargets = new List<Target>();
                for(int i = 0; i < ListToVerify.Targets.Count;i++)
                {
                    if(!p_VerifyTarget(ListToVerify.Targets[i],Source,CurrentTargets, TargetsToVerify[i],out OutError))
                    {
                        return (false);
                    }
                    CurrentTargets.Add(TargetsToVerify[i]);
                }
            }
            OutError = "";
            return (ReturnValue);
        }

        //Assumes valid UnitID
        void p_DestroyUnit(int UnitID)
        {
            UnitInfo UnitToRemoveInfo = m_UnitInfos[UnitID];
            List<Coordinate> UnitToRemoveTiles = p_GetAbsolutePositions(UnitToRemoveInfo.TopLeftCorner,UnitToRemoveInfo.UnitTileOffsets);
            m_UnitInfos.Remove(UnitID);
            if(m_EventHandler != null)
            {
                m_EventHandler.OnUnitDestroyed(UnitID);
            }
            if (m_UnitRegisteredContinousAbilityMap.ContainsKey(UnitID))
            {
                m_RegisteredContinousAbilities.Remove(m_UnitRegisteredContinousAbilityMap[UnitID]);
                m_UnitRegisteredContinousAbilityMap.Remove(UnitID);
            }
            //UnitToRemoveTile.StandingUnitID = 0;
            foreach (Coordinate Position in UnitToRemoveTiles)
            {
                m_Tiles[Position.Y][Position.X].StandingUnitID = 0;
            }
        }
        void p_DealDamage(int UnitID, int Damage)
        {
            m_UnitInfos[UnitID].Stats.HP -= Damage;
        }
        List<int> p_GetObjectiveScores(Coordinate ObjectiveCoordinate)
        {
            List<int> ReturnValue = new List<int>();
            for (int i = 0; i < m_PlayerCount; i++)
            {
                ReturnValue.Add(0);
            }
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    Coordinate CoordinateDiff = new Coordinate(i, j);
                    if (!(CoordinateDiff.X == 0 && CoordinateDiff.Y == 0))
                    {
                        Coordinate CurrentCoord = ObjectiveCoordinate + CoordinateDiff;
                        if (m_Tiles[CurrentCoord.Y][CurrentCoord.X].StandingUnitID != 0)
                        {
                            UnitInfo CurrentInfo = p_GetProcessedUnitInfo(m_Tiles[CurrentCoord.Y][CurrentCoord.X].StandingUnitID);
                            ReturnValue[CurrentInfo.PlayerIndex] += CurrentInfo.Stats.ObjectiveControll;
                        }
                    }
                }
            }
            return (ReturnValue);
        }
        public List<int> GetObjectiveContestion(Coordinate ObjectiveCoordinate)
        {
            return (p_GetObjectiveScores(ObjectiveCoordinate));
        }
        int p_GetObjectiveControllIndex(Coordinate ObjectiveCoordinate)
        {
            List<int> ObjectiveScores = p_GetObjectiveScores(ObjectiveCoordinate);
            int ReturnValue = -1;
            int CurrentMaxObjective = 0;
            for(int i = 0; i < m_PlayerCount;i++)
            {
                if(ObjectiveScores[i] > CurrentMaxObjective)
                {
                    CurrentMaxObjective = ObjectiveScores[i];
                    ReturnValue = i;
                }
            }
            return (ReturnValue);
        }
        IEnumerator p_ChangeBattleround()
        {
            List<int> NewScore = new List<int>();
            m_FirstRound = m_PlayerCount;
            m_ActionIsPlayed = false;
            m_BattleRoundCount += 1;
            m_CurrentPlayerTurn = m_BattleRoundCount % m_PlayerCount;
            m_CurrentPlayerPriority = m_CurrentPlayerTurn;
            for (int i = 0; i < m_PlayerCount; i++)
            {
                m_PlayerIntitiative[i] = 0;
                m_EmptyPassed[i] = false;
                if(m_EventHandler != null)
                {
                    m_EventHandler.OnInitiativeChange(0, i);
                }
            }

            for(int i = 0; i < m_PlayerCount;i++)
            {
                NewScore.Add(0);
            }
            for(int i = 0; i < m_Tiles.Count;i++)
            {
                for(int j = 0; j < m_Tiles[0].Count;j++)
                {
                    if(m_Tiles[i][j].HasObjective)
                    {
                        int PlayerControllIndex = p_GetObjectiveControllIndex(new Coordinate(j, i));
                        if(PlayerControllIndex != -1)
                        {
                            NewScore[PlayerControllIndex] += m_ObjectiveScoreGain;
                        }
                    }
                }
            }
            for(int i = 0; i <  m_PlayerCount;i++)
            {
                m_PlayerPoints[i] += NewScore[i];
                if(m_EventHandler != null)
                {
                    m_EventHandler.OnScoreChange(i, m_PlayerPoints[i]);
                }
                if(m_PlayerPoints[i] >= m_PlayerWinThreshold)
                {
                    m_GameFinished = true;
                    if(m_EventHandler != null)
                    {
                        m_EventHandler.OnPlayerWin(i);
                    }
                    yield break;
                }
            }
            foreach(KeyValuePair<int,UnitInfo> Units in m_UnitInfos)
            {
                p_RefreshUnit(Units.Value);
            }



            List<int> ContinousEffectsToRemove = new List<int>();
            foreach (KeyValuePair<int, RegisteredContinousEffect> RegisteredEffect in m_RegisteredContinousAbilities)
            {
                if (RegisteredEffect.Value.TurnDuration <= 0)
                {
                    continue;
                }
                RegisteredEffect.Value.TurnDuration -= 1;
                if (RegisteredEffect.Value.TurnDuration == 0)
                {
                    ContinousEffectsToRemove.Add(RegisteredEffect.Key);
                }
            }
            foreach (int EffectToRemove in ContinousEffectsToRemove)
            {
                m_RegisteredContinousAbilities.Remove(EffectToRemove);
            }
            List<int> RegisteredEffectsToRemove = new List<int>();
            foreach (KeyValuePair<int, RegisteredTrigger> RegisteredEffect in m_RegisteredTriggeredAbilities)
            {

            }
            foreach (int EffectToRemove in RegisteredEffectsToRemove)
            {
                //m_RegisteredContinousAbilities.Remove(EffectToRemove);
            }




            if (m_EventHandler != null)
            {
                m_CurrentRoundCount++;
                m_EventHandler.OnRoundChange(0, m_CurrentRoundCount);
            }






            TriggerEvent_RoundBegin RoundBegin = new TriggerEvent_RoundBegin();
            IEnumerator TriggersEnumerator = p_AddTriggers(RoundBegin);
            while(TriggersEnumerator.MoveNext())
            {
                yield return null;
            }
        }
        IEnumerator p_ChangeTurn()
        {
            m_EmptyPassed[m_CurrentPlayerTurn] = !m_ActionIsPlayed;
            m_ActionIsPlayed = false;
            bool ChangeRound = true;
            foreach (bool PassedEmpty in m_EmptyPassed)
            {
                if (!PassedEmpty)
                {
                    ChangeRound = false;
                    break;
                }
            }
            m_CurrentPlayerTurn = (m_CurrentPlayerTurn + 1) % m_PlayerCount;
            m_CurrentTurn += 1;
            m_CurrentPlayerPriority = m_CurrentPlayerTurn;
            if (ChangeRound)
            {
                IEnumerator ChangeEnum = p_ChangeBattleround();
                while (ChangeEnum.MoveNext())
                {
                    yield return null;
                }
            }

            m_FirstRound -= 1;


            if (m_EventHandler != null)
            {
                m_EventHandler.OnTurnChange(m_CurrentPlayerTurn, m_CurrentTurn);
            }

            //for(int i = 0; i < m_PlayerCount;i++)
            //{
                m_PlayerIntitiative[m_CurrentPlayerTurn] = Math.Min(Math.Min(m_PlayerIntitiative[m_CurrentPlayerTurn], m_PlayerInitiativeRetain) + m_PlayerTurnInitiativeGain, m_PlayerMaxInitiative);
                if(m_EventHandler != null)
                {
                    m_EventHandler.OnInitiativeChange(m_PlayerIntitiative[m_CurrentPlayerTurn], m_CurrentPlayerTurn);
                }
            //}

            List<int> ContinousEffectsToRemove = new List<int>();
            foreach(KeyValuePair<int,RegisteredContinousEffect> RegisteredEffect in m_RegisteredContinousAbilities)
            {
                if(RegisteredEffect.Value.PassDuration <= 0)
                {
                    continue;
                }
                RegisteredEffect.Value.PassDuration -= 1;
                if (RegisteredEffect.Value.PassDuration == 0)
                {
                    ContinousEffectsToRemove.Add(RegisteredEffect.Key);
                }
            }
            foreach(int EffectToRemove in ContinousEffectsToRemove)
            {
                m_RegisteredContinousAbilities.Remove(EffectToRemove);
            }
            List<int> RegisteredEffectsToRemove = new List<int>();
            foreach (KeyValuePair<int, RegisteredTrigger> RegisteredEffect in m_RegisteredTriggeredAbilities)
            {
                if (RegisteredEffect.Value.IsEndOfTurn)
                {
                    RegisteredEffectsToRemove.Add(RegisteredEffect.Key);
                }
            }
            foreach (int EffectToRemove in RegisteredEffectsToRemove)
            {
                m_RegisteredTriggeredAbilities.Remove(EffectToRemove);
            }
            yield break;
        }

        void p_PassPriority()
        {
            if (m_TheStack.Count > 0 && m_PriorityTabled && m_CurrentPlayerPriority == m_TheStack.Peek().Source.PlayerIndex)
            {
                IEnumerator ResolveResult = p_ResolveTopOfStack();
                bool NotFinished = ResolveResult.MoveNext();
                if (NotFinished)
                {
                    m_CurrentResolution = ResolveResult;
                }
            }
            else if (m_EndOfTurnPass && m_CurrentPlayerTurn != m_CurrentPlayerPriority)
            {
                IEnumerator EndEnumerator = p_ChangeTurn();
                EndEnumerator.MoveNext();
            }
            else
            {
                m_CurrentPlayerPriority += 1;
                m_CurrentPlayerPriority %= m_PlayerCount;
            }
            if(m_EventHandler != null)
            {
                //m_EventHandler.OnPlayerPassPriority();
                m_EventHandler.OnPlayerPassPriority(m_CurrentPlayerPriority);
            }
        }

        void p_CheckStateBasedAction()
        {
            List<int> UnitsToDestroy = new List<int>();
            foreach(KeyValuePair<int,UnitInfo> Unit in m_UnitInfos)
            {
                if(Unit.Value.Stats.HP <= 0)
                {
                    UnitsToDestroy.Add(Unit.Key);
                }
            }
            foreach(int Unit in UnitsToDestroy)
            {
                p_DestroyUnit(Unit);
            }
        }
        

        bool m_PriorityTabled = false;

        //Modifiers
        public void ExecuteAction(Action ActionToExecute)
        {
            string Error;
            if (ActionToExecute.PlayerIndex == -1)
            {
                ActionToExecute.PlayerIndex = m_CurrentPlayerPriority;
            }
            if (!ActionIsValid(ActionToExecute,out Error))
            {
                throw new ArgumentException("Invalid action to execute: "+Error);
            }
            bool EndOfTurnPass = false;
            bool PriorityTabled = false;

            bool ActionIsExecuted = true;

            if(ActionToExecute is  MoveAction)
            {
                MoveAction MoveToExecute = (MoveAction)ActionToExecute;
                UnitInfo UnitToMove = m_UnitInfos[MoveToExecute.UnitID];
                Coordinate OldPosition = UnitToMove.TopLeftCorner;
                //Bounds alreay checked
                p_MoveUnit(MoveToExecute.UnitID, MoveToExecute.NewPosition);
                
                if(m_EventHandler != null)
                {
                    m_EventHandler.OnUnitMove(MoveToExecute.UnitID, OldPosition, MoveToExecute.NewPosition);
                }
                if((UnitToMove.Flags & UnitFlags.IsActivated) == 0)
                {
                    UnitToMove.Flags |= UnitFlags.IsActivated;
                    m_PlayerIntitiative[UnitToMove.PlayerIndex] -= UnitToMove.Stats.ActivationCost;
                    if (m_EventHandler != null)
                    {
                        m_EventHandler.OnInitiativeChange(m_PlayerIntitiative[UnitToMove.PlayerIndex], UnitToMove.PlayerIndex);
                    }
                }
                UnitToMove.Flags |= UnitFlags.HasMoved;
                p_PassPriority();
            }
            else if (ActionToExecute is RotateAction)
            {
                RotateAction RotationToExecute = (RotateAction)ActionToExecute;
                UnitInfo UnitToMove = m_UnitInfos[RotationToExecute.UnitID];
                Coordinate OldPosition = UnitToMove.TopLeftCorner;
                //Bounds alreay checked
                p_RotateUnit(RotationToExecute.UnitID, RotationToExecute.NewRotation);
                if ((UnitToMove.Flags & UnitFlags.IsActivated) == 0)
                {
                    UnitToMove.Flags |= UnitFlags.IsActivated;
                    m_PlayerIntitiative[UnitToMove.PlayerIndex] -= UnitToMove.Stats.ActivationCost;
                    if (m_EventHandler != null)
                    {
                        m_EventHandler.OnInitiativeChange(m_PlayerIntitiative[UnitToMove.PlayerIndex], UnitToMove.PlayerIndex);
                    }
                }
                UnitToMove.Flags |= UnitFlags.HasMoved;
                p_PassPriority();
            }
            else if(ActionToExecute is AttackAction)
            {
                AttackAction AttackToExecute = (AttackAction)ActionToExecute;
                UnitInfo AttackerInfo = p_GetProcessedUnitInfo(AttackToExecute.AttackerID); //m_UnitInfos[AttackToExecute.AttackerID];
                UnitInfo DefenderInfo = p_GetProcessedUnitInfo(AttackToExecute.DefenderID);//m_UnitInfos[AttackToExecute.DefenderID];
                if (m_EventHandler != null)
                {
                    m_EventHandler.OnUnitAttack(AttackToExecute.AttackerID, AttackToExecute.DefenderID);
                }
                UnitInfo UnderlyingDefenderInfo = m_UnitInfos[AttackToExecute.DefenderID];
                //DefenderInfo.Stats.HP -= AttackerInfo.Stats.Damage;
                UnderlyingDefenderInfo.Stats.HP -= AttackerInfo.Stats.Damage;
                UnitInfo InfoToModify = m_UnitInfos[AttackToExecute.AttackerID];
                if ((AttackerInfo.Flags & UnitFlags.IsActivated) == 0)
                {
                    InfoToModify.Flags |=UnitFlags.IsActivated;
                    m_PlayerIntitiative[AttackerInfo.PlayerIndex] -= AttackerInfo.Stats.ActivationCost;
                    if (m_EventHandler != null)
                    {
                        m_EventHandler.OnInitiativeChange(m_PlayerIntitiative[AttackerInfo.PlayerIndex], AttackerInfo.PlayerIndex);
                    }
                }
                InfoToModify.Flags |= UnitFlags.HasAttacked;
                p_PassPriority();
            }
            else if(ActionToExecute is PassAction)
            {
                ActionIsExecuted = false; 
                if (m_TheStack.Count != 0 && m_CurrentPlayerPriority != m_TheStack.Peek().Source.PlayerIndex)
                {
                    PriorityTabled = true;
                    m_PriorityTabled = true;
                }
                if (m_TheStack.Count == 0 && m_CurrentPlayerTurn == m_CurrentPlayerPriority)
                {
                    EndOfTurnPass = true;
                    m_EndOfTurnPass = true;
                }
                p_PassPriority();
            }
            else if(ActionToExecute is EffectAction)
            {
                PriorityTabled = false;
                m_PriorityTabled = false;
                EffectAction EffectToExecute = (EffectAction)ActionToExecute;
                UnitInfo UnitWithEffect = m_UnitInfos[EffectToExecute.UnitID];
                Ability_Activated AbilityToActivate =(Ability_Activated) UnitWithEffect.Abilities[EffectToExecute.EffectIndex];
                StackEntity NewEntity = new StackEntity();
                NewEntity.EffectToResolve = AbilityToActivate.ActivatedEffect;
                NewEntity.EffectToResolve.Animation = AbilityToActivate.Animation;
                NewEntity.EffectToResolve.SetText(AbilityToActivate.GetDescription());
                NewEntity.Targets = EffectToExecute.Targets;
                NewEntity.Source = new EffectSource_Unit(ActionToExecute.PlayerIndex,EffectToExecute.UnitID,EffectToExecute.EffectIndex);
                NewEntity.Source.PlayerIndex = ActionToExecute.PlayerIndex;
                m_TheStack.Push(NewEntity);
                if(m_EventHandler != null)
                {
                    m_EventHandler.OnStackPush(NewEntity);
                }
                if ((UnitWithEffect.Flags & UnitFlags.IsActivated) == 0)
                {
                    UnitWithEffect.Flags |= UnitFlags.IsActivated;
                    m_PlayerIntitiative[UnitWithEffect.PlayerIndex] -= UnitWithEffect.Stats.ActivationCost;
                    if (m_EventHandler != null)
                    {
                        m_EventHandler.OnInitiativeChange(m_PlayerIntitiative[UnitWithEffect.PlayerIndex], UnitWithEffect.PlayerIndex);
                    }
                }
                UnitWithEffect.AbilityActivated[EffectToExecute.EffectIndex] = true;
                p_PassPriority();
            }
            else
            {
                throw new ArgumentException("Invalid Action type");
            }
            if(!m_ActionIsPlayed && ActionIsExecuted)
            {
                m_ActionIsPlayed = true;
            }
            p_CheckStateBasedAction();
            m_EndOfTurnPass = EndOfTurnPass;
            m_PriorityTabled = PriorityTabled;
        }

        //Observers
        IEnumerator<Target> p_TotalTargetIterator()
        {
            for(int i = 0; i < m_PlayerCount;i++)
            {
                Target_Player PlayerTarget = new Target_Player();
                PlayerTarget.PlayerIndex = i;
                yield return PlayerTarget;
            }
            for(int i = 0; i < m_Tiles.Count; i++)
            {
                for(int j = 0; j < m_Tiles[0].Count;j++)
                {
                    Target_Tile TileTarget = new Target_Tile();
                    TileTarget.TargetCoordinate = new Coordinate(j, i);
                    yield return TileTarget;
                }
            }
            foreach (KeyValuePair<int, UnitInfo> Unit in m_UnitInfos)
            {
                Target_Unit UnitTarget = new Target_Unit();
                UnitTarget.UnitID = Unit.Key;
                yield return (UnitTarget);
            }
            yield break;
        }
        object p_DealDamage(UnitScript.BuiltinFuncArgs Args)
        {
            int DamageToDeal = (int) Args.Arguments[1];
            UnitIdentifier IdToModify = (UnitIdentifier)Args.Arguments[0];
            p_DealDamage(IdToModify.ID,DamageToDeal);
            return null;
        }
        object p_Enemy(UnitScript.BuiltinFuncArgs Args)
        {
            UnitIdentifier IdToModify = (UnitIdentifier)Args.Arguments[0];
            return m_CurrentPlayerTurn != m_UnitInfos[IdToModify.ID].PlayerIndex;
        }
        object p_Friendly(UnitScript.BuiltinFuncArgs Args)
        {
            UnitIdentifier IdToModify = (UnitIdentifier)Args.Arguments[0];
            return m_CurrentPlayerTurn == m_UnitInfos[IdToModify.ID].PlayerIndex;
        }
        object p_DestroyUnit(UnitScript.BuiltinFuncArgs Args)
        {
            UnitIdentifier IdToModify = (UnitIdentifier)Args.Arguments[0];
            p_DestroyUnit(IdToModify.ID);
            return null;
        }
        //
        object p_Distance(UnitScript.BuiltinFuncArgs Args)
        {
            UnitIdentifier FirstUnit = (UnitIdentifier)Args.Arguments[0];
            UnitIdentifier SecondUnit = (UnitIdentifier)Args.Arguments[1];
            var FirstUnitInfo = m_UnitInfos[FirstUnit.ID];
            var SecondUnitInfo = m_UnitInfos[SecondUnit.ID];
            return p_CalculateUnitDistance(FirstUnitInfo,SecondUnitInfo);
        }
        object p_RegisterContinous(UnitScript.BuiltinFuncArgs Args)
        {
            UnitScript.ContinousAbility AbilityToRegister = (UnitScript.ContinousAbility)Args.Arguments[0];
            RegisteredContinousEffect EffectToRegister = new RegisteredContinousEffect();
            EffectToRegister.AffectedEntities = AbilityToRegister.Ability.AffectedEntities;
            EffectToRegister.EffectToApply = AbilityToRegister.Ability.EffectToApply;
            if(Args.KeyArguments.ContainsKey("TurnDuration"))
            {
                EffectToRegister.TurnDuration = (int)Args.KeyArguments["TurnDuration"];
            }
            if(Args.KeyArguments.ContainsKey("PassDuration"))
            {
                EffectToRegister.PassDuration = (int)Args.KeyArguments["PassDuration"];
            }
            //OBS Effect source most likely depreactd, should bbe baked in the unit environemnt
            //returned by the "lambda"
            //EffectToRegister.AbilitySource = new EffectSource_Empty();
            EffectToRegister.AbilitySource = (EffectSource)Args.Envir.GetVar("SOURCE");
            p_RegisterContinousEffect(EffectToRegister);
            return null;
        }
        object p_Tag(UnitScript.BuiltinFuncArgs Args)
        {
            UnitIdentifier FirstUnit = (UnitIdentifier)Args.Arguments[0];
            string TagToCheck = (string)Args.Arguments[1];
            return m_UnitInfos[FirstUnit.ID].Tags.Contains(TagToCheck);
        }
        Dictionary<string,UnitScript.Builtin_FuncInfo> GetUnitScriptFuncs()
        {
            Dictionary<string,UnitScript.Builtin_FuncInfo> ReturnValue = new Dictionary<string, UnitScript.Builtin_FuncInfo>();

            UnitScript.Builtin_FuncInfo DealDamage = new UnitScript.Builtin_FuncInfo();
            DealDamage.ArgTypes = new List<Type>{typeof(UnitIdentifier),typeof(int)};
            DealDamage.ResultType = typeof(void);
            DealDamage.ValidContexts = UnitScript.EvalContext.Resolve;
            DealDamage.Callable = p_DealDamage;
            ReturnValue["DealDamage"] = DealDamage;

            UnitScript.Builtin_FuncInfo DestroyUnit = new UnitScript.Builtin_FuncInfo();
            DestroyUnit.ArgTypes = new List<Type>{typeof(UnitIdentifier)};
            DestroyUnit.ResultType = typeof(void);
            DestroyUnit.ValidContexts = UnitScript.EvalContext.Resolve;
            DestroyUnit.Callable = p_DestroyUnit;
            ReturnValue["DestroyUnit"] = DestroyUnit;

            UnitScript.Builtin_FuncInfo Enemy = new UnitScript.Builtin_FuncInfo();
            Enemy.ArgTypes = new List<Type>{typeof(UnitIdentifier)};
            Enemy.ResultType = typeof(bool);
            Enemy.ValidContexts = UnitScript.EvalContext.Predicate | UnitScript.EvalContext.Resolve;
            Enemy.Callable = p_Enemy;
            ReturnValue["Enemy"] = Enemy;

            UnitScript.Builtin_FuncInfo Friendly = new UnitScript.Builtin_FuncInfo();
            Friendly.ArgTypes = new List<Type>{typeof(UnitIdentifier)};
            Friendly.ResultType = typeof(bool);
            Friendly.ValidContexts = UnitScript.EvalContext.Predicate | UnitScript.EvalContext.Resolve;
            Friendly.Callable = p_Friendly;
            ReturnValue["Friendly"] = Friendly;
            
            UnitScript.Builtin_FuncInfo Tag = new UnitScript.Builtin_FuncInfo();
            Tag.ArgTypes = new List<Type>{typeof(UnitIdentifier),typeof(string)};
            Tag.ResultType = typeof(bool);
            Tag.ValidContexts = UnitScript.EvalContext.Predicate | UnitScript.EvalContext.Resolve;
            Tag.Callable = p_Tag;
            ReturnValue["Tag"] = Tag;

            UnitScript.Builtin_FuncInfo Distance = new UnitScript.Builtin_FuncInfo();
            Distance.ArgTypes = new List<Type>{typeof(UnitIdentifier),typeof(UnitIdentifier)};
            Distance.ResultType = typeof(int);
            Distance.ValidContexts = UnitScript.EvalContext.Predicate | UnitScript.EvalContext.Resolve;
            Distance.Callable = p_Distance;
            ReturnValue["Distance"] = Distance;

            UnitScript.Builtin_FuncInfo RegisterContinous = new UnitScript.Builtin_FuncInfo();
            RegisterContinous.ArgTypes = new List<Type>{typeof(UnitScript.ContinousAbility)};
            RegisterContinous.KeyArgTypes = new Dictionary<string, Type>{{"TurnDuration",typeof(int)},{"PassDuration",typeof(int)}};
            RegisterContinous.ResultType = typeof(void);
            RegisterContinous.ValidContexts = UnitScript.EvalContext.Resolve;
            RegisterContinous.Callable = p_RegisterContinous;
            ReturnValue["RegisterContinous"] = RegisterContinous;

            return ReturnValue;
        }
        public void SetScriptHandler(UnitScript.UnitConverter ScriptHandler)
        {
            m_ScriptHandler = ScriptHandler;
            m_ScriptHandler.AddBuiltins(GetUnitScriptFuncs());
        }
        public List<Target> GetPossibleTargets(TargetCondition CurrentCondition , EffectSource Source, List<Target> CurrentTargets)
        {
            List<Target> ReturnValue = new List<Target>();
            IEnumerator<Target> TargetEnumerator = p_TotalTargetIterator();
            while (TargetEnumerator.MoveNext())
            {
                string Error;
                if(p_VerifyTarget(CurrentCondition,Source,CurrentTargets,TargetEnumerator.Current,out Error))
                {
                    ReturnValue.Add(TargetEnumerator.Current);
                }
            }
            return (ReturnValue);
        }
        int p_CalculateTileDistance(List<Coordinate> SourceTiles,List<Coordinate> TargetTiles)
        {
            int ReturnValue = -1;
            //O(n^2) implementation where n is the amount of tiles, inefficient, but the most generic
            foreach(Coordinate SourceCoord in SourceTiles)
            {
                foreach(Coordinate TargetCoord in TargetTiles)
                {
                    int NewDistance = Coordinate.Distance(SourceCoord, TargetCoord);
                    if (ReturnValue == -1 || NewDistance < ReturnValue)
                    {
                        ReturnValue = NewDistance;
                    }
                }
            }
            return (ReturnValue);
        }
        int p_CalculateUnitDistance(UnitInfo FirstUnit,UnitInfo SecondUnit)
        {
            int ReturnValue = 0;
            ReturnValue = p_CalculateTileDistance(p_GetAbsolutePositions(FirstUnit.TopLeftCorner,FirstUnit.UnitTileOffsets), p_GetAbsolutePositions(SecondUnit.TopLeftCorner,SecondUnit.UnitTileOffsets));
            return (ReturnValue);
        }
        public bool ActionIsValid(Action ActionToCheck,out string OutInfo)
        {
            if(m_GameFinished)
            {
                OutInfo = "Game is finished";
                return false;
            }
            if(ActionToCheck.PlayerIndex == -1)
            {
                ActionToCheck.PlayerIndex = m_CurrentPlayerPriority;
            }
            if(m_CurrentResolution != null)
            {
                OutInfo = "Cant execute action during resolution, appropriate targets must be specified to continue";
                return (false);
            }
            bool ReturnValue = true;
            string ErrorString = "";
            if(ActionToCheck.PlayerIndex != m_CurrentPlayerPriority)
            {
                ReturnValue = false;
                ErrorString = "Invalid player index: Player doesnt hold priority";
                OutInfo = ErrorString;
                return (ReturnValue);
            }
            if(ActionToCheck is MoveAction)
            {
                MoveAction MoveToCheck = (MoveAction)ActionToCheck;
                if (MoveToCheck.PlayerIndex != m_CurrentPlayerTurn)
                {
                    ReturnValue = false;
                    ErrorString = "Can only move on your own turn";
                    OutInfo = ErrorString;
                    return (ReturnValue);
                }
                if (!m_UnitInfos.ContainsKey(MoveToCheck.UnitID))
                {
                    ReturnValue = false;
                    ErrorString = "Invalid unit id";
                    OutInfo = ErrorString;
                    return (ReturnValue);
                }
                List<Coordinate> Moves = PossibleMoves(MoveToCheck.UnitID);
                if (!Moves.Contains(MoveToCheck.NewPosition))
                {
                    ReturnValue = false;
                    ErrorString = "Invalid new position for unit";
                    OutInfo = ErrorString;
                    return (ReturnValue);
                }
                UnitInfo AssociatedUnit = p_GetProcessedUnitInfo(MoveToCheck.UnitID); //m_UnitInfos[MoveToCheck.UnitID];
                if((AssociatedUnit.Flags & UnitFlags.HasMoved) != 0)
                {
                    ReturnValue = false;
                    ErrorString = "Can only move unit once per activation";
                    OutInfo = ErrorString;
                    return (ReturnValue);
                }
                if ((AssociatedUnit.Flags & UnitFlags.IsActivated) == 0 && AssociatedUnit.Stats.ActivationCost > m_PlayerIntitiative[MoveToCheck.PlayerIndex])
                {
                    ReturnValue = false;
                    ErrorString = "Not enough initiative to activate unit";
                    OutInfo = ErrorString;
                    return (ReturnValue);
                }

            }
            else if(ActionToCheck is RotateAction)
            {
                RotateAction RotationToCheck = (RotateAction)ActionToCheck;
                if (RotationToCheck.PlayerIndex != m_CurrentPlayerTurn)
                {
                    ReturnValue = false;
                    ErrorString = "Can only move on your own turn";
                    OutInfo = ErrorString;
                    return (ReturnValue);
                }
                if (!m_UnitInfos.ContainsKey(RotationToCheck.UnitID))
                {
                    ReturnValue = false;
                    ErrorString = "Invalid unit id";
                    OutInfo = ErrorString;
                    return (ReturnValue);
                }
                UnitInfo AssociatedUnit = p_GetProcessedUnitInfo(RotationToCheck.UnitID); //m_UnitInfos[MoveToCheck.UnitID];
                if ((AssociatedUnit.Flags & UnitFlags.HasMoved) != 0)
                {
                    ReturnValue = false;
                    ErrorString = "Can only move unit once per activation";
                    OutInfo = ErrorString;
                    return (ReturnValue);
                }
                if ((AssociatedUnit.Flags & UnitFlags.IsActivated) == 0 && AssociatedUnit.Stats.ActivationCost > m_PlayerIntitiative[RotationToCheck.PlayerIndex])
                {
                    ReturnValue = false;
                    ErrorString = "Not enough initiative to activate unit";
                    OutInfo = ErrorString;
                    return (ReturnValue);
                }
            }
            else if(ActionToCheck is AttackAction)
            {
                AttackAction AttackToCheck = (AttackAction)ActionToCheck;
                if(AttackToCheck.PlayerIndex != m_CurrentPlayerTurn)
                {
                    ReturnValue = false;
                    ErrorString = "Can only attack on your turn";
                    OutInfo = ErrorString;
                    return (ReturnValue);
                }
                if(m_UnitInfos.ContainsKey(AttackToCheck.AttackerID) == false || m_UnitInfos.ContainsKey(AttackToCheck.DefenderID) == false)
                {
                    ReturnValue = false;
                    ErrorString = "Invalid unit's for attack: Defender or attacker doesn't exist";
                    OutInfo = ErrorString;
                    return (ReturnValue);
                }
                UnitInfo DefenderInfo = p_GetProcessedUnitInfo(AttackToCheck.DefenderID);  //m_UnitInfos[AttackToCheck.DefenderID];
                UnitInfo AttackerInfo = p_GetProcessedUnitInfo(AttackToCheck.AttackerID); //m_UnitInfos[AttackToCheck.AttackerID];
                if(DefenderInfo.PlayerIndex == AttackerInfo.PlayerIndex)
                {
                    ReturnValue = false;
                    ErrorString = "Can't attack friendly unit";
                    OutInfo = ErrorString;
                    return (ReturnValue);
                }
                if (p_CalculateUnitDistance(DefenderInfo,AttackerInfo) > AttackerInfo.Stats.Range)
                {
                    ReturnValue = false;
                    ErrorString = "Defender out of range for attacker";
                    OutInfo = ErrorString;
                    return (ReturnValue);
                }
                if((AttackerInfo.Flags & UnitFlags.CantAttack) != 0)
                {
                    ReturnValue = false;
                    ErrorString = "Unit is unable to attack";
                    OutInfo = ErrorString;
                    return (ReturnValue);
                }

                if((AttackerInfo.Flags & UnitFlags.HasAttacked) != 0)
                {
                    ReturnValue = false;
                    ErrorString = "Can only attack once per activation";
                    OutInfo = ErrorString;
                    return (ReturnValue);
                }
                if ((AttackerInfo.Flags & UnitFlags.IsActivated) == 0 && AttackerInfo.Stats.ActivationCost > m_PlayerIntitiative[AttackToCheck.PlayerIndex])
                {
                    ReturnValue = false;
                    ErrorString = "Not enough initiative to activate unit";
                    OutInfo = ErrorString;
                    return (ReturnValue);
                }
            }
            else if (ActionToCheck is PassAction)
            {
                //always true at this point
            }
            else if(ActionToCheck is EffectAction)
            {
                EffectAction EffectToCheck = (EffectAction)ActionToCheck;
                if(m_UnitInfos.ContainsKey(EffectToCheck.UnitID) == false)
                {
                    ReturnValue = false;
                    ErrorString = "Can't activate effect of unit that doesn't exist";
                    OutInfo = ErrorString;
                    return (ReturnValue);
                }
                UnitInfo AssociatedUnit = p_GetProcessedUnitInfo(EffectToCheck.UnitID);
                if(AssociatedUnit.PlayerIndex != EffectToCheck.PlayerIndex)
                {
                    ReturnValue = false;
                    ErrorString = "Can't activate the effect of opponents units";
                    OutInfo = ErrorString;
                    return (ReturnValue);
                }
                if(AssociatedUnit.Abilities.Count <= EffectToCheck.EffectIndex || EffectToCheck.EffectIndex < 0)
                {
                    ReturnValue = false;
                    ErrorString = "Invalid effect index for unit";
                    OutInfo = ErrorString;
                    return (ReturnValue);
                }
                if(AssociatedUnit.AbilityActivated[EffectToCheck.EffectIndex])
                {
                    ReturnValue = false;
                    ErrorString = "Can only activate an effect once per turn";
                    OutInfo = ErrorString;
                    return (ReturnValue);
                }
                if(AssociatedUnit.Abilities[EffectToCheck.EffectIndex].Type != AbilityType.Activated)
                {
                    ReturnValue = false;
                    ErrorString = "Can only activate activated abilities";
                    OutInfo = ErrorString;
                    return (ReturnValue);
                }
                if((AssociatedUnit.Flags & UnitFlags.Silenced) != 0)
                {
                    ReturnValue = false;
                    ErrorString = "Unit is silenced";
                    OutInfo = ErrorString;
                    return (ReturnValue);
                }
                if(m_CurrentPlayerTurn != AssociatedUnit.PlayerIndex && ((Ability_Activated) AssociatedUnit.Abilities[EffectToCheck.EffectIndex]).Speed != SpellSpeed.Speed2)
                {
                    ReturnValue = false;
                    ErrorString = "Can only active spell speed 2 abilities on your opponents turn";
                    OutInfo = ErrorString;
                    return (ReturnValue);
                }
                Ability_Activated AbilityToActive =(Ability_Activated ) AssociatedUnit.Abilities[EffectToCheck.EffectIndex];
                bool PreconditionsSatisfied = p_VerifyActivationConditions(AbilityToActive.Conditions, out OutInfo);
                if(!PreconditionsSatisfied)
                {
                    return (PreconditionsSatisfied);
                }
                if(!p_VerifyTargets(AbilityToActive.ActivationTargets,new EffectSource_Unit(ActionToCheck.PlayerIndex,AssociatedUnit.UnitID,EffectToCheck.EffectIndex),EffectToCheck.Targets,out OutInfo))
                {
                    ReturnValue = false;
                    OutInfo = ErrorString;
                    return (ReturnValue);
                }
                if ((AssociatedUnit.Flags & UnitFlags.IsActivated) == 0 && AssociatedUnit.Stats.ActivationCost > m_PlayerIntitiative[AssociatedUnit.PlayerIndex])
                {
                    ReturnValue = false;
                    ErrorString = "Not enough initiative to activate unit";
                    OutInfo = ErrorString;
                    return (ReturnValue);
                }
            }
            else
            {
                throw new ArgumentException("Invalid action type");
            }
            OutInfo = ErrorString;
            return (ReturnValue);
        }

        public enum TraversalFlags
        {
            Null = 0,
            StartedInObscuring = 1,
            TraversedObscuring = 1<<2
        }

        void p_PossibleMoves(Coordinate CurrentDif,
            List<Coordinate> UnitTiles,
            int CurrentMovement,
            List<Coordinate> OutPossibleMoves,
            Dictionary<(Coordinate, TraversalFlags),int> VisitedSpaces,
            int UnitID,
            TraversalFlags CurrentTraversalFlags
            )
        {
            if(CurrentMovement == 0)
            {
                return;
            }
            foreach(Coordinate DiffDiff in new Coordinate[] {new Coordinate(1,0),new Coordinate(0,1),new Coordinate(-1,0),new Coordinate(0,-1)})
            {
                Coordinate TotalDiff = CurrentDif+DiffDiff;
                if(VisitedSpaces.ContainsKey( (TotalDiff, CurrentTraversalFlags) ))
                {
                    int PreviousMovement = VisitedSpaces[(TotalDiff, CurrentTraversalFlags)];
                    if(PreviousMovement >= CurrentMovement)
                    {
                        continue;
                    }
                }
                bool ValidPosition = true;
                foreach(Coordinate Coord in UnitTiles)
                {
                    Coordinate NewCoord = Coord+TotalDiff;
                    if ((NewCoord.Y >= m_Tiles.Count || NewCoord.Y < 0) || (NewCoord.X >= m_Tiles[0].Count || NewCoord.X < 0))
                    {
                        ValidPosition = false;
                        break;
                    }
                    if ((m_Tiles[NewCoord.Y][NewCoord.X].StandingUnitID != 0 && m_Tiles[NewCoord.Y][NewCoord.X].StandingUnitID != UnitID) || m_Tiles[NewCoord.Y][NewCoord.X].HasObjective
                        || (m_Tiles[NewCoord.Y][NewCoord.X].Flags & TileFlags.Impassable) != 0)
                    {
                        ValidPosition = false;
                        break;
                    }
                }
                if(!ValidPosition)
                {
                    continue;
                }
                TileInfo CurrentTileInfo = m_Tiles[TotalDiff.Y][TotalDiff.X];
                if((CurrentTraversalFlags &  TraversalFlags.StartedInObscuring) == TraversalFlags.Null && 
                    (CurrentTraversalFlags & TraversalFlags.TraversedObscuring) != TraversalFlags.Null &&
                        (CurrentTileInfo.Flags & TileFlags.Obscuring) == TileFlags.Null)

                {
                    //starting from outside obscuring, traversed in obscuring, and the going out is not allowed
                    continue;
                }
                VisitedSpaces[(TotalDiff,CurrentTraversalFlags)] = CurrentMovement;
                if((CurrentTileInfo.Flags & TileFlags.Obscuring) != TileFlags.Null)
                {
                    CurrentTraversalFlags = CurrentTraversalFlags | TraversalFlags.TraversedObscuring;
                }
                OutPossibleMoves.Add(TotalDiff);
                p_PossibleMoves(TotalDiff,UnitTiles, CurrentMovement - 1, OutPossibleMoves,VisitedSpaces,UnitID, CurrentTraversalFlags);
            }
        }
        List<Coordinate> p_NormalizeMoves(List<Coordinate> MovesToNormalize)
        {
            List<Coordinate> ReturnValue = new List<Coordinate>();
            MovesToNormalize.Sort();
            for(int i = 0; i < MovesToNormalize.Count-1;i++)
            {
                if(MovesToNormalize[i].Equals(MovesToNormalize[i+1]) == false)
                {
                    ReturnValue.Add(MovesToNormalize[i]);
                }
            }
            if(MovesToNormalize.Count > 0)
            {
                ReturnValue.Add(MovesToNormalize[MovesToNormalize.Count - 1]);
            }
            return (ReturnValue);
        }
        public List<Coordinate> PossibleMoves(int UnitID)
        {
            List<Coordinate> ReturnValue = new List<Coordinate>();

            if(m_UnitInfos.ContainsKey(UnitID) == false)
            {
                throw new Exception("Need valid unit to check possible moves");
            }
            UnitInfo UnitToMove = p_GetProcessedUnitInfo(UnitID);
            //if((UnitToMove.Flags & UnitFlags.HasMoved) != 0)
            //{
            //    return (ReturnValue);
            //}
            //ReturnValue.Add(new Coordinate(UnitToMove.Position));
            TraversalFlags InitialTraversalFlags = 0;
            foreach(Coordinate Offset in UnitToMove.UnitTileOffsets)
            {
                Coordinate NewCoord = Offset + UnitToMove.TopLeftCorner;
                if((m_Tiles[NewCoord.Y][NewCoord.X].Flags & TileFlags.Obscuring) != TileFlags.Null)
                {
                    InitialTraversalFlags = TraversalFlags.StartedInObscuring;
                    break;
                }
            }
            p_PossibleMoves(UnitToMove.TopLeftCorner,UnitToMove.UnitTileOffsets, UnitToMove.Stats.Movement, ReturnValue, new Dictionary<(Coordinate,TraversalFlags), int>(),UnitID,InitialTraversalFlags);
            ReturnValue = p_NormalizeMoves(ReturnValue);
            return (ReturnValue);
        }

        List<Coordinate> p_GetTiles(int Range,Coordinate Origin,Dictionary<Coordinate,int> DepthDict)
        {
            List<Coordinate> ReturnValue = new List<Coordinate>();
            for (int i = -Range; i <= Range; i++)
            {
                for (int j = -Math.Abs(Range - Math.Abs(i)); j <= Math.Abs(Range - Math.Abs(i)); j++)
                {
                    Coordinate NewCoordinate = new Coordinate();
                    NewCoordinate.X = Origin.X + i;
                    NewCoordinate.Y = Origin.Y + j;
                    if ((NewCoordinate.X < 0 || NewCoordinate.X >= m_Tiles[0].Count) || (NewCoordinate.Y < 0 || NewCoordinate.Y >= m_Tiles.Count))
                    {
                        continue;
                    }
                    if (!DepthDict.ContainsKey(NewCoordinate))
                    {
                        ReturnValue.Add(NewCoordinate);
                        DepthDict[NewCoordinate] = Math.Abs(i) + Math.Abs(j);
                    }
                }
            }
            return (ReturnValue);
        }
        //Incredibly naive implementation, basicsally runs the function 6 times
        List<Coordinate> p_GetTiles(int Range,List<Coordinate> UnitTiles)
        {
            List<Coordinate> ReturnValue = new List<Coordinate>();
            Dictionary<Coordinate, int> Depths = new Dictionary<Coordinate, int>();
            foreach(Coordinate Origin in UnitTiles)
            {
                ReturnValue.AddRange(p_GetTiles(Range, Origin, Depths));
            }
            return (ReturnValue);
        }
        bool p_CoordInField(Coordinate Coord)
        {
            bool ReturnValue = true;
            if ((Coord.X < 0 || Coord.X >= m_Tiles[0].Count) || (Coord.Y < 0 || Coord.Y >= m_Tiles.Count))
            {
                ReturnValue = false;
            }
            return (ReturnValue);
        }

        class LinEqSolution
        {
            public double X = 0;
            public double Y = 0;
        }
        class LinEq
        {
            public double X = 0;
            public double Y = 0;
            public double C = 0;

            public LinEq()
            {

            }
            public LinEq(double NewX,double NewY,double NewC)
            {
                X = NewX;
                Y = NewY;
                C = NewC;
            }
            public LinEq(LinEq OtherEq)
            {
                X = OtherEq.X;
                Y = OtherEq.Y;
                C = OtherEq.C;
            }
            public bool Solve(LinEq OtherEq,out LinEqSolution Solution)
            {
                bool ReturnValue = true;
                LinEqSolution Result = new LinEqSolution();
                double SolX = 0;
                double SolY = 0;
                double SolC = 0;
                LinEq XPivot = null;
                LinEq YPivot = null;
                //cannot be infinite solutions, only paralell
                if(X == 0)
                {
                    if(OtherEq.X == 0)
                    {
                        Solution = Result;
                        return (false);
                    }
                    XPivot = new LinEq(OtherEq);
                    YPivot = new LinEq(this);
                }
                else
                {
                    if(X == 0)
                    {
                        Solution = Result;
                        return (false);
                    }
                    XPivot = new LinEq(this);
                    YPivot = new LinEq(OtherEq);
                }
                YPivot.Y -= (YPivot.X / XPivot.X) * XPivot.Y;
                YPivot.C -= (YPivot.X / XPivot.X) * XPivot.C;
                if(YPivot.Y == 0)
                {
                    //can only exist parallell, never the same, lines
                    Solution = Result;
                    return (false);
                }
                XPivot.C -= (XPivot.Y / YPivot.Y)*YPivot.C;
                XPivot.C /= XPivot.X;
                YPivot.C /= YPivot.Y;
                Result.X = XPivot.C;
                Result.Y = YPivot.C;
                Solution = Result;
                return (ReturnValue);
            }
        }
        bool p_HasLineOfSight(List<Coordinate> SourceTiles,List<Coordinate> BlockingTiles,Coordinate TargeTile)
        {
            bool ReturnValue = false;
            foreach(Coordinate SourceTile in SourceTiles)
            {
                Coordinate DiffVector = TargeTile - SourceTile;
                LinEq TargetLine = new LinEq();
                if(DiffVector.Y != 0)
                {
                    TargetLine.Y = ((double)DiffVector.X) / DiffVector.Y;
                    TargetLine.C = -(TargeTile.X - (TargetLine.Y * TargeTile.Y));
                    TargetLine.X = -1;
                }
                else if(DiffVector.X != 0)
                {
                    TargetLine.X = ((double)DiffVector.Y) / DiffVector.X;
                    TargetLine.C = -(TargeTile.Y - (TargetLine.X * TargeTile.X));
                    TargetLine.Y = -1;
                }
                //always have line of sight to itself
                else
                {
                    return (true);
                }
                bool Intersects = false;
                foreach (Coordinate BlockingTile in BlockingTiles)
                {
                    //slightly complicated method to determine of square intersects
                    float MaxHeight = BlockingTile.Y + 0.5f;
                    float MinHeight = BlockingTile.Y - 0.5f;
                    float MinX = BlockingTile.X - 0.5f;
                    float MaxX = BlockingTile.X + 0.5f;
                    //iterate through every possible eq
                    foreach (LinEq Eq in new LinEq[] { new LinEq(1, 0, MinX), new LinEq(1, 0, MaxX), new LinEq(0, 1, MinHeight), new LinEq(0, 1, MaxHeight) }) 
                    {
                        LinEqSolution Solution = new LinEqSolution();
                        if(Eq.Solve(TargetLine,out Solution))
                        {
                            LinEq SolutionDir = new LinEq();
                            SolutionDir.X = Solution.X - SourceTile.X;
                            SolutionDir.Y = Solution.Y - SourceTile.Y;
                            if(Math.Sign(SolutionDir.X) != Math.Sign(DiffVector.X) || Math.Sign(SolutionDir.Y) != Math.Sign(DiffVector.Y))
                            {
                                continue;
                            }
                            if((Solution.X >= MinX && Solution.X <= MaxX) && (Solution.Y >= MinHeight && Solution.Y <= MaxHeight))
                            {
                                Intersects = true;
                                break;
                            }
                        }
                    }
                    if(Intersects)
                    {
                        break;
                    }
                }
                if(!Intersects)
                {
                    ReturnValue = true;
                    break;
                }
            }
            return (ReturnValue);
        }
        public List<Coordinate> PossibleAttacks(int UnitID)
        {
            List<Coordinate> ReturnValue = new List<Coordinate>();
            UnitInfo AttackingUnit = p_GetProcessedUnitInfo(UnitID);
            List<Coordinate> BlockingTiles = new List<Coordinate>();
            HashSet<Coordinate> VisitedTiles = new HashSet<Coordinate>();
            List<Coordinate> SourceTiles = p_GetAbsolutePositions(AttackingUnit.TopLeftCorner, AttackingUnit.UnitTileOffsets);
            Queue<Coordinate> TilesToExamine = new Queue<Coordinate>(SourceTiles);
            VisitedTiles.UnionWith(TilesToExamine);
            while (TilesToExamine.Count > 0)
            {
                Coordinate CurrentCoord = TilesToExamine.Dequeue();
                TileInfo CurrentTile = m_Tiles[CurrentCoord.Y][CurrentCoord.X];
                if((CurrentTile.Flags & TileFlags.Impassable) != 0)
                {
                    BlockingTiles.Add(CurrentCoord);
                }
                else
                {
                    //check of passable
                    if(p_HasLineOfSight(SourceTiles,BlockingTiles,CurrentCoord))
                    {
                        ReturnValue.Add(CurrentCoord);
                        foreach(Coordinate Offset in new Coordinate[] {new Coordinate(1,0), new Coordinate(-1, 0), new Coordinate(0, 1), new Coordinate(0, -1) })
                        {
                            Coordinate NewCoord = Offset+CurrentCoord;
                            if(VisitedTiles.Contains(NewCoord))
                            {
                                continue;
                            }
                            if (!p_CoordInField(NewCoord))
                            {
                                continue;
                            }
                            int CoordDistance = p_CalculateTileDistance(SourceTiles, new List<Coordinate>(new[] { NewCoord }));
                            if(CoordDistance > AttackingUnit.Stats.Range)
                            {
                                continue;
                            }
                            VisitedTiles.Add(NewCoord);
                            TilesToExamine.Enqueue(NewCoord);
                        }
                    }
                }
            }
            //ReturnValue = p_GetTiles(AttackingUnit.Stats.Range,p_GetAbsolutePositions(AttackingUnit.TopLeftCorner,AttackingUnit.UnitTileOffsets));
            return (ReturnValue);
        }

        TargetCondition_Range p_GetRange(TargetCondition ConditionToInspect)
        {
            TargetCondition_Range ReturnValue = null;
            if(ConditionToInspect is TargetCondition_Range)
            {
                return ((TargetCondition_Range) ConditionToInspect);
            }
            else if(ConditionToInspect is TargetCondition_And)
            {
                TargetCondition_And SubConditions = (TargetCondition_And)ConditionToInspect;
                foreach(TargetCondition Condition in SubConditions.Conditions)
                {
                    ReturnValue = p_GetRange(Condition);
                    if(ReturnValue != null)
                    {
                        return (ReturnValue);
                    }
                }
            }
            else if(ConditionToInspect is TargetCondition_Or)
            {
                TargetCondition_Or SubConditions = (TargetCondition_Or)ConditionToInspect;
                foreach (TargetCondition Condition in SubConditions.Conditions)
                {
                    ReturnValue = p_GetRange(Condition);
                    if (ReturnValue != null)
                    {
                        return (ReturnValue);
                    }
                }
            }
            return (ReturnValue);
        }
        

        TargetCondition p_GetCondition(int UnitID,int EffectIndex,List<Target> CurrentTargets)
        {
            TargetCondition ReturnValue = null;
            UnitInfo AssociatedUnit = p_GetProcessedUnitInfo(UnitID);
            Ability AbilityToInspect = AssociatedUnit.Abilities[EffectIndex];
            if (!(AbilityToInspect is Ability_Activated))
            {
                return ReturnValue;
            }
            Ability_Activated ActivatedAbility = (Ability_Activated)AbilityToInspect;
            TargetInfo_List TargetList = (TargetInfo_List)ActivatedAbility.ActivationTargets;
            if (TargetList.Targets.Count == 0)
            {
                return ReturnValue;
            }
            ReturnValue = TargetList.Targets[CurrentTargets.Count];

            return (ReturnValue);
        }
        public bool TargetIsValid(int UnitID,int EffectIndex,List<Target> currentTargets,Target NewTarget)
        {
            bool ReturnValue = false;
            EffectSource_Unit Source = new EffectSource_Unit(m_UnitInfos[UnitID].PlayerIndex, UnitID, EffectIndex);
            string ErrorString = "";
            TargetCondition Condition = p_GetCondition(UnitID, EffectIndex, currentTargets);
            if(Condition == null)
            {
                return (ReturnValue);
            }
            ReturnValue = p_VerifyTarget(Condition, Source, currentTargets, NewTarget,out ErrorString);
            return (ReturnValue);
        }
        public List<Coordinate> GetAbilityRange(int UnitID, int effectIndex, List<Target> currentTargets)
        {
            List<Coordinate> ReturnValue = new List<Coordinate>();
            UnitInfo AssociatedUnit = p_GetProcessedUnitInfo(UnitID);
            Ability AbilityToInspect = AssociatedUnit.Abilities[effectIndex];
            if(!(AbilityToInspect is Ability_Activated))
            {
                return ReturnValue;
            }
            Ability_Activated ActivatedAbility = (Ability_Activated)AbilityToInspect;
            TargetInfo_List TargetList = (TargetInfo_List)ActivatedAbility.ActivationTargets;
            if(TargetList.Targets.Count == 0)
            {
                return ReturnValue;
            }
            TargetCondition ConditionToSatsify = TargetList.Targets[currentTargets.Count];
            EffectSource_Unit Source = new EffectSource_Unit(AssociatedUnit.PlayerIndex,UnitID,effectIndex);

            TargetCondition_Range Range = p_GetRange(ConditionToSatsify);
            if(Range == null)
            {
                return (ReturnValue);
            }
            List<Coordinate> OriginTiles = p_GetAbsolutePositions(AssociatedUnit.TopLeftCorner,AssociatedUnit.UnitTileOffsets);
            if(Range.TargetIndex != -1)
            {
                Target OriginTarget = currentTargets[Range.TargetIndex];
                if(OriginTarget is Target_Unit)
                {
                    UnitInfo ProccessedInfo = p_GetProcessedUnitInfo(((Target_Unit)OriginTarget).UnitID);
                    OriginTiles = p_GetAbsolutePositions(ProccessedInfo.TopLeftCorner,ProccessedInfo.UnitTileOffsets);
                }
                else if(OriginTarget is Target_Tile)
                {
                    OriginTiles = new List<Coordinate>();
                    OriginTiles.Add(((Target_Tile)OriginTarget).TargetCoordinate);
                }
            }
            ReturnValue = p_GetTiles(Range.Range, OriginTiles);
            return (ReturnValue);
        }

        void p_ApplyContinousEffect(UnitInfo InfoToModify,Effect Modifier,EffectSource Source)
        {
            if(Modifier is Effect_ContinousUnitScript)
            {
                Effect_ContinousUnitScript UnitScriptEffect = (Effect_ContinousUnitScript)Modifier;
                UnitInfo SourceUnit = m_UnitInfos[ ((EffectSource_Unit) Source).UnitID];
                SourceUnit.Envir.AddVar(UnitScriptEffect.UnitName,InfoToModify);
                m_ScriptHandler.Eval(SourceUnit.Envir,UnitScriptEffect.Expr);
            }
            else if(Modifier is Effect_IncreaseDamage)
            {
                InfoToModify.Stats.Damage += ((Effect_IncreaseDamage)Modifier).DamageIncrease;
            }
            else if(Modifier is Effect_IncreaseMovement)
            {
                InfoToModify.Stats.Movement += ((Effect_IncreaseMovement)Modifier).MovementIncrease;
                if(InfoToModify.Stats.Movement < 0)
                {
                    InfoToModify.Stats.Movement = 0;
                }
            }
            else if(Modifier is Effect_SilenceUnit)
            {
                InfoToModify.Flags |= UnitFlags.Silenced;
            }
            else if(Modifier is Effect_HeavyAttack)
            {
                if((InfoToModify.Flags & UnitFlags.HasMoved) != 0)
                {
                    InfoToModify.Flags |= UnitFlags.CantAttack;
                }
            }
            else
            {
                throw new Exception("Invalid continous modifier");
            }
        }

        private UnitInfo p_GetProcessedUnitInfo(int ID)
        {
            UnitInfo ReturnValue = new UnitInfo(m_UnitInfos[ID]);
            List<Target> EmptyTargets = new List<Target>();
            string ErrorString = "";
            foreach(KeyValuePair<int,RegisteredContinousEffect> ContinousEffect in m_RegisteredContinousAbilities)
            {
                if (p_VerifyTarget(ContinousEffect.Value.AffectedEntities, ContinousEffect.Value.AbilitySource,EmptyTargets, new Target_Unit(ID),out ErrorString))
                {
                    p_ApplyContinousEffect(ReturnValue, ContinousEffect.Value.EffectToApply,ContinousEffect.Value.AbilitySource);
                }
            }

            return (ReturnValue);
        }

        public UnitInfo GetUnitInfo(int ID)
        {
            UnitInfo ReturnValue = null;
            if(m_UnitInfos.ContainsKey(ID))
            {
                ReturnValue = p_GetProcessedUnitInfo(ID);
            }
            return (ReturnValue);
        }
        public TileInfo GetTileInfo(int X,int Y)
        {
            if(Y >= m_Tiles.Count || Y < 0)
            {
                throw new ArgumentException("Invalid Y coordinate: out or range");
            }
            if(X < 0 || X >= m_Tiles[0].Count)
            {
                throw new ArgumentException("Invalid X coordinate: out or range");
            }
            return m_Tiles[Y][X];
        }
    }
}
