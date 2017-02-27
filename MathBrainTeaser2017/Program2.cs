﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Combinatorics.Collections;
using MathBrainTeaser2017;
using xFunc.Maths;
using xFunc.Maths.Expressions;
using xFunc.Maths.Expressions.Collections;
using xFunc.Maths.Tokens;

internal class CountingProblem
{
    //_patterns = {"nnonnoo", "nnonono", "nnnoono", "nnnonoo", "nnnnooo"};

    //private readonly string[]
    //_patterns = {"nunonnoo", "nnonono", "nnnoono", "nnnonoo", "nnnnooo", "nunuononou"};
    private readonly string[]
        _patterns = {"nunuonuonuou"};

    private readonly string[]
        _patterns2 = { "nunuounu", "nununuou" };

    private readonly string[]
        _patterns3 = {"nunuo"};

    private const string Ops = "+-/*^";
        private static readonly string[] UnaryOps = new[] {"abs", "!", "sqrt", "++"}; //++ is double fact
//    private static readonly string[] UnaryOps = new[] { "abs","sqrt",}; //++ is double fact

    public static void Main(string[] args)
    {
        //Processor _processor = new Processor();

        //var x = _processor.Parse("Sqrt(2! - Sqrt(1) + 7 - 0)");
        //var y = x.Execute();
        //        _processor.Parse("(sqrt(4) + 2) - 2");
    //    File f = new FileStyleUriParser();
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
        //var xx = _processor.Parse("3 - 7!");
        //var zz = xx.Execute();
        //List<IToken> tokens = new List<IToken>();
        //tokens.Add(new NumberToken(3));
        //tokens.Add(new FunctionToken(Functions.Absolute, 1));
        //tokens.Add(new NumberToken(7));
        //tokens.Add(new FunctionToken(Functions.Absolute, 1));
        //tokens.Add(new OperationToken(Operations.Factorial));
        //tokens.Add(new OperationToken(Operations.Subtraction));
        //var exp = (_processor.Parser.Parse(tokens));
        //var xxx = exp.Execute();
        //        var exp = _processor.Parse("3! + sqrt(2)");
        //var  exp = _processor.Parse("abs(-3)");
        // var s = exp.Execute();

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
        var results = new Dictionary<int, string>();
        for (int i = 1; i <= 100;i++)
            {
                results.Add(i, "");
            }
        //var opsPerms = new Permutations<char>(Ops.ToList(), GenerateOption.WithRepetition).ToList();
        //var unaryOpsPerms = new Permutations<string>(UnaryOps, GenerateOption.WithRepetition).ToList();
        //var opsPerms = new Variations<char>(Ops.ToList(), 3, GenerateOption.WithRepetition).ToList();
        // var unaryOpsPerms = new Variations<string>(UnaryOps, , GenerateOption.WithRepetition).ToList();
        //        var results = FindSolution(dPerms, opsPerms, unaryOpsPerms, _patterns);

        //var unarytypes = new Variations<string>(UnaryOps,2,GenerateOption.WithRepetition);
        //var unarytypes2 = new Variations<string>(UnaryOps, 4, GenerateOption.WithRepetition);


        var opsPerms3 = new Variations<char>(Ops.ToList(), 1, GenerateOption.WithRepetition).ToList();
        var unaryOpsPerms3 = new Variations<string>(UnaryOps, _patterns3.Max(x => x.Count(y => y == 'u')),
            GenerateOption.WithRepetition).ToList();
        FindSolution(results, dPerms3, opsPerms3, unaryOpsPerms3, _patterns3);

        var opsPerms2 = new Variations<char>(Ops.ToList(), _patterns2.Max(x => x.Count(y => y == 'o')), GenerateOption.WithRepetition).ToList();
        var unaryOpsPerms2 = new Variations<string>(UnaryOps, _patterns2.Max(x => x.Count(y => y == 'u')),
            GenerateOption.WithRepetition).ToList();
        FindSolution(results, dPerms2, opsPerms2, unaryOpsPerms2, _patterns2);

        var opsPerms = new Variations<char>(Ops.ToList(), _patterns.Max(x => x.Count(y => y == 'o')), GenerateOption.WithRepetition).ToList();
        var unaryOpsPerms = new Variations<string>(UnaryOps, _patterns.Max(x => x.Count(y => y == 'u')),
            GenerateOption.WithRepetition).ToList();
        FindSolution(results, dPerms, opsPerms, unaryOpsPerms, _patterns);


        foreach (int key in results.Keys)
        {
            File.AppendAllLines(@"D:\resultsFinal.txt", new[] {$"{key}: {results[key]}"});
        }

        //result = FindFirstSolution(dPerms2, opsPerms, _patterns2, i);
        //result = FindFirstSolution(dPerms3, opsPerms, _patterns3, i);
    }


