﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;
using System.Collections.Generic;
using System.Linq;

namespace Fl.Semantics.Inferrers
{
    class ClassMethodTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstClassMethodNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AstClassMethodNode method)
        {
            // Get the method symbol and type
            var methodSymbol = visitor.SymbolTable.GetSymbol(method.Name);
            Function methodType = methodSymbol.Type as Function;

            // Enter the requested method's block
            visitor.SymbolTable.EnterScope(ScopeType.Function, method.Name);

            // Grab all the parameters' symbols
            var parametersSymbols = new List<Symbol>();

            parametersSymbols.AddRange(method.Parameters.Parameters.Select(param => visitor.SymbolTable.GetSymbol(param.Value.ToString())));

            // Get the return symbol and assign a temporal type
            var retSymbol = visitor.SymbolTable.GetSymbol("@ret");

            // Visit the method's body
            var statements = method.Body.Select(s => (node: s, inferred: s.Visit(visitor))).ToList();

            if (method.IsLambda)
            {
                // If method is a lambda, the return type should be already populated by the lambda's body expression
                // and that should be reflected on the @ret symbol
                var lambdaReturnExpr = statements.Select(s => s.inferred).Last();

                // Try to unify these types
                visitor.Inferrer.MakeConclusion(lambdaReturnExpr.Type, retSymbol.Type);
            }
            else
            {
                // If it has a body, get all the AstReturnNode, and check that all the returned types are same
                // TODO: This needs to get the common ancestor type (union)
                var returnTypesNode = statements.Where(t => t.node is AstReturnNode).ToList();

                var returnTypes = returnTypesNode.Select(t => t.inferred).Distinct().ToList();

                if (returnTypes.Count() == 1)
                    visitor.Inferrer.MakeConclusion(returnTypes.First().Type, retSymbol.Type);
                else if (returnTypes.Count() == 0)
                    visitor.Inferrer.MakeConclusion(Void.Instance, retSymbol.Type);
                else
                    throw new System.Exception($"Unexpected multiple return types ({string.Join(", ", returnTypes)}) in method {method.Name}");
            }

            // Leave the method's scope
            visitor.SymbolTable.LeaveScope();

            // The inferred method type is a complex type, it might contain assumptions for parameters' types or return type
            // if that is the case, make this inferred type an assumption
            if (visitor.Inferrer.IsTypeAssumption(methodType))
                visitor.Inferrer.AssumeSymbolTypeAs(methodSymbol, methodType);

            // Return inferred method type
            return new InferredType(methodType, methodSymbol);
        }
    }
}