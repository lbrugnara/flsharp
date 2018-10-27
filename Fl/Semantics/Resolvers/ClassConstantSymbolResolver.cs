﻿using Fl.Ast;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;
using System;

namespace Fl.Semantics.Resolvers
{
    class ClassConstantSymbolResolver : INodeVisitor<SymbolResolverVisitor, ClassConstantNode>
    {
        public void Visit(SymbolResolverVisitor binder, ClassConstantNode node)
        {
            // Get the constant name
            var constantName = node.Name.Value;

            // Check if the symbol is already defined
            if (binder.SymbolTable.HasSymbol(constantName))
                throw new SymbolException($"Symbol {constantName} is already defined.");

            // Get the constant type from the declaration or assume an anonymous type
            var lhsType = SymbolHelper.GetType(binder.SymbolTable, binder.Inferrer, node.SymbolInfo.Type);

            // Create the new symbol for the property
            var symbol = binder.SymbolTable.NewSymbol(constantName, lhsType, SymbolHelper.GetAccess(node.SymbolInfo.Access), Storage.Constant);

            // If it is a type assumption, register the symbol under that assumption
            if (binder.Inferrer.IsTypeAssumption(lhsType))
                binder.Inferrer.AssumeSymbolTypeAs(symbol, lhsType);

            // Visit the right-hand side expression
            node.Definition.Visit(binder);
        }
    }
}
