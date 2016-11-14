using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib
{
    public abstract class ValueLayer
    {
        public Matrix Values { get; protected set; }

        protected ValueLayer(int unitsCount)
        {
            Values = new Matrix(unitsCount, 1);
        }
    }
}
