﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions;
using Fl.Engine.IL.Instructions.Operands;
using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using System.Collections.Generic;
using System.Linq;

namespace Fl.Engine.IL.EValuators
{
    public class ILProgram
    {
        public List<FlObject> Params { get; } = new List<FlObject>();
        public SymbolTable SymbolTable { get; }
        public List<Instruction> Instructions { get; }
        

        public ILProgram(SymbolTable symtable, List<Instruction> instructions)
        {
            this.SymbolTable = SymbolTable.NewInstance;
            this.Instructions = instructions;
        }
        
        public override string ToString()
        {
            return string.Join('\n', Instructions.Select(i => i.ToString()));
        }

        public void Run()
        {
            foreach (var inst in Instructions)
            {
                switch (inst)
                {
                    case NopInstruction ni:
                        break;

                    case VarInstruction vi:
                        VisitVar(vi);
                        break;

                    case ConstInstruction ci:
                        VisitConst(ci);
                        break;

                    case StoreInstruction li:
                        VisitLoad(li);
                        break;

                    case CallInstruction ci:
                        VisitCall(ci);
                        break;

                    case ParamInstruction pi:
                        VisitParam(pi);
                        break;

                    case BinaryInstruction bi:
                        VisitBinary(bi);
                        break;

                    case UnaryInstruction ui:
                        VisitUnary(ui);
                        break;
                }
            }
        }

        protected FlObject GetFlObjectFromOperand(Operand o)
        {
            FlObject value = FlNull.Value;

            if (o == null || o.TypeResolver?.TypeName == FlNullType.Instance.Name)
                return value;

            if (o is SymbolOperand)
            {
                var so = o as SymbolOperand;

                // Get the root value
                value = SymbolTable.GetSymbol(so.Name).Binding;
            }
            else
            {
                var io = (o as ImmediateOperand);
                FlType operandType = io.TypeResolver.Resolve(SymbolTable);                
                value = operandType.Activator(io.Value);
            }

            if (o.Member != null)
            {
                FlObject curval = value;
                FlObject ancestor = value;
                SymbolOperand tmpmember = o.Member;
                while (tmpmember != null)
                {
                    if (curval.Type == FlNamespaceType.Instance)
                    {
                        curval = (curval as FlNamespace)[tmpmember.Name].Binding;
                    }
                    else if (curval is FlType)
                    {
                        curval = (curval as FlType)[tmpmember.Name].Binding;
                    }
                    if (curval is FlMethod)
                    {
                        curval = (curval as FlMethod).Bind(ancestor);
                    }
                    ancestor = curval;
                    tmpmember = tmpmember.Member;
                }
                value = curval;
            }

            return value;
        }

        protected void VisitBinary(BinaryInstruction bi)
        {
            FlObject left = this.GetFlObjectFromOperand(bi.Left);
            FlObject right = this.GetFlObjectFromOperand(bi.Right);

            FlObject result = null; 
            switch (bi.OpCode)
            {
                case OpCode.Add:
                    result = left.Type.GetStaticMethod(FlType.OperatorAdd).Invoke(SymbolTable, left, right);
                    break;
                case OpCode.Sub:
                    result = left.Type.GetStaticMethod(FlType.OperatorSub).Invoke(SymbolTable, left, right);
                    break;
                case OpCode.Mult:
                    result = left.Type.GetStaticMethod(FlType.OperatorMult).Invoke(SymbolTable, left, right);
                    break;
                case OpCode.Div:
                    result = left.Type.GetStaticMethod(FlType.OperatorDiv).Invoke(SymbolTable, left, right);
                    break;

                case OpCode.And:
                case OpCode.Or:
                    {
                        bool l = left.Type != FlBoolType.Instance ? left.RawValue != null : (left as FlBool).Value;
                        bool r = right.Type != FlBoolType.Instance ? right.RawValue != null : (right as FlBool).Value;

                        switch (bi.OpCode)
                        {
                            case OpCode.And:
                                result = new FlBool(l && r);
                                break;
                            case OpCode.Or:
                                result = new FlBool(l || r);
                                break;
                        }
                        break;
                    }

                case OpCode.Ceq:
                    result = left.Type.GetStaticMethod(FlType.OperatorEquals).Invoke(SymbolTable, left, right);
                    break;
                case OpCode.Cgt:
                    result = left.Type.GetStaticMethod(FlType.OperatorGt).Invoke(SymbolTable, left, right);
                    break;
                case OpCode.Cgte:
                    result = left.Type.GetStaticMethod(FlType.OperatorGte).Invoke(SymbolTable, left, right);
                    break;
                case OpCode.Clt:
                    result = left.Type.GetStaticMethod(FlType.OperatorLt).Invoke(SymbolTable, left, right);
                    break;
                case OpCode.Clte:
                    result = left.Type.GetStaticMethod(FlType.OperatorLte).Invoke(SymbolTable, left, right);
                    break;
                default:
                    throw new System.Exception($"Unhandled binary operator");
            }
            SymbolTable.GetSymbol(bi.DestSymbol.Name).UpdateBinding(result);
        }

