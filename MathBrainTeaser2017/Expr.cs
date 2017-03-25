namespace MathBrainTeaser2017
{
    public abstract class Expr
    {
        protected internal readonly string Digits;
        public readonly int UsedDigits;

        private Rational? eval;
        private string text;

        protected Expr(string digits)
        {
            Digits = digits;
            UsedDigits = digits.Length;
        }

        public Rational Value
        {
            get
            {
                if (!eval.HasValue)
                {
                    if (IsValid())
                    {
                        eval = Evaluate();
                    }
                    else
                    {
                        eval = Rational.NaN;
                    }
                }

                return eval.Value;
            }
        }

        protected virtual bool IsValid()
        {
            return Problem.Validate(Digits);
        }

        protected abstract Rational Evaluate();

        public bool IsFinite()
        {
            return Value.IsFinite();
        }

        protected abstract string Stringify();

        public override string ToString()
        {
            return text ?? (text = Stringify());
        }

        public override int GetHashCode()
        {
            return Digits.GetHashCode() ^ Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            //e.g. 0! == 0!!
            Expr other = obj as Expr;
            return other != null && Digits.Equals(other.Digits) && Value.Equals(other.Value);
        }
    }
}