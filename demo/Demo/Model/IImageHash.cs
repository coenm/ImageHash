namespace Demo.Model
{
    public interface IImageHash
    {
        ulong CalculateAverageHash(string filename);

        ulong CalculateDifferenceHash(string filename);

        ulong CalculatePerceptualHash(string filename);
    }
}
