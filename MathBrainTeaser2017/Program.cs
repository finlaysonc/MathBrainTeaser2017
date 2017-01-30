using System;
using System.Collections.Generic;
using System.Linq;
//using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YuriyGuts.RegexBuilder;
using NCalc;
using PasswordUtility;
using KeePassLib;
using NCalc.Domain;
using Regextra;

namespace MathBrainTeaser2017
{



    class Program2
    {



        static string GenerateExpression()
        {
            string chars = "(2017)+-*/()^";
            string rand = new string(chars.OrderBy(x => Guid.NewGuid()).ToArray());
//            Console.WriteLine(rand);
            return rand;

            //            string possibilities1 = "0*(2+1-7)";
            //            string possibilities2 = "-*/+()2017()";
        }

        static void Main2(string[] args)
        {

            var e = new Expression("SurpriseFunction( a([X],[Y],[Z],[Q]),b([Y]),c([Z]),d([Q]))");
            e.Parameters["X"] = 2;
            e.Parameters["Y"] = 0;
            e.Parameters["Z"] = 1;
            e.Parameters["Q"] = 7;

            e.EvaluateFunction += delegate(string name, FunctionArgs functionArgs)
            {
                Console.WriteLine(e.ParsedExpression);
                switch (name)
                {
                    case "a":
                        functionArgs.Result = 2;
                        break;
                    case "b":
                    case "c":
                    case "d":
                        functionArgs.Result = 3;
                        break;
                    case "SurpriseFunction":
                        int[] result = functionArgs.EvaluateParameters().Cast<int>().ToArray();
                        Console.WriteLine(functionArgs.Parameters[0].ParsedExpression);
                        BinaryExpression be = new BinaryExpression(BinaryExpressionType.Times,
                            new ValueExpression(functionArgs.Parameters[0].Evaluate()),
                            new ValueExpression(functionArgs.Parameters[1].Evaluate()));
                        functionArgs.Result = new Expression(be).Evaluate();
                        break;
                }
            };
            if (e.HasErrors())
            {
                Console.WriteLine(e.Error);
            }
            Console.WriteLine(e.Evaluate());
            Console.WriteLine();
            Console.WriteLine(e.ParsedExpression.ToString());
        }
    }
}


//            Assert.AreEqual(9, e.Evaluate());


//            Expression e = new Expression("[A]+[B]+[C]+[D]", EvaluateOptions.None);
//            e.Parameters["A"] = 2;
//            e.Parameters["B"] = 0;
//            e.Parameters["C"] = 1;
//            e.Parameters["D"] = 7;
//            e.EvaluateParameter+=delegate (string )

//            //Expression e = new Expression("Round(Pow([Pi], 2) + Pow([Pi], 2) + [X], 2)");

//            //e.Parameters["Pi2"] = new Expression("Pi * [Pi]");
//            //e.Parameters["X"] = 10;

//            e.EvaluateParameter += delegate (string name, ParameterArgs args)
//            {
//                if (name == "Pi")
//                    args.Result = 3.14;
//            };

//            e.Parameters["a"] = new int[] { 1, 2, 3, 4, 5 };
//            e.Parameters["b"] = new int[] { 6, 7, 8, 9, 0 };
//            e.Parameters["c"] = 3;

//            foreach (var result in (IList)e.Evaluate())
//            {
//                Console.WriteLine(result);
//            }


//            Expression e = new Expression("(Round(Pow([Pi], 2) + Pow([Pi2], 2) + [X], 2)");

//            e.Parameters["Pi2"] = new Expression("Pi * [Pi]");
//            e.Parameters["X"] = 10;

//            e.EvaluateParameter += delegate (string name, ParameterArgs args)
//            {
//                if (name == "Pi")
//                    args.Result = 3.14;
//            };

//            for (int i = 0; i < 1000; i++)
//            {
//                string expression = GenerateExpression();
//                Expression e = new Expression()
//                if (!e.HasErrors())
//    //            {
//      //              Console.WriteLine(e.ParsedExpression.ToString());
//        //            Console.WriteLine(e.Evaluate());
                    

//          //      }


//            }

//            //YuriyGuts.RegexBuilder.RegexNode r = new RegexNodeCharacterSet("2017", false);
//            //r.Quantifier = RegexQuantifier.Exactly(1);
//            //var x = new RegexNodeCharacterSet(new char[] {'+', '-', '/', '*'},false);

//            //var zz = RegexBuilder.Build(r, x);
//            //Console.WriteLine(zz.ToString());

//            //RegexBuilder.PositiveLookAhead()
//            //RegexBuilder.PositiveLookAhead(new RegexNodeCharacterSet("2017",false),
//            //YuriyGuts.RegexBuilder.RegexBuilder.PositiveLookAhead(new look)
//            //    RegexNode.Add(new RegexNodeCharacterSet("2017",false)),
//            //var x = Regextra.PassphraseRegex.That.IncludesText("2017").ToRegex();

//            //Console.WriteLine(x.Pattern);
//            //Console.WriteLine(x.Regex.ToString());
//            //            Expression e = new Expression("2 + 3 * 5");
//            //           Console.WriteLine(e.Evaluate());
//        }
//    }

//}



////// Creating a parameter expression.  
////ParameterExpression value = Expression.Parameter(typeof(int), "value");

////// Creating an expression to hold a local variable.   
////ParameterExpression result = Expression.Parameter(typeof(int), "result");

////// Creating a label to jump to from a loop.  
////LabelTarget label = Expression.Label(typeof(int));

////// Creating a method body.  
////BlockExpression block = Expression.Block(
////    // Adding a local variable.  
////    new[] {result},
////    // Assigning a constant to a local variable: result = 1  
////    Expression.Assign(result, Expression.Constant(1)),
////    // Adding a loop.  
////    Expression.Loop(
////        // Adding a conditional block into the loop.  
////        Expression.IfThenElse(
////            // Condition: value > 1  
////            Expression.GreaterThan(value, Expression.Constant(1)),
////            // If true: result *= value --  
////            Expression.MultiplyAssign(result,
////                Expression.PostDecrementAssign(value)),
////            // If false, exit the loop and go to the label.  
////            Expression.Break(label, result)
////        ),
////        // Label to jump to.  
////        label
////    )
////);

////// Compile and execute an expression tree.  
////int factorial = Expression.Lambda<Func<int, int>>(block, value).Compile()(5);

////Console.WriteLine(factorial);
////}
////}
////}

////// Prints 120.  

//////            Regex emailRegex = RegexBuilder.Build
//////          (
//////                RegexBuilder
//////              RegexBuilder.MetaCharacter(RegexMetaChars.LineStart),
//////          // Match everythi
//////          RegexBuilder reg = new regexb
//////                string s = "0*(2+1+7)";
//////            char[] numbers = new[] {'2', '0', '1', '7'};
//////            char[] operands = new[] {'*', '-', '/', '^'};

//////            string vals = "()*-/+2017^";

//////            Expression e = new Expression("2 + 3 * 5");
//////            Console.WriteLine(e.Evaluate());
//////        }
//////    }
//////}

////
      