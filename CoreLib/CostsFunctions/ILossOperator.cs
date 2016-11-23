namespace CoreLib.CostsFunctions
{
    public interface ILossOperator
    {
        double ForwardPass(double h, double target);

        double ComputeLossGradient(double h, double target);
    }
}
