public static class GlobalMembers
{
	public static double fitScore(int value, int goal)
	{
		int diff = goal - value;
		diff = (diff > 0? diff : -diff);

		return (double)1 / (diff + 1);
	}

	public static void Example()
	{
		RandomNumbers.Seed(time(null));
		time_t startTime = new time_t();
		uint dt;
		const uint TIME_MAX = 45; // seconds

		int NUMBER_OF_GENERATIONS = 10000;
		const uint NUMBERSET_SIZE = 14;
		int[] NUMBERSET = {1,2,3,4,5,6,7,8,9,10,25,50,75,100};
		bool ALLOW_REPETITIONS = false;
		// Now I find the goal. El objetivo es obtener, en 45 segundos, un número entero natural (del 101 al 999)
		int GOAL = RandomNumbers.NextNumber() % 899 + 101; // Choose a number between 0 and 898 and add 101
		List<int> numberSet = new List<int>(NUMBERSET,NUMBERSET + NUMBERSET_SIZE);
		int numberOfNumbers = 6;
		random_shuffle(numberSet.GetEnumerator(), numberSet.end());
		numberSet.resize(numberOfNumbers);
		Forest forest = new Forest(numberSet, GOAL, ALLOW_REPETITIONS);
		Console.Write("Number Set: ");
		for (List<int>.Enumerator it = forest.NUMBERCONTAINER.begin(); it.MoveNext();)
		{
			Console.Write(it.Current);
			Console.Write(" ");
		}
		Console.Write("\n");
		Console.Write("Goal: ");
		Console.Write(GOAL);
		Console.Write("\n");

		// Create Father
		forest.createRandomTree();
		// Create Mother
		forest.createRandomTree();
		// Start Algorithm
//C++ TO C# CONVERTER NOTE: 'register' variable declarations are not supported in C#:
//ORIGINAL LINE: register int i=0;
		int i = 0;
		double bestScore = -1e100;
		time(startTime);
		do
		{
			// Add some new blood
			forest.createRandomTree();
			// Find a apropiate breeding couple according to Roulette Wheel Selection
			forest.mate();
			bestScore = forest.getBestScore();
			++i; // Generetion counter
			dt = difftime(time(null), startTime); // Time counter
		} while (i < NUMBER_OF_GENERATIONS && bestScore != 1 && dt < TIME_MAX);
		// Print results
		Console.Write("Solution was ");
		Console.Write((i == NUMBER_OF_GENERATIONS || dt >= TIME_MAX != 0? "NOT " : ""));
		Console.Write("found after ");
		Console.Write(i);
		Console.Write(" generations in ");
		Console.Write(dt);
		Console.Write(" seconds");
		Console.Write("\n");
		Console.Write(" Fittest for goal=");
		Console.Write(GOAL);
		Console.Write(": ");
		forest.fittest.print();
		Console.Write("\n");
	}
//C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
	//void Example();
	/*
	 * main.cpp
	 *
	 *  Created on: 26.05.2012
	 *      Author: ignacio
	 */

	static int Main()
	{
		neurignacio.Example();
		return 0;
	}
}