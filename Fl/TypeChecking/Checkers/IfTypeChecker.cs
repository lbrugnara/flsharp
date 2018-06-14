﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Checkers
{
    class IfTypeChecker : INodeVisitor<TypeCheckerVisitor, AstIfNode, SType>
    {
        public SType Visit(TypeCheckerVisitor checker, AstIfNode ifnode)
        {
            var conditionType = ifnode.Condition.Visit(checker);

            if (conditionType != Bool.Instance)
                throw new System.Exception($"For condition needs a {Bool.Instance} expression");

            // Add a new common block for the if's boyd
            checker.EnterBlock(ScopeType.Common, $"if-then-{ifnode.GetHashCode()}");

            // Generate the if's body
            ifnode.Then?.Visit(checker);

            // Leave the if's then block
            checker.LeaveBlock();

            if (ifnode.Else != null)
            {
                // Add a block for the else's body and generate it, then leave the block
                checker.EnterBlock(ScopeType.Common, $"if-else-{ifnode.GetHashCode()}");

                ifnode.Else.Visit(checker);

                checker.LeaveBlock();
            }

            return Null.Instance;
        }
    }
}
