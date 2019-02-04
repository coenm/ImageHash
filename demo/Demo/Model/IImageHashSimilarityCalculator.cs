namespace Demo.Model
{
    public interface IImageHashSimilarityCalculator
    {
        double Calculate(ulong imageHash1, ulong imageHash2);
    }

    public class ImageHashSimilarityCalculator : IImageHashSimilarityCalculator
    {
        public double Calculate(ulong imageHash1, ulong imageHash2)
        {
            return CoenM.ImageHash.CompareHash.Similarity(imageHash1, imageHash2);
        }
    }
}
