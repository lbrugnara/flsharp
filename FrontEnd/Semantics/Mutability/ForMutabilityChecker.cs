﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;

namespace Zenit.Semantics.Mutability
{
    class ForMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, ForNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, ForNode fornode)
        {
            // Create a new block to contain the for's initialization
            checker.SymbolTable.EnterLoopScope($"{fornode.Uid}");

            fornode.Init.Visit(checker);

            fornode.Condition.Visit(checker);

            fornode.Body.Visit(checker);

            fornode.Increment.Visit(checker);

            checker.SymbolTable.LeaveScope();

            return null;
        }
    }
}
