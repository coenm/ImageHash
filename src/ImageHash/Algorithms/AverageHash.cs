using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace CoenM.ImageSharp.ImageHash.Algorithms
{
    /// <summary>
    /// Contains a variety of methods useful in generating image hashes for image comparison
    /// and recognition.
    /// 
    /// Credit for the AverageHash implementation to David Oftedal of the University of Oslo.
    /// </summary>
    public class AverageHash : IImageHash
    {
        private const int Width = 8;
        private const int Height = 8;
        private const int NrPixels = Width * Height;

        /// <summary>
        /// Computes the average hash of an image according to the algorithm given by Dr. Neal Krawetz
        /// on his blog: http://www.hackerfactor.com/blog/index.php?/archives/432-Looks-Like-It.html.
        /// </summary>
        /// <param name="stream">The image to hash.</param>
        /// <returns>The hash of the image.</returns>
        public ulong Hash(Stream stream)
        {
            using (var img = Image.Load<Rgba32>(stream))
            {
                img.Mutate(ctx => ctx.Resize(Width, Height).Grayscale(GrayscaleMode.Bt601));

                var grayscale = new byte[NrPixels];
                uint averageValue = 0;

                var rawBytes = img.SavePixelData();
                for (var i = 0; i < NrPixels; i++)
                {
                    // Because image is of type Image<Rgba32> we know 4 bytes describes one pixel
                    // Also, it is already grayscaled, so R=G=B. Therefore, we can take one of these
                    // values. We take the R, (in the first of each 4 bytes).
                    var redIndex = i * 4;
                    grayscale[i] = rawBytes[redIndex];
                    averageValue += rawBytes[redIndex];
                }

                averageValue /= NrPixels;

                // Compute the hash: each bit is a pixel
                // 1 = higher than average, 0 = lower than average
                ulong hash = 0;
                for (var i = 0; i < NrPixels; i++)
                {
                    if (grayscale[i] >= averageValue)
                        hash |= 1UL << (NrPixels - 1 - i);
                }

                return hash;
            }
        }
    }
}