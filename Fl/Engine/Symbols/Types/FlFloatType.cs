﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols;
using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Objects;
using Fl.Engine.Symbols.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fl.Engine.Symbols.Types
{
    public class FlFloatType : FlType
    {
        private static FlFloatType _Instance;

        private FlFloatType(TypeDescriptor descr)
            : base (descr)
        {
        }

        public static FlType Instance => _Instance == null ? (_Instance = Initialize()) : _Instance;

        private static FlObject OperatorAddImpl(List<FlObject> args)
        {
            FlFloat self = args[0] as FlFloat;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
                return new FlFloat(self.Value + (float)arg.RawValue);

            throw new UnsupportedOperandException($"Operator '+' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorSubImpl(List<FlObject> args)
        {
            FlFloat self = args[0] as FlFloat;

            if (args.Count == 1)
                return new FlFloat(self.Value * -1.0F);

            FlObject arg = args[1];

            if (arg.Type == _Instance)
                return new FlFloat(self.Value - (float)arg.RawValue);

            throw new UnsupportedOperandException($"Operator '-' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorMultImpl(List<FlObject> args)
        {
            FlFloat self = args[0] as FlFloat;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
                return new FlFloat(self.Value * (float)arg.RawValue);

            throw new UnsupportedOperandException($"Operator '*' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorDivImpl(List<FlObject> args)
        {
            FlFloat self = args[0] as FlFloat;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
                return new FlFloat(self.Value / (float)arg.RawValue);

            throw new UnsupportedOperandException($"Operator '/' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorCallImpl(List<FlObject> args)
        {
            FlFloat self = args[0] as FlFloat;
            FlType type = args[1] as FlType;

            if (type == _Instance)
                return self.Clone();

            if (type == FlCharType.Instance)
                return new FlChar((char)self.RawValue);

            if (type == FlIntType.Instance)
                return new FlInt((int)self.RawValue);

            if (type == FlDoubleType.Instance)
                return new FlDouble((double)self.RawValue);

            if (type == FlDecimalType.Instance)
                return new FlDecimal((decimal)self.RawValue);

            if (type == FlStringType.Instance)
                return new FlString(self.RawValue.ToString());

            throw new CastException($"Cannot convert type {self.Type} to {type}");
        }

        private static FlObject OperatorPreIncrImpl(List<FlObject> args)
        {
            FlFloat self = args[0] as FlFloat;
            self.Value += 1.0F;
            return self;
        }

        private static FlObject OperatorPostIncrImpl(List<FlObject> args)
        {
            FlFloat self = args[0] as FlFloat;
            var res = self.Clone();
            self.Value += 1.0F;
            return res;
        }

        private static FlObject OperatorPreDecrImpl(List<FlObject> args)
        {
            FlFloat self = args[0] as FlFloat;
            self.Value -= 1.0F;
            return self;
        }

        private static FlObject OperatorPostDecrImpl(List<FlObject> args)
        {
            FlFloat self = args[0] as FlFloat;
            var res = self.Clone();
            self.Value -= 1.0F;
            return res;
        }

        private static FlObject OperatorAssignImpl(List<FlObject> args)
        {
            FlFloat self = args[0] as FlFloat;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
            {
                self.Value = (arg as FlFloat).Value;
                return new FlFloat(self.Value);
            }
            throw new UnsupportedOperandException($"Operator '=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorAddAndAssignImpl(List<FlObject> args)
        {
            FlFloat self = args[0] as FlFloat;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
            {
                self.Value += (arg as FlFloat).Value;
                return new FlFloat(self.Value);
            }
            throw new UnsupportedOperandException($"Operator '+=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorSubAndAssignImpl(List<FlObject> args)
        {
            FlFloat self = args[0] as FlFloat;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
            {
                self.Value -= (arg as FlFloat).Value;
                return new FlFloat(self.Value);
            }
            throw new UnsupportedOperandException($"Operator '-=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorMultAndAssignImpl(List<FlObject> args)
        {
            FlFloat self = args[0] as FlFloat;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
            {
                self.Value *= (arg as FlFloat).Value;
                return new FlFloat(self.Value);
            }
            throw new UnsupportedOperandException($"Operator '*=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlObject OperatorDivAndAssingImpl(List<FlObject> args)
        {
            FlFloat self = args[0] as FlFloat;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
            {
                self.Value /= (arg as FlFloat).Value;
                return new FlFloat(self.Value);
            }
            throw new UnsupportedOperandException($"Operator '/=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlBool OperatorEqualsImpl(List<FlObject> args)
        {
            FlFloat self = args[0] as FlFloat;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
                return new FlBool(self.Value == (float)arg.RawValue);

            return new FlBool(self.RawValue.Equals(arg.RawValue));
        }

        private static FlObject OperatorNotImpl(List<FlObject> args)
        {
            FlFloat self = args[0] as FlFloat;
            return new FlFloat(self.Value * -1);
        }

        private static FlBool OperatorGtImpl(List<FlObject> args)
        {
            FlFloat self = args[0] as FlFloat;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
                return new FlBool(self.Value > (float)arg.RawValue);

            throw new UnsupportedOperandException($"Operator '>' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlBool OperatorGteImpl(List<FlObject> args)
        {
            FlFloat self = args[0] as FlFloat;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
                return new FlBool(self.Value >= (float)arg.RawValue);

            throw new UnsupportedOperandException($"Operator '>=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlBool OperatorLtImpl(List<FlObject> args)
        {
            FlFloat self = args[0] as FlFloat;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
                return new FlBool(self.Value < (float)arg.RawValue);

            throw new UnsupportedOperandException($"Operator '<' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlBool OperatorLteImpl(List<FlObject> args)
        {
            FlFloat self = args[0] as FlFloat;
            FlObject arg = args[1];

            if (arg.Type == _Instance)
                return new FlBool(self.Value <= (float)arg.RawValue);

            throw new UnsupportedOperandException($"Operator '<=' cannot be applied to operands of type '{arg.Type}' and '{self.Type}'");
        }

        private static FlFloatType Initialize()
        {
            TypeDescriptor.Builder builder = new TypeDescriptor.Builder();

            builder
                
                // Class Name
                .WithName("float")

                // Activator
                .WithActivator((obj) => new FlFloat(obj != null ? float.Parse(obj.ToString()) : 0.0f))

                // Static constructor
                // ...

                // Constructors
                // ...

                // Static Methods
                .WithStaticMethod(FlType.OperatorCall, OperatorCallImpl)

                .WithStaticMethod(FlType.OperatorPreIncr, OperatorPreIncrImpl)
                .WithStaticMethod(FlType.OperatorPostIncr, OperatorPostIncrImpl)
                .WithStaticMethod(FlType.OperatorPreDecr, OperatorPreDecrImpl)
                .WithStaticMethod(FlType.OperatorPostDecr, OperatorPostDecrImpl)

                .WithStaticMethod(FlType.OperatorAdd, OperatorAddImpl)
                .WithStaticMethod(FlType.OperatorSub, OperatorSubImpl)
                .WithStaticMethod(FlType.OperatorMult, OperatorMultImpl)
                .WithStaticMethod(FlType.OperatorDiv, OperatorDivImpl)

                .WithStaticMethod(FlType.OperatorAssign, OperatorAssignImpl)
                .WithStaticMethod(FlType.OperatorAddAndAssign, OperatorAddAndAssignImpl)
                .WithStaticMethod(FlType.OperatorSubAndAssign, OperatorSubAndAssignImpl)
                .WithStaticMethod(FlType.OperatorMultAndAssign, OperatorMultAndAssignImpl)
                .WithStaticMethod(FlType.OperatorDivAndAssing, OperatorDivAndAssingImpl)

                .WithStaticMethod(FlType.OperatorEquals, OperatorEqualsImpl)
                .WithStaticMethod(FlType.OperatorNot, OperatorNotImpl)
                .WithStaticMethod(FlType.OperatorGt, OperatorGtImpl)
                .WithStaticMethod(FlType.OperatorGte, OperatorGteImpl)
                .WithStaticMethod(FlType.OperatorLt, OperatorLtImpl)
                .WithStaticMethod(FlType.OperatorLte, OperatorLteImpl)

                // Static Properties
                .WithStaticProperty("MAX", SymbolType.Constant, new FlFloat(float.MaxValue))
                .WithStaticProperty("MIN", SymbolType.Constant, new FlFloat(float.MinValue))
                
                // Instance Methods
                .WithMethod("str", (self, args) => new FlString(self.RawValue.ToString()));

            // Build
            return new FlFloatType(builder.Build());
        }
    }
}