using System;
using System.Collections.Generic;
using CoenM.ImageSharp.HashAlgorithms;
using Xunit;
using CoenM.ImageSharp.ImageHash.Test.Internal;

namespace CoenM.ImageSharp.ImageHash.Test.Algorithms
{
    public class PerceptualHashTest
    {
        private readonly PerceptualHash _sut;

        private readonly Dictionary<string, ulong> _expectedHashes = new Dictionary<string, ulong>
        {
            { "Alyson_Hannigan_500x500_0.jpg", 8393735134441359855},
            { "Alyson_Hannigan_500x500_1.jpg", 8393735134709533167},
            { "Alyson_Hannigan_200x200_0.jpg", 8393735134441359855},
            { "Alyson_Hannigan_4x4_0.jpg", 10632026805419366799},
            { "github_1.jpg", 02519932711947937405},
            { "github_2.jpg", 10591411288096084733}
        };

        public PerceptualHashTest()
        {
            _sut = new PerceptualHash();    
        }
        

        [Theory]
        [InlineData("Alyson_Hannigan_500x500_0.jpg", 8393735134441359855)]
        [InlineData("Alyson_Hannigan_500x500_1.jpg", 8393735134709533167)]
        [InlineData("Alyson_Hannigan_200x200_0.jpg", 8393735134441359855)]
        [InlineData("Alyson_Hannigan_4x4_0.jpg", 10632026805419366799)]
        [InlineData("github_1.jpg", 02519932711947937405)]
        [InlineData("github_2.jpg", 10591411288096084733)]
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