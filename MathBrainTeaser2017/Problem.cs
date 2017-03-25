using System;
using System.Collections.Generic;
using System.Text;

namespace Countdown2017
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

        public static long ExpectedSolutions
        {
            get { return MaxTarget - MinTarget + 1; }
        }

        public DateTime StartTime { get; private set; }

        public DateTime EndTime
        {
            get { return endTime ?? DateTime.Now; }
        }

        public double ExecutionTime
        {
            get { return Math.Round((EndTime - StartTime).TotalSeconds, 3); }
        }


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

        public IDictionary<long, string> Run()
        {
            StartTime = DateTime.Now;
            IDictionary<long, string> result = Solve();
            endTime = DateTime.Now;
            return result;
        }

        protected abstract IDictionary<long, string> Solve();

        private static void Main(string[] args)
        {
            //List<int> list = new List<int>() { 2, 0, 1, 7 };
            //IReadOnlyList<int> listR = new ReadOnlyCollection<int>(list);
            //var xx = listR.GetVariations();
            //foreach (var x in xx)
            //{
            //    foreach (int i in x)
            //    {
            //        Console.Write(i + " ");
            //    }
            //    Console.WriteLine();
            //}
            //Console.WriteLine("-----");
            //var xxx = listR.GetPowerSet();
            //foreach (var x in xxx)
            //{
            //    foreach (int i in x)
            //    {
            //        Console.Write(i + " ");
            //    }
            //    Console.WriteLine();
            //}
            //Console.WriteLine("-----");
            //var yyy = listR.GetPermutations();
            //foreach (var x in yyy)
            //{
            //    foreach (int i in x)
            //    {
            //        Console.Write(i + " ");
            //    }
            //    Console.WriteLine();
            //}

            //return;
            ASTSolution problem = new ASTSolution();

            IDictionary<long, string> result = problem.Run();

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