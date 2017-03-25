using System.Diagnostics;
using System.Text;
using Microsoft.SolverFoundation.Common;

namespace MathBrainTeaser2017
{
    public sealed class ConstExpr : Expr
    {
        protected override bool IsValid()
        {
            if (Digits[0] == 0  && UsedDigits + pow > 1)
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

        private readonly int pow;

        public ConstExpr(string digits, int pow)
            : base(digits)
        {
            Debug.Assert(pow <= 0, "Positive powers are not allowed");

            this.pow = pow;
        }

        protected override Rational Evaluate
        {
            get
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
                return Rational.Get(nom, denom);
            }
        }

        public override bool Equals(object obj)
        {
            ConstExpr other = obj as ConstExpr;
            if (other != null)
            {
                return Value.Equals(other.Value);
            }
            return false;
        }


        /// <summary>
        /// will not allow 21 and 21.0 together 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }


        protected override string Stringify()
        {
            int i = 0, dc = UsedDigits, pointPos = dc + pow;
            StringBuilder sb = new StringBuilder(dc + 1);

            for (i = 0; i < pointPos; i++)
            {
                sb.Append(Digits[i]);
            }

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