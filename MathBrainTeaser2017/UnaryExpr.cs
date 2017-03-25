using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Countdown2017
{
    public abstract class UnaryExpr : Expr
    {
        protected readonly Expr Operand;
        public UnaryExpr(Expr operand)
            : base(operand.Digits)
        {
            Operand = operand;
        }

        protected override bool IsValid()
        {
            return Operand.IsFinite();
        }
    }

    interface IFactorial
    {
    }

    public sealed class Factorial : UnaryExpr, IFactorial
    {
        protected override string Stringify()
        {
            if (Operand is IFactorial)
                return string.Format("({0})!", Operand);
            else
                return string.Format("{0}!", Operand);
        }

        public Factorial(Expr operand)
            : base(operand)
        {
        }

        protected override bool IsValid()
        {
            var op = Operand.Value;
            return op.IsInteger() && op.Nominator >= 0 && op.Nominator <= 20 && base.IsValid();
        }

        protected override Rational Evaluate()
        {
            var op = Operand.Value;
            Rational value = Rational.One;
            for (var n = op.Nominator; n > 0 && value.IsFinite(); n--)
                value *= new Rational(n, 1);
            return value;
        }
    }

    public sealed class DoubleFactorial : UnaryExpr, IFactorial
    {
        protected override string Stringify()
        {
            if (Operand is IFactorial)
                return string.Format("({0})!!", Operand);
            else
                return string.Format("{0}!!", Operand);

        }

        public DoubleFactorial(Expr operand)
            : base(operand)
        {
        }

        protected override bool IsValid()
        {
            var op = Operand.Value;
            return op.IsInteger() && op.Nominator >= 0 && op.Nominator <= 33 && base.IsValid();
        }

        protected override Rational Evaluate()
        {
            var op = Operand.Value;
            Rational value = Rational.One;
            for (var n = op.Nominator; n > 0; n -= 2)
                value *= new Rational(n, 1);
            return value;
        }
    }

    public sealed class Sqrt : UnaryExpr
    {
        protected override string Stringify()
        {
            if(Operand is BinaryExpr)
                return string.Format("sqrt{0}", Operand);
            else
                return string.Format("sqrt({0})", Operand);
        }

        public Sqrt(Expr operand)
            : base(operand)
        {
        }

        protected override bool IsValid()
        {
            var op = Operand.Value;
            return op.Nominator >= 0 && base.IsValid();
        }

        protected override Rational Evaluate()
        {
            return Rational.Sqrt(Operand.Value);
        }
    }

    public class NegateExpr : UnaryExpr
    {
        public NegateExpr(Expr operand) : base(operand)
        {
        }

        protected override Rational Evaluate()
        {
            return -Operand.Value;
        }

        protected override string Stringify()
        {
            if (Operand is BinaryExpr || Operand is ConstExpr)
                return string.Format("-{0}", Operand);
            else
                return string.Format("-({0})", Operand);
        }
    }

    public class InvertExpr : UnaryExpr
    {
        public InvertExpr(Expr operand) : base(operand)
        {
        }

        protected override Rational Evaluate()
        {
            return Rational.Invert(Operand.Value);
        }

        protected override string Stringify()
        {
            if (Operand is BinaryExpr)
                return string.Format("inv{0}", Operand);
            else
                return string.Format("inv({0})", Operand);
        }
    }

    public class OneComplementExpr : UnaryExpr
    {
        public OneComplementExpr(Expr operand) : base(operand)
        {
        }

        protected override Rational Evaluate()
        {
            return Rational.MinusOne - Operand.Value;
        }

        protected override string Stringify()
        {
            if (Operand is BinaryExpr || Operand is ConstExpr)
                return string.Format("~{0}", Operand);
            else
                return string.Format("~({0})", Operand);
        }
    }
}
