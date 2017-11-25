﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Types;

namespace Fl.Engine.Symbols.Objects
{
    public class FlNull : FlObject
    {
        private static FlNull _Instance;

        private FlNull()
        {
        }

        public static FlNull Value => _Instance == null ? (_Instance = new FlNull()) : _Instance;

        public override object RawValue => null;

        public override bool IsPrimitive => true;

        public override ObjectType ObjectType => NullType.Value;

        public override string ToDebugStr()
        {
            return "(null)";
        }

        public override FlObject Clone()
        {
            return _Instance;
        }

        public override FlObject ConvertTo(ObjectType type)
        {
            return _Instance;
            //throw new CastException($"Cannot convert type {ObjectType} to {type}");
        }
    }
}