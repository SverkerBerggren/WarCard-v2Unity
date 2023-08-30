using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        public List<Expression> Args;
    }
    public class Expression_List : Expression
    {
        public List<Expression> Contents;
    }
    public class Expression_Literal : Expression
    {
        public object Value;
    }
    public class Expression_Variable : Expression
    {
        public string VarName;
    }

    public class BuiltinFuncArgs
    {
        public List<object> Arguments = new List<object>();
    }
    public class Builtin_FuncInfo
    {
        public Type ResultType;
        public EvalContext ValidContexts = EvalContext.None;
        public List<Type> ArgTypes;//required arguments
        public Dictionary<string, Type> KeyArgTypes;//keyword required types
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
        Dictionary<string,object> m_Contents;
        public bool HasVar(string VariableToCheck)
        {
            return m_Contents.ContainsKey(VariableToCheck);
        }
        public object GetVar(string VariableToGet)
        {
            return m_Contents[VariableToGet];
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


        
        public UnitConverter()
        {
               
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
                foreach(Expression SubExpr in FuncExpr.Args)
                {
                    FuncArgs.Arguments.Add(Eval(Envir,SubExpr));
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
        public Expression ConvertExpression(List<Diagnostic> OutDiagnostics,EvalContext CurrentContext,EvaluationEnvironment Envir ,Parser.Expression ParsedExpression,out Type ResultType)
        {
            Expression ReturnValue = new Expression();
            Type OutType = typeof(void);
            if(ParsedExpression is Parser.Expression_Literal)
            {
                ReturnValue = ConvertExpression(OutDiagnostics, (Parser.Expression_Literal)ParsedExpression, out ResultType);
            }
            else if (ParsedExpression is Parser.Expression_FuncCall)
            {
                Parser.Expression_FuncCall Func = (Parser.Expression_FuncCall)ParsedExpression;
                Expression_FuncCall NewFunc = new Expression_FuncCall();
                List<Expression> ConvertedArgs = new List<Expression>();
                List<Type> ArgTypes = new List<Type>();
                foreach (Parser.Expression Argument in Func.Args)
                {
                    Type ArgType;
                    Expression ArgExpr = ConvertExpression(OutDiagnostics,CurrentContext, Envir,Argument, out ArgType);
                    ConvertedArgs.Add(ArgExpr);
                    ArgTypes.Add(ArgType);
                }
                if (m_BuiltinFuncs.ContainsKey(Func.FuncName.Value))
                {
                    Builtin_FuncInfo FuncInfo = m_BuiltinFuncs[Func.FuncName.Value];
                    for (int i = 0; i < Mathf.Min(ConvertedArgs.Count, FuncInfo.ArgTypes.Count); i++)
                    {
                        if (ArgTypes[i] != FuncInfo.ArgTypes[i])
                        {
                            OutDiagnostics.Add(new Diagnostic(Func.FuncName, "Argument has invalid type: "+FuncInfo.ArgTypes[i].Name + " expected"));
                            OutType = typeof(int);
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
                NewFunc.FuncName = Func.FuncName.Value;
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
            }
            Type OutType  = null;
            ReturnValue.Condition = ConvertExpression(OutDiagnostics,EvalContext.Predicate,Envir,ParsedTarget.Condition,out OutType);
            return ReturnValue;
        }
        RuleManager.TargetInfo ConvertTargets(List<Diagnostic> OutDiagnostics,EvaluationEnvironment Envir,List<Parser.ActivatedAbilityTarget> Targets)
        {
            RuleManager.TargetInfo_UnitScript ReturnValue = null;
            foreach(Parser.ActivatedAbilityTarget  Target in Targets)
            {
                ReturnValue.Targets.Add(ConvertTarget(OutDiagnostics,Envir,Target));
            }
            return ReturnValue;
        }
        public RuleManager.Ability_Activated ConvertActivated(List<Diagnostic> OutDiagnostics,EvaluationEnvironment Envir,Parser.Ability_Activated ParsedAbility)
        {
            RuleManager.Ability_Activated ReturnValue = new RuleManager.Ability_Activated();
            //Hopefully backwards compatible way to implement this, new TargetInfo and Effect that uses this internally
            ReturnValue.ActivatedEffect = ConvertEffect(OutDiagnostics,Envir,ParsedAbility.Statements);
            ReturnValue.ActivationTargets = ConvertTargets(OutDiagnostics,Envir,ParsedAbility.Targets);
            return ReturnValue;
        }
        public RuleManager.Ability ConvertAbility(List<Diagnostic> OutDiagnostics,EvaluationEnvironment Envir,Parser.Ability ParsedAbility)
        {
            RuleManager.Ability ReturnValue = null; 
            if(ParsedAbility is Parser.Ability_Activated)
            {
                ReturnValue = ConvertActivated(OutDiagnostics,Envir,(Parser.Ability_Activated)ParsedAbility);
            }
            return ReturnValue;
        }

        public  ResourceManager.UnitResource ConvertUnit(List<Diagnostic> OutDiagnostics,Parser.Unit ParsedUnit)
        {
            ResourceManager.UnitResource ReturnValue = new ResourceManager.UnitResource();
            ReturnValue.GameInfo.Stats = ConvertStats(OutDiagnostics,ParsedUnit.Stats);
            ReturnValue.UIInfo = ConvertVisuals(OutDiagnostics,ReturnValue.GameInfo.Envir,ParsedUnit.visuals);
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
    }
}
