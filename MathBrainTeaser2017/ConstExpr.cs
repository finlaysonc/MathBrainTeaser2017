using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Countdown2017
{
    public sealed class ConstExpr : Expr
    {
        protected override bool IsValid()
        {
            if (Digits[0] == 0 && UsedDigits + pow > 1)
            {
                //Numbers like 01, 07, 01.2
                return false;
            }
            else if (Digits[Digits.Length - 1] == 0 && pow < 0)
            {
                //Numbers like .0, .10, 1.0
                return false;
            }
            return base.IsValid();
        }

        readonly int pow;
        protected override Rational Evaluate()
        {
            long nom = 0L;
            for (int i = 0; i < UsedDigits; i++)
            {
                nom = nom * 10L + (Digits[i] - '0');
            }
            long denom = 1;
            for (int i = pow; i < 0; i++)
            {
                denom *= 10L;
            }
            return new Rational(nom, denom);
        }
        public ConstExpr(string digits, int pow)
            : base(digits)
        {
            Debug.Assert(pow <= 0, "Positive powers are not allowed");

            this.pow = pow;
        }

        public override bool Equals(object obj)
        {
            var other = obj as ConstExpr;
            if (other != null)
            {
                return Value.Equals(other.Value);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }


        protected override string Stringify()
        {
            int i = 0, dc = UsedDigits, pointPos = dc + pow;
            var sb = new StringBuilder(dc + 1);

            for (i = 0; i < pointPos; i++)
                sb.Append(Digits[i]);

            if (i < dc)
            {
                sb.Append('.');
                while (i < dc)
                {
                    sb.Append(Digits[i++]);
                }
            }
            return sb.ToString();
        }
    }
}
