using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Fare;
internal class Rextester
{

    internal readonly string[] 
patterns = new string[] { "nnonnoo", "nnonono", "nnnoono", "nnnonoo", "nnnnooo"};

//    internal readonly string[] patterns = new string[] { "nnononuo" };

    public static readonly string ops = "+-*/";

    internal string solution;
    internal IList<int> digits;

    public class Expression
    {
        public String op, ex;
        public int prec = 3;

        public Expression(String e)
        {
            ex = e;
        }

        public Expression(String e1, String e2, String o)
        {
            ex = $"{e1} {o} {e2}";
//            ex = String.Format("%s %s %s", e1, o, e2);
            op = o;
            prec = Rextester.ops.IndexOf(o) / 2;
        }
    }



    public static void Main(string[] args)
    {

        //string pattern = @"^(?=.* 2)(?=.* 0)(?=.* 1)(?=.* 7)\d\s\d\s[-*+]\s\d\s[-*+]\s\d$";
        //var r = new Regex(@"^(?=.*2)(?=.*0)(?=.*1)(?=.*7)\d\s\d\s[-*+/]\s\d\s[-*+/]\s\d\s[-*+/]$", RegexOptions.Singleline);
        //r.IsMatch("2 0 - 1 + 7 /");

        //string regex = "[ab]{4,6}c";
        //string regex2 = @"^(?=.*2)(?=.*0)(?=.*1)(?=.*7)\d\s\d\s[-\/*+]\s\d\s[-\/*+]\s\d$";
        //Xeger generator = new Xeger(@"^\d\s\d\s[-*+/]\s\d\s[-*+/]\s\d\s[-*+/]$");
        //string s = generator.Generate();
        //var strings = Enumerable.Range(1, 3).Select(i => generator.Generate()).ToArray();
        //Array.ForEach(strings, str => Console.WriteLine(str));

        //Rex
        //var settings = new Rex.RexSettings(pattern) { k = 1 };
        //var result = Enumerable.Range(1, 3).Select(i => Rex.RexEngine.GenerateMembers(settings).Single()).ToArray();
        (new Rextester()).play();
    }

    internal int goal = 1;

    internal virtual void play()
    {
        //        digits = getSolvableDigits();


        for (int i = 1; i <= 100; i++)
        {
            goal = i;
//            Console.Write("Make {0:D} using these digits: \n", i);
            Console.Write(i + ":> ");
            digits = SolvableDigits;
            if (digits != null)
            Console.WriteLine(solution);

            /*String line = "s";

			if (line.equalsIgnoreCase("s")) {
			    System.out.println(solution);
			    digits = getSolvableDigits();
			    continue;
			}*/

        }
    }
    internal virtual bool evaluate(char[] line)
    {
        Stack<int> s = new Stack<int>();
        try
        {
            foreach (char c in line)
            {
                if ('0' <= c && c <= '9')
                {
                    s.Push((int)c - '0');
                }
                else if (ops.IndexOf(c) >= 0)
                {
                    s.Push(applyOperator(s.Pop(), s.Pop(), c));
                }
                //else
                //{
                //    s.Push(applyUnaryOperator(s.Pop(), c));
                //}
            }
        }
        catch (Exception)
        {
            throw new Exception("Invalid entry.");
        }
        return (Math.Abs(goal - s.Peek()) < 0.001F);
    }

    internal virtual int applyUnaryOperator(int a, char c)
    {
        if (c == '!')
        {
            return Fact(a);
        }
        throw new Exception("hmmm");

    }

    public static int Fact(int n)
    {
        int result = 1;

        for (int i = n; i > 0; i--)
            result *= i;

        return result;
    }


    internal virtual int applyOperator(int a, int b, char c)
    {
        switch (c)
        {
            case '+':
                return a + b;
            case '-':
                return b - a;
            case '*':
                return a * b;
            case '/':
                return b / a;
            case '^':
                return (int)Math.Pow(a, b);
            default:
                return -10000;
        }
    }
    
    internal virtual IList<int> SolvableDigits
    {
        get
        {
            //        List<Integer> result;
            var result = new List<int>(4) {2, 0, 1, 7};
            if (!isSolvable(result))
            {
                Console.Write("{0:D} not solvable\n", goal);
                return null;

            }
            return result;
            //        do {
            //          result = randomDigits();
            //    } while (!isSolvable(result));
            //  return result;
        }
    }

    internal virtual bool isSolvable(List<int> digits)
    {
//        ISet<IList<int>> dPerms = new HashSet<IList<int>> (4 * 3 * 2);
ISet<List<int>> dPerms = new HashSet<List<int>>();

     permute(digits, dPerms, 0);

        int total = 4 * 4 * 4;
        IList<IList<int>> oPerms = new List<IList<int>>(total);
        //        permuteOperators(oPerms, 4, total);
        permuteOperators(oPerms, 4, total);

        StringBuilder sb = new StringBuilder(4 + 3);

        foreach (string pattern in patterns)
        {
            char[] patternChars = pattern.ToCharArray();

            foreach (IList<int> dig in dPerms)
            {
                foreach (IList<int> opr in oPerms)
                {

                    int i = 0, j = 0;
                    foreach (char c in patternChars)
                    {
                        if (c == 'n')
                        {
                            sb.Append(dig[i++]);
                        }
                        else if (c=='o')
                        {
                            sb.Append(ops[opr[j++]]);
                        }
                        else if (c == 'u')
                        {
                            sb.Append('!');
                        }
                    }

                    string candidate = sb.ToString();
                    try
                    {
                        if (evaluate(candidate.ToCharArray()))
                        {
                            solution = postfixToInfix(candidate);
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                    }
                    sb.Length = 0;
                }
            }
        }
        return false;
    }

    internal virtual string postfixToInfix(string postfix)
    {
        //JAVA TO C# CONVERTER TODO TASK: Local classes are not converted by Java to C# Converter:

    Stack<Expression> expr = new Stack<Expression>();

        foreach (char c in postfix.ToCharArray())
        {
            int idx = ops.IndexOf(c);
            if (idx != -1)
            {

                Expression r = expr.Pop();
                Expression l = expr.Pop();

                int opPrec = idx / 2;

                if (l.prec < opPrec)
                {
                    l.ex = '(' + l.ex + ')';
                }

                if (r.prec <= opPrec)
                {
                    r.ex = '(' + r.ex + ')';
                }

                expr.Push(new Expression(l.ex, r.ex, "" + c));
            }
            else
            {
                expr.Push(new Expression("" + c));
            }
        }
        return expr.Peek().ex;
    }
    static void Swap<T>(List<T> list, int index1, int index2)
    {
        T temp = list[index1];
        list[index1] = list[index2];
        list[index2] = temp;
    }

    internal virtual void permute(List<int> lst, ISet<List<int>> res, int k)
    {
        for (int i = k; i < lst.Count; i++)
        {
            Swap(lst, i, k);
//            Collections.swap(lst, i, k);
            permute(lst, res, k + 1);
            Swap(lst, k,i);
//            Collections.swap(lst, k, i);
        }
        if (k == lst.Count)
        {
            res.Add(new List<int>(lst));
        }
    }

    internal virtual void permuteOperators(IList<IList<int>> res, int n, int total)
    {
        for (int i = 0, npow = n * n; i < total; i++)
        {
            res.Add(new List<int>() {(i / npow), (i % npow) / n, i % n});
        }
    }
}