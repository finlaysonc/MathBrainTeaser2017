// Written by David Hancock mailto:code@davidhancock.net
// This code is published under The Code Project Open License.
// For full license terms see:
// http://www.codeproject.com/info/cpol10.aspx

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace CountDown
{
    /// <summary>
    /// Class that generates combinations of k items in the supplied collection 
    /// without duplicates.
    /// 
    /// If the supplied collection itself contains duplicate entries this implementation
    /// will by default not produce duplicate combinations e.g.
    /// 
    ///     For a source of {3, 1, 1, 1} and k = 2 the code generates 
    ///     two combinations:
    ///  
    ///     {1, 1} 
    ///     {1, 3}
    ///        
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Combinations<T> : IEnumerable<List<T>>
    {
        /// <summary>
        /// copy of the source 
        /// </summary>
        private T[] input;

        /// <summary>
        /// the "n choose k" variables
        /// </summary>
        private int n;
        private int k;

        /// <summary>
        /// if the source contains duplicate objects
        /// </summary>
        private bool noDuplicates = true;

         /// <summary>
        /// The comparer to use
        /// </summary>
        private IComparer<T> comparer;                     



        /// <summary>
        /// Hide the invalid default constructor
        /// </summary>
        protected Combinations() 
        { 
        }


        /// <summary>
        /// Constructor used to generate combinations of the supplied collection.
        /// </summary>
        /// <param name="source">source collection length n</param>
        /// <param name="k">the n choose k variable</param>
        /// <param name="allowDuplicates">allow duplicate combinations if the source contains duplicats</param>
        /// <param name="comparer">optional custom comparer</param>
        public Combinations(IEnumerable<T> source, int k, bool allowDuplicates = false, IComparer<T> comparer = null)
        {
            if (source == null)
                throw new ArgumentNullException();

            // copy the input
            input = source.ToArray();

            // record the "n choose k" variables 
            this.k = k;
            this.n = input.Length;

            if (k > n)
                throw new ArgumentOutOfRangeException();

            // if using the default comparer then T must implement IComparable<T> or IComparable
            this.comparer = comparer ?? Comparer<T>.Default;

            // sort the input
            Array.Sort(input, this.comparer);

            // check the source for duplicates which results in duplicate combinations.
            noDuplicates = true;

            if (!allowDuplicates)
            {
                for (int index = 1; (index < n) && noDuplicates; index++)
                    noDuplicates = this.comparer.Compare(input[index - 1], input[index]) != 0;
            }
        }


        /// <summary>
        /// Gets the non generic enumerator for the combinations.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        /// <summary>
        /// Gets the generic enumerator for the combinations.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<List<T>> GetEnumerator()
        {
            return new CombinationEnumerator(this);
        }



        /// <summary>
        /// The enumerator for the combinations. Each time MoveNext()
        /// is called a new combination generated.
        /// </summary>
        private class CombinationEnumerator : IEnumerator<List<T>>
        {
            /// <summary>
            /// the source collection
            /// </summary>
            private T[] input;

            /// <summary>
            /// the current combination
            /// </summary>
            private T[] current;

            /// <summary>
            /// The previous combination
            /// Used to filter out duplicate combinations
            /// </summary>
            private T[] previous;

            /// <summary>
            /// An integer look up table
            /// </summary>
            private int[] map;

            /// <summary>
            /// the "n choose k" variables
            /// </summary>
            private int n;
            private int k;

            /// <summary>
            /// Stores if the source contains duplicate objects
            /// </summary>
            bool noDuplicates = true;

            /// <summary>
            /// The comparer to use
            /// </summary>
            private IComparer<T> comparer;

            /// <summary>
            /// enumerator state 
            /// </summary>
            private bool setUpFirstItem = true;


            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="p"></param>
            public CombinationEnumerator(Combinations<T> c)
            {
                input = c.input;
                n = c.n;
                k = c.k;
                comparer = c.comparer;
                noDuplicates = c.noDuplicates;

                // allocate storage
                map = new int[k];
                current = new T[k];

                if (!noDuplicates)
                    previous = new T[k];
            }


            /// <summary>
            /// The IEnumerator.MoveNext interface implementation
            /// Moves to the next item. 
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                if (setUpFirstItem)
                {
                    setUpFirstItem = false;
                    Initialise() ;
                    return true;
                }

                if (GetNext())
                    return true;

                // prepare for the next iteration sequence
                setUpFirstItem = true;
                return false;
            }


            /// <summary>
            /// The IEnumerator.Reset interface implementation
            /// Provided for COM interoperability, but doesn't need to be implemented
            /// </summary>
            public void Reset()
            {
                throw new NotSupportedException();
            }


            /// <summary>
            /// IEnumerator<T>.Current interface implementation. 
            /// </summary>
            public List<T> Current
            {
                get { return new List<T>(current); }
            }


            /// <summary>
            /// IEnumerator.Current interface implementation.
            /// </summary>
            object IEnumerator.Current
            {
                get { return Current; }
            }


            /// <summary>
            /// Nothing to dispose, no unmanaged resources used
            /// </summary>
            void IDisposable.Dispose()
            {
            }


            /// <summary>
            /// Set up state for the first enumerator call
            /// </summary>
            private void Initialise()
            {
                // intialise the map with 0, 1, 2... the first combination
                for (int index = 0; index < k; index++)
                    map[index] = index;

                // copy sorted input to current 
                for (int index = 0; index < k; index++)
                    current[index] = input[index];
            }


            /// <summary>
            /// Generates the next combination of the input collection.
            /// It first gets the next map entry and uses it to
            /// build a list of the input objects of type T.
            /// </summary>
            private bool GetNext()
            {
                if (noDuplicates)
                {
                    if (GetNextMapEntry())
                    {
                        // build current from the map
                        for (int index = 0; index < k; index++)
                            current[index] = input[map[index]];

                        return true;
                    }

                    return false;
                }
                else
                {
                    // swap current to previous
                    T[] temp = previous;
                    previous = current;
                    current = temp;

                    while (GetNextMapEntry())
                    {
                        int cr = 0;

                        // build current from the map, checking for duplicates
                        for (int index = 0; index < k; index++)
                        {
                            current[index] = input[map[index]];

                            if (cr == 0)  // equal so far
                            {
                                cr = comparer.Compare(current[index], previous[index]);

                                if (cr < 0) // less than, its a duplicate
                                    break;
                            }
                        }

                        if (cr > 0)  // greater than, its not a duplicate
                            return true;
                    }

                    return false;
                }
            }


            /// <summary>
            /// Generate combinations of intergers in lexicographic order
            /// The resultant combinations are used as a look up table to build
            /// combinations of objects of type T 
            /// Algorithm by Donald Knuth
            /// </summary>
            private bool GetNextMapEntry()
            {
                if (k == 0)
                    return false;

                // start at last item
                int i = k - 1;

                while (map[i] == (n - k + i))  // find next item to increment 
                {
                    if (--i < 0)
                        return false; // all done
                }

                ++map[i]; // increment

                // do next 
                for (int j = i + 1; j < k; j++)
                    map[j] = map[i] + j - i;

                return true;
            }
        }
    }
}


