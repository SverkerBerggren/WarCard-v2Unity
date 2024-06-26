using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

namespace UnitScript
{
    public class Diagnostic
    {
        public MBCC.TokenPosition Position = new MBCC.TokenPosition();
        public int Length = 0;
        public string Message;
        public Diagnostic() { }
        public Diagnostic(MBCC.TokenPosition Pos,int Length,string Message)
        {
            Position = Pos;
            this.Length = Length;
            this.Message = Message;
        }
        public Diagnostic(Parser.Token Region,string Message)
        {
            Position = Region.Position;
            Length = Region.Value.Length;
            this.Message = Message;
        }
    }
    public class AbilityInformation
    {
        public int Index = 0;
        public RuleManager.Ability Ability;
        public ResourceManager.Animation Icon = null;
    }
    public class Expression
    {

    }
    public class Expression_FuncCall : Expression
    {
        public string FuncName;
        public List<Expression> Args = new List<Expression>();
        public Dictionary<string,Expression> KeyArgs = new Dictionary<string, Expression>();
    }
    public class Expression_List : Expression
    {
        public List<Expression> Contents = new List<Expression>();
    }
    public class Expression_Literal : Expression
    {
        public object Value;

        public Expression_Literal()
        {

        }
        public Expression_Literal(object Val)
        {
            Value = Val;
        }

    }
    public class Expression_Assignment : Expression
    {
        public string VariableName;
        public Expression VariableValue;
    }
    public class Expression_Variable : Expression
    {
        public string VarName;

        public Expression_Variable()
        {

        }
        public Expression_Variable(string Name)
        {
            VarName = Name;
        }
    }
    public class ContinousAbility
    {
        public EvaluationEnvironment Envir;
        public int Index = 0;
        public RuleManager.Ability_Continous Ability;
    }
    public class TriggeredAbility
    {
        public EvaluationEnvironment Envir;
        public int Index = 0;
        public RuleManager.Ability_Triggered Ability;
    }
    public class Expression_ContinousAbility : Expression
    {
        public int Index = 0;
        public RuleManager.Ability_Continous Ability;
    }

    public class Expression_TriggeredAbility : Expression
    {
        public int Index = 0;
        public RuleManager.Ability_Triggered Ability;
    }
    public enum ModificationType
    {
        Set,
        Add
    }
    public enum StatType
    {
        Unit,
        UnitField,
        Visual,
        Stat,
        Null
    }
    public class Expression_StatModification : Expression
    {
        public StatReference Stat;
        public ModificationType ModType;
        public Expression Value;
    }
    public class StatReference
    {
        public Expression_Variable UnitVar;
        public StatType statType;
        public string FieldName;
        public Type ReferenceType;
    }
    public class Expression_Eq : Expression
    {
        public Expression Lhs;
        public Expression Rhs;
    }

    public class BuiltinFuncArgs
    {
        public EvaluationEnvironment Envir;
        public UnitConverter Handler = null;
        public List<object> Arguments = new List<object>();
        public Dictionary<string,object> KeyArguments = new Dictionary<string, object>();

        public RuleManager.EffectSource GetSource()
        {
            return Envir.GetVar("SOURCE") as RuleManager.EffectSource;
        }
    }
    public class Builtin_FuncInfo
    {
        public Type ResultType;
        public EvalContext ValidContexts = EvalContext.None;
        public List<HashSet<Type>> ArgTypes = new List<HashSet<Type>>();//required arguments
        public Dictionary<string, Type> KeyArgTypes =  new Dictionary<string, Type>();//keyword required types
        public Func<BuiltinFuncArgs,object> Callable;
    }

    public enum EvalContext
    {
        None = 0,
        Compile  = 1<<1,
        Predicate  = 1<<2,
        Resolve  = 1<<3,
        Continous = 1<<4,
    }

    public class EvaluationEnvironment : MBJson.JSONSerializable,MBJson.JSONDeserializeable,IComparable<EvaluationEnvironment>
    {
        class ContentPair
        {
            public object Value;
            public Type ValueType;
            public ContentPair(object NewVal,Type NewValType)
            {
                Value = NewVal;
                ValueType = NewValType;
            }
        }
        Dictionary<string, ContentPair> m_Contents = new Dictionary<string, ContentPair>();
        HashSet<string> m_ShadowRemoved = new HashSet<string>();
        EvaluationEnvironment m_Parent = null;

        public int CompareTo(EvaluationEnvironment Envir)
        {
            if(m_Parent == null && Envir.m_Parent != null)
            {
                return -1;
            }
            else if(Envir.m_Parent == null && m_Parent != null)
            {
                return 1;
            }
            else
            {
                return Metadata.ID.CompareTo(Envir.Metadata.ID);
            }
        }

        public bool HasParent()
        {
            return m_Parent != null;
        }
        public EvaluationEnvironment GetParent()
        {
            return m_Parent;
        }

        public MBJson.JSONObject Serialize()
        {
            MBJson.JSONObject ReturnValue = new(new Dictionary<string, MBJson.JSONObject>());
            ReturnValue["Metadata"] = MBJson.JSONObject.SerializeObject(Metadata);
            return ReturnValue;
        }
        public object Deserialize(MBJson.JSONObject ObjectToParse)
        {
            Metadata = MBJson.JSONObject.DeserializeObject<EnvirMetadata>(ObjectToParse["Metadata"]);
            return this;
        }

        public MBJson.JSONObject SerializeEnvir()
        {
            MBJson.JSONObject ReturnValue = new(new Dictionary<string,MBJson.JSONObject>());
            ReturnValue["Metadata"] = MBJson.JSONObject.SerializeObject(Metadata);
            if(m_Parent != null)
            {
                ReturnValue["Parent"] = MBJson.JSONObject.SerializeObject(m_Parent.Metadata);
            }
            else
            {
                ReturnValue["Parent"] = new();
            }
            Dictionary<string, MBJson.JSONObject> Contents = new();
            if (Metadata.ResourceID == -1)
            {
                foreach (var Pair in m_Contents)
                {
                    MBJson.JSONObject NewValue = new(new Dictionary<string, MBJson.JSONObject>());
                    NewValue["Type"] = new(Pair.Value.Value.GetType().AssemblyQualifiedName);
                    NewValue["Value"] = MBJson.JSONObject.SerializeObject(Pair.Value.Value);
                    Contents[Pair.Key] = NewValue;
                }
            }
            ReturnValue["Contents"] = new(Contents);
            return ReturnValue;
        }

        public void DeserializeEnvir(MBJson.JSONObject ObjectToParse)
        {
            Metadata = MBJson.JSONObject.DeserializeObject<EnvirMetadata>(ObjectToParse["Metadata"]);
            if(ObjectToParse["Parent"].GetJSONType() != MBJson.JSONType.Null)
            {
                m_Parent = new();
                m_Parent.Metadata = MBJson.JSONObject.DeserializeObject<EnvirMetadata>(ObjectToParse["Parent"]);
            }
            if(Metadata.ResourceID == -1)
            {
                var ValueMap = ObjectToParse["Contents"];
                foreach(var Pair in ValueMap.GetAggregateData())
                {
                    Type StoredType = Type.GetType(Pair.Value["Type"].GetStringData());
                    ContentPair NewPair = new( typeof(MBJson.JSONObject).GetMethod("DeserializeObject").MakeGenericMethod(StoredType).Invoke(null,new object[] { Pair.Value["Value"] }),StoredType);
                    m_Contents[Pair.Key] = NewPair;
                }
            }
        }

        public EnvirMetadata Metadata = new();
        public class EnvirMetadata
        {
            public int ID = 0;
            public int ResourceID = -1;
        }

        public bool HasVar(string VariableToCheck)
        {
            if(m_ShadowRemoved.Contains(VariableToCheck))
            {
                return false;   
            }
            bool  ReturnValue = m_Contents.ContainsKey(VariableToCheck);
            if(!ReturnValue && m_Parent != null)
            {
                ReturnValue = m_Parent.HasVar(VariableToCheck);   
            }
            return ReturnValue;
        }
        public void SetParent(EvaluationEnvironment NewParent)
        {
            m_Parent = NewParent;
        }

