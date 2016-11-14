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
    public class DataFlowTests
    {
        [TestMethod]
        public void ForwardPassedZero()
        {
            Model model = new Model(1, 1);
            model.InputLayer.Values[0, 0] = 4;
            model.AddAffineLayer(1, ActivationType.ReLU);
            model.ForwardPass();
            Assert.AreEqual(model[0].Values[0, 0], 0);
        }

        [TestMethod]
        public void ForwardValueFlow_1Layer()
        {
            Model model = new Model(1, 1);
            model.InputLayer.Values[0, 0] = 4;
            model.AddAffineLayer(1, ActivationType.ReLU);
            Initialiser.InitWithConstValue(model[0].Weights, 1);

            model.ForwardPass();
            Assert.AreEqual(model.OutputLayer.Values[0, 0], 4);
        }

        [TestMethod]
        public void ForwardValueFlow_2Layers()
        {
            Model model = new Model(1, 1);
            model.InputLayer.Values[0, 0] = 4;
            model.AddAffineLayer(1, ActivationType.ReLU);
            model.AddAffineLayer(1, ActivationType.ReLU);

            Initialiser.InitWithConstValue(model[0].Weights, 1);
            Initialiser.InitWithConstValue(model[1].Weights, 1);


            model.ForwardPass();
            Assert.AreEqual(model.OutputLayer.Values[0, 0], 4);
        }
    }
}
