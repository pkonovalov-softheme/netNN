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
        }

        public override void ForwardPass()
        {
            throw new InvalidOperationException();
        }

        public override void BackwardPass()
        {
            throw new InvalidOperationException();
        }

        public new Matrix Values => PrevLayer.Values;

        private new DoubleSideLayer NextLayer
        {
            get { throw new InvalidOperationException("There is no NextLayer for output layer."); }
            set { throw new InvalidOperationException("There is no NextLayer for output layer."); }
        }
    }
}
