﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Fl.Syntax
{
    public class Token
    {
        public TokenType Type;
        public object Value;
        public int Line;
        public int Col;
    }
}