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
        //GeneralSettings.GradientsTracingEnabled = true;
        //GeneralSettings.ValuesTracingEnabled = true;

        private Model InitSimpleModel(int initValue)
        {
            Model model = new Model(1, 1);
            model.InputLayer.Values[0, 0] = initValue;
            model.AddAffineLayer(1, ActivationType.ReLU);
            model.InitWithRandomWeights();
            return model;
        }

        [TestMethod]
        public void PositiveGradMakeOutputSmaller()
        {
            const int passCount = 10;
            double prevValue = 0;
            bool firstRun = true;

            Model model = InitSimpleModel(4);

            for (int i = 0; i < passCount; i++)
            {
                model.ForwardPass();

                double curValue = model.OutputLayer.Values[0, 0];
                model.OutputLayer.Gradients[0, 0] = 1;

                if (!firstRun)
                {
                    Assert.IsTrue(curValue < prevValue);
                }

                firstRun = false;

                prevValue = curValue;
                model[0].BackwardPass();
            }
        }

        [TestMethod]
        public void NegativeGradMakeOutputLarger()
        {
            const int passCount = 10;
            double prevValue = 0;
            bool firstRun = true;

            Model model = InitSimpleModel( 4);

            for (int i = 0; i < passCount; i++)
            {
                model.ForwardPass();

                double curValue = model.OutputLayer.Values[0, 0];
                model.OutputLayer.Gradients[0, 0] = -1;

                if (!firstRun)
                {
                    Assert.IsTrue(curValue > prevValue);
                }

                firstRun = false;

                prevValue = curValue;
                model[0].BackwardPass();
            }
        }


        [TestMethod]
        public void SingleGradFlow()
        {
            const int seed = 1;
            const int passCount = 100;
            double targetY = 10;
            double prevAbsDif = 0;
            double prevValue = 0;
            bool firstRun = true;

            GeneralSettings.GradientsTracingEnabled = true;
            GeneralSettings.ValuesTracingEnabled = true;

            Model model = new Model(1, 1);
            model.InputLayer.Values[0, 0] = 4;  //0.02;
            model.AddAffineLayer(1, ActivationType.ReLU);
            model.InitWithRandomWeights(seed);

            for (int i = 0; i < passCount; i++)
            {
                model.ForwardPass();

                double curValue = model.OutputLayer.Values[0, 0];
                double absDif =  targetY - curValue;
                // double absDif = Math.Abs(curValue - targetY);
                model.OutputLayer.Gradients[0, 0] = absDif;

                if (!firstRun)
                {
                    Assert.IsTrue(curValue > prevValue);
                    Assert.IsTrue(prevAbsDif > absDif);
                }

                firstRun = false;

                prevAbsDif = absDif;
                prevValue = curValue;
                model[0].BackwardPass();
            }

        }
    }
}
