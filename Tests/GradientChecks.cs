using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.PerformanceData;
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
        //const int Seed = 1;
        Random _rnd = new Random();

        //GeneralSettings.GradientsTracingEnabled = true;
        //GeneralSettings.ValuesTracingEnabled = true;

        private Model InitSimpleModel(double initValue)
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
        public void NumericalGradientCheck()
        {
            const double inputValue = 5;
            const int passCount = 10;
            const double h = 1e-5;
            const double gradValue = 1;

            Model model = InitSimpleModel(inputValue);

            for (int i = 0; i < passCount; i++)
            {
                model.FirstInputValue = Utils.GetRandomNumber(_rnd, 0.05, 10);
                model.ForwardPass();

                //double curValue = model.OutputLayer.Values[0, 0];
                model.OutputLayer.Gradients[0, 0] = gradValue;
                model[0].ComputeGradient();

                double fa = model[0].Gradients[0, 0]; // Analytical gradient - dy/dw

                double initWeight = model[0].Weights[0, 0];
                model[0].Weights[0, 0] = initWeight + h;
                model.ForwardPass();
                double f1Val = model.FirstOutputValue;

                model[0].Weights[0, 0] = initWeight - h;
                model.ForwardPass();
                double f2Val = model.FirstOutputValue;

                double fn = (f1Val - f2Val) / (2 * h); // Numerical gradient
                double relativeError = Math.Abs(fa - fn)/Math.Max(Math.Abs(fa), Math.Abs(fn));

                // relative error > 1e-2 usually means the gradient is probably wrong
                // 1e-2 > relative error > 1e-4 should make you feel uncomfortable
                // 1e-4 > relative error is usually okay for objectives with kinks.But if there are no kinks(e.g.use of tanh nonlinearities and softmax), then 1e-4 is too high.
                // 1e-7 and less you should be happy.

                Assert.IsTrue(relativeError < 1e-4);
                model[0].ApplyGradient();
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
