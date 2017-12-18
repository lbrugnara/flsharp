﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Parser;
using Fl.Parser.Ast;
using System;
using System.Collections.Generic;

namespace Fl.Engine.IL.Generators
{
    class FuncDeclILGenerator : INodeVisitor<AstILGenerator, AstFuncDeclNode, Operand>
    {
        public Operand Visit(AstILGenerator generator, AstFuncDeclNode funcdecl)
        {
            return null;
        }
    }
}