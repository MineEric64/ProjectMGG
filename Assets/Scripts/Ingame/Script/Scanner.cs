using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

public class Scanner
{
    private static int _index;
    private static int _line;
    private static int _tab;
    private static int _tabPrev;

    public List<Token> Scan(string sourceCode)
    {
        List<Token> result = new List<Token>();
        sourceCode += '\0';
        _index = 0;
        _line = 1;
        _tab = 0;
        _tabPrev = 0;

        if (!ArgumentKinds.IsInitialized()) ArgumentKinds.Initialize();

        while (sourceCode[_index] != '\0')
        {
            char ch = sourceCode[_index];

            if (ch == '\n')
            {
                _line++;
                _tab = 0;
            }

            switch (GetCharType(ch))
            {
                case CharType.WhiteSpace:
                    if (ch == '\t') _tab++;
                    _index += 1;
                    break;

                case CharType.NumberLiteral:
                    result.Add(ScanNumberLiteral(sourceCode));
                    break;

                case CharType.StringLiteral:
                    result.Add(ScanStringLiteral(sourceCode));
                    break;

                case CharType.IdentifierAndKeyword:
                    if (_tab != _tabPrev)
                    {
                        if (_tab > _tabPrev) result.Add(new Token(ArgumentKind.LeftBrace));
                        else result.Add(new Token(ArgumentKind.RightBrace));
                        _tabPrev = _tab;
                    }

                    result.Add(ScanIdentifierAndKeyword(sourceCode));
                    break;

                case CharType.OperatorAndPunctuator:
                    result.Add(ScanOperatorAndPunctuator(sourceCode));
                    break;

                default:
                    ExceptionManager.Throw("Can't interpret the token.", "Script/Scanner", _line);
                    //return result;
                    break;
            }
        }

        result.Add(new Token(ArgumentKind.EndOfToken));
        return result;
    }

    private CharType GetCharType(char ch)
    {                             
        if (' ' == ch || '\t' == ch || '\r' == ch || '\n' == ch)
        {
            return CharType.WhiteSpace;
        }

        if ('0' <= ch && ch <= '9')
        {
            return CharType.NumberLiteral;
        }

        if (ch == '\'' || ch == '"')
        {
            return CharType.StringLiteral;
        }

        if ('a' <= ch && ch <= 'z' || 'A' <= ch && ch <= 'Z')
        {
            return CharType.IdentifierAndKeyword;
        }

        if (33 <= ch && ch <= 47 && ch != '\'' ||
                58 <= ch && ch <= 64 ||
                91 <= ch && ch <= 96 ||
                123 <= ch && ch <= 126)
        {
            return CharType.OperatorAndPunctuator;
        }

        return CharType.Unknown;
    }

    private Token ScanNumberLiteral(string sourceCode)
    {
        string content = string.Empty;

        while (IsCharType(sourceCode[_index], CharType.NumberLiteral))
        {
            content += sourceCode[_index];
            _index += 1;
        }
        if (sourceCode[_index] == '.') //±¸ºÐÀÚ (seperator)
        {
            content += sourceCode[_index];
            _index += 1;

            while (IsCharType(sourceCode[_index], CharType.NumberLiteral))
            {
                content += sourceCode[_index];
                _index += 1;
            }
        }

        return new Token(ArgumentKind.NumberLiteral, content);
    }

    private Token ScanStringLiteral(string sourceCode)
    {
        string content = string.Empty;
        _index += 1;

        while (IsCharType(sourceCode[_index], CharType.StringLiteral))
        {
            content += sourceCode[_index];
            _index += 1;
        }

        if (sourceCode[_index] != '\'' && sourceCode[_index] != '"')
        {
            ExceptionManager.Throw("Didn't close string properly.", "Script/Scanner", _line);
        }
        _index += 1;

        return new Token(ArgumentKind.StringLiteral, content);
    }

    private Token ScanIdentifierAndKeyword(String sourceCode)
    {
        string content = string.Empty;

        while (IsCharType(sourceCode[_index], CharType.IdentifierAndKeyword))
        {
            content += sourceCode[_index];
            _index += 1;
        }

        ArgumentKind kind = ArgumentKinds.ToKind(content);

        if (kind == ArgumentKind.Unknown)
        {
            kind = ArgumentKind.Identifier;
        }
        return new Token(kind, content);
    }

    private Token ScanOperatorAndPunctuator(String sourceCode)
    {
        StringBuilder sb = new StringBuilder();

        while (IsCharType(sourceCode[_index], CharType.OperatorAndPunctuator))
        {
            sb.Append(sourceCode[_index]);
            _index += 1;
        }
        while ((sb.Length > 0) && ArgumentKinds.ToKind(sb.ToString()) == ArgumentKind.Unknown)
        {
            sb.Remove(sb.Length - 1, 1);
            _index -= 1;
        }

        if (sb.Length == 0)
        {
            ExceptionManager.Throw($"Can't use the character '{sourceCode[_index]}'.", "Script/Scanner", _line);
        }

        string content = sb.ToString();
        return new Token(ArgumentKinds.ToKind(content), content);
    }

    private bool IsCharType(char ch, CharType type)
    {
        switch (type)
        {
            case CharType.NumberLiteral:
                    return '0' <= ch && ch <= '9';

            case CharType.StringLiteral:
                    return 32 <= ch && ch <= 126 && ch != '\'' && ch != '"';

            case CharType.IdentifierAndKeyword:
                    return '0' <= ch && ch <= '9' ||
                            'a' <= ch && ch <= 'z' ||
                            'A' <= ch && ch <= 'Z';

            case CharType.OperatorAndPunctuator:
                    return 33 <= ch && ch <= 47 ||
                            58 <= ch && ch <= 64 ||
                            91 <= ch && ch <= 96 ||
                            123 <= ch && ch <= 126;

            default:
                return false;
        }
    }
}