        public Type GetVarType(string VariableToGet)
        {
            if (m_Contents.ContainsKey(VariableToGet))
            {
                return m_Contents[VariableToGet].ValueType;
            }
            if (m_Parent != null)
            {
                return m_Parent.GetVarType(VariableToGet);
            }
            throw new Exception("Couldn't find variable \"" + VariableToGet + "\" in environment");
        }
        public object GetVar(string VariableToGet)
        {
            if(m_Contents.ContainsKey(VariableToGet))
            {
                return m_Contents[VariableToGet].Value;
            }
            if(m_Parent != null)
            {
                return m_Parent.GetVar(VariableToGet);
            }
            throw new Exception("Couldn't find variable \""+ VariableToGet+ "\" in environment");
        }
        public void AddVar(string Variable,object Value)
        {
            m_Contents[Variable] = new ContentPair(Value,Value.GetType());
        }
        public void AddVarType(string Variable, Type VarType)
        {
            m_Contents[Variable] = new ContentPair(null, VarType);
        }
        public void ShadowRemove(string Variable)
        {
            m_ShadowRemoved.Add(Variable);   
        }
    }
    public class UnitConverter
    {
        Dictionary<string, Builtin_FuncInfo> m_BuiltinFuncs = new Dictionary<string, Builtin_FuncInfo>();
        HashSet<string> m_ConstexpBuiltins = new HashSet<string>();
        Dictionary<int,ResourceManager.UnitResource> m_LoadedUnits = new Dictionary<int, ResourceManager.UnitResource>();
        Dictionary<string,int> m_StringToUnit = new Dictionary<string, int>();


        RuleManager.RuleManager m_AssociatedRuleManager = null;
        void SetRuleManager(RuleManager.RuleManager RuleManager)
        {
            m_AssociatedRuleManager = RuleManager;   
        }
        int m_CurrentUnitID = 0;

        delegate Expression SpecialFunc(TypeCheckingContext Context,EvalContext CurrentContext, EvaluationEnvironment Envir ,Parser.Expression_FuncCall ParsedExpression,out Type ResultType);
        Dictionary<string,SpecialFunc> m_SpecialFuncs;
        StatReference p_GetStatType(TypeCheckingContext Context,EvalContext CurrentContext,EvaluationEnvironment Envir ,Parser.Expression_StatReference Stat)
        {
            StatReference Result = new StatReference();
            Type ReturnValue  = typeof(void);
            StatType CurrentType = StatType.Null;
            ResourceManager.UnitResource AssociatedUnit = null;
            if(Envir.HasVar(Stat.Tokens[0].Value))
            {
                object Value = Envir.GetVar(Stat.Tokens[0].Value);
                if(Value is RuleManager.UnitIdentifier)
                {
                    CurrentType = StatType.Unit;
                    if(m_LoadedUnits.ContainsKey( ((RuleManager.UnitIdentifier)Value).ID))
                    {
                        AssociatedUnit = m_LoadedUnits[ ((RuleManager.UnitIdentifier)Value).ID];
                    }
                }
                else 
                {
                    Context.OutDiagnostics.Add(new Diagnostic(Stat.Tokens[0],"Cannot create reference to field with type "+Value.GetType().Name));
                }
            }
            else
            {
                Context.OutDiagnostics.Add(new Diagnostic(Stat.Tokens[0],"Invalid object, \""+Stat.Tokens[0].Value+"\" not found"));
            }
            if(CurrentType != StatType.Null)
            {
                for(int i = 1; i < Stat.Tokens.Count;i++)
                {
                    if(CurrentType == StatType.Unit)
                    {
                        if(Stat.Tokens[i].Value == "Visuals")
                        {
                            CurrentType = StatType.Visual;
                        }
                        else if(Stat.Tokens[i].Value == "Stats")
                        {
                            CurrentType = StatType.Stat;
                        }
                        else if(AssociatedUnit != null && AssociatedUnit.GameInfo.Envir.HasVar(Stat.Tokens[i].Value))
                        {
                            ReturnValue = AssociatedUnit.GameInfo.Envir.GetVar(Stat.Tokens[i].Value).GetType();
                            CurrentType = StatType.UnitField;
                        }
                        else
                        {
                            Context.OutDiagnostics.Add(new Diagnostic(Stat.Tokens[i],"Unit has no field named \""+Stat.Tokens[i].Value+"\""));
                        }
                    }
                    else if(CurrentType == StatType.Visual)
                    {
                        HashSet<string> UnitStats = new HashSet<string>{"Up","Down","Attack"};
                        if(UnitStats.Contains(Stat.Tokens[i].Value))
                        {
                            CurrentType = StatType.Visual;
                        }
                        else
                        {
                            Context.OutDiagnostics.Add(new Diagnostic(Stat.Tokens[i],"UnitVisuals has no field named \""+Stat.Tokens[i].Value+"\""));
                        }
                    }
                    else if(CurrentType == StatType.Stat)
                    {
                        HashSet<string> UnitStats = new HashSet<string>{"Movement","HP","Range","Damage"};
                        if(UnitStats.Contains(Stat.Tokens[i].Value))
                        {
                            ReturnValue = typeof(int);
                        }
                        else
                        {
                            Context.OutDiagnostics.Add(new Diagnostic(Stat.Tokens[i],"UnitStas has no field named \""+Stat.Tokens[i].Value+"\""));
                        }
                    }
                    else
                    {
                        Context.OutDiagnostics.Add(new Diagnostic(Stat.Tokens[i],"object of type "+CurrentType.ToString()+" has no field with name \""+Stat.Tokens[i].Value));
                    }
                }
                if(CurrentType == StatType.Unit)
                {
                    Context.OutDiagnostics.Add(new Diagnostic(Stat.Tokens[Stat.Tokens.Count-1],"Cannot create reference to object of type "+CurrentType.ToString()));
                }
            }
            Result.FieldName = Stat.Tokens[Stat.Tokens.Count-1].Value;
            Result.statType = CurrentType;
            Result.ReferenceType = ReturnValue;
            Result.UnitVar = new Expression_Variable();
            Result.UnitVar.VarName = Stat.Tokens[0].Value;
            return Result;
        }

    
        
        Expression_StatModification p_ParseStatModification(TypeCheckingContext Context,EvalContext CurrentContext, EvaluationEnvironment Envir ,Parser.Expression_FuncCall ParsedExpression,out Type ResultType)
        {
            Type OutType = typeof(void);
            Expression_StatModification ReturnValue = new Expression_StatModification();
            if(ParsedExpression.Args.Count != 2)
            {
                Context.OutDiagnostics.Add(new Diagnostic(ParsedExpression.FuncName,"AddStat requires exactly 2 arguments"));
                ResultType = OutType;
                return ReturnValue;
            }
            Type FirstArgType;
            Type SecondArgType;
            Expression FirstArg = ConvertExpression(Context,CurrentContext,Envir, ParsedExpression.Args[0],out FirstArgType);
            Expression SecondArg = ConvertExpression(Context,CurrentContext,Envir, ParsedExpression.Args[1],out SecondArgType);
            if( !(FirstArg is Expression_Literal) && !(((Expression_Literal)FirstArg).Value is StatReference) )
            {
                Context.OutDiagnostics.Add(new Diagnostic(ParsedExpression.FuncName,"AddStat requires that the first argument is a field reference"));
            }
            else
            {
                ReturnValue.Stat  =(StatReference)((Expression_Literal)FirstArg).Value;
                ReturnValue.Value = SecondArg;
                if(ReturnValue.Stat.ReferenceType != SecondArgType)
                {
                    Context.OutDiagnostics.Add(new Diagnostic(ParsedExpression.FuncName,"Invalid value for reference, "+ReturnValue.Stat.ReferenceType.Name+" expected"));
                }
            }
            ResultType = OutType;
            return ReturnValue;
        }
        public Expression ParseAddStat(TypeCheckingContext Context,EvalContext CurrentContext, EvaluationEnvironment Envir ,Parser.Expression_FuncCall ParsedExpression,out Type ResultType)
        {
            Expression_StatModification Modification = p_ParseStatModification(Context,CurrentContext, Envir,ParsedExpression,out ResultType);
            Modification.ModType = ModificationType.Add;
            return Modification;
        }
        public Expression ParseSetStat(TypeCheckingContext Context,EvalContext CurrentContext, EvaluationEnvironment Envir ,Parser.Expression_FuncCall ParsedExpression,out Type ResultType)
        {
            Expression_StatModification Modification = p_ParseStatModification(Context,CurrentContext,Envir,ParsedExpression,out ResultType);
            Modification.ModType = ModificationType.Set;
            return Modification;
        }
        public Expression ParseEq(TypeCheckingContext Context,EvalContext CurrentContext, EvaluationEnvironment Envir ,Parser.Expression_FuncCall ParsedExpression,out Type ResultType)
        {
            ResultType = typeof(bool);
            Expression_Eq ReturnValue = new Expression_Eq();
            if(ParsedExpression.Args.Count != 2)
            {
                 Context.OutDiagnostics.Add(new Diagnostic(ParsedExpression.FuncName,"Eq requires exactly 2 arguments"));
                return ReturnValue;
            }
            Type FirstArgType;
            Type SecondArgType;
            Expression FirstArg = ConvertExpression(Context,CurrentContext,Envir, ParsedExpression.Args[0],out FirstArgType);
            Expression SecondArg = ConvertExpression(Context,CurrentContext,Envir, ParsedExpression.Args[1],out SecondArgType);
            ReturnValue.Lhs  = FirstArg;
            ReturnValue.Rhs = SecondArg;
            if(FirstArgType != SecondArgType)
            {
                Context.OutDiagnostics.Add(new Diagnostic(ParsedExpression.FuncName,"Both arguments in Eq must be of the same type"));
                return ReturnValue;
            }
            if(!(FirstArgType == typeof(string) || FirstArgType == typeof(int) || FirstArgType != typeof(bool) || FirstArgType != typeof(RuleManager.UnitIdentifier)))
            {
                Context.OutDiagnostics.Add(new Diagnostic(ParsedExpression.FuncName,"Cannot check "+FirstArgType.Name+" for equality"));
            }
            return ReturnValue;
        }
        
