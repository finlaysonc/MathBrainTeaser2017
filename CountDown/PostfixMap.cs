// Written by David Hancock mailto:code@davidhancock.net
// This code is published under The Code Project Open License.
// For full license terms see:
// http://www.codeproject.com/info/cpol10.aspx

using System;
using System.Collections.Generic;



namespace CountDown
{
    /// <summary>
    /// The PostfixMap class creates a map that is used to build postfix equations. There are
    /// six map entries one for each combination of tiles i.e 1 of 6, 2 of 6, 3 of 6 etc. 
    /// Post fix equations are built up of a repeating sequence of pushing digits 
    /// on to a stack followed by executing zero or more operators. The diagram below
    /// shows the possible post fix equations for 6 tiles:
    ///
    ///            12.3..4...5....6......
    ///
    /// The digits represent tiles and the dots represent possible operator positions. All 
    /// equations will start by pushing 2 digits followed by executing 0 or 1 operator, 
    /// then another digit followed by 0 to 2 operators etc. There will always be one less 
    /// operator than digits and the final map entry will always be an operator. 
    /// 
    /// Consider the case for 4 digits, there are 5 map sub entries one for each possible 
    /// postfix equation:
    /// 
    ///         Equation        Operator Counts     Map Entries
    ///                        
    ///         12.3..4ooo      =>  0 0 3           =>  4 0 0 0        => push 4 digits, execute 3 operators
    ///         12.3.o4oo.	    =>  0 1 2           =>  3 0 1 0 0
    ///         12o3..4oo.	    =>  1 0 2           =>  2 0 2 0 0 
    ///         12.3oo4o..	    =>  0 2 1           =>  3 0 0 1 0
    ///         12o3.o4o..	    =>  1 1 1           =>  2 0 1 0 1 0 
    ///         
    ///  Here the "o" represent actual operator positions with in the post fix equation. The code 
    ///  that generates the map first counts all the variations of operators in the possible operator 
    ///  locations. It then converts these counts into a map entries where numbers greater than zero 
    ///  means push that number of digits onto the stack and zeros indicates that an operator should 
    ///  be executed. The map entry always starts by pushing at least two digits onto the stack.
    /// </summary>
    public sealed class PostfixMap
    {
        /// <summary>
        /// the map
        /// </summary>
        private List<List<List<int>>> postfixMap;


        /// <summary>
        /// Constructor. Builds the map dynamically.
        /// </summary>
        public PostfixMap()
        {
            postfixMap = new List<List<List<int>>>(5);

            for (int digits = 2; digits <= 6; digits++)
                BuildMap(digits);
        }



        /// <summary>
        /// Worker routine. Each digit has a seperate entry.
        /// </summary>
        /// <param name="digitCount"></param>
        private void BuildMap(int digitCount)
        {
            // the number of entries in the map for the digit count
            int[] mapSize = new int[] {0, 0, 1, 2, 5, 14, 42};
           
            List<List<int>> map = new List<List<int>>(mapSize[digitCount]);

            BuildMapForDigitCount(map, digitCount);

            // add the map entry for this number of digits
            postfixMap.Add(map);
        }



        /// <summary>
        /// Counts variations of operators for the number of digits supplied
        /// The are always 1 less operators than digits in an equation and the 
        /// equation always ends with an operator
        /// 
        ///     12 . 3 .. 4 ... 5 .... 6 ......
        ///        ^   ^    ^     ^      ^
        ///        |   |    |     |      |
        ///       op0  |   op2    |     op4
        ///           op1        op3
        ///    
        /// </summary>
        /// <param name="map"></param>
        /// <param name="digitCount"></param>
        private void BuildMapForDigitCount(List<List<int>> map, int digitCount)
        {
            int op0, op1, op2, op3, op4;
            int operatorCount = 0;

            for (op4 = 0; (op4 < 6) && (operatorCount < digitCount); op4++, operatorCount++)
            {
                for (op3 = 0; (op3 < 5) && (operatorCount < digitCount); op3++, operatorCount++)
                {
                    for (op2 = 0; (op2 < 4) && (operatorCount < digitCount); op2++, operatorCount++)
                    {
                        for (op1 = 0; (op1 < 3) && (operatorCount < digitCount); op1++, operatorCount++)
                        {
                            for (op0 = 0; (op0 < 2) && (operatorCount < digitCount); op0++, operatorCount++)
                            {
                                switch (digitCount)
                                {
                                    case 2:
                                        if (op1 > 0)
                                            return;
                                        if (operatorCount == 1)
                                            AddMapRow(map, op0);
                                        break;

                                    case 3:
                                        if (op2 > 0)
                                            return;
                                        if ((operatorCount == 2) && (op1 > 0))
                                            AddMapRow(map, op0, op1);
                                        break;

                                    case 4:
                                        if (op3 > 0)
                                            return;
                                        if ((operatorCount == 3) && (op2 > 0))
                                            AddMapRow(map, op0, op1, op2);
                                        break;

                                    case 5:
                                        if (op4 > 0)
                                            return;
                                        if ((operatorCount == 4) && (op3 > 0))
                                            AddMapRow(map, op0, op1, op2, op3);
                                        break;

                                    case 6:
                                        if ((operatorCount == 5) && (op4 > 0))
                                            AddMapRow(map, op0, op1, op2, op3, op4);
                                        break;

                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }

                            operatorCount -= op0;
                        }

                        operatorCount -= op1;
                    }

                    operatorCount -= op2;
                }

                operatorCount -= op3;
            }
        }


        /// <summary>
        /// Converts operator variation counts into map rows
        /// </summary>
        /// <param name="map"></param>
        /// <param name="operators"></param>
        private void AddMapRow(List<List<int>> map, params int[] operators)
        {
            int digitCount = 2; // start by pushing at least two digits on to the statck
            int digitTotal = 0;
            int operatorTotal = 0;

            List<int> row = new List<int>(operators.Length * 2);

            foreach (int operatorCount in operators)
            {
                if (operatorCount == 0) // push another digit on to the stack
                    digitCount++;
                else
                {
                    digitTotal += digitCount;
                    operatorTotal += operatorCount;

                    if (operatorTotal < digitTotal)
                    {
                        row.Add(digitCount);    // push digits

                        int count = operatorCount;  

                        while (count-- > 0)
                            row.Add(0); // add zeros which force the evaluation of an operator

                        digitCount = 1; // reset digit count
                    }
                    else
                        return; // invalid, more operators than digits so far, ignore this entry
                }
            }

            map.Add(row);
        }


        /// <summary>
        /// Indexer access.
        /// Index is the number of digits in the equation that this map entry is used for
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public List<List<int>> this[int index]
        {
            get
            {
                return postfixMap[index - 2];
            }
        }
    }
}
