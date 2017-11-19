using System.IO;

namespace CoenM.ImageSharp
{
    /// <summary>
    /// Interface for perceptual image hashing algorithm
    /// </summary>
    public interface IImageHash
    {
        /// <summary>
        /// Hash the image using the algorithm.
        /// </summary>
        /// <param name="stream">Stream should 'contain' raw image data.</param>
        /// <returns>hash value of the image.</returns>
        ulong Hash(Stream stream);
    }
}