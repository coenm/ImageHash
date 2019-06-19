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
        private const int Width = 9;
        private const int Height = 8;

        /// <inheritdoc />
        public ulong Hash(Image<Rgba32> image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            // We first auto orient because with and height differ.
            image.Mutate(ctx => ctx
                                .AutoOrient()
                                .Resize(Width, Height)
                                .Grayscale(GrayscaleMode.Bt601));

            var mask = 1UL << ((Height * (Width - 1)) - 1);
            var hash = 0UL;

            for (var y = 0; y < Height; y++)
            {
                var row = image.GetPixelRowSpan(y);
                var leftPixel = row[0];

                for (var x = 1; x < Width; x++)
                {
                    var rightPixel = row[x];
                    if (leftPixel.R < rightPixel.R)
                        hash |= mask;

                    leftPixel = rightPixel;
                    mask >>= 1;
                }
            }

            return hash;
        }
    }
}
