using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Combinatorics.Collections;
using MathBrainTeaser2017;
using xFunc.Maths;
using xFunc.Maths.Expressions;

internal class CountingProblem
{
    private readonly string[]
        _patterns = {"nnonnoo", "nnonono", "nnnoono", "nnnonoo", "nnnnooo"};

    private readonly string[]
        _patterns2 = {"nnono"};

    private readonly string[]
    _patterns3 = { "nno"};

    private const string Ops = "^+-/*";


    public static void Main(string[] args)
    {
        new CountingProblem().Go();
    }

    Processor _processor = new Processor();


    protected virtual void Go()
    {

        ConvertTo ct = new ConvertTo();
        ct.POSTstr = "231 7 +";
        var s = ct.PostToIn();


        var digits = new[] {"2", "0", "1", "7"};
        List<IList<string>> dPerms = new Permutations<string>(digits, GenerateOption.WithoutRepetition).ToList();
        List<IList<string>> dPerms2 = new List<IList<string>>();
        List<IList<string>> dPerms3 = new List<IList<string>>();
        //        var dPerms2 = new Permutations<string>(digits, GenerateOption.WithoutRepetition).ToList();

        foreach (var perm in dPerms)
        {
            dPerms2.Add(new[] {perm[0]+perm[1], perm[2],perm[3]});
        }
        foreach (var perm in dPerms)
        {
            dPerms3.Add(new[] { perm[0] + perm[1]+ perm[2], perm[3] });
        }
        var opsPerms = new Permutations<char>(Ops.ToList(), GenerateOption.WithRepetition).ToList();


        for (var i = 1; i <= 100; i++)
        {
            var result = "";
            result = FindFirstSolution(dPerms, opsPerms, _patterns, i);
            result = FindFirstSolution(dPerms2, opsPerms, _patterns2, i);
            result = FindFirstSolution(dPerms3, opsPerms, _patterns3, i);
            Console.WriteLine(i + ":> " + result);
        }
    }


    private string FindFirstSolution(List<IList<string>> dPerms, List<IList<char>> opsPerms, string[] postFixPatterns,
        int goal)
    {
        var sb = new StringBuilder(",");

        foreach (var pattern in postFixPatterns)
        {
            var patternChars = pattern.ToCharArray();

            foreach (var dig in dPerms)
            foreach (var opr in opsPerms)
            {
                int i = 0, j = 0;
                for (int pc = 0; pc < patternChars.Length; pc++)
//                foreach (var c in patternChars)
                {
                    char c = patternChars[pc];
                    if (c == 'n')
                    {
                        var var = dig[i++];
                        sb.Append(var);
                        if (patternChars[pc + 1] == 'n')
                        {
                            sb.Append(",");
                        }
                    }
                    else if (c == 'o')
                    {
                        sb.Append(opr[j++]);
                    }
//                    sb.Append(" ");
                }

                var candidate = sb.ToString();
                    sb.Clear();
                    var infix = "";
                try
                {
                    infix = PostfixToInfix(candidate);
                }
                   catch
                {
                     continue;
                }
                    IExpression ex1 = _processor.Parse(infix);
                    var expressions = Helpers.ConvertExpressionToCollection(ex1);
                //List<IExpression> expression = new List<IExpression>();
                //    foreach (var exp in expressions)
                //    {
                //        IExpression ex = new Sqrt(exp);
                //        expression.Add(ex);
                //    }

                //var isAllInts = Helpers.ConvertExpressionToCollection(ex1)
                //    .ToList().TrueForAll(x => ((double) x.Execute()) % 1 == 0);
                //if (!isAllInts)
                //{
                //    continue; //not allowed to have intermediate results that aren't integers
                //}

                int result = (Int32) ((Double) ex1.Execute());

                if ((Int32) result == goal)
                    return ex1.ToString();
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
