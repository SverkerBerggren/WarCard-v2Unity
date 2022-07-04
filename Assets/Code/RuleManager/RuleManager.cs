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
    }
    public class TargetInfo
    {
          
    }
    [Serializable]
    public struct Coordinate
    {
        public int X;
        public int Y;
        public Coordinate(int NewX, int NewY)
        {
            X = NewX;
            Y = NewY;
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
       public PassAction() : base(ActionType.Pass) { }
    }

    public enum EffectType
    {
        Continous,
        Triggered,
        Activated,
    }
    public class Effect
    {
        public EffectType Type;
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
        public List<Effect> Effects = new List<Effect>();
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
            Effects = InfoToCopy.Effects;
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

    interface RuleEventHandler
    {
        void OnUnitMove(int UnitID, Coordinate PreviousPosition, Coordinate NewPosition);
        void OnUnitAttack(int AttackerID, int DefenderID);
        void OnUnitDestroyed(int UnitID);
        void OnTurnChange(int CurrentPlayerTurnIndex,int CurrentTurnCount);
        void OnUnitCreate(UnitInfo NewUnit);
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

        private int m_CurrentID = 0;
        RuleEventHandler m_EventHandler;
        void SetEventHandler(RuleEventHandler NewHandler)
        {
            m_EventHandler = NewHandler;
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


        //Assumes valid UnitID
        void p_DestroyUnit(int UnitID)
        {
            UnitInfo UnitToRemoveInfo = m_UnitInfos[UnitID];
            TileInfo UnitToRemoveTile = m_Tiles[UnitToRemoveInfo.Position.Y][UnitToRemoveInfo.Position.X];
            m_UnitInfos.Remove(UnitID);
            UnitToRemoveTile.StandingUnitID = 0;
        }

        //Modifiers
        public void ExecuteAction(Action ActionToExecute)
        {
            string Error;
            if(!ActionIsValid(ActionToExecute,out Error))
            {
                throw new ArgumentException("Invalid action to execute: "+Error);
            }
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
            }
            else if(ActionToExecute is PassAction)
            {
                m_CurrentPlayerPriority += 1;
                m_CurrentPlayerPriority %= m_PlayerCount;
                if(m_CurrentPlayerPriority == 0)
                {
                    m_CurrentPlayerTurn = (m_CurrentPlayerTurn+1)%m_PlayerCount;
                    m_CurrentTurn += 1;
                    if(m_EventHandler != null)
                    {
                        m_EventHandler.OnTurnChange(m_CurrentPlayerTurn, m_CurrentTurn);
                    }
                }
            }
            else
            {
                throw new ArgumentException("Invalid Action type");
            }
        }

        //Observers
        public int GetPlayerActionIndex()
        {
            int ReturnValue = 0;
            return (ReturnValue);
        }
        public bool ActionIsValid(Action ActionToCheck,out string OutInfo)
        {
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
        public UnitInfo GetUnitInfo(int ID)
        {
            UnitInfo ReturnValue = null;
            if(m_UnitInfos.ContainsKey(ID))
            {
                ReturnValue = m_UnitInfos[ID];
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