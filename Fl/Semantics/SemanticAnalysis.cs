﻿using Fl.Ast;
using Fl.Semantics.Binders;
using Fl.Semantics.Checkers;
using Fl.Semantics.Inferrers;
using Fl.Semantics.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Semantics
{
    public class SemanticAnalysis
    {
        private TypeInferrer typeInferrer;
        private SymbolTable symbolTable;
        private SymbolBinderVisitor binder;
        private TypeInferrerVisitor inferrer;
        private TypeCheckerVisitor checker;

        public SemanticAnalysis()
        {
            this.typeInferrer = new TypeInferrer();
            this.symbolTable = new SymbolTable();

            this.binder = new SymbolBinderVisitor(this.symbolTable, this.typeInferrer);
            this.inferrer = new TypeInferrerVisitor(this.symbolTable, this.typeInferrer);
            this.checker = new TypeCheckerVisitor(this.symbolTable);

            /*var intClass = new Class();

            this.SymbolTable.NewSymbol("int", intClass);
            this.SymbolTable.EnterClassScope("int");
            this.SymbolTable.NewSymbol("toStr", new Function(String.Instance));
            this.SymbolTable.LeaveScope();*/
        }        

        public SymbolTable Run(AstNode ast)
        {
            this.binder.Visit(ast);
            this.inferrer.Visit(ast);
            this.checker.Visit(ast);

            return this.symbolTable;
        }
    }
}