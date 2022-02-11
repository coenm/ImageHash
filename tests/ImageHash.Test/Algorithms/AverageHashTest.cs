namespace CoenM.ImageHash.Test.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Threading.Tasks;
    using CoenM.ImageHash.HashAlgorithms;
    using CoenM.ImageHash.Test.Data;
    using EasyTestFile;
    using EasyTestFileXunit;
    using FluentAssertions;
    using Xunit;

    [UsesEasyTestFile]
    public class AverageHashTest
    {
        private readonly AverageHash _sut;

        private readonly Dictionary<string, ulong> _expectedHashes = new Dictionary<string, ulong>
        {
            { "Alyson_Hannigan_500x500_0.jpg", 16701559372701825768 },
            { "Alyson_Hannigan_500x500_1.jpg", 16701559372735380200 },
            { "Alyson_Hannigan_200x200_0.jpg", 16701559372701825768 },
            { "Alyson_Hannigan_4x4_0.jpg", 14395694381845246192 },
            { "github_1.jpg", 15835643108028573695 },
            { "github_2.jpg", 15835645411202688999 },
        };

        public AverageHashTest()
        {
            _sut = new AverageHash();
        }

        [Fact]
        public void NullArgumentShouldThrowArgumentNullExceptionTest()
        {
            // arrange

            // act
            Action act = () => _sut.Hash(null);

            // assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData("Alyson_Hannigan_500x500_0.jpg", 16701559372735380200)]
        [InlineData("Alyson_Hannigan_500x500_1.jpg", 16701559372735380200)]
        [InlineData("Alyson_Hannigan_200x200_0.jpg", 16701559372735380200)]
        [InlineData("Alyson_Hannigan_4x4_0.jpg", 14395694381845246192)]
        [InlineData("github_1.jpg", 15835643108028573695)]
        [InlineData("github_2.jpg", 15835645411202688999)]
        public async Task HashImagesTest3(string filename, ulong expectedHash)
        {
            // arrange
            ulong result;

            // act
            using (Stream stream = await TestData.GetByName(filename).AsStream())
            {
                result = _sut.Hash(stream);
            }

            // assert
            result.Should().Be(expectedHash);
        }

        [Fact]
        [SuppressMessage("ReSharper", "AccessToDisposedClosure", Justification = "Manually reviewed")]
        public async Task NotAnImageShouldThrowExceptionTest()
        {
            // arrange
            // act
            using Stream stream = await TestData.NotAnImage.AsStream();
            Action act = () => _sut.Hash(stream);

            // assert
            act.Should().Throw<SixLabors.ImageSharp.UnknownImageFormatException>();
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
            result.Should().Be(98.4375);
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
            result.Should().Be(100);
        }

        [Fact]
        public void ComparingExtremelySmallImageShouldDecreaseSimilarityTest()
        {
            // arrange
            var hash1 = _expectedHashes["Alyson_Hannigan_4x4_0.jpg"];
            var hash2 = _expectedHashes["Alyson_Hannigan_500x500_0.jpg"];

            // act
            var result = CompareHash.Similarity(hash1, hash2);

            // assert
            result.Should().Be(82.8125);
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
            result.Should().Be(89.0625);
        }
    }
}
