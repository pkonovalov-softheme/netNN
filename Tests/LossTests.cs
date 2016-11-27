using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLib.CostsFunctions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class LossTests
    {
        [TestMethod]
        public void SquareErrorForward()
        {
            SquareError errorOp = new SquareError();
            double res = errorOp.ForwardPass(2, 5);
            Assert.AreEqual(9, res);

            res = errorOp.ForwardPass(5, 2);
            Assert.AreEqual(9, res);
        }

        [TestMethod]
        public void SquareErrorBackward()
        {
            CheckSquareInternal(2, 5);
            CheckSquareInternal(5, 2);
            CheckSquareInternal(2, 2);
            CheckSquareInternal(0, 0);
        }

        private static void CheckSquareInternal(double h, double t)
        {
            SquareError errorOp = new SquareError();
            double fa1 = errorOp.ComputeLossGradient(h, t);

            double f1Val = errorOp.ForwardPass(h + GradUtils.H, t);
            double f2Val = errorOp.ForwardPass(h - GradUtils.H, t);
            bool res = GradUtils.ChechAnalGrad(fa1, f1Val, f2Val);
            Assert.IsTrue(res);
        }
    }
}
