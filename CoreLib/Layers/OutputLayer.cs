using System;
using System.Collections.Generic;

namespace CoreLib.Layers
{
    public class OutputLayer : DoubleSideLayer
    {
        public OutputLayer(int unitsCount) : base(unitsCount)
        {
        }

        public new void SetListNode(LinkedListNode<DoubleSideLayer> layersListNode)
        {
            LayersListNode = layersListNode;
            Values = PrevLayer.Values;
        }

        private new DoubleSideLayer NextLayer
        {
            get { throw new InvalidOperationException("There is no PrevLayer for input layer"); }
            set { throw new InvalidOperationException("There is no PrevLayer for input layer"); }
        }
    }
}
