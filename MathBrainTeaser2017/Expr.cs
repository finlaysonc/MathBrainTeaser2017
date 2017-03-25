using System;
using Microsoft.SolverFoundation.Common;

namespace MathBrainTeaser2017
{
    public abstract class Expr
    {
        protected internal readonly string Digits;
        public readonly int UsedDigits;


        protected Expr(string digits)
        {
            Digits = digits;
            UsedDigits = digits.Length;
            _isFinite = new Lazy<bool>(() => Value.IsFinite, true);
            _eval = new Lazy<Rational>(() => IsValid() ? Evaluate: Rational.Indeterminate, true);
            _hashCode = new Lazy<int>(() => Digits.GetHashCode() ^ Value.GetHashCode());
            _text = new Lazy<string>(Stringify);
        }

        private readonly Lazy<Rational> _eval;
        public Rational Value => _eval.Value;

        protected virtual bool IsValid() => Problem.Validate(Digits);

        protected abstract Rational Evaluate { get; }

        private readonly Lazy<bool> _isFinite;
        public bool IsFinite => _isFinite.Value;

        protected abstract string Stringify();


        private readonly Lazy<string> _text;
        public override string ToString() => _text.Value;

        private readonly Lazy<int> _hashCode;
        public override int GetHashCode()
        {
            return _hashCode.Value;
        }

        public override bool Equals(object obj)
        {
            //e.g. 0! == 0!!
            Expr other = obj as Expr;
            return other != null && Digits.Equals(other.Digits) && Value.Equals(other.Value);
        }
    }
}