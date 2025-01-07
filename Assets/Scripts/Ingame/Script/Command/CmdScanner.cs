using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class CmdScanner
{
    private bool Loop(string s) => _index < s.Length; //check if the current index is not out of range

    private static int _index;

    public List<CmdToken> Scan(string sourceCode)
    {
        List<CmdToken> result = new List<CmdToken>();

        _index = 0;
        sourceCode += '\0';
        bool needToExit = false;

        if (!CmdArgumentKinds.IsInitialized()) CmdArgumentKinds.Initialize();

        while (Loop(sourceCode) && sourceCode[_index] != '\0')
        {
            if (needToExit) break;
            char ch = sourceCode[_index];

            switch (GetCharType(ch))
            {
                case CharType.WhiteSpace:
                    _index += 1;
                    break;

                case CharType.StringLiteral:
                    result.Add(ScanStringLiteral(sourceCode));
                    break;

                case CharType.IdentifierAndKeyword:
                    CmdToken token = ScanIdentifierAndKeyword(sourceCode);

                    if (token == null)
                    {
                        //_index++; //comment this if the game is stopped
                        needToExit = true; //uncomment this if the game is stopped
                        break;
                    }

                    result.Add(token);
                    break;

                default:
                    ExceptionManager.Throw($"Invalid character for scanning token: '{sourceCode[_index]}'.", "Script/CmdScanner", _index);
                    needToExit = true;
                    break;
            }
        }
        if (!Loop(sourceCode)) //the code is already ended without EndOfToken (\0)
        {
            ExceptionManager.Throw("Something went wrong. The script is ended abnormally before end of token is processed. (Script Out of Range)", "Script/CmdScanner", _index);
        }

        
        result.Add(new CmdToken(CmdArgumentKind.EndOfToken));
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
            return CharType.IdentifierAndKeyword;
            //return CharType.NumberLiteral;
        }

        if (ch == '\'' || ch == '"')
        {
            return CharType.StringLiteral;
        }

        if ('a' <= ch && ch <= 'z' || 'A' <= ch && ch <= 'Z')
        {
            return CharType.IdentifierAndKeyword;
        }

        return CharType.IdentifierAndKeyword;
        //return CharType.Unknown;
    }

    //private CmdToken ScanNumberLiteral(string sourceCode)
    //{
    //    string content = string.Empty;

    //    while (Loop(sourceCode) && IsCharType(sourceCode[_index], CharType.NumberLiteral))
    //    {
    //        content += sourceCode[_index];
    //        _index += 1;        
    //    }
    //    if (Loop(sourceCode) && sourceCode[_index] == '.') //±¸ºÐÀÚ (seperator)
    //    {
    //        content += sourceCode[_index];
    //        _index += 1;

    //        while (Loop(sourceCode) && IsCharType(sourceCode[_index], CharType.NumberLiteral))
    //        {
    //            content += sourceCode[_index];
    //            _index += 1;
    //        }
    //    }

    //    return new CmdToken(CmdArgumentKind.NumberLiteral, content);
    //}

    private CmdToken ScanStringLiteral(string sourceCode)
    {
        string content = string.Empty;
        _index += 1;

        while (Loop(sourceCode) && IsCharType(sourceCode[_index], CharType.StringLiteral))
        {
            content += sourceCode[_index];
            _index += 1;
        }

        if (Loop(sourceCode) && sourceCode[_index] != '\'' && sourceCode[_index] != '"')
        {
            ExceptionManager.Throw("Didn't close string properly.", "Script/CmdScanner", _index);
        }
        _index += 1;

        return new CmdToken(CmdArgumentKind.StringLiteral, content);
    }

    private CmdToken ScanIdentifierAndKeyword(string sourceCode)
    {
        string content = string.Empty;

        while (Loop(sourceCode) && IsCharType(sourceCode[_index], CharType.IdentifierAndKeyword))
        {
            content += sourceCode[_index];
            _index += 1;
        }

        if (Loop(sourceCode) && string.IsNullOrEmpty(content))
        {
            ExceptionManager.Throw($"Invalid character for scanning token: '{sourceCode[_index]}'.", "Script/CmdScanner", _index);
            return null;
        }

        CmdArgumentKind kind = CmdArgumentKinds.ToKind(content);

        if (kind == CmdArgumentKind.Unknown)
        {
            kind = CmdArgumentKind.Identifier;
        }
        return new CmdToken(kind, content);
    }

    private bool IsCharType(char ch, CharType type)
    {
        switch (type)
        {
            //case CharType.NumberLiteral:
            //    return '0' <= ch && ch <= '9';

            case CharType.StringLiteral:
                return ch != '\'' && ch != '"';

            case CharType.IdentifierAndKeyword:
                return '0' <= ch && ch <= '9' ||
                        'a' <= ch && ch <= 'z' ||
                        'A' <= ch && ch <= 'Z' || ch == '_' || (!Path.GetInvalidFileNameChars().Contains(ch) && ch != ' ');

            default:
                return false;
        }
    }
}