﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Helpers;
using Fl.Semantics.Types;

namespace Fl.Semantics.Symbols
{
    public class AnonymousSymbol : ITypeSymbol
    {
        public string Name { get; }

        public ISymbolContainer Parent => null;

        public BuiltinType BuiltinType => BuiltinType.Anonymous;

        public AnonymousSymbol()
        {
            this.Name = $"'{NameGenerator.Instance.Next()}";
        }

        public override bool Equals(object obj)
        {
            var objType = obj as AnonymousSymbol;

            if (objType == null)
                return false;

            return this.Name == objType.Name;
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public override string ToString()
        {
            return this.Name;
        }

        public string ToValueString()
        {
            return this.Name;
        }        
    }
}
