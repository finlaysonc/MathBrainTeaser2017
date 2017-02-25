using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using Combinatorics.Collections;
using ExpressionEvaluator;
using MathBrainTeaser2017;
using xFunc.Maths;
using xFunc.Maths.Expressions.Collections;
using xFunc.Maths.Results;
using xFunc.Maths.Expressions;
using xFunc.Maths.Expressions.Statistical;
using PolishNotation;
internal class CountingProblem
{

    private readonly string[]
        _patterns = {"nnonnoo", "nnonono", "nnnoono", "nnnonoo", "nnnnooo"};

    private readonly string[]
        _patterns2 = {"nnnoo", "nnono"};

    private const string Ops = "^+-/*";


    public static void Main(string[] args)
    {
        new CountingProblem().Go();
    }
    Processor _processor = new Processor();


    protected virtual void Go()
    {
        ExpressionEvaluator.CompiledExpression e = new CompiledExpression("1 +2");
        //var parameters = _processor.Parameters.Variables;
        //parameters.Add(new Parameter("2", 2, ParameterType.Normal));
        //parameters.Add(new Parameter("0", 0, ParameterType.Normal));
        //parameters.Add(new Parameter("1", 1, ParameterType.Normal));
        //parameters.Add(new Parameter("7", 7, ParameterType.Normal));


        var digits = new[] {"2", "0", "1", "7"};
      var dPerms = new Permutations<string>(digits, GenerateOption.WithoutRepetition).ToList();
        var opsPerms = new Permutations<char>(Ops.ToList(), GenerateOption.WithRepetition).ToList();



        for (var i = 1; i <= 100; i++)
        {
            var result = "";
                result = FindFirstSolution(dPerms, opsPerms, _patterns, i);
            Console.WriteLine(i + ":> " + result);
        }
    }


    private string FindFirstSolution(List<IList<string>> dPerms, List<IList<char>>  opsPerms, string[] postFixPatterns,
        int goal)
    {
        var sb = new StringBuilder();

        foreach (var pattern in postFixPatterns)
        {
            var patternChars = pattern.ToCharArray();

            foreach (var dig in dPerms)
                foreach (var opr in opsPerms)
                {
                    int i = 0, j = 0;
                    foreach (var c in patternChars)
                    {
                        if (c == 'n')
                        {
                            var var = dig[i++];
                            sb.Append(var);
                        }
                        else if (c == 'o')
                        {
                            sb.Append(opr[j++]);
                        }
                        else if (c == 'u') //these are unary operators - 
                        {
                            sb.Append('!');
                        }
                        sb.Append(" ");
                    }

                    var candidate = sb.ToString();
                    var infix = "";
                    try
                    {
                        infix = PostfixToInfix(candidate);
                    }
                    catch
                    {
                        continue;
                    }
                    //infix = infix.Replace('a', '2').Replace('b', '0').Replace('c', '1').Replace('d', '7');
                    //infix = infix.Replace("e", "20");
                 //   Console.WriteLine(infix);
                    sb.Clear();
                    IExpression ex1 = _processor.Parse(infix);
                    var isAllInts = Helpers.ConvertExpressionToCollection(ex1)
                        .ToList().TrueForAll(x => ((double)x.Execute()) %1 == 0);
                    if (!isAllInts)
                    {
                        continue; //not allowed to have intermediate results that aren't integers
                    }
                    int result = (Int32) ((Double) ex1.Execute());

                    if ((Int32)result == goal)
                        return ex1.ToString();
                }
        }
        return "not found";
    }
    /*

    private class Expression
    {
        public string op, ex;
        public readonly int prec = 3;

        public Expression(string e)
        {
            ex = e;
        }

        public Expression(string e1, string e2, string o)
        {
            ex = $"{e1} {o} {e2}";
            op = o;
            prec = Ops.IndexOf(o, StringComparison.Ordinal)/2;
        }
    }


    protected virtual string PostfixToInfix(string postfix)
    {
        var expr = new Stack<Expression>();

        foreach (var c in postfix.ToCharArray())
        {
            var idx = Ops.IndexOf(c);
            if (idx != -1)
            {
                var r = expr.Pop();
                var l = expr.Pop();

                var opPrec = idx/2;

                if (l.prec < opPrec)
                    l.ex = '(' + l.ex + ')';

                if (r.prec <= opPrec)
                    r.ex = '(' + r.ex + ')';

                expr.Push(new Expression(l.ex, r.ex, "" + c));
            }
            else
            {
                expr.Push(new Expression("" + c));
            }
        }
        return expr.Peek().ex;
    }

    */
    class Intermediate
    {
        public string expr;     // subexpression string
        public string oper;     // the operator used to create this expression

        public Intermediate(string expr, string oper)
        {
            this.expr = expr;
            this.oper = oper;
        }
    }

    //
    // PostfixToInfix
    //
    public static string PostfixToInfix(string postfix)
    {
        ConvertTo ct = new ConvertTo();
        ct.POSTstr = postfix;
        return ct.PostToIn();
/*

        // Assumption: the postfix expression to be processed is space-delimited.
        // Split the individual tokens into an array for processing.
        var postfixTokens = postfix.Split(' ');

        // Create stack for holding intermediate infix expressions
        var stack = new Stack<Intermediate>();

        foreach (string token in postfixTokens)
        {
            if (token == "+" || token == "-")
            {
                // Get the left and right operands from the stack.
                // Note that since + and - are lowest precedence operators,
                // we do not have to add any parentheses to the operands.
                var rightIntermediate = stack.Pop();
                var leftIntermediate = stack.Pop();

                // construct the new intermediate expression by combining the left and right 
                // expressions using the operator (token).
                var newExpr = leftIntermediate.expr + token + rightIntermediate.expr;

                // Push the new intermediate expression on the stack
                stack.Push(new Intermediate(newExpr, token));
            }
            else if (token == "*" || token == "/")
            {
                string leftExpr, rightExpr;

                // Get the intermediate expressions from the stack.  
                // If an intermediate expression was constructed using a lower precedent
                // operator (+ or -), we must place parentheses around it to ensure 
                // the proper order of evaluation.

                var rightIntermediate = stack.Pop();
                if (rightIntermediate.oper == "+" || rightIntermediate.oper == "-")
                {
                    rightExpr = "(" + rightIntermediate.expr + ")";
                }
                else
                {
                    rightExpr = rightIntermediate.expr;
                }

                var leftIntermediate = stack.Pop();
                if (leftIntermediate.oper == "+" || leftIntermediate.oper == "-")
                {
                    leftExpr = "(" + leftIntermediate.expr + ")";
                }
                else
                {
                    leftExpr = leftIntermediate.expr;
                }

                // construct the new intermediate expression by combining the left and right 
                // using the operator (token).
                var newExpr = leftExpr + token + rightExpr;

                // Push the new intermediate expression on the stack
                stack.Push(new Intermediate(newExpr, token));
            }
            else
            {
                // Must be a number. Push it on the stack.
                stack.Push(new Intermediate(token, ""));
            }
        }

        // The loop above leaves the final expression on the top of the stack.
        return stack.Last().expr;*/
    }
}