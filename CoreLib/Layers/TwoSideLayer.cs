namespace CoreLib.Layers
{
    public abstract class DoubleSideLayer : InputLayer
    {
        protected readonly DoubleSideLayer PrevLayer;

        protected DoubleSideLayer(int unitsCount, DoubleSideLayer prevLayer, DoubleSideLayer nextLayer) : base(unitsCount, nextLayer)
        {
            PrevLayer = prevLayer;
        }
    }
}
