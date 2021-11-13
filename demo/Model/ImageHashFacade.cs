namespace Demo.Model
{
    using System;
    using CoenM.ImageHash.HashAlgorithms;

    public class ImageHashFacade : IDemoImageHash
    {
        private readonly IFileSystem _fileSystem;
        private readonly AverageHash _averageHash;
        private readonly DifferenceHash _differenceHash;
        private readonly PerceptualHash _perceptualHash;

        public ImageHashFacade(IFileSystem fileSystem)
        {
            this._fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _averageHash = new AverageHash();
            _differenceHash = new DifferenceHash();
            _perceptualHash = new PerceptualHash();
        }

        public ulong CalculateAverageHash(string filename) => CoenM.ImageHash.ImageHashExtensions.Hash(_averageHash, _fileSystem.OpenRead(filename));

        public ulong CalculateDifferenceHash(string filename) => CoenM.ImageHash.ImageHashExtensions.Hash(_differenceHash, _fileSystem.OpenRead(filename));

        public ulong CalculatePerceptualHash(string filename) => CoenM.ImageHash.ImageHashExtensions.Hash(_perceptualHash, _fileSystem.OpenRead(filename));
    }
}
