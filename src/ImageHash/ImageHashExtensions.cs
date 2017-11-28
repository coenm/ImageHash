using System;
using System.IO;
using SixLabors.ImageSharp;

namespace CoenM.ImageSharp
{
    /// <summary>
    /// Extension methods for IImageHash
    /// </summary>
    public static class ImageHashExtensions
    {
        /// <summary>
        /// </summary>
        /// <param name="hashImplementation"></param>
        /// <param name="stream">Stream should 'contain' raw image data</param>
        /// <returns></returns>
        public static ulong Hash(this IImageHash hashImplementation, Stream stream)
        {
            if (hashImplementation == null)
                throw new ArgumentNullException(nameof(hashImplementation));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            using (var image = Image.Load<Rgba32>(stream))
                return hashImplementation.Hash(image);
        }


        /// <summary>
        /// </summary>
        /// <param name="hashImplementation"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] HashToBytes(this IImageHash hashImplementation, Stream stream)
        {
            if (hashImplementation == null)
                throw new ArgumentNullException(nameof(hashImplementation));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var result = hashImplementation.Hash(stream);
            return BitConverter.GetBytes(result);
        }
    }
}