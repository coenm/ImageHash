namespace Demo.Model
{
    using System;

    using CoenM.ImageHash.HashAlgorithms;

    public class ImageHashFacade : IDemoImageHash
    {
        private readonly IFileSystem fileSystem;
        private readonly AverageHash averageHash;
        private readonly DifferenceHash differenceHash;
        private readonly PerceptualHash perceptualHash;

        public ImageHashFacade(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            averageHash = new AverageHash();
            differenceHash = new DifferenceHash();
            perceptualHash = new PerceptualHash();
        }

        public ulong CalculateAverageHash(string filename) => CoenM.ImageHash.ImageHashExtensions.Hash(averageHash, fileSystem.OpenRead(filename));

        public ulong CalculateDifferenceHash(string filename) => CoenM.ImageHash.ImageHashExtensions.Hash(differenceHash, fileSystem.OpenRead(filename));

        public ulong CalculatePerceptualHash(string filename) => CoenM.ImageHash.ImageHashExtensions.Hash(perceptualHash, fileSystem.OpenRead(filename));
    }
}
