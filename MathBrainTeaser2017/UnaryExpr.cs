using Microsoft.SolverFoundation.Common;

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
            return op.IsInteger() && op.Numerator >= 0 && op.Numerator <= 15 && base.IsValid();
      }

        protected override Rational Evaluate()
        {
            Rational op = Operand.Value;
            Rational value = Rational.One;
            for (BigInteger n = op.Numerator; n > 0 && value.IsFinite; n--)
            {
                value *= Rational.Get(n, 1);
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
            return op.IsInteger() && op.Numerator >= 0 && op.Numerator <= 15 && base.IsValid();
        }

        protected override Rational Evaluate()
        {
            Rational op = Operand.Value;
            Rational value = Rational.One;
            for (BigInteger n = op.Numerator; n > 0; n -= 2)
            {
                value *= Rational.Get(n, 1);
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
            return op.Numerator >= 0 && base.IsValid();
        }
        private static BigInteger LSqrt(BigInteger num)
        {
            if (num < 0)
            {
                return -1;
            }
            if (0 == num)
            {
                return 0;
            }

            BigInteger n = num / 2 + 1; // Initial estimate, never low  
            BigInteger n1 = (n + num / n) / 2;
            while (n1 < n)
            {
                n = n1;
                n1 = (n + num / n) / 2;
            }
            return n;
        }

        protected override Rational Evaluate()
        {

            BigInteger D  = Operand.Value.Denominator;
            if (D > 0)
            {
                BigInteger N = Operand.Value.Numerator;
                if (N >= 0)
                {
                    BigInteger nr = LSqrt(N);
                    if (nr * nr == N)
                    {
                        BigInteger dr = LSqrt(D);
                        if (dr * dr == D)
                        {
                            return Rational.Get(nr, dr);
                        }
                    }
                }
            }
            return Rational.Indeterminate;

        }
    }
}