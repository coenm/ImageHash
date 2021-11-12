namespace CoenM.ImageHash.Test
{
    using System.IO;
    using CoenM.ImageHash.HashAlgorithms;

    // These examples are used to generate snippets for markdown documentation.
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1123:Do not place regions within elements", Justification = "Source is used for generating markdown docs.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1515:Single-line comment should be preceded by blank line", Justification = "Source is used for generating markdown docs.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1512:Single-line comments should not be followed by blank line", Justification = "Source is used for generating markdown docs.")]
    public class Examples
    {
        public void CalculateImageHashDemo()
        {
            #region CalculateImageHash

            var hashAlgorithm = new AverageHash();
            // or one of the other available algorithms:
            // var hashAlgorithm = new DifferenceHash();
            // var hashAlgorithm = new PerceptualHash();

            string filename = "your filename";
            using var stream = File.OpenRead(filename);

            ulong imageHash = hashAlgorithm.Hash(stream);

            #endregion
        }

        public void CalculateSimilarityDemo()
        {
            var hashAlgorithm = new AverageHash();
            var imageStream1 = new MemoryStream();
            var imageStream2 = new MemoryStream();

            #region CalculateSimilarity

            // calculate the two image hashes
            ulong hash1 = hashAlgorithm.Hash(imageStream1);
            ulong hash2 = hashAlgorithm.Hash(imageStream2);

            double percentageImageSimilarity = CompareHash.Similarity(hash1, hash2);

            #endregion
        }
    }
}