        string m_CurrentPath = "";
        public string GetCurrentPath()
        {
            return m_CurrentPath;
        }
        public void SetCurrentPath(string NewPath)
        {
            m_CurrentPath = NewPath;
        }
        
        static object p_Not(BuiltinFuncArgs Args)
        {
            bool ReturnValue = false;
            if(Args.Arguments[0] is bool)
            {
                ReturnValue = !(bool)Args.Arguments[0];
            }
            return ReturnValue;
        }
        static object p_And(BuiltinFuncArgs Args)
        {
            bool ReturnValue = true;
            foreach(object Arg in Args.Arguments)
            {
                if( !(Arg is bool) || !(bool)Arg )
                {
                    return false;
                }
            }
            return ReturnValue;
        }
        static object p_Or(BuiltinFuncArgs Args)
        {
            bool ReturnValue = false;
            foreach(object Arg in Args.Arguments)
            {
                if( Arg is bool && (bool)Arg )
                {
                    return true;
                }
            }
            return ReturnValue;
        }
        static object p_Le(BuiltinFuncArgs Args)
        {
            return (int)Args.Arguments[0] < (int)Args.Arguments[1];
        }
        static object p_Leq(BuiltinFuncArgs Args)
        {
            return (int)Args.Arguments[0] <= (int)Args.Arguments[1];
        }
        static object p_Ge(BuiltinFuncArgs Args)
        {
            return (int)Args.Arguments[0] > (int)Args.Arguments[1];
        }
        static object p_Geq(BuiltinFuncArgs Args)
        {
            return (int)Args.Arguments[0] >= (int)Args.Arguments[1];
        }
        public UnitConverter()
        {
            m_SpecialFuncs = new Dictionary<string, SpecialFunc>();
            m_SpecialFuncs.Add("Eq",ParseEq);
            m_SpecialFuncs.Add("AddStat",ParseAddStat);
            m_SpecialFuncs.Add("SetStat",ParseSetStat);


            Builtin_FuncInfo And = new Builtin_FuncInfo();
            And.ArgTypes = new List<HashSet<Type>>{ new HashSet<Type>{typeof(bool)},new HashSet<Type>{typeof(bool)}};
            And.ResultType = typeof(bool);
            And.ValidContexts = EvalContext.Compile | EvalContext.Predicate |EvalContext.Resolve;
            And.Callable = p_And;
            m_BuiltinFuncs["&&"] = And;

            Builtin_FuncInfo Or = new Builtin_FuncInfo();
            Or.ArgTypes = new List<HashSet<Type>>{  new HashSet<Type>{typeof(bool)}, new HashSet<Type>{typeof(bool)}};
            Or.ResultType = typeof(bool);
            Or.ValidContexts = EvalContext.Compile | EvalContext.Predicate |EvalContext.Resolve;
            Or.Callable = p_Or;
            m_BuiltinFuncs["||"] = Or;

            Builtin_FuncInfo Not = new Builtin_FuncInfo();
            Not.ArgTypes = new List<HashSet<Type>>{  new HashSet<Type>{typeof(bool)}};
            Not.ResultType = typeof(bool);
            Not.ValidContexts = EvalContext.Compile | EvalContext.Predicate |EvalContext.Resolve;
            Not.Callable = p_Not;
            m_BuiltinFuncs["!"] = Not;

            Builtin_FuncInfo Le = new Builtin_FuncInfo();
            Le.ArgTypes = new List<HashSet<Type>>{  new HashSet<Type>{typeof(int)},   new HashSet<Type>{typeof(int)}};
            Le.ResultType = typeof(bool);
            Le.ValidContexts = EvalContext.Compile | EvalContext.Predicate |EvalContext.Resolve;
            Le.Callable = p_Le;
            m_BuiltinFuncs["<"] = Le;

            Builtin_FuncInfo Leq = new Builtin_FuncInfo();
            Leq.ArgTypes = new List<HashSet<Type>>{  new HashSet<Type>{typeof(int)},   new HashSet<Type>{typeof(int)}};
            Leq.ResultType = typeof(bool);
            Leq.ValidContexts = EvalContext.Compile | EvalContext.Predicate |EvalContext.Resolve;
            Leq.Callable = p_Leq;
            m_BuiltinFuncs["<="] = Leq;

            Builtin_FuncInfo Ge = new Builtin_FuncInfo();
            Ge.ArgTypes = new List<HashSet<Type>>{  new HashSet<Type>{typeof(int)},   new HashSet<Type>{typeof(int)}};
            Ge.ResultType = typeof(bool);
            Ge.ValidContexts = EvalContext.Compile | EvalContext.Predicate |EvalContext.Resolve;
            Ge.Callable = p_Ge;
            m_BuiltinFuncs[">"] = Ge;

            Builtin_FuncInfo Geq = new Builtin_FuncInfo();
            Geq.ArgTypes = new List<HashSet<Type>>{  new HashSet<Type>{typeof(int)},   new HashSet<Type>{typeof(int)}};
            Geq.ResultType = typeof(bool);
            Geq.ValidContexts = EvalContext.Compile | EvalContext.Predicate |EvalContext.Resolve;
            Geq.Callable = p_Geq;
            m_BuiltinFuncs[">="] = Geq;
        }
        public object Eval(EvaluationEnvironment Envir,Expression Expr)
        {
            object ReturnValue = null;
            if(Expr is Expression_Literal)
            {
                ReturnValue = ((Expression_Literal)Expr).Value;
            }
            else if(Expr is Expression_Variable)
            {
                ReturnValue = Envir.GetVar(((Expression_Variable)Expr).VarName);
            } 
            else if(Expr is Expression_FuncCall)
            {
                Expression_FuncCall FuncExpr = ((Expression_FuncCall)Expr);
                Builtin_FuncInfo FuncToCall = m_BuiltinFuncs[((Expression_FuncCall)Expr).FuncName];
                BuiltinFuncArgs FuncArgs = new BuiltinFuncArgs();
                FuncArgs.Envir = Envir;
                FuncArgs.Handler = this;
                foreach(Expression SubExpr in FuncExpr.Args)
                {
                    FuncArgs.Arguments.Add(Eval(Envir,SubExpr));
                }
                foreach(KeyValuePair<string,Expression> SubExpr in FuncExpr.KeyArgs)
                {
                    FuncArgs.KeyArguments[SubExpr.Key] = Eval(Envir,SubExpr.Value);
                }
                ReturnValue = FuncToCall.Callable(FuncArgs);
            }
            else if(Expr is Expression_List)
            {
                Expression_List ListExpr = (Expression_List)Expr;
                foreach(Expression SubExpr in ListExpr.Contents)
                {
                    Eval(Envir,SubExpr);
                }
                ReturnValue = null;
            }
            else if(Expr is Expression_Assignment)
            {
                var Assignment = Expr as Expression_Assignment;
                Envir.AddVar(Assignment.VariableName, Eval(Envir, Assignment.VariableValue));
            }
            else if(Expr is Expression_ContinousAbility)
            {
                Expression_ContinousAbility ContinousLiteral = (Expression_ContinousAbility)Expr;
                ContinousAbility Result = new ContinousAbility();
                Result.Envir = new EvaluationEnvironment();
                Result.Envir.SetParent(Envir);
                Result.Ability = new(ContinousLiteral.Ability);
                Result.Index = ContinousLiteral.Index;

                (Result.Ability.AffectedEntities as RuleManager.TargetCondition_UnitScript).Envir = Result.Envir;
                (Result.Ability.EffectToApply as RuleManager.Effect_ContinousUnitScript).Envir = Result.Envir;

                ReturnValue = Result;
            }
            else if (Expr is Expression_TriggeredAbility)
            {
                Expression_TriggeredAbility TriggeredLiteral = (Expression_TriggeredAbility)Expr;
                TriggeredAbility Result = new TriggeredAbility();
                Result.Envir = new EvaluationEnvironment();
                Result.Envir.SetParent(Envir);
                Result.Ability = new(TriggeredLiteral.Ability);
                Result.Index = TriggeredLiteral.Index;
                (Result.Ability.TriggeredEffect as RuleManager.Effect_UnitScript).Envir = Result.Envir;
                ReturnValue = Result;
            }
            else if(Expr is Expression_Eq)
            {
                Expression_Eq EqExpr = (Expression_Eq)Expr;
                object Lhs = Eval(Envir,EqExpr.Lhs);
                object Rhs = Eval(Envir,EqExpr.Rhs);
                if(Lhs is int)
                {
                    return (int)Lhs == (int)Rhs;
                }
                else if(Lhs is string)
                {
                    return (string)Lhs == (string)Rhs;
                }
                else if(Lhs is bool)
                {
                    return (bool)Lhs == (bool)Rhs;
                }
                else if(Lhs is RuleManager.UnitIdentifier)
                {
                    return ((RuleManager.UnitIdentifier)Lhs).ID == ((RuleManager.UnitIdentifier)Rhs).ID;
                }
                else if(Lhs is RuleManager.UnitInfo)
                {
                    return ((RuleManager.UnitInfo)Lhs).UnitID == ((RuleManager.UnitInfo)Rhs).UnitID;
                }
                ReturnValue = null;
            }
            else if(Expr is Expression_StatModification)
            {
                Expression_StatModification ModExpr = (Expression_StatModification)Expr;
                RuleManager.UnitInfo UnitToModify = (RuleManager.UnitInfo)Eval(Envir,ModExpr.Stat.UnitVar);
                object ModificationValue = Eval(Envir,ModExpr.Value);
                if(ModExpr.Stat.statType == StatType.Stat)
                {
                    //lowkey only stat we want to modify at the moment
                    if(ModExpr.Stat.FieldName == "Movement")
                    {
                        if(ModExpr.ModType == ModificationType.Add)
                        {
                            UnitToModify.Stats.Movement += (int)ModificationValue;
                        }
                        else if(ModExpr.ModType == ModificationType.Set)
                        {
                            UnitToModify.Stats.Movement += (int)ModificationValue;
                        }
                    }
                    else if(ModExpr.Stat.FieldName == "Damage")
                    {
                        if(ModExpr.ModType == ModificationType.Add)
                        {
                            UnitToModify.Stats.Damage += (int)ModificationValue;
                        }
                        else if(ModExpr.ModType == ModificationType.Set)
                        {
                            UnitToModify.Stats.Damage += (int)ModificationValue;
                        }
                    }
                }
                else if(ModExpr.Stat.statType == StatType.UnitField)
                {
                    if(ModExpr.ModType == ModificationType.Set)
                    {
                        UnitToModify.Envir.AddVar(ModExpr.Stat.FieldName,ModificationValue);
                    }
                    else if(ModExpr.ModType == ModificationType.Add)
                    {
                        if(ModExpr.Stat.ReferenceType == typeof(int))
                        {
                            int NewValue = ((int)UnitToModify.Envir.GetVar(ModExpr.Stat.FieldName)) + (int)ModificationValue;
                            UnitToModify.Envir.AddVar(  ModExpr.Stat.FieldName, NewValue);
                        }
                    }
                }
            }
            return ReturnValue;
        }

