using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLib;
using CoreLib.Layers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class GradientChecks
    {

        [TestMethod]
        public void TestForwardPassIsZero()
        {
            Model model = new Model(1, 1);
            model.InputLayer.Values[0, 0] = 4;
            model.AddAffineLayer(1, new ReLU());
            Assert.AreEqual(affineLayer.Values[0, 0], 0);
        }

        [TestMethod]
        public void TestSingleGradFlow()
        {
            const int passCount = 100;
            double targetY = 10;
            double prevAbsDif = 0;


            Model model = new Model(1, 1);
            AffineLayer affineLayer = new AffineLayer(1, new ReLU());

            OutputLayer outputLayer = new OutputLayer(1);

            for (int i = 0; i < passCount; i++)
            {
                affineLayer.ForwardPass();

                double absDif = Math.Abs(affineLayer.Values[0, 0] - targetY);
                outputLayer.Gradients[0, 0] = absDif;

                if (prevAbsDif > 0)
                {
                    Assert.IsTrue(prevAbsDif > absDif);
                }

                prevAbsDif = absDif;

                affineLayer.BackwardPass();
            }

        }
    }
}
