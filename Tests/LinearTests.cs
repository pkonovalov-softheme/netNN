using System;
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
    public class LinearTests
    {
        Random _rnd = new Random();

        private Model InitSimpleModel(double initValue)
        {
            Model model = new Model(1, ActivationType.ReLU, 1, ActivationType.ReLU, CostType.Abs);
            model.FirstInputValue = initValue;
            model.AddAffineLayer(1, ActivationType.Identity);
            model.InitWithRandomValues(_rnd);
            return model;
        }

        [TestMethod]
        public void PositiveGradMakeOutputSmaller()
        {
            Model model = InitSimpleModel(_rnd.Next(1, 15));

            for (int i = 0; i < 200; i++)
            {
            }
        }
     }
}
