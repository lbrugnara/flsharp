﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Checkers
{
    class NullCoalescingTypeChecker : INodeVisitor<TypeCheckerVisitor, AstNullCoalescingNode, SType>
    {
        public SType Visit(TypeCheckerVisitor checker, AstNullCoalescingNode nullc)
        {
            var left = nullc.Left.Visit(checker);
            var right = nullc.Right.Visit(checker);

            if (left != right)
                throw new System.Exception($"Operator ?? cannot be applied to operands of type {left} and {right}");

            return left;
        }
    }
}
