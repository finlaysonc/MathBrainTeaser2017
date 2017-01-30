// Written by David Hancock mailto:code@davidhancock.net
// This code is published under The Code Project Open License.
// For full license terms see:
// http://www.codeproject.com/info/cpol10.aspx

//#define DisplayPostfix

using System;
using System.Collections.Generic;
using System.Diagnostics;



namespace CountDown
{
    public sealed class PostfixSolvingEngine
    {
        /// <summary>
        /// The map that describes how the postfix equation is constructed
        /// </summary>
        private PostfixMap postfixMap;

        /// <summary>
        /// Supplied parameters
        /// </summary>
        private int target;
        private int[] tiles;

        /// <summary>
        /// Store for the equation that match the target
        /// </summary>
        private List<string> matches;

        /// <summary>
        /// Used to store and identify the closest equation that doesn't 
        /// match the target
        /// </summary>
        private string closestNonMatch = String.Empty;
        private int absDifference = 10;
        private int realDifference;

        /// <summary>
        /// Operator identifiers
        /// </summary>
        private const int cMultiply = 0;
        private const int cAdd = 1;
        private const int cSubtract = 2;
        private const int cDivide = 3;

        /// <summary>
        /// String equivilents of the operators
        /// </summary>
        private static readonly string[] opStr = { " × ", " + ", " - ", " ÷ " };

        /// <summary>
        /// The postfix equation evaluation stacks. 
        /// Each level of recursion has its own copy of the stack
        /// </summary>
        private int[][] stacks;

        /// <summary>
        /// Keeps a record of the operators used when evaluating the
        /// current postfix equation. Used to construct a string
        /// representation of the equation
        /// </summary>
        private int[] operators;



        /// <summary>
        /// Hide the default constructor
        /// </summary>
        private PostfixSolvingEngine()
        {
        }


        /// <summary>
        /// Public constructor
        /// </summary>
        /// <param name="tiles"></param>
        /// <param name="target"></param>
        /// <param name="UpdateUIMethod"></param>
        public PostfixSolvingEngine(int[] tiles, int target)
        {
            // create the map defining the sequence of pushing numbers
            // on to the stack and evaluation of operators
            postfixMap = new PostfixMap();

            // store for the matching equations
            matches = new List<string>(100);

            // initialise the stacks. Each recursive call gets a copy 
            // of the current stack so that when the recursion unwinds
            // the caller can simply proceed with the next operator
            stacks = new int[tiles.Length - 1][];

            for (int index = 0; index < stacks.Length; index++)
                stacks[index] = new int[tiles.Length];

            // store for the operators used to build a string representation of the current equation
            operators = new int[tiles.Length - 1];

            // record params
            this.tiles = tiles;
            this.target = target;
        }





        ///// <summary>
        ///// The postfix equation solving method. It implements a sequence of pushing zero or more tiles 
        ///// onto the stack before executing an operator. It then recurses executing another sequence. Each
        ///// recursive call gets its own copy of the current stack so that when the recursion unwinds it 
        ///// can simply continue with the next operator. The recursion ends when the map is completed.
        ///// Although it is a linear function conceptually it can be thought of as a tree. Each sequence
        ///// splits into 4 branches (one for each operator), and for each recursion every branch further 
        ///// splits into 4 more branches. This repeats until the last sequence in the map when the final 
        ///// result is obtained for the 4 operators. After the first operator is executed subsiquent
        ///// equations are calculated by executing only one operator rather than the complete equation.
        ///// The same principal applies for each node in the tree as the recursion unwinds.
        ///// </summary>
        ///// <param name="stackHead">the stack head pointer</param>
        ///// <param name="mapEntry">the current postfix map entry</param>
        ///// <param name="mapIndex">position within the map</param>
        ///// <param name="permutation">the current tile permutation</param>
        ///// <param name="permutationIndex">position in the permutation</param>
        ///// <param name="depth">recursion depth</param>
        //private void SolveRecursive(int stackHead, List<int> mapEntry, int mapIndex, List<int> permutation, int permutationIndex, int depth)
        //{
        //    // identify which stack to use for this depth of recursion
        //    int[] stack = stacks[depth];

        //    // read how many numbers that are required to be pushed on to the stack
        //    int pushCount = mapEntry[mapIndex++];

        //    // if at least two tiles are pushed onto the stack, the operator will act on
        //    // tiles directly rather than a derived value. This allows simple duplicate 
        //    // equations to be filtered out e.g. 4+6 == 6+4 so the latter can be ignored.
        //    bool derived = true;

        //    if (pushCount > 0)
        //    {
        //        derived = (pushCount < 2);

        //        while (pushCount-- > 0)
        //            stack[++stackHead] = (permutation[permutationIndex++]); // push

        //        ++mapIndex; // for the first operator
        //    }

        //    int right = stack[stackHead--]; // pop
        //    int left = stack[stackHead];    // peek

        //    for (int op = 0; op < 4; op++)
        //    {
        //        operators[depth] = op; // record the current operator

