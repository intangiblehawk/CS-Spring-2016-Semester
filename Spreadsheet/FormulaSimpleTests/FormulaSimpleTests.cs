﻿// Written by Joe Zachary for CS 3500, January 2016.
// Repaired error in Evaluate5.  Added TestMethod Attribute
//    for Evaluate4 and Evaluate5 - JLZ January 25, 2016
// Corrected comment for Evaluate3 - JLZ January 29, 2016

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formulas;
using System.Collections.Generic;

namespace FormulaTestCases
{
    /// <summary>
    /// These test cases are in no sense comprehensive!  They are intended to show you how
    /// client code can make use of the Formula class, and to show you how to create your
    /// own (which we strongly recommend).  To run them, pull down the Test menu and do
    /// Run > All Tests.
    /// </summary>
    [TestClass]
    public class UnitTests
    {
        /// <summary>
        /// This tests that a syntactically incorrect parameter to Formula results
        /// in a FormulaFormatException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct1()
        {
            Formula f = new Formula("_");
        }

        /// <summary>
        /// This is another syntax error
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct2()
        {
            Formula f = new Formula("2++3");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct3()
        {
            Formula f = new Formula("2 3");
        }
        /// <summary>
        /// Syntax error where there is no formula
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct4()
        {
            Formula f = new Formula("");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct5()
        {
            Formula f = new Formula("-5");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct5a()
        {
            Formula f = new Formula("-5");
        }


        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct6()
        {
            Formula f = new Formula("5-");
        }
        /// <summary>
        /// Makes sure that "2+3" evaluates to 5.  Since the Formula
        /// contains no variables, the delegate passed in as the
        /// parameter doesn't matter.  We are passing in one that
        /// maps all variables to zero.
        /// </summary>
        [TestMethod]
        public void Evaluate1()
        {
            Formula f = new Formula("2+3");
            Assert.AreEqual(f.Evaluate(v => 0), 5.0, 1e-6);
        }

        /// <summary>
        /// The Formula consists of a single variable (x5).  The value of
        /// the Formula depends on the value of x5, which is determined by
        /// the delegate passed to Evaluate.  Since this delegate maps all
        /// variables to 22.5, the return value should be 22.5.
        /// </summary>
        [TestMethod]
        public void Evaluate2()
        {
            Formula f = new Formula("x5", normalizer1, validator1);
            Assert.AreEqual(f.Evaluate(v => 22.5), 22.5, 1e-6);
        }

        public string normalizer1(string s)
        {
            return s.ToUpper();
        }

        public bool validator1(string s)
        {
            return true;
        }

        /// <summary>
        /// Here, the delegate passed to Evaluate always throws a
        /// UndefinedVariableException (meaning that no variables have
        /// values).  The test case checks that the result of
        /// evaluating the Formula is a FormulaEvaluationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate3()
        {
            Formula f = new Formula("x + y");
            f.Evaluate(v => { throw new UndefinedVariableException(v); });
        }

        /// <summary>
        /// The delegate passed to Evaluate is defined below.  We check
        /// that evaluating the formula returns in 10.
        /// </summary>
        [TestMethod]
        public void Evaluate4()
        {
            Formula f = new Formula("x + y");
            Assert.AreEqual(f.Evaluate(Lookup4), 10.0, 1e-6);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate4a()
        {
            Formula f = new Formula("x + y", normalizer1, validator1);
            Assert.AreEqual(f.Evaluate(Lookup4), 10.0, 1e-6);
        }

        /// <summary>
        /// This uses one of each kind of token.
        /// </summary>
        [TestMethod]
        public void Evaluate5 ()
        {
            Formula f = new Formula("(x + y) * (z / x) * 1.0");
            Assert.AreEqual(f.Evaluate(Lookup4), 20.0, 1e-6);
        }

        [TestMethod]
        
        public void Evaluate6()
        {
            Formula f = new Formula("(x * x + y) * (z / y) * 1.0");
            Assert.AreEqual(f.Evaluate(Lookup4), 29.3333326, 1e-6);
        }

        [TestMethod]

        public void Evaluate6a()
        {
            Formula f = new Formula("(x * x + y) * (z / y) * 1.0");
            ISet<string> test = f.GetVariables();
            string[] test2 = new string[3] {"x","y","z"};
            Assert.AreEqual(f.Evaluate(Lookup4), 29.3333326, 1e-6);
            int i = 0;
            foreach(string var in test)
            {
                Assert.AreEqual(test2[i], var);
                i++;
            }
        }


        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate7()
        {
            Formula f = new Formula("5/0 + 1 -9");
            Assert.AreEqual(f.Evaluate(Lookup4), 0, 1e-6);
        }

        [TestMethod]
        public void Evaluate8()
        {
            Formula f = new Formula("1.0e9");
            Assert.AreEqual(f.Evaluate(Lookup4), 1000000000, 1e-6);
        }

        [TestMethod]
        public void Evaluate8a()
        {
            Formula f = new Formula("1.0e9");
            Formula t = new Formula(f.ToString());
            Assert.AreEqual(t.Evaluate(Lookup4), 1000000000, 1e-6);
        }



        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate9()
        {
            Formula f = new Formula("1.0 * e9");
            Assert.AreEqual(f.Evaluate(Lookup4), 1000000000, 1e-6);
        }


        [TestMethod]
        public void Evaluate10()
        {
            Formula f = new Formula();
            Assert.AreEqual(f.Evaluate(Lookup4),0, 1e-6);
        }

        [TestMethod]
        public void Evaluate11()
        {
            Formula f = new Formula();
            double test;
            Assert.IsTrue(double.TryParse(f.ToString(), out test));
            Assert.AreEqual(0.0,test);
        }

        [TestMethod]
        public void Evaluate12()
        {
            Formula f = new Formula();
            HashSet<string> test = new HashSet<string>(f.GetVariables());
            Assert.AreEqual(0, test.Count);
        }
        /// <summary>
        /// A Lookup method that maps x to 4.0, y to 6.0, and z to 8.0.
        /// All other variables result in an UndefinedVariableException.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>


        public double Lookup4(string v)
        {
            switch (v)
            {
                case "x": return 4.0;
                case "y": return 6.0;
                case "z": return 8.0;
                default: throw new UndefinedVariableException(v);
            }
        }
    }
}
