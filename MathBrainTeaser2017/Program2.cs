using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Combinatorics.Collections;
using MathBrainTeaser2017;
using xFunc.Maths;
using xFunc.Maths.Expressions;
using xFunc.Maths.Expressions.Collections;
using xFunc.Maths.Tokens;

internal class CountingProblem
{
    //private readonly string[]
    //_patterns = {"nunonnoo", "nnonono", "nnnoono", "nnnonoo", "nnnnooo", "nunuononou"};
    private readonly string[]
        _patterns = { "nunuonuonuo" };

    private readonly string[]
        _patterns2 = {"nnono"};

    private readonly string[]
        _patterns3 = {"nno"};

    private const string Ops = "^+-/*";
    private static readonly string[] UnaryOps = new[] {"!"};

    public static void Main(string[] args)
    {
        //Processor _processor = new Processor();

        //var x = _processor.Parse("Sqrt(2! - Sqrt(1) + 7 - 0)");
        //var y = x.Execute();
//        _processor.Parse("(sqrt(4) + 2) - 2");
        new CountingProblem().Go();
    }

    Processor _processor = new Processor();


    protected virtual void Go()
    {
        //ConvertTo ct = new ConvertTo();
        //ct.POSTstr = "231 7 +";
        //var s = ct.PostToIn();
        //        var x = _processor.Parse("(sqrt(0!^sqrt(1)^2)^7)!").Execute();
        //        var x = _processor.Parse("(2 + 7)!").Execute();

        //List<IToken> tokens = new List<IToken>();
        //tokens.Add(new NumberToken(1));
        //tokens.Add(new FunctionToken(Functions.Sqrt, 1));
//        var exp = _processor.Parse("3! + sqrt(2)");
       var  exp = _processor.Parse("abs(3)");
        var s = exp.Execute();

        //var functions = new FunctionCollection();
        //functions.Add(new UserFunction("s", new IExpression[] {new Variable("x")}, 1), new Number((new Variable("x")).

        //var func = new UserFunction("f", new IExpression[] { new Number(1) }, 1);
        //Assert.Equal(Math.Log(1), func.Execute(functions));

        var digits = new[] {"2", "0", "1", "7"};
        List<IList<string>> dPerms = new Permutations<string>(digits, GenerateOption.WithoutRepetition).ToList();
        List<IList<string>> dPerms2 = new List<IList<string>>();
        List<IList<string>> dPerms3 = new List<IList<string>>();
        //        var dPerms2 = new Permutations<string>(digits, GenerateOption.WithoutRepetition).ToList();

        foreach (var perm in dPerms)
        {
            dPerms2.Add(new[] {perm[0] + perm[1], perm[2], perm[3]});
        }
        foreach (var perm in dPerms)
        {
            dPerms3.Add(new[] {perm[0] + perm[1] + perm[2], perm[3]});
        }
        //var opsPerms = new Permutations<char>(Ops.ToList(), GenerateOption.WithRepetition).ToList();
        //var unaryOpsPerms = new Permutations<string>(UnaryOps, GenerateOption.WithRepetition).ToList();
        var opsPerms = new Variations<char>(Ops.ToList(), 3, GenerateOption.WithRepetition).ToList();
        var unaryOpsPerms = new Variations<string>(UnaryOps, 4, GenerateOption.WithRepetition).ToList();

        //var unarytypes = new Variations<string>(UnaryOps,2,GenerateOption.WithRepetition);
        //var unarytypes2 = new Variations<string>(UnaryOps, 4, GenerateOption.WithRepetition);
        for (var i = 1; i <= 100; i++)
        {
            var result = "";
            result = FindFirstSolution(dPerms, opsPerms, unaryOpsPerms, _patterns, i);
            //result = FindFirstSolution(dPerms2, opsPerms, _patterns2, i);
            //result = FindFirstSolution(dPerms3, opsPerms, _patterns3, i);
            Console.WriteLine(i + ":> " + result);
        }
    }


