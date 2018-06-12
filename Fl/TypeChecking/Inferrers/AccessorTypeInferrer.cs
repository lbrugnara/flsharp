﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols;

namespace Fl.TypeChecking.Inferrers
{
    class AccessorTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstAccessorNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AstAccessorNode accessor)
        {
            ISymbolTable table = visitor.SymbolTable;

            if (accessor.Enclosing != null)
                table = accessor.Enclosing?.Visit(visitor).Symbol as ISymbolTable;

            // Get accessed symbol 
            var symbol = table.GetSymbol(accessor.Identifier.Value.ToString());

            if (symbol.Type == null)
                throw new System.Exception($"Symbol {symbol.Name} has a null type, it should have defined one at this point.");

            // Return the inferred type information for this symbol
            return new InferredType(symbol.Type, symbol);
        }
    }
}
