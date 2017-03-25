using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Countdown2017
{
    public struct Rational : IEquatable<Rational>
    {
        public static readonly Rational NaN = default(Rational);
        public static readonly Rational Zero = new Rational(0, 1);
        public static readonly Rational One = new Rational(1, 1);

        readonly long _nom;
        readonly long _denom;

        public long Nominator { get { return _nom; } }
        public long Denominator { get { return _denom; } }

        public Rational FractionalPart
        {
            get
            {
                if (_denom > 0)
                {
                    var wh = _nom / _denom;
                    var fr = _nom - _denom * wh;
                    return new Rational(fr < 0 ? -fr : fr, _denom);
                }
                else
                    return NaN;
            }
        }


        public Rational(long nom, long denom = 1L)
        {
            if (denom < 0)
            {
                nom = -nom;
                denom = -denom;
            }

            if (nom == 0)
            {
                _nom = 0;
                _denom = (denom > 0 ? 1 : 0);
            }
            else if (denom > 0)
            {
                long gcd = GCD(nom, denom);
                _nom = nom / gcd;
                _denom = denom / gcd;
            }
            else
            {
                _nom = 0;
                _denom = 0;
            }
        }


        public bool IsFinite()
        {
            return _denom > 0;
        }

        public bool IsInteger()
        {
            return _denom == 1;
        }

        static long GCD(long a, long b)
        {
            while (b != 0)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        static long LCM(long a, long b)
        {
            try
            {
                return checked((a / GCD(a, b)) * b);
            }
            catch (OverflowException)
            {
                return 0;
            }
        }

        public bool Equals(Rational other)
        {
            if (_denom == 0)
                return false;
            return _nom == other._nom && _denom == other._denom;
        }

        public override bool Equals(object obj)
        {
            return obj is Rational && Equals((Rational)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return _nom.GetHashCode() ^ ~_denom.GetHashCode();
            }
        }


        public static Rational operator +(Rational left, Rational right)
        {
            long ld = left._denom, rd = right._denom;
            if (ld > 0 && rd > 0)
            {
                try
                {
                    checked
                    {
                        if (ld == rd)
                            return new Rational(left._nom + right._nom, ld);
                        else
                        {
                            var lcm = LCM(ld, rd);
                            return new Rational(left._nom * (lcm / ld) + right._nom * (lcm / rd), lcm);
                        }
                    }
                }
                catch (OverflowException) { }
            }
        
            return NaN;
        }

        public static Rational operator -(Rational left, Rational right)
        {
            long ld = left._denom, rd = right._denom;
            if (ld > 0 && rd > 0)
            {
                try
                {
                    checked
                    {
                        if (ld == rd)
                            return new Rational(left._nom - right._nom, ld);
                        else
                        {
                            var lcm = LCM(ld, rd);
                            return new Rational(left._nom * (lcm / ld) - right._nom * (lcm / rd), lcm);
                        }
                    }
                }
                catch (OverflowException) { }
            }

            return NaN;
        }

        public static Rational operator *(Rational left, Rational right)
        {
            long ld = left._denom, rd = right._denom;
            if (ld > 0 && rd > 0)
            {
                try
                {
                    checked
                    {
                        return new Rational(left._nom * right._nom, ld * rd);
                    }
                }
                catch (OverflowException) { }
            }
            return NaN;
        }

        public static Rational operator /(Rational left, Rational right)
        {
            long ld = left._denom, rd = right._denom;
            if (ld > 0 && rd > 0)
            {
                try
                {
                    checked
                    {
                        return new Rational(left._nom * rd, right._nom * ld);
                    }
                }
                catch (OverflowException) { }
            }
            return NaN;
        }

        public static Rational Invert(Rational operand)
        {
            return new Rational(operand._denom, operand._nom);
        }

        static long LSqrt(long num)
        {
            if (num < 0)
                return -1;
            if (0 == num)
                return 0;

            long n = (num / 2) + 1;       // Initial estimate, never low  
            long n1 = (n + (num / n)) / 2;
            while (n1 < n)
            {
                n = n1;
                n1 = (n + (num / n)) / 2;
            }
            return n;
        }

        public static Rational Sqrt(Rational operand)
        {
            var D = operand._denom;
            if (D > 0)
            {
                var N = operand._nom;
                if (N >= 0)
                {
                    var nr = LSqrt(N);
                    if (nr * nr == N)
                    {
                        var dr = LSqrt(D);
                        if (dr*dr == D)
                        {
                            return new Rational(nr, dr);
                        }
                    }
                }
            }
            return NaN;
        }

        public override string ToString()
        {
            if (_denom == 0)
                return double.NaN.ToString();
            if (_denom == 1)
                return _nom.ToString();
            var wh = _nom / _denom;
            if (wh != 0)
                return string.Format("{0} {1}", wh, FractionalPart);
            else
                return string.Format("{0}/{1}", _nom, _denom);
        }

    }
}
