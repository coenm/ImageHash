namespace CoenM.ImageSharp.HashAlgorithms
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.PixelFormats;
    using SixLabors.ImageSharp.Processing;

    /// <summary>
    /// Perceptual hash; Calculate a hash of an image by first transforming the image to an 64x64 grayscale bitmap and then using the Discrete cosine transform to remove the high frequencies.
    /// </summary>
    public class PerceptualHash : IImageHash
    {
        private const int SIZE = 64;
        private static readonly double Sqrt2DivSize = Math.Sqrt((double)2 / SIZE);
        private static readonly double Sqrt2 = 1 / Math.Sqrt(2);

        /// <inheritdoc />
        public ulong Hash(Image<Rgba32> image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            var rows = new double[SIZE][];
            var sequence = new double[SIZE];
            var matrix = new double[SIZE][];

            image.Mutate(ctx => ctx
                                .Resize(SIZE, SIZE)
                                .Grayscale(GrayscaleMode.Bt601)
                                .AutoOrient());

            // Calculate the DCT for each row.
            for (var y = 0; y < SIZE; y++)
            {
                for (var x = 0; x < SIZE; x++)
                    sequence[x] = image[x, y].R;

                rows[y] = Dct1D(sequence);
            }

            // Calculate the DCT for each column.
            for (var x = 0; x < SIZE; x++)
            {
                for (var y = 0; y < SIZE; y++)
                    sequence[y] = rows[y][x];

                matrix[x] = Dct1D(sequence);
            }

            // Only use the top 8x8 values.
            var top8X8 = new List<double>(SIZE);
            for (var y = 0; y < 8; y++)
            for (var x = 0; x < 8; x++)
                top8X8.Add(matrix[y][x]);

            var topRight = top8X8.ToArray();

            // Get Median.
            var median = CalculateMedian64Values(topRight);

            // Calculate hash.
            var mask = 1UL << SIZE - 1;
            var hash = 0UL;

            for (var i = 0; i < SIZE; i++)
            {
                if (topRight[i] > median)
                    hash |= mask;

                mask = mask >> 1;
            }

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double CalculateMedian64Values(IReadOnlyCollection<double> values)
        {
            Debug.Assert(values.Count == 64, "This DCT method works with 64 doubles.");
            return values.OrderBy(value => value).Skip(31).Take(2).Average();
        }

        /// <summary>
        /// One dimensional Discrete Cosine Transformation
        /// </summary>
        /// <param name="values">Should be an array of doubles of length 64</param>
        /// <returns>array of doubles of length 64</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double[] Dct1D(IReadOnlyList<double> values)
        {
            Debug.Assert(values.Count == 64, "This DCT method works with 64 doubles.");
            var coefficients = new double[SIZE];

            for (var coef = 0; coef < SIZE; coef++)
            {
                for (var i = 0; i < SIZE; i++)
                    coefficients[coef] += values[i] * Math.Cos((2.0 * i + 1.0) * coef * Math.PI / (2.0 * SIZE));

                coefficients[coef] *= Sqrt2DivSize;
                if (coef == 0)
                    coefficients[coef] *= Sqrt2;
            }

            return coefficients;
        }
    }
}