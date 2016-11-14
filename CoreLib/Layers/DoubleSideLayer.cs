using System.Collections.Generic;

namespace CoreLib.Layers
{
    public abstract class DoubleSideLayer : GradLayer
    {
        private readonly Link _layersListNode;

        protected DoubleSideLayer(int unitsCount)
            : base(unitsCount)
        {
            _layersListNode = new Link();
        }

        public DoubleSideLayer NextLayer
        {
            get { return _layersListNode.NextLayer; }
            set { _layersListNode.PreviousLayer = value; }
        }

        public DoubleSideLayer PrevLayer
        {
            get { return _layersListNode.NextLayer; }
            set { _layersListNode.PreviousLayer = value; }
        }

        protected bool HasPrevLayer => _layersListNode.PreviousLayer != null;

        protected bool HasNextLayer => _layersListNode.NextLayer != null;
    }
}
