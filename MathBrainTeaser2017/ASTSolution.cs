using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Countdown2017
{
    class ASTSolution : Problem
    {
        readonly int MaxIterations = Digits.Length - 1;
        static readonly Func<Expr, UnaryExpr>[] unaryOps = new Func<Expr, UnaryExpr>[]
        {
            (operand) => new Sqrt(operand),
            (operand) => new Factorial(operand),
            (operand) => new DoubleFactorial(operand),
        };

        static readonly Func<Expr, Expr, BinaryExpr>[] binaryOps = new Func<Expr, Expr, BinaryExpr>[]
        {
            (left, right) => new AddExpr(left, right),
            (left, right) => new MulExpr(left, right),
            (left, right) => new SubExpr(left, right),
            (left, right) => new DivExpr(left, right),
            //(left, right) => new PowExpr(left, right),
        };

        HashSet<Expr> operands;

        protected override IDictionary<long, string> Solve()
        {
            //initial set of operands is the variations of the #'s with decimal points.  They will be combined later. 
            operands= new HashSet<Expr>(
                from comb in Digits.ToCharArray().GetVariations()
                from digits in comb.GetPermutations()
                from pow in Enumerable.Range(-digits.Length, digits.Length + 1).Reverse()
                let expr = new ConstExpr(new string(digits), pow)
                where expr.IsFinite()
                orderby expr.UsedDigits
                select expr
            );

            foreach (var operand in operands)
            {
                Console.WriteLine(operand);
            }

            for (int i = 0; i < Digits.Length && solutions.Count < ExpectedSolutions; i++)
            {
                Test(GetUnary);
                Test(GetBinary);
            };

            return solutions;
        }

        readonly Dictionary<long, string> solutions = new Dictionary<long, string>();

        /// <summary>
        /// If the given expression results in an integer between min and max target, 
        /// and and the resultant integer hasn't yet been solved, add it to the solutions.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        bool CheckSolution(Expr result)
        {
            var rVal = result.Value;
            if (rVal.IsInteger() && rVal.Nominator >= MinTarget && rVal.Nominator <= MaxTarget)
            {
                if (!solutions.ContainsKey(rVal.Nominator))
                {
                    solutions.Add(rVal.Nominator, result.ToString());
                    Console.WriteLine("{0} <- {1}", rVal.Nominator, result.ToString());
                    return true;
                }
            }
            return false;
        }

        void Test(Func<IReadOnlyList<Expr>> getResults)
        {
            if (solutions.Count >= ExpectedSolutions)
                return;

            bool found = false;
            foreach (var result in getResults())
            {

                if (result.UsedDigits == Digits.Length)
                {
                    //We reach the digits limit
                    found |= CheckSolution(result);

                    //We won't add the result to the operands set because further binary combinations are impossible
                    //Trying only useful unary combinations
                    foreach (var make_unary in unaryOps)
                    {
                        var unary = make_unary(result);
                        var uVal = unary.Value;
                        if(uVal.IsFinite() && !uVal.Equals(result.Value))
                        {
                            found |= CheckSolution(unary);
                            operands.Add(unary);
                        }
                    }
                    if (solutions.Count >= ExpectedSolutions)
                        break;
                }
                else
                {
                    //expression isn't using all the digits, but can be useful for further permutations....
                    operands.Add(result);
                }
            }
            if(found)
                Console.WriteLine("{0} solutions after {1} seconds", solutions.Count, ExecutionTime);
        }

        public IReadOnlyList<Expr> GetBinary()
        {
            var resultSet = new HashSet<Expr>();
            Expr[] elements = new Expr[operands.Count];
            operands.CopyTo(elements);
            for (int i = 0; i < elements.Length; i++)
            {
                var left = elements[i];
                var c1 = left.UsedDigits;
                if (c1 < Digits.Length)
                {
                    for (int j = i + 1; j < elements.Length; j++)
                    {
                        var right = elements[j];
                        if (c1 + right.UsedDigits <= Digits.Length)
                        {
                            for (int k = 0; k < binaryOps.Length; k++)
                            {
                                //make a binary expression from the left and right operands
                                var make_binary = binaryOps[k];
                                var op = make_binary(left, right);
                                if (op.IsFinite() && !operands.Contains(op))
                                {
                                    resultSet.Add(op);
                                }

                                //if we hit a non-commutative expression, check and add 
                                //   example1: if op = "2-1", then add to the resultSet "1-2"
                                //   example2: if op = "2+1", then don't bother as "1+2" = "2+1"
                                if (!(op is CommutativeExpr))
                                {
                                    op = make_binary(right, left);
                                    if (op.IsFinite() && !operands.Contains(op))
                                    {
                                        resultSet.Add(op);
                                    }

                                }
                            }
                        }
                    }
                }
            }
            Expr[] result = new Expr[resultSet.Count];
            resultSet.CopyTo(result);
            return result;
        }

        IReadOnlyList<UnaryExpr> GetUnary()
        {
            var result = new List<UnaryExpr>(operands.Count);
            foreach (var operand in operands)
            {
                var val = operand.Value;
                foreach (var make_unary in unaryOps)
                {
                    var op = make_unary(operand);

                    //Drop expressions that doesn't change the value
                    //e.g. 1!, 1!!, (1!)!...
                    if (op.IsFinite() && !op.Value.Equals(val) && !operands.Contains(op))
                    {
                        result.Add(op);
                    }
                }
            }
            return result;
        }

    }


}
