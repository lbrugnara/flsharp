﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Symbols.Resolvers
{
    class AccessorSymbolResolver : INodeVisitor<SymbolResolverVisitor, AstAccessorNode>
    {
        public void Visit(SymbolResolverVisitor checker, AstAccessorNode accessor)
        {
            accessor.Enclosing?.Visit(checker);
        }
    }
}
