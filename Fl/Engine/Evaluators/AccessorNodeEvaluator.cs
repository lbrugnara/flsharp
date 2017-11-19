﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Evaluators
{
    class AccessorNodeEvaluator : INodeEvaluator<AstEvaluator, AstAccessorNode, FlObject>
    {
        public FlObject Evaluate(AstEvaluator evaluator, AstAccessorNode invoke)
        {
            Symbol entry = null;
            var self = invoke.Self.Value.ToString();

            // If the identifier exists in the current scope, return it
            if (evaluator.Symtable.HasSymbol(self))
            {
                entry = evaluator.Symtable.GetSymbol(self);
            }
            else if (invoke.Member == null)
            {
                // If there is no member of "self" it means this (self) symbol doesn't exist
                throw new AstWalkerException($"Symbol '{self}' is not defined");
            }
            else
            {
                // If there is a member, resolve it ...                
                FlObject ns = invoke.Member.Exec(evaluator);

                // ... if it is a namespace and "self" is not defined, throw the exception
                if (ns.IsNamespace && ns.AsNamespace[self] == null)
                    throw new AstWalkerException($"Symbol '{self}' is not defined");
                entry = ns.AsNamespace[self];                
            }
            return entry.Binding;
        }
    }
}