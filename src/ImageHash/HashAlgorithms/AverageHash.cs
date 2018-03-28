using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Filters;
using SixLabors.ImageSharp.Processing.Transforms;

namespace CoenM.ImageSharp.HashAlgorithms
{
    /// <summary>
    /// Average hash; Calculate a hash of an image based on visual characteristics by transforming the image to an 8x8 grayscale bitmap. 
    /// Hash is based on each pixel compared to the bitmaps average grayscale.
    /// </summary>
    /// <remarks>
    /// Implementation based on David Oftedal's implementation of Average Hash. Algorith specified by Dr. Neal Krawetz.
    /// See http://www.hackerfactor.com/blog/index.php?/archives/432-Looks-Like-It.html for more information.
    /// </remarks>
    public class AverageHash : IImageHash
    {
        private const int Width = 8;
        private const int Height = 8;
        private const int NrPixels = Width * Height;
        private const ulong MostSignificantBitMask = 1UL << (NrPixels - 1);


        /// <summary>
        /// Computes the average hash of an image.
        /// </summary>
        /// <param name="image">The image to hash.</param>
        /// <returns>The hash of the image.</returns>
        public ulong Hash(Image<Rgba32> image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            image.Mutate(ctx => ctx
                .Resize(Width, Height)
                .Grayscale(GrayscaleMode.Bt601)
                .AutoOrient()
            );

            uint averageValue = 0;

            var rawBytes = image.SavePixelData();
            for (var i = 0; i < NrPixels; i++)
            {
                // We know 4 bytes (RGBA) are used to describe one pixel
                // Also, it is already grayscaled, so R=G=B. Therefore, we can take one of these
                // values for average calculation. We take the R (the first of each 4 bytes).
                averageValue += rawBytes[i * 4];
            }

            averageValue /= NrPixels;

            // Compute the hash: each bit is a pixel
            // 1 = higher than average, 0 = lower than average
            var hash = 0UL;
            var mask = MostSignificantBitMask;

            for (var i = 0; i < NrPixels; i++)
            {
                if (rawBytes[i * 4] >= averageValue)
                    hash |= mask;

                mask = mask >> 1;
            }

            return hash;
        }
    }
}