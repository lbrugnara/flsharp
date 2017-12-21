﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System;
using System.Collections.Generic;

namespace Fl.Engine.Symbols.Objects
{
    public class FlConstructor: FlInstanceMethod
    {
        private int _ParamsCount;
        private Action<FlObject, List<FlObject>> _Body;
        private string _Name;

        public FlConstructor(Action<FlObject, List<FlObject>> body)
            : base("constructor", (self, args) => { body(self, args); return self; })
        {
            _Body = body;
            _ParamsCount = -1;
            _Name = $"{base.Name}@params";
        }

        public FlConstructor(int paramsCount, Action<FlObject, List<FlObject>> body)
            : base("constructor", (self, args) => { body(self, args); return self; })
        {
            _Body = body;
            _ParamsCount = paramsCount;
            _Name = $"{base.Name}@{_ParamsCount}";
        }

        public override string Name => _Name;
    }
}
