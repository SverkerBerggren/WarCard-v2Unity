using System.Collections.Generic;
using System;

public class UIUnitscriptDefs
{
    public static Dictionary<string, UnitScript.Builtin_FuncInfo> GetUnitScriptFuncs()
    {
        Dictionary<string, UnitScript.Builtin_FuncInfo> ReturnValue = new();

        UnitScript.Builtin_FuncInfo MoveCamera = new UnitScript.Builtin_FuncInfo();
        MoveCamera.ArgTypes = new List<HashSet<Type>> { new HashSet<Type> { typeof(RuleManager.UnitIdentifier),typeof(RuleManager.Coordinate) }};
        MoveCamera.ResultType = typeof(void);
        MoveCamera.ValidContexts = UnitScript.EvalContext.Resolve;
        ReturnValue["MoveCamera"] = MoveCamera;

        UnitScript.Builtin_FuncInfo ColorTiles = new UnitScript.Builtin_FuncInfo();
        ColorTiles.ArgTypes = new List<HashSet<Type>> { new HashSet<Type> { typeof(List<RuleManager.Coordinate>)} };
        ColorTiles.ResultType = typeof(int);
        ColorTiles.ValidContexts = UnitScript.EvalContext.Resolve;
        ReturnValue["AddTileColoring"] = ColorTiles;

        UnitScript.Builtin_FuncInfo RemoveTiles = new UnitScript.Builtin_FuncInfo();
        RemoveTiles.ArgTypes = new List<HashSet<Type>> { new HashSet<Type> { typeof(int) } };
        RemoveTiles.ResultType = typeof(void);
        RemoveTiles.ValidContexts = UnitScript.EvalContext.Resolve;
        ReturnValue["RemoveTileColoring"] = RemoveTiles;

        return ReturnValue;
    }

}