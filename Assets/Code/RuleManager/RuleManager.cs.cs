

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
        Move,
        UnitEffect,
        Stratagem
    }
    class Action
    {
        public ActionType Type;
    }

    class UnitID
    {

    }
    enum EffectType
    {
        Continous,
        Triggered,
        Actived
    }
    class UnitInfo
    {
        public EffectType Temp = EffectType.Actived;
    }

    class RuleManager
    {


        public UnitInfo GetUnitInfo(UnitID ID)
        {
            return (new UnitInfo());
        }
    }
}