﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.StdLib;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using Fl.Parser.Ast;
using System.Linq;

namespace Fl.Engine.IL.Generators
{
    class ReturnILGenerator : INodeVisitor<ILGenerator, AstReturnNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AstReturnNode rnode)
        {
            if (!generator.InFunction)
                throw new ScopeOperationException("Invalid return statement in a non-function block");

            Operand expr = rnode.ReturnTuple.Exec(generator);
            SymbolOperand ret = null;
            if (expr is ImmediateOperand)
            {
                ret = generator.SymbolTable.NewTempSymbol();
                generator.Emmit(new VarInstruction(ret, expr.TypeResolver, expr));
            }
            else
            {
                ret = expr as SymbolOperand;
            }
            generator.Emmit(new ReturnInstruction(ret));
            return null;
        }
    }
}
