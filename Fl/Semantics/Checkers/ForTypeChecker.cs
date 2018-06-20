﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Checkers
{
    class ForTypeChecker : INodeVisitor<TypeCheckerVisitor, AstForNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstForNode fornode)
        {
            // Create a new block to contain the for's initialization
            checker.SymbolTable.EnterScope(ScopeType.Loop, $"for-{fornode.GetHashCode()}");

            // Initialize the for-block
            fornode.Init.Visit(checker);

            // Emmit the condition code
            var conditionType = fornode.Condition.Visit(checker);

            if (conditionType.Type != Bool.Instance)
                throw new System.Exception($"For condition needs a {Bool.Instance} expression");

            // Emmit the body code
            fornode.Body.Visit(checker);

            // Emmit the for's increment part
            fornode.Increment.Visit(checker);

            // Leave the for
            checker.SymbolTable.LeaveScope();

            return null;
        }
    }
}