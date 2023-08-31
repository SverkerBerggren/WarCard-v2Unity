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
    public class Unit
    {
        public Token Name = new Token();
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
    public class Ability
    {
        
    }
    public class TargetCondition
    {
        
    }
    public class ActivatedAbilityTarget
    {
        public Type TargetType = new Type();
        public Token Name = new Token();
        public Expression Condition = new Expression();
        
    }
    public class Ability_Activated : Ability
    {
        public MBCC.TokenPosition Begin = new MBCC.TokenPosition();
        public Token Name = new Token();
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
    public class Expression_Literal : Expression
    {
        public Literal literal = new Literal();
        
    }
    public class Expression_Variable : Expression
    {
        public Token Variable = new Token();
        
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
    public class Visuals
    {
        public MBCC.TokenPosition Begin = new MBCC.TokenPosition();
        public List<VariableDeclaration> Declarations = new List<VariableDeclaration>();
        
    }
    public class Parser
    {
        bool[,,] LOOKTable = {{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{true,false,true,true,true,true,false,true,false,false,false,false,false,false,false,false,false,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{true,false,true,true,true,true,false,true,false,false,false,false,false,false,false,false,false,false,true,true,},},{{false,false,false,false,false,false,false,false,true,true,true,false,false,false,false,false,false,false,true,false,},{true,false,true,true,true,true,false,true,false,false,false,false,false,false,false,false,false,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{true,false,true,true,true,true,false,true,false,false,false,false,false,false,false,false,false,false,true,true,},},{{false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,},{true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},},{{false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,},{true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},},{{false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,},{true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},},{{false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,},{true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{true,false,true,true,true,true,false,true,false,false,false,false,false,false,false,false,false,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{true,false,true,true,true,true,false,true,false,false,false,false,false,false,false,false,false,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{true,false,true,true,true,true,false,true,false,false,false,false,false,false,false,false,false,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{true,false,true,true,true,true,false,true,false,false,false,false,false,false,false,false,false,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{true,false,true,true,true,true,false,true,false,false,false,false,false,false,false,false,false,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{true,false,true,true,true,true,false,true,false,false,false,false,false,false,false,false,false,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{true,false,true,true,true,true,false,true,false,false,false,false,false,false,false,false,false,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{true,false,true,true,true,true,false,true,false,false,false,false,false,false,false,false,false,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{true,false,true,true,true,true,false,true,false,false,false,false,false,false,false,false,false,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{true,false,true,true,true,true,false,true,false,false,false,false,false,false,false,false,false,false,true,true,},},{{false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},},{{false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},{false,false,true,true,false,true,true,false,false,false,false,false,false,true,true,true,true,true,true,true,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},{false,false,true,true,false,true,true,false,false,false,false,false,false,true,true,true,true,true,true,true,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},{false,false,true,true,false,true,true,false,false,false,false,false,false,true,true,true,true,true,true,true,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},{false,false,true,true,false,true,true,false,false,false,false,false,false,true,true,true,true,true,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{true,false,true,true,true,true,false,true,false,false,false,false,false,false,false,false,false,false,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{true,false,true,true,true,true,false,true,false,false,false,false,false,false,false,false,false,false,true,true,},},{{false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,},{true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},},{{false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,},{true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},{false,false,true,true,false,true,true,false,false,false,false,false,false,true,true,true,true,true,true,true,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},{false,false,true,true,false,true,true,false,false,false,false,false,false,true,true,true,true,true,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},{false,false,true,true,false,true,true,false,false,false,false,false,false,true,true,true,true,true,true,true,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},{false,false,true,true,false,true,true,false,false,false,false,false,false,true,true,true,true,true,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,},{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},{false,false,true,true,false,true,true,false,false,false,false,false,false,true,true,true,true,true,true,true,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},{false,false,true,true,false,true,true,false,false,false,false,false,false,true,true,true,true,true,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},{false,false,true,true,false,true,true,false,false,false,false,false,false,true,true,true,true,true,true,true,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},{false,false,true,true,false,true,true,false,false,false,false,false,false,true,true,true,true,true,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},{false,false,true,true,false,true,true,false,false,false,false,false,false,true,true,true,true,true,true,true,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,true,false,false,true,true,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,true,false,false,true,true,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,true,false,false,true,true,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,true,false,false,true,true,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,true,false,false,true,true,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,true,false,false,true,true,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,true,false,false,true,true,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,true,false,false,true,true,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,false,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,true,false,false,true,true,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,true,false,false,true,true,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,true,false,false,true,true,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,true,false,false,true,true,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,true,false,false,true,true,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,true,false,false,true,true,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false,false,false,},{false,false,false,true,false,true,true,false,false,false,false,false,false,true,false,false,true,true,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},{false,false,true,true,false,true,true,true,false,false,false,false,false,true,true,true,true,true,true,true,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},{false,false,true,true,false,true,true,false,false,false,false,false,false,true,true,true,true,true,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},{false,false,true,true,false,true,true,true,false,false,false,false,false,true,true,true,true,true,true,true,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},{false,false,true,true,false,true,true,false,false,false,false,false,false,true,true,true,true,true,true,true,},},{{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,},{false,false,false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,},},{{false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},},{{false,false,false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,},{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},{false,false,true,true,false,true,true,true,false,false,false,false,false,true,true,true,true,true,true,true,},},{{false,false,true,false,false,false,false,false,false,false,false,false,false,false,true,true,false,false,true,false,},{false,false,true,true,false,true,true,true,false,false,false,false,false,true,true,true,true,true,true,true,},},};
        public Unit ParseUnit(MBCC.Tokenizer Tokenizer)
        {
            Unit ReturnValue = new Unit();
            ReturnValue = ParseUnit_0(Tokenizer);
            return(ReturnValue);
        }
        public Unit ParseUnit_0(MBCC.Tokenizer Tokenizer)
        {
            Unit ReturnValue = new Unit();
            if(!(LOOKTable[31,0,Tokenizer.Peek().Type]&& LOOKTable[31,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Unit" + ": Expected " + "Token");
                
            }
            ReturnValue.Name = ParseToken(Tokenizer);
            if(Tokenizer.Peek().Type != 0)
            {
                throw new System.Exception("Error parsing " + "Unit" + ": Expected " + "lcurl");
                
            }
            Tokenizer.ConsumeToken();
            while(LOOKTable[2,0,Tokenizer.Peek().Type]&& LOOKTable[2,1,Tokenizer.Peek(1).Type])
            {
                if (LOOKTable[3,0,Tokenizer.Peek().Type]&& LOOKTable[3,1,Tokenizer.Peek(1).Type])
                {
                    if(!(LOOKTable[11,0,Tokenizer.Peek().Type]&& LOOKTable[11,1,Tokenizer.Peek(1).Type]))
                    {
                        throw new System.Exception("Error parsing " + "_L1" + ": Expected " + "VariableDeclaration");
                        
                    }
                    ReturnValue.Variables.Add(ParseVariableDeclaration(Tokenizer));
                    
                }
                else if (LOOKTable[4,0,Tokenizer.Peek().Type]&& LOOKTable[4,1,Tokenizer.Peek(1).Type])
                {
                    if(!(LOOKTable[7,0,Tokenizer.Peek().Type]&& LOOKTable[7,1,Tokenizer.Peek(1).Type]))
                    {
                        throw new System.Exception("Error parsing " + "_L1" + ": Expected " + "UnitStats");
                        
                    }
                    ReturnValue.Stats = ParseUnitStats(Tokenizer);
                    
                }
                else if (LOOKTable[5,0,Tokenizer.Peek().Type]&& LOOKTable[5,1,Tokenizer.Peek(1).Type])
                {
                    if(!(LOOKTable[15,0,Tokenizer.Peek().Type]&& LOOKTable[15,1,Tokenizer.Peek(1).Type]))
                    {
                        throw new System.Exception("Error parsing " + "_L1" + ": Expected " + "Ability");
                        
                    }
                    ReturnValue.Abilities.Add(ParseAbility(Tokenizer));
                    
                }
                else if (LOOKTable[6,0,Tokenizer.Peek().Type]&& LOOKTable[6,1,Tokenizer.Peek(1).Type])
                {
                    if(!(LOOKTable[33,0,Tokenizer.Peek().Type]&& LOOKTable[33,1,Tokenizer.Peek(1).Type]))
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
            while(LOOKTable[9,0,Tokenizer.Peek().Type]&& LOOKTable[9,1,Tokenizer.Peek(1).Type])
            {
                ReturnValue.Declarations.Add(ParseStatDeclaration(Tokenizer));
                
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
            if(!(LOOKTable[31,0,Tokenizer.Peek().Type]&& LOOKTable[31,1,Tokenizer.Peek(1).Type]))
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
            if(Tokenizer.Peek().Type != 14)
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
            if(!(LOOKTable[31,0,Tokenizer.Peek().Type]&& LOOKTable[31,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "VariableDeclaration" + ": Expected " + "Token");
                
            }
            ReturnValue.VariableName = ParseToken(Tokenizer);
            if(Tokenizer.Peek().Type != 7)
            {
                throw new System.Exception("Error parsing " + "VariableDeclaration" + ": Expected " + "eq");
                
            }
            Tokenizer.ConsumeToken();
            if(!(LOOKTable[51,0,Tokenizer.Peek().Type]&& LOOKTable[51,1,Tokenizer.Peek(1).Type]))
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
            if(!(LOOKTable[31,0,Tokenizer.Peek().Type]&& LOOKTable[31,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Type" + ": Expected " + "Token");
                
            }
            ReturnValue.TypeIdentifier = ParseToken(Tokenizer);
            return(ReturnValue);
        }
        public Ability ParseAbility(MBCC.Tokenizer Tokenizer)
        {
            Ability ReturnValue = new Ability();
            ReturnValue = ParseAbility_0(Tokenizer);
            return(ReturnValue);
        }
        public Ability ParseAbility_0(MBCC.Tokenizer Tokenizer)
        {
            Ability ReturnValue = new Ability();
            if(!(LOOKTable[17,0,Tokenizer.Peek().Type]&& LOOKTable[17,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Ability" + ": Expected " + "Ability_Activated");
                
            }
            ReturnValue = ParseAbility_Activated(Tokenizer);
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
            if(Tokenizer.Peek().Type != 10)
            {
                throw new System.Exception("Error parsing " + "Ability_Activated" + ": Expected " + "activated");
                
            }
            Tokenizer.ConsumeToken();
            if(!(LOOKTable[31,0,Tokenizer.Peek().Type]&& LOOKTable[31,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Ability_Activated" + ": Expected " + "Token");
                
            }
            ReturnValue.Name = ParseToken(Tokenizer);
            if(Tokenizer.Peek().Type != 2)
            {
                throw new System.Exception("Error parsing " + "Ability_Activated" + ": Expected " + "lpar");
                
            }
            Tokenizer.ConsumeToken();
            if(LOOKTable[21,0,Tokenizer.Peek().Type]&& LOOKTable[21,1,Tokenizer.Peek(1).Type])
            {
                if (LOOKTable[22,0,Tokenizer.Peek().Type]&& LOOKTable[22,1,Tokenizer.Peek(1).Type])
                {
                    if(!(LOOKTable[23,0,Tokenizer.Peek().Type]&& LOOKTable[23,1,Tokenizer.Peek(1).Type]))
                    {
                        throw new System.Exception("Error parsing " + "_L3" + ": Expected " + "ActivatedAbilityTarget");
                        
                    }
                    ReturnValue.Targets.Add(ParseActivatedAbilityTarget(Tokenizer));
                    while(LOOKTable[19,0,Tokenizer.Peek().Type]&& LOOKTable[19,1,Tokenizer.Peek(1).Type])
                    {
                        ReturnValue.Targets.Add(Parse_L2(Tokenizer));
                        
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
            if(Tokenizer.Peek().Type != 0)
            {
                throw new System.Exception("Error parsing " + "Ability_Activated" + ": Expected " + "lcurl");
                
            }
            Tokenizer.ConsumeToken();
            while(LOOKTable[27,0,Tokenizer.Peek().Type]&& LOOKTable[27,1,Tokenizer.Peek(1).Type])
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
        public ActivatedAbilityTarget Parse_L2(MBCC.Tokenizer Tokenizer)
        {
            ActivatedAbilityTarget ReturnValue = new ActivatedAbilityTarget();
            ReturnValue = Parse_L2_0(Tokenizer);
            return(ReturnValue);
        }
        public ActivatedAbilityTarget Parse_L2_0(MBCC.Tokenizer Tokenizer)
        {
            ActivatedAbilityTarget ReturnValue = new ActivatedAbilityTarget();
            if(Tokenizer.Peek().Type != 5)
            {
                throw new System.Exception("Error parsing " + "_L2" + ": Expected " + "comma");
                
            }
            Tokenizer.ConsumeToken();
            if(!(LOOKTable[23,0,Tokenizer.Peek().Type]&& LOOKTable[23,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "_L2" + ": Expected " + "ActivatedAbilityTarget");
                
            }
            ReturnValue = ParseActivatedAbilityTarget(Tokenizer);
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
            if(!(LOOKTable[13,0,Tokenizer.Peek().Type]&& LOOKTable[13,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "ActivatedAbilityTarget" + ": Expected " + "Type");
                
            }
            ReturnValue.TargetType = ParseType(Tokenizer);
            if(!(LOOKTable[31,0,Tokenizer.Peek().Type]&& LOOKTable[31,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "ActivatedAbilityTarget" + ": Expected " + "Token");
                
            }
            ReturnValue.Name = ParseToken(Tokenizer);
            if(LOOKTable[25,0,Tokenizer.Peek().Type]&& LOOKTable[25,1,Tokenizer.Peek(1).Type])
            {
                if (LOOKTable[26,0,Tokenizer.Peek().Type]&& LOOKTable[26,1,Tokenizer.Peek(1).Type])
                {
                    if(Tokenizer.Peek().Type != 4)
                    {
                        throw new System.Exception("Error parsing " + "_L4" + ": Expected " + "colon");
                        
                    }
                    Tokenizer.ConsumeToken();
                    if(!(LOOKTable[51,0,Tokenizer.Peek().Type]&& LOOKTable[51,1,Tokenizer.Peek(1).Type]))
                    {
                        throw new System.Exception("Error parsing " + "_L4" + ": Expected " + "Expression");
                        
                    }
                    ReturnValue.Condition = ParseExpression(Tokenizer);
                    
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
            ReturnValue = ParseAbilityStatement_0(Tokenizer);
            return(ReturnValue);
        }
        public AbilityStatement ParseAbilityStatement_0(MBCC.Tokenizer Tokenizer)
        {
            AbilityStatement ReturnValue = new AbilityStatement();
            if(!(LOOKTable[29,0,Tokenizer.Peek().Type]&& LOOKTable[29,1,Tokenizer.Peek(1).Type]))
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
        public AbilityStatement_Expression ParseAbilityStatement_Expression(MBCC.Tokenizer Tokenizer)
        {
            AbilityStatement_Expression ReturnValue = new AbilityStatement_Expression();
            ReturnValue = ParseAbilityStatement_Expression_0(Tokenizer);
            return(ReturnValue);
        }
        public AbilityStatement_Expression ParseAbilityStatement_Expression_0(MBCC.Tokenizer Tokenizer)
        {
            AbilityStatement_Expression ReturnValue = new AbilityStatement_Expression();
            if(!(LOOKTable[51,0,Tokenizer.Peek().Type]&& LOOKTable[51,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "AbilityStatement_Expression" + ": Expected " + "Expression");
                
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
            if(Tokenizer.Peek().Type != 18)
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
            if(Tokenizer.Peek().Type != 9)
            {
                throw new System.Exception("Error parsing " + "Visuals" + ": Expected " + "visuals");
                
            }
            Tokenizer.ConsumeToken();
            if(Tokenizer.Peek().Type != 0)
            {
                throw new System.Exception("Error parsing " + "Visuals" + ": Expected " + "lcurl");
                
            }
            Tokenizer.ConsumeToken();
            while(LOOKTable[11,0,Tokenizer.Peek().Type]&& LOOKTable[11,1,Tokenizer.Peek(1).Type])
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
            if(!(LOOKTable[47,0,Tokenizer.Peek().Type]&& LOOKTable[47,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "ExpressionOpOr" + ": Expected " + "Expression_Base");
                
            }
            ReturnValue.Args.Add(ParseExpression_Base(Tokenizer));
            if(LOOKTable[39,0,Tokenizer.Peek().Type]&& LOOKTable[39,1,Tokenizer.Peek(1).Type])
            {
                if (LOOKTable[40,0,Tokenizer.Peek().Type]&& LOOKTable[40,1,Tokenizer.Peek(1).Type])
                {
                    if(Tokenizer.Peek().Type != 16)
                    {
                        throw new System.Exception("Error parsing " + "_L6" + ": Expected " + "or");
                        
                    }
                    Tokenizer.ConsumeToken();
                    ReturnValue.FuncName.Value = "||";
                    if(!(LOOKTable[47,0,Tokenizer.Peek().Type]&& LOOKTable[47,1,Tokenizer.Peek(1).Type]))
                    {
                        throw new System.Exception("Error parsing " + "_L6" + ": Expected " + "Expression_Base");
                        
                    }
                    ReturnValue.Args.Add(ParseExpression_Base(Tokenizer));
                    while(LOOKTable[37,0,Tokenizer.Peek().Type]&& LOOKTable[37,1,Tokenizer.Peek(1).Type])
                    {
                        ReturnValue.Args.Add(Parse_L5(Tokenizer));
                        
                    }
                    
                }
                else
                {
                    throw new System.Exception("Error parsing " + "ExpressionOpOr" + ": Expected " + "ExpressionOpOr");
                    
                }
                
            }
            return(ReturnValue);
        }
        public Expression Parse_L5(MBCC.Tokenizer Tokenizer)
        {
            Expression ReturnValue = new Expression();
            ReturnValue = Parse_L5_0(Tokenizer);
            return(ReturnValue);
        }
        public Expression Parse_L5_0(MBCC.Tokenizer Tokenizer)
        {
            Expression ReturnValue = new Expression();
            if(Tokenizer.Peek().Type != 16)
            {
                throw new System.Exception("Error parsing " + "_L5" + ": Expected " + "or");
                
            }
            Tokenizer.ConsumeToken();
            if(!(LOOKTable[41,0,Tokenizer.Peek().Type]&& LOOKTable[41,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "_L5" + ": Expected " + "ExpressionOpAnd");
                
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
            if(!(LOOKTable[47,0,Tokenizer.Peek().Type]&& LOOKTable[47,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "ExpressionOpAnd" + ": Expected " + "Expression_Base");
                
            }
            ReturnValue.Args.Add(ParseExpression_Base(Tokenizer));
            if(LOOKTable[45,0,Tokenizer.Peek().Type]&& LOOKTable[45,1,Tokenizer.Peek(1).Type])
            {
                if (LOOKTable[46,0,Tokenizer.Peek().Type]&& LOOKTable[46,1,Tokenizer.Peek(1).Type])
                {
                    if(Tokenizer.Peek().Type != 17)
                    {
                        throw new System.Exception("Error parsing " + "_L8" + ": Expected " + "and");
                        
                    }
                    Tokenizer.ConsumeToken();
                    ReturnValue.FuncName.Value = "&&";
                    if(!(LOOKTable[47,0,Tokenizer.Peek().Type]&& LOOKTable[47,1,Tokenizer.Peek(1).Type]))
                    {
                        throw new System.Exception("Error parsing " + "_L8" + ": Expected " + "Expression_Base");
                        
                    }
                    ReturnValue.Args.Add(ParseExpression_Base(Tokenizer));
                    while(LOOKTable[43,0,Tokenizer.Peek().Type]&& LOOKTable[43,1,Tokenizer.Peek(1).Type])
                    {
                        ReturnValue.Args.Add(Parse_L7(Tokenizer));
                        
                    }
                    
                }
                else
                {
                    throw new System.Exception("Error parsing " + "ExpressionOpAnd" + ": Expected " + "ExpressionOpAnd");
                    
                }
                
            }
            return(ReturnValue);
        }
        public Expression Parse_L7(MBCC.Tokenizer Tokenizer)
        {
            Expression ReturnValue = new Expression();
            ReturnValue = Parse_L7_0(Tokenizer);
            return(ReturnValue);
        }
        public Expression Parse_L7_0(MBCC.Tokenizer Tokenizer)
        {
            Expression ReturnValue = new Expression();
            if(Tokenizer.Peek().Type != 17)
            {
                throw new System.Exception("Error parsing " + "_L7" + ": Expected " + "and");
                
            }
            Tokenizer.ConsumeToken();
            if(!(LOOKTable[35,0,Tokenizer.Peek().Type]&& LOOKTable[35,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "_L7" + ": Expected " + "ExpressionOpOr");
                
            }
            ReturnValue = ParseExpressionOpOr(Tokenizer);
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
            if(!(LOOKTable[60,0,Tokenizer.Peek().Type]&& LOOKTable[60,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Expression_Base" + ": Expected " + "Expression_Term");
                
            }
            ReturnValue.Args.Add(ParseExpression_Term(Tokenizer));
            if(LOOKTable[49,0,Tokenizer.Peek().Type]&& LOOKTable[49,1,Tokenizer.Peek(1).Type])
            {
                if (LOOKTable[50,0,Tokenizer.Peek().Type]&& LOOKTable[50,1,Tokenizer.Peek(1).Type])
                {
                    if(Tokenizer.Peek().Type != 13)
                    {
                        throw new System.Exception("Error parsing " + "_L9" + ": Expected " + "comparison");
                        
                    }
                    ReturnValue.FuncName.Value = Tokenizer.Peek().Value;
                    Tokenizer.ConsumeToken();
                    if(!(LOOKTable[60,0,Tokenizer.Peek().Type]&& LOOKTable[60,1,Tokenizer.Peek(1).Type]))
                    {
                        throw new System.Exception("Error parsing " + "_L9" + ": Expected " + "Expression_Term");
                        
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
        public Expression_FuncCall ParseExpression(MBCC.Tokenizer Tokenizer)
        {
            Expression_FuncCall ReturnValue = new Expression_FuncCall();
            ReturnValue = ParseExpression_0(Tokenizer);
            return(ReturnValue);
        }
        public Expression_FuncCall ParseExpression_0(MBCC.Tokenizer Tokenizer)
        {
            Expression_FuncCall ReturnValue = new Expression_FuncCall();
            if(!(LOOKTable[47,0,Tokenizer.Peek().Type]&& LOOKTable[47,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Expression" + ": Expected " + "Expression_Base");
                
            }
            ReturnValue.Args.Add(ParseExpression_Base(Tokenizer));
            if(LOOKTable[57,0,Tokenizer.Peek().Type]&& LOOKTable[57,1,Tokenizer.Peek(1).Type])
            {
                if (LOOKTable[58,0,Tokenizer.Peek().Type]&& LOOKTable[58,1,Tokenizer.Peek(1).Type])
                {
                    do
                    {
                        if (LOOKTable[54,0,Tokenizer.Peek().Type]&& LOOKTable[54,1,Tokenizer.Peek(1).Type])
                        {
                            if(Tokenizer.Peek().Type != 16)
                            {
                                throw new System.Exception("Error parsing " + "_L10" + ": Expected " + "or");
                                
                            }
                            ReturnValue.FuncName.Value = Tokenizer.Peek().Value;
                            Tokenizer.ConsumeToken();
                            if(!(LOOKTable[41,0,Tokenizer.Peek().Type]&& LOOKTable[41,1,Tokenizer.Peek(1).Type]))
                            {
                                throw new System.Exception("Error parsing " + "_L10" + ": Expected " + "ExpressionOpAnd");
                                
                            }
                            ReturnValue.Args.Add(ParseExpressionOpAnd(Tokenizer));
                            
                        }
                        else
                        {
                            throw new System.Exception("Error parsing " + "_L12" + ": Expected " + "_L12");
                            
                        }
                        
                    }
                    while(LOOKTable[53,0,Tokenizer.Peek().Type]&& LOOKTable[53,1,Tokenizer.Peek(1).Type]);
                    
                }
                else if (LOOKTable[59,0,Tokenizer.Peek().Type]&& LOOKTable[59,1,Tokenizer.Peek(1).Type])
                {
                    do
                    {
                        if (LOOKTable[56,0,Tokenizer.Peek().Type]&& LOOKTable[56,1,Tokenizer.Peek(1).Type])
                        {
                            if(Tokenizer.Peek().Type != 17)
                            {
                                throw new System.Exception("Error parsing " + "_L11" + ": Expected " + "and");
                                
                            }
                            ReturnValue.FuncName.Value = Tokenizer.Peek().Value;
                            Tokenizer.ConsumeToken();
                            if(!(LOOKTable[35,0,Tokenizer.Peek().Type]&& LOOKTable[35,1,Tokenizer.Peek(1).Type]))
                            {
                                throw new System.Exception("Error parsing " + "_L11" + ": Expected " + "ExpressionOpOr");
                                
                            }
                            ReturnValue.Args.Add(ParseExpressionOpOr(Tokenizer));
                            
                        }
                        else
                        {
                            throw new System.Exception("Error parsing " + "_L12" + ": Expected " + "_L12");
                            
                        }
                        
                    }
                    while(LOOKTable[55,0,Tokenizer.Peek().Type]&& LOOKTable[55,1,Tokenizer.Peek(1).Type]);
                    
                }
                else
                {
                    throw new System.Exception("Error parsing " + "Expression" + ": Expected " + "Expression");
                    
                }
                
            }
            return(ReturnValue);
        }
        public Expression ParseExpression_Term(MBCC.Tokenizer Tokenizer)
        {
            Expression ReturnValue = new Expression();
            if (LOOKTable[61,0,Tokenizer.Peek().Type]&& LOOKTable[61,1,Tokenizer.Peek(1).Type])
            {
                ReturnValue = ParseExpression_Term_0(Tokenizer);
            }
            else if (LOOKTable[62,0,Tokenizer.Peek().Type]&& LOOKTable[62,1,Tokenizer.Peek(1).Type])
            {
                ReturnValue = ParseExpression_Term_1(Tokenizer);
            }
            else if (LOOKTable[63,0,Tokenizer.Peek().Type]&& LOOKTable[63,1,Tokenizer.Peek(1).Type])
            {
                ReturnValue = ParseExpression_Term_2(Tokenizer);
            }
            else if (LOOKTable[64,0,Tokenizer.Peek().Type]&& LOOKTable[64,1,Tokenizer.Peek(1).Type])
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
            if(!(LOOKTable[51,0,Tokenizer.Peek().Type]&& LOOKTable[51,1,Tokenizer.Peek(1).Type]))
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
            if(!(LOOKTable[65,0,Tokenizer.Peek().Type]&& LOOKTable[65,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Expression_Term" + ": Expected " + "Expression_Literal");
                
            }
            ReturnValue = ParseExpression_Literal(Tokenizer);
            return(ReturnValue);
        }
        public Expression ParseExpression_Term_2(MBCC.Tokenizer Tokenizer)
        {
            Expression ReturnValue = new Expression();
            if(!(LOOKTable[73,0,Tokenizer.Peek().Type]&& LOOKTable[73,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Expression_Term" + ": Expected " + "Expression_Variable");
                
            }
            ReturnValue = ParseExpression_Variable(Tokenizer);
            return(ReturnValue);
        }
        public Expression ParseExpression_Term_3(MBCC.Tokenizer Tokenizer)
        {
            Expression ReturnValue = new Expression();
            if(!(LOOKTable[84,0,Tokenizer.Peek().Type]&& LOOKTable[84,1,Tokenizer.Peek(1).Type]))
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
            if(!(LOOKTable[75,0,Tokenizer.Peek().Type]&& LOOKTable[75,1,Tokenizer.Peek(1).Type]))
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
            if(Tokenizer.Peek().Type != 18)
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
            if(Tokenizer.Peek().Type != 18)
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
            if(Tokenizer.Peek().Type != 18)
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
            if(!(LOOKTable[69,0,Tokenizer.Peek().Type]&& LOOKTable[69,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Expression_Variable" + ": Expected " + "Token_2");
                
            }
            ReturnValue.Variable = ParseToken_2(Tokenizer);
            return(ReturnValue);
        }
        public Literal ParseLiteral(MBCC.Tokenizer Tokenizer)
        {
            Literal ReturnValue = new Literal();
            if (LOOKTable[76,0,Tokenizer.Peek().Type]&& LOOKTable[76,1,Tokenizer.Peek(1).Type])
            {
                ReturnValue = ParseLiteral_0(Tokenizer);
            }
            else if (LOOKTable[77,0,Tokenizer.Peek().Type]&& LOOKTable[77,1,Tokenizer.Peek(1).Type])
            {
                ReturnValue = ParseLiteral_1(Tokenizer);
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
            if(!(LOOKTable[78,0,Tokenizer.Peek().Type]&& LOOKTable[78,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Literal" + ": Expected " + "Literal_String");
                
            }
            ReturnValue = ParseLiteral_String(Tokenizer);
            return(ReturnValue);
        }
        public Literal ParseLiteral_1(MBCC.Tokenizer Tokenizer)
        {
            Literal ReturnValue = new Literal();
            if(!(LOOKTable[80,0,Tokenizer.Peek().Type]&& LOOKTable[80,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Literal" + ": Expected " + "Literal_Int");
                
            }
            ReturnValue = ParseLiteral_Int(Tokenizer);
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
            if(Tokenizer.Peek().Type != 15)
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
            if(Tokenizer.Peek().Type != 14)
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
            if(!(LOOKTable[71,0,Tokenizer.Peek().Type]&& LOOKTable[71,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "KeyArg" + ": Expected " + "Token_3");
                
            }
            ReturnValue.Name = ParseToken_3(Tokenizer);
            if(Tokenizer.Peek().Type != 7)
            {
                throw new System.Exception("Error parsing " + "KeyArg" + ": Expected " + "eq");
                
            }
            Tokenizer.ConsumeToken();
            if(!(LOOKTable[51,0,Tokenizer.Peek().Type]&& LOOKTable[51,1,Tokenizer.Peek(1).Type]))
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
            if(!(LOOKTable[67,0,Tokenizer.Peek().Type]&& LOOKTable[67,1,Tokenizer.Peek(1).Type]))
            {
                throw new System.Exception("Error parsing " + "Expression_FuncCall" + ": Expected " + "Token_1");
                
            }
            ReturnValue.FuncName = ParseToken_1(Tokenizer);
            if(Tokenizer.Peek().Type != 2)
            {
                throw new System.Exception("Error parsing " + "Expression_FuncCall" + ": Expected " + "lpar");
                
            }
            Tokenizer.ConsumeToken();
            if(LOOKTable[94,0,Tokenizer.Peek().Type]&& LOOKTable[94,1,Tokenizer.Peek(1).Type])
            {
                if (LOOKTable[95,0,Tokenizer.Peek().Type]&& LOOKTable[95,1,Tokenizer.Peek(1).Type])
                {
                    if (LOOKTable[87,0,Tokenizer.Peek().Type]&& LOOKTable[87,1,Tokenizer.Peek(1).Type])
                    {
                        if(!(LOOKTable[51,0,Tokenizer.Peek().Type]&& LOOKTable[51,1,Tokenizer.Peek(1).Type]))
                        {
                            throw new System.Exception("Error parsing " + "_L13" + ": Expected " + "Expression");
                            
                        }
                        ReturnValue.Args.Add(ParseExpression(Tokenizer));
                        
                    }
                    else if (LOOKTable[88,0,Tokenizer.Peek().Type]&& LOOKTable[88,1,Tokenizer.Peek(1).Type])
                    {
                        if(!(LOOKTable[82,0,Tokenizer.Peek().Type]&& LOOKTable[82,1,Tokenizer.Peek(1).Type]))
                        {
                            throw new System.Exception("Error parsing " + "_L13" + ": Expected " + "KeyArg");
                            
                        }
                        ReturnValue.KeyArgs.Add(ParseKeyArg(Tokenizer));
                        
                    }
                    else
                    {
                        throw new System.Exception("Error parsing " + "_L16" + ": Expected " + "_L16");
                        
                    }
                    while(LOOKTable[92,0,Tokenizer.Peek().Type]&& LOOKTable[92,1,Tokenizer.Peek(1).Type])
                    {
                        if (LOOKTable[93,0,Tokenizer.Peek().Type]&& LOOKTable[93,1,Tokenizer.Peek(1).Type])
                        {
                            if(Tokenizer.Peek().Type != 5)
                            {
                                throw new System.Exception("Error parsing " + "_L15" + ": Expected " + "comma");
                                
                            }
                            Tokenizer.ConsumeToken();
                            if (LOOKTable[90,0,Tokenizer.Peek().Type]&& LOOKTable[90,1,Tokenizer.Peek(1).Type])
                            {
                                if(!(LOOKTable[51,0,Tokenizer.Peek().Type]&& LOOKTable[51,1,Tokenizer.Peek(1).Type]))
                                {
                                    throw new System.Exception("Error parsing " + "_L14" + ": Expected " + "Expression");
                                    
                                }
                                ReturnValue.Args.Add(ParseExpression(Tokenizer));
                                
                            }
                            else if (LOOKTable[91,0,Tokenizer.Peek().Type]&& LOOKTable[91,1,Tokenizer.Peek(1).Type])
                            {
                                if(!(LOOKTable[82,0,Tokenizer.Peek().Type]&& LOOKTable[82,1,Tokenizer.Peek(1).Type]))
                                {
                                    throw new System.Exception("Error parsing " + "_L14" + ": Expected " + "KeyArg");
                                    
                                }
                                ReturnValue.KeyArgs.Add(ParseKeyArg(Tokenizer));
                                
                            }
                            else
                            {
                                throw new System.Exception("Error parsing " + "_L15" + ": Expected " + "_L15");
                                
                            }
                            
                        }
                        else
                        {
                            throw new System.Exception("Error parsing " + "_L16" + ": Expected " + "_L16");
                            
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
            return new MBCC.Tokenizer("\\s*","\\{","\\}","\\(","\\)",":",",",";","=","stats","visuals","activated","triggered","continouss","<|<=|>|>=","\\d+","\"([^\"\\\\\\\\]|\\\\\\\\.)*\"","\\|\\|","&&","\\w\\w*");
        }
        
    }
    
}
