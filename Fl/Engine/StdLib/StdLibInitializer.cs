﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.StdLib
{
    public class StdLibInitializer
    {
        public static void Import(Scope scope)
        {
            // Namespace: global
            scope.AddSymbol("import", new Symbol(ObjectType.Function, StorageType.Constant), new ImportFunction());
            scope.AddSymbol("using", new Symbol(ObjectType.Function, StorageType.Constant), new UsingFunction());

            // Namespace: std
            FlNamespace std = new FlNamespace("std");
            scope.AddSymbol(std.Name, new Symbol(ObjectType.Namespace, StorageType.Constant), std);

            // Namespace: std.lang
            FlNamespace lang = new FlNamespace("lang", std);
            lang.AddSymbol("debug", new Symbol(ObjectType.Function, StorageType.Constant), new std.lang.DebugFunction());
            lang.AddSymbol("version", new Symbol(ObjectType.Function, StorageType.Constant), new FlObject(ObjectType.String, "0.0.1a"));

            // Namespace: std.io
            FlNamespace io = new FlNamespace("io", std);
            io.AddSymbol("print", new Symbol(ObjectType.Function, StorageType.Constant), new std.io.PrintFunction());
            io.AddSymbol("println", new Symbol(ObjectType.Function, StorageType.Constant), new std.io.PrintLnFunction());

            // Namespace: os
            FlNamespace os = new FlNamespace("os");
            scope.AddSymbol(os.Name, new Symbol(ObjectType.Namespace, StorageType.Constant), os);
            os.AddSymbol("cwd", new Symbol(ObjectType.Function, StorageType.Constant), new os.CwdFunction());
        }
    }
}