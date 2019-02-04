namespace Demo.Model
{
    using System.IO;
    using System.Runtime.CompilerServices;
    using CoenM.ImageHash.HashAlgorithms;
    using JetBrains.Annotations;

    public interface IImageHash
    {
        ulong CalculateAverageHash(string filename);

        ulong CalculateDifferenceHash(string filename);

        ulong CalculatePerceptualHash(string filename);
    }

    public class ImageHashAdapter : IImageHash
    {
        [NotNull] private readonly AverageHash averageHash;
        [NotNull] private readonly DifferenceHash differenceHash;
        [NotNull] private readonly PerceptualHash perceptualHash;

        private ImageHashAdapter()
        {
            averageHash = new AverageHash();
            differenceHash = new DifferenceHash();
            perceptualHash = new PerceptualHash();
        }
        public ulong CalculateAverageHash(string filename) => CoenM.ImageHash.ImageHashExtensions.Hash(averageHash, Open(filename));

        public ulong CalculateDifferenceHash(string filename) => CoenM.ImageHash.ImageHashExtensions.Hash(differenceHash, Open(filename));

        public ulong CalculatePerceptualHash(string filename) => CoenM.ImageHash.ImageHashExtensions.Hash(perceptualHash, Open(filename));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Stream Open(string filename) => System.IO.File.OpenRead(filename);
    }
}
