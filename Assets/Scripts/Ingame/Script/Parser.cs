using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using UnityEngine;
using SmartFormat;

using ProjectMGG.Ingame.Script.Keywords;
using ProjectMGG.Ingame.Script.Keywords.Renpy;
using ProjectMGG.Ingame.Script.Keywords.Renpy.ATL;
using ProjectMGG.Ingame.Script.Keywords.Renpy.Transitions;

using Ease = PrimeTween.Ease;

namespace ProjectMGG.Ingame.Script
{
    public class Parser
    {
        private int _index;
        private List<Token> _tokens = new List<Token>();

        private bool _skipPostfix = false; //ex: is 'fx nc [360, 360]' index access? or identifier & list?

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
                        var function = ParseFunction();

                        if (function == null || function.Block == null)
                        {
                            EndOfToken();
                            break;
                        }

                        result.Functions.Add(function);
                        break;

                    default:
                        var block = ParseOneBlock();

                        if (block == null)
                        {
                            EndOfToken();
                            break;
                        }

                        result.Blocks.Add(block);
                        break;
                }
            }

            return result;
        }

        private void SkipCurrent(ArgumentKind kind)
        {
            if (_tokens[_index].Kind != kind)
            {
                ExceptionManager.Throw($"Expected a token '{kind}', but got a token '{_tokens[_index].Kind}'.", "Script/Parser");
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

        private void EndOfToken()
        {
            _index = _tokens.Count - 1;
        }

        /// <summary>
        /// Check for selective argument in statement
        /// </summary>
        private bool IsUnknown(ArgumentKind kind, int offset)
        {
            return _index + offset < _tokens.Count && _tokens[_index + offset].Kind == kind && _tokens[_index + offset].Line == _tokens[_index + offset - 1].Line;
        }

        private Function ParseFunction()
        {
            Function result = new Function();
            result.Line = _tokens[_index].Line;
            SkipCurrent(ArgumentKind.Function);     

            result.Name = _tokens[_index].Content; //identifier
            SkipCurrent(ArgumentKind.Identifier);

            SkipCurrent(ArgumentKind.Colon); //equals to LeftBrace
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

                    case ArgumentKind.Image:
                        result.Add(ParseImage());
                        break;

                    case ArgumentKind.StringLiteral: //narration
                        if (IsUnknown(ArgumentKind.StringLiteral, 1)) //it's dialog (ex: "temporary character name" "text")
                        {
                            result.Add(ParseDialog());
                            break;
                        }

                        result.Add(ParseNarration());
                        break;

                    case ArgumentKind.Identifier: //dialog
                        var dialog = ParseDialog();
                           
                        if (dialog == null) goto default; //it's real and pure identifier
                        else result.Add(dialog);
                        break;

                    case ArgumentKind.If:
                        result.Add(ParseIf());
                        break;

                    case ArgumentKind.Transform:
                        var t = ParseTransform();

                        if (t == null) return null;
                        result.Add(t);
                        break;

                    case ArgumentKind.Menu:
                        result.Add(ParseMenu());
                        break;

                    case ArgumentKind.Show:
                        result.Add(ParseShow());
                        break;

                    case ArgumentKind.Hide:
                        result.Add(ParseShow(false, true));
                        break;

                    case ArgumentKind.Scene:
                        result.Add(ParseShow(true));
                        break;

                    case ArgumentKind.With:
                        result.Add(ParseWith(true));
                        break;

                    case ArgumentKind.Window:
                        result.Add(ParseWindow());
                        break;

                    case ArgumentKind.Play:
                        result.Add(ParsePlay());
                        break;

                    case ArgumentKind.Stop:
                        result.Add(ParseStop());
                        break;

                    case ArgumentKind.Reeverb:
                        result.Add(ParseReeverb());
                        break;

                    case ArgumentKind.Fx:
                        result.Add(ParseFX());
                        break;

                    case ArgumentKind.Pause:
                        result.Add(ParsePause());
                        break;

                    case ArgumentKind.While:
                        //result.Add(ParseWhile());
                        break;

                    case ArgumentKind.Jump:
                        result.Add(ParseJump());
                        break;

                    case ArgumentKind.Call:
                        result.Add(ParseCall());
                        break;

                    case ArgumentKind.Pass:
                        result.Add(ParsePass());
                        break;

                    case ArgumentKind.Return:
                        result.Add(ParseReturn());
                        break;

                    case ArgumentKind.Comment:
                        result.Add(ParseComment());
                        break;

                    case ArgumentKind.EndOfToken:
                        return result;

                    default:
                        var es = ParseExpressionStatement();

                        if (es.Expression != null) result.Add(es);
                        else //unsupported feature
                        {
                            ExceptionManager.Throw($"Invalid Argument - {_tokens[_index]}", "Script/Parser");
                            SkipCurrent();
                        }
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// for Exterior Block
        /// </summary>
        private IStatement ParseOneBlock()
        {
            switch (_tokens[_index].Kind)
            {
                case ArgumentKind.Variable:
                    return ParseVariable(true);

                case ArgumentKind.Image:
                    return ParseImage(true);

                case ArgumentKind.Transform:
                    return ParseTransform(true);

                case ArgumentKind.EndOfToken:
                    return null;

                case ArgumentKind.Comment:
                    return ParseComment();

                default:
                    ExceptionManager.Throw($"Invalid Argument - {_tokens[_index]}", "Script/Parser");
                    return null;
            }
        }

        private Variable ParseVariable(bool isGlobal = false)
        {
            Variable result = new Variable();
            result.Line = _tokens[_index].Line;
            SkipCurrent(ArgumentKind.Variable);
            result.Name = ParseIdentifier();
            SkipCurrent(ArgumentKind.Assignment);
            result.Expression = ParseExpression();
            result.IsGlobal = isGlobal;

            if (result.Expression == null)
            {
                ExceptionManager.Throw($"Variable '{result.Name}' is used before it has been assigned a value.", "Script/Parser");
            }
            return result;
        }

        private Image ParseImage(bool isGlobal = false)
        {
            Image result = new Image();
            result.Line = _tokens[_index].Line;
            SkipCurrent(ArgumentKind.Image);

            result.Tag = ParseIdentifier();
            result.Attributes = ParseIdentifier(true);
            SkipCurrent(ArgumentKind.Assignment);

            result.Data = ParseExpression();
            result.IsGlobal = isGlobal;

            if (result.Data == null)
            {
                ExceptionManager.Throw($"Image '{result.Tag}' is used before it has been assigned a value.", "Script/Parser");
            }
            return result;
        }

        private If ParseIf()
        {
            If result = new If();
            result.Line = _tokens[_index].Line;
            SkipCurrent(ArgumentKind.If);

            do
            {
                IExpression condition = ParseExpression();

                if (condition == null)
                {
                    ExceptionManager.Throw("Doesn't have a condition in if statement.", "Script/Parser");
                    return null;
                }
                result.Conditions.Add(condition);

                SkipCurrent(ArgumentKind.Colon);
                result.Blocks.Add(ParseBlock());
                SkipCurrent(ArgumentKind.RightBrace);
            } while (SkipCurrentIf(ArgumentKind.Elif));

            if (SkipCurrentIf(ArgumentKind.Else))
            {
                SkipCurrent(ArgumentKind.Colon);
                result.ElseBlock = ParseBlock();
                SkipCurrent(ArgumentKind.RightBrace);
            }

            return result;
        }

        private Return ParseReturn()
        {
            Return result = new Return();
            SkipCurrent(ArgumentKind.Return);

            return result;
        }

        private Jump ParseJump()
        {
            Jump result = new Jump();
            SkipCurrent(ArgumentKind.Jump);
            result.Name = ParseExpression();

            return result;
        }

        private Pass ParsePass()
        {
            Pass result = new Pass();
            SkipCurrent(ArgumentKind.Pass);

            return result;
        }

        private Narration ParseNarration()
        {
            Narration result = new Narration();
            result.Line = _tokens[_index].Line;
            result.Argument = ParseStringLiteral();
            return result;
        }

        private Dialog ParseDialog()
        {
            if (!IsUnknown(ArgumentKind.StringLiteral, 1)) return null; //it's not dialog

            Dialog result = new Dialog();
            result.Line = _tokens[_index].Line;

            string chrName = "";
            if (_tokens[_index].Kind == ArgumentKind.StringLiteral) chrName = ParseStringLiteral();
            else if (_tokens[_index].Kind == ArgumentKind.Identifier) chrName = ParseIdentifier();

            result.CharacterName = chrName;
            result.Content = ParseStringLiteral();

            return result;
        }

        private Reeverb ParseReeverb()
        {
            Reeverb result = new Reeverb();
            result.Line = _tokens[_index].Line;
            SkipCurrent();

            if (IsUnknown(ArgumentKind.Identifier, 0) || IsUnknown(ArgumentKind.LeftBracket, 0))
                result.Intervals = ParseExpression();

            return result;
        }

        private FX ParseFX()
        {
            FX result = new FX();
            result.Line = _tokens[_index].Line;
            SkipCurrent();

            _skipPostfix = true;

            if (IsUnknown(ArgumentKind.Identifier, 0) || IsUnknown(ArgumentKind.StringLiteral, 0))
                result.Name = ParseExpression();

            if (_tokens[_index].Kind == ArgumentKind.At)
            {
                SkipCurrent();
                result.At = ParseIdentifier();
            }

            _skipPostfix = false;

            return result;
        }

        private Pause ParsePause()
        {
            Pause result = new Pause();
            result.Line = _tokens[_index].Line;
            SkipCurrent();

            if (IsUnknown(ArgumentKind.NumberLiteral, 0)) result.DelayAsExpression = ParseExpression();
            else
            {
                int line = result.Line;
                result = Pause.GetInfinity();
                result.Line = line;
            }

            //Custom syntax
            if (IsUnknown(ArgumentKind.Identifier, 0) && _tokens[_index].Content.ToLower() == "hard") result.Hard = true;
            
            return result;
        }

        private Comment ParseComment()
        {
            Comment result = new Comment();
            result.Line = _tokens[_index].Line;
            result.Content = _tokens[_index].Content;
            SkipCurrent();

            return result;
        }

        private bool IsShowKeyword(ArgumentKind kind)
        {
            return kind == ArgumentKind.At || kind == ArgumentKind.With;
        }

        private Show ParseShow(bool isScene = false, bool isHide = false)
        {
            Show result = new Show();
            result.Line = _tokens[_index].Line;
            SkipCurrent();

            result.Tag = ParseIdentifier();
            result.Attributes = ParseIdentifier(true);
            result.IsScene = isScene;
            result.IsHide = isHide;

            while (IsShowKeyword(_tokens[_index].Kind))
            {
                switch (_tokens[_index].Kind)
                {
                    case ArgumentKind.At:
                        SkipCurrent();
                        result.At = ParseIdentifier();
                        break;

                    case ArgumentKind.With:
                        result.With = ParseWith(false);
                        break;
                }
            }

            return result;
        }

        private With ParseWith(bool alone)
        {
            With result = new With(alone);

            result.Line = _tokens[_index].Line;
            SkipCurrent();
            result.Transition = ParseTransition();

            return result;
        }

        private IStatement ParseTransform(bool isGlobal = false)
        {
            RpyTransform result = new RpyTransform();
            var block = new ATLBlock();

            result.Line = _tokens[_index].Line;
            SkipCurrent(ArgumentKind.Transform);
            result.Name = ParseIdentifier();
            result.IsGlobal = isGlobal;
            SkipCurrent(ArgumentKind.Colon); //equals to LeftBrace

            while (true)
            {
                if (_index - 1 >= 0 && (_tokens[_index - 1].Line != _tokens[_index].Line)) //If line is different than previous one
                {
                    result.Blocks.Add(block);
                    block = new ATLBlock();
                }
                if (_tokens[_index].Kind == ArgumentKind.RightBrace) break; //condition

                if (_tokens[_index].Kind == ArgumentKind.Comment)
                {
                    ParseComment();
                    continue;
                }

                switch (_tokens[_index].Kind)
                {
                    case ArgumentKind.Xpos:
                        {
                            SkipCurrent();

                            var atl = new RpyPos();
                            atl.Line = _tokens[_index].Line;
                            atl.IsX = true;
                            atl.Value = ParseExpression();
                            block.Interior.Add(atl);

                            break;
                        }

                    case ArgumentKind.Ypos:
                        {
                            SkipCurrent();

                            var atl = new RpyPos();
                            atl.Line = _tokens[_index].Line;
                            atl.IsX = false;
                            atl.Value = ParseExpression();
                            block.Interior.Add(atl);

                            break;
                        }

                    case ArgumentKind.Xcenter:
                        {
                            SkipCurrent();

                            var atl = new RpyCenter();
                            atl.Line = _tokens[_index].Line;
                            atl.IsX = true;
                            atl.Value = ParseExpression();
                            block.Interior.Add(atl);

                            break;
                        }

                    case ArgumentKind.Ycenter:
                        {
                            SkipCurrent();

                            var atl = new RpyCenter();
                            atl.Line = _tokens[_index].Line;
                            atl.IsX = false;
                            atl.Value = ParseExpression();
                            block.Interior.Add(atl);

                            break;
                        }

                    case ArgumentKind.Xalign:
                        {
                            SkipCurrent();

                            var atl = new RpyAlign();
                            atl.Line = _tokens[_index].Line;
                            atl.IsX = true;
                            atl.Value = ParseExpression();
                            block.Interior.Add(atl);

                            break;
                        }

                    case ArgumentKind.Yalign:
                        {
                            SkipCurrent();

                            var atl = new RpyAlign();
                            atl.Line = _tokens[_index].Line;
                            atl.IsX = false;
                            atl.Value = ParseExpression();
                            block.Interior.Add(atl);

                            break;
                        }

                    case ArgumentKind.Xanchor:
                        {
                            SkipCurrent();

                            var atl = new RpyAnchor();
                            atl.Line = _tokens[_index].Line;
                            atl.IsX = true;
                            atl.Value = ParseExpression();
                            block.Interior.Add(atl);

                            break;
                        }

                    case ArgumentKind.Yanchor:
                        {
                            SkipCurrent();

                            var atl = new RpyAnchor();
                            atl.Line = _tokens[_index].Line;
                            atl.IsX = false;
                            atl.Value = ParseExpression();
                            block.Interior.Add(atl);

                            break;
                        }

                    case ArgumentKind.Zoom:
                        {
                            SkipCurrent();

                            var atl = new RpyZoom();
                            atl.Line = _tokens[_index].Line;
                            atl.Value = ParseExpression();
                            block.Interior.Add(atl);

                            break;
                        }

                    case ArgumentKind.Repeat:
                        {
                            SkipCurrent();

                            var atl = new RpyRepeat();
                            atl.Line = _tokens[_index].Line;
                            if (_index - 1 >= 0 && _tokens[_index - 1].Line == _tokens[_index].Line) atl.Value = ParseExpression();
                            block.Interior.Add(atl);

                            break;
                        }

                    case ArgumentKind.Identifier: //same as ease
                        {
                            var map = new Dictionary<string, Ease>()
                            {
                                { "linear", Ease.Linear },
                                { "ease", Ease.InOutSine },
                                { "easein", Ease.InSine },
                                { "easeout", Ease.OutSine }
                            };

                            if (map.TryGetValue(_tokens[_index].Content, out Ease ease))
                            {
                                block.EaseEnabled = true;
                                block.EaseKind = ease;

                                SkipCurrent();
                                if (_index - 1 >= 0 && _tokens[_index - 1].Line == _tokens[_index].Line) block.EaseDurationAsExpression = ParseExpression();
                                else ExceptionManager.Throw("Failed to parse ease duration's value in transform. Line is not same as ease syntax.", "Script/Parser", _tokens[_index].Line);
                            }
                            else goto default;

                            break;
                        }

                    case ArgumentKind.Pause:
                        {
                            SkipCurrent();

                            block.EaseEnabled = true;
                            block.EaseDurationAsExpression = ParseExpression();
                            result.Blocks.Add(block);
                            block = new ATLBlock();

                            break;
                        }

                    //Custom syntax
                    case ArgumentKind.Colour:
                        {
                            SkipCurrent();

                            var atl = new RpyColour();
                            atl.Line = _tokens[_index].Line;
                            atl.Value = ParseExpression();
                            block.Interior.Add(atl);

                            break;
                        }

                    default:
                        ExceptionManager.Throw($"Invalid attribute '{_tokens[_index].Content}' on transform keyword.", "Script/Parser");
                        return null;
                }
            }

            SkipCurrentIf(ArgumentKind.RightBrace);
            return result;
        }

        private IStatement ParseWindow()
        {
            var result = new Window();

            result.Line = _tokens[_index].Line;
            SkipCurrent(ArgumentKind.Window);
            if (SkipCurrentIf(ArgumentKind.Show))
            {
                result.Method = 0;
                result.Transition = ParseTransition(true);
            }
            else if (SkipCurrentIf(ArgumentKind.Hide))
            {
                result.Method = 1;
                result.Transition = ParseTransition(true);
            }
            else
            {
                bool invalid = true;

                if (_tokens[_index].Kind == ArgumentKind.Identifier)
                {
                    string name = ParseIdentifier();

                    if (name == "auto")
                    {
                        var exp = ParseExpression();

                        if (exp != null && exp.Interpret() is bool value)
                        {
                            invalid = false;

                            if (value) result.Method = 2;
                            else result.Method = 3;
                        }
                    }
                }

                if (invalid)
                {
                    ExceptionManager.Throw($"Invalid attribute '{_tokens[_index].Content}' on window keyword.", "Script/Parser");
                    return null;
                }
            }

            return result;
        }

        private IStatement ParseMenu(bool isGlobal = false)
        {
            var result = new Menu();

            result.Line = _tokens[_index].Line;
            SkipCurrent(ArgumentKind.Menu);
            SkipCurrent(ArgumentKind.Colon); //equals to LeftBrace

            Stack<bool> s = new Stack<bool>();
            s.Push(true); //LeftBrace

            while (s.Count > 0)
            {
                if (s.Count == 1) //StringLiteral (Menu Name)
                {
                    if (SkipCurrentIf(ArgumentKind.RightBrace))
                    {
                        s.Pop();
                        break;
                    }

                    string name = ParseStringLiteral();

                    if (_tokens[_index].Kind == ArgumentKind.Colon) //Menu Name
                    {
                        result.Names.Add(name);
                        result.Count++;
                    }
                    else //Head
                    {
                        result.Head = name;
                    }
                }
                else //Blocks
                {
                    var block = ParseBlock();
                    result.Blocks.Add(block);
                }

                if (SkipCurrentIf(ArgumentKind.Colon)) s.Push(true);
                else if (SkipCurrentIf(ArgumentKind.RightBrace)) s.Pop();
                else if (_tokens[_index].Kind == ArgumentKind.EndOfToken) break;
            }

            return result;
        }

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

            if (result is GetVariable getVariable)
            {
                SetVariable setVariable = new SetVariable();
                setVariable.Name = getVariable.Name;
                setVariable.Value = ParseAssignment();

                return setVariable;
            }

            if (result is GetElement getElement)
            { //Array or Map
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

                case ArgumentKind.NullLiteral:
                    result = new NullLiteral();
                    SkipCurrent();
                    break;

                case ArgumentKind.NumberLiteral:
                    result = ParseNumberLiteral();
                    break;

                case ArgumentKind.StringLiteral:
                    result = ParseStringLiteral();
                    break;

                case ArgumentKind.Character:
                    result = ParseCharacter();
                    break;

                case ArgumentKind.Solid:
                    result = ParseSolid();
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
                    ExceptionManager.Throw($"Invalid Operand Expression '{_tokens[_index].Kind}'.", "Script/Parser");
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

        private NumberLiteral ParseNumberLiteral()
        {
            NumberLiteral result = new NumberLiteral();
            result.Value = float.Parse(_tokens[_index].Content);
            result.IsFloat = _tokens[_index].Content.Contains('.');
            SkipCurrent(ArgumentKind.NumberLiteral);
            return result;
        }

        private StringLiteral ParseStringLiteral()
        {
            StringLiteral result = new StringLiteral();
            result.Value = ConvertToSyntax(_tokens[_index].Content);
            SkipCurrent(ArgumentKind.StringLiteral);
            return result;
        }

        private static string ConvertToSyntax(string text)
        {
            string text2 = text;

            text2 = text2.Replace("[playername:은]", Smart.Format("{0:은}", IngameManagerV2.PlayerName));
            text2 = text2.Replace("[playername:는]", Smart.Format("{0:는}", IngameManagerV2.PlayerName));
            text2 = text2.Replace("[playername:이]", Smart.Format("{0:이}", IngameManagerV2.PlayerName));
            text2 = text2.Replace("[playername:가]", Smart.Format("{0:가}", IngameManagerV2.PlayerName));

            text2 = text2.Replace("[playername2:은]", Smart.Format("{0:은}", IngameManagerV2.PlayerName2));
            text2 = text2.Replace("[playername2:는]", Smart.Format("{0:는}", IngameManagerV2.PlayerName2));
            text2 = text2.Replace("[playername2:이]", Smart.Format("{0:이}", IngameManagerV2.PlayerName2));
            text2 = text2.Replace("[playername2:가]", Smart.Format("{0:가}", IngameManagerV2.PlayerName2));
            text2 = text2.Replace("[playername2:야]", Smart.Format("{0:야}", IngameManagerV2.PlayerName2));

            text2 = text2.Replace("[playername]", IngameManagerV2.PlayerName);
            text2 = text2.Replace("[playername2]", IngameManagerV2.PlayerName2);

            text2 = text2.Replace("\\n", "\n");

            return text2;
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

        private GetVariable ParseIdentifier(bool allowWhiteSpace = false, bool allowKeyword = true)
        {
            GetVariable result = new GetVariable();
            result.IsCommand = true;

            if (allowWhiteSpace)
            {
                var sb = new StringBuilder();

                while (true)
                {
                    if (!allowKeyword && _tokens[_index].Kind != ArgumentKind.Identifier) break;
                    if (_tokens[_index].Kind == ArgumentKind.Assignment) break; //image
                    if (IsShowKeyword(_tokens[_index].Kind)) break; //with
                    if (_tokens[_index - 1].Line != _tokens[_index].Line) break; //new line

                    sb.Append(_tokens[_index].Content);
                    SkipCurrent();
                }

                result.Name = sb.ToString();
            }
            else
            {
                result.Name = _tokens[_index].Content;

                if (allowKeyword) SkipCurrent();
                else SkipCurrent(ArgumentKind.Identifier);
            }


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
            if (_skipPostfix) return sub;

            while (true)
            {
                switch (_tokens[_index].Kind)
                {
                    case ArgumentKind.LeftParen:
                        sub = ParseCall(sub); //function call
                        break;

                    case ArgumentKind.LeftBracket:
                        sub = ParseElement(sub); //index access
                        break;

                    default:
                        return sub;
                }
            }
        }

        private ExpressionStatement ParseCall()
        {
            var result = new ExpressionStatement();
            SkipCurrent(ArgumentKind.Call);
            result.Expression = ParseCall(ParseExpression());
            return result;
        }

        private IExpression ParseCall(IExpression sub)
        {
            Call result = new Call();
            result.Sub = sub;

            if (SkipCurrentIf(ArgumentKind.LeftParen))
            {
                if (_tokens[_index].Kind != ArgumentKind.RightParen)
                {
                    do
                    {
                        result.Arguments.Add(ParseExpression());
                    } while (SkipCurrentIf(ArgumentKind.Comma));
                }
                SkipCurrent(ArgumentKind.RightParen);
            }

            return result;
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

        private void ParseClass(Dictionary<string, Action> essential, Dictionary<string, Action> args = null)
        {
            SkipCurrent();
            SkipCurrent(ArgumentKind.LeftParen);

            foreach (string key in essential.Keys.ToList())
            {
                if (_index + 1 < _tokens.Count)
                {
                    if (_tokens[_index + 1].Kind == ArgumentKind.Assignment) break;
                    essential[key].Invoke();
                    essential[key] = null;
                }
                SkipCurrentIf(ArgumentKind.Comma);
            }
            
            if (_tokens[_index].Kind != ArgumentKind.RightParen)
            {
                do
                {
                    var varName = _tokens[_index].Content;
                    SkipCurrent();
                    SkipCurrent(ArgumentKind.Assignment);

                    if (args != null && args.TryGetValue(varName, out var action))
                    {
                        action?.Invoke();
                        args[varName] = null;
                    }
                    else
                    {
                        SkipCurrent();
                    }
                } while (SkipCurrentIf(ArgumentKind.Comma));
            }
            SkipCurrent(ArgumentKind.RightParen);
        }

        private IExpression ParseCharacter()
        {
            Character result = new Character();
            var essential = new Dictionary<string, Action>();
            var args = new Dictionary<string, Action>();

            essential.Add("name", () => result.Name = ParseExpression());
            args.Add("color", () =>
            {
                string content = ParseExpression()?.Interpret() as string;

                if (!ColorUtility.TryParseHtmlString(content, out var color))
                {
                    ExceptionManager.Throw("Invalid Color format on Character Class.", "Script/Parser");
                    SkipCurrent();
                    return;
                }
                result.Colour = color;
            });
            ParseClass(essential, args);

            return result;
        }

        private IExpression ParseSolid()
        {
            Solid result = new Solid();
            var essential = new Dictionary<string, Action>();

            essential.Add("color", () =>
            {
                string content = ParseExpression()?.Interpret() as string;

                if (!ColorUtility.TryParseHtmlString(content, out var color))
                {
                    ExceptionManager.Throw("Invalid Color format on Character Class.", "Script/Parser");
                    SkipCurrent();
                    return;
                }
                result.Colour = color;
            });
            ParseClass(essential);

            return result;
        }

        private static string[] _transitionIdentifierNames = new string[] { "fade", "dissolve" };

        private IPause ParseTransition(bool checkUnknown = false)
        {
            IPause result = null;

            switch (_tokens[_index].Kind)
            {
                case ArgumentKind.Dissolve:
                    result = ParseDissolve();
                    break;

                case ArgumentKind.Fade:
                    result = ParseFade();
                    break;

                case ArgumentKind.Identifier:
                    if (checkUnknown && (!IsUnknown(ArgumentKind.Identifier, 1) || !_transitionIdentifierNames.Contains(_tokens[_index].Content))) return null;
                    result = ParseIdentifier();
                    break;
            }

            return result;
        }

        private IPause ParseDissolve()
        {
            Dissolve result = new Dissolve();

            SkipCurrent();
            SkipCurrent(ArgumentKind.LeftParen);
            
            result.Time = ParseExpression();
            
            SkipCurrent(ArgumentKind.RightParen);
            return result;
        }

        private IPause ParseFade()
        {
            Fade result = new Fade();

            SkipCurrent();
            SkipCurrent(ArgumentKind.LeftParen);

            result.OutTime = ParseExpression();
            SkipCurrent(ArgumentKind.Comma);
            result.HoldTime = ParseExpression();
            SkipCurrent(ArgumentKind.Comma);
            result.InTime = ParseExpression();
            SkipCurrentIf(ArgumentKind.Comma);

            if (_tokens[_index].Kind != ArgumentKind.RightParen)
            {
                do
                {
                    var varName = _tokens[_index].Content;
                    SkipCurrent();
                    SkipCurrent(ArgumentKind.Assignment);

                    switch (varName)
                    {
                        case "color":
                            {
                                string content = ParseExpression()?.Interpret() as string;

                                if (!ColorUtility.TryParseHtmlString(content, out var color))
                                {
                                    ExceptionManager.Throw("Invalid Color format on Character Class.", "Script/Parser");
                                    SkipCurrent();
                                    break;
                                }
                                result.Colour = color;
                                break;
                            }
                    }

                } while (SkipCurrentIf(ArgumentKind.Comma));
            }

            SkipCurrent(ArgumentKind.RightParen);
            return result;
        }

        private IStatement ParsePlay()
        {
            var result = new RpyAudio();

            result.Line = _tokens[_index].Line;
            SkipCurrent();
            result.State = RpyAudioStates.Play;
            result.Channel = ParseIdentifier();
            result.Path = ParseExpression();

            while (IsUnknown(ArgumentKind.Identifier, 0))
            {
                if (_tokens[_index].Content == "fadein" && _tokens[_index].Line == _tokens[_index + 1].Line)
                {
                    SkipCurrent();
                    result.fadein = ParseExpression();
                }
                else if (_tokens[_index].Content == "fadeout" && _tokens[_index].Line == _tokens[_index + 1].Line)
                {
                    SkipCurrent();
                    result.fadeout = ParseExpression();
                }
                else if (_tokens[_index].Content == "volume" && _tokens[_index].Line == _tokens[_index + 1].Line)
                {
                    SkipCurrent();
                    result.volume = ParseExpression();
                }
                else if (_tokens[_index].Content == "if_changed")
                {
                    SkipCurrent();
                    result.if_changed = true;
                }
                else if (_tokens[_index].Content == "loop")
                {
                    SkipCurrent();
                    result.isloop = 1;
                }
                else if (_tokens[_index].Content == "noloop")
                {
                    SkipCurrent();
                    result.isloop = 0;
                }
            }

            return result;
        }

        private IStatement ParseStop()
        {
            var result = new RpyAudio();

            result.Line = _tokens[_index].Line;
            SkipCurrent();
            result.State = RpyAudioStates.Stop;
            result.Channel = ParseIdentifier();

            if (IsUnknown(ArgumentKind.Identifier, 0) && _tokens[_index].Content == "fadeout" && _tokens[_index].Line == _tokens[_index + 1].Line)
            {
                SkipCurrent();
                result.fadeout = ParseExpression();
            }

            return result;
        }
    }
}