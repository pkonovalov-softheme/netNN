using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLib.Layers;

namespace CoreLib
{
    public class Model
    {
        readonly LinkedList<DoubleSideLayer> _layers = new LinkedList<DoubleSideLayer>(); 

        public void AddLayer(DoubleSideLayer layer)
        {
            _layers.AddLast(layer);
            _layers.Last = 
        }
    }
}