        public Expression ConvertLiteral(List<Diagnostic> OutDiagnostics, Parser.Expression_Literal ParsedExpression, out Type ResultType)
        {
            Type OutType = typeof(void);
            Expression ReturnValue = new Expression();
           if(ParsedExpression.literal is Parser.Literal_Int)
           {
               Expression_Literal NewLiteral = new Expression_Literal();
               NewLiteral.Value = ((Parser.Literal_Int)ParsedExpression.literal).Value;
               OutType = typeof(int);
               ReturnValue = NewLiteral;
           }
           else if(ParsedExpression.literal is Parser.Literal_String)
           {
               Expression_Literal NewLiteral = new Expression_Literal();
               string StringValue = ((Parser.Literal_String)ParsedExpression.literal).Value;
               NewLiteral.Value = StringValue.Substring(1,StringValue.Length-2);
               OutType = typeof(string);
               ReturnValue = NewLiteral;
           }
           else if(ParsedExpression.literal is Parser.Literal_Bool)
           {
               Expression_Literal NewLiteral = new Expression_Literal();
               NewLiteral.Value = ((Parser.Literal_Bool)ParsedExpression.literal).Value;
               OutType = typeof(bool);
               ReturnValue = NewLiteral;
           }
           else
           {
               throw new Exception("Unhandled literal case");
           }
           ResultType = OutType;
           return ReturnValue;
        }
        public Expression ConvertExpression_Ability(TypeCheckingContext Context,EvalContext CurrentContext, EvaluationEnvironment Envir ,Parser.Expression_Ability ParsedExpression,out Type ResultType)
        {
            Expression ReturnValue = null;
            Type OutType = typeof(void);
            EvaluationEnvironment NewEnvir = new();
            NewEnvir.SetParent(Envir);

            AbilityInformation ConvertedAbility = ConvertAbility(Context, Envir, ParsedExpression.AbilityLiteral);
            if(ParsedExpression.AbilityLiteral is Parser.Ability_Activated)
            {
                   
            }
            else if(ConvertedAbility.Ability is RuleManager.Ability_Triggered)
            {
                Expression_TriggeredAbility Result = new Expression_TriggeredAbility();
                Result.Ability = ConvertedAbility.Ability as RuleManager.Ability_Triggered;
                Result.Index = ConvertedAbility.Index;
                ReturnValue = Result;
                OutType = typeof(TriggeredAbility);
            }
            else if(ConvertedAbility.Ability is RuleManager.Ability_Continous)
            {
                Expression_ContinousAbility Result = new Expression_ContinousAbility();
                Result.Ability = ConvertedAbility.Ability as RuleManager.Ability_Continous;
                Result.Index = ConvertedAbility.Index;
                ReturnValue = Result;
                OutType = typeof(ContinousAbility);
            }
            ResultType = OutType;
            return ReturnValue;
        }
        public Expression ConvertExpression(TypeCheckingContext Context, EvalContext CurrentContext, EvaluationEnvironment Envir ,Parser.Expression ParsedExpression,out Type ResultType)
        {
            Expression ReturnValue = new Expression();
            Type OutType = typeof(void);
            if(ParsedExpression is Parser.Expression_Literal)
            {
                return ConvertLiteral(Context.OutDiagnostics, (Parser.Expression_Literal)ParsedExpression, out ResultType);
            }
            else if (ParsedExpression is Parser.Expression_FuncCall)
            {
                Parser.Expression_FuncCall Func = (Parser.Expression_FuncCall)ParsedExpression;
                if(Func.FuncName.Value == "")
                {
                    return ConvertExpression(Context,CurrentContext,Envir,Func.Args[0],out ResultType);
                }
                Expression_FuncCall NewFunc = new Expression_FuncCall();
                NewFunc.FuncName = Func.FuncName.Value;
                List<Type> ArgTypes = new List<Type>();
                Dictionary<Parser.Token,Type> KeyArgTypes = new Dictionary<Parser.Token,Type>();
                foreach (Parser.Expression Argument in Func.Args)
                {
                    Type ArgType;
                    Expression ArgExpr = ConvertExpression(Context,CurrentContext, Envir,Argument, out ArgType);
                    NewFunc.Args.Add(ArgExpr);
                    ArgTypes.Add(ArgType);
                }
                foreach (Parser.KeyArg Argument in Func.KeyArgs) {
                    Type ArgType;
                    Expression ArgExpr = ConvertExpression(Context,CurrentContext, Envir,Argument.Value, out ArgType);
                    NewFunc.KeyArgs[Argument.Name.Value] = ArgExpr;
                    KeyArgTypes[Argument.Name] = ArgType;
                }
                if (m_BuiltinFuncs.ContainsKey(Func.FuncName.Value))
                {
                    Builtin_FuncInfo FuncInfo = m_BuiltinFuncs[Func.FuncName.Value];
                    for (int i = 0; i < Math.Min(NewFunc.Args.Count, FuncInfo.ArgTypes.Count); i++)
                    {
                        if (!FuncInfo.ArgTypes[i].Contains(ArgTypes[i]))
                        {
                            string TypeString = "";
                            int Index = 0;
                            foreach(Type ValidType in FuncInfo.ArgTypes[i])
                            {
                                TypeString += ValidType.Name;
                                if(Index + 1 < FuncInfo.ArgTypes[i].Count)
                                {
                                    TypeString += " or ";
                                }
                                Index += 1;
                            }
                            Context.OutDiagnostics.Add(new Diagnostic(Func.FuncName, "Argument has invalid type: "+ TypeString + " expected"));
                            OutType = typeof(int);
                        }
                    }
                    foreach(KeyValuePair<Parser.Token,Type> KeyArg in KeyArgTypes)
                    {
                        if(!FuncInfo.KeyArgTypes.ContainsKey(KeyArg.Key.Value))
                        {
                            Context.OutDiagnostics.Add(new Diagnostic(KeyArg.Key,"Invalid key argument \""+KeyArg.Key.Value+"\""));
                        }
                        else
                        {
                            Type ExpectedType = FuncInfo.KeyArgTypes[KeyArg.Key.Value];
                            if(ExpectedType != KeyArg.Value)
                            {
                               Context.OutDiagnostics.Add(new Diagnostic(KeyArg.Key,
                                            "Invalid key argument type: "+ExpectedType.Name + " expected, "+KeyArg.Value.Name+ " recieved"));
                            }
                        }
                    }
                    if( (FuncInfo.ValidContexts & CurrentContext) == 0)
                    {
                        Context.OutDiagnostics.Add(new Diagnostic(Func.FuncName,"Cannot call \""+Func.FuncName.Value+"\" in currenct evaluation context"));
                    }
                    OutType = FuncInfo.ResultType;
                }
                else if(m_SpecialFuncs.ContainsKey(Func.FuncName.Value))
                {
                    return m_SpecialFuncs[Func.FuncName.Value](Context,CurrentContext,Envir, Func,out ResultType);
                }
                else
                {
                    Context.OutDiagnostics.Add(new Diagnostic(Func.FuncName, "No builtin function named \"" + Func.FuncName.Value + "\""));
                    OutType = typeof(void);
                }
                ReturnValue = NewFunc;
            }
            else if (ParsedExpression is Parser.Expression_Variable)
            {
                Parser.Token VarToken = ((Parser.Expression_Variable)ParsedExpression).Variable;
                if(Envir.HasVar(VarToken.Value))
                {
                    Expression_Variable NewValue = new Expression_Variable();
                    NewValue.VarName  = VarToken.Value;
                    OutType = Envir.GetVarType(VarToken.Value);
                    ReturnValue = NewValue;
                }
                else
                {
                    Context.OutDiagnostics.Add(new Diagnostic(VarToken,"No variable found with name \""+VarToken.Value+"\""));
                }
            }
            else if(ParsedExpression is Parser.Expression_StatReference)
            {
                OutType = typeof(StatReference);
                ReturnValue = ConvertStatReference(Context,CurrentContext,Envir,(Parser.Expression_StatReference)ParsedExpression);
            }
            else if(ParsedExpression is Parser.Expression_Ability)
            {
                return ConvertExpression_Ability(Context,CurrentContext,Envir,(Parser.Expression_Ability)ParsedExpression,out ResultType);
            }
            ResultType = OutType;
            //ResultType = ResultType;
            return ReturnValue;
        }
        public Expression ConvertStatReference(TypeCheckingContext Context,EvalContext CurrentContext,EvaluationEnvironment Envir, Parser.Expression_StatReference StatRef)
        {
            var NewStat = p_GetStatType(Context,CurrentContext,Envir,StatRef);
            var  ReturnValue = new Expression_Literal();
            ReturnValue.Value = NewStat;
            return ReturnValue;
        }
        public RuleManager.UnitStats ConvertStats(RuleManager.UnitInfo CurrentInfo,List<Diagnostic> OutDiagnostics, Parser.UnitStats ParsedUnit)
        {
            RuleManager.UnitStats ReturnValue = new RuleManager.UnitStats();
            int Width = 1;
            int Height = 1;
            foreach(Parser.StatDeclaration Declaration in ParsedUnit.Declarations)
            {
                if(Declaration.AssignedValue < 0)
                {
                    OutDiagnostics.Add(new Diagnostic(Declaration.Stat,"Stat cannot be negative"));
                }
                if(Declaration.Stat.Value == "HP")
                {
                    ReturnValue.HP = Declaration.AssignedValue;
                }
                else if(Declaration.Stat.Value == "Movement")
                {
                    ReturnValue.Movement = Declaration.AssignedValue;
                }
                else if(Declaration.Stat.Value == "Range")
                {
                    ReturnValue.Range = Declaration.AssignedValue;
                }
                else if(Declaration.Stat.Value == "Damage")
                {
                    ReturnValue.Damage = Declaration.AssignedValue;
                }
                else if(Declaration.Stat.Value == "ActivationCost")
                {
                    ReturnValue.ActivationCost = Declaration.AssignedValue;
                }
                else if(Declaration.Stat.Value == "ObjectiveControll")
                {
                    ReturnValue.ObjectiveControll = Declaration.AssignedValue;
                }
                else if(Declaration.Stat.Value == "Width")
                {
                    Width = Declaration.AssignedValue;
                }
                else if(Declaration.Stat.Value == "Height")
                {
                    Height = Declaration.AssignedValue;
                }
                else
                {
                    OutDiagnostics.Add(new Diagnostic(Declaration.Stat,"No stat field named \""+Declaration.Stat.Value+"\""));
                }
            }
            foreach(var Tag in ParsedUnit.Tags)
            {
                CurrentInfo.Tags.Add(Tag.Value);
            }
            if(!(Width == 1 && Height == 1))
            {
                CurrentInfo.UnitTileOffsets = new List<RuleManager.Coordinate>();
                for(int i = 0; i < Width; i++)
                {
                    for(int j = 0; j < Height; j++)
                    {
                        CurrentInfo.UnitTileOffsets.Add( new RuleManager.Coordinate(i,j));
                    }   
                }
            }
            return ReturnValue;
        }
        public ResourceManager.UnitUIInfo ConvertVisuals(TypeCheckingContext Context, EvaluationEnvironment Envir, Parser.Visuals ParsedVisuals)
        {
            ResourceManager.UnitUIInfo ReturnValue = new ResourceManager.UnitUIInfo();
            foreach(Parser.VariableDeclaration Declaration in ParsedVisuals.Declarations)
            {
                int PrevCount = Context.OutDiagnostics.Count;
                Type ResultType = null;
                Expression ExpressionToEvaluate = ConvertExpression(Context,EvalContext.Compile,Envir,Declaration.VariableValue,out ResultType);
                ResourceManager.Animation Result = null;
                try
                {
                    if(ResultType != typeof(ResourceManager.Animation))
                    {
                        Context.OutDiagnostics.Add(new Diagnostic(Declaration.VariableName,"Expression doesn't evaluate to an animation"));
                    }
                    else if(Context.OutDiagnostics.Count == PrevCount)
                    {
                        Result = (ResourceManager.Animation) Eval(Envir,ExpressionToEvaluate);
                    }
                    if(Declaration.VariableName.Value == "Up")
                    {
                        ReturnValue.UpAnimation = Result;
                    }
                    else if(Declaration.VariableName.Value == "Down")
                    {
                        ReturnValue.DownAnimation = Result;
                    }
                    else if(Declaration.VariableName.Value == "Attack")
                    {
                        ReturnValue.AttackAnimation = Result;
                    }
                    else
                    {
                        Context.OutDiagnostics.Add(new Diagnostic(Declaration.VariableName,"No visual field named \""+Declaration.VariableName.Value+"\""));
                    }
                }
                catch(System.Exception e)
                {
                    Context.OutDiagnostics.Add(new Diagnostic(Declaration.VariableName,"Error evaluationg rhs: "+e.Message));
                }
            }
            return ReturnValue;
        }
        RuleManager.Effect  ConvertEffect(TypeCheckingContext Context , EvalContext EvalContext, EvaluationEnvironment Envir,List<Parser.AbilityStatement> ParsedAbility)
        {
            RuleManager.Effect_UnitScript ReturnValue = new RuleManager.Effect_UnitScript();
            Expression_List NewEffect = new Expression_List();
            foreach(Parser.AbilityStatement Statement in ParsedAbility)
            {
                if(Statement is Parser.AbilityStatement_Expression)
                {
                    Type OutType = null;
                    NewEffect.Contents.Add(ConvertExpression(Context,EvalContext,Envir, ((Parser.AbilityStatement_Expression)Statement).Expr,out OutType));
                }
                else if(Statement is Parser.AbilityStatement_Assignment)
                {
                    var Assignment = Statement as Parser.AbilityStatement_Assignment;
                    if(Envir.HasVar(Assignment.Variable.Value))
                    {
                        Context.OutDiagnostics.Add(new Diagnostic(Assignment.Variable, "Variables in the same scope with the same name is not allowed"));
                    }
                    Type OutType = null;
                    var Expr = ConvertExpression(Context, EvalContext, Envir, Assignment.Expr,out OutType);
                    Envir.AddVarType(Assignment.Variable.Value,OutType);
                    Expression_Assignment Effect = new();
                    Effect.VariableName = Assignment.Variable.Value;
                    Effect.VariableValue = Expr;
                    NewEffect.Contents.Add(Effect);
                }
            }
            ReturnValue.Expr = NewEffect;

            Context.AssociatedUnit.TotalEffects[Context.AssociatedUnit.CurrentEffectID] = ReturnValue;
            ReturnValue.ResourceID =  Context.AssociatedUnit.ResourceID;
            ReturnValue.EffectID = Context.AssociatedUnit.CurrentEffectID;
            Context.AssociatedUnit.CurrentEffectID += 1;
            return ReturnValue;
        }
        RuleManager.TargetType StringToType(TypeCheckingContext Context,string Type)
        {
            RuleManager.TargetType ReturnValue = RuleManager.TargetType.Null;
            if(Type == "Unit")
            {
                return RuleManager.TargetType.Unit;
            }
            else if(Type == "Tile")
            {
                return RuleManager.TargetType.Tile;
            }
            else if(Context.ImportedUnits.ContainsKey(Type))
            {
                return RuleManager.TargetType.Unit;
            }
            else if(Type == "Player")
            {
                return RuleManager.TargetType.Player;
            }
            return ReturnValue;
        }
        RuleManager.UnitScriptTarget ConvertTarget(TypeCheckingContext Context, EvaluationEnvironment Envir,Parser.ActivatedAbilityTarget ParsedTarget)
        {
            RuleManager.UnitScriptTarget ReturnValue = new RuleManager.UnitScriptTarget();
            ReturnValue.Name = ParsedTarget.Name.Value;
            ReturnValue.Type = StringToType(Context, ParsedTarget.TargetType.TypeIdentifier.Value);

            if (ReturnValue.Type == RuleManager.TargetType.Null)
            {
                Context.OutDiagnostics.Add(new Diagnostic(ParsedTarget.TargetType.TypeIdentifier,"No target type with name \""+ParsedTarget.TargetType.TypeIdentifier.Value+"\""));   
                return ReturnValue;
            }
            if(!(ParsedTarget.RangeBegin.Line == 0 && ParsedTarget.RangeBegin.ByteOffset == 0))
            {
                Type ResultType;
                ReturnValue.Range = ConvertExpression(Context,EvalContext.Predicate,Envir,ParsedTarget.RangeExpression,out ResultType);
                if(ResultType != typeof(List<RuleManager.Coordinate>))
                {
                    Context.OutDiagnostics.Add(new Diagnostic(ParsedTarget.RangeBegin,5,"Result of range expression must be of type List<Coordinate>"));
                }
            }
            bool SpecificUnitTarget = false;
            //add dummy type to environment
            if (ReturnValue.Type == RuleManager.TargetType.Unit)
            {
                if(Context.ImportedUnits.ContainsKey(ParsedTarget.TargetType.TypeIdentifier.Value))
                {
                    SpecificUnitTarget = true;
                    Envir.AddVar(ReturnValue.Name, Context.ImportedUnits[ParsedTarget.TargetType.TypeIdentifier.Value]);
                }
                else
                {
                    Envir.AddVar(ReturnValue.Name, new RuleManager.UnitIdentifier());
                }
            }
            else if (ReturnValue.Type == RuleManager.TargetType.Tile)
            {
                Envir.AddVar(ReturnValue.Name, new RuleManager.Coordinate());
            }
            if (!(ParsedTarget.HoverBegin.Line == 0 && ParsedTarget.HoverBegin.ByteOffset == 0))
            {
                Type ResultType;
                ReturnValue.Hover = ConvertExpression(Context, EvalContext.Predicate, Envir, ParsedTarget.HoverExpression, out ResultType);
                if (ResultType != typeof(List<RuleManager.Coordinate>))
                {
                    Context.OutDiagnostics.Add(new Diagnostic(ParsedTarget.HoverBegin, 5, "Result of range expression must be of type List<Coordinate>"));
                }
            }
            Type OutType  = null;
            ReturnValue.Condition = ConvertExpression(Context,EvalContext.Predicate,Envir,ParsedTarget.Condition,out OutType);
            if(SpecificUnitTarget)
            {
                Expression_FuncCall AndCondition = new();
                int ID = Context.ImportedUnits[ParsedTarget.TargetType.TypeIdentifier.Value].ID;
                Expression_FuncCall IsTypeCondition = new();
                IsTypeCondition.Args = new List<Expression> { new Expression_Variable(ParsedTarget.Name.Value), new Expression_Literal(m_LoadedUnits[ID])};
                IsTypeCondition.FuncName = "IsUnitType";

                AndCondition.Args = new List<Expression>{ IsTypeCondition,ReturnValue.Condition};
                AndCondition.FuncName = "&&";
                ReturnValue.Condition = AndCondition;
            }
            return ReturnValue;
        }
        RuleManager.TargetInfo ConvertTargets(TypeCheckingContext Context, EvaluationEnvironment Envir,List<Parser.ActivatedAbilityTarget> Targets)
        {
            RuleManager.TargetInfo_List ReturnValue = new RuleManager.TargetInfo_List();
            List<RuleManager.UnitScriptTarget> ConvertedTargets = new List<RuleManager.UnitScriptTarget>();
            foreach(Parser.ActivatedAbilityTarget  Target in Targets)
            {
                ConvertedTargets.Add(ConvertTarget(Context, Envir,Target));
            }
            RuleManager.TargetCondition_UnitScript Condition = new RuleManager.TargetCondition_UnitScript();
            Condition.Targets = ConvertedTargets;
            foreach(Parser.ActivatedAbilityTarget  Target in Targets)
            {
                ReturnValue.Targets.Add(Condition);
            }

            Context.AssociatedUnit.TotalTargetConditions[Context.AssociatedUnit.CurrentEffectID] = Condition;
            Condition.ConditionID = Context.AssociatedUnit.CurrentEffectID;
            Condition.ResourceID = Context.AssociatedUnit.ResourceID;
            Context.AssociatedUnit.CurrentEffectID++;
            return ReturnValue;
        }

