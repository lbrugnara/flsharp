﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Collections.Generic;

namespace Fl
{
    public class Lexer
    {
        /// <summary>
        /// Input source
        /// </summary>
        private string _Source;
        
        /// <summary>
        /// Keep track of the current char of source that is being pointed
        /// </summary>
        private int _Pointer;
        
        /// <summary>
        /// Track lines numbers
        /// </summary>
        private int _Line;

        /// <summary>
        /// Track column numbers
        /// </summary>
        private int _Col;

        /// <summary>
        /// Set of Fl's reserved words
        /// </summary>
        private static readonly Dictionary<string, TokenType> _Keywords = new Dictionary<string, TokenType>()
        {
            { "null", TokenType.Null },
            { "true", TokenType.Boolean },
            { "false", TokenType.Boolean },
            { "var", TokenType.Variable },
            { "const", TokenType.Constant },
            { "if", TokenType.If },
            { "else", TokenType.Else },
            { "while", TokenType.While },
            { "for", TokenType.For },
            { "break", TokenType.Break },
            { "continue", TokenType.Continue },
            { "return", TokenType.Return },
            { "fn", TokenType.Function },
            { "namespace", TokenType.Namespace }
        };

        public Lexer(string source)
        {
            _Source = source;
            _Pointer = 0;
            _Line = 1;
            _Col = 0;
        }

        #region Scanning helpers
        private void Reset()
        {
            _Pointer = 0;
            _Line = 1;
            _Col = 0;
        }

        private bool HasInput() => _Pointer < _Source.Length;

        private char Peek() => _Pointer < _Source.Length ? _Source[_Pointer] : '\0';

        private string Peek(int amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than 0");
            return _Pointer + amount < _Source.Length ? _Source.Substring(_Pointer, amount) : null;
        }

        private char Consume()
        {
            char c = _Pointer < _Source.Length ? _Source[_Pointer] : '\0';
            _Pointer++;
            _Col++;
            return c;
        }

        private string Consume(int amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than 0");
            string str = _Pointer + amount < _Source.Length ? _Source.Substring(_Pointer, amount) : null;
            _Pointer += amount;
            _Col += amount;
            return str;
        }
        #endregion

        public Token NextToken()
        {
            char c = Peek();

            // Consume new line and update line and col number
            if (c == '\n')
            {
                Consume();
                _Line++;
                _Col = 0;
            }

            // Consume Whitespaces
            while (HasInput() && char.IsWhiteSpace(Peek()))
                Consume();

            if (!HasInput())
                return null;

            // Check tokens
            Token t = CheckPunctuation()
                    ?? CheckParen()
                    ?? CheckBrace()
                    ?? CheckNumber()
                    ?? CheckLogicalOperator()
                    ?? CheckArithmeticOperators()
                    ?? CheckString()
                    ?? CheckIdentifier();

            return t;
        }

        private Token BuildToken(TokenType type, object value, int line, int col)
        {
            return new Token()
            {
                Type = type,
                Line = line,
                Col = col,
                Value = value
            };
        }

        private Token CheckPunctuation()
        {
            char c = Peek();
            int line = _Line;
            int col = _Col;
            switch (c)
            {
                case ';':
                    return BuildToken(TokenType.Semicolon, Consume(), line, col);
                case ',':
                    return BuildToken(TokenType.Comma, Consume(), line, col);
                case '.':
                    return BuildToken(TokenType.Dot, Consume(), line, col);
            }
            return null;
        }

        private Token CheckParen()
        {
            char c = Peek();
            if (c != '(' && c != ')')
                return null;
            int line = _Line;
            int col = _Col;
            return BuildToken(c == '(' ? TokenType.LeftParen : TokenType.RightParen, Consume(), line, col);
        }

        private Token CheckBrace()
        {
            char c = Peek();
            if (c != '{' && c != '}')
                return null;
            int line = _Line;
            int col = _Col;
            return BuildToken(c == '{' ? TokenType.LeftBrace : TokenType.RightBrace, Consume(), line, col);
        }

