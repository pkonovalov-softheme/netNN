namespace CoreLib.Layers
{
    public abstract class BaseLayer
    {
        public Matrix Gradients { get; protected set; }

        public Matrix Values { get; protected set; }

        protected BaseLayer(int unitsCount)
        {
            Gradients = new Matrix(unitsCount, 1);
            Values = new Matrix(unitsCount, 1);
        }
    }
}
