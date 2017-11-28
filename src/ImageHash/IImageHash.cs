using System.IO;
using SixLabors.ImageSharp;

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
        /// <param name="image">image</param>
        /// <returns>hash value of the image.</returns>
        ulong Hash(Image<Rgba32> image);
    }
}