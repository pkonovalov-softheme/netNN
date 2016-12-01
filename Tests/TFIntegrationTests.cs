using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLib;
using CoreLib.CostsFunctions;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class TfIntegrationTests
    {
        const string DumpFolder = @"D:\netNN\dump\";
        const string Path = @"D:\netNN\TFwrapper\TFwrapper.py";
        const double Epsilon = 0.00001;

        //GeneralSettings.GradientsTracingEnabled = true;
        //GeneralSettings.ValuesTracingEnabled = true;

        //ScriptEngine engine = Python.CreateEngine();
        //engine.Execute("import tensorflow as tf");
        //Assert.AreEqual(9, 9);

        [TestMethod]
        public void TestRandomNetworkConfig()
        {
            const int passCount = 10;
            const int configsCount = 30;
            int seed = DateTime.Now.Millisecond;
            Random rnd = new Random(seed);

            for (int i = 0; i < configsCount; i++)
            {
                double init_w1 = rnd.NextDouble() * rnd.Next(-300, 300);
                double init_w2 = rnd.NextDouble() * rnd.Next(-300, 300);
                double init_b1 = rnd.NextDouble() * rnd.Next(-300, 300);
                double init_b2 = rnd.NextDouble() * rnd.Next(-300, 300);
                double initValue = rnd.NextDouble() * rnd.Next(-100, 100);
                double targetValue = rnd.NextDouble() * rnd.Next(-100, 100);

                TestWeightsAndBiases(init_w1, init_b1, init_w2, init_b2, passCount, initValue, targetValue);
                Debug.WriteLine("Pass {0} from {1} ", i, passCount);
            }
        }


        [TestMethod]
        public void CheckParamsInTwoUnitsNetwork_1()
        {
            const double init_w1 = 1;
            const double init_b1 = 1;
            const double init_w2 = 1;
            const double init_b2 = 1;

            const double initValue = 10;
            const double targetValue = 10;
            const int passCount = 10;

            TestWeightsAndBiases(init_w1, init_b1, init_w2, init_b2, passCount, initValue, targetValue);
        }

        [TestMethod]
        public void CheckParamsInTwoUnitsNetwork_2()
        {
            const double init_w1 = 5.3;
            const double init_b1 = 0.15;
            const double init_w2 = 6.7;
            const double init_b2 = 8.3;

            const double initValue = 150.6;
            const double targetValue = 8.3;
            const int passCount = 10;

            TestWeightsAndBiases(init_w1, init_b1, init_w2, init_b2, passCount, initValue, targetValue);
        }

        [TestMethod]
        public void CheckParamsInTwoUnitsNetwork_3()
        {
            const double init_w1 = -20.3;
            const double init_b1 = 34.65;
            const double init_w2 = 6.7;
            const double init_b2 = -18.5;

            const double initValue = 150.6;
            const double targetValue = 8.3;
            const int passCount = 10;

            TestWeightsAndBiases(init_w1, init_b1, init_w2, init_b2, passCount, initValue, targetValue);
        }

        private static void TestWeightsAndBiases(double init_w1, double init_b1, double init_w2, double init_b2, int passCount,
            double initValue, double targetValue)
        {
            List<double> list = new List<double> {init_w1, init_b1, init_w2, init_b2, passCount, initValue, targetValue};

            List<string> strList = list.Select(curWar => curWar.ToString(CultureInfo.InvariantCulture)).ToList();

            Utils.InvokePythonScript(Path, strList.ToArray());

            List<double> w1_dump = Utils.CsvToDoubles(DumpFolder + "W1_dump.csv");
            List<double> b1_dump = Utils.CsvToDoubles(DumpFolder + "B1_dump.csv");

            List<double> w2_dump = Utils.CsvToDoubles(DumpFolder + "W2_dump.csv");
            List<double> b2_dump = Utils.CsvToDoubles(DumpFolder + "B2_dump.csv");

            Model model = new Model(1, ActivationType.ReLU, 1, ActivationType.ReLU, CostType.Square);
            model.FirstInputValue = initValue;

            Initialiser.InitWithConstValue(model[0].Weights.Primal, init_w1);
            Initialiser.InitWithConstValue(model[0].Biases.Primal, init_b1);
            Initialiser.InitWithConstValue(model[1].Weights.Primal, init_w2);
            Initialiser.InitWithConstValue(model[1].Biases.Primal, init_b2);

            for (int i = 0; i < passCount; i++)
            {
                model.ForwardPass(new Matrix(initValue));

                Matrix target = new Matrix(targetValue);
                model.BackwardPass(target);

                double w1 = model[0].Weights.Primal[0, 0];
                double b1 = model[0].Biases.Primal[0, 0];

                double w2 = model[1].Weights.Primal[0, 0];
                double b2 = model[1].Biases.Primal[0, 0];

                AssertAreNearlyEqual(w1, w1_dump[i]);
                AssertAreNearlyEqual(b1, b1_dump[i]);
                AssertAreNearlyEqual(w2, w2_dump[i]);
                AssertAreNearlyEqual(b2, b2_dump[i]);
            }
        }

        private static void AssertAreNearlyEqual(double val1, double val2)
        {
            Assert.IsTrue(Utils.NearlyEqual(val1, val2, Epsilon));
        }
    }
}