        public RuleManager.Ability_Activated ConvertActivated(TypeCheckingContext Context, EvaluationEnvironment Envir,Parser.Ability_Activated ParsedAbility)
        {
            RuleManager.Ability_Activated ReturnValue = new RuleManager.Ability_Activated();
            //Hopefully backwards compatible way to implement this, new TargetInfo and Effect that uses this internally
            ReturnValue.ActivationTargets = ConvertTargets(Context,Envir,ParsedAbility.Targets);
            ReturnValue.ActivatedEffect = ConvertEffect(Context,EvalContext.Resolve,Envir,ParsedAbility.Statements);
            //get targets
            if(ParsedAbility.Targets.Count > 0)
            {
                var EffectToModify = ReturnValue.ActivatedEffect as RuleManager.Effect_UnitScript;
                EffectToModify.Targets = ((ReturnValue.ActivationTargets as RuleManager.TargetInfo_List).Targets[0] as RuleManager.TargetCondition_UnitScript).Targets;
            }
            if(ParsedAbility.ActivationCount != -1)
            {
                ReturnValue.AllowedActivations = ParsedAbility.ActivationCount;
            }

            //AssociatedUnit.TotalTargetConditions[AssociatedUnit.CurrentEffectID] = ReturnValue.ActivationTargets;

            return ReturnValue;
        }
        class TriggerEventInfo
        {
            public RuleManager.TriggerType Event;
            public List<RuleManager.TargetType> PossibleTargets = new();

