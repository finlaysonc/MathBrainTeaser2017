using System;
using System.Collections.Generic;
using System.Linq;

namespace MathBrainTeaser2017
{


    http://www.geeksforgeeks.org/all-ways-to-add-parenthesis-for-evaluation/
    public class GlobalMembers2
    {
        // C++ program to output all possible values of
        // an expression by parenthesizing it.

        // method checks, character is operator or not
        private bool isOperator(char op)
        {
            return (op == '+' || op == '-' || op == '*');
        }

        // Utility recursive method to get all possible
        // result of input string
        private List<int> possibleResultUtil(string input)
        {
            // If already calculated, then return from memo
            if (memo.ContainsKey(input))
            {
                return memo[input];
            }

            List<int> res = new List<int>();
            for (int i = 0; i < input.Length; i++)
            {
                if (isOperator(input[i]))
                {
                    // If character is operator then split and
                    // calculate recursively
                    //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
                    //ORIGINAL LINE: ClassicVector<int> resPre = possibleResultUtil(input.substr(0, i), memo);
                    List<int> resPre = possibleResultUtil(input.Substring(0, i));
                    //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
                    //ORIGINAL LINE: ClassicVector<int> resSuf = possibleResultUtil(input.substr(i + 1), memo);
                    List<int> resSuf = possibleResultUtil(input.Substring(i + 1));

                    // Combine all possible combination
                    for (int j = 0; j < resPre.Count; j++)
                    {
                        for (int k = 0; k < resSuf.Count; k++)
                        {
                            if (input[i] == '+')
                            {
                                res.Add(resPre[j] + resSuf[k]);
                            }
                            else if (input[i] == '-')
                            {
                                res.Add(resPre[j] - resSuf[k]);
                            }
                            else if (input[i] == '*')
                            {
                                res.Add(resPre[j] * resSuf[k]);
                            }
                        }
                    }
                }
            }

            // if input contains only number then save that 
            // into res vector
            if (res.Count == 0)
            {
                res.Add(Convert.ToInt32(input));
            }

            // Store in memo so that input string is not 
            // processed repeatedly
            memo.Add(input, res);
            return res;
        }
        public SortedDictionary<string, List<int>> memo = new SortedDictionary<string, List<int>>();

        // method to return all possible output 
        // from input expression
        private List<int> possibleResult(string input)
        {
            memo = new SortedDictionary<string, List<int>>();
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
            //ORIGINAL LINE: return possibleResultUtil(input, memo);
            return possibleResultUtil(input);
        }

        // Driver code to test above methods
        public static void Main()
        {
            GlobalMembers2 g = new GlobalMembers2();
            string input = "5*4-3*2";
            List<int> res = g.possibleResult(input);

            for (int i = 0; i < res.Count; i++)
            {
                Console.WriteLine(g.memo.Keys.ToArray()[i]);
                Console.Write(res[i]);
                Console.Write(" ");
            }
        }

    }


    public static class GlobalMembers
    {
        // C++ program to find all possible expression which
        // evaluate to target

        // Utility recursive method to generate all possible
        // expressions
        public static void getExprUtil(List<string> res, string curExp, string input, int target, int pos, int curVal, int last)
        {
            // true if whole input is processed with some
            // operators
            if (pos == input.Length)
            {
                // if current value is equal to target
                //then only add to final solution
                // if question is : all possible o/p then just
                //push_back without condition
                if (curVal == target)
                {
                    res.Add(curExp);
                }
                return;
            }

            // loop to put operator at all positions
            for (int i = pos; i < input.Length; i++)
            {
                // ignoring case which start with 0 as they
                // are useless for evaluation
                if (i != pos && input[pos] == '0')
                {
                    break;
                }

                // take part of input from pos to i
                string part = input.Substring(pos, i + 1 - pos);

                // take numeric value of part
                int cur = Convert.ToInt32(part);

                // if pos is 0 then just send numeric value
                // for next recurion
                if (pos == 0)
                {
                    getExprUtil(res, curExp + part, input, target, i + 1, cur, cur);
                }


                // try all given binary operator for evaluation
                else
                {
                    getExprUtil(res, curExp + "+" + part, input, target, i + 1, curVal + cur, cur);
                    getExprUtil(res, curExp + "-" + part, input, target, i + 1, curVal - cur, -cur);
                    getExprUtil(res, curExp + "*" + part, input, target, i + 1, curVal * cur, curVal * cur); //, last * cur);
                    if (cur != 0)
                    {
                        getExprUtil(res, curExp + "/" + part, input, target, i + 1, curVal - last + last * last / cur, last / cur);
                    }
                }
            }
        }

        // Below method returns all possible expression
        // evaluating to target
        public static List<string> getExprs(string input, int target)
        {
            List<string> res = new List<string>();
            getExprUtil(res, "", input, target, 0, 0, 0);
            return res;
        }

        // method to print result
        public static void printResult(List<string> res)
        {
            for (int i = 0; i < res.Count; i++)
            {
                Console.Write(res[i]);
                Console.Write(" ");
            }
            Console.Write("\n");
        }

        // Driver code to test above methods
      public  static void Main2()
      {
          Console.WriteLine("hello");

            string input = "2017";
          for (int i = 0; i < 100; i++)
          {
              List<string> res = getExprs(input, i);
              //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
              //ORIGINAL LINE: printResult(res);

              if (res.Count > 0)
              {
                  Console.Write($"{i}: ");
                  printResult(new List<string>(res));
              }
          }

          //input = "2017";
          //  target = 7;
          //  res = new List<string>(getExprs(input, target));
          //  //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
          //  //ORIGINAL LINE: printResult(res);
          //  printResult(new List<string>(res));

        }
    }
}
