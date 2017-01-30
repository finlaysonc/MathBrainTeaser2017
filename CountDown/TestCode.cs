// Written by David Hancock mailto:code@davidhancock.net
// This code is published under The Code Project Open License.
// For full license terms see:
// http://www.codeproject.com/info/cpol10.aspx

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CountDown
{
    /// <summary>
    /// Unfortunately Visual Studio Express doesn't have a unit test framework built in 
    /// and to keep the project as simple as possible I don't want to rely on any third
    /// party libraries (such as NUnit http://www.nunit.org). This test code uses 
    /// System.Diagnostics routines and will be removed from release builds automatically 
    /// but obviously cannot be run as part of the build process. 
    /// </summary>
    public static class TestCode
    {
        #region Run Tests

        public static bool Run()
        {
            try
            {
                // test cases use comparers so test them first
                TestComparers();

                // combinations tests
                TestCombinationsEdgeCases();
                TestCombinationsUniqueSource();
                TestCombinationsDuplicateSource();
                TestCombinationsCustomComparer();
                TestCombinationsGenerics();
                TestDuplicateCombinations(new int[] { 1, 2, 3, 4, 5, 6 }, 0); // no dupliucates
                TestDuplicateCombinations(new int[] { 1, 1, 3, 4, 5, 6 }, 1); // 1 duplicate
                TestDuplicateCombinations(new int[] { 1, 1, 3, 3, 5, 6 }, 2); // 2 duplicates
                
                // permutations tests
                TestPermutationsEdgeCases();
                TestPermutationsUniqueSource();
                TestPermutationsDuplicateSource();
                TestPermutationsCustomComparer();
                TestPermutationsGenerics();
                TestDuplicatePermutations(new int[] { 1, 2, 3, 4, 5, 6 }, 0); // no dupliucates
                TestDuplicatePermutations(new int[] { 1, 1, 3, 4, 5, 6 }, 1); // 1 duplicate
                TestDuplicatePermutations(new int[] { 1, 1, 3, 3, 5, 6 }, 2); // 2 duplicates
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.ToString());  // unhandled exception, fail
            }
           
            return true;
        }


        #endregion

        #region Test Permutations

        /// <summary>
        /// This only checks that the correct number of permutations are generated
        /// although thats a pretty good indicator of success...
        /// </summary>
        /// <param name="source"></param>
        /// <param name="numDuplicates"></param>
        private static void TestDuplicatePermutations(int[] source, int numDuplicates)
        {
            Permutations<int> p1 = new Permutations<int>(source);
            List<List<int>> p1list = new List<List<int>>(p1);

            switch (numDuplicates)
            {
                case 0:
                    Debug.Assert(p1list.Count == Factorial(source.Length));
                    break;

                case 1:
                    Debug.Assert(p1list.Count == Factorial(source.Length) / Factorial(2));
                    break;

                case 2:
                    Debug.Assert(p1list.Count == Factorial(source.Length) / (Factorial(2) * Factorial(2)));
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }
        }



        private static void TestPermutationsUniqueSource()
        {
            int[] source = new int[] { 1, 3, 2 };

            List<List<int>> output = new List<List<int>> {  new List<int> { 1, 2, 3 },
                                                            new List<int> { 1, 3, 2 },
                                                            new List<int> { 2, 1, 3 },
                                                            new List<int> { 2, 3, 1 },
                                                            new List<int> { 3, 1, 2 },
                                                            new List<int> { 3, 2, 1 }};

            Permutations<int> p1 = new Permutations<int>(source);
            List<List<int>> p1list = new List<List<int>>(p1);

            Debug.Assert(Compare<int>(p1list, output) == 0);
        }


        private static void TestPermutationsDuplicateSource()
        {
            int[] source = new int[] { 1, 3, 1 };

            List<List<int>> output = new List<List<int>> {  new List<int> { 1, 1, 3 },
                                                            new List<int> { 1, 3, 1 },
                                                            new List<int> { 3, 1, 1 }};

            Permutations<int> p1 = new Permutations<int>(source);
            List<List<int>> p1list = new List<List<int>>(p1);

            Debug.Assert(Compare<int>(p1list, output) == 0);
        }



 


        private static void TestPermutationsCustomComparer()
        {
            int[] source = new int[] { 1, 3, 2 };

            List<List<int>> output = new List<List<int>> {  new List<int> { 3, 2, 1 },
                                                            new List<int> { 3, 1, 2 },
                                                            new List<int> { 2, 3, 1 },
                                                            new List<int> { 2, 1, 3 },
                                                            new List<int> { 1, 3, 2 },
                                                            new List<int> { 1, 2, 3 }};
            
            // supply an inverse comparer to generate permutations in 
            // decreasing rather than increasing lexicographic order
            Permutations<int> p1 = new Permutations<int>(source, new ReverseComparerClass<int>());
            List<List<int>> p1list = new List<List<int>>(p1);

            Debug.Assert(Compare<int>(p1list, output) == 0);
        }




        private static void TestPermutationsGenerics()
        {
            string[] source = new string[] { "A", "C", "B" };

            List<List<string>> output = new List<List<string>> {new List<string> { "A", "B", "C" },
                                                                new List<string> { "A", "C", "B" },
                                                                new List<string> { "B", "A", "C" },
                                                                new List<string> { "B", "C", "A" },
                                                                new List<string> { "C", "A", "B" },
                                                                new List<string> { "C", "B", "A" }};
         
            Permutations<string> p1 = new Permutations<string>(source);
            List<List<string>> p1list = new List<List<string>>(p1);

            Debug.Assert(Compare<string>(p1list, output) == 0);
        }



        private static void TestPermutationsEdgeCases()
        {
            // empty source
            int[] source = new int[0];
            // empty results
            List<List<int>> output = new List<List<int>> { new List<int>() };

            Permutations<int> p1 = new Permutations<int>(new int[0]);
            List<List<int>> p1list = new List<List<int>>(p1);

            Debug.Assert(Compare<int>(p1list, output) == 0);

            // null source
            try
            {
                Permutations<int> p2 = new Permutations<int>(null);
                List<List<int>> p2list = new List<List<int>>(p2);
            }
            catch (ArgumentNullException) // expected
            {
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }


            // input type T doesn't implement IComparable<T> and no custom comparer supplied
            object[] input = new object[] { new object(), new object() };

            try
            {
                Permutations<object> p3 = new Permutations<object>(input);
                List<List<object>> p3list = new List<List<object>>(p3);
            }
            catch (InvalidOperationException) // expected
            {
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }

        #endregion

        #region Test Combinations

        /// <summary>
        /// Builds two combinations, one without duplicates and one including duplicates. 
        /// It then removes the duplicates from the latter and compares the two which
        /// at that stage should be equal.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="numDuplicates"></param>
        private static void TestDuplicateCombinations(int[] source, int numDuplicates)
        {
            int n = source.Length ;

            for (int k = 1 ; k <= n ; k++)
            {
                Combinations<int> c1 = new Combinations<int>(source, k);
                Combinations<int> c2 = new Combinations<int>(source, k, true);

                List<List<int>> c1list = new List<List<int>>(c1);
                List<List<int>> c2list = new List<List<int>>(c2);

                Debug.Assert(c2list.Count == CombinationSize(n, k));

                if ((numDuplicates > 0) && (c1list.Count > 1))  // one entry so cannot have duplicates
                {
                    Debug.Assert(Compare<int>(c1list, c2list) != 0);
                    RemoveDuplicates(c2list); // c1list should now equal c2list
                }

                Debug.Assert(Compare<int>(c1list, c2list) == 0);
            }
        }



        private static void TestCombinationsEdgeCases()
        {
            // empty source
            int[] source = new int[0];
            // empty results
            List<List<int>> output = new List<List<int>> { new List<int>() };

            Combinations<int> c1 = new Combinations<int>(source, 0);
            List<List<int>> c1list = new List<List<int>>(c1);

            Debug.Assert(Compare<int>(c1list, output) == 0);

            // null source
            try
            {
                Combinations<int> c2 = new Combinations<int>(null, 0);
                List<List<int>> c2list = new List<List<int>>(c2);
            }
            catch (ArgumentNullException) // expected
            {
            }
            catch(Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }

            // k > n
            try
            {
                Combinations<int> c3 = new Combinations<int>(source, source.Length + 1);
                List<List<int>> c3list = new List<List<int>>(c3);
            }
            catch (ArgumentOutOfRangeException) // expected
            {
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }

            // input type T doesn't implement IComparable<T> and no custom comparer supplied
            object[] input = new object[] { new object(), new object() };
            
            try
            {
                Combinations<object> c4 = new Combinations<object>(input, input.Length);
                List<List<object>> c4list = new List<List<object>>(c4);
            }
            catch (InvalidOperationException) // expected
            {
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }


        private static void TestCombinationsCustomComparer()
        {
            int[] source = new int[] { 1, 3, 2, 4 };

            List<List<int>> output = new List<List<int>> {  new List<int> { 4, 3 },                                          
                                                            new List<int> { 4, 2 },
                                                            new List<int> { 4, 1 },
                                                            new List<int> { 3, 2 },
                                                            new List<int> { 3, 1 },
                                                            new List<int> { 2, 1 }};

            // supply an inverse comparer to generate combinations in 
            // decreasing rather than increasing lexicographic order
            Combinations<int> c1 = new Combinations<int>(source, 2, false, new ReverseComparerClass<int>());
            List<List<int>> c1list = new List<List<int>>(c1);

            Debug.Assert(Compare<int>(c1list, output) == 0);
        }




        private static void TestCombinationsUniqueSource()
        {
            int[] source = new int[] { 1, 3, 2, 4 };

            List<List<int>> output = new List<List<int>> {  new List<int> { 1, 2 },
                                                            new List<int> { 1, 3 },
                                                            new List<int> { 1, 4 },
                                                            new List<int> { 2, 3 },
                                                            new List<int> { 2, 4 },
                                                            new List<int> { 3, 4 }};
                                                            
            Combinations<int> c1 = new Combinations<int>(source, 2);
            List<List<int>> c1list = new List<List<int>>(c1);

            Debug.Assert(Compare<int>(c1list, output) == 0);
        }





        private static void TestCombinationsDuplicateSource()
        {
            int[] source = new int[] { 1, 23, 1, 14, 1 };

            List<List<int>> output = new List<List<int>> {  new List<int> { 1, 1 },
                                                            new List<int> { 1, 14 },
                                                            new List<int> { 1, 23 },
                                                            new List<int> { 14, 23 }};

            Combinations<int> c1 = new Combinations<int>(source, 2);
            List<List<int>> c1list = new List<List<int>>(c1);

            Debug.Assert(Compare<int>(c1list, output) == 0);
        }



        private static void TestCombinationsGenerics()
        {
            string[] source = new string[] { "A", "C", "B", "D" };

            List<List<string>> output = new List<List<string>> {    new List<string> { "A", "B" },
                                                                    new List<string> { "A", "C" },
                                                                    new List<string> { "A", "D" },
                                                                    new List<string> { "B", "C" },
                                                                    new List<string> { "B", "D" },
                                                                    new List<string> { "C", "D" }};

            Combinations<string> c1 = new Combinations<string>(source, 2);
            List<List<string>> c1list = new List<List<string>>(c1);

            Debug.Assert(Compare<string>(c1list, output) == 0);
        }




        #endregion

        #region Test Comparers


        private static void TestComparers()
        {
            List<int> l0 = null;
            List<int> l1 = new List<int> { 1, 2, 3 };
            List<int> l2 = new List<int> { 1, 2, 3 };
            List<int> l3 = new List<int> { 1, 2, 4 };
            List<int> l4 = new List<int> { 1, 2 };

            Debug.Assert(Compare<int>(l0, l0) == 0);    // nulls
            Debug.Assert(Compare<int>(l1, l0) > 0);
            Debug.Assert(Compare<int>(l0, l1) < 0);

            Debug.Assert(Compare<int>(l1, l2) == 0);    // contents
            Debug.Assert(Compare<int>(l1, l3) < 0);
            Debug.Assert(Compare<int>(l3, l1) > 0);

            Debug.Assert(Compare<int>(l1, l1) == 0);    // reference 

            Debug.Assert(Compare<int>(l1, l4) > 0);     // lengths
            Debug.Assert(Compare<int>(l4, l1) < 0);


            List<List<int>> ll0 = null;
            List<List<int>> ll1 = new List<List<int>> { l1, l1 };
            List<List<int>> ll2 = new List<List<int>> { l2, l2 };
            List<List<int>> ll3 = new List<List<int>> { l1, l3 };
            List<List<int>> ll4 = new List<List<int>> { l1 };


            Debug.Assert(Compare<int>(ll0, ll0) == 0);  // nulls
            Debug.Assert(Compare<int>(ll1, ll0) > 0);
            Debug.Assert(Compare<int>(ll0, ll1) < 0);

            Debug.Assert(Compare<int>(ll1, ll2) == 0);  // contents
            Debug.Assert(Compare<int>(ll1, ll3) < 0);
            Debug.Assert(Compare<int>(ll3, ll1) > 0);

            Debug.Assert(Compare<int>(ll1, ll1) == 0);    // reference 

            Debug.Assert(Compare<int>(ll1, ll4) > 0);     // lengths
            Debug.Assert(Compare<int>(ll4, ll1) < 0);
        }

        #endregion

        #region Utility Code


        /// <summary>
        /// Calculates the combination size using the "n choose k" equation
        /// (the binomial coefficient)
        /// </summary>
        /// <param name="n"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        private static int CombinationSize(int n, int k)
        {
            return Factorial(n) / (Factorial(k) * Factorial(n - k));
        }



        /// <summary>
        /// Factorials without recursion or multiplication
        /// Not pretty but quick.
        /// </summary>
        /// <param name="n"></param>
        /// <returns>n!</returns>
        private static int Factorial(int n)
        {
            switch (n)
            {
                case 0: return 1;
                case 1: return 1;
                case 2: return 2;
                case 3: return 6;
                case 4: return 24;
                case 5: return 120;
                case 6: return 720;
                case 7: return 5040;
                case 8: return 40320;
                case 9: return 362880;
                case 10: return 3628800;
                case 11: return 39916800;
                case 12: return 479001600;
                default:
                    // 13! overflows an Int32
                    throw new ArgumentOutOfRangeException();
            }
        }

        
        /// <summary>
        /// Not particularly efficient but ok for test code due to its simplicity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        private static void RemoveDuplicates<T>(List<List<T>> list) where T : IComparable<T>
        {
            list.Sort(new ListComparerClass<T>());

            int current = 0;
            int next = 1;

            while (next < list.Count)
            {
                if (Compare<T>(list[current], list[next]) == 0)
                {
                    list.RemoveAt(next); 
                }
                else
                {
                    ++current;
                    ++next;
                }
            }
        }


        #endregion

        #region Comparer Code

        private class ListComparerClass<T> : IComparer<List<T>> where T : IComparable<T>
        {
            public int Compare(List<T> left, List<T> right) 
            {
                return Compare<T>(left, right) ;
            }
        }


        private class ReverseComparerClass<T> : IComparer<T> where T : IComparable<T>
        {
            private IComparer<T> comparer;

            public ReverseComparerClass()
            {
                comparer = Comparer<T>.Default;
            }

            public int Compare(T left, T right)
            {
                // reverse the normal comparison order
                return comparer.Compare(right, left);
            }
        }


        /// <summary>
        /// Simplistic method to compare two ILists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int Compare<T>(IList<T> left, IList<T> right) where T : IComparable<T>
        {
            if (Object.ReferenceEquals(left, right))
                return 0;

            if (left == null)
                return -1;

            if (right == null)
                return 1;

            if (left.Count != right.Count)
                return (left.Count < right.Count) ? -1 : 1;

            IComparer<T> comparer = Comparer<T>.Default;

            for (int index = 0; index < left.Count; index++)
            {
                int cr = comparer.Compare(left[index], right[index]);

                if (cr != 0)
                    return cr;
            }

            return 0;
        }



        private static int Compare<T>(List<List<T>> left, List<List<T>> right) where T : IComparable<T>
        {
            if (Object.ReferenceEquals(left, right))
                return 0;

            if (left == null)
                return -1;

            if (right == null)
                return 1;

            if (left.Count != right.Count)
                return (left.Count < right.Count) ? -1 : 1;

            for (int index = 0; index < left.Count; index++)
            {
                int cr = Compare<T>(left[index], right[index]);

                if (cr != 0)
                    return cr;
            }

            return 0;
        }

        #endregion
    }
}
