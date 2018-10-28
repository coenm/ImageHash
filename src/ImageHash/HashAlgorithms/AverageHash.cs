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
    /// Implementation based on David Oftedal's implementation of Average Hash. Algorithm specified by Dr. Neal Krawetz.
    /// See <see href="http://www.hackerfactor.com/blog/index.php?/archives/432-Looks-Like-It.html"/> for more information.
    /// </remarks>
    // ReSharper disable once StyleCop.SA1650
    public class AverageHash : IImageHash
    {
        private const int Width = 8;
        private const int Height = 8;
        private const int NrPixels = Width * Height;
        private const ulong MostSignificantBitMask = 1UL << (NrPixels - 1);

        /// <inheritdoc />
        public ulong Hash(Image<Rgba32> image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            image.Mutate(ctx => ctx
                                .Resize(Width, Height)
                                .Grayscale(GrayscaleMode.Bt601)
                                .AutoOrient());

            uint averageValue = 0;

            for (var y = 0; y < Height; y++)
            {
                var row = image.GetPixelRowSpan(y);
                for (var x = 0; x < Width; x++)
                {
                    // We know 4 bytes (RGBA) are used to describe one pixel
                    // Also, it is already grayscaled, so R=G=B. Therefore, we can take one of these
                    // values for average calculation. We take the R (the first of each 4 bytes).
                    averageValue += row[x].R;
                }
            }

            averageValue /= NrPixels;

            // Compute the hash: each bit is a pixel
            // 1 = higher than average, 0 = lower than average
            var hash = 0UL;
            var mask = MostSignificantBitMask;

            for (var y = 0; y < Height; y++)
            {
                var row = image.GetPixelRowSpan(y);
                for (var x = 0; x < Width; x++)
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
