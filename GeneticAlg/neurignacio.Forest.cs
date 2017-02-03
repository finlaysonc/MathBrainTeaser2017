using GeneticAlg;

namespace neurignacio
{

	public class Forest
	{
//C++ TO C# CONVERTER WARNING: The original C++ declaration of the following method implementation was not found:
		public NumberNode newNumberNode(ref int num)
		{
			this.numberNodeContainer.push_back(new NumberNode(num));
			NumberNode z;
			z = numberNodeContainer.back();
			z.p = Tree.nil;
			z.left = Tree.nil;
			z.right = Tree.nil;
			return z;
		}

//C++ TO C# CONVERTER WARNING: The original C++ declaration of the following method implementation was not found:
		public OperatorNode newOperatorNode(VirtualOperator op)
		{
			operatorNodeContainer.push_back(new OperatorNode(op));
			OperatorNode z;
			z = operatorNodeContainer.back();
			z.p = Tree.nil;
			z.left = Tree.nil;
			z.right = Tree.nil;
			return z;
		}

//C++ TO C# CONVERTER WARNING: The original C++ declaration of the following method implementation was not found:
		public LinkedList<GenTree>.Enumerator RouletteWheelSelection(double totalScore, LinkedList<GenTree> container)
		{
			double random = (double)RandomNumbers.NextNumber() / RAND_MAX; // random double between 0.0 and 1.0
			random *= totalScore; // random value between 0.0 and totalScore
			LinkedList<GenTree>.Enumerator tree = container.GetEnumerator();
		//C++ TO C# CONVERTER TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			double score = getScore(tree);
			while (random > score)
			{
		//C++ TO C# CONVERTER TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
				++tree;
				score += tree.score;
			}
			return tree;
		}
	}
}