        public void VisitUnary(UnaryInstruction ui)
        {
            FlObject left = this.GetFlObjectFromOperand(ui.Left);

            FlObject result = null; 
            switch (ui.OpCode)
            {
                case OpCode.Not:
                    result = left.Type.GetStaticMethod(FlType.OperatorNot).Invoke(SymbolTable, left);
                    break;
                case OpCode.Neg:
                    result = left.Type.GetStaticMethod(FlType.OperatorSub).Invoke(SymbolTable, left);
                    break;
                case OpCode.PreInc:
                    result = left.Type.GetStaticMethod(FlType.OperatorPreIncr).Invoke(SymbolTable, left);
                    break;
                case OpCode.PreDec:
                    result = left.Type.GetStaticMethod(FlType.OperatorPreDecr).Invoke(SymbolTable, left);
                    break;
                case OpCode.PostInc:
                    result = left.Type.GetStaticMethod(FlType.OperatorPostIncr).Invoke(SymbolTable, left);
                    break;
                case OpCode.PostDec:
                    result = left.Type.GetStaticMethod(FlType.OperatorPostDecr).Invoke(SymbolTable, left);
                    break;
                default:
                    throw new System.Exception($"Unhandled unary operator");
            }
            SymbolTable.GetSymbol(ui.DestSymbol.Name).UpdateBinding(result);
        }

        public void VisitLoad(StoreInstruction li)
        {
            FlObject value = this.GetFlObjectFromOperand(li.Value);
            SymbolTable.GetSymbol(li.DestSymbol.Name).UpdateBinding(value);
        }

        public void VisitVar(VarInstruction vi)
        {
            FlObject value = this.GetFlObjectFromOperand(vi.Value);
            if (vi.TypeResolver != null)
            {
                FlType type = vi.TypeResolver.Resolve(SymbolTable);
                Symbol symbol = new Symbol(SymbolType.Variable);
                SymbolTable.AddSymbol(vi.DestSymbol.Name, symbol, FlNull.Value);
                var defaultValue = type.Activator(null);
                if (value == null)
                {
                    if (defaultValue != null && defaultValue != FlNull.Value)
                        symbol.UpdateBinding(defaultValue);
                }
                else
                {
                    symbol.UpdateBinding(value);
                }                
            }
            else
            {
                SymbolTable.AddSymbol(vi.DestSymbol.Name, new Symbol(SymbolType.Variable), value);
            }
        }

        public void VisitConst(ConstInstruction ci)
        {
            FlObject value = this.GetFlObjectFromOperand(ci.Value);
            SymbolTable.AddSymbol(ci.DestSymbol.Name, new Symbol(SymbolType.Constant), value);
        }

        public void VisitCall(CallInstruction ci)
        {
            FlObject target = GetFlObjectFromOperand(ci.Func);

            List<FlObject> parameters = new List<FlObject>(Params);
            parameters.Reverse();

            /*if (node is AstIndexerNode)
            {
                Symbol clasz = evaluator.Symtable.GetSymbol(target.ObjectType.Name) ?? evaluator.Symtable.GetSymbol(target.ObjectType.Name);
                var claszobj = (clasz.Binding as FlClass);
                FlIndexer indexer = claszobj.GetIndexer(node.Arguments.Count);
                if (indexer == null)
                    throw new AstWalkerException($"{claszobj} does not contain an indexer that accepts {node.Arguments.Count} {(node.Arguments.Count == 1 ? "argument" : "arguments")}");
                return indexer.Bind(target).Invoke(evaluator.Symtable, node.Arguments.Expressions.Select(e => e.Exec(evaluator)).ToList());
            }
            */

            if (target is FlFunction)
            {
                (target as FlFunction).Invoke(SymbolTable, Params);
            }/*
            else if (target is FlClass)
            {
                var clasz = (target as FlClass);

                if (node.New != null)
                {
                    return clasz.InvokeConstructor(node.Arguments.Expressions.Select(a => a.Exec(evaluator)).ToList());
                }
                else
                {
                    target = clasz.StaticConstructor ?? throw new AstWalkerException($"{target} does not contain a definition for the static constructor");
                }
            }*/
            //throw new AstWalkerException($"{target} is not a callable object");

            Params.Clear();
        }

        public void VisitParam(ParamInstruction pi)
        {
            Params.Add(GetFlObjectFromOperand(pi.Parameter));
        }
    }
}
