using System.Collections;
using System.Collections.Generic;
using System;

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
    }
    public class Expression_Variable : Expression
    {
        public string VarName;
    }
    public class ContinousAbility
    {
        public EvaluationEnvironment Envir;
        public RuleManager.Ability_Continous Ability;
    }
    public class Expression_ContinousAbility : Expression
    {
        public RuleManager.Ability_Continous Ability;
    }

    public class BuiltinFuncArgs
    {
        public EvaluationEnvironment Envir;
        public UnitConverter Handler = null;
        public List<object> Arguments = new List<object>();
        public Dictionary<string,object> KeyArguments = new Dictionary<string, object>();
    }
    public class Builtin_FuncInfo
    {
        public Type ResultType;
        public EvalContext ValidContexts = EvalContext.None;
        public List<Type> ArgTypes = new List<Type>();//required arguments
        public Dictionary<string, Type> KeyArgTypes =  new Dictionary<string, Type>();//keyword required types
        public Func<BuiltinFuncArgs,object> Callable;
    }

    public enum EvalContext
    {
        None = 0,
        Compile  = 1<<1,
        Predicate  = 1<<2,
        Resolve  = 1<<3,
    }

    public class EvaluationEnvironment
    {
        Dictionary<string,object> m_Contents = new Dictionary<string,object>();
        EvaluationEnvironment m_Parent = null;
        public bool HasVar(string VariableToCheck)
        {
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
        public object GetVar(string VariableToGet)
        {
            if(m_Contents.ContainsKey(VariableToGet))
            {
                return m_Contents[VariableToGet];
            }
            if(m_Parent != null)
            {
                return m_Parent.GetVar(VariableToGet);
            }
            throw new Exception("Couldn't find variable in environment");
        }
        public void AddVar(string Variable,object Value)
        {
            m_Contents[Variable] = Value;
        }
    }
    public class UnitConverter
    {
        Dictionary<string, Builtin_FuncInfo> m_BuiltinFuncs = new Dictionary<string, Builtin_FuncInfo>();
        HashSet<string> m_ConstexpBuiltins = new HashSet<string>();

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
            Builtin_FuncInfo And = new Builtin_FuncInfo();
            And.ArgTypes = new List<Type>{typeof(bool),typeof(bool)};
            And.ResultType = typeof(bool);
            And.ValidContexts = EvalContext.Compile | EvalContext.Predicate |EvalContext.Resolve;
            And.Callable = p_And;
            m_BuiltinFuncs["&&"] = And;

            Builtin_FuncInfo Or = new Builtin_FuncInfo();
            Or.ArgTypes = new List<Type>{typeof(bool),typeof(bool)};
            Or.ResultType = typeof(bool);
            Or.ValidContexts = EvalContext.Compile | EvalContext.Predicate |EvalContext.Resolve;
            Or.Callable = p_Or;
            m_BuiltinFuncs["||"] = Or;

            Builtin_FuncInfo Not = new Builtin_FuncInfo();
            Not.ArgTypes = new List<Type>{typeof(bool)};
            Not.ResultType = typeof(bool);
            Not.ValidContexts = EvalContext.Compile | EvalContext.Predicate |EvalContext.Resolve;
            Not.Callable = p_Not;
            m_BuiltinFuncs["!"] = Not;

            Builtin_FuncInfo Le = new Builtin_FuncInfo();
            Le.ArgTypes = new List<Type>{typeof(int),typeof(int)};
            Le.ResultType = typeof(bool);
            Le.ValidContexts = EvalContext.Compile | EvalContext.Predicate |EvalContext.Resolve;
            Le.Callable = p_Le;
            m_BuiltinFuncs["<"] = Le;

            Builtin_FuncInfo Leq = new Builtin_FuncInfo();
            Leq.ArgTypes = new List<Type>{typeof(int),typeof(int)};
            Leq.ResultType = typeof(bool);
            Leq.ValidContexts = EvalContext.Compile | EvalContext.Predicate |EvalContext.Resolve;
            Leq.Callable = p_Leq;
            m_BuiltinFuncs["<="] = Leq;

            Builtin_FuncInfo Ge = new Builtin_FuncInfo();
            Ge.ArgTypes = new List<Type>{typeof(int),typeof(int)};
            Ge.ResultType = typeof(bool);
            Ge.ValidContexts = EvalContext.Compile | EvalContext.Predicate |EvalContext.Resolve;
            Ge.Callable = p_Ge;
            m_BuiltinFuncs[">"] = Ge;

            Builtin_FuncInfo Geq = new Builtin_FuncInfo();
            Geq.ArgTypes = new List<Type>{typeof(int),typeof(int)};
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
            else if(Expr is Expression_ContinousAbility)
            {
                Expression_ContinousAbility ContinousLiteral = (Expression_ContinousAbility)Expr;
                ContinousAbility Result = new ContinousAbility();
                Result.Envir = Envir;
                Result.Ability = ContinousLiteral.Ability;
                ReturnValue = null;
            }
            return ReturnValue;
        }

        public Expression ConvertExpression(List<Diagnostic> OutDiagnostics, Parser.Expression_Literal ParsedExpression, out Type ResultType)
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
               NewLiteral.Value = ((Parser.Literal_String)ParsedExpression.literal).Value;
               OutType = typeof(string);
               ReturnValue = NewLiteral;
           }
           else
           {
           }
            ResultType = OutType;
            return ReturnValue;
        }
        public Expression ConvertExpression_Ability(List<Diagnostic> OutDiagnostics,EvalContext CurrentContext,EvaluationEnvironment Envir ,Parser.Expression_Ability ParsedExpression,out Type ResultType)
        {
            Expression ReturnValue = null;
            Type OutType = typeof(void);
            if(ParsedExpression.AbilityLiteral is Parser.Ability_Activated)
            {
                   
            }
            else if(ParsedExpression.AbilityLiteral is Parser.Ability_Continous)
            {

                Expression_ContinousAbility Result = new Expression_ContinousAbility();
                Result.Ability = ConvertContinous(OutDiagnostics,Envir,(Parser.Ability_Continous)ParsedExpression.AbilityLiteral);
                OutType = typeof(ContinousAbility);
            }
            ResultType = OutType;
            return ReturnValue;
        }
        public Expression ConvertExpression(List<Diagnostic> OutDiagnostics,EvalContext CurrentContext,EvaluationEnvironment Envir ,Parser.Expression ParsedExpression,out Type ResultType)
        {
            Expression ReturnValue = new Expression();
            Type OutType = typeof(void);
            if(ParsedExpression is Parser.Expression_Literal)
            {
                return ConvertExpression(OutDiagnostics, (Parser.Expression_Literal)ParsedExpression, out ResultType);
            }
            else if (ParsedExpression is Parser.Expression_FuncCall)
            {
                Parser.Expression_FuncCall Func = (Parser.Expression_FuncCall)ParsedExpression;
                if(Func.FuncName.Value == "")
                {
                    return ConvertExpression(OutDiagnostics,CurrentContext,Envir,Func.Args[0],out ResultType);
                }
                Expression_FuncCall NewFunc = new Expression_FuncCall();
                NewFunc.FuncName = Func.FuncName.Value;
                List<Type> ArgTypes = new List<Type>();
                Dictionary<Parser.Token,Type> KeyArgTypes = new Dictionary<Parser.Token,Type>();
                foreach (Parser.Expression Argument in Func.Args)
                {
                    Type ArgType;
                    Expression ArgExpr = ConvertExpression(OutDiagnostics,CurrentContext, Envir,Argument, out ArgType);
                    NewFunc.Args.Add(ArgExpr);
                    ArgTypes.Add(ArgType);
                }
                foreach (Parser.KeyArg Argument in Func.KeyArgs)
                {
                    Type ArgType;
                    Expression ArgExpr = ConvertExpression(OutDiagnostics,CurrentContext, Envir,Argument.Value, out ArgType);
                    NewFunc.KeyArgs[Argument.Name.Value] = ArgExpr;
                    KeyArgTypes[Argument.Name] = ArgType;
                }
                if (m_BuiltinFuncs.ContainsKey(Func.FuncName.Value))
                {
                    Builtin_FuncInfo FuncInfo = m_BuiltinFuncs[Func.FuncName.Value];
                    for (int i = 0; i < Math.Min(NewFunc.Args.Count, FuncInfo.ArgTypes.Count); i++)
                    {
                        if (ArgTypes[i] != FuncInfo.ArgTypes[i])
                        {
                            OutDiagnostics.Add(new Diagnostic(Func.FuncName, "Argument has invalid type: "+FuncInfo.ArgTypes[i].Name + " expected"));
                            OutType = typeof(int);
                        }
                    }
                    foreach(KeyValuePair<Parser.Token,Type> KeyArg in KeyArgTypes)
                    {
                        if(!FuncInfo.KeyArgTypes.ContainsKey(KeyArg.Key.Value))
                        {
                            OutDiagnostics.Add(new Diagnostic(KeyArg.Key,"Invalid key argument \""+KeyArg.Key.Value+"\""));
                        }
                        else
                        {
                            Type ExpectedType = FuncInfo.KeyArgTypes[KeyArg.Key.Value];
                            if(ExpectedType != KeyArg.Value)
                            {
                                   
                                OutDiagnostics.Add(new Diagnostic(KeyArg.Key,
                                            "Invalid key argument type: "+ExpectedType.Name + " expected, "+KeyArg.Value.Name+ " recieved"));
                            }
                        }
                    }
                    if( (FuncInfo.ValidContexts & CurrentContext) == 0)
                    {
                        OutDiagnostics.Add(new Diagnostic(Func.FuncName,"Cannot call \""+Func.FuncName.Value+"\" in currenct evaluation context"));
                    }
                    OutType = FuncInfo.ResultType;
                }
                else
                {
                    OutDiagnostics.Add(new Diagnostic(Func.FuncName, "No builtin function named \"" + Func.FuncName.Value + "\""));
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
                    OutType = Envir.GetVar(VarToken.Value).GetType();
                    ReturnValue = NewValue;
                }
                else
                {
                    OutDiagnostics.Add(new Diagnostic(VarToken,"No variable found with name \""+VarToken.Value+"\""));
                }
            }
            else if(ParsedExpression is Parser.Expression_Ability)
            {
                return ConvertExpression_Ability(OutDiagnostics,CurrentContext ,Envir,(Parser.Expression_Ability)ParsedExpression,out ResultType);
            }
            ResultType = OutType;
            //ResultType = ResultType;
            return ReturnValue;
        }
        public RuleManager.UnitStats ConvertStats(List<Diagnostic> OutDiagnostics, Parser.UnitStats ParsedUnit)
        {
            RuleManager.UnitStats ReturnValue = new RuleManager.UnitStats();
            foreach(Parser.StatDeclaration Declaration in ParsedUnit.Declarations)
            {
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
                else
                {
                    OutDiagnostics.Add(new Diagnostic(Declaration.Stat,"No stat field named \""+Declaration.Stat.Value+"\""));
                }
            }
            return ReturnValue;
        }
        public ResourceManager.UnitUIInfo ConvertVisuals(List<Diagnostic> OutDiagnostics,EvaluationEnvironment Envir, Parser.Visuals ParsedVisuals)
        {
            ResourceManager.UnitUIInfo ReturnValue = new ResourceManager.UnitUIInfo();
            foreach(Parser.VariableDeclaration Declaration in ParsedVisuals.Declarations)
            {
                int PrevCount = OutDiagnostics.Count;
                Type ResultType = null;
                Expression ExpressionToEvaluate = ConvertExpression(OutDiagnostics,EvalContext.Compile,Envir,Declaration.VariableValue,out ResultType);
                ResourceManager.Animation Result = null;
                try
                {
                    if(ResultType != typeof(ResourceManager.Animation))
                    {
                        OutDiagnostics.Add(new Diagnostic(Declaration.VariableName,"Expression doesn't evaluate to an animation"));
                    }
                    else if(OutDiagnostics.Count == PrevCount)
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
                        OutDiagnostics.Add(new Diagnostic(Declaration.VariableName,"No visual field named \""+Declaration.VariableName.Value+"\""));
                    }
                }
                catch(System.Exception e)
                {
                    OutDiagnostics.Add(new Diagnostic(Declaration.VariableName,"Error evaluationg rhs: "+e.Message));
                }
            }
            return ReturnValue;
        }
        RuleManager.Effect  ConvertEffect(List<Diagnostic> OutDiagnostics,EvaluationEnvironment Envir,List<Parser.AbilityStatement> ParsedAbility)
        {
            RuleManager.Effect_UnitScript ReturnValue = new RuleManager.Effect_UnitScript();
            Expression_List NewEffect = new Expression_List();
            foreach(Parser.AbilityStatement Statement in ParsedAbility)
            {
                if(Statement is Parser.AbilityStatement_Expression)
                {
                    Type OutType = null;
                    NewEffect.Contents.Add(ConvertExpression(OutDiagnostics,EvalContext.Resolve,Envir, ((Parser.AbilityStatement_Expression)Statement).Expr,out OutType));
                }
            }
            ReturnValue.Expr = NewEffect;
            return ReturnValue;
        }
        RuleManager.TargetType StringToType(string Type)
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
            else if(Type == "Player")
            {
                return RuleManager.TargetType.Player;
            }
            return ReturnValue;
        }
        RuleManager.UnitScriptTarget ConvertTarget(List<Diagnostic> OutDiagnostics,EvaluationEnvironment Envir,Parser.ActivatedAbilityTarget ParsedTarget)
        {
            RuleManager.UnitScriptTarget ReturnValue = new RuleManager.UnitScriptTarget();
            ReturnValue.Name = ParsedTarget.Name.Value;
            ReturnValue.Type = StringToType(ParsedTarget.TargetType.TypeIdentifier.Value);
            if(ReturnValue.Type == RuleManager.TargetType.Null)
            {
                OutDiagnostics.Add(new Diagnostic(ParsedTarget.TargetType.TypeIdentifier,"No target type with name \""+ParsedTarget.TargetType.TypeIdentifier.Value+"\""));   
                return ReturnValue;
            }
            //add dummy type to environment
            if(ReturnValue.Type == RuleManager.TargetType.Unit)
            {
                Envir.AddVar(ReturnValue.Name,new RuleManager.UnitIdentifier());
            }
            else if(ReturnValue.Type == RuleManager.TargetType.Tile)
            {
                Envir.AddVar(ReturnValue.Name,new RuleManager.Coordinate());
            }
            Type OutType  = null;
            ReturnValue.Condition = ConvertExpression(OutDiagnostics,EvalContext.Predicate,Envir,ParsedTarget.Condition,out OutType);
            return ReturnValue;
        }
        RuleManager.TargetInfo ConvertTargets(List<Diagnostic> OutDiagnostics,EvaluationEnvironment Envir,List<Parser.ActivatedAbilityTarget> Targets)
        {
            RuleManager.TargetInfo_List ReturnValue = new RuleManager.TargetInfo_List();
            List<RuleManager.UnitScriptTarget> ConvertedTargets = new List<RuleManager.UnitScriptTarget>();
            foreach(Parser.ActivatedAbilityTarget  Target in Targets)
            {
                ConvertedTargets.Add(ConvertTarget(OutDiagnostics,Envir,Target));
            }
            RuleManager.TargetCondition_UnitScript Condition = new RuleManager.TargetCondition_UnitScript();
            Condition.Targets = ConvertedTargets;
            foreach(Parser.ActivatedAbilityTarget  Target in Targets)
            {
                ReturnValue.Targets.Add(Condition);
            }
            return ReturnValue;
        }
        public RuleManager.Ability_Activated ConvertActivated(List<Diagnostic> OutDiagnostics,EvaluationEnvironment Envir,Parser.Ability_Activated ParsedAbility)
        {
            RuleManager.Ability_Activated ReturnValue = new RuleManager.Ability_Activated();
            //Hopefully backwards compatible way to implement this, new TargetInfo and Effect that uses this internally
            ReturnValue.ActivationTargets = ConvertTargets(OutDiagnostics,Envir,ParsedAbility.Targets);
            ReturnValue.ActivatedEffect = ConvertEffect(OutDiagnostics,Envir,ParsedAbility.Statements);
            return ReturnValue;
        }
        public RuleManager.Ability_Continous ConvertContinous(List<Diagnostic> OutDiagnostics,EvaluationEnvironment Envir,Parser.Ability_Continous ParsedAbility)
        {
            RuleManager.Ability_Continous ReturnValue = new RuleManager.Ability_Continous();
            //Hopefully backwards compatible way to implement this, new TargetInfo and Effect that uses this internally
            ReturnValue.AffectedEntities =((RuleManager.TargetInfo_List)  ConvertTargets(OutDiagnostics,Envir,new List<Parser.ActivatedAbilityTarget>{ ParsedAbility.AffectedEntities})).Targets[0];
            ReturnValue.EffectToApply = ConvertEffect(OutDiagnostics,Envir,ParsedAbility.Statements);
            return ReturnValue;
        }
        public RuleManager.Ability ConvertAbility(List<Diagnostic> OutDiagnostics,EvaluationEnvironment Envir,Parser.Ability ParsedAbility)
        {
            RuleManager.Ability ReturnValue = null; 
            //ensure that the "this" pointer is present
            EvaluationEnvironment NewEnvir = new EvaluationEnvironment();
            NewEnvir.AddVar("this",new RuleManager.UnitIdentifier(0));
            NewEnvir.SetParent(Envir);
            if(ParsedAbility is Parser.Ability_Activated)
            {
                ReturnValue = ConvertActivated(OutDiagnostics,Envir,(Parser.Ability_Activated)ParsedAbility);
            }
            else if(ParsedAbility is Parser.Ability_Continous)
            {
                ReturnValue = ConvertContinous(OutDiagnostics,Envir,(Parser.Ability_Continous)ParsedAbility);
            }
            return ReturnValue;
        }

        public  ResourceManager.UnitResource ConvertUnit(List<Diagnostic> OutDiagnostics,Parser.Unit ParsedUnit)
        {
            ResourceManager.UnitResource ReturnValue = new ResourceManager.UnitResource();
            ReturnValue.GameInfo.Stats = ConvertStats(OutDiagnostics,ParsedUnit.Stats);
            ReturnValue.UIInfo = ConvertVisuals(OutDiagnostics,ReturnValue.GameInfo.Envir,ParsedUnit.visuals);
            ReturnValue.Name = ParsedUnit.Name.Value;
            foreach(Parser.VariableDeclaration Vars in ParsedUnit.Variables)
            {
                int ErrCount = OutDiagnostics.Count;
                Type ResultType = null;
                //arbitrary non-allowed value
                object ResultObject = 1f;
                Expression ExprToEvaluate = ConvertExpression(OutDiagnostics,EvalContext.Compile,ReturnValue.GameInfo.Envir,Vars.VariableValue, out ResultType);
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
            foreach(Parser.Ability Ability in ParsedUnit.Abilities)
            {
                ReturnValue.GameInfo.Abilities.Add(ConvertAbility(OutDiagnostics,ReturnValue.GameInfo.Envir,Ability));
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
