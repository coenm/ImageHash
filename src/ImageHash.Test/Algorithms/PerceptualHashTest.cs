namespace CoenM.ImageSharp.ImageHash.Test.Algorithms
{
    using System;
    using System.Collections.Generic;

    using CoenM.ImageSharp.HashAlgorithms;
    using CoenM.ImageSharp.ImageHash.Test.Internal;

    using Xunit;

    public class PerceptualHashTest
    {
        private readonly PerceptualHash _sut;

        private readonly Dictionary<string, ulong> _expectedHashes = new Dictionary<string, ulong>
                                                                         {
                                                                             { "Alyson_Hannigan_500x500_0.jpg", 17839858461443178030 },
                                                                             { "Alyson_Hannigan_500x500_1.jpg", 17839823311430827566 },
                                                                             { "Alyson_Hannigan_200x200_0.jpg", 17839858461443178030 },
                                                                             { "Alyson_Hannigan_4x4_0.jpg", 17409736169497899465 },
                                                                             { "github_1.jpg", 13719320793338945348 },
                                                                             { "github_2.jpg", 13783795072850083657 }
                                                                         };

        public PerceptualHashTest()
        {
            _sut = new PerceptualHash();
        }

        [Theory]
        [InlineData("Alyson_Hannigan_500x500_0.jpg", 17839858461443178030)]
        [InlineData("Alyson_Hannigan_500x500_1.jpg", 17839823311430827566)]
        [InlineData("Alyson_Hannigan_200x200_0.jpg", 17839858461443178030)]
        [InlineData("Alyson_Hannigan_4x4_0.jpg", 17409736169497899465)]
        [InlineData("github_1.jpg", 13719320793338945348)]
        [InlineData("github_2.jpg", 13783795072850083657)]
        public void HashImagesTest(string filename, ulong expectedHash)
        {
            // arrange
            ulong result;

            // act
            using (var stream = TestHelper.OpenStream(filename))
                result = _sut.Hash(stream);

            // assert
            Assert.Equal(expectedHash, result);
        }

        [Fact]
        public void NotAnImageShouldThrowExceptionTest()
        {
            // arrange
            const string FILENAME = "Not_an_image.txt";

            // act
            // assert
            using (var stream = TestHelper.OpenStream(FILENAME))
                Assert.Throws<NotSupportedException>(() => _sut.Hash(stream));
        }

        [Fact]
        public void ImageWithFilterShouldHaveAlmostOrExactly100Similarity1Test()
        {
            // arrange
            var hash1 = _expectedHashes["Alyson_Hannigan_500x500_0.jpg"];
            var hash2 = _expectedHashes["Alyson_Hannigan_500x500_1.jpg"];

            // act
            var result = CompareHash.Similarity(hash1, hash2);

            // assert
            Assert.Equal(96.875, result);
        }

        [Fact]
        public void ResizedImageShouldHaveAlmostOrExactly100Similarity2Test()
        {
            // arrange
            var hash1 = _expectedHashes["Alyson_Hannigan_500x500_0.jpg"];
            var hash2 = _expectedHashes["Alyson_Hannigan_200x200_0.jpg"];

            // act
            var result = CompareHash.Similarity(hash1, hash2);

            // assert
            Assert.Equal(100, result);
        }

        [Fact]
        public void ComparingExtreamlySmallImageShouldDecreaseSimilarityTest()
        {
            // arrange
            var hash1 = _expectedHashes["Alyson_Hannigan_4x4_0.jpg"];
            var hash2 = _expectedHashes["Alyson_Hannigan_500x500_0.jpg"];

            // act
            var result = CompareHash.Similarity(hash1, hash2);

            // assert
            Assert.Equal(59.375, result);
        }

        [Fact]
        public void TwoDifferentImagesOfGithubArePrettySimilarTests()
        {
            // arrange
            var hash1 = _expectedHashes["github_1.jpg"];
            var hash2 = _expectedHashes["github_2.jpg"];

            // act
            var result = CompareHash.Similarity(hash1, hash2);

            // assert
            Assert.Equal(71.875, result);
        }
    }
}