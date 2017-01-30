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
    /// Generates all permutations for a collection of objects. 
    /// </summary>
    public class Permutations<T> : IEnumerable<List<T>>
    {
        /// <summary>
        /// store for the source 
        /// </summary>
        private T[] input;

        /// <summary>
        /// The comparer to use
        /// </summary>
        private IComparer<T> comparer;


        /// <summary>
        /// Hide the invalid default constructor
        /// </summary>
        protected Permutations()
        {
        }


        /// <summary>
        /// Constructor used to generate permutations of the supplied collection.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="customComparer"></param>
        public Permutations(IEnumerable<T> source, IComparer<T> comparer = null)
        {
            if (source == null)
                throw new ArgumentNullException();

            // copy source
            input = source.ToArray();

            // if using the default comparer then T must implement IComparable<T> or IComparable
            this.comparer = comparer ?? Comparer<T>.Default;
        }


        /// <summary>
        /// Gets the non generic enumerator for the permutations.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        /// <summary>
        /// Gets the generic enumerator for the permutations.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<List<T>> GetEnumerator()
        {
            return new PermutationEnumerator(this);
        }




        /// <summary>
        /// The enumerator for the permutations. Each time MoveNext()
        /// is called a new permutaion generated.
        /// </summary>
        private sealed class PermutationEnumerator : IEnumerator<List<T>>
        {
            /// <summary>
            /// the current permutation
            /// </summary>
            private T[] current;

            /// <summary>
            /// the comparer to use
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
            public PermutationEnumerator(Permutations<T> p)
            {
                // copy the input, the enumerator changes it
                current = p.input.ToArray();
                comparer = p.comparer;
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
                    Initialise();
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
                // sort the input - its the first permutation
                Array.Sort(current, comparer);
            }


            /// <summary>
            /// Gets the next non duplicate permutation in lexicographic order.
            /// Algorithm L by Donald Knuth. This algorithm is described in 
            /// The Art of Computer Programming, Volume 4, Fascicle 2: Generating All Tuples and Permutations.
            /// </summary>
            /// <returns></returns>
            private bool GetNext()
            {
                int end = current.Length - 1;
                int i = end;

                while (i > 0)
                {
                    int j = i;
                    i--;

                    // find the end of the head and start of the tail
                    if (comparer.Compare(current[i], current[j]) < 0)
                    {
                        int k = end;

                        // find smallest tail element that is less than or equal to the last head element
                        while (comparer.Compare(current[k], current[i]) < 1)
                            k--;

                        // swap the head and tail elements
                        Swap(i, k);

                        // reverse the tail
                        while (j < end)
                            Swap(j++, end--) ;

                        return true;
                    }
                }

                return false;
            }


            /// <summary>
            /// Utility swap routine
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            private void Swap(int a, int b)
            {
                T temp = current[a];
                current[a] = current[b];
                current[b] = temp;
            }
        }
    }
}

