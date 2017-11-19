﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl.Parser.Ast
{
    public class AstIfNode : AstNode
    {
        public Token Keyword { get; }
        public AstNode Condition { get; }
        public AstNode Then { get; }
        public AstNode Else { get; }

        public AstIfNode(Token keyword, AstNode condition, AstNode thenbranch, AstNode elsebranch)
        {
            Keyword = keyword;
            Condition = condition;
            Then = thenbranch;
            Else = elsebranch;
        }
    }
}