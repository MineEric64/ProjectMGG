using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using UnityEngine;
using SmartFormat;

public class CmdParser
{
    private int _index;
    private List<CmdToken> _tokens = new List<CmdToken>();

    public CmdParser(ref List<CmdToken> tokens)
    {
        _tokens = new List<CmdToken>(tokens);
    }

    public Program Parse()
    {
        Program result = new Program();
        _index = 0;

        while (_tokens[_index].Kind != CmdArgumentKind.EndOfToken)
        {
            var block = ParseOneBlock();

            if (block == null)
            {
                EndOfToken();
                break;
            }

            result.Blocks.Add(block);
            break;
        }

        return result;
    }

    private void SkipCurrent(CmdArgumentKind kind)
    {
        if (_tokens[_index].Kind != kind)
        {
            ExceptionManager.Throw($"Expected a token '{kind}', but got a token '{_tokens[_index].Kind}'.", "Script/CmdParser");
        }
        _index += 1;
    }

    private void SkipCurrent()
    {
        _index += 1;
    }

    private bool SkipCurrentIf(CmdArgumentKind kind)
    {
        if (_tokens[_index].Kind != kind)
        {
            return false;
        }
        _index += 1;
        return true;
    }

    private void EndOfToken()
    {
        _index = _tokens.Count - 1;
    }

    private IStatement ParseOneBlock()
    {
        switch (_tokens[_index].Kind)
        {
            case CmdArgumentKind.Day:
                return ParseDay();

            case CmdArgumentKind.EndOfToken:
                return null;

            default:
                var es = ParseExpressionStatement();

                if (es.Expression != null) return es;
                else //unsupported feature
                {
                    ExceptionManager.Throw($"Invalid Argument - {_tokens[_index]}", "Script/Parser");
                    return null;
                }
        }
    }

    private CmdDay ParseDay()
    {
        CmdDay result = new CmdDay();
        SkipCurrent();
        result.FileName = ParseExpression();

        return result;
    }

    private IExpression ParseExpression()
    {
        IExpression result = null;

        switch (_tokens[_index].Kind)
        {
            case CmdArgumentKind.StringLiteral:
                result = ParseStringLiteral();
                break;

            case CmdArgumentKind.Identifier:
                result = ParseIdentifier();
                break;

            default:
                ExceptionManager.Throw($"Invalid Operand Expression '{_tokens[_index].Kind}'.", "Script/CmdParser");
                break;
        }

        return ParsePostfix(result);
    }

    private StringLiteral ParseStringLiteral()
    {
        StringLiteral result = new StringLiteral();
        result.Value = ConvertToSyntax(_tokens[_index].Content);
        SkipCurrent(CmdArgumentKind.StringLiteral);
        return result;
    }

    private static string ConvertToSyntax(string text)
    {
        string text2 = text;

        text2 = text2.Replace("[playername:은]", Smart.Format("{0:은}", ParamManager.PlayerName));
        text2 = text2.Replace("[playername:는]", Smart.Format("{0:는}", ParamManager.PlayerName));
        text2 = text2.Replace("[playername:이]", Smart.Format("{0:이}", ParamManager.PlayerName));
        text2 = text2.Replace("[playername:가]", Smart.Format("{0:가}", ParamManager.PlayerName));

        text2 = text2.Replace("[playername2:은]", Smart.Format("{0:은}", ParamManager.PlayerName2));
        text2 = text2.Replace("[playername2:는]", Smart.Format("{0:는}", ParamManager.PlayerName2));
        text2 = text2.Replace("[playername2:이]", Smart.Format("{0:이}", ParamManager.PlayerName2));
        text2 = text2.Replace("[playername2:가]", Smart.Format("{0:가}", ParamManager.PlayerName2));
        text2 = text2.Replace("[playername2:야]", Smart.Format("{0:야}", ParamManager.PlayerName2));

        text2 = text2.Replace("[playername]", ParamManager.PlayerName);
        text2 = text2.Replace("[playername2]", ParamManager.PlayerName2);

        text2 = text2.Replace("\\n", "\n");

        return text2;
    }

    private GetVariable ParseIdentifier(bool allowWhiteSpace = false, bool allowKeyword = true)
    {
        GetVariable result = new GetVariable();
        result.IsCommand = true;

        if (allowWhiteSpace)
        {
            var sb = new StringBuilder();

            while (true)
            {
                if (!allowKeyword && _tokens[_index].Kind != CmdArgumentKind.Identifier) break;
                if (_tokens[_index].Kind == CmdArgumentKind.EndOfToken) break;

                sb.Append(_tokens[_index].Content);
                SkipCurrent();
            }

            result.Name = sb.ToString();
        }
        else
        {
            result.Name = _tokens[_index].Content;

            if (allowKeyword) SkipCurrent();
            else SkipCurrent(CmdArgumentKind.Identifier);
        }

        return result;
    }

    private IExpression ParsePostfix(IExpression sub) //identifier : (), []
    {
        while (true)
        {
            switch (_tokens[_index].Kind)
            {
                case CmdArgumentKind.LeftParen:
                    sub = ParseCall(sub); //function call
                    break;

                case CmdArgumentKind.LeftBracket:
                    sub = ParseElement(sub); //index access
                    break;

                default:
                    return sub;
            }
        }
    }

    private ExpressionStatement ParseExpressionStatement()
    {
        ExpressionStatement result = new ExpressionStatement();
        result.Expression = ParseExpression();
        return result;
    }

    private IExpression ParseCall(IExpression sub)
    {
        Call result = new Call();
        result.Sub = sub;
        SkipCurrent(CmdArgumentKind.LeftParen);

        if (_tokens[_index].Kind != CmdArgumentKind.RightParen)
        {
            do
            {
                result.Arguments.Add(ParseExpression());
            } while (SkipCurrentIf(CmdArgumentKind.Comma));
        }
        SkipCurrent(CmdArgumentKind.RightParen);
        return result;
    }

    private IExpression ParseElement(IExpression sub)
    {
        GetElement result = new GetElement();
        result.Sub = sub;
        SkipCurrent(CmdArgumentKind.LeftBracket);
        result.Index = ParseExpression();
        SkipCurrent(CmdArgumentKind.RightBracket);

        return result;
    }
}
