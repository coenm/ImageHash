namespace CoenM.ImageHash.HashAlgorithms
{
    using System;

    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Advanced;
    using SixLabors.ImageSharp.PixelFormats;
    using SixLabors.ImageSharp.Processing;

    /// <summary>
    /// Average hash; Calculate a hash of an image based on visual characteristics by transforming the image to an 8x8 grayscale bitmap.
    /// Hash is based on each pixel compared to the bitmaps average grayscale.
    /// </summary>
    /// <remarks>
    /// Implementation based on David Oftedal's implementation of Average Hash. Algorith specified by Dr. Neal Krawetz.
    /// See http://www.hackerfactor.com/blog/index.php?/archives/432-Looks-Like-It.html for more information.
    /// </remarks>
    // ReSharper disable once StyleCop.SA1650
    public class AverageHash : IImageHash
    {
        private const int WIDTH = 8;
        private const int HEIGHT = 8;
        private const int NR_PIXELS = WIDTH * HEIGHT;
        private const ulong MOST_SIGNIFICANT_BIT_MASK = 1UL << (NR_PIXELS - 1);

        /// <inheritdoc />
        public ulong Hash(Image<Rgba32> image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            image.Mutate(ctx => ctx
                                .Resize(WIDTH, HEIGHT)
                                .Grayscale(GrayscaleMode.Bt601)
                                .AutoOrient());

            uint averageValue = 0;

            for (var y = 0; y < HEIGHT; y++)
            {
                var row = image.GetPixelRowSpan(y);
                for (var x = 0; x < WIDTH; x++)
                {
                    // We know 4 bytes (RGBA) are used to describe one pixel
                    // Also, it is already grayscaled, so R=G=B. Therefore, we can take one of these
                    // values for average calculation. We take the R (the first of each 4 bytes).
                    averageValue += row[x].R;
                }
            }

            averageValue /= NR_PIXELS;

            // Compute the hash: each bit is a pixel
            // 1 = higher than average, 0 = lower than average
            var hash = 0UL;
            var mask = MOST_SIGNIFICANT_BIT_MASK;

            for (var y = 0; y < HEIGHT; y++)
            {
                var row = image.GetPixelRowSpan(y);
                for (var x = 0; x < WIDTH; x++)
                {
                    if (row[x].R >= averageValue)
                        hash |= mask;

                    mask = mask >> 1;
                }
            }

            return hash;
        }
    }
}