namespace Demo.Model
{
    public interface IImageHashSimilarityCalculator
    {
        double Calculate(ulong imageHash1, ulong imageHash2);
    }
}
