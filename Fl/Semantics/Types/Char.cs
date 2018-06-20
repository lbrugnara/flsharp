﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Semantics.Types
{
    public class Char : Primitive
    {
        public static Char Instance { get; } = new Char();

        private Char()
            : base("char")
        {
        }
    }
}