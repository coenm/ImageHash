using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CoenM.ImageSharp.HashAlgorithms
{
    /// <summary>
    /// </summary>
    public class PerceptualHash : IImageHash
    {
        private const int Size = 64;
        private static readonly double Sqrt2DivSize = Math.Sqrt((double)2 / Size);
        private static readonly double Sqrt2 = 1 / Math.Sqrt(2);

        /// <summary>
        /// Computes the perceptual hash of an image.
        /// </summary>
        /// <param name="image">The image to hash.</param>
        /// <returns>The hash of the image.</returns>
        public ulong Hash(Image<Rgba32> image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            var rows = new double[Size][];
            var sequence = new double[Size];
            var matrix = new double[Size][];

            image.Mutate(ctx => ctx
                .Resize(Size, Size)
                .Grayscale(GrayscaleMode.Bt601)
                .AutoOrient()
            );

            // Calculate the DCT for each row.
            for (var y = 0; y < Size; y++)
            {
                for (var x = 0; x < Size; x++)
                    sequence[x] = image[x, y].R;

                rows[y] = Dct1D(sequence);
            }

            // Calculate the DCT for each column.
            for (var x = 0; x < Size; x++)
            {
                for (var y = 0; y < Size; y++)
                    sequence[y] = rows[y][x];

                matrix[x] = Dct1D(sequence);
            }


            // Only use the top 8x8 values.
            var top8X8 = new List<double>(Size);
            for (var y = 0; y < 8; y++)
            for (var x = 0; x < 8; x++)
                top8X8.Add(matrix[y][x]);

            var topRight = top8X8.ToArray();


            // Get Median.
            var median = CalculateMedian64Values(topRight);


            // Calculate hash.
            var mask = 1UL << Size - 1;
            var hash = 0UL;

            for (var i = 0; i < Size; i++)
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
            Debug.Assert(values.Count == 64);
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

            var coefficients = new double[Size];

            for (var coef = 0; coef < Size; coef++)
            {
                for (var i = 0; i < Size; i++)
                    coefficients[coef] += values[i] * Math.Cos((2.0 * i + 1.0) * coef * Math.PI / (2.0 * Size));

                coefficients[coef] *= Sqrt2DivSize;
                if (coef == 0)
                    coefficients[coef] *= Sqrt2;
            }

            return coefficients;
        }
    } 
}