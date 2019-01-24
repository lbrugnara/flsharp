﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Semantics.Symbols.Containers;
using Fl.Semantics.Types;

namespace Fl.Semantics.Symbols.Types.Specials
{
    public class UnresolvedTypeSymbol : IUnresolvedTypeSymbol
    {
        public string Name { get; }

        public IContainer Parent { get; }

        public BuiltinType BuiltinType { get; set; }

        public UnresolvedTypeSymbol(string name, IContainer parent)
        {
            this.Name = name;
            this.Parent = parent;
            this.BuiltinType = BuiltinType.None;
        }

        public override string ToString()
        {
            return this.ToValueString();
        }

        public string ToValueString()
        {
            return $"unresolved {this.Name}";
        }
    }
}
