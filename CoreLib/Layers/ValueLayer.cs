namespace CoreLib.Layers
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
