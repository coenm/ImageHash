using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace CoenM.ImageSharp.HashAlgorithms
{
    /// <summary>
    /// Difference hash; Calculate a hash of an image based on visual characteristics by transforming the image to an 9x8 grayscale bitmap. 
    /// Hash is based on each pixel compared to it's right neighbour pixel.
    /// </summary>
    /// <remarks>
    /// Algorith specified by David Oftedal and slightly adjusted by Dr. Neal Krawetz.
    /// See http://www.hackerfactor.com/blog/index.php?/archives/529-Kind-of-Like-That.html for more information.
    /// </remarks>
    public class DifferenceHash : IImageHash
    {
        private const int Width = 9;
        private const int Height = 8;

        /// <summary>
        /// Computes the diffefence hash of an image.
        /// </summary>
        /// <param name="stream">The image to hash.</param>
        /// <returns>The hash of the image.</returns>
        public ulong Hash(Stream stream)
        {
            using (var img = Image.Load<Rgba32>(stream))
            {
                img.Mutate(ctx => ctx.Resize(Width, Height).Grayscale(GrayscaleMode.Bt601));

                var mask = 1UL;
                var hash = 0UL;

                for (var y = 0; y < Height; y++)
                {
                    var leftPixel = img[0, y];
                    for (var x = 1; x < Width; x++)
                    {
                        var rightPixel = img[x, y];

                        if (leftPixel.R < rightPixel.R)
                            hash |= mask;

                        leftPixel = rightPixel;
                        mask = mask << 1;
                    }
                }

                return hash;
            }
        }
    } 
}