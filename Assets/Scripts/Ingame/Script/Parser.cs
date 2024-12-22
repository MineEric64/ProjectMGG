using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class Parser
{
    private int _index;
    private List<Token> _tokens = new List<Token>();

    public Parser(ref List<Token> tokens)
    {
        _tokens = new List<Token>(tokens);
    }

    public Program Parse()
    {
        Program result = new Program();
        _index = 0;

        while (_tokens[_index].Kind != ArgumentKind.EndOfToken)
        {
            switch (_tokens[_index].Kind)
            {
                case ArgumentKind.Function:
                    result.Functions.Add(ParseFunction());
                    break;

                default:
                    ExceptionManager.Throw($"Invalid Argument - {_tokens[_index]}", "Script/Parser");
                    _index++;
                    break;
                    //return null;
            }
        }

        return result;
    }

    private void SkipCurrent(ArgumentKind kind)
    {
        if (_tokens[_index].Kind != kind)
        {
            ExceptionManager.Throw($"Need a token '{kind}'.", "Script/Parser");
        }
        _index += 1;
    }

    private void SkipCurrent()
    {
        _index += 1;
    }

    private bool SkipCurrentIf(ArgumentKind kind)
    {
        if (_tokens[_index].Kind != kind)
        {
            return false;
        }
        _index += 1;
        return true;
    }

    //private Function ParseFunction()
    //{
    //    Function result = new Function();
    //    SkipCurrent(ArgumentKind.Function);

    //    result.Name = _tokens[_index].Content; //identifier
    //    SkipCurrent(ArgumentKind.Identifier);

    //    SkipCurrent(ArgumentKind.LeftParen);
    //    if (_tokens[_index].Kind != ArgumentKind.RightParen)
    //    {
    //        do
    //        {
    //            result.Add(_tokens[_index].Content); //parameter
    //            SkipCurrent(ArgumentKind.Identifier);
    //        } while (SkipCurrentIf(ArgumentKind.Comma));
    //    }
    //    SkipCurrent(ArgumentKind.RightParen);

    //    SkipCurrent(ArgumentKind.LeftBrace);
    //    result.Block = ParseBlock();
    //    SkipCurrent(ArgumentKind.RightBrace);
    //    return result;
    //}

    private Function ParseFunction()
    {
        Function result = new Function();
        SkipCurrent(ArgumentKind.Function);

        result.Name = _tokens[_index].Content; //identifier
        SkipCurrent(ArgumentKind.Identifier);

        SkipCurrent(ArgumentKind.Colon);
        SkipCurrent(ArgumentKind.LeftBrace);
        result.Block = ParseBlock();
        SkipCurrentIf(ArgumentKind.RightBrace);

        return result;
    }

    private List<IStatement> ParseBlock()
    {
        List<IStatement> result = new List<IStatement>();

        while (_tokens[_index].Kind != ArgumentKind.RightBrace)
        {
            switch (_tokens[_index].Kind)
            {
                case ArgumentKind.Variable:
                    result.Add(ParseVariable());
                    break;

                case ArgumentKind.If:
                    result.Add(ParseIf());
                    break;

                case ArgumentKind.Scene:
                    result.Add(ParseScene());
                    break;

                case ArgumentKind.Reeverb:
                    result.Add(ParseReeverb());
                    break;

                case ArgumentKind.While:
                    //result.Add(ParseWhile());
                    break;

                case ArgumentKind.Jump:
                    //result.Add(ParseJump());

                    break;

                case ArgumentKind.Pass:
                    //result.Add(ParsePass());
                    break;

                case ArgumentKind.Return:
                    //result.Add(ParseReturn());
                    break;

                case ArgumentKind.EndOfToken:
                    //ExceptionManager.Throw($"Invalid Argument - {_tokens[_index]}", "Script/Parser");
                    return result;

                default:
                    var es = ParseExpressionStatement();

                    if (es.Expression != null) result.Add(es);
                    else //unsupported feature
                    {
                        ExceptionManager.Throw($"Invalid Argument - {_tokens[_index]}", "Script/Parser");
                    }
                    break;
            }
        }

        return result;
    }

    private Variable ParseVariable()
    {
        Variable result = new Variable();
        SkipCurrent(ArgumentKind.Variable);
        result.Name = _tokens[_index].Content;
        SkipCurrent(ArgumentKind.Identifier);
        SkipCurrent(ArgumentKind.Assignment);
        result.Expression = ParseExpression();

        if (result.Expression == null)
        {
            ExceptionManager.Throw($"Variable '{result.Name}' is used before it has been assigned a value.", "Script/Parser");
        }
        return result;
    }

    private If ParseIf()
    {
        If result = new If();
        SkipCurrent(ArgumentKind.If);

        do
        {
            IExpression condition = ParseExpression();

            if (condition == null)
            {
                ExceptionManager.Throw("Doesn't have a condition in if statement.", "Script/Parser");
            }
            result.Conditions.Add(condition);

            SkipCurrent(ArgumentKind.LeftBrace);
            result.Blocks.Add(ParseBlock());
            SkipCurrent(ArgumentKind.RightBrace);
        } while (SkipCurrentIf(ArgumentKind.Elif));

        if (SkipCurrentIf(ArgumentKind.Else))
        {
            SkipCurrent(ArgumentKind.LeftBrace);
            result.ElseBlock = ParseBlock();
            SkipCurrent(ArgumentKind.RightBrace);
        }

        return result;
    }

    private Reeverb ParseReeverb()
    {
        Reeverb result = new Reeverb();
        SkipCurrent();

        return result;
    }

    private Scene ParseScene()
    {
        Scene result = new Scene();
        SkipCurrent();

        result.Argument = ParseExpression();
        return result;
    }

    //private Return parseReturn()
    //{
    //    Return result = new Return();
    //    SkipCurrent(ArgumentKind.Return);
    //    result.setExpression(ParseExpression());

    //    if (result.getExpression() == null)
    //    {
    //        throw new RuntimeException("return 문에 식이 없습니다.");
    //    }
    //    skipCurrent(tokens, Kind.Semicolon);
    //    return result;
    //}

    private ExpressionStatement ParseExpressionStatement()
    {
        ExpressionStatement result = new ExpressionStatement();
        result.Expression = ParseExpression();
        return result;
    }

    private IExpression ParseExpression()
    {
        return ParseAssignment();
    }

    private IExpression ParseAssignment()
    {
        IExpression result = ParseOr();

        if (_tokens[_index].Kind != ArgumentKind.Assignment)
        {
            return result;
        }
        SkipCurrent(ArgumentKind.Assignment);

        if (result is GetVariable getVariable) {
            SetVariable setVariable = new SetVariable();
            setVariable.Name = getVariable.Name;
            setVariable.Value = ParseAssignment();

            return setVariable;
        }

        if (result is GetElement getElement) { //Array or Map
            SetElement setElement = new SetElement();
            setElement.Sub = getElement.Sub;
            setElement.Index = getElement.Index;
            setElement.Value = ParseAssignment();

            return setElement;
        }

        ExceptionManager.Throw("Invalid Assignment Operator Used.", "Script/Parser");
        return null;
    }

    private IExpression ParseOr()
    {
        IExpression result = ParseAnd();

        while (SkipCurrentIf(ArgumentKind.LogicalOr))
        {
            Or temp = new Or();

            temp.Lhs = result;
            temp.Rhs = ParseAnd();
            result = temp;
        }
        return result;
    }

    private IExpression ParseAnd()
    {
        IExpression result = ParseRelational();

        while (SkipCurrentIf(ArgumentKind.LogicalAnd))
        {
            And temp = new And();

            temp.Lhs = result;
            temp.Rhs = ParseRelational();
            result = temp;
        }
        return result;
    }

    private IExpression ParseRelational()
    {
        HashSet<ArgumentKind> operators = new HashSet<ArgumentKind>() {
                ArgumentKind.Equal,
                ArgumentKind.NotEqual,
                ArgumentKind.LessThan,
                ArgumentKind.GreaterThan,
                ArgumentKind.LessOrEqual,
                ArgumentKind.GreaterOrEqual
        };
        IExpression result = ParseArithmetic1();

        while (operators.Contains(_tokens[_index].Kind))
        {
            Relational temp = new Relational();
            temp.Kind = _tokens[_index].Kind;
            SkipCurrent();
            temp.Lhs = result;
            temp.Rhs = ParseArithmetic1();
            result = temp;
        }
        return result;
    }

    private IExpression ParseArithmetic1()
    {
        HashSet<ArgumentKind> operators = new HashSet<ArgumentKind>() {
                ArgumentKind.Add,
                ArgumentKind.Subtract
        };
        IExpression result = ParseArithmetic2();

        while (operators.Contains(_tokens[_index].Kind))
        {
            Arithmetic temp = new Arithmetic();
            temp.Kind = _tokens[_index].Kind;
            SkipCurrent();
            temp.Lhs = result;
            temp.Rhs = ParseArithmetic2();
            result = temp;
        }
        return result;
    }

    private IExpression ParseArithmetic2()
    {
        HashSet<ArgumentKind> operators = new HashSet<ArgumentKind>() {
            ArgumentKind.Multiply,
            ArgumentKind.Divide,
            ArgumentKind.Modulo
        };
        IExpression result = ParseUnary();

        while (operators.Contains(_tokens[_index].Kind))
        {
            Arithmetic temp = new Arithmetic();
            temp.Kind = _tokens[_index].Kind;
            SkipCurrent();
            temp.Lhs = result;
            temp.Rhs = ParseUnary();
            result = temp;
        }
        return result;
    }

    private IExpression ParseUnary()
    {
        HashSet<ArgumentKind> operators = new HashSet<ArgumentKind>() {
                ArgumentKind.Add,
                ArgumentKind.Subtract
        };

        while (operators.Contains(_tokens[_index].Kind))
        {
            Unary result = new Unary();

            result.Kind = _tokens[_index].Kind;
            SkipCurrent();
            result.Sub = ParseUnary();
            return result;
        }

        return ParseOperand();
    }

    private IExpression ParseOperand()
    {
        IExpression result = null;

        switch (_tokens[_index].Kind)
        {
            case ArgumentKind.TrueLiteral:
            case ArgumentKind.FalseLiteral:
                result = ParseBooleanLiteral();
                break;

            case ArgumentKind.NumberLiteral:
                result = ParseNumberLiteral();
                break;

            case ArgumentKind.StringLiteral:
                result = ParseStringLiteral();
                break;

            case ArgumentKind.LeftBracket:
                result = ParseListLiteral();
                break;

            case ArgumentKind.LeftBrace:
                result = ParseMapLiteral();
                break;

            case ArgumentKind.Identifier:
                result = ParseIdentifier();
                break;

            case ArgumentKind.LeftParen:
                result = ParseInnerExpression();
                break;

            default:
                ExceptionManager.Throw("Invalid Operand Expression.", "Script/Parser");
                break;
        }

        return ParsePostfix(result);
    }

    private IExpression ParseBooleanLiteral()
    {
        BooleanLiteral result = new BooleanLiteral();
        result.Value = _tokens[_index].Kind == ArgumentKind.TrueLiteral;
        SkipCurrent();
        return result;
    }

    private IExpression ParseNumberLiteral()
    {
        NumberLiteral result = new NumberLiteral();
        result.Value = double.Parse(_tokens[_index].Content);
        SkipCurrent(ArgumentKind.NumberLiteral);
        return result;
    }

    private IExpression ParseStringLiteral()
    {
        StringLiteral result = new StringLiteral();
        result.Value = _tokens[_index].Content;
        SkipCurrent(ArgumentKind.StringLiteral);
        return result;
    }

    private IExpression ParseListLiteral()
    {
        ArrayLiteral result = new ArrayLiteral();
        SkipCurrent(ArgumentKind.LeftBracket);

        if (_tokens[_index].Kind != ArgumentKind.RightBracket)
        {
            do
            {
                result.Values.Add(ParseExpression());
            } while (SkipCurrentIf(ArgumentKind.Comma));
        }
        SkipCurrent(ArgumentKind.RightBracket);
        return result;
    }

    private IExpression ParseMapLiteral()
    {
        MapLiteral result = new MapLiteral();
        SkipCurrent(ArgumentKind.LeftBrace);
        if (_tokens[_index].Kind != ArgumentKind.RightBrace)
        {
            do
            {
                string name = _tokens[_index].Content;
                SkipCurrent(ArgumentKind.StringLiteral);
                SkipCurrent(ArgumentKind.Colon);

                IExpression value = ParseExpression();
                result.Values[name] = value;
            } while (SkipCurrentIf(ArgumentKind.Comma));
        }
        SkipCurrent(ArgumentKind.RightBrace);
        return result;
    }

    private IExpression ParseIdentifier()
    {
        GetVariable result = new GetVariable();
        result.Name = _tokens[_index].Content;
        SkipCurrent(ArgumentKind.Identifier);
        return result;
    }

    private IExpression ParseInnerExpression()
    {
        SkipCurrent(ArgumentKind.LeftParen);
        IExpression result = ParseExpression();
        SkipCurrent(ArgumentKind.RightParen);
        return result;
    }

    private IExpression ParsePostfix(IExpression sub) //identifier : (), []
    {
        while (true)
        {
            switch (_tokens[_index].Kind)
            {
                case ArgumentKind.LeftParen:
                    //sub = ParseCall(sub); //function call
                    break;

                case ArgumentKind.LeftBracket:
                    sub = ParseElement(sub); //index access
                    break;

                default:
                    return sub;
            }
        }
    }

    private IExpression ParseElement(IExpression sub)
    {
        GetElement result = new GetElement();
        result.Sub = sub;
        SkipCurrent(ArgumentKind.LeftBracket);
        result.Index = ParseExpression();
        SkipCurrent(ArgumentKind.RightBracket);

        return result;
    }
}
