namespace CoenM.ImageHash.HashAlgorithms
{
    using System;
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Advanced;
    using SixLabors.ImageSharp.PixelFormats;
    using SixLabors.ImageSharp.Processing;

    /// <summary>
    /// Difference hash; Calculate a hash of an image based on visual characteristics by transforming the image to an 9x8 grayscale bitmap.
    /// Hash is based on each pixel compared to it's right neighbor pixel.
    /// </summary>
    /// <remarks>
    /// Algorithm specified by David Oftedal and slightly adjusted by Dr. Neal Krawetz.
    /// See <see href="http://www.hackerfactor.com/blog/index.php?/archives/529-Kind-of-Like-That.html"/> for more information.
    /// </remarks>
    // ReSharper disable once StyleCop.SA1650
    public class DifferenceHash : IImageHash
    {
        private const int WIDTH = 9;
        private const int HEIGHT = 8;

        /// <inheritdoc />
        public ulong Hash(Image<Rgba32> image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            // We first auto orient because with and height differ.
            image.Mutate(ctx => ctx
                                .AutoOrient()
                                .Resize(WIDTH, HEIGHT)
                                .Grayscale(GrayscaleMode.Bt601));

            var hash = 0UL;

            image.ProcessPixelRows((imageAccessor) =>
                {
                    var mask = 1UL << ((HEIGHT * (WIDTH - 1)) - 1);

                    for (var y = 0; y < HEIGHT; y++)
                    {
                        Span<Rgba32> row = imageAccessor.GetRowSpan(y);
                        Rgba32 leftPixel = row[0];

                        for (var index = 1; index < WIDTH; index++)
                        {
                            Rgba32 rightPixel = row[index];
                            if (leftPixel.R < rightPixel.R)
                            {
                                hash |= mask;
                            }

                            leftPixel = rightPixel;
                            mask >>= 1;
                        }
                    }
                });

            return hash;
        }
    }
}
