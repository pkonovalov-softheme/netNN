namespace CoreLib.Layers
{
    public class OutputLayer : GradLayer
    {
        private readonly DoubleSideLayer _prevLayer;

        public OutputLayer(int unitsCount, DoubleSideLayer prevLayer) : base(unitsCount)
        {
            _prevLayer = prevLayer;
        }
    }
}
