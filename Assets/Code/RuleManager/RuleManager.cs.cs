using System.Collections;
using System.Collections.Generic;
using System;

namespace RuleManager
{
    enum TargetType
    {
        Player,
        Unit,
        Tile,
    }
    class TargetInfo
    {
          
    }
    enum ActionType
    {
        Null,
        Move,
        UnitEffect,
        Stratagem,
        Pass,
    }
    class Action
    {
        public ActionType Type = ActionType.Null;
        public int PlayerIndex = -1;
    }

    class MoveAction : Action
    {
        public int UnitID = 0;
        Coordinate NewPosition;
    }

    enum EffectType
    {
        Continous,
        Triggered,
        Activated,
    }
    class Effect
    {
        public EffectType Type;
    }

    class UnitStats
    {
        public int HP = 0;
        public int Movement = 0;
        public int ActivationCost = 0;
    }
    class Coordinate
    {
        public int X = -1;
        public int Y = -1;
    }
    class UnitInfo
    {
        //public EffectType Temp = EffectType.Activated;
        public int UnitID = 0;
        public int PlayerIndex = 0;
        public Coordinate Position;
        public List<Effect> Effects;
        public UnitStats Stats;
    }
    class TileInfo
    {
        public int StandingUnitID = 0;
    }
    class RuleManager
    {
        //retunrs UnitInfo with invalid UnitID on error, that is to say UnitID = 0
        private Dictionary<int,UnitInfo> m_UnitInfos;
        private List<List<TileInfo>> m_Tiles;

        bool ActionIsValid(Action ActionToExecute)
        {
            bool ReturnValue = true;

            return (ReturnValue);
        }

        //Modifiers
        void ExecuteAction(Action ActionToExecute)
        {
            if(ActionToExecute is  MoveAction)
            {
                MoveAction MoveToExecute = (MoveAction)ActionToExecute;
                
            }
        }

        //Observers
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
            return m_Tiles[Y][X];
        }
    }
}