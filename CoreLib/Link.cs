using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLib.Layers;

namespace CoreLib
{
    public class Link
    {
        public DoubleSideLayer NextLayer { get; set; }

        public DoubleSideLayer PreviousLayer { get; set; }
    }
}
