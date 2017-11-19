using System;
using System.IO;

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
        /// <param name="filename"></param>
        /// <returns></returns>
        public static ulong Hash(this IImageHash hashImplementation, string filename)
        {
            using (var stream = File.OpenRead(filename))
                return hashImplementation.Hash(stream);
        }


        /// <summary>
        /// </summary>
        /// <param name="hashImplementation"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static byte[] HashToBytes(this IImageHash hashImplementation, string filename)
        {
            var result = hashImplementation.Hash(filename);
            return BitConverter.GetBytes(result);
        }

        /// <summary>
        /// </summary>
        /// <param name="hashImplementation"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] HashToBytes(this IImageHash hashImplementation, Stream stream)
        {
            var result = hashImplementation.Hash(stream);
            return BitConverter.GetBytes(result);
        }
    }
}