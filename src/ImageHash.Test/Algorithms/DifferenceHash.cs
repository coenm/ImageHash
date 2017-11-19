using System;
using System.Collections.Generic;
using CoenM.ImageSharp.HashAlgorithms;
using Xunit;
using CoenM.ImageSharp.ImageHash.Test.Internal;

namespace CoenM.ImageSharp.ImageHash.Test.Algorithms
{
    public class DifferenceHashTest
    {
        private readonly DifferenceHash _sut;

        private readonly Dictionary<string, ulong> _expectedHashes = new Dictionary<string, ulong>
        {
            { "Alyson_Hannigan_500x500_0.jpg", 5344123681044769265},
            { "Alyson_Hannigan_500x500_1.jpg", 5344123681044769265},
            { "Alyson_Hannigan_200x200_0.jpg", 5344123681044769265},
            { "Alyson_Hannigan_4x4_0.jpg", 14339478909768759544},
            { "github_1.jpg", 8102028990804191308},
            { "github_2.jpg", 3472328590912938060}
        };

        public DifferenceHashTest()
        {
            _sut = new DifferenceHash();    
        }
        

        [Theory]
        [InlineData("Alyson_Hannigan_500x500_0.jpg", 5344123681044769265)]
        [InlineData("Alyson_Hannigan_500x500_1.jpg", 5344123681044769265)]
        [InlineData("Alyson_Hannigan_200x200_0.jpg", 5344123681044769265)]
        [InlineData("Alyson_Hannigan_4x4_0.jpg", 14339478909768759544)]
        [InlineData("github_1.jpg", 8102028990804191308)]
        [InlineData("github_2.jpg", 3472328590912938060)]
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
            const string filename = "Not_an_image.txt";

            // act
            // assert
            using (var stream = TestHelper.OpenStream(filename))
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
            Assert.Equal(100, result);
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
            Assert.Equal(89.0625, result);
        }
    }
}