        private Token CheckNumber()
        {
            char c = Peek();
            if (!char.IsDigit(c))
                return null;

            int col = _Col;
            int line = _Line;
            TokenType type = TokenType.Integer;
            string val = Consume().ToString();
            while (HasInput())
            {
                c = Peek();
                if (char.IsDigit(c))
                {
                    val += Consume();
                }
                else if (c == '.')
                {
                    val += Consume();
                    type = TokenType.Double;
                }
                else if (c == 'M')
                {
                    if (type != TokenType.Double)
                        throw new Exception("Invalid character 'M'");
                    // Do not add the M now, just Consume and let's try what happens
                    Consume();
                    type = TokenType.Decimal;
                }
                else
                {
                    break;
                }
            }

            return BuildToken(type, val, line, col);
        }

        private Token CheckLogicalOperator()
        {
            int col = _Col;
            int line = _Line;
            TokenType? type = null;
            string val = null;
            
            char c = Peek();
            switch (c)
            {
                case '>':
                    type = TokenType.GreatThan;
                    val = Consume().ToString();
                    if (Peek() == '=')
                    {
                        type = TokenType.GreatThanEqual;
                        val += Consume();
                    }
                    break;
                case '<':
                    type = TokenType.LessThan;
                    val = Consume().ToString();
                    if (Peek() == '=')
                    {
                        type = TokenType.LessThanEqual;
                        val += Consume().ToString();
                    }
                    break;
                case '!':
                    type = TokenType.Not;
                    val = Consume().ToString();                    
                    if (Peek() == '=')
                    {
                        type = TokenType.NotEqual;
                        val += Consume();
                    }
                    break;
                case '=':
                    type = TokenType.Assignment;
                    val = Consume().ToString();
                    if (Peek() == '=')
                    {
                        type = TokenType.Equal;
                        val += Consume();
                    }
                    break;
                case '&':
                    if (Peek(2) == "&&")
                    {
                        type = TokenType.And;
                        val = Consume(2);
                    }
                    break;
                case '|':
                    if (Peek(2) == "||")
                    {
                        type = TokenType.Or;
                        val = Consume(2);
                    }
                    break;
                default:
                    return null;
        }

            if (!type.HasValue)
                return null;

            return BuildToken(type.Value, val, line, col);
        }

        private Token CheckArithmeticOperators()
        {
            char c = Peek();

            int col = _Col;
            int line = _Line;
            string val = null;
            TokenType? type = null;
            switch (c)
            {
                case '+':
                    type = TokenType.Addition;
                    val = Consume().ToString();
                    if (Peek() == '+')
                    {
                        type = TokenType.Increment;
                        val += Consume().ToString();
                    }
                    else if (Peek() == '=')
                    {
                        type = TokenType.IncrementAndAssign;
                        val += Consume().ToString();
                    }
                    break;
                case '-':
                    type = TokenType.Minus;
                    val = Consume().ToString();
                    if (Peek() == '-')
                    {
                        type = TokenType.Decrement;
                        val += Consume().ToString();
                    }
                    else if (Peek() == '=')
                    {
                        type = TokenType.DecrementAndAssign;
                        val += Consume().ToString();
                    }
                    break;
                case '*':
                    type = TokenType.Multiplication;
                    val = Consume().ToString();
                    if (Peek() == '=')
                    {
                        type = TokenType.MultAndAssign;
                        val += Consume().ToString();
                    }
                    break;
                case '/':
                    type = TokenType.Division;
                    val = Consume().ToString();
                    if (Peek() == '=')
                    {
                        type = TokenType.DivideAndAssign;
                        val += Consume().ToString();
                    }
                    break;
                default:
                    return null;
            }
            return BuildToken(type.Value, val, line, col);
        }

        private Token CheckIdentifier()
        {
            char c = Peek();

            if (!char.IsLetter(c) && c != '_' && c != '$')
                return null;

            int line = _Line;
            int col = _Col;
            string val = Consume().ToString();
            while (HasInput())
            {
                c = Peek();
                if (!char.IsLetterOrDigit(c) && c != '_')
                    break;
                val += Consume();
            }

            return BuildToken(_Keywords.ContainsKey(val) ? _Keywords[val] : TokenType.Identifier, val, line, col);
        }

        private Token CheckString()
        {
            char c = Peek();

            if (c != '"')
                return null;

            int line = _Line;
            int col = _Col;
            Consume(); // first "
            string val = "";
            c = '\0';
            while (HasInput())
            {
                c = Peek();
                if (c == '"')
                    break;
                val += Consume();
            }

            if (c != '"')
                throw new Exception("Bad string");

            Consume(); // Last "

            return BuildToken(TokenType.String, val, line, col);
        }
    }
}
