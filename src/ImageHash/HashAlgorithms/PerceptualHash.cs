namespace CoenM.ImageHash.HashAlgorithms
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Numerics;
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
        private static readonly double _sqrt2DivSize = Math.Sqrt(2D / SIZE);
        private static readonly double _sqrt2 = 1 / Math.Sqrt(2);
        private static readonly List<Vector<double>>[] _dctCoeffsSimd = GenerateDctCoeffsSimd();

        /// <inheritdoc />
        public ulong Hash(Image<Rgba32> image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            var rows = new double[SIZE, SIZE];
            var sequence = new double[SIZE];
            var matrix = new double[SIZE, SIZE];

            image.Mutate(ctx => ctx
                                .Resize(SIZE, SIZE)
                                .Grayscale(GrayscaleMode.Bt601)
                                .AutoOrient());

            // Calculate the DCT for each row.
            for (var y = 0; y < SIZE; y++)
            {
                for (var x = 0; x < SIZE; x++)
                {
                    sequence[x] = image[x, y].R;
                }

                Dct1D_SIMD(sequence, rows, y);
            }

            // Calculate the DCT for each column.
            for (var x = 0; x < 8; x++)
            {
                for (var y = 0; y < SIZE; y++)
                {
                    sequence[y] = rows[y, x];
                }

                Dct1D_SIMD(sequence, matrix, x, limit: 8);
            }

            // Only use the top 8x8 values.
            var top8X8 = new double[SIZE];
            for (var y = 0; y < 8; y++)
            {
                for (var x = 0; x < 8; x++)
                {
                    top8X8[(y * 8) + x] = matrix[y, x];
                }
            }

            // Get Median.
            var median = CalculateMedian64Values(top8X8);

            // Calculate hash.
            var mask = 1UL << (SIZE - 1);
            var hash = 0UL;

            for (var i = 0; i < SIZE; i++)
            {
                if (top8X8[i] > median)
                {
                    hash |= mask;
                }

                mask >>= 1;
            }

            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double CalculateMedian64Values(IReadOnlyCollection<double> values)
        {
            Debug.Assert(values.Count == 64, "This DCT method works with 64 doubles.");
            return values.OrderBy(value => value).Skip(31).Take(2).Average();
        }

        private static double[,] GenerateDctCoeffs()
        {
            double[,] c = new double[SIZE, SIZE];
            for (var coef = 0; coef < SIZE; coef++)
            {
                for (var i = 0; i < SIZE; i++)
                {
                    c[i, coef] = Math.Cos(((2.0 * i) + 1.0) * coef * Math.PI / (2.0 * SIZE));
                }
            }

            return c;
        }

        private static List<Vector<double>>[] GenerateDctCoeffsSimd()
        {
            List<Vector<double>>[] results = new List<Vector<double>>[SIZE];
            for (var coef = 0; coef < SIZE; coef++)
            {
                var singleResultRaw = new double[SIZE];
                for (var i = 0; i < SIZE; i++)
                {
                    singleResultRaw[i] = Math.Cos(((2.0 * i) + 1.0) * coef * Math.PI / (2.0 * SIZE));
                }

                var singleResultList = new List<Vector<double>>();
                var stride = Vector<double>.Count;
                Debug.Assert(SIZE % stride == 0, "Size must be a multiple of SIMD stride");
                for (int i = 0; i < SIZE; i += stride)
                {
                    var v = new Vector<double>(singleResultRaw, i);
                    singleResultList.Add(v);
                }

                results[coef] = singleResultList;
            }

            return results;
        }

        /// <summary>
        /// One dimensional Discrete Cosine Transformation.
        /// </summary>
        /// <param name="valuesRaw">Should be an array of doubles of length 64.</param>
        /// <param name="coefficients">Coefficients.</param>
        /// <param name="ci">Coefficients index.</param>
        /// <param name="limit">Limit.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Dct1D_SIMD(double[] valuesRaw, double[,] coefficients, int ci, int limit = SIZE)
        {
            Debug.Assert(valuesRaw.Length == 64, "This DCT method works with 64 doubles.");

            var valuesList = new List<Vector<double>>();
            var stride = Vector<double>.Count;
            for (int i = 0; i < valuesRaw.Length; i += stride)
            {
                valuesList.Add(new Vector<double>(valuesRaw, i));
            }

            for (var coef = 0; coef < limit; coef++)
            {
                for (int i = 0; i < valuesList.Count; i++)
                {
                    coefficients[ci, coef] += Vector.Dot(valuesList[i], _dctCoeffsSimd[coef][i]);
                }

                coefficients[ci, coef] *= _sqrt2DivSize;
                if (coef == 0)
                {
                    coefficients[ci, coef] *= _sqrt2;
                }
            }
        }
    }
}
