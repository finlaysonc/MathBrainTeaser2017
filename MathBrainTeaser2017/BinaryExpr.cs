using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Countdown2017
{
    public abstract class BinaryExpr : Expr
    {
        protected readonly Expr Left;
        protected readonly Expr Right;
        protected readonly string Operator;
        public BinaryExpr(Expr left, Expr right, string symbol)
            : base(left.Digits+right.Digits)
        {
            Left = left;
            Right = right;
            Operator = symbol;
        }

        protected override bool IsValid()
        {
            return Left.IsFinite() && Right.IsFinite() && base.IsValid();
        }

        protected override string Stringify()
        {
            return $"({Left} {Operator} {Right})";
        }
    }

    public abstract class CommutativeExpr : BinaryExpr
    {
        public CommutativeExpr(Expr left, Expr right, string symbol) : base(left, right, symbol)
        {
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode() ^ Left.GetHashCode() ^ Right.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var comm = obj as CommutativeExpr;
            if (comm != null && comm.GetType() == GetType())
            {
                return Left.Equals(comm.Left) && Right.Equals(comm.Left) ||
                    Right.Equals(comm.Left) && Left.Equals(comm.Left);
            }
            return false;
        }
    }

    public sealed class AddExpr : CommutativeExpr
    {
        public AddExpr(Expr left, Expr right) : base(left, right, "+")
        {
        }

        protected override Rational Evaluate()
        {
            return Left.Value + Right.Value;
        }
    }

    public sealed class MulExpr : CommutativeExpr
    {
        public MulExpr(Expr left, Expr right) : base(left, right, "*")
        {
        }

        protected override Rational Evaluate()
        {
            return Left.Value * Right.Value;
        }
    }

    public sealed class SubExpr : BinaryExpr
    {
        public SubExpr(Expr left, Expr right) : base(left, right, "-")
        {
        }

        protected override Rational Evaluate()
        {
            return Left.Value - Right.Value;
        }
    }

    public sealed class DivExpr : BinaryExpr
    {
        public DivExpr(Expr left, Expr right) : base(left, right, "/")
        {
        }

        protected override Rational Evaluate()
        {
            return Left.Value / Right.Value;
        }
    }

    public sealed class PowExpr : BinaryExpr
    {
        public PowExpr(Expr left, Expr right) : base(left, right, "^")
        {
        }

        protected override Rational Evaluate()
        {
            return Rational.Power(Left.Value, Right.Value);
        }
    }
}
