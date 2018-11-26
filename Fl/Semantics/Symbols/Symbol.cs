﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Types;

namespace Fl.Semantics.Symbols
{
    public class Symbol
    {
        /// <summary>
        /// Symbol name (user-defined name)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type information
        /// </summary>
        public virtual Object Type { get; set; }

        /// <summary>
        /// Symbol's access level
        /// </summary>
        public Access Access { get; set; }

        /// <summary>
        /// Symbol's storage type
        /// </summary>
        public Storage Storage { get; set; }

        public Symbol(string name, Object type, Access access, Storage storage)
        {
            this.Name = name;
            this.Type = type;
            this.Access = access;
            this.Storage = storage;
        }

        protected Symbol(string name)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return $"{this.Access.ToKeyword()} {this.Storage.ToKeyword()} {this.Name}: {this.Type}";
        }
    }
}
