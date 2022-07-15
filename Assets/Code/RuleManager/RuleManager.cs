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
    }

    //public class Effect_DestroyTargets : Effect
    //{
    //    TargetRetriever Targets;
    //}
    public class Effect_DealDamage : Effect
    {
        public TargetRetriever Targets;
        public int Damage = 0;
    }
    public class Effect_MoveUnit : Effect
    {
        public TargetRetriever UnitToMove;
        public TargetRetriever TargetPosition;
    }

    public class Effect_RegisterContinousAbility : Effect
    {
        public TargetRetriever OptionalAffectedTarget;
        public TargetCondition ContinousCondition = new TargetCondition_True();
        public Effect ContinousEffect;
    }
    public class Effect_RegisterTrigger : Effect
    {
        public bool IsOneshot = false;
        public bool IsEndOfTurn = false;
        public TargetRetriever OptionalAffectedTarget;
        public TriggerCondition Condition;
        public Effect TriggerEffect;
    }
    public class Effect_DamageArea : Effect
    {
        public TargetRetriever Origin;
        public int Range = 0;
        public int Damage = 0;
    }
    public class Effect_IncreaseDamage : Effect
    {
        public int DamageIncrease = 0;
        public Effect_IncreaseDamage(int NewDamage)
        {
            DamageIncrease = NewDamage;
        }
    }
    public enum RetrieverType
    {
        Index,
        Choose,
        Null
    }
    public class TargetRetriever
    {
        public RetrieverType Type = RetrieverType.Null;
        protected TargetRetriever(RetrieverType NewType)
        {
            Type = NewType;
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


    public class Ability
    {
        public readonly AbilityType Type = AbilityType.Null;
        protected Ability(AbilityType NewType)
        {
            Type = NewType;
        }
    }

    public class Ability_Activated : Ability
    {
        public TargetInfo ActivationTargets;
        public Effect ActivatedEffect;

        public Ability_Activated() : base(AbilityType.Activated)
        {

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


    public class Target
    {
        public readonly TargetType Type = TargetType.Null;

        public virtual bool Equals(Target OtherTarget)
        {
            return (false);
        }
        protected Target(TargetType TypeToUse)
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

    public class TargetRestriction
    {

        public virtual bool SatisfiesRestriction(Target TargetToInspect)
        {
            return (false);
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
    public class TargetCondition_Range : TargetCondition
    {
        public int Range = 0;
        public TargetCondition_Range(int RangeToUse)
        {
            Range = RangeToUse;
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
    public class TargetCondition_True : TargetCondition
    {

    }
    public class EffectSource
    {
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
        public EffectSource_Unit(int NewUnitID)
        {
            UnitID = NewUnitID;
        }
    }
    public class EffectSource_Player : EffectSource
    {
        public int PlayerIndex = 0;
    }

    public class TriggerCondition
    {

    }

    [Serializable]
    public class Coordinate : IEquatable<Coordinate>
    {
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
    }

    public class UnitStats
    {
        public int HP = 0;
        public int Movement = 0;
        public int Range = 0;
        public int Damage = 0;
        public int ActivationCost = 0;

        public UnitStats(UnitStats StatToCopy)
        {
            HP = StatToCopy.HP;
            Movement = StatToCopy.Movement;
            ActivationCost = StatToCopy.ActivationCost;
            Range = StatToCopy.Range;
            Damage = StatToCopy.Damage;
        }
        public UnitStats()
        {

        }
    }
    [Serializable]
    public class UnitInfo
    {
        //public EffectType Temp = EffectType.Activated;
        public int UnitID = 0;
        public int PlayerIndex = 0;
        public object OpaqueInteger = null;
        public Coordinate Position = new Coordinate();
        public List<Ability> Abilities = new List<Ability>();
        public UnitStats Stats = new UnitStats();

        public UnitInfo()
        {

        }
        public UnitInfo(UnitInfo InfoToCopy)
        {
            UnitID = InfoToCopy.UnitID;
            UnitID = InfoToCopy.PlayerIndex;
            OpaqueInteger = InfoToCopy.OpaqueInteger;
            Position = InfoToCopy.Position;
            Abilities = new List<Ability>(InfoToCopy.Abilities);
            Stats = new UnitStats(InfoToCopy.Stats);
        }
    }
    public class TileInfo
    {
        public int StandingUnitID = 0;

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
        void OnUnitCreate(UnitInfo NewUnit);
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

        private bool m_EndOfTurnPass = false;

        class RegisteredContinousEffect
        {
            public bool IsEndOfTurn = false;//hacky, but encapsulates important and very common functionality
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
        Dictionary<int, RegisteredContinousEffect> m_RegisteredContinousAbilities = new Dictionary<int, RegisteredContinousEffect>();
        Dictionary<int, RegisteredTrigger> m_RegisteredTriggeredAbilities = new Dictionary<int, RegisteredTrigger>();

        Stack<StackEntity> m_TheStack = new Stack<StackEntity>();
        IEnumerator m_CurrentResolution = null;

        private int m_CurrentID = 0;
        RuleEventHandler m_EventHandler;

        List<Target> m_ChoosenTargets = null;
        
        public void SetEventHandler(RuleEventHandler NewHandler)
        {
            m_EventHandler = NewHandler;
        }

        public int getPlayerPriority()
        {
            return m_CurrentPlayerPriority; 
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
            m_UnitInfos = new Dictionary<int, UnitInfo>();
        }
        public int RegisterUnit(UnitInfo NewUnit,int PlayerIndex)
        {
            NewUnit = new UnitInfo(NewUnit);
            int NewID = m_CurrentID + 1;
            m_CurrentID++;
            NewUnit.UnitID = NewID;
            NewUnit.PlayerIndex = PlayerIndex;
            m_UnitInfos[NewID] = NewUnit;
            m_Tiles[NewUnit.Position.Y][NewUnit.Position.X].StandingUnitID = NewID;

            return (NewID);
        }

        bool p_MoveUnit(int UnitID, Coordinate TilePosition)
        {
            bool ReturnValue = true;
            UnitInfo AssociatedInfo = m_UnitInfos[UnitID];
            m_Tiles[AssociatedInfo.Position.Y][AssociatedInfo.Position.X].StandingUnitID = 0;
            AssociatedInfo.Position = TilePosition;
            m_Tiles[TilePosition.Y][TilePosition.X].StandingUnitID = UnitID;
            return (ReturnValue);
        }

        IEnumerator p_RetrieveTargets(List<Target> Targets,TargetRetriever Retriever)
        {
            if(Retriever is TargetRetriever_Index)
            {
                TargetRetriever_Index IndexRetriever = (TargetRetriever_Index)Retriever;
                List<Target> NewTargets = new List<Target>();
                NewTargets.Add(Targets[IndexRetriever.Index]);
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
            yield break;
        }
        IEnumerator p_ResolveEffect(List<Target> Targets,EffectSource Source,Effect EffectToResolve)
        {
            if (EffectToResolve is Effect_DealDamage)
            {
                Effect_DealDamage DamageEffect = (Effect_DealDamage)EffectToResolve;
                IEnumerator RetrievedTargets = p_RetrieveTargets(Targets, DamageEffect.Targets);
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
                IEnumerator RetrievedMoveTargets = p_RetrieveTargets(Targets, MoveEffect.UnitToMove);
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


                IEnumerator RetrievedTileTargets = p_RetrieveTargets(Targets, MoveEffect.TargetPosition);
                RetrievedMoveTargets.MoveNext();
                while(RetrievedMoveTargets.Current == null)
                {
                    yield return null;
                    RetrievedMoveTargets.MoveNext();
                }
                List<Target> TileTargets = (List<Target>) RetrievedMoveTargets.Current;
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
            else if (EffectToResolve is Effect_RegisterTrigger)
            {
                Effect_RegisterTrigger TriggerEffect = (Effect_RegisterTrigger)EffectToResolve;
                IEnumerator RetrievedAffectedTargets = p_RetrieveTargets(Targets,TriggerEffect.OptionalAffectedTarget);
                RetrievedAffectedTargets.MoveNext();
                while (RetrievedAffectedTargets.Current == null)
                {
                    yield return null;
                    RetrievedAffectedTargets.MoveNext();
                }
                List<Target> AffectedTargets = (List<Target>)RetrievedAffectedTargets.Current;
                TriggerCondition ConditionToAdd = TriggerEffect.Condition;
                if(AffectedTargets.Count != 0)
                {

                }
                RegisteredTrigger TriggerToRegister = new RegisteredTrigger();
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
                IEnumerator RetrievedAffectedTargets = p_RetrieveTargets(Targets, ContinousEffect.OptionalAffectedTarget);
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
                ContinousEffectToRegister.IsEndOfTurn = true;
                p_RegisterContinousEffect(ContinousEffectToRegister);
            }
            else if(EffectToResolve is Effect_DamageArea)
            {
                Effect_DamageArea AreaDamageEffect = (Effect_DamageArea)EffectToResolve;
                IEnumerator RetrievedOrigin = p_RetrieveTargets(Targets, AreaDamageEffect.Origin);
                RetrievedOrigin.MoveNext();
                while (RetrievedOrigin.Current == null)
                {
                    yield return null;
                    RetrievedOrigin.MoveNext();
                }
                List<Target> Origins = (List<Target>)RetrievedOrigin.Current;
                if(Origins.Count != 1)
                {
                    throw new Exception("Need exactly 1 origin to resolve DamageArea");
                }
                if(Origins[0].Type != TargetType.Tile)
                {
                    throw new Exception("Can only target Tile for damage area");
                }
                Coordinate TargetTile = ((Target_Tile)Origins[0]).TargetCoordinate;
                for(int i = -AreaDamageEffect.Range; i <= AreaDamageEffect.Range;i++)
                {
                    for(int j = (AreaDamageEffect.Range - i); j < AreaDamageEffect.Range-i;j++)
                    {
                        Coordinate CurrentTile = TargetTile + new Coordinate(i, j);
                        if(m_Tiles[CurrentTile.Y][CurrentTile.X].StandingUnitID != 0)
                        {
                            p_DealDamage(m_Tiles[CurrentTile.Y][CurrentTile.X].StandingUnitID, AreaDamageEffect.Damage);
                        }
                    }
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

            //state based actions

            yield break;
        }
        public bool p_VerifyTarget(Target TargetToVerify)
        {
            bool ReturnValue = true;
            if(TargetToVerify is Target_Unit)
            {
                Target_Unit UnitTarget = (Target_Unit)TargetToVerify;
                if(!m_UnitInfos.ContainsKey(UnitTarget.UnitID))
                {
                    return (false);
                }
            }
            else if(TargetToVerify is Target_Tile)
            {
                Target_Tile TileTarget = (Target_Tile)TargetToVerify;
                if ((TileTarget.TargetCoordinate.Y >= m_Tiles.Count || TileTarget.TargetCoordinate.Y < 0) || (TileTarget.TargetCoordinate.X >= m_Tiles[0].Count || TileTarget.TargetCoordinate.X < 0))
                {
                    return (false);
                }
            }
            else if(TargetToVerify is Target_Player)
            {
                Target_Player PlayerTarget = (Target_Player)TargetToVerify;
                if(PlayerTarget.PlayerIndex < 0 ||  PlayerTarget.PlayerIndex >= m_PlayerCount)
                {
                    return (false);
                }
            }
            return (ReturnValue);
        }

        public bool p_VerifyTarget(TargetCondition Condition,EffectSource Source,Target TargetToVerify)
        {
            bool ReturnValue = true;
            if(!p_VerifyTarget(TargetToVerify))
            {
                return (false);
            }
            if(Condition is TargetCondition_Type)
            {
                TargetCondition_Type TypeCondition = (TargetCondition_Type)Condition;
                ReturnValue = TypeCondition.ValidType == TargetToVerify.Type;
            }
            else if(Condition is TargetCondition_Range)
            {
                TargetCondition_Range RangeCondition = (TargetCondition_Range)Condition;
                Coordinate SourceCoordinate = null;
                if(Source is EffectSource_Unit)
                {
                    SourceCoordinate = m_UnitInfos[((EffectSource_Unit)Source).UnitID].Position;
                }
                else
                {
                    throw new Exception("Source of range condition has to be Unit");
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

            }
            else if(Condition is TargetCondition_Target)
            {
                TargetCondition_Target TargetCondition = (TargetCondition_Target)Condition;
                return (TargetCondition.NeededTarget.Equals(TargetToVerify));
            }
            else if(Condition is TargetCondition_And)
            {
                TargetCondition_And AndCondition = (TargetCondition_And)Condition;
                foreach(TargetCondition AndClause in AndCondition.Conditions)
                {
                    if(!p_VerifyTarget(AndClause,Source,TargetToVerify))
                    {
                        ReturnValue = false;
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
            return (ReturnValue);
        }

        public bool p_VerifyTargets(TargetInfo Info,EffectSource Source,List<Target> TargetsToVerify)
        {
            bool ReturnValue = true;
            if(Info is TargetInfo_List)
            {
                TargetInfo_List ListToVerify = (TargetInfo_List)Info;
                if(ListToVerify.Targets.Count != ListToVerify.Targets.Count)
                {
                    ReturnValue = false;
                    return (ReturnValue);
                }
                for(int i = 0; i < ListToVerify.Targets.Count;i++)
                {
                    if(!p_VerifyTarget(ListToVerify.Targets[i],Source, TargetsToVerify[i]))
                    {
                        return (false);
                    }
                }
            }
            return (ReturnValue);
        }

        //Assumes valid UnitID
        void p_DestroyUnit(int UnitID)
        {
            UnitInfo UnitToRemoveInfo = m_UnitInfos[UnitID];
            TileInfo UnitToRemoveTile = m_Tiles[UnitToRemoveInfo.Position.Y][UnitToRemoveInfo.Position.X];
            m_UnitInfos.Remove(UnitID);
            UnitToRemoveTile.StandingUnitID = 0;
        }
        void p_DealDamage(int UnitID, int Damage)
        {
            m_UnitInfos[UnitID].Stats.HP -= Damage;
        }
        void p_ChangeTurn()
        {
            m_CurrentPlayerTurn = (m_CurrentPlayerTurn + 1) % m_PlayerCount;
            m_CurrentTurn += 1;
            m_CurrentPlayerPriority = m_CurrentPlayerTurn;
            if (m_EventHandler != null)
            {
                m_EventHandler.OnTurnChange(m_CurrentPlayerTurn, m_CurrentTurn);
            }
            List<int> ContinousEffectsToRemove = new List<int>();
            foreach(KeyValuePair<int,RegisteredContinousEffect> RegisteredEffect in m_RegisteredContinousAbilities)
            {
                if (RegisteredEffect.Value.IsEndOfTurn)
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
        }

        void p_PassPriority()
        {
            m_CurrentPlayerPriority += 1;
            m_CurrentPlayerPriority %= m_PlayerCount;

            if(m_CurrentPlayerTurn == m_CurrentPlayerPriority)
            {
                if(m_TheStack.Count > 0)
                {
                    IEnumerator ResolveResult = p_ResolveTopOfStack();
                    bool NotFinished = ResolveResult.MoveNext();
                    if (NotFinished)
                    {
                        m_CurrentResolution = ResolveResult;
                    }
                }
                else if(m_EndOfTurnPass)
                {
                    p_ChangeTurn();
                }
            }
        }

        //Modifiers
        public void ExecuteAction(Action ActionToExecute)
        {
            string Error;
            if(!ActionIsValid(ActionToExecute,out Error))
            {
                throw new ArgumentException("Invalid action to execute: "+Error);
            }
            bool EndOfTurnPass = false;
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
                DefenderInfo.Stats.HP = AttackerInfo.Stats.Damage;
                if(DefenderInfo.Stats.HP <= 0)
                {
                    if(m_EventHandler != null)
                    {
                        m_EventHandler.OnUnitDestroyed(DefenderInfo.Stats.HP);
                    }
                    p_DestroyUnit(DefenderInfo.UnitID);
                }
                p_PassPriority();
            }
            else if(ActionToExecute is PassAction)
            {
                EndOfTurnPass = true;
                p_PassPriority();
            }
            else if(ActionToExecute is EffectAction)
            {
                EffectAction EffectToExecute = (EffectAction)ActionToExecute;
                UnitInfo UnitWithEffect = m_UnitInfos[EffectToExecute.UnitID];
                Ability_Activated AbilityToActivate =(Ability_Activated) UnitWithEffect.Abilities[EffectToExecute.EffectIndex];
                StackEntity NewEntity = new StackEntity();
                NewEntity.EffectToResolve = AbilityToActivate.ActivatedEffect;
                NewEntity.Targets = EffectToExecute.Targets;
                NewEntity.Source = new EffectSource_Unit(EffectToExecute.UnitID);
                m_TheStack.Push(NewEntity);
                if(m_EventHandler != null)
                {
                    m_EventHandler.OnStackPush(NewEntity);
                }
                p_PassPriority();
            }
            else
            {
                throw new ArgumentException("Invalid Action type");
            }
            m_EndOfTurnPass = EndOfTurnPass;
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
        public IEnumerable<Target> GetPossibleTargets(TargetInfo InfoToInspect)
        {
            List<Target> ReturnValue = new List<Target>();

            if(InfoToInspect is TargetInfo_List)
            {
                TargetInfo_List ListInfo = (TargetInfo_List)InfoToInspect;
                if(ListInfo.Targets.Count == 1)
                {
                    IEnumerator<Target> TargetEnumerator = p_TotalTargetIterator();
                    while (TargetEnumerator.MoveNext())
                    {
                        List<Target> TargetList = new List<Target>();
                        TargetList.Add(TargetEnumerator.Current);
                        if(p_VerifyTargets(InfoToInspect,new EffectSource_Empty(),TargetList))
                        {
                            ReturnValue.Add(TargetEnumerator.Current);
                        }
                    }
                }
            }
            return (ReturnValue);
        }
        public int GetPlayerActionIndex()
        {
            int ReturnValue = 0;
            return (ReturnValue);
        }
        public bool ActionIsValid(Action ActionToCheck,out string OutInfo)
        {

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

            }
            else if(ActionToCheck is AttackAction)
            {
                AttackAction AttackToCheck = (AttackAction)ActionToCheck;
                if(m_UnitInfos.ContainsKey(AttackToCheck.AttackerID) == false || m_UnitInfos.ContainsKey(AttackToCheck.DefenderID) == false)
                {
                    ReturnValue = false;
                    ErrorString = "Invalid unit's for attack: Defender or attacker doesn't exist";
                    OutInfo = ErrorString;
                    return (ReturnValue);
                }
                UnitInfo DefenderInfo = m_UnitInfos[AttackToCheck.DefenderID];
                UnitInfo AttackerInfo = m_UnitInfos[AttackToCheck.AttackerID];
                if(Coordinate.Distance(AttackerInfo.Position,DefenderInfo.Position) > AttackerInfo.Stats.Range)
                {
                    ReturnValue = false;
                    ErrorString = "Defender out of range for attacker";
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
                UnitInfo AssociatedUnit = m_UnitInfos[EffectToCheck.UnitID];
                if(AssociatedUnit.PlayerIndex != EffectToCheck.PlayerIndex)
                {
                    ReturnValue = false;
                    ErrorString = "Can't activate the effect of opponents units";
                    OutInfo = ErrorString;
                    return (ReturnValue);
                }
                if(AssociatedUnit.Abilities.Count <= EffectToCheck.EffectIndex)
                {
                    ReturnValue = false;
                    ErrorString = "Invalid effect index for unit";
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
                Ability_Activated AbilityToActive =(Ability_Activated ) AssociatedUnit.Abilities[EffectToCheck.EffectIndex];
                if(!p_VerifyTargets(AbilityToActive.ActivationTargets,new EffectSource_Unit(AssociatedUnit.UnitID),EffectToCheck.Targets))
                {
                    ReturnValue = false;
                    ErrorString = "Invalid targets for ability";
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
        public List<Coordinate> PossibleMoves(int UnitID)
        {
            List<Coordinate> ReturnValue = new List<Coordinate>();

            for(int i = 0; i < m_Tiles.Count; i++)
            {
                for(int j = 0; j < m_Tiles[0].Count; j++)
                {
                    ReturnValue.Add(new Coordinate(j, i));
                }
            }


            return (ReturnValue);
        }

        void p_ApplyContinousEffect(UnitInfo InfoToModify,Effect Modifier)
        {
            if(Modifier is Effect_IncreaseDamage)
            {
                InfoToModify.Stats.Damage += ((Effect_IncreaseDamage)Modifier).DamageIncrease;
            }
            else
            {
                throw new Exception("Invalid continous modifier");
            }
        }

        private UnitInfo p_GetProcessedUnitInfo(int ID)
        {
            UnitInfo ReturnValue = new UnitInfo(m_UnitInfos[ID]);
            foreach(KeyValuePair<int,RegisteredContinousEffect> ContinousEffect in m_RegisteredContinousAbilities)
            {
                if (p_VerifyTarget(ContinousEffect.Value.AffectedEntities, ContinousEffect.Value.AbilitySource, new Target_Unit(ID)))
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
            return new TileInfo(m_Tiles[Y][X]);
        }
    }
}