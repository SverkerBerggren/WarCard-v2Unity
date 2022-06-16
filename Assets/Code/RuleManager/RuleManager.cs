using System.Collections;
using System.Collections.Generic;
using System;

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
    public enum ActionType
    {
        Null,
        Move,
        Attack,
        UnitEffect,
        Stratagem,
        Pass,
    }
    public class Action
    {
        public int PlayerIndex = -1;
    }

    public class MoveAction : Action
    {
        public int UnitID = 0;
        public Coordinate NewPosition;
    }
    public class AttackAction : Action
    {
        public int AttackerID = 0;
        public int DefenderID = 0;
    }
    public class PassAction : Action
    {

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
        }
    }
    public struct Coordinate
    {
        public int X;
        public int Y;
        public Coordinate(int NewX,int NewY)
        {
            X = NewX;
            Y = NewY;
        }
        public static int Distance(Coordinate LeftCoordinate,Coordinate RightCoordinate)
        {
            return (Math.Abs(LeftCoordinate.X - RightCoordinate.X) + Math.Abs(LeftCoordinate.Y - RightCoordinate.Y));
        }
    }
    public class UnitInfo
    {
        //public EffectType Temp = EffectType.Activated;
        public int UnitID = 0;
        public int PlayerIndex = 0;
        public int OpaqueInteger = 0;
        public Coordinate Position;
        public List<Effect> Effects;
        public UnitStats Stats;
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
    }

    class RuleManager
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
            int NewID = m_CurrentID + 1;
            m_CurrentID++;
            NewUnit.UnitID = NewID;
            NewUnit.PlayerIndex = PlayerIndex;
            m_UnitInfos[NewID] = NewUnit;
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
        void ExecuteAction(Action ActionToExecute)
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
        bool ActionIsValid(Action ActionToCheck,out string OutInfo)
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