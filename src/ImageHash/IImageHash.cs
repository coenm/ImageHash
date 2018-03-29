namespace CoenM.ImageSharp
{
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.PixelFormats;

    /// <summary>
    /// Interface for perceptual image hashing algorithm
    /// </summary>
    public interface IImageHash
    {
        /// <summary>Hash the image using the algorithm.</summary>
        /// <param name="image">image to calculate hash from</param>
        /// <returns>hash value of the image.</returns>
        ulong Hash(Image<Rgba32> image);
    }
}