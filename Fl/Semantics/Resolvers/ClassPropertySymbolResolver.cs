﻿using Fl.Ast;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;
using System;
using System.Collections.Generic;

namespace Fl.Semantics.Resolvers
{
    class ClassPropertySymbolResolver : INodeVisitor<SymbolResolverVisitor, ClassPropertyNode>
    {
        public void Visit(SymbolResolverVisitor binder, ClassPropertyNode node)
        {
            // Get the property name
            var propertyName = node.Name.Value;

            // Check if the symbol is already defined
            if (binder.SymbolTable.HasSymbol(propertyName))
                throw new SymbolException($"Symbol {propertyName} is already defined.");

            // Create the property type
            var propertyType = SymbolHelper.GetType(binder.SymbolTable, binder.Inferrer, node.SymbolInfo.Type);

            // Create the new symbol for the property
            var access = SymbolHelper.GetAccess(node.SymbolInfo.Access);
            var storage = SymbolHelper.GetStorage(node.SymbolInfo.Mutability);
            var symbol = binder.SymbolTable.NewSymbol(propertyName, propertyType, access, storage);

            // If it is a type assumption, register the symbol under that assumption
            if (binder.Inferrer.IsTypeAssumption(propertyType))
                binder.Inferrer.AssumeSymbolTypeAs(symbol, propertyType);

            // If the property has a definition, visit the right-hand side expression
            node.Definition?.Visit(binder);
        }
    }
}
