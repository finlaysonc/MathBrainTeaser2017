using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressions
{
    class Program3
    {
        private static IList<Expression<Func<double, double>>> _singles;
        private static IList<Expression<Func<double, double, double>>> _doubles;
        private static readonly Expression<Func<double, double, double>> _concatenation = (a, b) => Concat(a, b);
        private static readonly Expression<Func<double, double>> _noOp = x => x;

        static void Main2(string[] args)
        {
            _singles = GetSingleExpressions().ToList();
            _doubles = GetDoubleExpressions().ToList();
            double[] digits = { 2, 0, 1, 7 };

            var results = new List<string>[100];

            var sw = Stopwatch.StartNew();
            bool done = false;
            //foreach (double[] inputs in GetPermutations(digits, 4, false))
            Parallel.ForEach(GetPermutations(digits, 4, false), inputs =>
            {
                //Parallel.ForEach(GetExpressions(inputs), expression =>
                foreach (var expression in GetExpressions(inputs))
                {
                    var result = expression.Compile()();
                    if (result > 0.0 && result <= 100.0 && result.IsInteger())
                    {
                        string equation = DisplayExpression.GetDisplayString(expression);
                        int resultIndex = (int)result - 1;
                        lock (results)
                        {
                            if (results[resultIndex] == null)
                            {
                                results[resultIndex] = new List<string>();
                            }
                            results[resultIndex].Add(equation);
                            done = results.All(x => x?.Count > 0);
                        }
                    }
                } //});
            }); //}
            sw.Stop();
            Console.WriteLine($"Found {results.Count(x => x?.Count > 0)} results in {sw.Elapsed}");
            Console.ReadLine();
        }

        private static IEnumerable<Expression<Func<double>>> GetExpressions(double[] inputs)
        {
            foreach (Expression<Func<double, double, double>>[] permutationArray in GetPermutations(_doubles, 3, true))
            {
                //We are only going to allow concatenations at the bottom of the tree.
                if (permutationArray[0] == _concatenation)
                    continue;

                //Tree looks like this
                /*        result
                 *          c1
                 *         /  \
                 *       c2    c3
                 *      / \    / \
                 *     i1  i2 i3  i4
                 */

                foreach (Expression<Func<double, double>>[] singleFuncs in GetPermutations(_singles, 6, true))
                {
                    if (permutationArray[1] == _concatenation && (singleFuncs[0] != _noOp || singleFuncs[1] != _noOp))
                        continue;
                    if (permutationArray[2] == _concatenation && (singleFuncs[2] != _noOp || singleFuncs[3] != _noOp))
                        continue;

                    Expression param1 = Expression.Constant(inputs[0]);
                    Expression param2 = Expression.Constant(inputs[1]);
                    Expression param3 = Expression.Constant(inputs[2]);
                    Expression param4 = Expression.Constant(inputs[3]);

                    if (singleFuncs[0] != _noOp)
                        param1 = Expression.Invoke(singleFuncs[0], param1);
                    if (singleFuncs[1] != _noOp)
                        param2 = Expression.Invoke(singleFuncs[1], param2);
                    if (singleFuncs[2] != _noOp)
                        param3 = Expression.Invoke(singleFuncs[2], param3);
                    if (singleFuncs[3] != _noOp)
                        param4 = Expression.Invoke(singleFuncs[3], param4);


                    Expression c2 = Expression.Invoke(permutationArray[1], param1, param2);
                    Expression c3 = Expression.Invoke(permutationArray[2], param3, param4);

                    if (singleFuncs[4] != _noOp)
                        c2 = Expression.Invoke(singleFuncs[4], c2);
                    if (singleFuncs[5] != _noOp)
                        c3 = Expression.Invoke(singleFuncs[5], c3);

                    var c1 = Expression.Invoke(permutationArray[0], c2, c3);

                    yield return Expression.Lambda<Func<double>>(c1);
                }
            }
        }

        private static IEnumerable<Expression<Func<double, double>>> GetSingleExpressions()
        {
            yield return _noOp;
            yield return x => Factorial(x);
            yield return x => DoubleFactorial(x);
            yield return x => Math.Sqrt(x);
        }

        private static IEnumerable<Expression<Func<double, double, double>>> GetDoubleExpressions()
        {
            yield return (a, b) => a + b;
            yield return (a, b) => a - b;
            yield return (a, b) => a * b;
            yield return (a, b) => a / b;
            yield return (a, b) => Math.Pow(a, b);
            yield return _concatenation;
        }

        //Variation on algorythm from here with multiple enumeration problem fixed.
        //http://stackoverflow.com/questions/1952153/what-is-the-best-way-to-find-all-combinations-of-items-in-an-array/10629938#10629938
        private static IEnumerable<T[]> GetPermutations<T>(IList<T> list, int length, bool allowRepeats)
        {
            if (length == 1) return list.Select(t => new[] { t });
            return GetPermutations(list, length - 1, allowRepeats).SelectMany(t => list.Where(o => allowRepeats || !t.Contains(o)), (t1, t2) => t1.Concat(new[] { t2 }).ToArray());
        }

        private static double Factorial(double x)
        {
            int number = (int)x;
            for (int i = number - 1; i >= 1; i--)
            {
                number = number * i;
            }
            return number;
        }

        private static double DoubleFactorial(double x)
        {
            int number = (int)x;
            for (int i = number - 1; i >= 1; i--)
            {
                number = number * i;
            }
            return number;
        }

        private static double Concat(double x, double y)
        {
            if (x.IsInteger() && y.IsInteger())
            {
                return double.Parse($"{(int)x}{(int)y}");
            }
            throw new InvalidOperationException();
        }

        private class DisplayExpression : ExpressionVisitor
        {
            private readonly StringBuilder _displayString = new StringBuilder();

            public static string GetDisplayString(Expression expression)
            {
                var visitor = new DisplayExpression();
                visitor.Visit(expression);
                return visitor._displayString.ToString();
            }

            protected override Expression VisitBinary(BinaryExpression node)
            {
                Visit(node.Left);
                //Taken from System.Linq.Expressions.DebugViewWriter.VisitBinary()
                switch (node.NodeType)
                {
                    case ExpressionType.Add:
                        _displayString.Append("+");
                        break;
                    case ExpressionType.AddChecked:
                        _displayString.Append("+");
                        break;
                    case ExpressionType.And:
                        _displayString.Append("&");
                        break;
                    case ExpressionType.AndAlso:
                        _displayString.Append("&&");
                        break;
                    case ExpressionType.Coalesce:
                        _displayString.Append("??");
                        break;
                    case ExpressionType.Divide:
                        _displayString.Append("/");
                        break;
                    case ExpressionType.Equal:
                        _displayString.Append("==");
                        break;
                    case ExpressionType.ExclusiveOr:
                        _displayString.Append("^");
                        break;
                    case ExpressionType.GreaterThan:
                        _displayString.Append(">");
                        break;
                    case ExpressionType.GreaterThanOrEqual:
                        _displayString.Append(">=");
                        break;
                    case ExpressionType.LeftShift:
                        _displayString.Append("<<");
                        break;
                    case ExpressionType.LessThan:
                        _displayString.Append("<");
                        break;
                    case ExpressionType.LessThanOrEqual:
                        _displayString.Append("<=");
                        break;
                    case ExpressionType.Modulo:
                        _displayString.Append("%");
                        break;
                    case ExpressionType.Multiply:
                        _displayString.Append("*");
                        break;
                    case ExpressionType.MultiplyChecked:
                        _displayString.Append("*");
                        break;
                    case ExpressionType.NotEqual:
                        _displayString.Append("!=");
                        break;
                    case ExpressionType.Or:
                        _displayString.Append("|");
                        break;
                    case ExpressionType.OrElse:
                        _displayString.Append("||");
                        break;
                    case ExpressionType.Power:
                        _displayString.Append("**");
                        break;
                    case ExpressionType.RightShift:
                        _displayString.Append(">>");
                        break;
                    case ExpressionType.Subtract:
                        _displayString.Append("-");
                        break;
                    case ExpressionType.SubtractChecked:
                        _displayString.Append("-");
                        break;
                    case ExpressionType.Assign:
                        _displayString.Append("=");
                        break;
                    case ExpressionType.AddAssign:
                        _displayString.Append("+=");
                        break;
                    case ExpressionType.AndAssign:
                        _displayString.Append("&=");
                        break;
                    case ExpressionType.DivideAssign:
                        _displayString.Append("/=");
                        break;
                    case ExpressionType.ExclusiveOrAssign:
                        _displayString.Append("^=");
                        break;
                    case ExpressionType.LeftShiftAssign:
                        _displayString.Append("<<=");
                        break;
                    case ExpressionType.ModuloAssign:
                        _displayString.Append("%=");
                        break;
                    case ExpressionType.MultiplyAssign:
                        _displayString.Append("*=");
                        break;
                    case ExpressionType.OrAssign:
                        _displayString.Append("|=");
                        break;
                    case ExpressionType.PowerAssign:
                        _displayString.Append("**=");
                        break;
                    case ExpressionType.RightShiftAssign:
                        _displayString.Append(">>=");
                        break;
                    case ExpressionType.SubtractAssign:
                        _displayString.Append("-=");
                        break;
                    case ExpressionType.AddAssignChecked:
                        _displayString.Append("+=");
                        break;
                    case ExpressionType.MultiplyAssignChecked:
                        _displayString.Append("*=");
                        break;
                    case ExpressionType.SubtractAssignChecked:
                        _displayString.Append("-=");
                        break;
                    default:
                        break;
                }
                Visit(node.Right);
                return base.VisitBinary(node);
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                if (node.Method.DeclaringType == typeof(Math))
                {
                    switch (node.Method.Name)
                    {
                        case nameof(Math.Pow):
                            _displayString.Append("^");
                            break;
                        case nameof(Math.Sqrt):
                            _displayString.Append("sqrt(");
                            return base.VisitMethodCall(node);
                    }
                }
                else if (node.Method.DeclaringType == typeof(Program3))
                {
                    switch (node.Method.Name)
                    {
                        case nameof(Factorial):
                            _displayString.Append("!");
                            break;
                        case nameof(DoubleFactorial):
                            _displayString.Append("!!");
                            break;
                        case nameof(Concat):
                            _displayString.Append("");
                            break;
                    }
                }
                return base.VisitMethodCall(node);
            }

            protected override Expression VisitConstant(ConstantExpression node)
            {
                _displayString.Append(node.Value);
                return base.VisitConstant(node);
            }

            protected override Expression VisitInvocation(InvocationExpression node)
            {
                _displayString.Append("(");
                if (node.Arguments.Count == 2)
                {
                    Visit(node.Arguments[0]);
                    Visit(node.Expression);
                    Visit(node.Arguments[1]);
                }
                else if (((node.Expression as LambdaExpression)?.Body as MethodCallExpression)?.Method.Name == nameof(Math.Sqrt))
                {
                    base.VisitInvocation(node);
                    _displayString.Append(")");
                }
                else
                {
                    foreach (Expression argument in node.Arguments)
                    {
                        Visit(argument);
                    }
                    Visit(node.Expression);
                }
                _displayString.Append(")");

                return Expression.Invoke(node.Expression, node.Arguments);
            }

        }
    }

    public static class NumberMixins
    {
        public static bool IsInteger(this double @double, double threshold = double.Epsilon * 1000)
        {
            return @double % 1 <= threshold;
        }
    }
}