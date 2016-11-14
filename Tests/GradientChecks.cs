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
        public void TestSingleGradFlow()
        {
            const int passCount = 100;
            double targetY = 10;
            double prevAbsDif = 0;

            Model model = new Model(1, 1);
            model.InputLayer.Values[0, 0] = 0.02; 
            model.AddAffineLayer(1, ActivationType.ReLU);
            model.InitWithRandomWeights();

            for (int i = 0; i < passCount; i++)
            {
                model.ForwardPass();

                double absDif = Math.Abs(model[0].Values[0, 0] - targetY);
                model.OutputLayer.Gradients[0, 0] = absDif;

                if (prevAbsDif > 0)
                {
                    Assert.IsTrue(prevAbsDif > absDif);
                }

                prevAbsDif = absDif;
                model[0].BackwardPass();
            }

        }
    }
}
