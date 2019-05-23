﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;

namespace Zenit.Syntax
{
    class LexerException : Exception
    {
        public LexerException(string message) : base(message)
        {
        }
    }
}