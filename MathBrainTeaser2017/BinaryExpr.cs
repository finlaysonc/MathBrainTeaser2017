using System;
using Microsoft.SolverFoundation.Common;

namespace MathBrainTeaser2017
{
    public abstract class BinaryExpr : Expr
    {
        protected readonly Expr Left;
        protected readonly string Operator;
        protected readonly Expr Right;

        protected BinaryExpr(Expr left, Expr right, string symbol)
            : base(left.Digits + right.Digits)
        {
            Left = left;
            Right = right;
            Operator = symbol;
        }

        protected override bool IsValid()
        {
            return Left.IsFinite&& Right.IsFinite&& base.IsValid();
        }

        protected override string Stringify()
        {
            return $"({Left} {Operator} {Right})";
        }
    }

    public abstract class CommutativeExpr : BinaryExpr
    {
        protected CommutativeExpr(Expr left, Expr right, string symbol) : base(left, right, symbol) {}


        public override bool Equals(object obj)
        {
            CommutativeExpr comm = obj as CommutativeExpr;
            if (comm != null && comm.GetType() == GetType())
            {
                return Left.Equals(comm.Left) && Right.Equals(comm.Right) ||
                       Right.Equals(comm.Left) && Left.Equals(comm.Right);
            }
            return false;
        }
    }

    public sealed class AddExpr : CommutativeExpr
    {
        public AddExpr(Expr left, Expr right) : base(left, right, "+") {}

        protected override Rational Evaluate
        {
            get
            {
                return Left.Value + Right.Value;
            }
        }
    }

    public sealed class MulExpr : CommutativeExpr
    {
        public MulExpr(Expr left, Expr right) : base(left, right, "*") {}

        protected override Rational Evaluate
        {
            get
            {
                return Left.Value * Right.Value;
            }
        }
    }

    public sealed class SubExpr : BinaryExpr
    {
        public SubExpr(Expr left, Expr right) : base(left, right, "-") {}

        protected override Rational Evaluate
        {
            get
            {
                return Left.Value - Right.Value;
            }
        }
    }

    public sealed class DivExpr : BinaryExpr
    {
        public DivExpr(Expr left, Expr right) : base(left, right, "/") {}


        protected override Rational Evaluate
        {
            get { return Left.Value / Right.Value; }
        }
    }
}