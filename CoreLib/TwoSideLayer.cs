using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib
{
    public abstract class DoubleSideLayer : BasicLayer
    {
        public Matrix Gradients { get; protected set; }

        protected DoubleSideLayer(int unitsCount) : base(unitsCount)
        {
            Gradients = new Matrix(unitsCount, 1);
        }
    }
}
