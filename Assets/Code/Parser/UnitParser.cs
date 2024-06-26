using System;
using System.Collections.Generic;
namespace Parser
{
    public class Token
    {
        public MBCC.TokenPosition Position = new MBCC.TokenPosition();
        public string Value = "";
        
    }
    public class Expression
    {
        
    }
    public class Type
    {
        public Token TypeIdentifier = new Token();
        
    }
    public class ArgList
    {
        public List<Expression> Args = new List<Expression>();
        
    }
    public class Import
    {
        public MBCC.TokenPosition ImportBegin = new MBCC.TokenPosition();
        public Token Path = new Token();
        public MBCC.TokenPosition AsBegin = new MBCC.TokenPosition();
        public Token Name = new Token();
        
    }
    public class Unit
    {
        public Token Name = new Token();
        public List<Import> Imports = new List<Import>();
        public List<Ability> Abilities = new List<Ability>();
        public List<VariableDeclaration> Variables = new List<VariableDeclaration>();
        public UnitStats Stats = new UnitStats();
        public Visuals visuals = new Visuals();
        
    }
    public class AbilityStatement
    {
        
    }
    public class AbilityStatement_Expression : AbilityStatement
    {
        public Expression Expr = new Expression();
        
    }
    public class AbilityStatement_Assignment : AbilityStatement
    {
        public Token Variable = new Token();
        public Expression Expr = new Expression();
        
    }
    public class Ability
    {
        public List<VariableDeclaration> Attributes = new List<VariableDeclaration>();
        
    }
    public class TargetCondition
    {
        
    }
    public class ActivatedAbilityTarget
    {
        public Type TargetType = new Type();
        public Token Name = new Token();
        public Expression Condition = new Expression();
        public MBCC.TokenPosition RangeBegin = new MBCC.TokenPosition();
        public Expression RangeExpression = new Expression();
        public MBCC.TokenPosition HoverBegin = new MBCC.TokenPosition();
        public Expression HoverExpression = new Expression();
        
    }
    public class Ability_Activated : Ability
    {
        public MBCC.TokenPosition Begin = new MBCC.TokenPosition();
        public Token Name = new Token();
        public List<ActivatedAbilityTarget> Targets = new List<ActivatedAbilityTarget>();
        public List<AbilityStatement> Statements = new List<AbilityStatement>();
        public MBCC.TokenPosition ActCountBegin = new MBCC.TokenPosition();
        public int ActivationCount =  -1;
        
    }
    public class Ability_Continous : Ability
    {
        public MBCC.TokenPosition Begin = new MBCC.TokenPosition();
        public Token Name = new Token();
        public ActivatedAbilityTarget AffectedEntities = new ActivatedAbilityTarget();
        public List<AbilityStatement> Statements = new List<AbilityStatement>();
        
    }
    public class Ability_Triggered : Ability
    {
        public Token Name = new Token();
        public MBCC.TokenPosition TriggeredBegin = new MBCC.TokenPosition();
        public List<Token> TriggerKinds = new List<Token>();
        public List<ActivatedAbilityTarget> Targets = new List<ActivatedAbilityTarget>();
        public List<AbilityStatement> Statements = new List<AbilityStatement>();
        
    }
    public class VariableDeclaration
    {
        public Token VariableName = new Token();
        public Expression VariableValue = new Expression();
        
    }
    public class StatDeclaration
    {
        public Token Stat = new Token();
        public MBCC.TokenPosition ValueBegin = new MBCC.TokenPosition();
        public int AssignedValue =  0;
        
    }
    public class UnitStats
    {
        public MBCC.TokenPosition Position = new MBCC.TokenPosition();
        public List<StatDeclaration> Declarations = new List<StatDeclaration>();
        public List<Token> Tags = new List<Token>();
        
    }
    public class Literal
    {
        public MBCC.TokenPosition Position = new MBCC.TokenPosition();
        
    }
    public class Literal_String : Literal
    {
        public string Value = "";
        
    }
    public class Literal_Int : Literal
    {
        public int Value = new int();
        
    }
    public class Literal_Bool : Literal
    {
        public bool Value = new bool();
        
    }
    public class Expression_Literal : Expression
    {
        public Literal literal = new Literal();
        
    }
    public class Expression_Variable : Expression
    {
        public Token Variable = new Token();
        
    }
    public class Expression_Ability : Expression
    {
        public Ability AbilityLiteral = new Ability();
        
    }
    public class KeyArg
    {
        public Token Name = new Token();
        public Expression Value = new Expression();
        
    }
    public class Expression_FuncCall : Expression
    {
        public Token FuncName = new Token();
        public List<Expression> Args = new List<Expression>();
        public List<KeyArg> KeyArgs = new List<KeyArg>();
        
    }
    public class Expression_StatReference : Expression
    {
        public List<Token> Tokens = new List<Token>();
        
    }
    public class Visuals
    {
        public MBCC.TokenPosition Begin = new MBCC.TokenPosition();
        public List<VariableDeclaration> Declarations = new List<VariableDeclaration>();
        
    }
    public class Parser
    {
        bool[,,] LOOKTable = {{{false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,},},{{false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,},},{{false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,true,false,false,false,true,true,true,true,false,false,false,false,false,false,false,false,false,false,false,true,false,},{true,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,true,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},},{{false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},},{{false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},},{{false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,true,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,true,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,},},{{false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,},},{{false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,true,true,true,false,true,true,false,false,true,false,true,false,false,false,true,false,},},{{false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,true,true,true,false,true,true,false,false,true,false,true,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,true,true,true,false,true,true,false,false,true,false,true,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,true,true,true,false,true,true,false,false,true,false,true,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,true,true,true,false,true,true,false,false,true,false,true,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,true,true,true,false,true,true,false,false,true,false,true,false,false,false,true,false,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,true,true,true,false,true,true,false,false,true,false,true,false,false,false,true,false,},{false,false,true,false,false,false,true,true,false,false,false,false,false,true,true,true,true,true,true,true,true,true,false,true,false,false,false,true,true,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,true,true,true,false,true,true,false,false,true,false,true,false,false,false,true,false,},{false,false,true,false,false,false,true,false,false,false,false,false,false,true,true,true,true,true,true,true,true,true,false,true,false,false,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,true,true,true,false,true,true,false,false,true,false,true,false,false,false,true,false,},{false,false,true,false,false,false,true,false,false,false,false,false,false,true,true,true,true,true,true,true,true,true,false,true,false,false,false,true,true,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,true,true,true,false,true,true,false,false,true,false,true,false,false,false,true,false,},{false,false,true,false,false,false,true,false,false,false,false,false,false,true,true,true,true,true,true,true,true,true,false,true,false,false,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{true,false,true,true,true,true,true,true,false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,false,true,true,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{true,false,true,true,true,true,true,true,false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,false,true,true,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,true,false,},{false,false,true,true,false,true,true,false,false,false,false,false,false,true,true,true,true,true,true,true,true,true,false,true,true,true,false,true,true,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,true,false,},{false,false,true,true,false,true,true,false,false,false,false,false,false,true,true,true,true,true,true,true,true,true,false,true,true,true,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,true,false,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,true,false,},{false,false,true,true,false,true,true,false,false,false,false,false,false,true,true,true,true,true,true,true,true,true,false,true,true,true,false,true,true,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,true,false,},{false,false,true,true,false,true,true,false,false,false,false,false,false,true,true,true,true,true,true,true,true,true,false,true,true,true,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,true,false,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,true,false,},{false,false,true,true,false,true,true,false,false,false,false,false,false,true,true,true,true,true,true,true,true,true,false,true,true,true,false,true,true,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,true,false,},{false,false,true,true,false,true,true,false,false,false,false,false,false,true,true,true,true,true,true,true,true,true,false,true,true,true,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,true,false,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,true,false,},{false,false,true,true,false,true,true,false,false,false,false,false,false,true,true,true,true,true,true,true,true,true,false,true,true,true,false,true,true,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,true,false,},{false,false,true,true,false,true,true,false,false,false,false,false,false,true,true,true,true,true,true,true,true,true,false,true,true,true,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,true,false,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,true,true,true,false,true,true,false,false,true,false,true,false,false,false,true,false,},{false,false,true,true,false,true,true,false,false,false,false,false,false,true,true,true,true,true,true,true,true,true,false,true,true,true,false,true,true,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,true,false,},{false,false,true,true,false,true,true,false,false,false,false,false,false,true,true,true,true,true,true,true,true,true,false,true,true,true,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,true,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,true,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,true,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,true,false,},{false,false,true,true,false,true,true,false,false,false,false,false,false,true,true,true,true,true,true,true,true,true,false,true,true,true,false,true,true,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,true,true,true,false,true,true,false,false,true,false,true,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,false,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,false,false,false,true,false,false,true,true,false,false,false,true,true,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,false,false,false,true,false,false,true,true,false,false,false,true,true,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,false,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,false,false,false,true,false,false,true,true,false,false,false,true,true,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,false,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,false,false,false,true,false,false,true,true,false,false,false,true,true,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,false,false,false,true,false,false,true,true,false,false,false,true,true,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,false,false,false,true,false,false,true,true,false,false,false,true,true,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,true,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,true,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,false,false,false,true,false,false,true,true,false,false,false,true,true,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,false,false,false,true,false,false,true,true,false,false,false,true,true,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,true,false,false,false,false,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,false,false,false,true,false,false,true,true,false,false,false,true,true,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,false,false,false,true,false,false,true,true,false,false,false,true,true,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,false,false,false,true,false,false,true,true,false,false,false,true,true,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,false,false,false,true,false,false,true,true,false,false,false,true,true,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,false,false,false,true,false,false,true,true,false,false,false,true,true,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,false,false,false,true,false,false,true,true,false,false,false,true,true,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,false,false,false,true,false,false,true,true,false,false,false,true,true,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,false,false,false,true,false,false,true,true,false,false,false,true,true,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,false,false,false,true,false,false,true,true,false,false,false,true,true,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,false,false,false,true,false,false,true,true,false,false,false,true,true,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,true,true,true,false,true,true,false,false,true,false,true,false,false,false,true,false,},{false,false,true,true,false,true,false,true,false,false,false,false,false,true,true,true,true,true,true,true,true,true,false,true,false,false,false,true,true,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,true,true,true,false,true,true,false,false,true,false,true,false,false,false,true,false,},{false,false,true,true,false,true,false,false,false,false,false,false,false,true,true,true,true,true,true,true,true,true,false,true,false,false,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,true,true,true,false,true,true,false,false,true,false,true,false,false,false,true,false,},{false,false,true,true,false,true,false,true,false,false,false,false,false,true,true,true,true,true,true,true,true,true,false,true,false,false,false,true,true,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,true,true,true,false,true,true,false,false,true,false,true,false,false,false,true,false,},{false,false,true,true,false,true,false,false,false,false,false,false,false,true,true,true,true,true,true,true,true,true,false,true,false,false,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,true,true,true,false,true,true,false,false,true,false,true,false,false,false,true,false,},},{{false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,true,true,true,false,true,true,false,false,true,false,true,false,false,false,true,false,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,true,true,true,false,true,true,false,false,true,false,true,false,false,false,true,false,},{false,false,true,true,false,true,false,true,false,false,false,false,false,true,true,true,true,true,true,true,true,true,false,true,false,false,false,true,true,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,true,true,true,false,true,true,false,false,true,false,true,false,false,false,true,false,},{false,false,true,true,false,true,false,true,false,false,false,false,false,true,true,true,true,true,true,true,true,true,false,true,false,false,false,true,true,},},};
        public Import ParseImport(MBCC.Tokenizer Tokenizer)
        {
            Import ReturnValue = new Import();
            ReturnValue = ParseImport_0(Tokenizer);
            return(ReturnValue);
        }
        public Import ParseImport_0(MBCC.Tokenizer Tokenizer)
        {
            Import ReturnValue = new Import();
            ReturnValue.ImportBegin = Tokenizer.Peek().Position;
            if(Tokenizer.Peek().Type != 10)
            {
                throw new System.Exception("Error parsing " + "Import" + ": Expected " + "import");
                
            }
            Tokenizer.ConsumeToken();
            ReturnValue.Path.Position = Tokenizer.Peek().Position;
            if(Tokenizer.Peek().Type != 18)
            {
                throw new System.Exception("Error parsing " + "Import" + ": Expected " + "str");
                
            }
            ReturnValue.Path.Value = Tokenizer.Peek().Value;
            Tokenizer.ConsumeToken();
            ReturnValue.AsBegin = Tokenizer.Peek().Position;
            if(Tokenizer.Peek().Type != 11)
            {
                throw new System.Exception("Error parsing " + "Import" + ": Expected " + "as");
                
            }
            Tokenizer.ConsumeToken();
            if(!(LOOKTable[68,0,Tokenizer.Peek().Type]&& LOOKTable[68,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Import" + ": Expected " + "Token");
                
            }
            ReturnValue.Name = ParseToken(Tokenizer);
            return(ReturnValue);
        }
        public Unit ParseUnit(MBCC.Tokenizer Tokenizer)
        {
            Unit ReturnValue = new Unit();
            ReturnValue = ParseUnit_0(Tokenizer);
            return(ReturnValue);
        }
        public Unit ParseUnit_0(MBCC.Tokenizer Tokenizer)
        {
            Unit ReturnValue = new Unit();
            while(LOOKTable[0,0,Tokenizer.Peek().Type]&& LOOKTable[0,1,Tokenizer.Peek(1).Type])
            {
                ReturnValue.Imports.Add(ParseImport(Tokenizer));
                
            }
            if(!(LOOKTable[68,0,Tokenizer.Peek().Type]&& LOOKTable[68,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Unit" + ": Expected " + "Token");
                
            }
            ReturnValue.Name = ParseToken(Tokenizer);
            if(Tokenizer.Peek().Type != 0)
            {
                throw new System.Exception("Error parsing " + "Unit" + ": Expected " + "lcurl");
                
            }
            Tokenizer.ConsumeToken();
            while(LOOKTable[4,0,Tokenizer.Peek().Type]&& LOOKTable[4,1,Tokenizer.Peek(1).Type])
            {
                if (LOOKTable[5,0,Tokenizer.Peek().Type]&& LOOKTable[5,1,Tokenizer.Peek(1).Type])
                {
                    if(!(LOOKTable[18,0,Tokenizer.Peek().Type]&& LOOKTable[18,1,Tokenizer.Peek(1).Type]))
                    {
                        throw new System.Exception("Error parsing " + "_L1" + ": Expected " + "VariableDeclaration");
                        
                    }
                    ReturnValue.Variables.Add(ParseVariableDeclaration(Tokenizer));
                    
                }
                else if (LOOKTable[6,0,Tokenizer.Peek().Type]&& LOOKTable[6,1,Tokenizer.Peek(1).Type])
                {
                    if(!(LOOKTable[9,0,Tokenizer.Peek().Type]&& LOOKTable[9,1,Tokenizer.Peek(1).Type]))
                    {
                        throw new System.Exception("Error parsing " + "_L1" + ": Expected " + "UnitStats");
                        
                    }
                    ReturnValue.Stats = ParseUnitStats(Tokenizer);
                    
                }
                else if (LOOKTable[7,0,Tokenizer.Peek().Type]&& LOOKTable[7,1,Tokenizer.Peek(1).Type])
                {
                    if(!(LOOKTable[22,0,Tokenizer.Peek().Type]&& LOOKTable[22,1,Tokenizer.Peek(1).Type]))
                    {
                        throw new System.Exception("Error parsing " + "_L1" + ": Expected " + "Ability");
                        
                    }
                    ReturnValue.Abilities.Add(ParseAbility(Tokenizer));
                    
                }
                else if (LOOKTable[8,0,Tokenizer.Peek().Type]&& LOOKTable[8,1,Tokenizer.Peek(1).Type])
                {
                    if(!(LOOKTable[70,0,Tokenizer.Peek().Type]&& LOOKTable[70,1,Tokenizer.Peek(1).Type]))
                    {
                        throw new System.Exception("Error parsing " + "_L1" + ": Expected " + "Visuals");
                        
                    }
                    ReturnValue.visuals = ParseVisuals(Tokenizer);
                    
                }
                else
                {
                    throw new System.Exception("Error parsing " + "Unit" + ": Expected " + "Unit");
                    
                }
                
            }
            if(Tokenizer.Peek().Type != 1)
            {
                throw new System.Exception("Error parsing " + "Unit" + ": Expected " + "rcurl");
                
            }
            Tokenizer.ConsumeToken();
            return(ReturnValue);
        }
        public UnitStats ParseUnitStats(MBCC.Tokenizer Tokenizer)
        {
            UnitStats ReturnValue = new UnitStats();
            ReturnValue = ParseUnitStats_0(Tokenizer);
            return(ReturnValue);
        }
        public UnitStats ParseUnitStats_0(MBCC.Tokenizer Tokenizer)
        {
            UnitStats ReturnValue = new UnitStats();
            ReturnValue.Position = Tokenizer.Peek().Position;
            if(Tokenizer.Peek().Type != 8)
            {
                throw new System.Exception("Error parsing " + "UnitStats" + ": Expected " + "stats");
                
            }
            Tokenizer.ConsumeToken();
            if(Tokenizer.Peek().Type != 0)
            {
                throw new System.Exception("Error parsing " + "UnitStats" + ": Expected " + "lcurl");
                
            }
            Tokenizer.ConsumeToken();
            while(LOOKTable[13,0,Tokenizer.Peek().Type]&& LOOKTable[13,1,Tokenizer.Peek(1).Type])
            {
                if (LOOKTable[14,0,Tokenizer.Peek().Type]&& LOOKTable[14,1,Tokenizer.Peek(1).Type])
                {
                    if(!(LOOKTable[16,0,Tokenizer.Peek().Type]&& LOOKTable[16,1,Tokenizer.Peek(1).Type]))
                    {
                        throw new System.Exception("Error parsing " + "_L2" + ": Expected " + "StatDeclaration");
                        
                    }
                    ReturnValue.Declarations.Add(ParseStatDeclaration(Tokenizer));
                    
                }
                else if (LOOKTable[15,0,Tokenizer.Peek().Type]&& LOOKTable[15,1,Tokenizer.Peek(1).Type])
                {
                    if(Tokenizer.Peek().Type != 9)
                    {
                        throw new System.Exception("Error parsing " + "_L2" + ": Expected " + "tags");
                        
                    }
                    Tokenizer.ConsumeToken();
                    if(Tokenizer.Peek().Type != 7)
                    {
                        throw new System.Exception("Error parsing " + "_L2" + ": Expected " + "eq");
                        
                    }
                    Tokenizer.ConsumeToken();
                    if(!(LOOKTable[68,0,Tokenizer.Peek().Type]&& LOOKTable[68,1,Tokenizer.Peek(1).Type]))
                    {
                        throw new System.Exception("Error parsing " + "_L2" + ": Expected " + "Token");
                        
                    }
                    ReturnValue.Tags.Add(ParseToken(Tokenizer));
                    while(LOOKTable[11,0,Tokenizer.Peek().Type]&& LOOKTable[11,1,Tokenizer.Peek(1).Type])
                    {
                        if (LOOKTable[12,0,Tokenizer.Peek().Type]&& LOOKTable[12,1,Tokenizer.Peek(1).Type])
                        {
                            if(Tokenizer.Peek().Type != 5)
                            {
                                throw new System.Exception("Error parsing " + "_L3" + ": Expected " + "comma");
                                
                            }
                            Tokenizer.ConsumeToken();
                            if(!(LOOKTable[68,0,Tokenizer.Peek().Type]&& LOOKTable[68,1,Tokenizer.Peek(1).Type]))
                            {
                                throw new System.Exception("Error parsing " + "_L3" + ": Expected " + "Token");
                                
                            }
                            ReturnValue.Tags.Add(ParseToken(Tokenizer));
                            
                        }
                        else
                        {
                            throw new System.Exception("Error parsing " + "_L2" + ": Expected " + "_L2");
                            
                        }
                        
                    }
                    if(Tokenizer.Peek().Type != 6)
                    {
                        throw new System.Exception("Error parsing " + "_L2" + ": Expected " + "semi");
                        
                    }
                    Tokenizer.ConsumeToken();
                    
                }
                else
                {
                    throw new System.Exception("Error parsing " + "UnitStats" + ": Expected " + "UnitStats");
                    
                }
                
            }
            if(Tokenizer.Peek().Type != 1)
            {
                throw new System.Exception("Error parsing " + "UnitStats" + ": Expected " + "rcurl");
                
            }
            Tokenizer.ConsumeToken();
            return(ReturnValue);
        }
        public StatDeclaration ParseStatDeclaration(MBCC.Tokenizer Tokenizer)
        {
            StatDeclaration ReturnValue = new StatDeclaration();
            ReturnValue = ParseStatDeclaration_0(Tokenizer);
            return(ReturnValue);
        }
        public StatDeclaration ParseStatDeclaration_0(MBCC.Tokenizer Tokenizer)
        {
            StatDeclaration ReturnValue = new StatDeclaration();
            if(!(LOOKTable[68,0,Tokenizer.Peek().Type]&& LOOKTable[68,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "StatDeclaration" + ": Expected " + "Token");
                
            }
            ReturnValue.Stat = ParseToken(Tokenizer);
            if(Tokenizer.Peek().Type != 7)
            {
                throw new System.Exception("Error parsing " + "StatDeclaration" + ": Expected " + "eq");
                
            }
            Tokenizer.ConsumeToken();
            ReturnValue.ValueBegin = Tokenizer.Peek().Position;
            if(Tokenizer.Peek().Type != 17)
            {
                throw new System.Exception("Error parsing " + "StatDeclaration" + ": Expected " + "Int");
                
            }
            ReturnValue.AssignedValue = Int32.Parse(Tokenizer.Peek().Value);
            Tokenizer.ConsumeToken();
            if(Tokenizer.Peek().Type != 6)
            {
                throw new System.Exception("Error parsing " + "StatDeclaration" + ": Expected " + "semi");
                
            }
            Tokenizer.ConsumeToken();
            return(ReturnValue);
        }
        public VariableDeclaration ParseVariableDeclaration(MBCC.Tokenizer Tokenizer)
        {
            VariableDeclaration ReturnValue = new VariableDeclaration();
            ReturnValue = ParseVariableDeclaration_0(Tokenizer);
            return(ReturnValue);
        }
        public VariableDeclaration ParseVariableDeclaration_0(MBCC.Tokenizer Tokenizer)
        {
            VariableDeclaration ReturnValue = new VariableDeclaration();
            if(!(LOOKTable[68,0,Tokenizer.Peek().Type]&& LOOKTable[68,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "VariableDeclaration" + ": Expected " + "Token");
                
            }
            ReturnValue.VariableName = ParseToken(Tokenizer);
            if(Tokenizer.Peek().Type != 7)
            {
                throw new System.Exception("Error parsing " + "VariableDeclaration" + ": Expected " + "eq");
                
            }
            Tokenizer.ConsumeToken();
            if(!(LOOKTable[97,0,Tokenizer.Peek().Type]&& LOOKTable[97,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "VariableDeclaration" + ": Expected " + "Expression");
                
            }
            ReturnValue.VariableValue = ParseExpression(Tokenizer);
            if(Tokenizer.Peek().Type != 6)
            {
                throw new System.Exception("Error parsing " + "VariableDeclaration" + ": Expected " + "semi");
                
            }
            Tokenizer.ConsumeToken();
            return(ReturnValue);
        }
        public Type ParseType(MBCC.Tokenizer Tokenizer)
        {
            Type ReturnValue = new Type();
            ReturnValue = ParseType_0(Tokenizer);
            return(ReturnValue);
        }
        public Type ParseType_0(MBCC.Tokenizer Tokenizer)
        {
            Type ReturnValue = new Type();
            if(!(LOOKTable[68,0,Tokenizer.Peek().Type]&& LOOKTable[68,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Type" + ": Expected " + "Token");
                
            }
            ReturnValue.TypeIdentifier = ParseToken(Tokenizer);
            return(ReturnValue);
        }
        public Ability ParseAbility(MBCC.Tokenizer Tokenizer)
        {
            Ability ReturnValue = new Ability();
            if (LOOKTable[23,0,Tokenizer.Peek().Type]&& LOOKTable[23,1,Tokenizer.Peek(1).Type])
            {
                ReturnValue = ParseAbility_0(Tokenizer);
            }
            else if (LOOKTable[24,0,Tokenizer.Peek().Type]&& LOOKTable[24,1,Tokenizer.Peek(1).Type])
            {
                ReturnValue = ParseAbility_1(Tokenizer);
            }
            else if (LOOKTable[25,0,Tokenizer.Peek().Type]&& LOOKTable[25,1,Tokenizer.Peek(1).Type])
            {
                ReturnValue = ParseAbility_2(Tokenizer);
            }
            else
            {
                throw new System.Exception("Error parsing " + "Ability" + ": Expected " + "Ability");
                
            }
            return(ReturnValue);
        }
        public Ability ParseAbility_0(MBCC.Tokenizer Tokenizer)
        {
            Ability ReturnValue = new Ability();
            if(!(LOOKTable[39,0,Tokenizer.Peek().Type]&& LOOKTable[39,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Ability" + ": Expected " + "Ability_Activated");
                
            }
            ReturnValue = ParseAbility_Activated(Tokenizer);
            return(ReturnValue);
        }
        public Ability ParseAbility_1(MBCC.Tokenizer Tokenizer)
        {
            Ability ReturnValue = new Ability();
            if(!(LOOKTable[49,0,Tokenizer.Peek().Type]&& LOOKTable[49,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Ability" + ": Expected " + "Ability_Continous");
                
            }
            ReturnValue = ParseAbility_Continous(Tokenizer);
            return(ReturnValue);
        }
        public Ability ParseAbility_2(MBCC.Tokenizer Tokenizer)
        {
            Ability ReturnValue = new Ability();
            if(!(LOOKTable[26,0,Tokenizer.Peek().Type]&& LOOKTable[26,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Ability" + ": Expected " + "Ability_Triggered");
                
            }
            ReturnValue = ParseAbility_Triggered(Tokenizer);
            return(ReturnValue);
        }
        public Ability_Triggered ParseAbility_Triggered(MBCC.Tokenizer Tokenizer)
        {
            Ability_Triggered ReturnValue = new Ability_Triggered();
            ReturnValue = ParseAbility_Triggered_0(Tokenizer);
            return(ReturnValue);
        }
        public Ability_Triggered ParseAbility_Triggered_0(MBCC.Tokenizer Tokenizer)
        {
            Ability_Triggered ReturnValue = new Ability_Triggered();
            ReturnValue.TriggeredBegin = Tokenizer.Peek().Position;
            if(Tokenizer.Peek().Type != 14)
            {
                throw new System.Exception("Error parsing " + "Ability_Triggered" + ": Expected " + "triggered");
                
            }
            Tokenizer.ConsumeToken();
            if(!(LOOKTable[68,0,Tokenizer.Peek().Type]&& LOOKTable[68,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Ability_Triggered" + ": Expected " + "Token");
                
            }
            ReturnValue.Name = ParseToken(Tokenizer);
            if (LOOKTable[35,0,Tokenizer.Peek().Type]&& LOOKTable[35,1,Tokenizer.Peek(1).Type])
            {
                if(!(LOOKTable[116,0,Tokenizer.Peek().Type]&& LOOKTable[116,1,Tokenizer.Peek(1).Type]))
                {
                    throw new System.Exception("Error parsing " + "_L4" + ": Expected " + "Token_2");
                    
                }
                ReturnValue.TriggerKinds.Add(ParseToken_2(Tokenizer));
                do
                {
                    if (LOOKTable[29,0,Tokenizer.Peek().Type]&& LOOKTable[29,1,Tokenizer.Peek(1).Type])
                    {
                        if(Tokenizer.Peek().Type != 5)
                        {
                            throw new System.Exception("Error parsing " + "_L5" + ": Expected " + "comma");
                            
                        }
                        Tokenizer.ConsumeToken();
                        if(!(LOOKTable[68,0,Tokenizer.Peek().Type]&& LOOKTable[68,1,Tokenizer.Peek(1).Type]))
                        {
                            throw new System.Exception("Error parsing " + "_L5" + ": Expected " + "Token");
                            
                        }
                        ReturnValue.TriggerKinds.Add(ParseToken(Tokenizer));
                        
                    }
                    else
                    {
                        throw new System.Exception("Error parsing " + "_L4" + ": Expected " + "_L4");
                        
                    }
                    
                }
                while(LOOKTable[28,0,Tokenizer.Peek().Type]&& LOOKTable[28,1,Tokenizer.Peek(1).Type]);
                
            }
            else if (LOOKTable[36,0,Tokenizer.Peek().Type]&& LOOKTable[36,1,Tokenizer.Peek(1).Type])
            {
                if(!(LOOKTable[118,0,Tokenizer.Peek().Type]&& LOOKTable[118,1,Tokenizer.Peek(1).Type]))
                {
                    throw new System.Exception("Error parsing " + "_L4" + ": Expected " + "Token_3");
                    
                }
                ReturnValue.TriggerKinds.Add(ParseToken_3(Tokenizer));
                if(Tokenizer.Peek().Type != 2)
                {
                    throw new System.Exception("Error parsing " + "_L4" + ": Expected " + "lpar");
                    
                }
                Tokenizer.ConsumeToken();
                if(LOOKTable[32,0,Tokenizer.Peek().Type]&& LOOKTable[32,1,Tokenizer.Peek(1).Type])
                {
                    if (LOOKTable[33,0,Tokenizer.Peek().Type]&& LOOKTable[33,1,Tokenizer.Peek(1).Type])
                    {
                        if(!(LOOKTable[53,0,Tokenizer.Peek().Type]&& LOOKTable[53,1,Tokenizer.Peek(1).Type]))
                        {
                            throw new System.Exception("Error parsing " + "_L6" + ": Expected " + "ActivatedAbilityTarget");
                            
                        }
                        ReturnValue.Targets.Add(ParseActivatedAbilityTarget(Tokenizer));
                        while(LOOKTable[30,0,Tokenizer.Peek().Type]&& LOOKTable[30,1,Tokenizer.Peek(1).Type])
                        {
                            if (LOOKTable[31,0,Tokenizer.Peek().Type]&& LOOKTable[31,1,Tokenizer.Peek(1).Type])
                            {
                                if(Tokenizer.Peek().Type != 5)
                                {
                                    throw new System.Exception("Error parsing " + "_L7" + ": Expected " + "comma");
                                    
                                }
                                Tokenizer.ConsumeToken();
                                if(!(LOOKTable[53,0,Tokenizer.Peek().Type]&& LOOKTable[53,1,Tokenizer.Peek(1).Type]))
                                {
                                    throw new System.Exception("Error parsing " + "_L7" + ": Expected " + "ActivatedAbilityTarget");
                                    
                                }
                                ReturnValue.Targets.Add(ParseActivatedAbilityTarget(Tokenizer));
                                
                            }
                            else
                            {
                                throw new System.Exception("Error parsing " + "_L6" + ": Expected " + "_L6");
                                
                            }
                            
                        }
                        
                    }
                    else
                    {
                        throw new System.Exception("Error parsing " + "_L4" + ": Expected " + "_L4");
                        
                    }
                    
                }
                if(Tokenizer.Peek().Type != 3)
                {
                    throw new System.Exception("Error parsing " + "_L4" + ": Expected " + "rpar");
                    
                }
                Tokenizer.ConsumeToken();
                
            }
            else
            {
                throw new System.Exception("Error parsing " + "Ability_Triggered" + ": Expected " + "Ability_Triggered");
                
            }
            if(LOOKTable[37,0,Tokenizer.Peek().Type]&& LOOKTable[37,1,Tokenizer.Peek(1).Type])
            {
                if (LOOKTable[38,0,Tokenizer.Peek().Type]&& LOOKTable[38,1,Tokenizer.Peek(1).Type])
                {
                    if(Tokenizer.Peek().Type != 4)
                    {
                        throw new System.Exception("Error parsing " + "_L8" + ": Expected " + "colon");
                        
                    }
                    Tokenizer.ConsumeToken();
                    do
                    {
                        if(!(LOOKTable[18,0,Tokenizer.Peek().Type]&& LOOKTable[18,1,Tokenizer.Peek(1).Type]))
                        {
                            throw new System.Exception("Error parsing " + "_L8" + ": Expected " + "VariableDeclaration");
                            
                        }
                        ReturnValue.Attributes.Add(ParseVariableDeclaration(Tokenizer));
                        
                    }
                    while(LOOKTable[18,0,Tokenizer.Peek().Type]&& LOOKTable[18,1,Tokenizer.Peek(1).Type]);
                    
                }
                else
                {
                    throw new System.Exception("Error parsing " + "Ability_Triggered" + ": Expected " + "Ability_Triggered");
                    
                }
                
            }
            if(Tokenizer.Peek().Type != 0)
            {
                throw new System.Exception("Error parsing " + "Ability_Triggered" + ": Expected " + "lcurl");
                
            }
            Tokenizer.ConsumeToken();
            while(LOOKTable[61,0,Tokenizer.Peek().Type]&& LOOKTable[61,1,Tokenizer.Peek(1).Type])
            {
                ReturnValue.Statements.Add(ParseAbilityStatement(Tokenizer));
                
            }
            if(Tokenizer.Peek().Type != 1)
            {
                throw new System.Exception("Error parsing " + "Ability_Triggered" + ": Expected " + "rcurl");
                
            }
            Tokenizer.ConsumeToken();
            return(ReturnValue);
        }
        public Ability_Activated ParseAbility_Activated(MBCC.Tokenizer Tokenizer)
        {
            Ability_Activated ReturnValue = new Ability_Activated();
            ReturnValue = ParseAbility_Activated_0(Tokenizer);
            return(ReturnValue);
        }
        public Ability_Activated ParseAbility_Activated_0(MBCC.Tokenizer Tokenizer)
        {
            Ability_Activated ReturnValue = new Ability_Activated();
            ReturnValue.Begin = Tokenizer.Peek().Position;
            if(Tokenizer.Peek().Type != 13)
            {
                throw new System.Exception("Error parsing " + "Ability_Activated" + ": Expected " + "activated");
                
            }
            Tokenizer.ConsumeToken();
            if(!(LOOKTable[68,0,Tokenizer.Peek().Type]&& LOOKTable[68,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Ability_Activated" + ": Expected " + "Token");
                
            }
            ReturnValue.Name = ParseToken(Tokenizer);
            if(Tokenizer.Peek().Type != 2)
            {
                throw new System.Exception("Error parsing " + "Ability_Activated" + ": Expected " + "lpar");
                
            }
            Tokenizer.ConsumeToken();
            if(LOOKTable[43,0,Tokenizer.Peek().Type]&& LOOKTable[43,1,Tokenizer.Peek(1).Type])
            {
                if (LOOKTable[44,0,Tokenizer.Peek().Type]&& LOOKTable[44,1,Tokenizer.Peek(1).Type])
                {
                    if(!(LOOKTable[53,0,Tokenizer.Peek().Type]&& LOOKTable[53,1,Tokenizer.Peek(1).Type]))
                    {
                        throw new System.Exception("Error parsing " + "_L9" + ": Expected " + "ActivatedAbilityTarget");
                        
                    }
                    ReturnValue.Targets.Add(ParseActivatedAbilityTarget(Tokenizer));
                    while(LOOKTable[41,0,Tokenizer.Peek().Type]&& LOOKTable[41,1,Tokenizer.Peek(1).Type])
                    {
                        ReturnValue.Targets.Add(Parse_L10(Tokenizer));
                        
                    }
                    
                }
                else
                {
                    throw new System.Exception("Error parsing " + "Ability_Activated" + ": Expected " + "Ability_Activated");
                    
                }
                
            }
            if(Tokenizer.Peek().Type != 3)
            {
                throw new System.Exception("Error parsing " + "Ability_Activated" + ": Expected " + "rpar");
                
            }
            Tokenizer.ConsumeToken();
            if(LOOKTable[45,0,Tokenizer.Peek().Type]&& LOOKTable[45,1,Tokenizer.Peek(1).Type])
            {
                if (LOOKTable[46,0,Tokenizer.Peek().Type]&& LOOKTable[46,1,Tokenizer.Peek(1).Type])
                {
                    ReturnValue.ActCountBegin = Tokenizer.Peek().Position;
                    if(Tokenizer.Peek().Type != 26)
                    {
                        throw new System.Exception("Error parsing " + "_L11" + ": Expected " + "actCount");
                        
                    }
                    Tokenizer.ConsumeToken();
                    if(Tokenizer.Peek().Type != 17)
                    {
                        throw new System.Exception("Error parsing " + "_L11" + ": Expected " + "Int");
                        
                    }
                    ReturnValue.ActivationCount = Int32.Parse(Tokenizer.Peek().Value);
                    Tokenizer.ConsumeToken();
                    
                }
                else
                {
                    throw new System.Exception("Error parsing " + "Ability_Activated" + ": Expected " + "Ability_Activated");
                    
                }
                
            }
            if(LOOKTable[47,0,Tokenizer.Peek().Type]&& LOOKTable[47,1,Tokenizer.Peek(1).Type])
            {
                if (LOOKTable[48,0,Tokenizer.Peek().Type]&& LOOKTable[48,1,Tokenizer.Peek(1).Type])
                {
                    if(Tokenizer.Peek().Type != 4)
                    {
                        throw new System.Exception("Error parsing " + "_L12" + ": Expected " + "colon");
                        
                    }
                    Tokenizer.ConsumeToken();
                    do
                    {
                        if(!(LOOKTable[18,0,Tokenizer.Peek().Type]&& LOOKTable[18,1,Tokenizer.Peek(1).Type]))
                        {
                            throw new System.Exception("Error parsing " + "_L12" + ": Expected " + "VariableDeclaration");
                            
                        }
                        ReturnValue.Attributes.Add(ParseVariableDeclaration(Tokenizer));
                        
                    }
                    while(LOOKTable[18,0,Tokenizer.Peek().Type]&& LOOKTable[18,1,Tokenizer.Peek(1).Type]);
                    
                }
                else
                {
                    throw new System.Exception("Error parsing " + "Ability_Activated" + ": Expected " + "Ability_Activated");
                    
                }
                
            }
            if(Tokenizer.Peek().Type != 0)
            {
                throw new System.Exception("Error parsing " + "Ability_Activated" + ": Expected " + "lcurl");
                
            }
            Tokenizer.ConsumeToken();
            while(LOOKTable[61,0,Tokenizer.Peek().Type]&& LOOKTable[61,1,Tokenizer.Peek(1).Type])
            {
                ReturnValue.Statements.Add(ParseAbilityStatement(Tokenizer));
                
            }
            if(Tokenizer.Peek().Type != 1)
            {
                throw new System.Exception("Error parsing " + "Ability_Activated" + ": Expected " + "rcurl");
                
            }
            Tokenizer.ConsumeToken();
            return(ReturnValue);
        }
        public ActivatedAbilityTarget Parse_L10(MBCC.Tokenizer Tokenizer)
        {
            ActivatedAbilityTarget ReturnValue = new ActivatedAbilityTarget();
            ReturnValue = Parse_L10_0(Tokenizer);
            return(ReturnValue);
        }
        public ActivatedAbilityTarget Parse_L10_0(MBCC.Tokenizer Tokenizer)
        {
            ActivatedAbilityTarget ReturnValue = new ActivatedAbilityTarget();
            if(Tokenizer.Peek().Type != 5)
            {
                throw new System.Exception("Error parsing " + "_L10" + ": Expected " + "comma");
                
            }
            Tokenizer.ConsumeToken();
            if(!(LOOKTable[53,0,Tokenizer.Peek().Type]&& LOOKTable[53,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "_L10" + ": Expected " + "ActivatedAbilityTarget");
                
            }
            ReturnValue = ParseActivatedAbilityTarget(Tokenizer);
            return(ReturnValue);
        }
        public Ability_Continous ParseAbility_Continous(MBCC.Tokenizer Tokenizer)
        {
            Ability_Continous ReturnValue = new Ability_Continous();
            ReturnValue = ParseAbility_Continous_0(Tokenizer);
            return(ReturnValue);
        }
        public Ability_Continous ParseAbility_Continous_0(MBCC.Tokenizer Tokenizer)
        {
            Ability_Continous ReturnValue = new Ability_Continous();
            ReturnValue.Begin = Tokenizer.Peek().Position;
            if(Tokenizer.Peek().Type != 15)
            {
                throw new System.Exception("Error parsing " + "Ability_Continous" + ": Expected " + "continous");
                
            }
            Tokenizer.ConsumeToken();
            if(!(LOOKTable[68,0,Tokenizer.Peek().Type]&& LOOKTable[68,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Ability_Continous" + ": Expected " + "Token");
                
            }
            ReturnValue.Name = ParseToken(Tokenizer);
            if(Tokenizer.Peek().Type != 2)
            {
                throw new System.Exception("Error parsing " + "Ability_Continous" + ": Expected " + "lpar");
                
            }
            Tokenizer.ConsumeToken();
            if(!(LOOKTable[53,0,Tokenizer.Peek().Type]&& LOOKTable[53,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Ability_Continous" + ": Expected " + "ActivatedAbilityTarget");
                
            }
            ReturnValue.AffectedEntities = ParseActivatedAbilityTarget(Tokenizer);
            if(Tokenizer.Peek().Type != 3)
            {
                throw new System.Exception("Error parsing " + "Ability_Continous" + ": Expected " + "rpar");
                
            }
            Tokenizer.ConsumeToken();
            if(LOOKTable[51,0,Tokenizer.Peek().Type]&& LOOKTable[51,1,Tokenizer.Peek(1).Type])
            {
                if (LOOKTable[52,0,Tokenizer.Peek().Type]&& LOOKTable[52,1,Tokenizer.Peek(1).Type])
                {
                    if(Tokenizer.Peek().Type != 4)
                    {
                        throw new System.Exception("Error parsing " + "_L13" + ": Expected " + "colon");
                        
                    }
                    Tokenizer.ConsumeToken();
                    do
                    {
                        if(!(LOOKTable[18,0,Tokenizer.Peek().Type]&& LOOKTable[18,1,Tokenizer.Peek(1).Type]))
                        {
                            throw new System.Exception("Error parsing " + "_L13" + ": Expected " + "VariableDeclaration");
                            
                        }
                        ReturnValue.Attributes.Add(ParseVariableDeclaration(Tokenizer));
                        
                    }
                    while(LOOKTable[18,0,Tokenizer.Peek().Type]&& LOOKTable[18,1,Tokenizer.Peek(1).Type]);
                    
                }
                else
                {
                    throw new System.Exception("Error parsing " + "Ability_Continous" + ": Expected " + "Ability_Continous");
                    
                }
                
            }
            if(Tokenizer.Peek().Type != 0)
            {
                throw new System.Exception("Error parsing " + "Ability_Continous" + ": Expected " + "lcurl");
                
            }
            Tokenizer.ConsumeToken();
            while(LOOKTable[61,0,Tokenizer.Peek().Type]&& LOOKTable[61,1,Tokenizer.Peek(1).Type])
            {
                ReturnValue.Statements.Add(ParseAbilityStatement(Tokenizer));
                
            }
            if(Tokenizer.Peek().Type != 1)
            {
                throw new System.Exception("Error parsing " + "Ability_Continous" + ": Expected " + "rcurl");
                
            }
            Tokenizer.ConsumeToken();
            return(ReturnValue);
        }
        public ActivatedAbilityTarget ParseActivatedAbilityTarget(MBCC.Tokenizer Tokenizer)
        {
            ActivatedAbilityTarget ReturnValue = new ActivatedAbilityTarget();
            ReturnValue = ParseActivatedAbilityTarget_0(Tokenizer);
            return(ReturnValue);
        }
        public ActivatedAbilityTarget ParseActivatedAbilityTarget_0(MBCC.Tokenizer Tokenizer)
        {
            ActivatedAbilityTarget ReturnValue = new ActivatedAbilityTarget();
            if(!(LOOKTable[20,0,Tokenizer.Peek().Type]&& LOOKTable[20,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "ActivatedAbilityTarget" + ": Expected " + "Type");
                
            }
            ReturnValue.TargetType = ParseType(Tokenizer);
            if(!(LOOKTable[68,0,Tokenizer.Peek().Type]&& LOOKTable[68,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "ActivatedAbilityTarget" + ": Expected " + "Token");
                
            }
            ReturnValue.Name = ParseToken(Tokenizer);
            if(LOOKTable[55,0,Tokenizer.Peek().Type]&& LOOKTable[55,1,Tokenizer.Peek(1).Type])
            {
                if (LOOKTable[56,0,Tokenizer.Peek().Type]&& LOOKTable[56,1,Tokenizer.Peek(1).Type])
                {
                    if(Tokenizer.Peek().Type != 4)
                    {
                        throw new System.Exception("Error parsing " + "_L14" + ": Expected " + "colon");
                        
                    }
                    Tokenizer.ConsumeToken();
                    if(!(LOOKTable[97,0,Tokenizer.Peek().Type]&& LOOKTable[97,1,Tokenizer.Peek(1).Type]))
                    {
                        throw new System.Exception("Error parsing " + "_L14" + ": Expected " + "Expression");
                        
                    }
                    ReturnValue.Condition = ParseExpression(Tokenizer);
                    
                }
                else
                {
                    throw new System.Exception("Error parsing " + "ActivatedAbilityTarget" + ": Expected " + "ActivatedAbilityTarget");
                    
                }
                
            }
            if(LOOKTable[57,0,Tokenizer.Peek().Type]&& LOOKTable[57,1,Tokenizer.Peek(1).Type])
            {
                if (LOOKTable[58,0,Tokenizer.Peek().Type]&& LOOKTable[58,1,Tokenizer.Peek(1).Type])
                {
                    ReturnValue.RangeBegin = Tokenizer.Peek().Position;
                    if(Tokenizer.Peek().Type != 24)
                    {
                        throw new System.Exception("Error parsing " + "_L15" + ": Expected " + "range");
                        
                    }
                    Tokenizer.ConsumeToken();
                    if(!(LOOKTable[97,0,Tokenizer.Peek().Type]&& LOOKTable[97,1,Tokenizer.Peek(1).Type]))
                    {
                        throw new System.Exception("Error parsing " + "_L15" + ": Expected " + "Expression");
                        
                    }
                    ReturnValue.RangeExpression = ParseExpression(Tokenizer);
                    
                }
                else
                {
                    throw new System.Exception("Error parsing " + "ActivatedAbilityTarget" + ": Expected " + "ActivatedAbilityTarget");
                    
                }
                
            }
            if(LOOKTable[59,0,Tokenizer.Peek().Type]&& LOOKTable[59,1,Tokenizer.Peek(1).Type])
            {
                if (LOOKTable[60,0,Tokenizer.Peek().Type]&& LOOKTable[60,1,Tokenizer.Peek(1).Type])
                {
                    ReturnValue.HoverBegin = Tokenizer.Peek().Position;
                    if(Tokenizer.Peek().Type != 25)
                    {
                        throw new System.Exception("Error parsing " + "_L16" + ": Expected " + "hover");
                        
                    }
                    Tokenizer.ConsumeToken();
                    if(!(LOOKTable[97,0,Tokenizer.Peek().Type]&& LOOKTable[97,1,Tokenizer.Peek(1).Type]))
                    {
                        throw new System.Exception("Error parsing " + "_L16" + ": Expected " + "Expression");
                        
                    }
                    ReturnValue.HoverExpression = ParseExpression(Tokenizer);
                    
                }
                else
                {
                    throw new System.Exception("Error parsing " + "ActivatedAbilityTarget" + ": Expected " + "ActivatedAbilityTarget");
                    
                }
                
            }
            return(ReturnValue);
        }
        public AbilityStatement ParseAbilityStatement(MBCC.Tokenizer Tokenizer)
        {
            AbilityStatement ReturnValue = new AbilityStatement();
            if (LOOKTable[62,0,Tokenizer.Peek().Type]&& LOOKTable[62,1,Tokenizer.Peek(1).Type])
            {
                ReturnValue = ParseAbilityStatement_0(Tokenizer);
            }
            else if (LOOKTable[63,0,Tokenizer.Peek().Type]&& LOOKTable[63,1,Tokenizer.Peek(1).Type])
            {
                ReturnValue = ParseAbilityStatement_1(Tokenizer);
            }
            else
            {
                throw new System.Exception("Error parsing " + "AbilityStatement" + ": Expected " + "AbilityStatement");
                
            }
            return(ReturnValue);
        }
        public AbilityStatement ParseAbilityStatement_0(MBCC.Tokenizer Tokenizer)
        {
            AbilityStatement ReturnValue = new AbilityStatement();
            if(!(LOOKTable[64,0,Tokenizer.Peek().Type]&& LOOKTable[64,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "AbilityStatement" + ": Expected " + "AbilityStatement_Expression");
                
            }
            ReturnValue = ParseAbilityStatement_Expression(Tokenizer);
            if(Tokenizer.Peek().Type != 6)
            {
                throw new System.Exception("Error parsing " + "AbilityStatement" + ": Expected " + "semi");
                
            }
            Tokenizer.ConsumeToken();
            return(ReturnValue);
        }
        public AbilityStatement ParseAbilityStatement_1(MBCC.Tokenizer Tokenizer)
        {
            AbilityStatement ReturnValue = new AbilityStatement();
            if(!(LOOKTable[66,0,Tokenizer.Peek().Type]&& LOOKTable[66,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "AbilityStatement" + ": Expected " + "AbilityStatement_Assignment");
                
            }
            ReturnValue = ParseAbilityStatement_Assignment(Tokenizer);
            if(Tokenizer.Peek().Type != 6)
            {
                throw new System.Exception("Error parsing " + "AbilityStatement" + ": Expected " + "semi");
                
            }
            Tokenizer.ConsumeToken();
            return(ReturnValue);
        }
        public AbilityStatement_Expression ParseAbilityStatement_Expression(MBCC.Tokenizer Tokenizer)
        {
            AbilityStatement_Expression ReturnValue = new AbilityStatement_Expression();
            ReturnValue = ParseAbilityStatement_Expression_0(Tokenizer);
            return(ReturnValue);
        }
        public AbilityStatement_Expression ParseAbilityStatement_Expression_0(MBCC.Tokenizer Tokenizer)
        {
            AbilityStatement_Expression ReturnValue = new AbilityStatement_Expression();
            if(!(LOOKTable[97,0,Tokenizer.Peek().Type]&& LOOKTable[97,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "AbilityStatement_Expression" + ": Expected " + "Expression");
                
            }
            ReturnValue.Expr = ParseExpression(Tokenizer);
            return(ReturnValue);
        }
        public AbilityStatement_Assignment ParseAbilityStatement_Assignment(MBCC.Tokenizer Tokenizer)
        {
            AbilityStatement_Assignment ReturnValue = new AbilityStatement_Assignment();
            ReturnValue = ParseAbilityStatement_Assignment_0(Tokenizer);
            return(ReturnValue);
        }
        public AbilityStatement_Assignment ParseAbilityStatement_Assignment_0(MBCC.Tokenizer Tokenizer)
        {
            AbilityStatement_Assignment ReturnValue = new AbilityStatement_Assignment();
            if(!(LOOKTable[68,0,Tokenizer.Peek().Type]&& LOOKTable[68,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "AbilityStatement_Assignment" + ": Expected " + "Token");
                
            }
            ReturnValue.Variable = ParseToken(Tokenizer);
            if(Tokenizer.Peek().Type != 7)
            {
                throw new System.Exception("Error parsing " + "AbilityStatement_Assignment" + ": Expected " + "eq");
                
            }
            Tokenizer.ConsumeToken();
            if(!(LOOKTable[97,0,Tokenizer.Peek().Type]&& LOOKTable[97,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "AbilityStatement_Assignment" + ": Expected " + "Expression");
                
            }
            ReturnValue.Expr = ParseExpression(Tokenizer);
            return(ReturnValue);
        }
        public Token ParseToken(MBCC.Tokenizer Tokenizer)
        {
            Token ReturnValue = new Token();
            ReturnValue = ParseToken_0(Tokenizer);
            return(ReturnValue);
        }
        public Token ParseToken_0(MBCC.Tokenizer Tokenizer)
        {
            Token ReturnValue = new Token();
            ReturnValue.Position = Tokenizer.Peek().Position;
            if(Tokenizer.Peek().Type != 27)
            {
                throw new System.Exception("Error parsing " + "Token" + ": Expected " + "idf");
                
            }
            ReturnValue.Value = Tokenizer.Peek().Value;
            Tokenizer.ConsumeToken();
            return(ReturnValue);
        }
        public Visuals ParseVisuals(MBCC.Tokenizer Tokenizer)
        {
            Visuals ReturnValue = new Visuals();
            ReturnValue = ParseVisuals_0(Tokenizer);
            return(ReturnValue);
        }
        public Visuals ParseVisuals_0(MBCC.Tokenizer Tokenizer)
        {
            Visuals ReturnValue = new Visuals();
            ReturnValue.Begin = Tokenizer.Peek().Position;
            if(Tokenizer.Peek().Type != 12)
            {
                throw new System.Exception("Error parsing " + "Visuals" + ": Expected " + "visuals");
                
            }
            Tokenizer.ConsumeToken();
            if(Tokenizer.Peek().Type != 0)
            {
                throw new System.Exception("Error parsing " + "Visuals" + ": Expected " + "lcurl");
                
            }
            Tokenizer.ConsumeToken();
            while(LOOKTable[18,0,Tokenizer.Peek().Type]&& LOOKTable[18,1,Tokenizer.Peek(1).Type])
            {
                ReturnValue.Declarations.Add(ParseVariableDeclaration(Tokenizer));
                
            }
            if(Tokenizer.Peek().Type != 1)
            {
                throw new System.Exception("Error parsing " + "Visuals" + ": Expected " + "rcurl");
                
            }
            Tokenizer.ConsumeToken();
            return(ReturnValue);
        }
        public Expression_FuncCall ParseExpressionOpOr(MBCC.Tokenizer Tokenizer)
        {
            Expression_FuncCall ReturnValue = new Expression_FuncCall();
            ReturnValue = ParseExpressionOpOr_0(Tokenizer);
            return(ReturnValue);
        }
        public Expression_FuncCall ParseExpressionOpOr_0(MBCC.Tokenizer Tokenizer)
        {
            Expression_FuncCall ReturnValue = new Expression_FuncCall();
            if(!(LOOKTable[93,0,Tokenizer.Peek().Type]&& LOOKTable[93,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "ExpressionOpOr" + ": Expected " + "Expression_Base");
                
            }
            ReturnValue.Args.Add(ParseExpression_Base(Tokenizer));
            if(LOOKTable[76,0,Tokenizer.Peek().Type]&& LOOKTable[76,1,Tokenizer.Peek(1).Type])
            {
                if (LOOKTable[77,0,Tokenizer.Peek().Type]&& LOOKTable[77,1,Tokenizer.Peek(1).Type])
                {
                    if(Tokenizer.Peek().Type != 19)
                    {
                        throw new System.Exception("Error parsing " + "_L17" + ": Expected " + "or");
                        
                    }
                    Tokenizer.ConsumeToken();
                    ReturnValue.FuncName.Value = "||";
                    if(!(LOOKTable[93,0,Tokenizer.Peek().Type]&& LOOKTable[93,1,Tokenizer.Peek(1).Type]))
                    {
                        throw new System.Exception("Error parsing " + "_L17" + ": Expected " + "Expression_Base");
                        
                    }
                    ReturnValue.Args.Add(ParseExpression_Base(Tokenizer));
                    while(LOOKTable[74,0,Tokenizer.Peek().Type]&& LOOKTable[74,1,Tokenizer.Peek(1).Type])
                    {
                        ReturnValue.Args.Add(Parse_L18(Tokenizer));
                        
                    }
                    
                }
                else
                {
                    throw new System.Exception("Error parsing " + "ExpressionOpOr" + ": Expected " + "ExpressionOpOr");
                    
                }
                
            }
            return(ReturnValue);
        }
        public Expression Parse_L18(MBCC.Tokenizer Tokenizer)
        {
            Expression ReturnValue = new Expression();
            ReturnValue = Parse_L18_0(Tokenizer);
            return(ReturnValue);
        }
        public Expression Parse_L18_0(MBCC.Tokenizer Tokenizer)
        {
            Expression ReturnValue = new Expression();
            if(Tokenizer.Peek().Type != 19)
            {
                throw new System.Exception("Error parsing " + "_L18" + ": Expected " + "or");
                
            }
            Tokenizer.ConsumeToken();
            if(!(LOOKTable[78,0,Tokenizer.Peek().Type]&& LOOKTable[78,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "_L18" + ": Expected " + "ExpressionOpAnd");
                
            }
            ReturnValue = ParseExpressionOpAnd(Tokenizer);
            return(ReturnValue);
        }
        public Expression_FuncCall ParseExpressionOpAnd(MBCC.Tokenizer Tokenizer)
        {
            Expression_FuncCall ReturnValue = new Expression_FuncCall();
            ReturnValue = ParseExpressionOpAnd_0(Tokenizer);
            return(ReturnValue);
        }
        public Expression_FuncCall ParseExpressionOpAnd_0(MBCC.Tokenizer Tokenizer)
        {
            Expression_FuncCall ReturnValue = new Expression_FuncCall();
            if(!(LOOKTable[93,0,Tokenizer.Peek().Type]&& LOOKTable[93,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "ExpressionOpAnd" + ": Expected " + "Expression_Base");
                
            }
            ReturnValue.Args.Add(ParseExpression_Base(Tokenizer));
            if(LOOKTable[82,0,Tokenizer.Peek().Type]&& LOOKTable[82,1,Tokenizer.Peek(1).Type])
            {
                if (LOOKTable[83,0,Tokenizer.Peek().Type]&& LOOKTable[83,1,Tokenizer.Peek(1).Type])
                {
                    if(Tokenizer.Peek().Type != 20)
                    {
                        throw new System.Exception("Error parsing " + "_L19" + ": Expected " + "and");
                        
                    }
                    Tokenizer.ConsumeToken();
                    ReturnValue.FuncName.Value = "&&";
                    if(!(LOOKTable[93,0,Tokenizer.Peek().Type]&& LOOKTable[93,1,Tokenizer.Peek(1).Type]))
                    {
                        throw new System.Exception("Error parsing " + "_L19" + ": Expected " + "Expression_Base");
                        
                    }
                    ReturnValue.Args.Add(ParseExpression_Base(Tokenizer));
                    while(LOOKTable[80,0,Tokenizer.Peek().Type]&& LOOKTable[80,1,Tokenizer.Peek(1).Type])
                    {
                        ReturnValue.Args.Add(Parse_L20(Tokenizer));
                        
                    }
                    
                }
                else
                {
                    throw new System.Exception("Error parsing " + "ExpressionOpAnd" + ": Expected " + "ExpressionOpAnd");
                    
                }
                
            }
            return(ReturnValue);
        }
        public Expression Parse_L20(MBCC.Tokenizer Tokenizer)
        {
            Expression ReturnValue = new Expression();
            ReturnValue = Parse_L20_0(Tokenizer);
            return(ReturnValue);
        }
        public Expression Parse_L20_0(MBCC.Tokenizer Tokenizer)
        {
            Expression ReturnValue = new Expression();
            if(Tokenizer.Peek().Type != 20)
            {
                throw new System.Exception("Error parsing " + "_L20" + ": Expected " + "and");
                
            }
            Tokenizer.ConsumeToken();
            if(!(LOOKTable[72,0,Tokenizer.Peek().Type]&& LOOKTable[72,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "_L20" + ": Expected " + "ExpressionOpOr");
                
            }
            ReturnValue = ParseExpressionOpOr(Tokenizer);
            return(ReturnValue);
        }
        public Expression_FuncCall ParseExpression_Ordinary(MBCC.Tokenizer Tokenizer)
        {
            Expression_FuncCall ReturnValue = new Expression_FuncCall();
            ReturnValue = ParseExpression_Ordinary_0(Tokenizer);
            return(ReturnValue);
        }
        public Expression_FuncCall ParseExpression_Ordinary_0(MBCC.Tokenizer Tokenizer)
        {
            Expression_FuncCall ReturnValue = new Expression_FuncCall();
            if(!(LOOKTable[93,0,Tokenizer.Peek().Type]&& LOOKTable[93,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Expression_Ordinary" + ": Expected " + "Expression_Base");
                
            }
            ReturnValue.Args.Add(ParseExpression_Base(Tokenizer));
            if(LOOKTable[90,0,Tokenizer.Peek().Type]&& LOOKTable[90,1,Tokenizer.Peek(1).Type])
            {
                if (LOOKTable[91,0,Tokenizer.Peek().Type]&& LOOKTable[91,1,Tokenizer.Peek(1).Type])
                {
                    do
                    {
                        if (LOOKTable[87,0,Tokenizer.Peek().Type]&& LOOKTable[87,1,Tokenizer.Peek(1).Type])
                        {
                            if(Tokenizer.Peek().Type != 19)
                            {
                                throw new System.Exception("Error parsing " + "_L22" + ": Expected " + "or");
                                
                            }
                            ReturnValue.FuncName.Value = Tokenizer.Peek().Value;
                            Tokenizer.ConsumeToken();
                            if(!(LOOKTable[78,0,Tokenizer.Peek().Type]&& LOOKTable[78,1,Tokenizer.Peek(1).Type]))
                            {
                                throw new System.Exception("Error parsing " + "_L22" + ": Expected " + "ExpressionOpAnd");
                                
                            }
                            ReturnValue.Args.Add(ParseExpressionOpAnd(Tokenizer));
                            
                        }
                        else
                        {
                            throw new System.Exception("Error parsing " + "_L21" + ": Expected " + "_L21");
                            
                        }
                        
                    }
                    while(LOOKTable[86,0,Tokenizer.Peek().Type]&& LOOKTable[86,1,Tokenizer.Peek(1).Type]);
                    
                }
                else if (LOOKTable[92,0,Tokenizer.Peek().Type]&& LOOKTable[92,1,Tokenizer.Peek(1).Type])
                {
                    do
                    {
                        if (LOOKTable[89,0,Tokenizer.Peek().Type]&& LOOKTable[89,1,Tokenizer.Peek(1).Type])
                        {
                            if(Tokenizer.Peek().Type != 20)
                            {
                                throw new System.Exception("Error parsing " + "_L23" + ": Expected " + "and");
                                
                            }
                            ReturnValue.FuncName.Value = Tokenizer.Peek().Value;
                            Tokenizer.ConsumeToken();
                            if(!(LOOKTable[72,0,Tokenizer.Peek().Type]&& LOOKTable[72,1,Tokenizer.Peek(1).Type]))
                            {
                                throw new System.Exception("Error parsing " + "_L23" + ": Expected " + "ExpressionOpOr");
                                
                            }
                            ReturnValue.Args.Add(ParseExpressionOpOr(Tokenizer));
                            
                        }
                        else
                        {
                            throw new System.Exception("Error parsing " + "_L21" + ": Expected " + "_L21");
                            
                        }
                        
                    }
                    while(LOOKTable[88,0,Tokenizer.Peek().Type]&& LOOKTable[88,1,Tokenizer.Peek(1).Type]);
                    
                }
                else
                {
                    throw new System.Exception("Error parsing " + "Expression_Ordinary" + ": Expected " + "Expression_Ordinary");
                    
                }
                
            }
            return(ReturnValue);
        }
        public Expression_FuncCall ParseExpression_Base(MBCC.Tokenizer Tokenizer)
        {
            Expression_FuncCall ReturnValue = new Expression_FuncCall();
            ReturnValue = ParseExpression_Base_0(Tokenizer);
            return(ReturnValue);
        }
        public Expression_FuncCall ParseExpression_Base_0(MBCC.Tokenizer Tokenizer)
        {
            Expression_FuncCall ReturnValue = new Expression_FuncCall();
            if(!(LOOKTable[107,0,Tokenizer.Peek().Type]&& LOOKTable[107,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Expression_Base" + ": Expected " + "Expression_Term");
                
            }
            ReturnValue.Args.Add(ParseExpression_Term(Tokenizer));
            if(LOOKTable[95,0,Tokenizer.Peek().Type]&& LOOKTable[95,1,Tokenizer.Peek(1).Type])
            {
                if (LOOKTable[96,0,Tokenizer.Peek().Type]&& LOOKTable[96,1,Tokenizer.Peek(1).Type])
                {
                    if(Tokenizer.Peek().Type != 16)
                    {
                        throw new System.Exception("Error parsing " + "_L24" + ": Expected " + "comparison");
                        
                    }
                    ReturnValue.FuncName.Value = Tokenizer.Peek().Value;
                    Tokenizer.ConsumeToken();
                    if(!(LOOKTable[107,0,Tokenizer.Peek().Type]&& LOOKTable[107,1,Tokenizer.Peek(1).Type]))
                    {
                        throw new System.Exception("Error parsing " + "_L24" + ": Expected " + "Expression_Term");
                        
                    }
                    ReturnValue.Args.Add(ParseExpression_Term(Tokenizer));
                    
                }
                else
                {
                    throw new System.Exception("Error parsing " + "Expression_Base" + ": Expected " + "Expression_Base");
                    
                }
                
            }
            return(ReturnValue);
        }
        public Expression ParseExpression(MBCC.Tokenizer Tokenizer)
        {
            Expression ReturnValue = new Expression();
            if (LOOKTable[98,0,Tokenizer.Peek().Type]&& LOOKTable[98,1,Tokenizer.Peek(1).Type])
            {
                ReturnValue = ParseExpression_0(Tokenizer);
            }
            else if (LOOKTable[99,0,Tokenizer.Peek().Type]&& LOOKTable[99,1,Tokenizer.Peek(1).Type])
            {
                ReturnValue = ParseExpression_1(Tokenizer);
            }
            else if (LOOKTable[100,0,Tokenizer.Peek().Type]&& LOOKTable[100,1,Tokenizer.Peek(1).Type])
            {
                ReturnValue = ParseExpression_2(Tokenizer);
            }
            else
            {
                throw new System.Exception("Error parsing " + "Expression" + ": Expected " + "Expression");
                
            }
            return(ReturnValue);
        }
        public Expression ParseExpression_0(MBCC.Tokenizer Tokenizer)
        {
            Expression ReturnValue = new Expression();
            if(!(LOOKTable[84,0,Tokenizer.Peek().Type]&& LOOKTable[84,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Expression" + ": Expected " + "Expression_Ordinary");
                
            }
            ReturnValue = ParseExpression_Ordinary(Tokenizer);
            return(ReturnValue);
        }
        public Expression ParseExpression_1(MBCC.Tokenizer Tokenizer)
        {
            Expression ReturnValue = new Expression();
            if(!(LOOKTable[101,0,Tokenizer.Peek().Type]&& LOOKTable[101,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Expression" + ": Expected " + "Expression_Ability");
                
            }
            ReturnValue = ParseExpression_Ability(Tokenizer);
            return(ReturnValue);
        }
        public Expression ParseExpression_2(MBCC.Tokenizer Tokenizer)
        {
            Expression ReturnValue = new Expression();
            if(!(LOOKTable[103,0,Tokenizer.Peek().Type]&& LOOKTable[103,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Expression" + ": Expected " + "Expression_StatReference");
                
            }
            ReturnValue = ParseExpression_StatReference(Tokenizer);
            return(ReturnValue);
        }
        public Expression_Ability ParseExpression_Ability(MBCC.Tokenizer Tokenizer)
        {
            Expression_Ability ReturnValue = new Expression_Ability();
            ReturnValue = ParseExpression_Ability_0(Tokenizer);
            return(ReturnValue);
        }
        public Expression_Ability ParseExpression_Ability_0(MBCC.Tokenizer Tokenizer)
        {
            Expression_Ability ReturnValue = new Expression_Ability();
            if(!(LOOKTable[22,0,Tokenizer.Peek().Type]&& LOOKTable[22,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Expression_Ability" + ": Expected " + "Ability");
                
            }
            ReturnValue.AbilityLiteral = ParseAbility(Tokenizer);
            return(ReturnValue);
        }
        public Expression_StatReference ParseExpression_StatReference(MBCC.Tokenizer Tokenizer)
        {
            Expression_StatReference ReturnValue = new Expression_StatReference();
            ReturnValue = ParseExpression_StatReference_0(Tokenizer);
            return(ReturnValue);
        }
        public Expression_StatReference ParseExpression_StatReference_0(MBCC.Tokenizer Tokenizer)
        {
            Expression_StatReference ReturnValue = new Expression_StatReference();
            if(Tokenizer.Peek().Type != 21)
            {
                throw new System.Exception("Error parsing " + "Expression_StatReference" + ": Expected " + "amp");
                
            }
            Tokenizer.ConsumeToken();
            if(!(LOOKTable[68,0,Tokenizer.Peek().Type]&& LOOKTable[68,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Expression_StatReference" + ": Expected " + "Token");
                
            }
            ReturnValue.Tokens.Add(ParseToken(Tokenizer));
            while(LOOKTable[105,0,Tokenizer.Peek().Type]&& LOOKTable[105,1,Tokenizer.Peek(1).Type])
            {
                ReturnValue.Tokens.Add(Parse_L25(Tokenizer));
                
            }
            return(ReturnValue);
        }
        public Token Parse_L25(MBCC.Tokenizer Tokenizer)
        {
            Token ReturnValue = new Token();
            ReturnValue = Parse_L25_0(Tokenizer);
            return(ReturnValue);
        }
        public Token Parse_L25_0(MBCC.Tokenizer Tokenizer)
        {
            Token ReturnValue = new Token();
            if(Tokenizer.Peek().Type != 22)
            {
                throw new System.Exception("Error parsing " + "_L25" + ": Expected " + "dot");
                
            }
            Tokenizer.ConsumeToken();
            if(!(LOOKTable[68,0,Tokenizer.Peek().Type]&& LOOKTable[68,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "_L25" + ": Expected " + "Token");
                
            }
            ReturnValue = ParseToken(Tokenizer);
            return(ReturnValue);
        }
        public Expression ParseExpression_Term(MBCC.Tokenizer Tokenizer)
        {
            Expression ReturnValue = new Expression();
            if (LOOKTable[108,0,Tokenizer.Peek().Type]&& LOOKTable[108,1,Tokenizer.Peek(1).Type])
            {
                ReturnValue = ParseExpression_Term_0(Tokenizer);
            }
            else if (LOOKTable[109,0,Tokenizer.Peek().Type]&& LOOKTable[109,1,Tokenizer.Peek(1).Type])
            {
                ReturnValue = ParseExpression_Term_1(Tokenizer);
            }
            else if (LOOKTable[110,0,Tokenizer.Peek().Type]&& LOOKTable[110,1,Tokenizer.Peek(1).Type])
            {
                ReturnValue = ParseExpression_Term_2(Tokenizer);
            }
            else if (LOOKTable[111,0,Tokenizer.Peek().Type]&& LOOKTable[111,1,Tokenizer.Peek(1).Type])
            {
                ReturnValue = ParseExpression_Term_3(Tokenizer);
            }
            else
            {
                throw new System.Exception("Error parsing " + "Expression_Term" + ": Expected " + "Expression_Term");
                
            }
            return(ReturnValue);
        }
        public Expression ParseExpression_Term_0(MBCC.Tokenizer Tokenizer)
        {
            Expression ReturnValue = new Expression();
            if(Tokenizer.Peek().Type != 2)
            {
                throw new System.Exception("Error parsing " + "Expression_Term" + ": Expected " + "lpar");
                
            }
            Tokenizer.ConsumeToken();
            if(!(LOOKTable[97,0,Tokenizer.Peek().Type]&& LOOKTable[97,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Expression_Term" + ": Expected " + "Expression");
                
            }
            ReturnValue = ParseExpression(Tokenizer);
            if(Tokenizer.Peek().Type != 3)
            {
                throw new System.Exception("Error parsing " + "Expression_Term" + ": Expected " + "rpar");
                
            }
            Tokenizer.ConsumeToken();
            return(ReturnValue);
        }
        public Expression ParseExpression_Term_1(MBCC.Tokenizer Tokenizer)
        {
            Expression ReturnValue = new Expression();
            if(!(LOOKTable[112,0,Tokenizer.Peek().Type]&& LOOKTable[112,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Expression_Term" + ": Expected " + "Expression_Literal");
                
            }
            ReturnValue = ParseExpression_Literal(Tokenizer);
            return(ReturnValue);
        }
        public Expression ParseExpression_Term_2(MBCC.Tokenizer Tokenizer)
        {
            Expression ReturnValue = new Expression();
            if(!(LOOKTable[120,0,Tokenizer.Peek().Type]&& LOOKTable[120,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Expression_Term" + ": Expected " + "Expression_Variable");
                
            }
            ReturnValue = ParseExpression_Variable(Tokenizer);
            return(ReturnValue);
        }
        public Expression ParseExpression_Term_3(MBCC.Tokenizer Tokenizer)
        {
            Expression ReturnValue = new Expression();
            if(!(LOOKTable[134,0,Tokenizer.Peek().Type]&& LOOKTable[134,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Expression_Term" + ": Expected " + "Expression_FuncCall");
                
            }
            ReturnValue = ParseExpression_FuncCall(Tokenizer);
            return(ReturnValue);
        }
        public Expression_Literal ParseExpression_Literal(MBCC.Tokenizer Tokenizer)
        {
            Expression_Literal ReturnValue = new Expression_Literal();
            ReturnValue = ParseExpression_Literal_0(Tokenizer);
            return(ReturnValue);
        }
        public Expression_Literal ParseExpression_Literal_0(MBCC.Tokenizer Tokenizer)
        {
            Expression_Literal ReturnValue = new Expression_Literal();
            if(!(LOOKTable[122,0,Tokenizer.Peek().Type]&& LOOKTable[122,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Expression_Literal" + ": Expected " + "Literal");
                
            }
            ReturnValue.literal = ParseLiteral(Tokenizer);
            return(ReturnValue);
        }
        public Token ParseToken_1(MBCC.Tokenizer Tokenizer)
        {
            Token ReturnValue = new Token();
            ReturnValue = ParseToken_1_0(Tokenizer);
            return(ReturnValue);
        }
        public Token ParseToken_1_0(MBCC.Tokenizer Tokenizer)
        {
            Token ReturnValue = new Token();
            ReturnValue.Position = Tokenizer.Peek().Position;
            if(Tokenizer.Peek().Type != 27)
            {
                throw new System.Exception("Error parsing " + "Token_1" + ": Expected " + "idf");
                
            }
            ReturnValue.Value = Tokenizer.Peek().Value;
            Tokenizer.ConsumeToken();
            return(ReturnValue);
        }
        public Token ParseToken_2(MBCC.Tokenizer Tokenizer)
        {
            Token ReturnValue = new Token();
            ReturnValue = ParseToken_2_0(Tokenizer);
            return(ReturnValue);
        }
        public Token ParseToken_2_0(MBCC.Tokenizer Tokenizer)
        {
            Token ReturnValue = new Token();
            ReturnValue.Position = Tokenizer.Peek().Position;
            if(Tokenizer.Peek().Type != 27)
            {
                throw new System.Exception("Error parsing " + "Token_2" + ": Expected " + "idf");
                
            }
            ReturnValue.Value = Tokenizer.Peek().Value;
            Tokenizer.ConsumeToken();
            return(ReturnValue);
        }
        public Token ParseToken_3(MBCC.Tokenizer Tokenizer)
        {
            Token ReturnValue = new Token();
            ReturnValue = ParseToken_3_0(Tokenizer);
            return(ReturnValue);
        }
        public Token ParseToken_3_0(MBCC.Tokenizer Tokenizer)
        {
            Token ReturnValue = new Token();
            ReturnValue.Position = Tokenizer.Peek().Position;
            if(Tokenizer.Peek().Type != 27)
            {
                throw new System.Exception("Error parsing " + "Token_3" + ": Expected " + "idf");
                
            }
            ReturnValue.Value = Tokenizer.Peek().Value;
            Tokenizer.ConsumeToken();
            return(ReturnValue);
        }
        public Expression_Variable ParseExpression_Variable(MBCC.Tokenizer Tokenizer)
        {
            Expression_Variable ReturnValue = new Expression_Variable();
            ReturnValue = ParseExpression_Variable_0(Tokenizer);
            return(ReturnValue);
        }
        public Expression_Variable ParseExpression_Variable_0(MBCC.Tokenizer Tokenizer)
        {
            Expression_Variable ReturnValue = new Expression_Variable();
            if(!(LOOKTable[116,0,Tokenizer.Peek().Type]&& LOOKTable[116,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Expression_Variable" + ": Expected " + "Token_2");
                
            }
            ReturnValue.Variable = ParseToken_2(Tokenizer);
            return(ReturnValue);
        }
        public Literal ParseLiteral(MBCC.Tokenizer Tokenizer)
        {
            Literal ReturnValue = new Literal();
            if (LOOKTable[123,0,Tokenizer.Peek().Type]&& LOOKTable[123,1,Tokenizer.Peek(1).Type])
            {
                ReturnValue = ParseLiteral_0(Tokenizer);
            }
            else if (LOOKTable[124,0,Tokenizer.Peek().Type]&& LOOKTable[124,1,Tokenizer.Peek(1).Type])
            {
                ReturnValue = ParseLiteral_1(Tokenizer);
            }
            else if (LOOKTable[125,0,Tokenizer.Peek().Type]&& LOOKTable[125,1,Tokenizer.Peek(1).Type])
            {
                ReturnValue = ParseLiteral_2(Tokenizer);
            }
            else
            {
                throw new System.Exception("Error parsing " + "Literal" + ": Expected " + "Literal");
                
            }
            return(ReturnValue);
        }
        public Literal ParseLiteral_0(MBCC.Tokenizer Tokenizer)
        {
            Literal ReturnValue = new Literal();
            if(!(LOOKTable[128,0,Tokenizer.Peek().Type]&& LOOKTable[128,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Literal" + ": Expected " + "Literal_String");
                
            }
            ReturnValue = ParseLiteral_String(Tokenizer);
            return(ReturnValue);
        }
        public Literal ParseLiteral_1(MBCC.Tokenizer Tokenizer)
        {
            Literal ReturnValue = new Literal();
            if(!(LOOKTable[130,0,Tokenizer.Peek().Type]&& LOOKTable[130,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Literal" + ": Expected " + "Literal_Int");
                
            }
            ReturnValue = ParseLiteral_Int(Tokenizer);
            return(ReturnValue);
        }
        public Literal ParseLiteral_2(MBCC.Tokenizer Tokenizer)
        {
            Literal ReturnValue = new Literal();
            if(!(LOOKTable[126,0,Tokenizer.Peek().Type]&& LOOKTable[126,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Literal" + ": Expected " + "Literal_Bool");
                
            }
            ReturnValue = ParseLiteral_Bool(Tokenizer);
            return(ReturnValue);
        }
        public Literal_Bool ParseLiteral_Bool(MBCC.Tokenizer Tokenizer)
        {
            Literal_Bool ReturnValue = new Literal_Bool();
            ReturnValue = ParseLiteral_Bool_0(Tokenizer);
            return(ReturnValue);
        }
        public Literal_Bool ParseLiteral_Bool_0(MBCC.Tokenizer Tokenizer)
        {
            Literal_Bool ReturnValue = new Literal_Bool();
            ReturnValue.Position = Tokenizer.Peek().Position;
            if(Tokenizer.Peek().Type != 23)
            {
                throw new System.Exception("Error parsing " + "Literal_Bool" + ": Expected " + "bool");
                
            }
            ReturnValue.Value = Tokenizer.Peek().Value == "true";
            Tokenizer.ConsumeToken();
            return(ReturnValue);
        }
        public Literal_String ParseLiteral_String(MBCC.Tokenizer Tokenizer)
        {
            Literal_String ReturnValue = new Literal_String();
            ReturnValue = ParseLiteral_String_0(Tokenizer);
            return(ReturnValue);
        }
        public Literal_String ParseLiteral_String_0(MBCC.Tokenizer Tokenizer)
        {
            Literal_String ReturnValue = new Literal_String();
            ReturnValue.Position = Tokenizer.Peek().Position;
            if(Tokenizer.Peek().Type != 18)
            {
                throw new System.Exception("Error parsing " + "Literal_String" + ": Expected " + "str");
                
            }
            ReturnValue.Value = Tokenizer.Peek().Value;
            Tokenizer.ConsumeToken();
            return(ReturnValue);
        }
        public Literal_Int ParseLiteral_Int(MBCC.Tokenizer Tokenizer)
        {
            Literal_Int ReturnValue = new Literal_Int();
            ReturnValue = ParseLiteral_Int_0(Tokenizer);
            return(ReturnValue);
        }
        public Literal_Int ParseLiteral_Int_0(MBCC.Tokenizer Tokenizer)
        {
            Literal_Int ReturnValue = new Literal_Int();
            ReturnValue.Position = Tokenizer.Peek().Position;
            if(Tokenizer.Peek().Type != 17)
            {
                throw new System.Exception("Error parsing " + "Literal_Int" + ": Expected " + "Int");
                
            }
            ReturnValue.Value = Int32.Parse(Tokenizer.Peek().Value);
            Tokenizer.ConsumeToken();
            return(ReturnValue);
        }
        public KeyArg ParseKeyArg(MBCC.Tokenizer Tokenizer)
        {
            KeyArg ReturnValue = new KeyArg();
            ReturnValue = ParseKeyArg_0(Tokenizer);
            return(ReturnValue);
        }
        public KeyArg ParseKeyArg_0(MBCC.Tokenizer Tokenizer)
        {
            KeyArg ReturnValue = new KeyArg();
            if(!(LOOKTable[118,0,Tokenizer.Peek().Type]&& LOOKTable[118,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "KeyArg" + ": Expected " + "Token_3");
                
            }
            ReturnValue.Name = ParseToken_3(Tokenizer);
            if(Tokenizer.Peek().Type != 7)
            {
                throw new System.Exception("Error parsing " + "KeyArg" + ": Expected " + "eq");
                
            }
            Tokenizer.ConsumeToken();
            if(!(LOOKTable[97,0,Tokenizer.Peek().Type]&& LOOKTable[97,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "KeyArg" + ": Expected " + "Expression");
                
            }
            ReturnValue.Value = ParseExpression(Tokenizer);
            return(ReturnValue);
        }
        public Expression_FuncCall ParseExpression_FuncCall(MBCC.Tokenizer Tokenizer)
        {
            Expression_FuncCall ReturnValue = new Expression_FuncCall();
            ReturnValue = ParseExpression_FuncCall_0(Tokenizer);
            return(ReturnValue);
        }
        public Expression_FuncCall ParseExpression_FuncCall_0(MBCC.Tokenizer Tokenizer)
        {
            Expression_FuncCall ReturnValue = new Expression_FuncCall();
            if(!(LOOKTable[114,0,Tokenizer.Peek().Type]&& LOOKTable[114,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Expression_FuncCall" + ": Expected " + "Token_1");
                
            }
            ReturnValue.FuncName = ParseToken_1(Tokenizer);
            if(Tokenizer.Peek().Type != 2)
            {
                throw new System.Exception("Error parsing " + "Expression_FuncCall" + ": Expected " + "lpar");
                
            }
            Tokenizer.ConsumeToken();
            if(LOOKTable[144,0,Tokenizer.Peek().Type]&& LOOKTable[144,1,Tokenizer.Peek(1).Type])
            {
                if (LOOKTable[145,0,Tokenizer.Peek().Type]&& LOOKTable[145,1,Tokenizer.Peek(1).Type])
                {
                    if (LOOKTable[137,0,Tokenizer.Peek().Type]&& LOOKTable[137,1,Tokenizer.Peek(1).Type])
                    {
                        if(!(LOOKTable[97,0,Tokenizer.Peek().Type]&& LOOKTable[97,1,Tokenizer.Peek(1).Type]))
                        {
                            throw new System.Exception("Error parsing " + "_L27" + ": Expected " + "Expression");
                            
                        }
                        ReturnValue.Args.Add(ParseExpression(Tokenizer));
                        
                    }
                    else if (LOOKTable[138,0,Tokenizer.Peek().Type]&& LOOKTable[138,1,Tokenizer.Peek(1).Type])
                    {
                        if(!(LOOKTable[132,0,Tokenizer.Peek().Type]&& LOOKTable[132,1,Tokenizer.Peek(1).Type]))
                        {
                            throw new System.Exception("Error parsing " + "_L27" + ": Expected " + "KeyArg");
                            
                        }
                        ReturnValue.KeyArgs.Add(ParseKeyArg(Tokenizer));
                        
                    }
                    else
                    {
                        throw new System.Exception("Error parsing " + "_L26" + ": Expected " + "_L26");
                        
                    }
                    while(LOOKTable[142,0,Tokenizer.Peek().Type]&& LOOKTable[142,1,Tokenizer.Peek(1).Type])
                    {
                        if (LOOKTable[143,0,Tokenizer.Peek().Type]&& LOOKTable[143,1,Tokenizer.Peek(1).Type])
                        {
                            if(Tokenizer.Peek().Type != 5)
                            {
                                throw new System.Exception("Error parsing " + "_L28" + ": Expected " + "comma");
                                
                            }
                            Tokenizer.ConsumeToken();
                            if (LOOKTable[140,0,Tokenizer.Peek().Type]&& LOOKTable[140,1,Tokenizer.Peek(1).Type])
                            {
                                if(!(LOOKTable[97,0,Tokenizer.Peek().Type]&& LOOKTable[97,1,Tokenizer.Peek(1).Type]))
                                {
                                    throw new System.Exception("Error parsing " + "_L29" + ": Expected " + "Expression");
                                    
                                }
                                ReturnValue.Args.Add(ParseExpression(Tokenizer));
                                
                            }
                            else if (LOOKTable[141,0,Tokenizer.Peek().Type]&& LOOKTable[141,1,Tokenizer.Peek(1).Type])
                            {
                                if(!(LOOKTable[132,0,Tokenizer.Peek().Type]&& LOOKTable[132,1,Tokenizer.Peek(1).Type]))
                                {
                                    throw new System.Exception("Error parsing " + "_L29" + ": Expected " + "KeyArg");
                                    
                                }
                                ReturnValue.KeyArgs.Add(ParseKeyArg(Tokenizer));
                                
                            }
                            else
                            {
                                throw new System.Exception("Error parsing " + "_L28" + ": Expected " + "_L28");
                                
                            }
                            
                        }
                        else
                        {
                            throw new System.Exception("Error parsing " + "_L26" + ": Expected " + "_L26");
                            
                        }
                        
                    }
                    
                }
                else
                {
                    throw new System.Exception("Error parsing " + "Expression_FuncCall" + ": Expected " + "Expression_FuncCall");
                    
                }
                
            }
            if(Tokenizer.Peek().Type != 3)
            {
                throw new System.Exception("Error parsing " + "Expression_FuncCall" + ": Expected " + "rpar");
                
            }
            Tokenizer.ConsumeToken();
            return(ReturnValue);
        }
        public MBCC.Tokenizer GetTokenizer()
        {
            return new MBCC.Tokenizer("\\s*","\\{","\\}","\\(","\\)",":",",",";","=","stats","Tags","import","as","visuals","activated","triggered","continous","<|<=|>|>=","\\d+","\"([^\"\\\\\\\\]|\\\\\\\\.)*\"","\\|\\|","&&","&","\\.","true|false","range","hover","actCount","\\w\\w*");
        }
        
    }
    
}
