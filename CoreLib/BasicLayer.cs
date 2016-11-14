using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib
{
    public abstract class BasicLayer
    {
        public Matrix Values { get; protected set; }

        protected BasicLayer(int unitsCount)
        {
            Values = new Matrix(unitsCount, 1);
        }
    }
}
