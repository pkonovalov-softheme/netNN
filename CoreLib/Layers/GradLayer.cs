namespace CoreLib.Layers
{
    public class GradLayer : ValueLayer
    {
        public Matrix Gradients { get; protected set; }

        protected GradLayer(int unitsCount) : base(unitsCount)
        {
            Gradients = new Matrix(unitsCount, 1);
        }
    }
}
