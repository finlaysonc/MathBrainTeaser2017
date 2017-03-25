using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Countdown2017
{
    public abstract class Problem
    {
        public const string Digits = "2017";
        public static readonly long MinTarget = 1, MaxTarget = 100;

        static readonly char[] digitCount;
        static Problem()
        {
            var digits = new char[10];
            for(int i = 0; i < Digits.Length; i++)
            {
                digits[Digits[i] - '0']++;
            }

            digitCount = digits;
        }

        public static bool Validate(string digits)
        {
            int[] count = new int[10];

            for (int i = 0; i< digits.Length; i++)
            {
                var d = digits[i] - '0';
                if (++count[d] > digitCount[d])
                    return false;
            }
            return true;
        }

        public static long ExpectedSolutions { get { return MaxTarget - MinTarget + 1; } }

        public DateTime StartTime { get; private set; }
        DateTime? endTime;
        public DateTime EndTime { get { return endTime ?? DateTime.Now; } }
        public double ExecutionTime { get { return Math.Round((EndTime - StartTime).TotalSeconds, 3); } }

        public IDictionary<long, string> Run()
        {
            StartTime = DateTime.Now;
            var result = Solve();
            endTime = DateTime.Now;
            return result;
        }

        protected abstract IDictionary<long, string> Solve();

        static void Main(string[] args)
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
            var problem = new ASTSolution();

            var result = problem.Run();

            //Validate results

            int s_count = 0;
            StringBuilder missing = new StringBuilder();
            int missingCount = 0;
            for (var i = MinTarget; i <= MaxTarget; i++)
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
                Console.WriteLine("Missing {0} solutions:{1}", missingCount, missing.ToString());
                Console.WriteLine("---------------------------------------------------");
            }
            Console.WriteLine("Found {0} solutions in {1} seconds", s_count, problem.ExecutionTime);
        }
    }
}
