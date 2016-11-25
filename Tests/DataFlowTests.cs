﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLib;
using CoreLib.CostsFunctions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class DataFlowTests
    {
        [TestMethod]
        public void ForwardPassedZero()
        {
            Model model = new Model(1, ActivationType.ReLU, 1, ActivationType.ReLU, CostType.Abs);
            model.FirstInputValue = 4;
            model.ForwardPass();
            Assert.AreEqual(0, model.FirstOutputValue);
        }

        [TestMethod]
        public void ForwardValueFlow_1Layer_N1()
        {
            Model model = new Model(1, ActivationType.ReLU, 1, ActivationType.ReLU, CostType.Abs);
            model.InputLayer.Values[0, 0] = 4;
            model.InitWithConstWeights(1);

            model.ForwardPass();
            Assert.AreEqual(model.OutputLayer.Values[0, 0], 4);
        }

        [TestMethod]
        public void ForwardValueFlow_1Layer_N2()
        {
            Model model = new Model(1, ActivationType.ReLU, 1, ActivationType.ReLU, CostType.Abs);
            model.InputLayer.Values[0, 0] = 4;

            model.InitWithConstWeights(0.5);

            model.ForwardPass();
            Assert.AreEqual(1, model.OutputLayer.Values[0, 0]);
        }

        [TestMethod]
        public void ForwardValueFlow_1Layer_N3()
        {
            Model model = new Model(1, ActivationType.ReLU, 1, ActivationType.ReLU, CostType.Abs);
            model.InputLayer.Values[0, 0] = 4;
            model.InitWithConstWeights(2);

            model.ForwardPass();
            Assert.AreEqual(16, model.OutputLayer.Values[0, 0]);
        }

        [TestMethod]
        public void ForwardValueFlow_1Layer_N4()
        {
            Model model = new Model(1, ActivationType.ReLU, 1, ActivationType.ReLU, CostType.Abs);
            model.InputLayer.Values[0, 0] = 4;
            model.InitWithConstWeights(2);
            model[0].Biases[0, 0] = 3;
            //(4 * 2 + 3) * 2 
            model.ForwardPass();
            Assert.AreEqual(22, model.OutputLayer.Values[0, 0]);
        }

        [TestMethod]
        public void ForwardValueFlow_2Layers()
        {
            Model model = new Model(1, ActivationType.ReLU, 1, ActivationType.ReLU, CostType.Abs);
            model.InputLayer.Values[0, 0] = 4;

            Initialiser.InitWithConstValue(model[0].Weights, 1);
            Initialiser.InitWithConstValue(model[1].Weights, 2);

            model.ForwardPass();
            Assert.AreEqual(8, model.OutputLayer.Values[0, 0]);
        }
    }
}