    private  void FindSolution(Dictionary<int, string> results, List<IList<string>> dPerms, List<IList<char>> opsPerms,
        List<IList<string>> unaryOpsPerms,
        string[] postFixPatterns)
    {
        DateTime now = DateTime.Now;
        Console.WriteLine(now);

        long counter = 0;
        int max = postFixPatterns.Length * dPerms.Count * opsPerms.Count * unaryOpsPerms.Count;
        Parallel.ForEach(postFixPatterns,
            (pattern) =>
            {
                Parallel.ForEach(dPerms, (dig) =>
                {
                    if (results.Values.Count(x=>x.Length>0) > 90)
                    {
                        return;
                    }
                    Parallel.ForEach(opsPerms, (opr) =>
                    {
                        if (results.Values.Count(x => x.Length > 0) > 90)
                        {
                            return;
                        }
                        Parallel.ForEach(unaryOpsPerms, (unaryOp) =>
                        {
                            Interlocked.Increment(ref counter);
                            if (counter % 100 == 0)
                            {
                                Console.WriteLine($"{counter} of {max}");
                            }
                            List<IToken> tokens = new List<IToken>();
                            int i = 0, j = 0, k = 0;
                            var patternChars = pattern.ToCharArray();
                            for (int pc = 0; pc < patternChars.Length; pc++)
                            {
                                char c = patternChars[pc];
                                IToken token = null;
                                if (c == 'n')
                                {
                                    token = new NumberToken(Int32.Parse(dig[i++]));
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
                                            //                                token = new FunctionToken(Functions.Factorial);
                                            token = new OperationToken(Operations.Factorial);
                                            break;
                                        case "abs":
                                            token = new FunctionToken(Functions.Absolute, 1);
                                            break;
                                        case "sqrt":
                                            token = new FunctionToken(Functions.Sqrt, 1);
                                            break;
                                        case "++":
                                            token = new OperationToken(Operations.Increment);
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
                                //double result = Convert.ToDouble(rpnExp.Execute());
                                object resultObj = rpnExp.Execute();
                                double result;
                                if (resultObj is Complex)
                                {
                                    result = ((Complex) resultObj).Real;
                                }
                                else
                                {
                                    result = Convert.ToDouble(resultObj);
                                }
                                if (result % 1 != 0)
                                {
                                    return;
                                }
                                if (result > 100 || result < 1)
                                {
                                    return;
                                }
                                if (Double.IsInfinity(result) || Double.IsNaN(result) ||
                                    Double.IsNegativeInfinity(result))
                                {
                                    return;
                                }
                                var resultInt = Convert.ToInt32(result);
                                if (1 <= resultInt && resultInt <= 100)
                                {
                                    string exp = rpnExp.ToString().Replace("abs", "").Replace("++", "!!");
                                    Console.WriteLine($"{resultInt}: {exp}");
                                    File.AppendAllLines(@"D:\results.txt", new []{$"{resultInt}: {exp}"});
                                    results[resultInt] = exp;
                                }
                            }
                            catch (Exception e)
                            {
                                //                    Console.WriteLine("error: " + tokens.ToString());
                                return;
                            }
                        });
                    });
                });
            });
        Console.WriteLine("minutes: " + (DateTime.Now - now).TotalMinutes);
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