        //        int result = -1;

        //        if (op == cMultiply)
        //        {
        //            if ((left > 1) && (right > 1) && (derived || (left <= right)))
        //                result = left * right;
        //        }
        //        else if (op == cAdd)
        //        {
        //            if (derived || (left <= right))
        //                result = left + right;
        //        }
        //        else if (op == cSubtract)
        //        {
        //            result = left - right;
        //        }
        //        else // cDivide
        //        {
        //            if ((right > 1) && (left >= right) && ((left % right) == 0))
        //                result = left / right;
        //        }


        //        if (result > 0)  // valid result
        //        {
        //            if (mapIndex < mapEntry.Count)   // some left
        //            {
        //                // copy the current stack for the next depth of recursion
        //                int[] localStack = stacks[depth + 1];

        //                for (int index = 0; index < stackHead; index++)
        //                    localStack[index] = stack[index];

        //                // record the current result
        //                localStack[stackHead] = result; // poke

        //                // evaluate the next sequence
        //                SolveRecursive(stackHead, mapEntry, mapIndex, permutation, permutationIndex, depth + 1);
        //            }
        //            else
        //            {
        //                if (result == target)   // got one...
        //                {
        //                    matches.Add(AsString(mapEntry, permutation));
        //                    break;
        //                }

        //                if (matches.Count == 0) // no matches, record the closest result
        //                {
        //                    int difference;

        //                    if (result < target)
        //                        difference = target - result;
        //                    else
        //                        difference = result - target;

        //                    if (difference < absDifference)
        //                    {
        //                        absDifference = difference;
        //                        realDifference = target - result;
        //                        closestNonMatch = AsString(mapEntry, permutation);
        //                    }
        //                }

        //                if ((op == cMultiply) && (result < target))
        //                {
        //                    // multiplied but result is still less than target
        //                    // no point evaluating the other three operators
        //                    // (multiply by one is an invalid operation)
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}




        /// <summary>
        /// The postfix equation solving method. It implements a sequence of pushing zero or more tiles 
        /// onto the stack before executing an operator. It then recurses executing another sequence. Each
        /// recursive call gets its own copy of the current stack so that when the recursion unwinds it 
        /// can simply continue with the next operator. The recursion ends when the map is completed.
        /// Although it is a linear function conceptually it can be thought of as a tree. Each sequence
        /// splits into 4 branches (one for each operator), and for each recursion every branch further 
        /// splits into 4 more branches. This repeats until the last sequence in the map when the final 
        /// result is obtained for the 4 operators. After the first operator is executed subsiquent
        /// equations are calculated by executing only one operator rather than the complete equation.
        /// The same principal applies for each node in the tree as the recursion unwinds.
        /// </summary>
        /// <param name="stackHead">the stack head pointer</param>
        /// <param name="mapEntry">the current postfix map entry</param>
        /// <param name="mapIndex">position within the map</param>
        /// <param name="permutation">the current tile permutation</param>
        /// <param name="permutationIndex">position in the permutation</param>
        /// <param name="depth">recursion depth</param>
        private void SolveRecursive2(int stackHead, List<int> mapEntry, int mapIndex, List<int> permutation, int permutationIndex, int depth)
        {
            // identify which stack to use for this depth of recursion
            int[] stack = stacks[depth];

            // read how many numbers that are required to be pushed on to the stack
            int pushCount = mapEntry[mapIndex++];

            // if at least two tiles are pushed onto the stack, the operator will act on
            // tiles directly rather than a derived value. This allows simple duplicate 
            // equations to be filtered out e.g. 4+6 == 6+4 so the latter can be ignored.
            bool derived = true;

            if (pushCount > 0)
            {
                derived = (pushCount < 2);

                while (pushCount-- > 0)
                    stack[++stackHead] = (permutation[permutationIndex++]); // push

                ++mapIndex; // for the first operator
            }

            int right = stack[stackHead--]; // pop
            int left = stack[stackHead];    // peek

            for (int op = 0; op < 4; op++)
            {
                operators[depth] = op; // record the current operator

                int result = -1;

                if (op == cMultiply)
                {
                    if ((left > 1) && (right > 1) && (derived || (left <= right)))
                        result = left * right;
                }
                else if (op == cAdd)
                {
                    if (derived || (left <= right))
                        result = left + right;
                }
                else if (op == cSubtract)
                {
                    result = left - right;
                }
                else // cDivide
                {
                    if ((right > 1) && (left >= right) && ((left % right) == 0))
                        result = left / right;
                }


                    if (mapIndex < mapEntry.Count)   // some left
                    {
                        // copy the current stack for the next depth of recursion
                        int[] localStack = stacks[depth + 1];

                        for (int index = 0; index < stackHead; index++)
                            localStack[index] = stack[index];

                        // record the current result
                        localStack[stackHead] = result; // poke

                        // evaluate the next sequence
                        SolveRecursive2(stackHead, mapEntry, mapIndex, permutation, permutationIndex, depth + 1);
                    }
                    else
                    {
                        if (result == target)   // got one...
                        {
                            matches.Add(AsString(mapEntry, permutation));
                            //break;
                            return;
                        }

                        //if (matches.Count == 0) // no matches, record the closest result
                        //{
                        //    int difference;

                        //    if (result < target)
                        //        difference = target - result;
                        //    else
                        //        difference = result - target;

                        //    if (difference < absDifference)
                        //    {
                        //        absDifference = difference;
                        //        realDifference = target - result;
                        //        closestNonMatch = AsString(mapEntry, permutation);
                        //    }
                        //}

                        if ((op == cMultiply) && (result < target))
                        {
                            // multiplied but result is still less than target
                            // no point evaluating the other three operators
                            // (multiply by one is an invalid operation)
                            break;
                        }
                    }
                
            }
        }




