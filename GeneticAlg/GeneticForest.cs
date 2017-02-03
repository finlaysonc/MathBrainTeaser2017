using System;
using System.Collections.Generic;
using GeneticAlg;

/*
 * GeneticForest.cpp
 *
 *  Created on: 24.06.2012
 *      Author: ignacio
 */

/*
 * GeneticForest.h
 *
 *  Created on: 24.06.2012
 *      Author: ignacio
 */




namespace neurignacio
{
// typedef
// GenTree
public class GenTree : Tree
{
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void inorderNumberLeaf(Node* x, ClassicLinkedList<int>* l) const
	private void inorderNumberLeaf(Node x, LinkedList<int> l)
	{
		if (x != this.nil)
		{
			inorderNumberLeaf(x.left, l);
			NumberNode n = x as NumberNode;
			if (n != null)
			{
				l.AddLast(*n.number);
			}
			inorderNumberLeaf(x.right, l);
		}
	}
	public double score;
	public GenTree() : base()
	{
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void print() const
	public new void print()
	{
		base.print();
		Console.Write(" = ");
		Console.Write(this.eval());
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int eval() const
	public int eval()
	{
		return root.eval();
	}
	public void swap(GenTree X, GenTree Y)
	{
		// 1a. If we are in a NumberNode, swap from here
		// 1b. If we are not in a NumberNode, choose randomly between go left, go right or swap from here
		Node n = 0; // Store the visited Node
		// Tree X
		Node x = 0; // Swap point of Genome X
		Tree.Child xChild = Tree.LEFT;
		n = X.root;
		OperatorNode a = n as OperatorNode;
		// Here the probability of Swap Point is the same for each node
		while (a != null && x == null) // while we are a Operator Node and a Swap Point was not decided, go next
		{
			float probability = 1.0 / X.size();
			float randomNumber = (float)RandomNumbers.NextNumber() / RAND_MAX;
			if (randomNumber <= probability) // Choose this node
			{
//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
//ORIGINAL LINE: x=n;
				x = n;
				xChild = Tree.NONE;
			}
			else if (randomNumber > probability && randomNumber <= (probability + 1) / 2) // randomNumber is in the first half after the probability segment => probability + (1-probability)/2
			{
				// Go left
				n = n.left;
				xChild = Tree.LEFT;
			}
			else
			{
				// Go right
				n = n.right;
				xChild = Tree.RIGHT;
			}
			a = n as OperatorNode;
		}
		if (x == null) // we are not in a NumberNode
		{
//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
//ORIGINAL LINE: x=n;
			x = n;
		}

		// Tree Y
		Node y = 0; // Swap point of Genome Y
		Tree.Child yChild = Tree.LEFT;
		n = Y.root; // Store the visited Node
		a = n as OperatorNode;
		// Here the probability of Swap Point is the same for each node
		while (a != null && y == null) // while we are a Operator Node and a Swap Point was not decided, go next
		{
			float probability = 1.0 / Y.size();
			float randomNumber = (float)RandomNumbers.NextNumber() / RAND_MAX;
			if (randomNumber <= probability) // Choose this node
			{
//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
//ORIGINAL LINE: y=n;
				y = n;
				yChild = Tree.NONE;
			}
			else if (randomNumber > probability && randomNumber <= (probability + 1) / 2) // randomNumber is in the first half after the probability segment => probability + (1-probability)/2
			{
				// Go left
				n = n.left;
				yChild = Tree.LEFT;
			}
			else
			{
				// Go right
				n = n.right;
				yChild = Tree.RIGHT;
			}
			a = n as OperatorNode;
		}
		if (y == null)
		{
//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
//ORIGINAL LINE: y=n;
			y = n;
		}

		// Cross-Over

		// Check Root
		// 1. Swap parents
		Node temp = 0;

		temp = x.p;
		x.p = y.p;
//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
//ORIGINAL LINE: y->p=temp;
		y.p = temp;

		// 2. Swap Parent's child
		if (xChild == Tree.LEFT)
		{
//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
//ORIGINAL LINE: y->p->left=y;
			y.p.left = y;
		}
		else if (xChild == Tree.RIGHT)
		{
//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
//ORIGINAL LINE: y->p->right=y;
			y.p.right = y;
		}
		if (yChild == Tree.LEFT)
		{
//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
//ORIGINAL LINE: x->p->left=x;
			x.p.left = x;
		}
		else if (yChild == Tree.RIGHT)
		{
//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
//ORIGINAL LINE: x->p->right=x;
			x.p.right = x;
		}
		//
		if (x == X.root)
		{
//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
//ORIGINAL LINE: X.root=y;
			X.root = y;
		}
		if (y == Y.root)
		{
//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
//ORIGINAL LINE: Y.root=x;
			Y.root = x;
		}
	}
	public void setRoot(OperatorNode op)
	{
//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
//ORIGINAL LINE: root=op;
		root = op;
		root.p = Tree.nil;
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool hasRepetitions() const
	public bool hasRepetitions()
	{
		SortedSet<int> numberSet = new SortedSet<int>();
		System.Tuple< SortedSet<int>.Enumerator, bool> ret = new System.Tuple< SortedSet<int>.Enumerator, bool>(null, false);
		bool repetition = true;
		LinkedList<int> listOfLeaf = this.getNumberLeaf();
		LinkedList<int>.Enumerator it = listOfLeaf.GetEnumerator();
		while (it.MoveNext())
		{
			ret = numberSet.Add(it.Current);
			repetition = repetition && ret.Item2;
		}
		// repetition = true => all the insertions were successful because there were no repetitions
		// repetition = false => at least one insertion failed because it was repeated
		return !repetition; // true => at least one insertion failed because it was repeated
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: ClassicLinkedList<int> getNumberLeaf() const
	public LinkedList<int> getNumberLeaf()
	{
		LinkedList<int> l = new LinkedList<int>();
		inorderNumberLeaf(this.root, l);
		return l;
	}
//	void inorderCopy(Node* x, Node* y, GenTree* T);
}

public class GenTreeComparison
{
	public int goal;
	public GenTreeComparison(int goal)
	{
		this.goal = goal;
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator ()(const GenTree& lhs, const GenTree& rhs) const
	public static bool functorMethod(GenTree lhs, GenTree rhs)
	{
		return (fitScore(lhs.eval(), goal) > fitScore(rhs.eval(), goal));
	}
}

public class Forest
{
	public GenTree fittest;
	public int goal;
	public bool allow_repetitions;
	// const
	public Addition ADDITION = new Addition();
	public Substraction SUBSTRACTION = new Substraction();
	public Multiplication MULTIPLICATION = new Multiplication();
	public Division DIVISION = new Division();
	public OperatorPointerContainer_t OPERATORPOINTERCONTAINER = new OperatorPointerContainer_t();
	public NumberContainer_t NUMBERCONTAINER = new NumberContainer_t();
	//
	public OperatorNodeContainer_t operatorNodeContainer = new OperatorNodeContainer_t();
	public NumberNodeContainer_t numberNodeContainer = new NumberNodeContainer_t();
	//
	public GenTreeContainer_t treeContainer = new GenTreeContainer_t();
	//
	public GenTreeComparison comparison = new GenTreeComparison();


	private const byte NUMBER_OF_OPERATIONS = 4;
	public Forest(NumberContainer_t v, int goal, bool allow_repetitions = true)
	{
		this.fittest = 0;
		this.goal = goal;
		this.allow_repetitions = allow_repetitions;
		this.ADDITION = new Addition();
		this.SUBSTRACTION = new Substraction();
		this.MULTIPLICATION = new Multiplication();
		this.DIVISION = new Division();
		this.OPERATORPOINTERCONTAINER = new OperatorPointerContainer_t(NUMBER_OF_OPERATIONS,0);
		this.NUMBERCONTAINER = new NumberContainer_t(v.begin(), v.end());
		this.operatorNodeContainer = new OperatorNodeContainer_t();
		this.numberNodeContainer = new NumberNodeContainer_t();
		this.treeContainer = new GenTreeContainer_t();
		this.comparison = new neurignacio.GenTreeComparison(goal);
			init();
	}
	public Forest(NumberPointer numberArray, uint arraySize, int goal, bool allow_repetitions = true)
	{
		this.fittest = 0;
		this.goal = goal;
		this.allow_repetitions = allow_repetitions;
		this.ADDITION = new Addition();
		this.SUBSTRACTION = new Substraction();
		this.MULTIPLICATION = new Multiplication();
		this.DIVISION = new Division();
		this.OPERATORPOINTERCONTAINER = new OperatorPointerContainer_t(NUMBER_OF_OPERATIONS,0);
		this.NUMBERCONTAINER = new NumberContainer_t(numberArray,numberArray + arraySize);
		this.operatorNodeContainer = new OperatorNodeContainer_t();
		this.numberNodeContainer = new NumberNodeContainer_t();
		this.treeContainer = new GenTreeContainer_t();
		this.comparison = new neurignacio.GenTreeComparison(goal);
			init();
	}
	public void init()
	{
		OPERATORPOINTERCONTAINER[0] = ADDITION.functorMethod;
		OPERATORPOINTERCONTAINER[1] = SUBSTRACTION.functorMethod;
		OPERATORPOINTERCONTAINER[2] = MULTIPLICATION.functorMethod;
		OPERATORPOINTERCONTAINER[3] = DIVISION.functorMethod;
	}
	//
	public GenTree createRandomTree()
	{
		// 0. push a new tree in the container
		treeContainer.push_back(new GenTree());
		GenTree tree = treeContainer.back();
		// 1. I will create the root, this is always a operator node
		OperatorNode op = newRandomOperatorNode();
		tree.setRoot(op);
		// 2. Create children of different values
		NumberNode z;
		uint numberContainerSize = NUMBERCONTAINER.size();
		//	Chose random number for left and right
		uint randomIndex_right = RandomNumbers.NextNumber() % numberContainerSize;
		uint randomIndex_left;
		do
		{
			randomIndex_left = RandomNumbers.NextNumber() % numberContainerSize;
		} while (randomIndex_left == randomIndex_right);
		// Create left child
		z = newNumberNode(NUMBERCONTAINER[randomIndex_left]);
		z.p = tree.root;
//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
//ORIGINAL LINE: tree.root->left=z;
		tree.root.left = z;
		// Create right child
		z = newNumberNode(NUMBERCONTAINER[randomIndex_right]);
		z.p = tree.root;
//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
//ORIGINAL LINE: tree.root->right=z;
		tree.root.right = z;
		return tree;
	}
//C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
//	void createDefaultTree();
	public NumberNode newRandomNumberNode()
	{
		uint randomInt = RandomNumbers.NextNumber() % NUMBERCONTAINER.size();
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the following C++ macro:
		Console.Write("Forest::newRandomNumberNode(); randomInt=");
		Console.Write(randomInt);
		Console.Write(" LINE ");
		Console.Write(__LINE__);
		Console.Write("\n");
		return newNumberNode(NUMBERCONTAINER[randomInt]);
	}
	public NumberNode newNumberNode(int value)
	{
//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to value types:
//ORIGINAL LINE: int* numberPointer=&(*find(NUMBERCONTAINER.begin(), NUMBERCONTAINER.end(),value));
		int numberPointer = (*find(NUMBERCONTAINER.begin(), NUMBERCONTAINER.end(),value));
		return newNumberNode(numberPointer);
	}
//C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
//	NumberNode newNumberNode(NumberPointer num);
	public OperatorNode newRandomOperatorNode()
	{
		uint randomInt = RandomNumbers.NextNumber() % OPERATORPOINTERCONTAINER.size();
		return newOperatorNode(randomInt);
	}
	public OperatorNode newOperatorNode(uint index)
	{
		VirtualOperator op = OPERATORPOINTERCONTAINER[index];
		return newOperatorNode(op);
	}
//C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
//	OperatorNode newOperatorNode(OperatorPointer op);
	public void initiate()
	{
		// Clear all containers
		operatorNodeContainer.clear();
		numberNodeContainer.clear();
		treeContainer.clear();
		//
		List<int> shuffleContainer = NUMBERCONTAINER;
		random_shuffle(shuffleContainer.GetEnumerator(), shuffleContainer.end());
		int numberContainerSize = shuffleContainer.Count;
		for (int i = 0;i < numberContainerSize; i += 2)
		{
			treeContainer.push_back(new GenTree());
			GenTree tree = treeContainer.back();
			// 1. Root
			OperatorNode op = newRandomOperatorNode();
			tree.setRoot(op);
			// 2a. Root->left
			tree.root.left = newNumberNode(shuffleContainer[i]);
			tree.root.left.p = tree.root;
			// 2b. Root->right
			tree.root.right = newNumberNode(shuffleContainer[i + 1]);
			tree.root.right.p = tree.root;
		}
	}
//C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
//	GenTreeContainer_t::iterator RouletteWheelSelection(double totalScore, GenTreeContainer_t container);
	public double getScore(GenTree tree)
	{
		if (allow_repetitions)
		{
			return fitScore(tree.eval(), goal);
		}
		else
		{
			return fitScore(tree.eval() * (1 - tree.hasRepetitions()), goal); // if repetitions, the tree evaluates to 0 for computation of score
		}
	}
	public double getPopulationScore()
	{
		double totalScore = 0;
		for (LinkedList<GenTree>.Enumerator tree = treeContainer.begin(); tree.MoveNext();)
		{
			tree.score = getScore(tree.Current); // Update the score of each tree
			totalScore += tree.score;
		}
		return totalScore;
	}
	public double getBestScore()
	{
		double bestScore = -1e100;
		for (LinkedList<GenTree>.Enumerator tree = treeContainer.begin(); tree.MoveNext();)
		{
			tree.score = getScore(tree.Current);
			if (tree.score > bestScore)
			{
				bestScore = tree.score;
				fittest = (tree.Current);
			}

		}
		return bestScore;
	}
//	void updatePopulationScore();
	public void mate()
	{
		// Update the score of each tree of the population and returns the total score of the population
		double totalScore = getPopulationScore();
		// Sort population by decreasing fitness score
		LinkedList<GenTree> sortedContainer = treeContainer;
		sortedContainer.sort(comparison.functorMethod);
		// Choose a breeding couple according to the Roulette Wheel Selection
		LinkedList<GenTree>.Enumerator father = RouletteWheelSelection(totalScore, sortedContainer);
		LinkedList<GenTree>.Enumerator mother = RouletteWheelSelection(totalScore, sortedContainer);
		while (father == mother)
		{
			mother = RouletteWheelSelection(totalScore, sortedContainer);
		}
		// Create offspring
//C++ TO C# CONVERTER TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
		GenTree son = this.duplicate(father);
//C++ TO C# CONVERTER TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
		GenTree daughter = this.duplicate(mother);
		// Crossover offspring according to swap rule
		swap(son, daughter);
	}
	public GenTree duplicate(GenTree tree)
	{
		this.treeContainer.push_back(new GenTree());
		GenTree copyTree = treeContainer.back();
		CopyGenTree copy = new CopyGenTree(tree, copyTree);
		copy.inorderCopy(tree.root, this);
		return copyTree;
	}
	public void print()
	{
		for (LinkedList<GenTree>.Enumerator tree = treeContainer.begin(); tree.MoveNext();)
		{
			tree.print();
			Console.Write(" ");

		}
	}
}

public class CopyGenTree
{
	// Duplicate X in Y by value
	public Node x; // Node in 'X' tree
	public Node y; // Node in 'Y' tree
	public Node yPrevious; // Node in 'Y' tree
//	Node* y2;
	public GenTree X;
	public GenTree Y;
	public CopyGenTree(GenTree X, GenTree Y)
	{
		this.x = X.root;
		this.y = Y.root;
		this.yPrevious = Y.nil;
//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
//ORIGINAL LINE: this.X = X;
		this.X = X;
//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
//ORIGINAL LINE: this.Y = Y;
		this.Y = Y;
	}
	public void inorderCopy(Node x, Forest forest)
	// x: Node in 'this' tree
	// loop: loop counter for debugging purposes
	{
		if (x != X.nil)
		{
			inorderCopy(x.left, forest);

			NumberNode n = x as NumberNode;
			OperatorNode op = x as OperatorNode;
			if (n != null && op == null)
			{
				y = forest.newNumberNode(*n.number);
				if (X.root == x)
				{
					Y.root = y;
				}
			}
			else if (op != null && n == null)
			{
				OperatorNode newOp = forest.newOperatorNode(op.operation);
				if (yPrevious != Y.nil)
				{
					newOp.p = yPrevious;
//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
//ORIGINAL LINE: yPrevious->right=newOp;
					yPrevious.right = newOp;
				}
				newOp.left = y;
//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
//ORIGINAL LINE: y->p=newOp;
				y.p = newOp;
//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
//ORIGINAL LINE: y=newOp;
				y = newOp;
//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
//ORIGINAL LINE: yPrevious=newOp;
				yPrevious = newOp;
				if (X.root == x)
				{
					Y.setRoot(newOp);
				}
			}
			else
			{
			}
			inorderCopy(x.right, forest);
			if (x as NumberNode)
			{
			}
			else if (x as OperatorNode)
			{
				yPrevious.right = y;
				y.p = yPrevious;
				y = yPrevious;
				yPrevious = y.p;
			}
			else
			{
				//	I will do nothing
			}
		}
	}
}

} // end namespace neurignacio



//#include <iostream>
//using std::cout;
//using std::endl;




