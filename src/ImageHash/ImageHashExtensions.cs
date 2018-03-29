namespace CoenM.ImageSharp
{
    using System;
    using System.IO;

    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.PixelFormats;

    /// <summary>
    /// Extension methods for IImageHash
    /// </summary>
    public static class ImageHashExtensions
    {
        /// <summary>Calculate the hash of the image (stream) using the hashImplementation.</summary>
        /// <param name="hashImplementation">HashImplementation to calculate the hash.</param>
        /// <param name="stream">Stream should 'contain' raw image data</param>
        /// <returns>hash value</returns>
        public static ulong Hash(this IImageHash hashImplementation, Stream stream)
        {
            if (hashImplementation == null)
                throw new ArgumentNullException(nameof(hashImplementation));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            using (var image = Image.Load<Rgba32>(stream))
                return hashImplementation.Hash(image);
        }
    }
}