            public TriggerEventInfo()
            {

            }
            public TriggerEventInfo(RuleManager.TriggerType Event,List<RuleManager.TargetType> Targets)
            {
                this.Event = Event;
                PossibleTargets = Targets;
            }
        }

        Dictionary<string, TriggerEventInfo> m_TriggerEvents = new Dictionary<string, TriggerEventInfo> { 
            { "NewBattleround", new TriggerEventInfo(RuleManager.TriggerType.BattleroundBegin,new List<RuleManager.TargetType>()) } 
        };
        public RuleManager.Ability_Triggered ConvertTriggered(TypeCheckingContext Context,EvaluationEnvironment Envir, Parser.Ability_Triggered ParsedAbility)
        {
            RuleManager.Ability_Triggered ReturnValue = new();
            RuleManager.TriggerCondition_Or Condition = new();
            foreach(var Kind in ParsedAbility.TriggerKinds)
            {
                if(!m_TriggerEvents.ContainsKey(Kind.Value))
                {
                    Context.OutDiagnostics.Add(new Diagnostic(Kind, "Invalid trigger kind: " + Kind.Value));
                }
                else
                {
                    var Info = m_TriggerEvents[Kind.Value];
                    Condition.ConditionsToSatisfy.Add(new RuleManager.TriggerCondition_Type(Info.Event));
                }
                ReturnValue.Condition = Condition;
            }
            var Targets = new List<RuleManager.UnitScriptTarget>();
            var ConvertedTargetInfo = ConvertTargets(Context, Envir, ParsedAbility.Targets) as RuleManager.TargetInfo_List;
            if(ConvertedTargetInfo.Targets.Count > 0)
            {
                Targets = (ConvertedTargetInfo.Targets[0] as RuleManager.TargetCondition_UnitScript).Targets;
            }
            ReturnValue.TriggeredEffect = ConvertEffect(Context,EvalContext.Resolve, Envir, ParsedAbility.Statements);
            (ReturnValue.TriggeredEffect as RuleManager.Effect_UnitScript).Targets = Targets;
            if(ParsedAbility.Targets.Count != 0)
            {
                var Kind = ParsedAbility.TriggerKinds[ParsedAbility.TriggerKinds.Count - 1];
                if (m_TriggerEvents.ContainsKey(Kind.Value))
                {
                    var Info = m_TriggerEvents[Kind.Value];
                    for(int i = 0; i < Info.PossibleTargets.Count && i < Targets.Count;i++)
                    {
                        if(Info.PossibleTargets[i] != Targets[i].Type)
                        {
                            Context.OutDiagnostics.Add(new Diagnostic(ParsedAbility.Targets[i].Name, "Invalid target type for trigger event \"" + Kind.Value + "\""));
                        }
                    }
                    for (int i = Info.PossibleTargets.Count; i < Targets.Count; i++)
                    {
                        Context.OutDiagnostics.Add(new Diagnostic(ParsedAbility.Targets[i].Name, "Invalid target type for trigger event \"" + Kind.Value + "\""));
                    }
                }
            }
            return ReturnValue;
        }
        public RuleManager.Ability_Continous ConvertContinous(TypeCheckingContext Context , EvaluationEnvironment Envir,Parser.Ability_Continous ParsedAbility)
        {
            RuleManager.Ability_Continous ReturnValue = new RuleManager.Ability_Continous();

            EvaluationEnvironment EffectEnvir = new EvaluationEnvironment();
            EffectEnvir.ShadowRemove("this");
            EffectEnvir.SetParent(Envir);
            //Hopefully backwards compatible way to implement this, new TargetInfo and Effect that uses this internally
            ReturnValue.AffectedEntities =((RuleManager.TargetInfo_List)  ConvertTargets(Context,Envir,new List<Parser.ActivatedAbilityTarget>{ ParsedAbility.AffectedEntities})).Targets[0];
            RuleManager.Effect_ContinousUnitScript Effect = new RuleManager.Effect_ContinousUnitScript();
            Effect.UnitName = ParsedAbility.AffectedEntities.Name.Value;
            Effect.Expr = ((RuleManager.Effect_UnitScript)  ConvertEffect(Context,EvalContext.Continous,EffectEnvir,ParsedAbility.Statements)).Expr;
            ReturnValue.EffectToApply = Effect;

            Context.AssociatedUnit.TotalEffects[Context.AssociatedUnit.CurrentEffectID] = ReturnValue.EffectToApply;
            Effect.ResourceID = Context.AssociatedUnit.ResourceID;
            Effect.EffectID = Context.AssociatedUnit.CurrentEffectID;
            Context.AssociatedUnit.CurrentEffectID += 1;

            return ReturnValue;
        }
        public AbilityInformation ConvertAbility(TypeCheckingContext Context,EvaluationEnvironment Envir,Parser.Ability ParsedAbility)
        {
            AbilityInformation ReturnValue = new AbilityInformation();
            //ensure that the "this" pointer is present
            if(ParsedAbility is Parser.Ability_Activated)
            {
                var NewAbility = ConvertActivated(Context, Envir, (Parser.Ability_Activated)ParsedAbility);
                ReturnValue.Ability = NewAbility;
            }
            else if(ParsedAbility is Parser.Ability_Triggered)
            {
                var NewAbility = ConvertTriggered(Context, Envir, (Parser.Ability_Triggered)ParsedAbility);
                ReturnValue.Ability = NewAbility;
            }
            else if(ParsedAbility is Parser.Ability_Continous)
            {
                ReturnValue.Ability = ConvertContinous(Context,Envir,(Parser.Ability_Continous)ParsedAbility);
            }
            foreach(var Attribute in ParsedAbility.Attributes)
            {
                string Error;
                object AttributeValue = EvalConstexpr(Context,Envir,Attribute.VariableValue,out Error);
                string AttributeString = "";
                if(AttributeValue == null)
                {
                    Context.OutDiagnostics.Add(new Diagnostic(Attribute.VariableName,"Error evaluating rhs: "+Error));
                }
                if(Attribute.VariableName.Value == "Icon")
                {
                    if(AttributeValue != null && AttributeValue.GetType() != typeof(ResourceManager.Animation))
                    {
                        Context.OutDiagnostics.Add(new Diagnostic(Attribute.VariableName,"Rhs needs to be of type visual, is of type "+
                                    AttributeValue.GetType().Name));
                    }
                    else
                    {
                        ReturnValue.Icon = (ResourceManager.Animation)AttributeValue;
                    }
                }
                else
                {
                    if(AttributeValue != null && AttributeValue.GetType() != typeof(string))
                    {
                        Context.OutDiagnostics.Add(new Diagnostic(Attribute.VariableName,"Rhs needs to be of type string, is of type "+
                                    AttributeValue.GetType().Name));
                    }
                    else
                    {
                        AttributeString = (string)AttributeValue;   
                    }
                    if(Attribute.VariableName.Value == "Name")
                    {
                        ReturnValue.Ability.SetName(AttributeString);
                    }
                    else if(Attribute.VariableName.Value == "Description")
                    {
                        ReturnValue.Ability.SetDescription(AttributeString);
                    }
                    else if(Attribute.VariableName.Value == "Flavour")
                    {
                        ReturnValue.Ability.SetFlavour(AttributeString);
                    }
                    else
                    {
                        Context.OutDiagnostics.Add(new Diagnostic(Attribute.VariableName,"Invalid Ability attribute \""+Attribute.VariableName.Value+"\""));   
                    }
                }
            }
            ReturnValue.Index = Context.AssociatedUnit.CurrentAbilityID;
            Context.AssociatedUnit.TotalAbilities[Context.AssociatedUnit.CurrentAbilityID] = ReturnValue;
            Context.AssociatedUnit.CurrentAbilityID++;
            return ReturnValue;
        }
        //returns null on error
        object EvalConstexpr(TypeCheckingContext Context ,EvaluationEnvironment Envir,Parser.Expression Expr,out string OutError)
        {
            int ErrCount = Context.OutDiagnostics.Count;
            object ResultObject = 1f;
            Type ResultType;
            Expression ExprToEvaluate = ConvertExpression(Context,EvalContext.Compile,Envir,Expr,out ResultType);
            if(Context.OutDiagnostics.Count == ErrCount)
            {
                try
                {
                    ResultObject = Eval(Envir,ExprToEvaluate);
                }
                catch(System.Exception e)
                {
                    ResultObject = null;
                    OutError = e.Message;
                    return ResultObject;
                }
            }
            OutError = "";
            return ResultObject;
        }

