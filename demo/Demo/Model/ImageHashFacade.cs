namespace Demo.Model
{
    using System;

    using CoenM.ImageHash.HashAlgorithms;
    using JetBrains.Annotations;

    public class ImageHashFacade : IDemoImageHash
    {
        [NotNull] private readonly IFileSystem fileSystem;
        [NotNull] private readonly AverageHash averageHash;
        [NotNull] private readonly DifferenceHash differenceHash;
        [NotNull] private readonly PerceptualHash perceptualHash;

        public ImageHashFacade([NotNull] IFileSystem fileSystem)
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