    private string FindFirstSolution(List<IList<string>> dPerms, List<IList<char>> opsPerms,
        List<IList<string>> unaryOpsPerms,
        string[] postFixPatterns,
        int goal)
    {
        var sb = new StringBuilder(",");

        foreach (var pattern in postFixPatterns)
        {
            var patternChars = pattern.ToCharArray();
            foreach (var dig in dPerms)
            foreach (var opr in opsPerms)
            foreach (var unaryOp  in unaryOpsPerms)
            {
                List<IToken> tokens = new List<IToken>();
                int i = 0, j = 0, k = 0;
                for (int pc = 0; pc < patternChars.Length; pc++)
//                foreach (var c in patternChars)
                {
                    char c = patternChars[pc];
                    IToken token = null;
                    if (c == 'n')
                    {
                        token = new NumberToken(Int32.Parse(dig[i++]));

                        //var var = dig[i++];
                        //sb.Append(var);
                        //if (patternChars[pc + 1] == 'n')
                        //{
                        //    sb.Append(",");
                        //}
                    }
                    else if (c == 'o')
                    {
                        char operand = opr[j++];
                        switch (operand)
                        {
                            case '+':
                                token = new OperationToken(Operations.Addition);
                                break;
                            case '-':
                                token = new OperationToken(Operations.Subtraction);
                                break;
                            case '/':
                                token = new OperationToken(Operations.Division);
                                break;
                            case '*':
                                token = new OperationToken(Operations.Multiplication);
                                break;
                            case '^':
                                token = new OperationToken(Operations.Exponentiation);
                                break;
                        }

//                        sb.Append(opr[j++]);
                    }
                    else if (c == 'u')
                    {
                        string unaryOperand = unaryOp[k++];
                        switch (unaryOperand)
                        {
                            case "!":
                                        token = new FunctionToken(Functions.Factorial);
//                                        token = new OperationToken(Operations.Factorial);
                                break;
                                    case ":=":
                                token = new OperationToken(Operations.Assign);
                                break;
                                    case "sqrt":
                                token = new FunctionToken(Functions.Sqrt, 1);

                                        break;
                                }
//                        sb.Append(unaryOp);
                    }

                    tokens.Add(token);
//                    sb.Append(" ");
                }

                try
                {
                    var rpnExp = _processor.Parser.Parse(tokens);
                    double result = Convert.ToDouble(rpnExp.Execute());
//                Console.WriteLine($@"{rpnExp.ToString()} {result}") ;

                    if (Double.IsInfinity(result) || Double.IsNaN(result) || Double.IsNegativeInfinity(result))
                    {
                        continue;
                    }
                    if (Convert.ToInt32(result) == goal)
                    {
                        return rpnExp.ToString();
                    }
                }
                catch (Exception e)
                {
//                    Console.WriteLine("error: " + tokens.ToString());
                    continue;
                }

                //int result = (Int32)((Double)ex1.Execute());

                //if ((Int32)result == goal)
                //    return ex1.ToString();

                //        var candidate = sb.ToString();
                //sb.Clear();
                //var infix = "";
                //try
                //{
                //    infix = PostfixToInfix(candidate);
                //}
                //catch
                //{
                //    continue;
                //}
                //IExpression ex1 = _processor.Parse(infix);
                //var expressions = Helpers.ConvertExpressionToCollection(ex1);
                //var isAllInts = Helpers.ConvertExpressionToCollection(ex1)
                //    .ToList().TrueForAll(x => ((double) x.Execute()) % 1 == 0);
                //if (!isAllInts)
                //{
                //    continue; //not allowed to have intermediate results that aren't integers
                //}

                //int result = (Int32) ((Double) ex1.Execute());

                //if ((Int32) result == goal)
                //    return ex1.ToString();
            }
        }
        return "not found";
    }

    //
    // PostfixToInfix
    //
    public static string PostfixToInfix(string postfix)
    {
        //string commaPostfix = ",";
        //for (int i = 0; i < postfix.Length-2; i++)
        //{
        //    commaPostfix += postfix[i];
        //    if (char.IsNumber(postfix[i]) && !char.IsNumber(postfix[i + 1]))
        //    {
        //        commaPostfix += ",";
        //    }
        //}
        ConvertTo ct = new ConvertTo();
        ct.POSTstr = postfix;
        return ct.PostToIn();
    }
}