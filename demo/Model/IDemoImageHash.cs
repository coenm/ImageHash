namespace Demo.Model
{
    public interface IDemoImageHash
    {
        ulong CalculateAverageHash(string filename);

        ulong CalculateDifferenceHash(string filename);

        ulong CalculatePerceptualHash(string filename);
    }
}
