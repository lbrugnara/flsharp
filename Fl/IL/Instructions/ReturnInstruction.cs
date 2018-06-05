﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.IL.Instructions.Operands;

namespace Fl.IL.Instructions
{
    public class ReturnInstruction : Instruction
    {
        public Operand Value { get; }

        public ReturnInstruction(Operand value = null)
            : base(OpCode.Return)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            if (this.Value == null)
                return this.OpCode.InstructionName();
            return $"{this.OpCode.InstructionName()} {this.Value}";
        }
    }
}