﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


namespace Fl.Lang.Types
{
    public class Package : ComplexType
    {
        public static Package Instance { get; } = new Package();

        private Package()
            : base("package")
        {
        }
    }
}
