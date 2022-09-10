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
        public virtual string GetText()
        {
            return m_Text;
        }
        public void SetText(string NewText)
        {
            m_Text = NewText;
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
        public Effect_RegisterContinousAbility(TargetRetriever Retriever,TargetCondition Condition,Effect NewEffect,int NewPassDuration = 0,int NewTurnDuration = 0)
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
        public TargetCondition AffectedEntities = new TargetCondition_True();
        public Effect EffectToApply;
        public Ability_Continous() : base(AbilityType.Continous)
        {

        }
    }


    public class Target : MBJson.JSONTypeConverter,MBJson.JSONDeserializeable
    {
        public Type GetType(int SerializedType)
        {
            Type ReturnValue = null;
            if(Type == TargetType.Player)
            {
                ReturnValue = typeof(Target_Player);
            }
            else if(Type == TargetType.Tile)
            {
                ReturnValue = typeof(Target_Tile);
            }
            else if (Type == TargetType.Unit)
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
    public class TargetCondition
    {
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
        public int X;
        public int Y;
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
        public static Coordinate operator+(Coordinate LHS,Coordinate RHS)
        {
            Coordinate ReturnValue = new Coordinate();
            ReturnValue.X = LHS.X + RHS.X;
            ReturnValue.Y = LHS.Y + RHS.Y;
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
        public Coordinate NewPosition;
    }
    [Serializable]
    public class AttackAction : Action
    {
        public AttackAction() : base(ActionType.Attack) { }
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
            ActivationCost = StatToCopy.ActivationCost;
            Range = StatToCopy.Range;
            Damage = StatToCopy.Damage;
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


        //Dynamic stuff
        public List<bool> AbilityActivated = new List<bool>(); 
        public Coordinate Position = new Coordinate();
        //public bool IsActivated = false;
        //public bool HasMoved = false;
        //public bool HasAttacked = false;
        public UnitFlags Flags = 0;
        //
        public UnitInfo()
        {

        }
        public UnitInfo(UnitInfo InfoToCopy)
        {
            UnitID = InfoToCopy.UnitID;
            PlayerIndex = InfoToCopy.PlayerIndex;
            OpaqueInteger = InfoToCopy.OpaqueInteger;
            Position = InfoToCopy.Position;
            Abilities = new List<Ability>(InfoToCopy.Abilities);
            Stats = new UnitStats(InfoToCopy.Stats);
            //IsActivated =   InfoToCopy.IsActivated;
            //HasMoved    =   InfoToCopy.HasMoved   ;
            //HasAttacked =   InfoToCopy.HasAttacked;
            Flags = InfoToCopy.Flags;
            Tags = new HashSet<string>(InfoToCopy.Tags);
            AbilityActivated = new List<bool>(InfoToCopy.AbilityActivated);

        }
    }
    public class TileInfo
    {
        public int StandingUnitID = 0;
        public bool HasObjective = false;
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
                m_PlayerIntitiative.Add(m_PlayerTurnInitiativeGain);
                m_PlayerPoints.Add(0);
            }
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
            }
            
            m_Tiles[NewUnit.Position.Y][NewUnit.Position.X].StandingUnitID = NewID;

            return (NewID);
        }

        void p_RefreshUnit(UnitInfo UnitToRefresh)
        {
            UnitToRefresh.Flags &= ~(UnitFlags.HasAttacked);
            UnitToRefresh.Flags &= ~(UnitFlags.HasMoved);
            //UnitToRefresh.HasAttacked = false;
            //UnitToRefresh.HasMoved = false;
            //UnitToRefresh.IsActivated = false;
            for(int i = 0; i < UnitToRefresh.AbilityActivated.Count;i++)
            {
                UnitToRefresh.AbilityActivated[i] = false;
            }
        }

        bool p_MoveUnit(int UnitID, Coordinate TilePosition)
        {
            bool ReturnValue = true;
            UnitInfo AssociatedInfo = m_UnitInfos[UnitID];
            Coordinate PrevPos = new Coordinate(AssociatedInfo.Position.X, AssociatedInfo.Position.Y);
            m_Tiles[AssociatedInfo.Position.Y][AssociatedInfo.Position.X].StandingUnitID = 0;
            AssociatedInfo.Position = TilePosition;
            m_Tiles[TilePosition.Y][TilePosition.X].StandingUnitID = UnitID;
            if(m_EventHandler != null)
            {
                m_EventHandler.OnUnitMove(UnitID, PrevPos, TilePosition);
            }
            return (ReturnValue);
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
        IEnumerator p_ResolveEffect(List<Target> Targets,EffectSource Source,Effect EffectToResolve)
        {
            if (EffectToResolve is Effect_DealDamage)
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
                Coordinate TargetTile = null;
                if(Origins.Count != 1)
                {
                    throw new Exception("Need exactly 1 origin to resolve DamageArea");
                }
                if(Origins[0].Type == TargetType.Tile)
                {
                    TargetTile = ((Target_Tile)Origins[0]).TargetCoordinate;
                }
                if (Origins[0].Type == TargetType.Unit)
                {
                    TargetTile = m_UnitInfos[((Target_Unit)Origins[0]).UnitID].Position;
                }
                for(int i = -AreaDamageEffect.Range; i <= AreaDamageEffect.Range;i++)
                {
                    for(int j = -(AreaDamageEffect.Range - Math.Abs(i)); j <= AreaDamageEffect.Range- Math.Abs(i); j++)
                    {
                        Coordinate CurrentTile = TargetTile + new Coordinate(i, j);
                        if((CurrentTile.X < 0 || CurrentTile.X >= m_Tiles[0].Count) || (CurrentTile.Y < 0 || CurrentTile.Y >= m_Tiles[0].Count))
                        {
                            continue;
                        }
                        if(m_Tiles[CurrentTile.Y][CurrentTile.X].StandingUnitID != 0)
                        {
                            UnitInfo UnitToDamage = p_GetProcessedUnitInfo(m_Tiles[CurrentTile.Y][CurrentTile.X].StandingUnitID);
                            if(UnitToDamage.PlayerIndex == Source.PlayerIndex)
                            {
                                continue;
                            }
                            p_DealDamage(m_Tiles[CurrentTile.Y][CurrentTile.X].StandingUnitID, AreaDamageEffect.Damage);
                        }
                    }
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
        public bool p_VerifyTarget(Target TargetToVerify,out string OutError)
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
            if(Condition is TargetCondition_Type)
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
                Coordinate SourceCoordinate = null;
                if(RangeCondition.TargetIndex == -1)
                {
                    SourceCoordinate = m_UnitInfos[((EffectSource_Unit)Source).UnitID].Position;
                }
                else
                {
                    Target PreviousTarget = CurrentTargets[RangeCondition.TargetIndex];
                    if(PreviousTarget is Target_Unit)
                    {
                        SourceCoordinate = m_UnitInfos[((Target_Unit)PreviousTarget).UnitID].Position;
                    }
                    else if(PreviousTarget is Target_Tile)
                    {
                        SourceCoordinate = ((Target_Tile)PreviousTarget).TargetCoordinate;
                    }
                }
                Coordinate TargetCoordinate = null;
                if(TargetToVerify is Target_Unit)
                {
                    TargetCoordinate = m_UnitInfos[((Target_Unit)TargetToVerify).UnitID].Position;
                }
                else if(TargetToVerify is Target_Tile)
                {
                    TargetCoordinate = ((Target_Tile)TargetToVerify).TargetCoordinate;
                }
                else
                {
                    throw new Exception("Target condition only applies to targets of type Unit and Tile");
                }
                if(!(Coordinate.Distance(SourceCoordinate,TargetCoordinate) <= RangeCondition.Range))
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
            TileInfo UnitToRemoveTile = m_Tiles[UnitToRemoveInfo.Position.Y][UnitToRemoveInfo.Position.X];
            m_UnitInfos.Remove(UnitID);
            if(m_EventHandler != null)
            {
                m_EventHandler.OnUnitDestroyed(UnitID);
            }
            UnitToRemoveTile.StandingUnitID = 0;
        }
        void p_DealDamage(int UnitID, int Damage)
        {
            m_UnitInfos[UnitID].Stats.HP -= Damage;
        }

        int p_GetObjectiveControllIndex(Coordinate ObjectiveCoordinate)
        {
            List<int> ObjectiveScores = new List<int>();
            for(int i = 0; i <  m_PlayerCount;i++)
            {
                ObjectiveScores.Add(0);
            }
            for(int i = -1; i <= 1;i++)
            {
                for(int j = -1; j <= 1;j++)
                {
                    Coordinate CoordinateDiff = new Coordinate(i, j);
                    if(!(CoordinateDiff.X == 0 && CoordinateDiff.Y == 0))
                    {
                        Coordinate CurrentCoord = ObjectiveCoordinate + CoordinateDiff;
                        if (m_Tiles[CurrentCoord.Y][CurrentCoord.X].StandingUnitID != 0)
                        {
                            UnitInfo CurrentInfo = p_GetProcessedUnitInfo(m_Tiles[CurrentCoord.Y][CurrentCoord.X].StandingUnitID);
                            ObjectiveScores[CurrentInfo.PlayerIndex] += CurrentInfo.Stats.ObjectiveControll;
                        }
                    }
                }
            }
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
            for(int i = 0; i < m_PlayerCount; i++)
            {
                m_PlayerIntitiative[i] = 0;
                m_EmptyPassed[i] = false;
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
                m_RegisteredContinousAbilities.Remove(EffectToRemove);
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
            if (ChangeRound)
            {
                IEnumerator ChangeEnum = p_ChangeBattleround();
                while (ChangeEnum.MoveNext())
                {
                    yield return null;
                }
            }

            m_FirstRound -= 1;

            m_CurrentPlayerTurn = (m_CurrentPlayerTurn + 1) % m_PlayerCount;
            m_CurrentTurn += 1;
            m_CurrentPlayerPriority = m_CurrentPlayerTurn;


            if (m_EventHandler != null)
            {
                m_EventHandler.OnTurnChange(m_CurrentPlayerTurn, m_CurrentTurn);
            }

            for(int i = 0; i < m_PlayerCount;i++)
            {
                m_PlayerIntitiative[i] = Math.Min(Math.Min(m_PlayerIntitiative[i], m_PlayerInitiativeRetain) + m_PlayerTurnInitiativeGain, m_PlayerMaxInitiative);
                if(m_EventHandler != null)
                {
                    m_EventHandler.OnInitiativeChange(m_PlayerIntitiative[i], i);
                }
            }

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
                m_RegisteredContinousAbilities.Remove(EffectToRemove);
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
                Coordinate OldPosition = UnitToMove.Position;
                TileInfo UnitTile = m_Tiles[UnitToMove.Position.Y][UnitToMove.Position.X];
                UnitToMove.Position = MoveToExecute.NewPosition;
                UnitTile.StandingUnitID = 0;
                TileInfo NewTile = m_Tiles[MoveToExecute.NewPosition.Y][MoveToExecute.NewPosition.X];
                NewTile.StandingUnitID = MoveToExecute.UnitID;
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
            else if(ActionToExecute is AttackAction)
            {
                AttackAction AttackToExecute = (AttackAction)ActionToExecute;
                UnitInfo AttackerInfo = m_UnitInfos[AttackToExecute.AttackerID];
                UnitInfo DefenderInfo = m_UnitInfos[AttackToExecute.DefenderID];
                if(m_EventHandler != null)
                {
                    m_EventHandler.OnUnitAttack(AttackToExecute.AttackerID, AttackToExecute.DefenderID);
                }
                DefenderInfo.Stats.HP -= AttackerInfo.Stats.Damage;
                if ((AttackerInfo.Flags & UnitFlags.IsActivated) == 0)
                {
                    AttackerInfo.Flags |=UnitFlags.IsActivated;
                    m_PlayerIntitiative[AttackerInfo.PlayerIndex] -= AttackerInfo.Stats.ActivationCost;
                    if (m_EventHandler != null)
                    {
                        m_EventHandler.OnInitiativeChange(m_PlayerIntitiative[AttackerInfo.PlayerIndex], AttackerInfo.PlayerIndex);
                    }
                }
                AttackerInfo.Flags |= UnitFlags.HasAttacked;
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
        //public IEnumerable<Target> GetPossibleTargets(TargetInfo InfoToInspect)
        //{
        //    List<Target> ReturnValue = new List<Target>();
        //
        //    if(InfoToInspect is TargetInfo_List)
        //    {
        //        TargetInfo_List ListInfo = (TargetInfo_List)InfoToInspect;
        //        if(ListInfo.Targets.Count == 1)
        //        {
        //            IEnumerator<Target> TargetEnumerator = p_TotalTargetIterator();
        //            while (TargetEnumerator.MoveNext())
        //            {
        //                List<Target> TargetList = new List<Target>();
        //                TargetList.Add(TargetEnumerator.Current);
        //                if(p_VerifyTargets(InfoToInspect,new EffectSource_Empty(),TargetList))
        //                {
        //                    ReturnValue.Add(TargetEnumerator.Current);
        //                }
        //            }
        //        }
        //    }
        //    return (ReturnValue);
        //}
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
                if ((AssociatedUnit.Flags & UnitFlags.IsActivated) == 0 && AssociatedUnit.Stats.ActivationCost >= m_PlayerIntitiative[MoveToCheck.PlayerIndex])
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
                if (Coordinate.Distance(AttackerInfo.Position,DefenderInfo.Position) > AttackerInfo.Stats.Range)
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
                if ((AttackerInfo.Flags & UnitFlags.IsActivated) == 0 && AttackerInfo.Stats.ActivationCost >= m_PlayerIntitiative[AttackToCheck.PlayerIndex])
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
                if ((AssociatedUnit.Flags & UnitFlags.IsActivated) == 0 && AssociatedUnit.Stats.ActivationCost >= m_PlayerIntitiative[AssociatedUnit.PlayerIndex])
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

        void p_PossibleMoves(Coordinate CurrentCoord,int CurrentMovement,List<Coordinate> OutPossibleMoves,Dictionary<Coordinate,int> VisitedSpaces)
        {
            if(CurrentMovement == 0)
            {
                return;
            }
            foreach(Coordinate CurrentDiff in new Coordinate[] {new Coordinate(1,0),new Coordinate(0,1),new Coordinate(-1,0),new Coordinate(0,-1)})
            {
                Coordinate NewCoord = CurrentCoord + CurrentDiff;
                if(VisitedSpaces.ContainsKey(NewCoord))
                {
                    int PreviousMovement = VisitedSpaces[NewCoord];
                    if(PreviousMovement >= CurrentMovement)
                    {
                        continue;
                    }
                }
                if((NewCoord.Y >= m_Tiles.Count || NewCoord.Y < 0) || (NewCoord.X >= m_Tiles[0].Count || NewCoord.X < 0))
                {
                    continue;
                }
                if(m_Tiles[NewCoord.Y][NewCoord.X].StandingUnitID != 0 || m_Tiles[NewCoord.Y][NewCoord.X].HasObjective)
                {
                    continue;
                }
                VisitedSpaces[NewCoord] = CurrentMovement;
                OutPossibleMoves.Add(NewCoord);
                p_PossibleMoves(NewCoord, CurrentMovement - 1, OutPossibleMoves,VisitedSpaces);
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
            ReturnValue.Add(MovesToNormalize[MovesToNormalize.Count - 1]);
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
            if((UnitToMove.Flags & UnitFlags.HasMoved) != 0)
            {
                return (ReturnValue);
            }
            ReturnValue.Add(UnitToMove.Position);
            p_PossibleMoves(UnitToMove.Position, UnitToMove.Stats.Movement, ReturnValue, new Dictionary<Coordinate, int>());
            ReturnValue = p_NormalizeMoves(ReturnValue);
            return (ReturnValue);
        }

        void p_ApplyContinousEffect(UnitInfo InfoToModify,Effect Modifier)
        {
            if(Modifier is Effect_IncreaseDamage)
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
                    p_ApplyContinousEffect(ReturnValue, ContinousEffect.Value.EffectToApply);
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