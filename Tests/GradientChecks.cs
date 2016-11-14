using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class GradientChecks
    {

        [TestMethod]
        public void TestForwardPAssIsZero()
        {
         //   InputLayer layer = new InputLayer(1);
         //   layer.Values[0, 0] = 4;

            AffineLayer affineLayer = new AffineLayer(1, new ReLU());
            affineLayer.ForwardPass(layer);
            Assert.AreEqual(affineLayer.Values[0, 0], 0);
        }

        [TestMethod]
        public void TestSingleGradFlow()
        {
            const int passCount = 100;
            double targetY = 10;
            
            InputLayer inputLayer = new InputLayer(1);
            AffineLayer affineLayer = new AffineLayer(1, new ReLU());
            OutputLayer outputLayer = new OutputLayer(1);

            for (int i = 0; i < passCount; i++)
            {
                affineLayer.ForwardPass(inputLayer);

                double absDif = Math.Abs(affineLayer.Values[0, 0] - targetY);
                outputLayer.Gradients[0, 0] = absDif;


            }

        }
    }
}