        Dictionary<string,int> m_ConvertedUnits = new();

        public class TypeCheckingContext
        {
            public List<Diagnostic> OutDiagnostics = new(); 
            public Dictionary<string, RuleManager.UnitIdentifier> ImportedUnits = new();
            public ResourceManager.UnitResource AssociatedUnit = new();
        }

        public  ResourceManager.UnitResource ConvertUnit(List<Diagnostic> OutDiagnostics,string UnitPath, Parser.Unit ParsedUnit)
        {
            string AbsolutePath = Path.GetFullPath(UnitPath);
            if (m_ConvertedUnits.ContainsKey(UnitPath))
            {
                return m_LoadedUnits[m_ConvertedUnits[UnitPath]];
            }
            ResourceManager.UnitResource ReturnValue = new ResourceManager.UnitResource();
            int CurrentUnitID = m_CurrentUnitID;
            m_StringToUnit[ParsedUnit.Name.Value] = m_CurrentUnitID;
            m_LoadedUnits[m_CurrentUnitID] = ReturnValue;
            m_CurrentUnitID++;


            TypeCheckingContext Context = new();
            Context.OutDiagnostics = OutDiagnostics;
            Context.AssociatedUnit = ReturnValue;

            ReturnValue.GameInfo.Stats = ConvertStats(ReturnValue.GameInfo,OutDiagnostics,ParsedUnit.Stats);
            ReturnValue.UIInfo = ConvertVisuals(Context, ReturnValue.GameInfo.Envir,ParsedUnit.visuals);
            ReturnValue.Name = ParsedUnit.Name.Value;
            ReturnValue.GameInfo.Envir.Metadata.ResourceID = ReturnValue.ResourceID;

            m_ConvertedUnits[AbsolutePath] = CurrentUnitID;

            Context.ImportedUnits[ParsedUnit.Name.Value] = new(CurrentUnitID);

            foreach(Parser.VariableDeclaration Vars in ParsedUnit.Variables)
            {
                int ErrCount = OutDiagnostics.Count;
                Type ResultType = null;
                //arbitrary non-allowed value
                object ResultObject = 1f;
                Expression ExprToEvaluate = ConvertExpression(Context,EvalContext.Compile, ReturnValue.GameInfo.Envir,Vars.VariableValue, out ResultType);
                if(OutDiagnostics.Count == ErrCount)
                {
                    try
                    {
                        ResultObject = Eval(ReturnValue.GameInfo.Envir,ExprToEvaluate);
                    }
                    catch(System.Exception e)
                    {
                        OutDiagnostics.Add(new Diagnostic(Vars.VariableName,"Error evaluating rhs: "+e.Message));
                    }
                }
                ReturnValue.GameInfo.Envir.AddVar(Vars.VariableName.Value,ResultObject);
            }

            foreach (var Import in ParsedUnit.Imports)
            {
                string Dir = Path.GetDirectoryName(AbsolutePath);
                string SubPath = Dir + "/" + Import.Path.Value.Substring(1,Import.Path.Value.Length-2);
                if (File.Exists(SubPath))
                {
                    SubPath = Path.GetFullPath(SubPath);
                    int SubUnitID = -1;
                    if (m_ConvertedUnits.ContainsKey(SubPath))
                    {
                        SubUnitID = m_ConvertedUnits[SubPath];
                    }
                    else
                    {
                        string Text = File.ReadAllText(SubPath);
                        Parser.Parser Parser = new();
                        var Tokenizer = Parser.GetTokenizer();
                        Tokenizer.SetText(Text);
                        List<UnitScript.Diagnostic> Errors = new List<UnitScript.Diagnostic>();
                        Parser.Unit SubUnit = null;
                        try
                        {
                            SubUnit = Parser.ParseUnit(Tokenizer);
                        }
                        catch
                        {
                            OutDiagnostics.Add(new Diagnostic(Import.Path.Position, Import.Path.Value.Length + 2, "Error parsing file"));
                        }
                        List<Diagnostic> SubDiagnostics = new();
                        ConvertUnit(SubDiagnostics, UnitPath, SubUnit);
                        SubUnitID = m_ConvertedUnits[SubPath];
                    }
                    if( Context.ImportedUnits.ContainsKey(Import.Name.Value))
                    {
                        OutDiagnostics.Add(new Diagnostic(Import.Name, "Imported unit with name already exists"));
                    }
                    Context.ImportedUnits[Import.Name.Value] = new RuleManager.UnitIdentifier(SubUnitID);

                }
                else
                {
                    OutDiagnostics.Add(new Diagnostic(Import.Path.Position, Import.Path.Value.Length + 2, "File doesnt exist"));
                }
            }
            int Index = 0;
            ReturnValue.CurrentAbilityID = ParsedUnit.Abilities.Count;
            foreach(Parser.Ability Ability in ParsedUnit.Abilities)
            {
                EvaluationEnvironment NewEnvir = new EvaluationEnvironment();
                NewEnvir.SetParent(ReturnValue.GameInfo.Envir);
                NewEnvir.AddVar("this",new RuleManager.UnitIdentifier(CurrentUnitID));
                AbilityInformation NewAbility = ConvertAbility(Context,NewEnvir,Ability);
                ReturnValue.TotalAbilities[Index] = NewAbility;
                NewAbility.Index = Index;
                ReturnValue.GameInfo.Abilities.Add(NewAbility.Ability);
                Index++;
            }
            foreach(var Ability in ReturnValue.TotalAbilities)
            {
                ReturnValue.GameInfo.TotalAbilities[Ability.Value.Index] = Ability.Value.Ability;
            }
            return ReturnValue;
        }
        public void AddBuiltins(Dictionary<string,Builtin_FuncInfo> FuncsToAdd)
        {
             foreach(KeyValuePair<string,Builtin_FuncInfo> Func in FuncsToAdd)
             {
                 m_BuiltinFuncs[Func.Key] = Func.Value;
             } 
        }
    }
}