        //#if DisplayPostfix
        //#warning Caution - DisplayPostfix is defined 

        /// <summary>
        /// Convert the current postfix equation to a string
        /// Usefull for debugging
        /// </summary>
        /// <param name="mapEntry"></param>
        /// <param name="permutation"></param>
        /// <returns></returns>
        private String AsString(List<int> mapEntry, List<int> permutation)
        {
            int mapIndex = 0;
            int operatorCount = 0;
            int permutationCount = 0;

            System.Text.StringBuilder sb = new System.Text.StringBuilder(25);

            do
            {
                int count = mapEntry[mapIndex++];

                if (count > 0)
                {
                    while (count-- > 0)
                    {
                        sb.Append(permutation[permutationCount++]);

                        if (count > 0)
                            sb.Append(" ");
                    }
                }
                else
                    sb.Append(opStr[operators[operatorCount++]]);
            }
            while (mapIndex < mapEntry.Count);

            return sb.ToString();
        }

//#else

//        /// <summary>
//        /// Convert the current postfix equation to an infix formatted string
//        /// This works in the same way as evaluating the postfix equation
//        /// but instead of calculating simply concatinates strings
//        /// </summary>
//        /// <param name="mapEntry"></param>
//        /// <param name="permutation"></param>
//        /// <returns></returns>
//        private String AsString(List<int> mapEntry, List<int> permutation)
//        {
//            int mapIndex = 0;
//            int operatorCount = 0;
//            int permutationCount = 0;

//            string[] stack = new string[tiles.Length];
//            int stackHead = -1;

//            do
//            {
//                int digitCount = mapEntry[mapIndex++];

//                if (digitCount > 0) // push digits
//                {
//                    while (digitCount-- > 0)
//                        stack[++stackHead] = (permutation[permutationCount++]).ToString();
//                }
//                else // execute operator
//                {
//                    string right = stack[stackHead--]; // pop
//                    string left = stack[stackHead];    // peek

//                    int op = operators[operatorCount++];

//                    if (mapIndex < mapEntry.Count)
//                        stack[stackHead] = "(" + left + opStr[op] + right + ")"; // poke
//                    else
//                        stack[stackHead] = left + opStr[op] + right;
//                }
//            }
//            while (mapIndex < mapEntry.Count);

//            return stack[stackHead];
//        }
//#endif



        /// <summary>
        /// Runs the solving engine. 
        /// First it builds a list of all combinations of the tiles. 
        /// Then for each combination it build a list of all permutations of the tiles in that combination.
        /// Next using a postfix map that describes how to build postfix equations for the number of tiles
        /// it evaluates all possible equations.
        /// </summary>
        public void Solve()
        {
            tiles = new int[] {2,0,1,7};

            for (int i = 1; i < 100; i++)
            {

                for (int k = 2; k <= tiles.Length; k++)
                {
                    // get all the combinations for k digits
                    Combinations<int> combinations = new Combinations<int>(tiles, k);
                    // get the associated map
                    List<List<int>> map = postfixMap[k];

                    foreach (List<int> combination in combinations)
                    {
                        // get all the permutations for the combination
                        Permutations<int> permutations = new Permutations<int>(combination);

                        foreach (List<int> permutation in permutations)
                        {
                            foreach (List<int> mapRow in map)
                                SolveRecursive2(-1, mapRow, 0, permutation, 0, 0);
                        }
                    }
                }
            }
        }




        /// <summary>
        /// Nested type that is a contract defining how the results of the solving
        /// engine are notified via a delegate
        /// </summary>
        public class Results
        {
            public int[] Tiles { get; private set; }
            public int Target { get; private set; }
            public List<string> Matches { get; private set; }
            public string ClosestNonMatch { get; private set; }
            public int Difference { get; private set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="pse"></param>
            /// <param name="duration"></param>
            public Results(PostfixSolvingEngine pse)
            {
                Tiles = pse.tiles;
                Target = pse.target;
                Matches = pse.matches;
                ClosestNonMatch = pse.closestNonMatch;
                Difference = pse.realDifference;
            }
        }
    }
}
