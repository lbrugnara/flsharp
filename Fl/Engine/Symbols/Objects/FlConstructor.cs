﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Evaluators;
using System;
using System.Collections.Generic;
using System.Linq;
using Fl.Engine.Symbols.Types;

namespace Fl.Engine.Symbols.Objects
{
    public class FlConstructor: FlMethod
    {
        private int _ParamsCount;
        private Action<FlObject, List<FlObject>> _Body;

        public FlConstructor(int paramsCount, Action<FlObject, List<FlObject>> body)
            : base("constructor", (self, args) => { body(self, args); return self; })
        {
            _Body = body;
            _ParamsCount = paramsCount;
        }

        public override string Name => $"{base.Name}@{_ParamsCount}";
    }
}