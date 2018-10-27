﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Checkers
{
    class WhileTypeChecker : INodeVisitor<TypeCheckerVisitor, WhileNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, WhileNode wnode)
        {
            // Generate an eblock instruction for the whole while-block
            checker.SymbolTable.EnterScope(ScopeType.Loop, $"while-body-{wnode.GetHashCode()}");

            // Emmit the condition code
            var conditionType = wnode.Condition.Visit(checker);

            if (conditionType.Type != Bool.Instance)
                throw new System.Exception($"For condition needs a {Bool.Instance} expression");

            // Emmit the body code
            wnode.Body.Visit(checker);

            // Leave the while-block
            checker.SymbolTable.LeaveScope();

            return null;
        }
    }
}
