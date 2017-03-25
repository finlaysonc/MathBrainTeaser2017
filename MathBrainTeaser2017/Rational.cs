using System;

namespace Countdown2017
{
    public struct Rational : IEquatable<Rational>
    {
        public static readonly Rational NaN = default(Rational);
        public static readonly Rational Zero = new Rational(0, 1);
        public static readonly Rational One = new Rational(1, 1);

        public long Nominator { get; }
        public long Denominator { get; }

        public Rational FractionalPart
        {
            get
            {
                if (Denominator > 0)
                {
                    long wh = Nominator / Denominator;
                    long fr = Nominator - Denominator * wh;
                    return new Rational(fr < 0 ? -fr : fr, Denominator);
                }
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
                Nominator = 0;
                Denominator = denom > 0 ? 1 : 0;
            }
            else if (denom > 0)
            {
                long gcd = GCD(nom, denom);
                Nominator = nom / gcd;
                Denominator = denom / gcd;
            }
            else
            {
                Nominator = 0;
                Denominator = 0;
            }
        }


        public bool IsFinite()
        {
            return Denominator > 0;
        }

        public bool IsInteger()
        {
            return Denominator == 1;
        }

        private static long GCD(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        private static long LCM(long a, long b)
        {
            try
            {
                return checked(a / GCD(a, b) * b);
            }
            catch (OverflowException)
            {
                return 0;
            }
        }

        public bool Equals(Rational other)
        {
            if (Denominator == 0)
            {
                return false;
            }
            return Nominator == other.Nominator && Denominator == other.Denominator;
        }

        public override bool Equals(object obj)
        {
            return obj is Rational && Equals((Rational) obj);
        }

        public override int GetHashCode()
        {
            return Nominator.GetHashCode() ^ ~Denominator.GetHashCode();
        }


        public static Rational operator +(Rational left, Rational right)
        {
            long ld = left.Denominator, rd = right.Denominator;
            if (ld > 0 && rd > 0)
            {
                try
                {
                    checked
                    {
                        if (ld == rd)
                        {
                            return new Rational(left.Nominator + right.Nominator, ld);
                        }
                        long lcm = LCM(ld, rd);
                        return new Rational(left.Nominator * (lcm / ld) + right.Nominator * (lcm / rd), lcm);
                    }
                }
                catch (OverflowException) {}
            }

            return NaN;
        }

        public static Rational operator -(Rational left, Rational right)
        {
            long ld = left.Denominator, rd = right.Denominator;
            if (ld > 0 && rd > 0)
            {
                try
                {
                    checked
                    {
                        if (ld == rd)
                        {
                            return new Rational(left.Nominator - right.Nominator, ld);
                        }
                        long lcm = LCM(ld, rd);
                        return new Rational(left.Nominator * (lcm / ld) - right.Nominator * (lcm / rd), lcm);
                    }
                }
                catch (OverflowException) {}
            }

            return NaN;
        }

        public static Rational operator *(Rational left, Rational right)
        {
            long ld = left.Denominator, rd = right.Denominator;
            if (ld > 0 && rd > 0)
            {
                try
                {
                    checked
                    {
                        return new Rational(left.Nominator * right.Nominator, ld * rd);
                    }
                }
                catch (OverflowException) {}
            }
            return NaN;
        }

        public static Rational operator /(Rational left, Rational right)
        {
            long ld = left.Denominator, rd = right.Denominator;
            if (ld > 0 && rd > 0)
            {
                try
                {
                    checked
                    {
                        return new Rational(left.Nominator * rd, right.Nominator * ld);
                    }
                }
                catch (OverflowException) {}
            }
            return NaN;
        }

        public static Rational Invert(Rational operand)
        {
            return new Rational(operand.Denominator, operand.Nominator);
        }

        private static long LSqrt(long num)
        {
            if (num < 0)
            {
                return -1;
            }
            if (0 == num)
            {
                return 0;
            }

            long n = num / 2 + 1; // Initial estimate, never low  
            long n1 = (n + num / n) / 2;
            while (n1 < n)
            {
                n = n1;
                n1 = (n + num / n) / 2;
            }
            return n;
        }

        public static Rational Sqrt(Rational operand)
        {
            long D = operand.Denominator;
            if (D > 0)
            {
                long N = operand.Nominator;
                if (N >= 0)
                {
                    long nr = LSqrt(N);
                    if (nr * nr == N)
                    {
                        long dr = LSqrt(D);
                        if (dr * dr == D)
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
            if (Denominator == 0)
            {
                return double.NaN.ToString();
            }
            if (Denominator == 1)
            {
                return Nominator.ToString();
            }
            long wh = Nominator / Denominator;
            if (wh != 0)
            {
                return string.Format("{0} {1}", wh, FractionalPart);
            }
            return string.Format("{0}/{1}", Nominator, Denominator);
        }
    }
}