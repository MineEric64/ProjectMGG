using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using ProjectMGG.Ingame.Script.Keywords;

namespace ProjectMGG.Ingame.Script
{
    public class Scanner
    {
        private bool Loop(string s) => _index < s.Length; //check if the current index is not out of range

        private static int _index;
        private static int _line;
        private static int _tab;
        private static int _tabPrev;
        private static bool _isShow = false;

        public List<Token> Scan(string sourceCode)
        {
            List<Token> result = new List<Token>();

            sourceCode += '\0';
            sourceCode = sourceCode.Replace("    ", "\t"); //indentation, TODO: is available for 3 space characters?

            bool needToExit = false;
            _index = 0;
            _line = 1;
            _tab = 0;
            _tabPrev = 0;
            _isShow = false;

            if (!ArgumentKinds.IsInitialized()) ArgumentKinds.Initialize();

            while (Loop(sourceCode) && sourceCode[_index] != '\0')
            {
                if (needToExit) break;
                char ch = sourceCode[_index];
                CharType charType = GetCharType(ch);

                if (ch == '\n')
                {
                    _line++;
                    _tab = 0;
                }

                if (ch == '.' && _index + 1 < sourceCode.Length) //ex: .2
                {
                    char next = sourceCode[_index + 1];
                    if (GetCharType(next) == CharType.NumberLiteral) charType = CharType.NumberLiteral;
                }

                switch (charType)
                {
                    case CharType.WhiteSpace:
                        if (ch == '\t') _tab++;
                        _index += 1;
                        break;

                    case CharType.NumberLiteral:
                        result.Add(ScanNumberLiteral(sourceCode));
                        break;

                    case CharType.StringLiteral:
                        ProcessBlockEnd(ref result);

                        //for distinguish dialog (the issue about new line)
                        bool isPreviousString = result.Count > 0 && result.Last().Kind == ArgumentKind.StringLiteral;
                        bool isPreviousNewLine = _index - 1 >= 0 && sourceCode[_index - 1] != '\n'; //wtf?
                        if (isPreviousString && isPreviousNewLine) result.Add(new Token(ArgumentKind.Unknown));

                        result.Add(ScanStringLiteral(sourceCode));
                        break;

                    case CharType.IdentifierAndKeyword:
                        ProcessBlockEnd(ref result);
                        Token token = ScanIdentifierAndKeyword(sourceCode, out bool forShow);

                        if (token == null)
                        {
                            _index++; //comment this if the game is stopped
                                      //needToExit = true; //uncomment this if the game is stopped
                            break;
                        }

                        result.Add(token);
                        if (token.Kind == ArgumentKind.Show ||
                            (token.Kind == ArgumentKind.Identifier &&
                            token.Content == "scene") || token.Kind == ArgumentKind.Reeverb) _isShow = true;
                        else if (forShow) result.Add(new Token(ArgumentKind.Unknown)); //for distinguish new line

                        break;

                    case CharType.OperatorAndPunctuator:
                        result.Add(ScanOperatorAndPunctuator(sourceCode));
                        break;

                    case CharType.Comment:
                        ProcessBlockEnd(ref result);
                        result.Add(ScanComment(sourceCode));
                        break;

                    default:
                        ExceptionManager.Throw($"Invalid character for scanning token: '{sourceCode[_index]}'.",
                            "Script/Scanner", _line);
                        needToExit = true;
                        break;
                }
            }
            if (!Loop(sourceCode)) //the code is already ended without EndOfToken (\0)
            {
                ExceptionManager.Throw("Something went wrong. The script is ended abnormally before end of token is processed. (Script Out of Range)", "Script/Scanner", _line);
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

            if (ch == '#')
            {
                return CharType.Comment;
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

            while (Loop(sourceCode) && IsCharType(sourceCode[_index], CharType.NumberLiteral))
            {
                content += sourceCode[_index];
                _index += 1;
            }
            if (Loop(sourceCode) && sourceCode[_index] == '.') //������ (seperator)
            {
                content += sourceCode[_index];
                _index += 1;

                while (Loop(sourceCode) && IsCharType(sourceCode[_index], CharType.NumberLiteral))
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

            while (Loop(sourceCode) && IsCharType(sourceCode[_index], CharType.StringLiteral))
            {
                content += sourceCode[_index];
                _index += 1;
            }

            if (Loop(sourceCode) && sourceCode[_index] != '\'' && sourceCode[_index] != '"')
            {
                ExceptionManager.Throw("Didn't close string properly.", "Script/Scanner", _line);
            }
            _index += 1;

            return new Token(ArgumentKind.StringLiteral, content);
        }

        private Token ScanIdentifierAndKeyword(string sourceCode, out bool forShow)
        {
            forShow = false;
            string content = string.Empty;

            while (Loop(sourceCode) && IsCharType(sourceCode[_index], CharType.IdentifierAndKeyword))
            {
                content += sourceCode[_index];
                _index += 1;
            }
            if (Loop(sourceCode) && _isShow && (sourceCode[_index] == '\r' || sourceCode[_index] == '\n'))
            {
                forShow = true;
                _isShow = false;
            }

            if (Loop(sourceCode) && string.IsNullOrEmpty(content))
            {
                ExceptionManager.Throw($"Invalid character for scanning token: '{sourceCode[_index]}'.", "Script/Scanner", _line);
                return null;
            }

            ArgumentKind kind = ArgumentKinds.ToKind(content);

            if (kind == ArgumentKind.Unknown)
            {
                kind = ArgumentKind.Identifier;
            }
            return new Token(kind, content);
        }

        private Token ScanOperatorAndPunctuator(string sourceCode)
        {
            StringBuilder sb = new StringBuilder();

            while (Loop(sourceCode) && IsCharType(sourceCode[_index], CharType.OperatorAndPunctuator))
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

        private Token ScanComment(string sourceCode)
        {
            string content = string.Empty;
            _index++;

            while (Loop(sourceCode) && sourceCode[_index] != '\n')
            {
                if (sourceCode[_index] == '\r')
                {
                    _index++;
                    continue;
                }

                content += sourceCode[_index];
                _index++;
            }

            return new Token(ArgumentKind.Comment, content);
        }

        private bool IsCharType(char ch, CharType type)
        {
            switch (type)
            {
                case CharType.NumberLiteral:
                    return '0' <= ch && ch <= '9';

                case CharType.StringLiteral:
                    return ch != '\'' && ch != '"';

                case CharType.IdentifierAndKeyword:
                    return '0' <= ch && ch <= '9' ||
                            'a' <= ch && ch <= 'z' ||
                            'A' <= ch && ch <= 'Z' || ch == '_';

                case CharType.OperatorAndPunctuator:
                    return 33 <= ch && ch <= 47 ||
                            58 <= ch && ch <= 64 ||
                            91 <= ch && ch <= 96 ||
                            123 <= ch && ch <= 126;

                default:
                    return false;
            }
        }

        private void ProcessBlockEnd(ref List<Token> result)
        {
            if (_tab != _tabPrev)
            {
                if (_tab < _tabPrev) result.Add(new Token(ArgumentKind.RightBrace));
                _tabPrev = _tab;
            }
        }
    }
}