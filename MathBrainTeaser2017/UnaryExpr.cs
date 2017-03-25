namespace MathBrainTeaser2017
{
    public abstract class UnaryExpr : Expr
    {
        protected readonly Expr Operand;

        protected UnaryExpr(Expr operand)
            : base(operand.Digits)
        {
            Operand = operand;
        }

        protected override bool IsValid()
        {
            return Operand.IsFinite();
        }
    }

    internal interface IFactorial {}

    public sealed class Factorial : UnaryExpr, IFactorial
    {
        public Factorial(Expr operand)
            : base(operand) {}

        protected override string Stringify()
        {
            //so we don't inadvertantly make it a double factorial
            //e.g. if Operand = 2! (a factorial) - then to make this a factorial,
            //we need to do (2!)! as opposed to just 2!! - which would be a different expression
            if (Operand is IFactorial)
            {
                //e.g. Operand = 2!; return (2!)!
                return $"({Operand})!";
            }
            //eg Operand = 2; return 2!
            return $"{Operand}!";
        }

        protected override bool IsValid()
        {
            Rational op = Operand.Value;
            return op.IsInteger() && op.Nominator >= 0 && op.Nominator <= 20 && base.IsValid();
        }

        protected override Rational Evaluate()
        {
            Rational op = Operand.Value;
            Rational value = Rational.One;
            for (long n = op.Nominator; n > 0 && value.IsFinite(); n--)
            {
                value *= new Rational(n, 1);
            }
            return value;
        }
    }

    public sealed class DoubleFactorial : UnaryExpr, IFactorial
    {
        public DoubleFactorial(Expr operand)
            : base(operand) {}

        protected override string Stringify()
        {
            if (Operand is IFactorial)
            {
                return $"({Operand})!!";
            }
            return $"{Operand}!!";
        }

        protected override bool IsValid()
        {
            Rational op = Operand.Value;
            return op.IsInteger() && op.Nominator >= 0 && op.Nominator <= 33 && base.IsValid();
        }

        protected override Rational Evaluate()
        {
            Rational op = Operand.Value;
            Rational value = Rational.One;
            for (long n = op.Nominator; n > 0; n -= 2)
            {
                value *= new Rational(n, 1);
            }
            return value;
        }
    }

    public sealed class Sqrt : UnaryExpr
    {
        public Sqrt(Expr operand)
            : base(operand) {}

        protected override string Stringify()
        {
            if (Operand is BinaryExpr)
            {
                return $"sqrt{Operand}";
            }
            return $"sqrt({Operand})";
        }

        protected override bool IsValid()
        {
            Rational op = Operand.Value;
            return op.Nominator >= 0 && base.IsValid();
        }

        protected override Rational Evaluate()
        {
            return Rational.Sqrt(Operand.Value);
        }
    }
}