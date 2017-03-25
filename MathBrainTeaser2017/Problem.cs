using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SolverFoundation.Common;
using ResultsDict= System.Collections.Generic.IDictionary<Microsoft.SolverFoundation.Common.BigInteger, string>;
namespace MathBrainTeaser2017
{

    public abstract class Problem
    {
        //1. Put whatever you want for the digits to use
        // e.g. "991122", "1982", etc. 
        public const string Digits = "2017";


        //the range of #'s to solve
        public static readonly long MinTarget = 1, MaxTarget = 100;

        /// <summary>
        ///     Defines how many of what digits are required in the expression
        ///     e.g., 2017
        ///     digitCount =
        ///     0 -> 1
        ///     1 -> 1
        ///     2 -> 1
        ///     3 -> 0
        ///     4 -> 0
        ///     5 -> 0
        ///     6 -> 0
        ///     7 -> 1
        ///     8 -> 0
        ///     9 -> 0
        /// </summary>
        private static readonly char[] digitCount;

        private DateTime? endTime;


        /// <summary>
        ///     Sets up digitCount based on Digits
        /// </summary>
        static Problem()
        {
            char[] digits = new char[10];
            for (int i = 0; i < Digits.Length; i++)
            {
                digits[Digits[i] - '0']++;
            }

            digitCount = digits;
        }

        public static long ExpectedSolutions => MaxTarget - MinTarget + 1;

        public DateTime StartTime { get; private set; }

        public DateTime EndTime => endTime ?? DateTime.Now;

        public double ExecutionTime => Math.Round((EndTime - StartTime).TotalSeconds, 3);


        /// <summary>
        ///     Ensure the passed digits is within bounds.  E.g.
        ///     given Digits = 2017;
        ///     example Valid digits = "2", "2017"
        ///     example invalid digits = "22", "567"
        /// </summary>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static bool Validate(string digits)
        {
            int[] count = new int[10];

            for (int i = 0; i < digits.Length; i++)
            {
                int d = digits[i] - '0';
                if (++count[d] > digitCount[d])
                {
                    return false;
                }
            }
            return true;
        }

        public ResultsDict Run()
        {
            StartTime = DateTime.Now;
            ResultsDict result = Solve();
            endTime = DateTime.Now;
            return result;
        }

        protected abstract ResultsDict Solve();

        private static void Main(string[] args)
        {
            ASTSolution problem = new ASTSolution();

            ResultsDict result = problem.Run();

            //Validate results

            int s_count = 0;
            StringBuilder missing = new StringBuilder();
            int missingCount = 0;
            for (long i = MinTarget; i <= MaxTarget; i++)
            {
                string sol;
                if (!result.TryGetValue(i, out sol))
                {
                    missing.Append(' ').Append(i);
                    missingCount++;
                }
                else
                {
                    s_count++;
                    Console.WriteLine("{0} <- {1}", i, sol);
                }
            }

            if (missingCount > 0)
            {
                Console.WriteLine("---------------------------------------------------");
                Console.WriteLine("Missing {0} solutions:{1}", missingCount, missing);
                Console.WriteLine("---------------------------------------------------");
            }
            Console.WriteLine("Found {0} solutions in {1} seconds", s_count, problem.ExecutionTime);
        }
    }
}