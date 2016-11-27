using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLib;
using CoreLib.CostsFunctions;
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

        private Model InitSimpleModel(double initValue, CostType costType = CostType.Abs)
        {
            int seed = DateTime.Now.Millisecond;
            Trace.WriteLine("Seed " + seed);
            _rnd = new Random(seed);

            Model model = new Model(1, ActivationType.ReLU, 1, ActivationType.ReLU, costType);
            model.FirstInputValue = initValue;
            model.InitWithRandomValues(_rnd);

            Trace.WriteLine("Init value: " + initValue);
            Trace.WriteLine("Weights: " + Environment.NewLine + model[0].Weights.Primal);
            return model;
        }

        [TestMethod]
        public void UnderShootingMakeOutputLarger()
        {
            for (int j = 0; j < 5; j++)
            {
                const int passCount = 10;
                double prevValue = 0;
                bool firstRun = true;

                double initValue = _rnd.Next(1, 15);
                Model model = InitSimpleModel(initValue);

                for (int i = 0; i < passCount; i++)
                {
                    model.ForwardPass(new Matrix(0));

                    double curValue = model.FirstOutputValue;

                    if (!firstRun)
                    {
                        Assert.IsTrue(curValue == 0 || curValue > prevValue, "Failed on " + i + " try");
                    }

                    firstRun = false;

                    prevValue = curValue;
                    var target = new Matrix(curValue + 1);
                    model.BackwardPass(target);
                }
            }
         }

        [TestMethod]
        public void OverShootingMakeOutputSmaller()
        {
            const int passCount = 10;
            double prevValue = 0;
            bool firstRun = true;

            Model model = InitSimpleModel(_rnd.Next(1, 15));

            for (int i = 0; i < passCount; i++)
            {
                model.ForwardPass(new Matrix(0));

                double curValue = model.FirstOutputValue;

                if (!firstRun)
                {
                    Assert.IsTrue(curValue == 0 || curValue < prevValue, "Failed on " + i + " try");

                }

                firstRun = false;

                prevValue = curValue;

                Matrix target = new Matrix(curValue - 1);
                model.BackwardPass(target);
            }
        }

        [TestMethod]
        public void NumericalGradientCheckAbs()
        {
            Model model = InitSimpleModel(0, CostType.Abs);
            model.InitWithConstWeights(0.5);
            NumericGradCheckInternal(model);
        }

        [TestMethod]
        public void NumericalGradientCheckSqr()
        {
            Model model = InitSimpleModel(0, CostType.Square);
            model.InitWithConstWeights(0.5);
            NumericGradCheckInternal(model);
        }

        private void NumericGradCheckInternal(Model model)
        {
            const int passCount = 10;

            for (int i = 0; i < passCount; i++)
            {
                model.FirstInputValue = 4;
                //model.FirstInputValue = Utils.GetRandomNumber(_rnd, 0.05, 10);
                Matrix target = new Matrix(2*model.FirstInputValue);

                model.ForwardPass(target);

                model.LossLayer.ComputeLossGradients(target);
                model[1].ComputeGradient();
                model[0].ComputeGradient();

                double fa = model[0].Weights.Extra[0, 0]; // Analytical gradient - dy/dw

                double initWeight = model[0].Weights.Primal[0, 0];
                model[0].Weights.Primal[0, 0] = initWeight + GradUtils.H;
                model.ForwardPass(target);
                double f1Val = model.FirstLossValue;

                model[0].Weights.Primal[0, 0] = initWeight - GradUtils.H;
                model.ForwardPass(target);
                double f2Val = model.FirstLossValue;

                Assert.IsTrue(GradUtils.ChechAnalGrad(fa, f1Val, f2Val), "Failed on " + i + " try");
                model[1].ApplyGradient();
                model[0].ApplyGradient();
            }
        }


        [TestMethod]
        public void SingleGradFlow()
        {
            const int passCount = 10;
            double targetY = 10;
            double prevAbsDif = 0;
            double prevValue = 0;

            for (int k = 0; k < 10; k++)
            {
                bool firstRun = true;
                prevAbsDif = 0;
                prevValue = 0;

                Model model = InitSimpleModel(_rnd.Next(1, 15));
                model.FirstInputValue = 12;
                model[0].Weights.Primal[0, 0] = 1.70257318629072;
                model[0].Biases.Primal[0, 0] = 1.70257318629072;

                for (int i = 0; i < passCount; i++)
                {
                    model.ForwardPass(new Matrix(0));

                    double curValue = model.FirstOutputValue;
                    double absDif = Math.Abs(curValue - targetY);
                    model.LossLayer.Values.Extra[0, 0] = -absDif;

                    Trace.WriteLine("W = " + model[0].Weights.Primal[0, 0]);
                    Trace.WriteLine("B = " + model[0].Biases.Extra[0, 0]);
                    Trace.WriteLine("Loss = " + absDif);

                    if (!firstRun)
                    {
                        if (!(prevAbsDif > 0 && prevAbsDif > absDif))
                        {
                            Console.WriteLine(1);
                        }
                        //Assert.IsTrue(prevAbsDif > 0 && prevAbsDif > absDif);
                    }

                    firstRun = false;

                    prevAbsDif = absDif;
                    prevValue = curValue;
                    model[0].BackwardPass();
                }
            }
        }
    }
}
