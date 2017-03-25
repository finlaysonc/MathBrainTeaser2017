using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Countdown2017
{
    public abstract class Expr
    {
        public Rational Value
        {
            get
            {
                if (!eval.HasValue)
                {
                    if (IsValid())
                        eval = Evaluate();
                    else
                        eval = Rational.NaN;
                }

                return eval.Value;
            }
        }

        protected virtual bool IsValid()
        {
            return Problem.Validate(Digits);
        }

        Rational? eval;
        protected abstract Rational Evaluate();

        public bool IsFinite()
        {
            return Value.IsFinite();
        }

        protected internal readonly string Digits;
        protected abstract string Stringify();
        public readonly int UsedDigits;
        string text;
        public Expr(string digits)
        {
            Digits = digits;
            UsedDigits = digits.Length;
        }

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
            Expr other = obj as Expr;
            return other != null && Digits.Equals(other.Digits) && Value.Equals(other.Value);
        }
    }
    
}
