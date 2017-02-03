using System;

/*
 * ArithmeticParser.cpp
 *
 *  Created on: 27.05.2012
 *      Author: ignacio
 */
/*
 * ArithmeticParser.h
 *
 *  Created on: 27.05.2012
 *      Author: ignacio
 */

namespace GeneticAlg
{

//C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
//class Node;


public abstract class VirtualOperator : System.IDisposable
{
	public virtual void Dispose()
	{
	}
	public static int operator()(Node x, Node y);
	public virtual string symbol()
	{
		return "?";
	}
//C++ TO C# CONVERTER TODO TASK: C# has no concept of a 'friend' function:
//ORIGINAL LINE: friend ostream& operator <<(ostream& out, VirtualOperator& op)
	public static ostream operator << (ostream @out, VirtualOperator op)
	{
		@out << op.symbol();
		return @out;
	}
}

public class Addition : VirtualOperator
{
	public static override int functorMethod(Node x, Node y)
	{
		return x.eval() + y.eval();
	}
	public override string symbol()
	{
		return "+";
	}
//C++ TO C# CONVERTER TODO TASK: C# has no concept of a 'friend' function:
//ORIGINAL LINE: friend ostream& operator <<(ostream& out, Addition& op)
	public static ostream operator << (ostream @out, Addition op)
	{
		@out << op.symbol();
		return @out;
	}
}

public class Multiplication : VirtualOperator
{
	public static override int functorMethod(Node x, Node y)
	{
		return x.eval() * y.eval();
	}
	public override string symbol()
	{
		return "*";
	}
//C++ TO C# CONVERTER TODO TASK: C# has no concept of a 'friend' function:
//ORIGINAL LINE: friend ostream& operator <<(ostream& out, Multiplication& op)
	public static ostream operator << (ostream @out, Multiplication op)
	{
		@out << op.symbol();
		return @out;
	}
}

public class Substraction : VirtualOperator
{
	public static override int functorMethod(Node x, Node y)
	{
		return x.eval() - y.eval();
	}
	public override string symbol()
	{
		return "-";
	}
//C++ TO C# CONVERTER TODO TASK: C# has no concept of a 'friend' function:
//ORIGINAL LINE: friend ostream& operator <<(ostream& out, Substraction& op)
	public static ostream operator << (ostream @out, Substraction op)
	{
		@out << op.symbol();
		return @out;
	}
}

public class Division : VirtualOperator
{
	public static override int functorMethod(Node x, Node y)
	{
		int evalX = x.eval();
		int evalY = y.eval();
		if (evalY != 0)
		{
			return (evalX % evalY == 0? evalX / evalY : 0);
		}
		else
		{
			return 0;
		}
	}
	public override string symbol()
	{
		return "/";
	}
//C++ TO C# CONVERTER TODO TASK: C# has no concept of a 'friend' function:
//ORIGINAL LINE: friend ostream& operator <<(ostream& out, Division& op)
	public static ostream operator << (ostream @out, Division op)
	{
		@out << op.symbol();
		return @out;
	}
}


public class Node : System.IDisposable
{
	public Node left; // left child
	public Node right; // right child
	public Node p; // parent
	public Node()
	{
		this.left = 0;
		this.right = 0;
		this.p = 0;
	}
	public Node(Node l, Node r)
	{
//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
//ORIGINAL LINE: this.left = l;
		this.left = l;
//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
//ORIGINAL LINE: this.right = r;
		this.right = r;
		this.p = 0;
	}
	public virtual void Dispose()
	{
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual int eval() const
	public virtual int eval()
	{
		return 0;
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual string info() const
	public virtual string info()
	{
		return "Node";
	}
	public virtual string print()
	{
		ostringstream oss = new ostringstream();
		oss << this.info();
		oss << " (&" << this << ")";
		oss << ", p=";
		if (this.p != null)
		{
			oss << this.p;
		}
		else
		{
			oss << "?";
		}
		oss << " (&" << this.p << ")";
		oss << ", left=";
		if (this.left != null)
		{
				oss << this.left;
		}
			else
			{
				oss << "?";
			}
		oss << " (&" << this.left << ")";
		oss << ", right=";
		if (this.right != null)
		{
				oss << this.right;
		}
			else
			{
				oss << "?";
			}
		oss << " (&" << this.right << ")";
		return oss.str();
	}
	public virtual uint size()
	{
		return 0;
	}
//C++ TO C# CONVERTER TODO TASK: C# has no concept of a 'friend' function:
//ORIGINAL LINE: friend ostream& operator <<(ostream& out, Node& z)
	public static ostream operator << (ostream @out, Node z)
	{
		@out << z.info();
		return @out;
	}
}

public class OperatorNode : Node
{
	public OperatorPointer operation = new OperatorPointer();
	public OperatorNode() : base()
	{
		this.operation = 0;
	}
	public OperatorNode(OperatorPointer op, Node l = 0, Node r = 0) : base(l, r)
	{
		this.operation = op;
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int eval() const
	public new int eval()
	{
		return operation(left, right);
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator ==(const OperatorNode& op) const
	public static bool operator == (OperatorNode ImpliedObject, OperatorNode op)
	{
		return (ImpliedObject.left == op.left && ImpliedObject.right == op.right && ImpliedObject.p == op.p && ImpliedObject.operation == op.operation);
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual string info() const
	public override string info()
	{
		return operation.symbol();
	}
	public override string print()
	{
		ostringstream oss = new ostringstream();
		oss << base.print();
		oss << ", operation=";
		if (this.operation != null)
		{
			oss << this.operation;
		}
		else
		{
			oss << "?";
		}
		oss << " (&" << this.operation << ")";
		return oss.str();
	}
	public override uint size()
	{
		return left.size() + right.size() + 1;
	}
//C++ TO C# CONVERTER TODO TASK: C# has no concept of a 'friend' function:
//ORIGINAL LINE: friend ostream& operator <<(ostream& out, OperatorNode& z)
	public static ostream operator << (ostream @out, OperatorNode z)
	{
		@out << z.info();
		return @out;
	}
}


public class NumberNode: Node
{
	public NumberPointer number = new NumberPointer();
	public NumberNode() : base()
	{
		this.number = 0;
	}
	public NumberNode(NumberPointer n, Node l = 0, Node r = 0) : base(l, r)
	{
		this.number = n;
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int eval() const
	public new int eval()
	{
		return *number;
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator ==(const NumberNode& num) const
	public static bool operator == (NumberNode ImpliedObject, NumberNode num)
	{
		return (ImpliedObject.left == num.left && ImpliedObject.right == num.right && ImpliedObject.p == num.p && ImpliedObject.number == num.number);
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual string info() const
	public override string info()
	{
		ostringstream oss = new ostringstream();
		oss << *number;
		return oss.str();
	}
	public override string print()
	{
		ostringstream oss = new ostringstream();
		oss << base.print();
		oss << ", number=";
		if (this.number != null)
		{
			oss << this.number;
		}
		else
		{
			oss << "?";
		}
		oss << " (&" << this.number << ")";
		return oss.str();
	}
	public override uint size()
	{
		return left.size() + right.size() + 1;
	}
//C++ TO C# CONVERTER TODO TASK: C# has no concept of a 'friend' function:
//ORIGINAL LINE: friend ostream& operator <<(ostream& out, NumberNode& z)
	public static ostream operator << (ostream @out, NumberNode z)
	{
		@out << z.info();
		return @out;
	}
}

public class Tree : System.IDisposable
{
	protected const int SENTINEL_KEY = -1;
	protected const int NIL = 0;
	protected static Node SENTINEL = new Node();
	protected enum Child
	{
		NONE,
		LEFT,
		RIGHT
	}
	public static Node nil = SENTINEL;
	public Node root;
	//

	public Tree()
	{
		nil.left = SENTINEL;
		nil.right = SENTINEL;
		nil.p = SENTINEL;
		root = nil;

	}
	public void Dispose()
	{

	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void inorderTreeWalk(Node* x) const
	public void inorderTreeWalk(Node x)
	{
		ostringstream @out = new ostringstream();
		if (x != this.nil)
		{
			Console.Write("(");
			inorderTreeWalk(x.left);
			Console.Write(x);
			inorderTreeWalk(x.right);
			Console.Write(")");
		}
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void print() const
	public void print()
	{
		inorderTreeWalk(this.root);
	}
	public uint size()
	{
		return root.size();
	}
}

} // end namespace neurignacio





